using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecificExpertiseAreaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecificExpertiseArea",
                schema: "Education",
                columns: table => new
                {
                    SpecificExpertiseFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeneralExpertiseFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecificExpertiseFieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecificExpertiseArea",
                schema: "Education");
        }
    }
}
