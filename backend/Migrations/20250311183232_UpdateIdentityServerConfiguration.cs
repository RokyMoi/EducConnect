using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentityServerConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonAvailability_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonAvailability");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCareerInformation_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonCareerInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonDetails_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonEducationInformation_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonEducationInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonEmail_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonEmail");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonPassword_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonPassword");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonPhoneNumber_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonPhoneNumber");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonProfilePicture_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonProfilePicture");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonSalt_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonSalt");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonVerificationCode_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonVerificationCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_PersonId",
                schema: "Student",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutor_AspNetUsers_PersonId",
                schema: "Tutor",
                table: "Tutor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "Person");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "UserLoginLog");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Person",
                newName: "PersonId1");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "UserLoginLog",
                newName: "IX_UserLoginLog_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person",
                table: "Person",
                column: "PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLoginLog",
                table: "UserLoginLog",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Person_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_Person_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonAvailability_Person_PersonId",
                schema: "Person",
                table: "PersonAvailability",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCareerInformation_Person_PersonId",
                schema: "Person",
                table: "PersonCareerInformation",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonDetails_Person_PersonId",
                schema: "Person",
                table: "PersonDetails",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonEducationInformation_Person_PersonId",
                schema: "Person",
                table: "PersonEducationInformation",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonEmail_Person_PersonId",
                schema: "Person",
                table: "PersonEmail",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonPassword_Person_PersonId",
                schema: "Person",
                table: "PersonPassword",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonPhoneNumber_Person_PersonId",
                schema: "Person",
                table: "PersonPhoneNumber",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonProfilePicture_Person_PersonId",
                schema: "Person",
                table: "PersonProfilePicture",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonSalt_Person_PersonId",
                schema: "Person",
                table: "PersonSalt",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonVerificationCode_Person_PersonId",
                schema: "Person",
                table: "PersonVerificationCode",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Person_PersonId",
                schema: "Student",
                table: "Student",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tutor_Person_PersonId",
                schema: "Tutor",
                table: "Tutor",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginLog_Person_UserId",
                table: "UserLoginLog",
                column: "UserId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_AspNetRoles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Person_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Person_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_Person_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonAvailability_Person_PersonId",
                schema: "Person",
                table: "PersonAvailability");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCareerInformation_Person_PersonId",
                schema: "Person",
                table: "PersonCareerInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonDetails_Person_PersonId",
                schema: "Person",
                table: "PersonDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonEducationInformation_Person_PersonId",
                schema: "Person",
                table: "PersonEducationInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonEmail_Person_PersonId",
                schema: "Person",
                table: "PersonEmail");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonPassword_Person_PersonId",
                schema: "Person",
                table: "PersonPassword");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonPhoneNumber_Person_PersonId",
                schema: "Person",
                table: "PersonPhoneNumber");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonProfilePicture_Person_PersonId",
                schema: "Person",
                table: "PersonProfilePicture");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonSalt_Person_PersonId",
                schema: "Person",
                table: "PersonSalt");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonVerificationCode_Person_PersonId",
                schema: "Person",
                table: "PersonVerificationCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Person_PersonId",
                schema: "Student",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutor_Person_PersonId",
                schema: "Tutor",
                table: "Tutor");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLoginLog_Person_UserId",
                table: "UserLoginLog");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_AspNetRoles_RoleId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Person_UserId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLoginLog",
                table: "UserLoginLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "UserLoginLog",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "AspNetUsers");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLoginLog_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameColumn(
                name: "PersonId1",
                table: "AspNetUsers",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonAvailability_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonAvailability",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCareerInformation_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonCareerInformation",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonDetails_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonDetails",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonEducationInformation_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonEducationInformation",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonEmail_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonEmail",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonPassword_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonPassword",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonPhoneNumber_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonPhoneNumber",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonProfilePicture_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonProfilePicture",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonSalt_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonSalt",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonVerificationCode_AspNetUsers_PersonId",
                schema: "Person",
                table: "PersonVerificationCode",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_PersonId",
                schema: "Student",
                table: "Student",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tutor_AspNetUsers_PersonId",
                schema: "Tutor",
                table: "Tutor",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
