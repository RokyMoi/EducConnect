using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseLessonResource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseLessonResource",
                schema: "Course",
                columns: table => new
                {
                    CourseLessonResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseLessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ResourceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLessonResource", x => x.CourseLessonResourceId);
                    table.ForeignKey(
                        name: "FK_CourseLessonResource_CourseLesson_CourseLessonId",
                        column: x => x.CourseLessonId,
                        principalSchema: "Course",
                        principalTable: "CourseLesson",
                        principalColumn: "CourseLessonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonResource_CourseLessonId",
                schema: "Course",
                table: "CourseLessonResource",
                column: "CourseLessonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseLessonResource",
                schema: "Course");
        }
    }
}
