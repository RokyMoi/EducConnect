using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class SwitchPrimaryKeyForTutor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tutor",
                schema: "Tutor",
                table: "Tutor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tutor",
                schema: "Tutor",
                table: "Tutor",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tutor_PersonId",
                schema: "Tutor",
                table: "Tutor",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tutor",
                schema: "Tutor",
                table: "Tutor");

            migrationBuilder.DropIndex(
                name: "IX_Tutor_PersonId",
                schema: "Tutor",
                table: "Tutor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tutor",
                schema: "Tutor",
                table: "Tutor",
                column: "PersonId");
        }
    }
}
