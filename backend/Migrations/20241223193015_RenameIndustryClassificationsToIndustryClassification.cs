using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class RenameIndustryClassificationsToIndustryClassification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IndustryClassifications",
                schema: "Reference",
                table: "IndustryClassifications");

            migrationBuilder.RenameTable(
                name: "IndustryClassifications",
                schema: "Reference",
                newName: "IndustryClassification",
                newSchema: "Reference");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IndustryClassification",
                schema: "Reference",
                table: "IndustryClassification",
                column: "IndustryClassificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IndustryClassification",
                schema: "Reference",
                table: "IndustryClassification");

            migrationBuilder.RenameTable(
                name: "IndustryClassification",
                schema: "Reference",
                newName: "IndustryClassifications",
                newSchema: "Reference");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IndustryClassifications",
                schema: "Reference",
                table: "IndustryClassifications",
                column: "IndustryClassificationId");
        }
    }
}
