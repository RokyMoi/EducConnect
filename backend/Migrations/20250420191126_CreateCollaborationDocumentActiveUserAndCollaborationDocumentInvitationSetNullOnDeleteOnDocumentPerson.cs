using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class CreateCollaborationDocumentActiveUserAndCollaborationDocumentInvitationSetNullOnDeleteOnDocumentPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Person_CreatedByPersonId",
                schema: "Document",
                table: "Document");

            migrationBuilder.CreateTable(
                name: "CollaborationDocumentInvitation",
                schema: "Document",
                columns: table => new
                {
                    CollaborationDocumentInvitationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvitedPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvitedByPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    StatusChangedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborationDocumentInvitation", x => x.CollaborationDocumentInvitationId);
                    table.ForeignKey(
                        name: "FK_CollaborationDocumentInvitation_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "Document",
                        principalTable: "Document",
                        principalColumn: "DocumentId");
                    table.ForeignKey(
                        name: "FK_CollaborationDocumentInvitation_Person_InvitedByPersonId",
                        column: x => x.InvitedByPersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK_CollaborationDocumentInvitation_Person_InvitedPersonId",
                        column: x => x.InvitedPersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "CollaborationDocumentActiveUser",
                schema: "Document",
                columns: table => new
                {
                    CollaborationDocumentActiveUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActiveUserPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    StatusChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CollaborationDocumentInvitationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborationDocumentActiveUser", x => x.CollaborationDocumentActiveUserId);
                    table.ForeignKey(
                        name: "FK_CollaborationDocumentActiveUser_CollaborationDocumentInvitation_CollaborationDocumentInvitationId",
                        column: x => x.CollaborationDocumentInvitationId,
                        principalSchema: "Document",
                        principalTable: "CollaborationDocumentInvitation",
                        principalColumn: "CollaborationDocumentInvitationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaborationDocumentActiveUser_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "Document",
                        principalTable: "Document",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaborationDocumentActiveUser_Person_ActiveUserPersonId",
                        column: x => x.ActiveUserPersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationDocumentActiveUser_ActiveUserPersonId",
                schema: "Document",
                table: "CollaborationDocumentActiveUser",
                column: "ActiveUserPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationDocumentActiveUser_CollaborationDocumentInvitationId",
                schema: "Document",
                table: "CollaborationDocumentActiveUser",
                column: "CollaborationDocumentInvitationId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationDocumentActiveUser_DocumentId",
                schema: "Document",
                table: "CollaborationDocumentActiveUser",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationDocumentInvitation_DocumentId",
                schema: "Document",
                table: "CollaborationDocumentInvitation",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationDocumentInvitation_InvitedByPersonId",
                schema: "Document",
                table: "CollaborationDocumentInvitation",
                column: "InvitedByPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationDocumentInvitation_InvitedPersonId",
                schema: "Document",
                table: "CollaborationDocumentInvitation",
                column: "InvitedPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Person_CreatedByPersonId",
                schema: "Document",
                table: "Document",
                column: "CreatedByPersonId",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Person_CreatedByPersonId",
                schema: "Document",
                table: "Document");

            migrationBuilder.DropTable(
                name: "CollaborationDocumentActiveUser",
                schema: "Document");

            migrationBuilder.DropTable(
                name: "CollaborationDocumentInvitation",
                schema: "Document");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Person_CreatedByPersonId",
                schema: "Document",
                table: "Document",
                column: "CreatedByPersonId",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
