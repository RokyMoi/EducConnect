using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseLessonContentAndAddVirtualNavPropertyToCourseLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseLessonContent",
                schema: "Course",
                columns: table => new
                {
                    CourseLessonContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseLessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLessonContent", x => x.CourseLessonContentId);
                    table.ForeignKey(
                        name: "FK_CourseLessonContent_CourseLesson_CourseLessonId",
                        column: x => x.CourseLessonId,
                        principalSchema: "Course",
                        principalTable: "CourseLesson",
                        principalColumn: "CourseLessonId",
                        onDelete: ReferentialAction.Cascade);
                });

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
            migrationBuilder.DropTable(
                name: "CourseLessonContent",
                schema: "Course");
        }
    }
}
