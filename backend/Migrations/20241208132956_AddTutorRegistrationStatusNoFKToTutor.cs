using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddTutorRegistrationStatusNoFKToTutor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutor_TutorRegistrationStatus_TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor");

            migrationBuilder.DropIndex(
                name: "IX_Tutor_TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor");

            migrationBuilder.DropColumn(
                name: "TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tutor_TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor",
                column: "TutorRegistrationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutor_TutorRegistrationStatus_TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor",
                column: "TutorRegistrationStatusId",
                principalSchema: "Tutor",
                principalTable: "TutorRegistrationStatus",
                principalColumn: "TutorRegistrationStatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
