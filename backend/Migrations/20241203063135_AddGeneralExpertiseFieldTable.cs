using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneralExpertiseFieldTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Education");

            migrationBuilder.CreateTable(
                name: "GeneralExpertiseField",
                schema: "Education",
                columns: table => new
                {
                    ExpertiseGeneralFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpertiseGeneralFieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpertiseGeneralFieldDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralExpertiseField", x => x.ExpertiseGeneralFieldId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralExpertiseField",
                schema: "Education");
        }
    }
}
