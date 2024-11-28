using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class SchemaOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Person");

            migrationBuilder.EnsureSchema(
                name: "Student");

            migrationBuilder.EnsureSchema(
                name: "Tutor");

            migrationBuilder.RenameTable(
                name: "TutorRating",
                newName: "TutorRating",
                newSchema: "Tutor");

            migrationBuilder.RenameTable(
                name: "TutorDetails",
                newName: "TutorDetails",
                newSchema: "Tutor");

            migrationBuilder.RenameTable(
                name: "TutorCertification",
                newName: "TutorCertification",
                newSchema: "Tutor");

            migrationBuilder.RenameTable(
                name: "TutorAvailability",
                newName: "TutorAvailability",
                newSchema: "Tutor");

            migrationBuilder.RenameTable(
                name: "Tutor",
                newName: "Tutor",
                newSchema: "Tutor");

            migrationBuilder.RenameTable(
                name: "StudentDetails",
                newName: "StudentDetails",
                newSchema: "Student");

            migrationBuilder.RenameTable(
                name: "Student",
                newName: "Student",
                newSchema: "Student");

            migrationBuilder.RenameTable(
                name: "PersonSalt",
                newName: "PersonSalt",
                newSchema: "Person");

            migrationBuilder.RenameTable(
                name: "PersonProfilePicture",
                newName: "PersonProfilePicture",
                newSchema: "Person");

            migrationBuilder.RenameTable(
                name: "PersonPassword",
                newName: "PersonPassword",
                newSchema: "Person");

            migrationBuilder.RenameTable(
                name: "PersonEmail",
                newName: "PersonEmail",
                newSchema: "Person");

            migrationBuilder.RenameTable(
                name: "PersonDetails",
                newName: "PersonDetails",
                newSchema: "Person");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "Person",
                newSchema: "Person");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TutorRating",
                schema: "Tutor",
                newName: "TutorRating");

            migrationBuilder.RenameTable(
                name: "TutorDetails",
                schema: "Tutor",
                newName: "TutorDetails");

            migrationBuilder.RenameTable(
                name: "TutorCertification",
                schema: "Tutor",
                newName: "TutorCertification");

            migrationBuilder.RenameTable(
                name: "TutorAvailability",
                schema: "Tutor",
                newName: "TutorAvailability");

            migrationBuilder.RenameTable(
                name: "Tutor",
                schema: "Tutor",
                newName: "Tutor");

            migrationBuilder.RenameTable(
                name: "StudentDetails",
                schema: "Student",
                newName: "StudentDetails");

            migrationBuilder.RenameTable(
                name: "Student",
                schema: "Student",
                newName: "Student");

            migrationBuilder.RenameTable(
                name: "PersonSalt",
                schema: "Person",
                newName: "PersonSalt");

            migrationBuilder.RenameTable(
                name: "PersonProfilePicture",
                schema: "Person",
                newName: "PersonProfilePicture");

            migrationBuilder.RenameTable(
                name: "PersonPassword",
                schema: "Person",
                newName: "PersonPassword");

            migrationBuilder.RenameTable(
                name: "PersonEmail",
                schema: "Person",
                newName: "PersonEmail");

            migrationBuilder.RenameTable(
                name: "PersonDetails",
                schema: "Person",
                newName: "PersonDetails");

            migrationBuilder.RenameTable(
                name: "Person",
                schema: "Person",
                newName: "Person");
        }
    }
}
