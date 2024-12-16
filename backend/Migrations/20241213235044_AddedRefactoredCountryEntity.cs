using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddedRefactoredCountryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommonName",
                schema: "Reference",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "FlagUrl",
                schema: "Reference",
                table: "Country");

            migrationBuilder.RenameColumn(
                name: "OfficialName",
                schema: "Reference",
                table: "Country",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "Reference",
                table: "Country",
                newName: "OfficialName");

            migrationBuilder.AddColumn<string>(
                name: "CommonName",
                schema: "Reference",
                table: "Country",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FlagUrl",
                schema: "Reference",
                table: "Country",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
