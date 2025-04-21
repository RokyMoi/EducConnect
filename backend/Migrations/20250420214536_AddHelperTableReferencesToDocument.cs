using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddHelperTableReferencesToDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId1",
                schema: "Document",
                table: "CollaborationDocumentInvitation",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationDocumentInvitation_DocumentId1",
                schema: "Document",
                table: "CollaborationDocumentInvitation",
                column: "DocumentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CollaborationDocumentInvitation_Document_DocumentId1",
                schema: "Document",
                table: "CollaborationDocumentInvitation",
                column: "DocumentId1",
                principalSchema: "Document",
                principalTable: "Document",
                principalColumn: "DocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollaborationDocumentInvitation_Document_DocumentId1",
                schema: "Document",
                table: "CollaborationDocumentInvitation");

            migrationBuilder.DropIndex(
                name: "IX_CollaborationDocumentInvitation_DocumentId1",
                schema: "Document",
                table: "CollaborationDocumentInvitation");

            migrationBuilder.DropColumn(
                name: "DocumentId1",
                schema: "Document",
                table: "CollaborationDocumentInvitation");
        }
    }
}
