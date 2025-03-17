using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLanguagePropertyFromCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Language_LanguageId",
                schema: "Course",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_LanguageId",
                schema: "Course",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                schema: "Course",
                table: "Course");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                schema: "Course",
                table: "Course",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Course_LanguageId",
                schema: "Course",
                table: "Course",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Language_LanguageId",
                schema: "Course",
                table: "Course",
                column: "LanguageId",
                principalSchema: "Reference",
                principalTable: "Language",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
