using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddedVirtualUserRolesNavPropertyToPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PersonId",
                table: "AspNetRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_PersonId",
                table: "AspNetRoles",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_Person_PersonId",
                table: "AspNetRoles",
                column: "PersonId",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_Person_PersonId",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_PersonId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "AspNetRoles");
        }
    }
}
