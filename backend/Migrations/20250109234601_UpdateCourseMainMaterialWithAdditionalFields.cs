using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseMainMaterialWithAdditionalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedAt",
                schema: "Course",
                table: "CourseMainMaterial",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimePointOfFileCreation",
                schema: "Course",
                table: "CourseMainMaterial",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "UpdatedAt",
                schema: "Course",
                table: "CourseMainMaterial",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Course",
                table: "CourseMainMaterial");

            migrationBuilder.DropColumn(
                name: "DateTimePointOfFileCreation",
                schema: "Course",
                table: "CourseMainMaterial");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "Course",
                table: "CourseMainMaterial");
        }
    }
}
