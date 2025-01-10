using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseAndHelperTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseDetails_Course_CourseId",
                schema: "Course",
                table: "CourseDetails");

            migrationBuilder.DropColumn(
                name: "CourseEndsAt",
                schema: "Course",
                table: "CourseDetails");

            migrationBuilder.DropColumn(
                name: "CourseStartsAt",
                schema: "Course",
                table: "CourseDetails");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Course",
                table: "CourseDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "Course",
                table: "CourseDetails");

            migrationBuilder.RenameTable(
                name: "LearningSubcategory",
                schema: "Learning",
                newName: "LearningSubcategory",
                newSchema: "Reference");

            migrationBuilder.RenameTable(
                name: "LearningCategory",
                schema: "Learning",
                newName: "LearningCategory",
                newSchema: "Reference");

            migrationBuilder.RenameColumn(
                name: "EstimatedDurationToCompleteTheCourseInHours",
                schema: "Course",
                table: "CourseDetails",
                newName: "CourseTypeId");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                schema: "Course",
                table: "CourseDetails",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.CreateTable(
                name: "CourseType",
                columns: table => new
                {
                    CourseTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseType", x => x.CourseTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseDetails_CourseTypeId",
                schema: "Course",
                table: "CourseDetails",
                column: "CourseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseDetails_CourseType_CourseTypeId",
                schema: "Course",
                table: "CourseDetails",
                column: "CourseTypeId",
                principalTable: "CourseType",
                principalColumn: "CourseTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseDetails_CourseType_CourseTypeId",
                schema: "Course",
                table: "CourseDetails");

            migrationBuilder.DropTable(
                name: "CourseType");

            migrationBuilder.DropIndex(
                name: "IX_CourseDetails_CourseTypeId",
                schema: "Course",
                table: "CourseDetails");

            migrationBuilder.EnsureSchema(
                name: "Learning");

            migrationBuilder.RenameTable(
                name: "LearningSubcategory",
                schema: "Reference",
                newName: "LearningSubcategory",
                newSchema: "Learning");

            migrationBuilder.RenameTable(
                name: "LearningCategory",
                schema: "Reference",
                newName: "LearningCategory",
                newSchema: "Learning");

            migrationBuilder.RenameColumn(
                name: "CourseTypeId",
                schema: "Course",
                table: "CourseDetails",
                newName: "EstimatedDurationToCompleteTheCourseInHours");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                schema: "Course",
                table: "CourseDetails",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<DateTime>(
                name: "CourseEndsAt",
                schema: "Course",
                table: "CourseDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CourseStartsAt",
                schema: "Course",
                table: "CourseDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedAt",
                schema: "Course",
                table: "CourseDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedAt",
                schema: "Course",
                table: "CourseDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseDetails_Course_CourseId",
                schema: "Course",
                table: "CourseDetails",
                column: "CourseId",
                principalSchema: "Course",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
