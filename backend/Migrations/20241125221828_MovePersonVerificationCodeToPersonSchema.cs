using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class MovePersonVerificationCodeToPersonSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PersonVerificationCode",
                newName: "PersonVerificationCode",
                newSchema: "Person");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PersonVerificationCode",
                schema: "Person",
                newName: "PersonVerificationCode");
        }
    }
}
