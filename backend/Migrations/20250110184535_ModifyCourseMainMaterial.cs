using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCourseMainMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileTitle",
                schema: "Course",
                table: "CourseMainMaterial");

            migrationBuilder.RenameColumn(
                name: "CourseMainMaterialsId",
                schema: "Course",
                table: "CourseMainMaterial",
                newName: "CourseMainMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMainMaterial_CourseId",
                schema: "Course",
                table: "CourseMainMaterial",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMainMaterial_Course_CourseId",
                schema: "Course",
                table: "CourseMainMaterial",
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
                name: "FK_CourseMainMaterial_Course_CourseId",
                schema: "Course",
                table: "CourseMainMaterial");

            migrationBuilder.DropIndex(
                name: "IX_CourseMainMaterial_CourseId",
                schema: "Course",
                table: "CourseMainMaterial");

            migrationBuilder.RenameColumn(
                name: "CourseMainMaterialId",
                schema: "Course",
                table: "CourseMainMaterial",
                newName: "CourseMainMaterialsId");

            migrationBuilder.AddColumn<string>(
                name: "FileTitle",
                schema: "Course",
                table: "CourseMainMaterial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
