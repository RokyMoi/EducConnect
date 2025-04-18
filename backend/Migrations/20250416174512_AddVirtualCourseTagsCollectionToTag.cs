using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddVirtualCourseTagsCollectionToTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TagId1",
                schema: "Course",
                table: "CourseTag",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseTag_TagId1",
                schema: "Course",
                table: "CourseTag",
                column: "TagId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTag_Tag_TagId1",
                schema: "Course",
                table: "CourseTag",
                column: "TagId1",
                principalSchema: "Course",
                principalTable: "Tag",
                principalColumn: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseTag_Tag_TagId1",
                schema: "Course",
                table: "CourseTag");

            migrationBuilder.DropIndex(
                name: "IX_CourseTag_TagId1",
                schema: "Course",
                table: "CourseTag");

            migrationBuilder.DropColumn(
                name: "TagId1",
                schema: "Course",
                table: "CourseTag");
        }
    }
}
