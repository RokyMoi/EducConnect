using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddedPersonPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonPhoneNumber",
                schema: "Person",
                columns: table => new
                {
                    PersonPhoneNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationalCallingCodeCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonPhoneNumber", x => x.PersonPhoneNumberId);
                    table.ForeignKey(
                        name: "FK_PersonPhoneNumber_Country_NationalCallingCodeCountryId",
                        column: x => x.NationalCallingCodeCountryId,
                        principalSchema: "Reference",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonPhoneNumber_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonPhoneNumber_NationalCallingCodeCountryId",
                schema: "Person",
                table: "PersonPhoneNumber",
                column: "NationalCallingCodeCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonPhoneNumber_PersonId",
                schema: "Person",
                table: "PersonPhoneNumber",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonPhoneNumber",
                schema: "Person");
        }
    }
}
