using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddTableTutorRegistrationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TutorRegistrationStatus",
                columns: table => new
                {
                    TutorRegistrationStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorRegistrationStatus", x => x.TutorRegistrationStatusId);
                });

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
                principalTable: "TutorRegistrationStatus",
                principalColumn: "TutorRegistrationStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutor_TutorRegistrationStatus_TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor");

            migrationBuilder.DropTable(
                name: "TutorRegistrationStatus");

            migrationBuilder.DropIndex(
                name: "IX_Tutor_TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor");

            migrationBuilder.DropColumn(
                name: "TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor");
        }
    }
}
