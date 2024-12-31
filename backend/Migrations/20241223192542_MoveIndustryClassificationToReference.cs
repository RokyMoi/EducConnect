using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class MoveIndustryClassificationToReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IndustryClassification",
                table: "IndustryClassification");

            migrationBuilder.RenameTable(
                name: "IndustryClassification",
                newName: "IndustryClassifications",
                newSchema: "Reference");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IndustryClassifications",
                schema: "Reference",
                table: "IndustryClassifications",
                column: "IndustryClassificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IndustryClassifications",
                schema: "Reference",
                table: "IndustryClassifications");

            migrationBuilder.RenameTable(
                name: "IndustryClassifications",
                schema: "Reference",
                newName: "IndustryClassification");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IndustryClassification",
                table: "IndustryClassification",
                column: "IndustryClassificationId");
        }
    }
}
