using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddedCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Reference");

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "Reference",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfficialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalCallingCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISOAlpha2Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlagUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlagEmoji = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Country",
                schema: "Reference");
        }
    }
}
