using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnIndustryClassificationToPersonCareerInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Industry",
                schema: "Person",
                table: "PersonCareerInformation");

            migrationBuilder.AddColumn<Guid>(
                name: "IndustryClassificationId",
                schema: "Person",
                table: "PersonCareerInformation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PersonCareerInformation_IndustryClassificationId",
                schema: "Person",
                table: "PersonCareerInformation",
                column: "IndustryClassificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCareerInformation_IndustryClassification_IndustryClassificationId",
                schema: "Person",
                table: "PersonCareerInformation",
                column: "IndustryClassificationId",
                principalSchema: "Reference",
                principalTable: "IndustryClassification",
                principalColumn: "IndustryClassificationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonCareerInformation_IndustryClassification_IndustryClassificationId",
                schema: "Person",
                table: "PersonCareerInformation");

            migrationBuilder.DropIndex(
                name: "IX_PersonCareerInformation_IndustryClassificationId",
                schema: "Person",
                table: "PersonCareerInformation");

            migrationBuilder.DropColumn(
                name: "IndustryClassificationId",
                schema: "Person",
                table: "PersonCareerInformation");

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                schema: "Person",
                table: "PersonCareerInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
