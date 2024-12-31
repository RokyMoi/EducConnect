using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class SeparatePhoneNumberFromPersonDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryOfOrigin",
                schema: "Person",
                table: "PersonDetails");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "Person",
                table: "PersonDetails");

            migrationBuilder.DropColumn(
                name: "PhoneNumberCountryCode",
                schema: "Person",
                table: "PersonDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "CountryOfOriginCountryId",
                schema: "Person",
                table: "PersonDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonDetails_CountryOfOriginCountryId",
                schema: "Person",
                table: "PersonDetails",
                column: "CountryOfOriginCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonDetails_Country_CountryOfOriginCountryId",
                schema: "Person",
                table: "PersonDetails",
                column: "CountryOfOriginCountryId",
                principalSchema: "Reference",
                principalTable: "Country",
                principalColumn: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonDetails_Country_CountryOfOriginCountryId",
                schema: "Person",
                table: "PersonDetails");

            migrationBuilder.DropIndex(
                name: "IX_PersonDetails_CountryOfOriginCountryId",
                schema: "Person",
                table: "PersonDetails");

            migrationBuilder.DropColumn(
                name: "CountryOfOriginCountryId",
                schema: "Person",
                table: "PersonDetails");

            migrationBuilder.AddColumn<string>(
                name: "CountryOfOrigin",
                schema: "Person",
                table: "PersonDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "Person",
                table: "PersonDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberCountryCode",
                schema: "Person",
                table: "PersonDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
