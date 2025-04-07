using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class CreateCoursePromotionImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoursePromotionImage",
                schema: "Course",
                columns: table => new
                {
                    CoursePromotionImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursePromotionImage", x => x.CoursePromotionImageId);
                    table.ForeignKey(
                        name: "FK_CoursePromotionImage_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoursePromotionImage_CourseId",
                schema: "Course",
                table: "CoursePromotionImage",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoursePromotionImage",
                schema: "Course");
        }
    }
}
