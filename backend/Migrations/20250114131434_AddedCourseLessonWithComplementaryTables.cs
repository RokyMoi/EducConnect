using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddedCourseLessonWithComplementaryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseLesson",
                schema: "Course",
                columns: table => new
                {
                    CourseLessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonSequenceOrder = table.Column<int>(type: "int", nullable: false),
                    LessonPrerequisites = table.Column<string>(type: "nvarchar(510)", maxLength: 510, nullable: false),
                    LessonObjective = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LessonCompletionTimeInMinutes = table.Column<int>(type: "int", nullable: false),
                    LessonTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLesson", x => x.CourseLessonId);
                    table.ForeignKey(
                        name: "FK_CourseLesson_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseLessonContent",
                schema: "Course",
                columns: table => new
                {
                    CourseLessonContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseLessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentSize = table.Column<long>(type: "bigint", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DateTimePointOfFileCreation = table.Column<long>(type: "bigint", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "CourseLessonSupplementaryMaterial",
                schema: "Course",
                columns: table => new
                {
                    CourseLessonSupplementaryMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseLessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentSize = table.Column<long>(type: "bigint", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DateTimePointOfFileCreation = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLessonSupplementaryMaterial", x => x.CourseLessonSupplementaryMaterialId);
                    table.ForeignKey(
                        name: "FK_CourseLessonSupplementaryMaterial_CourseLesson_CourseLessonId",
                        column: x => x.CourseLessonId,
                        principalSchema: "Course",
                        principalTable: "CourseLesson",
                        principalColumn: "CourseLessonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseLesson_CourseId",
                schema: "Course",
                table: "CourseLesson",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonContent_CourseLessonId",
                schema: "Course",
                table: "CourseLessonContent",
                column: "CourseLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonSupplementaryMaterial_CourseLessonId",
                schema: "Course",
                table: "CourseLessonSupplementaryMaterial",
                column: "CourseLessonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseLessonContent",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CourseLessonSupplementaryMaterial",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CourseLesson",
                schema: "Course");
        }
    }
}
