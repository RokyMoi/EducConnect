using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddCollaborationDocumentActiveUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "CollaborationDocumentActiveUser",
                newName: "CollaborationDocumentActiveUser",
                newSchema: "Document");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "CollaborationDocumentActiveUser",
                schema: "Document",
                newName: "CollaborationDocumentActiveUser");
        }
    }
}
