using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCourseLessonContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentSize",
                schema: "Course",
                table: "CourseLessonContent");

            migrationBuilder.DropColumn(
                name: "Data",
                schema: "Course",
                table: "CourseLessonContent");

            migrationBuilder.DropColumn(
                name: "DateTimePointOfFileCreation",
                schema: "Course",
                table: "CourseLessonContent");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Course",
                table: "CourseLessonContent");

            migrationBuilder.RenameColumn(
                name: "FileName",
                schema: "Course",
                table: "CourseLessonContent",
                newName: "ContentData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentData",
                schema: "Course",
                table: "CourseLessonContent",
                newName: "FileName");

            migrationBuilder.AddColumn<long>(
                name: "ContentSize",
                schema: "Course",
                table: "CourseLessonContent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                schema: "Course",
                table: "CourseLessonContent",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<long>(
                name: "DateTimePointOfFileCreation",
                schema: "Course",
                table: "CourseLessonContent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Course",
                table: "CourseLessonContent",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
