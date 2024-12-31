using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintTutorTeachingInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TutorTeachingInformation_TutorId",
                schema: "Tutor",
                table: "TutorTeachingInformation");

            migrationBuilder.AlterColumn<int>(
                name: "ExpectedResponseTime",
                schema: "Tutor",
                table: "TutorTeachingInformation",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<long>(
                name: "CreatedAt",
                schema: "Tutor",
                table: "TutorTeachingInformation",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedAt",
                schema: "Tutor",
                table: "TutorTeachingInformation",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_TutorId",
                schema: "Tutor",
                table: "TutorTeachingInformation",
                column: "TutorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TutorTeachingInformation_TutorId",
                schema: "Tutor",
                table: "TutorTeachingInformation");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Tutor",
                table: "TutorTeachingInformation");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "Tutor",
                table: "TutorTeachingInformation");

            migrationBuilder.AlterColumn<string>(
                name: "ExpectedResponseTime",
                schema: "Tutor",
                table: "TutorTeachingInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_TutorId",
                schema: "Tutor",
                table: "TutorTeachingInformation",
                column: "TutorId");
        }
    }
}
