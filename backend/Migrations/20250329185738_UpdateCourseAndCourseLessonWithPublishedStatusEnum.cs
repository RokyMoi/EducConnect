using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseAndCourseLessonWithPublishedStatusEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PublishedStatus",
                schema: "Course",
                table: "CourseLesson",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PublishedStatus",
                schema: "Course",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "PublishedStatus",
                schema: "Course",
                table: "CourseLesson",
                type: "bit",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "PublishedStatus",
                schema: "Course",
                table: "Course",
                type: "bit",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
