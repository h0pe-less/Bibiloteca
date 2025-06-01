using localLib.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace localLib.Data
{
    public class BibliotecaContext : IdentityDbContext<IdentityUser>
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
            : base(options)
        {
        }

        public DbSet<Categorie> Categorii { get; set; }
        public DbSet<Editura> Edituri { get; set; }
        public DbSet<ZonaColectie> ZoneColectie { get; set; }
        public DbSet<Limba> Limbi { get; set; }
        public DbSet<Tara> Tari { get; set; }
        public DbSet<Autor> Autori { get; set; }
        public DbSet<Carte> Carti { get; set; }
        public DbSet<CarteAutor> CartiAutori { get; set; }
        public DbSet<CarteCategorie> CartiCategorii { get; set; }
        public DbSet<Imprumut> Imprumuturi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<CarteAutor>()
                .HasKey(ca => new { ca.CarteId, ca.AutorId });

            modelBuilder.Entity<CarteAutor>()
                .HasOne(ca => ca.Carte)
                .WithMany(c => c.Autori)
                .HasForeignKey(ca => ca.CarteId);

            modelBuilder.Entity<CarteAutor>()
                .HasOne(ca => ca.Autor)
                .WithMany(a => a.CartiAutor)
                .HasForeignKey(ca => ca.AutorId);

            modelBuilder.Entity<Imprumut>()
                .HasOne(i => i.Carte)
                .WithMany()
                .HasForeignKey(i => i.CarteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CarteCategorie>()
                .HasKey(cc => new { cc.CarteId, cc.CategorieId });

            modelBuilder.Entity<CarteCategorie>()
                .HasOne(cc => cc.Carte)
                .WithMany(c => c.CarteCategorii)
                .HasForeignKey(cc => cc.CarteId);

            modelBuilder.Entity<CarteCategorie>()
                .HasOne(cc => cc.Categorie)
                .WithMany(c => c.CartiCategorii)
                .HasForeignKey(cc => cc.CategorieId);

            modelBuilder.Entity<Carte>()
                .Property(c => c.Pret)
                .HasColumnType("decimal(18,2)");
        }
    }
}