using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class MoveTutorRegistrationStatusToReferenceSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TutorRegistrationStatus",
                schema: "Tutor",
                newName: "TutorRegistrationStatus",
                newSchema: "Reference");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TutorRegistrationStatus",
                schema: "Reference",
                newName: "TutorRegistrationStatus",
                newSchema: "Tutor");
        }
    }
}
