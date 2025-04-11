using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class CreateCourseViewershipData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseViewershipData",
                schema: "Course",
                columns: table => new
                {
                    CourseViewershipDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViewedByPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClickedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnteredDetailsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeftDetailsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserCameFrom = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseViewershipData", x => x.CourseViewershipDataId);
                    table.ForeignKey(
                        name: "FK_CourseViewershipData_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseViewershipData_Person_ViewedByPersonId",
                        column: x => x.ViewedByPersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseViewershipData_CourseId",
                schema: "Course",
                table: "CourseViewershipData",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseViewershipData_ViewedByPersonId",
                schema: "Course",
                table: "CourseViewershipData",
                column: "ViewedByPersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseViewershipData",
                schema: "Course");
        }
    }
}
