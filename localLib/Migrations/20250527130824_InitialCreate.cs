using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localLib.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autori",
                columns: table => new
                {
                    AutorId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumePrenume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biografie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNasterii = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autori", x => x.AutorId);
                });

            migrationBuilder.CreateTable(
                name: "Categorii",
                columns: table => new
                {
                    CategorieId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descriere = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorii", x => x.CategorieId);
                });

            migrationBuilder.CreateTable(
                name: "Edituri",
                columns: table => new
                {
                    EdituraId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edituri", x => x.EdituraId);
                });

            migrationBuilder.CreateTable(
                name: "Limbi",
                columns: table => new
                {
                    LimbaId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DenumireLimba = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Limbi", x => x.LimbaId);
                });

            migrationBuilder.CreateTable(
                name: "Tari",
                columns: table => new
                {
                    TaraId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DenumireTara = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tari", x => x.TaraId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeUtilizator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parola = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumePrenume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ZoneColectie",
                columns: table => new
                {
                    ZonaColectieId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DenumireZona = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneColectie", x => x.ZonaColectieId);
                });

            migrationBuilder.CreateTable(
                name: "Carti",
                columns: table => new
                {
                    CarteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISSN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cota = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titlu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitluInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MentiuniResponsabilitate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Editie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EdituraId = table.Column<long>(type: "bigint", nullable: false),
                    DataPublicarii = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoculPublicarii = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bibliografie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descriere = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NrPagini = table.Column<int>(type: "int", nullable: false),
                    Pret = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ZonaColectieId = table.Column<long>(type: "bigint", nullable: false),
                    LimbaId = table.Column<long>(type: "bigint", nullable: false),
                    TaraId = table.Column<long>(type: "bigint", nullable: false),
                    NumarInventar = table.Column<long>(type: "bigint", nullable: false),
                    Paginatie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ilustratii = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CopertaURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carti", x => x.CarteId);
                    table.ForeignKey(
                        name: "FK_Carti_Edituri_EdituraId",
                        column: x => x.EdituraId,
                        principalTable: "Edituri",
                        principalColumn: "EdituraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carti_Limbi_LimbaId",
                        column: x => x.LimbaId,
                        principalTable: "Limbi",
                        principalColumn: "LimbaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carti_Tari_TaraId",
                        column: x => x.TaraId,
                        principalTable: "Tari",
                        principalColumn: "TaraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carti_ZoneColectie_ZonaColectieId",
                        column: x => x.ZonaColectieId,
                        principalTable: "ZoneColectie",
                        principalColumn: "ZonaColectieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartiAutori",
                columns: table => new
                {
                    CarteId = table.Column<long>(type: "bigint", nullable: false),
                    AutorId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartiAutori", x => new { x.CarteId, x.AutorId });
                    table.ForeignKey(
                        name: "FK_CartiAutori_Autori_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autori",
                        principalColumn: "AutorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartiAutori_Carti_CarteId",
                        column: x => x.CarteId,
                        principalTable: "Carti",
                        principalColumn: "CarteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartiCategorii",
                columns: table => new
                {
                    CarteId = table.Column<long>(type: "bigint", nullable: false),
                    CategorieId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartiCategorii", x => new { x.CarteId, x.CategorieId });
                    table.ForeignKey(
                        name: "FK_CartiCategorii_Carti_CarteId",
                        column: x => x.CarteId,
                        principalTable: "Carti",
                        principalColumn: "CarteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartiCategorii_Categorii_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "Categorii",
                        principalColumn: "CategorieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carti_EdituraId",
                table: "Carti",
                column: "EdituraId");

            migrationBuilder.CreateIndex(
                name: "IX_Carti_LimbaId",
                table: "Carti",
                column: "LimbaId");

            migrationBuilder.CreateIndex(
                name: "IX_Carti_TaraId",
                table: "Carti",
                column: "TaraId");

            migrationBuilder.CreateIndex(
                name: "IX_Carti_ZonaColectieId",
                table: "Carti",
                column: "ZonaColectieId");

            migrationBuilder.CreateIndex(
                name: "IX_CartiAutori_AutorId",
                table: "CartiAutori",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_CartiCategorii_CategorieId",
                table: "CartiCategorii",
                column: "CategorieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartiAutori");

            migrationBuilder.DropTable(
                name: "CartiCategorii");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Autori");

            migrationBuilder.DropTable(
                name: "Carti");

            migrationBuilder.DropTable(
                name: "Categorii");

            migrationBuilder.DropTable(
                name: "Edituri");

            migrationBuilder.DropTable(
                name: "Limbi");

            migrationBuilder.DropTable(
                name: "Tari");

            migrationBuilder.DropTable(
                name: "ZoneColectie");
        }
    }
}
