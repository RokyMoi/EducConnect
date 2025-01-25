using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddNavPropertiesToCourseLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CourseLessonContent_CourseLessonId",
                schema: "Course",
                table: "CourseLessonContent");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonContent_CourseLessonId",
                schema: "Course",
                table: "CourseLessonContent",
                column: "CourseLessonId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CourseLessonContent_CourseLessonId",
                schema: "Course",
                table: "CourseLessonContent");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonContent_CourseLessonId",
                schema: "Course",
                table: "CourseLessonContent",
                column: "CourseLessonId");
        }
    }
}
