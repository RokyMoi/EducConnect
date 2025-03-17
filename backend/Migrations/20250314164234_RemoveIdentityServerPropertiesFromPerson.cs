using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdentityServerPropertiesFromPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PersonSalt_PersonId",
                schema: "Person",
                table: "PersonSalt");

            migrationBuilder.DropColumn(
                name: "Hash",
                schema: "Person",
                table: "PersonPassword");

            migrationBuilder.DropColumn(
                name: "Salt",
                schema: "Person",
                table: "PersonPassword");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Person",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                schema: "Person",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "Person",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "Person",
                table: "Person");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                schema: "Person",
                table: "PersonPassword",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSalt_PersonId",
                schema: "Person",
                table: "PersonSalt",
                column: "PersonId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PersonSalt_PersonId",
                schema: "Person",
                table: "PersonSalt");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                schema: "Person",
                table: "PersonPassword");

            migrationBuilder.AddColumn<byte[]>(
                name: "Hash",
                schema: "Person",
                table: "PersonPassword",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                schema: "Person",
                table: "PersonPassword",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Person",
                table: "Person",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                schema: "Person",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "Person",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "Person",
                table: "Person",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonSalt_PersonId",
                schema: "Person",
                table: "PersonSalt",
                column: "PersonId");
        }
    }
}
