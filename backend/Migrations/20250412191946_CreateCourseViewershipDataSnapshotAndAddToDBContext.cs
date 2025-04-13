using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class CreateCourseViewershipDataSnapshotAndAddToDBContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseViewershipDataSnapshot",
                schema: "Course",
                columns: table => new
                {
                    CourseViewershipDataSnapshotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalViews = table.Column<int>(type: "int", nullable: false),
                    NumberOfUniqueVisitors = table.Column<int>(type: "int", nullable: false),
                    CurrentlyViewing = table.Column<int>(type: "int", nullable: false),
                    AverageViewDurationInMinutes = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseViewershipDataSnapshot", x => x.CourseViewershipDataSnapshotId);
                    table.ForeignKey(
                        name: "FK_CourseViewershipDataSnapshot_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseViewershipDataSnapshot_CourseId",
                schema: "Course",
                table: "CourseViewershipDataSnapshot",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseViewershipDataSnapshot",
                schema: "Course");
        }
    }
}
