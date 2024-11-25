using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedEnitites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TutorAvailability",
                schema: "Tutor");

            migrationBuilder.DropTable(
                name: "TutorCertification",
                schema: "Tutor");

            migrationBuilder.DropTable(
                name: "TutorDetails",
                schema: "Tutor");

            migrationBuilder.DropTable(
                name: "TutorRating",
                schema: "Tutor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TutorAvailability",
                schema: "Tutor",
                columns: table => new
                {
                    TutorAvailabilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    DayOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorAvailability", x => x.TutorAvailabilityId);
                    table.ForeignKey(
                        name: "FK_TutorAvailability_Tutor_TutorId",
                        column: x => x.TutorId,
                        principalSchema: "Tutor",
                        principalTable: "Tutor",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TutorCertification",
                schema: "Tutor",
                columns: table => new
                {
                    TutorCertificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CertificateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertificateScan = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ExpirationDate = table.Column<long>(type: "bigint", nullable: true),
                    IssueDate = table.Column<long>(type: "bigint", nullable: false),
                    IssuedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkToCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorCertification", x => x.TutorCertificationId);
                    table.ForeignKey(
                        name: "FK_TutorCertification_Tutor_TutorId",
                        column: x => x.TutorId,
                        principalSchema: "Tutor",
                        principalTable: "Tutor",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TutorDetails",
                schema: "Tutor",
                columns: table => new
                {
                    TutorDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    MainAreaOfSpecialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true),
                    ShortBiography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorDetails", x => x.TutorDetailsId);
                    table.ForeignKey(
                        name: "FK_TutorDetails_Tutor_TutorId",
                        column: x => x.TutorId,
                        principalSchema: "Tutor",
                        principalTable: "Tutor",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TutorRating",
                schema: "Tutor",
                columns: table => new
                {
                    TutorRatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true),
                    RatingScore = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorRating", x => x.TutorRatingId);
                    table.ForeignKey(
                        name: "FK_TutorRating_Tutor_TutorId",
                        column: x => x.TutorId,
                        principalSchema: "Tutor",
                        principalTable: "Tutor",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TutorAvailability_TutorId",
                schema: "Tutor",
                table: "TutorAvailability",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorCertification_TutorId",
                schema: "Tutor",
                table: "TutorCertification",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorDetails_TutorId",
                schema: "Tutor",
                table: "TutorDetails",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorRating_TutorId",
                schema: "Tutor",
                table: "TutorRating",
                column: "TutorId");
        }
    }
}
