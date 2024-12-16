using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToPersonPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PersonPhoneNumber_PersonId",
                schema: "Person",
                table: "PersonPhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PersonPhoneNumber_PersonId",
                schema: "Person",
                table: "PersonPhoneNumber",
                column: "PersonId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PersonPhoneNumber_PersonId",
                schema: "Person",
                table: "PersonPhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PersonPhoneNumber_PersonId",
                schema: "Person",
                table: "PersonPhoneNumber",
                column: "PersonId");
        }
    }
}
