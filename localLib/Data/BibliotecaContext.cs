using localLib.Models;
using Microsoft.EntityFrameworkCore;

namespace localLib.Data
{
    public class BibliotecaContext : DbContext
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
        public DbSet<User> Users { get; set; }
        public DbSet<Carte> Carti { get; set; }
        public DbSet<CarteAutor> CartiAutori { get; set; }
        public DbSet<CarteCategorie> CartiCategorii { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<User>()
                .Property(u => u.Rol)
                .HasConversion<string>();
        }
    }
}
