using localLib.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace localLib.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<BibliotecaContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var adminUser = await userManager.FindByEmailAsync("admin@locallib.com");
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = "admin@locallib.com",
                    Email = "admin@locallib.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            Initialize(context);
        }

        public static void Initialize(BibliotecaContext context)
        {
            context.Database.EnsureCreated();

            if (context.Carti.Any())
            {
                return;
            }

            var tari = new Tara[]
            {
                new Tara{DenumireTara="România"},
                new Tara{DenumireTara="Republica Moldova"},
                new Tara{DenumireTara="Franța"}
            };
            foreach (Tara t in tari)
            {
                context.Tari.Add(t);
            }
            context.SaveChanges();

            var limbi = new Limba[]
            {
                new Limba{DenumireLimba="Română"},
                new Limba{DenumireLimba="Engleză"},
                new Limba{DenumireLimba="Franceză"}
            };
            foreach (Limba l in limbi)
            {
                context.Limbi.Add(l);
            }
            context.SaveChanges();

            var edituri = new Editura[]
            {
                new Editura{Denumire="Humanitas"},
                new Editura{Denumire="Polirom"},
                new Editura{Denumire="Litera"}
            };
            foreach (Editura e in edituri)
            {
                context.Edituri.Add(e);
            }
            context.SaveChanges();

            var zone = new ZonaColectie[]
            {
                new ZonaColectie{DenumireZona="Literatură română"},
                new ZonaColectie{DenumireZona="Literatură universală"},
                new ZonaColectie{DenumireZona="Știință"}
            };
            foreach (ZonaColectie z in zone)
            {
                context.ZoneColectie.Add(z);
            }
            context.SaveChanges();

            var categorii = new Categorie[]
            {
                new Categorie{Denumire="Roman", Descriere="Opere literare în proză"},
                new Categorie{Denumire="Poezie", Descriere="Opere literare în versuri"},
                new Categorie{Denumire="Eseu", Descriere="Scrieri de reflecție"}
            };
            foreach (Categorie c in categorii)
            {
                context.Categorii.Add(c);
            }
            context.SaveChanges();

            var autori = new Autor[]
            {
                new Autor{
                    NumePrenume="Mircea Eliade",
                    Biografie="Scriitor, istoric al religiilor și filozof român",
                    DataNasterii=new DateTime(1907, 3, 9)
                },
                new Autor{
                    NumePrenume="Mihai Eminescu",
                    Biografie="Poețul național al României",
                    DataNasterii=new DateTime(1850, 1, 15)
                },
                new Autor{
                    NumePrenume="Mircea Cărtărescu",
                    Biografie="Scriitor contemporan român",
                    DataNasterii=new DateTime(1956, 6, 1)
                }
            };
            foreach (Autor a in autori)
            {
                context.Autori.Add(a);
            }
            context.SaveChanges();

            var carti = new Carte[]
            {
                new Carte{
                    Titlu="Maitreyi",
                    ISBN="978-973-50-1234-5",
                    Cota="821.135.1",
                    EdituraId=edituri[0].EdituraId,
                    DataPublicarii=new DateTime(1933, 1, 1),
                    LoculPublicarii="București",
                    NrPagini=256,
                    Pret=29.99m,
                    ZonaColectieId=zone[0].ZonaColectieId,
                    LimbaId=limbi[0].LimbaId,
                    TaraId=tari[0].TaraId,
                    NumarInventar=1001,
                    Descriere="Roman autobiografic de Mircea Eliade",
                    CopertaURL="https://example.com/maitreyi.jpg",
                    Editie = "Editia 1",
                    Paginatie = "1-256",
                    Ilustratii = "Nu",
                    TitluInfo = "Orbitor - Trilogia",
                    MentiuniResponsabilitate = "Această carte conține poeziile celebre ale lui Mihai Eminescu, inclusiv 'Luceafărul' și 'Scrisoarea III'.",
                    ISSN = "1",
                    Bibliografie = "https://ro.wikipedia.org/wiki/Mihai_Eminescu"
                },
                new Carte{
                    Titlu="Poezii",
                    ISBN="978-973-50-1234-0",
                    Cota="123123.23",
                    EdituraId=edituri[1].EdituraId,
                    DataPublicarii=new DateTime(1884, 1, 1),
                    LoculPublicarii="București",
                    NrPagini=320,
                    Pret=49.99m,
                    ZonaColectieId=zone[0].ZonaColectieId,
                    LimbaId=limbi[0].LimbaId,
                    TaraId=tari[0].TaraId,
                    NumarInventar=1002,
                    Descriere="Opera poetică a lui Mihai Eminescu",
                    CopertaURL="https://example.com/eminescu.jpg",
                    Editie = "Editia 1",
                    Paginatie = "1-320",
                    Ilustratii = "Nu",
                    TitluInfo = "Orbitor - Trilogia",
                    MentiuniResponsabilitate = "Această carte conține poeziile celebre ale lui Mihai Eminescu, inclusiv 'Luceafărul' și 'Scrisoarea III'.",
                    ISSN = "3",
                    Bibliografie = "https://ro.wikipedia.org/wiki/Mihai_Eminescu"
                },
                new Carte{
                    Titlu="Orbitor",
                    ISBN="978-973-50-4567-0",
                    Cota="123123.23",
                    EdituraId=edituri[2].EdituraId,
                    DataPublicarii=new DateTime(1996, 1, 1),
                    LoculPublicarii="București",
                    NrPagini=512,
                    Pret=59.99m,
                    ZonaColectieId=zone[0].ZonaColectieId,
                    LimbaId=limbi[0].LimbaId,
                    TaraId=tari[0].TaraId,
                    NumarInventar=1003,
                    Descriere="Trilogia fantastică a lui Mircea Cărtărescu",
                    CopertaURL="https://example.com/orbitor.jpg",
                    Editie = "Editia 1",
                    Paginatie = "1-512",
                    Ilustratii = "Da",
                    TitluInfo = "Orbitor - Trilogia",
                    MentiuniResponsabilitate = "Această carte conține poeziile celebre ale lui Mihai Eminescu, inclusiv 'Luceafărul' și 'Scrisoarea III'.",
                    ISSN = "2",
                    Bibliografie = "https://ro.wikipedia.org/wiki/Mihai_Eminescu"
                }
            };
            foreach (Carte c in carti)
            {
                context.Carti.Add(c);
            }
            context.SaveChanges();

            var carteAutori = new CarteAutor[]
            {
                new CarteAutor{CarteId=carti[0].CarteId, AutorId=autori[0].AutorId},
                new CarteAutor{CarteId=carti[1].CarteId, AutorId=autori[1].AutorId},
                new CarteAutor{CarteId=carti[2].CarteId, AutorId=autori[2].AutorId}
            };
            foreach (CarteAutor ca in carteAutori)
            {
                context.CartiAutori.Add(ca);
            }
            context.SaveChanges();

            var carteCategorii = new CarteCategorie[]
            {
                new CarteCategorie{CarteId=carti[0].CarteId, CategorieId=categorii[0].CategorieId},
                new CarteCategorie{CarteId=carti[1].CarteId, CategorieId=categorii[1].CategorieId},
                new CarteCategorie{CarteId=carti[2].CarteId, CategorieId=categorii[0].CategorieId}
            };
            foreach (CarteCategorie cc in carteCategorii)
            {
                context.CartiCategorii.Add(cc);
            }
            
            context.SaveChanges();
        }
    }
}
