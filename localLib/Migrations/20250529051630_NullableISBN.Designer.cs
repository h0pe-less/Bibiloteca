﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using localLib.Data;

#nullable disable

namespace localLib.Migrations
{
    [DbContext(typeof(BibliotecaContext))]
    [Migration("20250529051630_NullableISBN")]
    partial class NullableISBN
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("localLib.Models.Autor", b =>
                {
                    b.Property<long>("AutorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("AutorId"));

                    b.Property<string>("Biografie")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataNasterii")
                        .HasColumnType("datetime2");

                    b.Property<string>("NumePrenume")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AutorId");

                    b.ToTable("Autori");
                });

            modelBuilder.Entity("localLib.Models.Carte", b =>
                {
                    b.Property<long>("CarteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CarteId"));

                    b.Property<string>("Bibliografie")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CopertaURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cota")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataPublicarii")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descriere")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Editie")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("EdituraId")
                        .HasColumnType("bigint");

                    b.Property<string>("ISBN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ISSN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ilustratii")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("LimbaId")
                        .HasColumnType("bigint");

                    b.Property<string>("LoculPublicarii")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MentiuniResponsabilitate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NrPagini")
                        .HasColumnType("int");

                    b.Property<long>("NumarInventar")
                        .HasColumnType("bigint");

                    b.Property<string>("Paginatie")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Pret")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("TaraId")
                        .HasColumnType("bigint");

                    b.Property<string>("Titlu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TitluInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ZonaColectieId")
                        .HasColumnType("bigint");

                    b.HasKey("CarteId");

                    b.HasIndex("EdituraId");

                    b.HasIndex("LimbaId");

                    b.HasIndex("TaraId");

                    b.HasIndex("ZonaColectieId");

                    b.ToTable("Carti");
                });

            modelBuilder.Entity("localLib.Models.CarteAutor", b =>
                {
                    b.Property<long>("CarteId")
                        .HasColumnType("bigint");

                    b.Property<long>("AutorId")
                        .HasColumnType("bigint");

                    b.HasKey("CarteId", "AutorId");

                    b.HasIndex("AutorId");

                    b.ToTable("CartiAutori");
                });

            modelBuilder.Entity("localLib.Models.CarteCategorie", b =>
                {
                    b.Property<long>("CarteId")
                        .HasColumnType("bigint");

                    b.Property<long>("CategorieId")
                        .HasColumnType("bigint");

                    b.HasKey("CarteId", "CategorieId");

                    b.HasIndex("CategorieId");

                    b.ToTable("CartiCategorii");
                });

            modelBuilder.Entity("localLib.Models.Categorie", b =>
                {
                    b.Property<long>("CategorieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CategorieId"));

                    b.Property<string>("Denumire")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descriere")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategorieId");

                    b.ToTable("Categorii");
                });

            modelBuilder.Entity("localLib.Models.Editura", b =>
                {
                    b.Property<long>("EdituraId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("EdituraId"));

                    b.Property<string>("Denumire")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EdituraId");

                    b.ToTable("Edituri");
                });

            modelBuilder.Entity("localLib.Models.Imprumut", b =>
                {
                    b.Property<long>("ImprumutId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ImprumutId"));

                    b.Property<long>("CarteId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DataImprumut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataReturnare")
                        .HasColumnType("datetime2");

                    b.Property<bool>("EsteReturnat")
                        .HasColumnType("bit");

                    b.Property<string>("Grupa")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Nume")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Prenume")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ImprumutId");

                    b.HasIndex("CarteId");

                    b.ToTable("Imprumuturi");
                });

            modelBuilder.Entity("localLib.Models.Limba", b =>
                {
                    b.Property<long>("LimbaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("LimbaId"));

                    b.Property<string>("DenumireLimba")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LimbaId");

                    b.ToTable("Limbi");
                });

            modelBuilder.Entity("localLib.Models.Tara", b =>
                {
                    b.Property<long>("TaraId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TaraId"));

                    b.Property<string>("DenumireTara")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TaraId");

                    b.ToTable("Tari");
                });

            modelBuilder.Entity("localLib.Models.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumePrenume")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeUtilizator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Parola")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("localLib.Models.ZonaColectie", b =>
                {
                    b.Property<long>("ZonaColectieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ZonaColectieId"));

                    b.Property<string>("DenumireZona")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ZonaColectieId");

                    b.ToTable("ZoneColectie");
                });

            modelBuilder.Entity("localLib.Models.Carte", b =>
                {
                    b.HasOne("localLib.Models.Editura", "Editura")
                        .WithMany("Carti")
                        .HasForeignKey("EdituraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localLib.Models.Limba", "Limba")
                        .WithMany("Carti")
                        .HasForeignKey("LimbaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localLib.Models.Tara", "Tara")
                        .WithMany("Carti")
                        .HasForeignKey("TaraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localLib.Models.ZonaColectie", "ZonaColectie")
                        .WithMany("Carti")
                        .HasForeignKey("ZonaColectieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Editura");

                    b.Navigation("Limba");

                    b.Navigation("Tara");

                    b.Navigation("ZonaColectie");
                });

            modelBuilder.Entity("localLib.Models.CarteAutor", b =>
                {
                    b.HasOne("localLib.Models.Autor", "Autor")
                        .WithMany("CartiAutor")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localLib.Models.Carte", "Carte")
                        .WithMany("Autori")
                        .HasForeignKey("CarteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");

                    b.Navigation("Carte");
                });

            modelBuilder.Entity("localLib.Models.CarteCategorie", b =>
                {
                    b.HasOne("localLib.Models.Carte", "Carte")
                        .WithMany("CarteCategorii")
                        .HasForeignKey("CarteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("localLib.Models.Categorie", "Categorie")
                        .WithMany("CartiCategorii")
                        .HasForeignKey("CategorieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Carte");

                    b.Navigation("Categorie");
                });

            modelBuilder.Entity("localLib.Models.Imprumut", b =>
                {
                    b.HasOne("localLib.Models.Carte", "Carte")
                        .WithMany()
                        .HasForeignKey("CarteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Carte");
                });

            modelBuilder.Entity("localLib.Models.Autor", b =>
                {
                    b.Navigation("CartiAutor");
                });

            modelBuilder.Entity("localLib.Models.Carte", b =>
                {
                    b.Navigation("Autori");

                    b.Navigation("CarteCategorii");
                });

            modelBuilder.Entity("localLib.Models.Categorie", b =>
                {
                    b.Navigation("CartiCategorii");
                });

            modelBuilder.Entity("localLib.Models.Editura", b =>
                {
                    b.Navigation("Carti");
                });

            modelBuilder.Entity("localLib.Models.Limba", b =>
                {
                    b.Navigation("Carti");
                });

            modelBuilder.Entity("localLib.Models.Tara", b =>
                {
                    b.Navigation("Carti");
                });

            modelBuilder.Entity("localLib.Models.ZonaColectie", b =>
                {
                    b.Navigation("Carti");
                });
#pragma warning restore 612, 618
        }
    }
}
