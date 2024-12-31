using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddReferenceTablesForTutorTeachingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutor_TutorRegistrationStatus_TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor");

            migrationBuilder.AlterColumn<int>(
                name: "TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CommunicationType",
                schema: "Reference",
                columns: table => new
                {
                    CommunicationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationType", x => x.CommunicationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "EngagementMethod",
                schema: "Reference",
                columns: table => new
                {
                    EngagementMethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngagementMethod", x => x.EngagementMethodId);
                });

            migrationBuilder.CreateTable(
                name: "TutorTeachingStyleType",
                schema: "Reference",
                columns: table => new
                {
                    TutorTeachingStyleTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorTeachingStyleType", x => x.TutorTeachingStyleTypeId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tutor_TutorRegistrationStatus_TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor",
                column: "TutorRegistrationStatusId",
                principalSchema: "Reference",
                principalTable: "TutorRegistrationStatus",
                principalColumn: "TutorRegistrationStatusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutor_TutorRegistrationStatus_TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor");

            migrationBuilder.DropTable(
                name: "CommunicationType",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "EngagementMethod",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "TutorTeachingStyleType",
                schema: "Reference");

            migrationBuilder.AlterColumn<int>(
                name: "TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutor_TutorRegistrationStatus_TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor",
                column: "TutorRegistrationStatusId",
                principalSchema: "Reference",
                principalTable: "TutorRegistrationStatus",
                principalColumn: "TutorRegistrationStatusId");
        }
    }
}
