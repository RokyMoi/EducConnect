using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddCarrerInformationWorkTypeEmploymentTypeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmploymentType",
                schema: "Reference",
                columns: table => new
                {
                    EmploymentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentType", x => x.EmploymentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "WorkType",
                columns: table => new
                {
                    WorkTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkType", x => x.WorkTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PersonCareerInformation",
                schema: "Person",
                columns: table => new
                {
                    PersonCareerInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyWebsite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityOfEmployment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryOfEmployment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmploymentTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<long>(type: "bigint", nullable: false),
                    EndDate = table.Column<long>(type: "bigint", nullable: true),
                    JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Responsibilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Achievements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillsUsed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkTypeId = table.Column<int>(type: "int", nullable: true),
                    AdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonCareerInformation", x => x.PersonCareerInformationId);
                    table.ForeignKey(
                        name: "FK_PersonCareerInformation_EmploymentType_EmploymentTypeId",
                        column: x => x.EmploymentTypeId,
                        principalSchema: "Reference",
                        principalTable: "EmploymentType",
                        principalColumn: "EmploymentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonCareerInformation_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonCareerInformation_WorkType_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkType",
                        principalColumn: "WorkTypeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonCareerInformation_EmploymentTypeId",
                schema: "Person",
                table: "PersonCareerInformation",
                column: "EmploymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCareerInformation_PersonId",
                schema: "Person",
                table: "PersonCareerInformation",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCareerInformation_WorkTypeId",
                schema: "Person",
                table: "PersonCareerInformation",
                column: "WorkTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonCareerInformation",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "EmploymentType",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "WorkType");
        }
    }
}
