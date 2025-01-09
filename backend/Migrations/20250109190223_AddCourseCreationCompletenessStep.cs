using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseCreationCompletenessStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseCreationCompletenessStepId",
                schema: "Course",
                table: "Course",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CourseCreationCompletenessStep",
                schema: "Reference",
                columns: table => new
                {
                    CourseCreationCompletenessStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    StepName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCreationCompletenessStep", x => x.CourseCreationCompletenessStepId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_CourseCreationCompletenessStepId",
                schema: "Course",
                table: "Course",
                column: "CourseCreationCompletenessStepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_CourseCreationCompletenessStep_CourseCreationCompletenessStepId",
                schema: "Course",
                table: "Course",
                column: "CourseCreationCompletenessStepId",
                principalSchema: "Reference",
                principalTable: "CourseCreationCompletenessStep",
                principalColumn: "CourseCreationCompletenessStepId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_CourseCreationCompletenessStep_CourseCreationCompletenessStepId",
                schema: "Course",
                table: "Course");

            migrationBuilder.DropTable(
                name: "CourseCreationCompletenessStep",
                schema: "Reference");

            migrationBuilder.DropIndex(
                name: "IX_Course_CourseCreationCompletenessStepId",
                schema: "Course",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CourseCreationCompletenessStepId",
                schema: "Course",
                table: "Course");
        }
    }
}
