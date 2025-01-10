using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCourseLangugageAndCourseDetailsWithPKAndFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseLanguage",
                schema: "Course",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLanguage", x => new { x.CourseId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_CourseLanguage_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseLanguage_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Reference",
                        principalTable: "Language",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseLanguage_LanguageId",
                schema: "Course",
                table: "CourseLanguage",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseDetails_Course_CourseId",
                schema: "Course",
                table: "CourseDetails",
                column: "CourseId",
                principalSchema: "Course",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseDetails_Course_CourseId",
                schema: "Course",
                table: "CourseDetails");

            migrationBuilder.DropTable(
                name: "CourseLanguage",
                schema: "Course");
        }
    }
}
