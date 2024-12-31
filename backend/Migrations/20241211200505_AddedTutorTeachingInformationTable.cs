using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddedTutorTeachingInformationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TutorTeachingInformation",
                columns: table => new
                {
                    TutorTeachingInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeachingStyleTypeId = table.Column<int>(type: "int", nullable: false),
                    PrimaryCommunicationTypeId = table.Column<int>(type: "int", nullable: false),
                    SecondaryCommunicationTypeId = table.Column<int>(type: "int", nullable: true),
                    PrimaryEngagementMethodId = table.Column<int>(type: "int", nullable: false),
                    SecondaryEngagementMethodId = table.Column<int>(type: "int", nullable: true),
                    ExpectedResponseTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialConsiderations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorTeachingInformation", x => x.TutorTeachingInformationId);
                    table.ForeignKey(
                        name: "FK_TutorTeachingInformation_CommunicationType_PrimaryCommunicationTypeId",
                        column: x => x.PrimaryCommunicationTypeId,
                        principalSchema: "Reference",
                        principalTable: "CommunicationType",
                        principalColumn: "CommunicationTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TutorTeachingInformation_CommunicationType_SecondaryCommunicationTypeId",
                        column: x => x.SecondaryCommunicationTypeId,
                        principalSchema: "Reference",
                        principalTable: "CommunicationType",
                        principalColumn: "CommunicationTypeId");
                    table.ForeignKey(
                        name: "FK_TutorTeachingInformation_EngagementMethod_PrimaryEngagementMethodId",
                        column: x => x.PrimaryEngagementMethodId,
                        principalSchema: "Reference",
                        principalTable: "EngagementMethod",
                        principalColumn: "EngagementMethodId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TutorTeachingInformation_EngagementMethod_SecondaryEngagementMethodId",
                        column: x => x.SecondaryEngagementMethodId,
                        principalSchema: "Reference",
                        principalTable: "EngagementMethod",
                        principalColumn: "EngagementMethodId");
                    table.ForeignKey(
                        name: "FK_TutorTeachingInformation_TutorTeachingStyleType_TeachingStyleTypeId",
                        column: x => x.TeachingStyleTypeId,
                        principalSchema: "Reference",
                        principalTable: "TutorTeachingStyleType",
                        principalColumn: "TutorTeachingStyleTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TutorTeachingInformation_Tutor_TutorId",
                        column: x => x.TutorId,
                        principalSchema: "Tutor",
                        principalTable: "Tutor",
                        principalColumn: "TutorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_PrimaryCommunicationTypeId",
                table: "TutorTeachingInformation",
                column: "PrimaryCommunicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_PrimaryEngagementMethodId",
                table: "TutorTeachingInformation",
                column: "PrimaryEngagementMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_SecondaryCommunicationTypeId",
                table: "TutorTeachingInformation",
                column: "SecondaryCommunicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_SecondaryEngagementMethodId",
                table: "TutorTeachingInformation",
                column: "SecondaryEngagementMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_TeachingStyleTypeId",
                table: "TutorTeachingInformation",
                column: "TeachingStyleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_TutorId",
                table: "TutorTeachingInformation",
                column: "TutorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TutorTeachingInformation");
        }
    }
}
