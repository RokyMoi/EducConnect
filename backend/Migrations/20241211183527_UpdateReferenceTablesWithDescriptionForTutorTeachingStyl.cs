using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReferenceTablesWithDescriptionForTutorTeachingStyl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Reference",
                table: "TutorTeachingStyleType",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Reference",
                table: "EngagementMethod",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Reference",
                table: "TutorTeachingStyleType");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Reference",
                table: "EngagementMethod");
        }
    }
}
