using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddedVirtualUserRolesNavListPropertyToPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "PersonId",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_PersonId",
                table: "UserRoles",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Person_PersonId",
                table: "UserRoles",
                column: "PersonId",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Person_PersonId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_PersonId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "UserRoles");

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
    }
}
