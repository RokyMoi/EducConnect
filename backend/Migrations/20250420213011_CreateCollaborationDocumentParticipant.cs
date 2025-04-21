using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class CreateCollaborationDocumentParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollaborationDocumentActiveUser_CollaborationDocumentInvitation_CollaborationDocumentInvitationId",
                schema: "Document",
                table: "CollaborationDocumentActiveUser");

            migrationBuilder.DropIndex(
                name: "IX_CollaborationDocumentActiveUser_CollaborationDocumentInvitationId",
                schema: "Document",
                table: "CollaborationDocumentActiveUser");

            migrationBuilder.DropColumn(
                name: "CollaborationDocumentInvitationId",
                schema: "Document",
                table: "CollaborationDocumentActiveUser");

            migrationBuilder.RenameTable(
                name: "CollaborationDocumentActiveUser",
                schema: "Document",
                newName: "CollaborationDocumentActiveUser");

            migrationBuilder.CreateTable(
                name: "CollaborationDocumentParticipant",
                schema: "Document",
                columns: table => new
                {
                    CollaborationDocumentParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CollaborationDocumentInvitationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborationDocumentParticipant", x => x.CollaborationDocumentParticipantId);
                    table.ForeignKey(
                        name: "FK_CollaborationDocumentParticipant_CollaborationDocumentInvitation_CollaborationDocumentInvitationId",
                        column: x => x.CollaborationDocumentInvitationId,
                        principalSchema: "Document",
                        principalTable: "CollaborationDocumentInvitation",
                        principalColumn: "CollaborationDocumentInvitationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaborationDocumentParticipant_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "Document",
                        principalTable: "Document",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaborationDocumentParticipant_Person_ParticipantPersonId",
                        column: x => x.ParticipantPersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationDocumentParticipant_CollaborationDocumentInvitationId",
                schema: "Document",
                table: "CollaborationDocumentParticipant",
                column: "CollaborationDocumentInvitationId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationDocumentParticipant_DocumentId",
                schema: "Document",
                table: "CollaborationDocumentParticipant",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationDocumentParticipant_ParticipantPersonId",
                schema: "Document",
                table: "CollaborationDocumentParticipant",
                column: "ParticipantPersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollaborationDocumentParticipant",
                schema: "Document");

            migrationBuilder.RenameTable(
                name: "CollaborationDocumentActiveUser",
                newName: "CollaborationDocumentActiveUser",
                newSchema: "Document");

            migrationBuilder.AddColumn<Guid>(
                name: "CollaborationDocumentInvitationId",
                schema: "Document",
                table: "CollaborationDocumentActiveUser",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationDocumentActiveUser_CollaborationDocumentInvitationId",
                schema: "Document",
                table: "CollaborationDocumentActiveUser",
                column: "CollaborationDocumentInvitationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CollaborationDocumentActiveUser_CollaborationDocumentInvitation_CollaborationDocumentInvitationId",
                schema: "Document",
                table: "CollaborationDocumentActiveUser",
                column: "CollaborationDocumentInvitationId",
                principalSchema: "Document",
                principalTable: "CollaborationDocumentInvitation",
                principalColumn: "CollaborationDocumentInvitationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
