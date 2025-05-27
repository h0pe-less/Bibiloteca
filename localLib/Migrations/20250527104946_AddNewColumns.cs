using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace localLib.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ilustratii",
                table: "Carti",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Paginatie",
                table: "Carti",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ilustratii",
                table: "Carti");

            migrationBuilder.DropColumn(
                name: "Paginatie",
                table: "Carti");
        }
    }
}
