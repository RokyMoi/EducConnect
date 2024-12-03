using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class RenameEducationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecificExpertiseArea",
                schema: "Education");

            migrationBuilder.DropTable(
                name: "GeneralExpertiseField",
                schema: "Education");

            migrationBuilder.CreateTable(
                name: "GeneralExpertiseArea",
                schema: "Education",
                columns: table => new
                {
                    GeneralExpertiseAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeneralExpertiseAreaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneralExpertiseAreaDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralExpertiseArea", x => x.GeneralExpertiseAreaId);
                });

            migrationBuilder.CreateTable(
                name: "SpecificExpertiseField",
                schema: "Education",
                columns: table => new
                {
                    SpecificExpertiseFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeneralExpertiseAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecificExpertiseFieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificExpertiseField", x => x.SpecificExpertiseFieldId);
                    table.ForeignKey(
                        name: "FK_SpecificExpertiseField_GeneralExpertiseArea_GeneralExpertiseAreaId",
                        column: x => x.GeneralExpertiseAreaId,
                        principalSchema: "Education",
                        principalTable: "GeneralExpertiseArea",
                        principalColumn: "GeneralExpertiseAreaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecificExpertiseField_GeneralExpertiseAreaId",
                schema: "Education",
                table: "SpecificExpertiseField",
                column: "GeneralExpertiseAreaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecificExpertiseField",
                schema: "Education");

            migrationBuilder.DropTable(
                name: "GeneralExpertiseArea",
                schema: "Education");

            migrationBuilder.CreateTable(
                name: "GeneralExpertiseField",
                schema: "Education",
                columns: table => new
                {
                    ExpertiseGeneralFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ExpertiseGeneralFieldDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpertiseGeneralFieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralExpertiseField", x => x.ExpertiseGeneralFieldId);
                });

            migrationBuilder.CreateTable(
                name: "SpecificExpertiseArea",
                schema: "Education",
                columns: table => new
                {
                    SpecificExpertiseFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeneralExpertiseFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificExpertiseFieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificExpertiseArea", x => x.SpecificExpertiseFieldId);
                    table.ForeignKey(
                        name: "FK_SpecificExpertiseArea_GeneralExpertiseField_GeneralExpertiseFieldId",
                        column: x => x.GeneralExpertiseFieldId,
                        principalSchema: "Education",
                        principalTable: "GeneralExpertiseField",
                        principalColumn: "ExpertiseGeneralFieldId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecificExpertiseArea_GeneralExpertiseFieldId",
                schema: "Education",
                table: "SpecificExpertiseArea",
                column: "GeneralExpertiseFieldId");
        }
    }
}
