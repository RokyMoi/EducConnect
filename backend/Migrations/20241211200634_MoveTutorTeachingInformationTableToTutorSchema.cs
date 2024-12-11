using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class MoveTutorTeachingInformationTableToTutorSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TutorTeachingInformation",
                newName: "TutorTeachingInformation",
                newSchema: "Tutor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TutorTeachingInformation",
                schema: "Tutor",
                newName: "TutorTeachingInformation");
        }
    }
}
