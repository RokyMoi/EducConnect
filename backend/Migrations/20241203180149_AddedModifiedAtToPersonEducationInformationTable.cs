using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddedModifiedAtToPersonEducationInformationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedAt",
                schema: "Person",
                table: "PersonEducationInformation",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedAt",
                schema: "Person",
                table: "PersonEducationInformation",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Person",
                table: "PersonEducationInformation");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                schema: "Person",
                table: "PersonEducationInformation");
        }
    }
}
