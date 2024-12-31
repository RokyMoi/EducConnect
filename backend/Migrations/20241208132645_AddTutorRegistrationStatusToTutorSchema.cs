using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddTutorRegistrationStatusToTutorSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TutorRegistrationStatus",
                newName: "TutorRegistrationStatus",
                newSchema: "Tutor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TutorRegistrationStatus",
                schema: "Tutor",
                newName: "TutorRegistrationStatus");
        }
    }
}
