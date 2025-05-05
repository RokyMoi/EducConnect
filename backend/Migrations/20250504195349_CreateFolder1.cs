using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class CreateFolder1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FolderId",
                schema: "Course",
                table: "CourseThumbnail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FolderId",
                schema: "Course",
                table: "CourseTeachingResource",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FolderId",
                schema: "Course",
                table: "CoursePromotionImage",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FolderId",
                schema: "Course",
                table: "CourseLessonResource",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Folder",
                schema: "Course",
                columns: table => new
                {
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentFolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OwnerPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.FolderId);
                    table.ForeignKey(
                        name: "FK_Folder_Folder_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalSchema: "Course",
                        principalTable: "Folder",
                        principalColumn: "FolderId");
                    table.ForeignKey(
                        name: "FK_Folder_Person_OwnerPersonId",
                        column: x => x.OwnerPersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseThumbnail_FolderId",
                schema: "Course",
                table: "CourseThumbnail",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTeachingResource_FolderId",
                schema: "Course",
                table: "CourseTeachingResource",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePromotionImage_FolderId",
                schema: "Course",
                table: "CoursePromotionImage",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonResource_FolderId",
                schema: "Course",
                table: "CourseLessonResource",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_OwnerPersonId",
                schema: "Course",
                table: "Folder",
                column: "OwnerPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_ParentFolderId",
                schema: "Course",
                table: "Folder",
                column: "ParentFolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLessonResource_Folder_FolderId",
                schema: "Course",
                table: "CourseLessonResource",
                column: "FolderId",
                principalSchema: "Course",
                principalTable: "Folder",
                principalColumn: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePromotionImage_Folder_FolderId",
                schema: "Course",
                table: "CoursePromotionImage",
                column: "FolderId",
                principalSchema: "Course",
                principalTable: "Folder",
                principalColumn: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTeachingResource_Folder_FolderId",
                schema: "Course",
                table: "CourseTeachingResource",
                column: "FolderId",
                principalSchema: "Course",
                principalTable: "Folder",
                principalColumn: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseThumbnail_Folder_FolderId",
                schema: "Course",
                table: "CourseThumbnail",
                column: "FolderId",
                principalSchema: "Course",
                principalTable: "Folder",
                principalColumn: "FolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseLessonResource_Folder_FolderId",
                schema: "Course",
                table: "CourseLessonResource");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursePromotionImage_Folder_FolderId",
                schema: "Course",
                table: "CoursePromotionImage");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTeachingResource_Folder_FolderId",
                schema: "Course",
                table: "CourseTeachingResource");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseThumbnail_Folder_FolderId",
                schema: "Course",
                table: "CourseThumbnail");

            migrationBuilder.DropTable(
                name: "Folder",
                schema: "Course");

            migrationBuilder.DropIndex(
                name: "IX_CourseThumbnail_FolderId",
                schema: "Course",
                table: "CourseThumbnail");

            migrationBuilder.DropIndex(
                name: "IX_CourseTeachingResource_FolderId",
                schema: "Course",
                table: "CourseTeachingResource");

            migrationBuilder.DropIndex(
                name: "IX_CoursePromotionImage_FolderId",
                schema: "Course",
                table: "CoursePromotionImage");

            migrationBuilder.DropIndex(
                name: "IX_CourseLessonResource_FolderId",
                schema: "Course",
                table: "CourseLessonResource");

            migrationBuilder.DropColumn(
                name: "FolderId",
                schema: "Course",
                table: "CourseThumbnail");

            migrationBuilder.DropColumn(
                name: "FolderId",
                schema: "Course",
                table: "CourseTeachingResource");

            migrationBuilder.DropColumn(
                name: "FolderId",
                schema: "Course",
                table: "CoursePromotionImage");

            migrationBuilder.DropColumn(
                name: "FolderId",
                schema: "Course",
                table: "CourseLessonResource");
        }
    }
}
