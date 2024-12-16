using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonEducationInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonEducationInformation",
                schema: "Person",
                columns: table => new
                {
                    PersonEducationInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstitutionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstitutionOfficialWebsite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstitutionAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinorFieldOfStudy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    FinalGrade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonEducationInformation", x => x.PersonEducationInformationId);
                    table.ForeignKey(
                        name: "FK_PersonEducationInformation_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonEducationInformation_PersonId",
                schema: "Person",
                table: "PersonEducationInformation",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonEducationInformation",
                schema: "Person");
        }
    }
}
