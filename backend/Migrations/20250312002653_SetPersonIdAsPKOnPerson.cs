using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class SetPersonIdAsPKOnPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonId1",
                schema: "Person",
                table: "Person",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Person",
                table: "Person",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Person",
                table: "Person");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Person",
                table: "Person",
                newName: "PersonId1");
        }
    }
}
