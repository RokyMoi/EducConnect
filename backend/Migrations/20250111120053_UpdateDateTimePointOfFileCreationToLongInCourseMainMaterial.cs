using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDateTimePointOfFileCreationToLongInCourseMainMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "DateTimePointOfFileCreation",
                schema: "Course",
                table: "CourseMainMaterial",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimePointOfFileCreation",
                schema: "Course",
                table: "CourseMainMaterial",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
