using FluentValidation;
using FluentValidation.AspNetCore;
using localLib.Data;
using localLib.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using localLib.Services;

namespace localLib
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMvc().AddRazorRuntimeCompilation();

            builder.Services.AddControllersWithViews();

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<CarteValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ImprumutValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<AutorValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ZonaColectieValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<EdituraValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CategorieValidator>();

            builder.Services.AddDbContext<BibliotecaContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("BibliotecaConnection")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<BibliotecaContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "MultiScheme";
                options.DefaultChallengeScheme = "MultiScheme";
                options.DefaultScheme = "MultiScheme";
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.Response.Redirect("/Account/Login");
                        context.HandleResponse();
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.Redirect("/Account/AccessDenied");
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("jwt_token"))
                        {
                            context.Token = context.Request.Cookies["jwt_token"];
                        }
                        return Task.CompletedTask;
                    }
                };
            })
            .AddPolicyScheme("MultiScheme", "Multi-Auth", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    if (context.Request.Cookies.ContainsKey("jwt_token"))
                    {
                        return JwtBearerDefaults.AuthenticationScheme;
                    }
                    
                    return IdentityConstants.ApplicationScheme;
                };
            });

            builder.Services.AddScoped<JwtService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    await DbInitializer.InitializeAsync(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            app.Run();
        }
    }
}