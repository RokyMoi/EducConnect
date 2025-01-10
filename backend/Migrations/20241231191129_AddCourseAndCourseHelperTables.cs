using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseAndCourseHelperTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Course");

            migrationBuilder.CreateTable(
                name: "Course",
                schema: "Course",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Course_Tutor_TutorId",
                        column: x => x.TutorId,
                        principalSchema: "Tutor",
                        principalTable: "Tutor",
                        principalColumn: "TutorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                schema: "Reference",
                columns: table => new
                {
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRightToLeft = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.LanguageId);
                });

            migrationBuilder.CreateTable(
                name: "LearningDifficultyLevel",
                schema: "Reference",
                columns: table => new
                {
                    LearningDifficultyLevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningDifficultyLevel", x => x.LearningDifficultyLevelId);
                });

            migrationBuilder.CreateTable(
                name: "CourseDetails",
                schema: "Course",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningSubcategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LearningDifficultyLevelId = table.Column<int>(type: "int", nullable: false),
                    CourseStartsAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseEndsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstimatedDurationToCompleteTheCourseInHours = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDetails", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_CourseDetails_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseDetails_LearningDifficultyLevel_LearningDifficultyLevelId",
                        column: x => x.LearningDifficultyLevelId,
                        principalSchema: "Reference",
                        principalTable: "LearningDifficultyLevel",
                        principalColumn: "LearningDifficultyLevelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseDetails_LearningSubcategory_LearningSubcategoryId",
                        column: x => x.LearningSubcategoryId,
                        principalSchema: "Learning",
                        principalTable: "LearningSubcategory",
                        principalColumn: "LearningSubcategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_TutorId",
                schema: "Course",
                table: "Course",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDetails_LearningDifficultyLevelId",
                schema: "Course",
                table: "CourseDetails",
                column: "LearningDifficultyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDetails_LearningSubcategoryId",
                schema: "Course",
                table: "CourseDetails",
                column: "LearningSubcategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseDetails",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "Language",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "Course",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "LearningDifficultyLevel",
                schema: "Reference");
        }
    }
}
