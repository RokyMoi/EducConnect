using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddNewLearningTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecificExpertiseField",
                schema: "Education");

            migrationBuilder.DropTable(
                name: "GeneralExpertiseArea",
                schema: "Education");

            migrationBuilder.EnsureSchema(
                name: "Learning");

            migrationBuilder.CreateTable(
                name: "LearningCategory",
                schema: "Learning",
                columns: table => new
                {
                    LearningCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LearningCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningCategoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningCategory", x => x.LearningCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "LearningSubcategory",
                schema: "Learning",
                columns: table => new
                {
                    LearningSubcategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LearningCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LearningSubcategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningSubcategory", x => x.LearningSubcategoryId);
                    table.ForeignKey(
                        name: "FK_LearningSubcategory_LearningCategory_LearningCategoryId",
                        column: x => x.LearningCategoryId,
                        principalSchema: "Learning",
                        principalTable: "LearningCategory",
                        principalColumn: "LearningCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningSubcategory_LearningCategoryId",
                schema: "Learning",
                table: "LearningSubcategory",
                column: "LearningCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearningSubcategory",
                schema: "Learning");

            migrationBuilder.DropTable(
                name: "LearningCategory",
                schema: "Learning");

            migrationBuilder.EnsureSchema(
                name: "Education");

            migrationBuilder.CreateTable(
                name: "GeneralExpertiseArea",
                schema: "Education",
                columns: table => new
                {
                    GeneralExpertiseAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    GeneralExpertiseAreaDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneralExpertiseAreaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificExpertiseFieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
    }
}
