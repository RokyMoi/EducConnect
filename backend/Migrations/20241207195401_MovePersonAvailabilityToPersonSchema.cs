using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class MovePersonAvailabilityToPersonSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonAvailibility_Person_PersonId",
                table: "PersonAvailibility");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonAvailibility",
                table: "PersonAvailibility");

            migrationBuilder.RenameTable(
                name: "PersonAvailibility",
                newName: "PersonAvailability",
                newSchema: "Person");

            migrationBuilder.RenameIndex(
                name: "IX_PersonAvailibility_PersonId",
                schema: "Person",
                table: "PersonAvailability",
                newName: "IX_PersonAvailability_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonAvailability",
                schema: "Person",
                table: "PersonAvailability",
                column: "PersonAvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonAvailability_Person_PersonId",
                schema: "Person",
                table: "PersonAvailability",
                column: "PersonId",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonAvailability_Person_PersonId",
                schema: "Person",
                table: "PersonAvailability");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonAvailability",
                schema: "Person",
                table: "PersonAvailability");

            migrationBuilder.RenameTable(
                name: "PersonAvailability",
                schema: "Person",
                newName: "PersonAvailibility");

            migrationBuilder.RenameIndex(
                name: "IX_PersonAvailability_PersonId",
                table: "PersonAvailibility",
                newName: "IX_PersonAvailibility_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonAvailibility",
                table: "PersonAvailibility",
                column: "PersonAvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonAvailibility_Person_PersonId",
                table: "PersonAvailibility",
                column: "PersonId",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
