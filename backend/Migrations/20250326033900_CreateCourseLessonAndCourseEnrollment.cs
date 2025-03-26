using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class CreateCourseLessonAndCourseEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseEnrollment",
                schema: "Course",
                columns: table => new
                {
                    CourseEnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEnrollment", x => x.CourseEnrollmentId);
                    table.ForeignKey(
                        name: "FK_CourseEnrollment_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrollment_Student_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "Student",
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseLesson",
                schema: "Course",
                columns: table => new
                {
                    CourseLessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonSequenceOrder = table.Column<int>(type: "int", nullable: true),
                    PublishedStatus = table.Column<bool>(type: "bit", nullable: true),
                    StatusChangedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_CourseLesson_Tutor_TutorId",
                        column: x => x.TutorId,
                        principalSchema: "Tutor",
                        principalTable: "Tutor",
                        principalColumn: "TutorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollment_CourseId",
                schema: "Course",
                table: "CourseEnrollment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollment_StudentId",
                schema: "Course",
                table: "CourseEnrollment",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLesson_CourseId",
                schema: "Course",
                table: "CourseLesson",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLesson_TutorId",
                schema: "Course",
                table: "CourseLesson",
                column: "TutorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseEnrollment",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CourseLesson",
                schema: "Course");
        }
    }
}
