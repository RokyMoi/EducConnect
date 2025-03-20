using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseThumbnailNavigationPropertyToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CourseThumbnail_CourseId",
                schema: "Course",
                table: "CourseThumbnail");

            migrationBuilder.CreateIndex(
                name: "IX_CourseThumbnail_CourseId",
                schema: "Course",
                table: "CourseThumbnail",
                column: "CourseId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CourseThumbnail_CourseId",
                schema: "Course",
                table: "CourseThumbnail");

            migrationBuilder.CreateIndex(
                name: "IX_CourseThumbnail_CourseId",
                schema: "Course",
                table: "CourseThumbnail",
                column: "CourseId");
        }
    }
}
