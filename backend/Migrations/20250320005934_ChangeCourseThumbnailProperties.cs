using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCourseThumbnailProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                schema: "Course",
                table: "CourseThumbnail");

            migrationBuilder.DropColumn(
                name: "ThumbnailData",
                schema: "Course",
                table: "CourseThumbnail");

            migrationBuilder.RenameColumn(
                name: "ThumbnailName",
                schema: "Course",
                table: "CourseThumbnail",
                newName: "ThumbnailUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                schema: "Course",
                table: "CourseThumbnail",
                newName: "ThumbnailName");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                schema: "Course",
                table: "CourseThumbnail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "ThumbnailData",
                schema: "Course",
                table: "CourseThumbnail",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
