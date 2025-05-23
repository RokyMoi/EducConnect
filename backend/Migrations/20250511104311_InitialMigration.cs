﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Person");

            migrationBuilder.EnsureSchema(
                name: "Document");

            migrationBuilder.EnsureSchema(
                name: "Reference");

            migrationBuilder.EnsureSchema(
                name: "Course");

            migrationBuilder.EnsureSchema(
                name: "Student");

            migrationBuilder.EnsureSchema(
                name: "Tutor");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationType",
                schema: "Reference",
                columns: table => new
                {
                    CommunicationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationType", x => x.CommunicationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "Reference",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalCallingCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISOAlpha2Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlagEmoji = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "CourseCategory",
                schema: "Reference",
                columns: table => new
                {
                    CourseCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCategory", x => x.CourseCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "CourseType",
                schema: "Reference",
                columns: table => new
                {
                    CourseTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseType", x => x.CourseTypeId);
                });

            migrationBuilder.CreateTable(
                name: "EmploymentType",
                schema: "Reference",
                columns: table => new
                {
                    EmploymentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentType", x => x.EmploymentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "EngagementMethod",
                schema: "Reference",
                columns: table => new
                {
                    EngagementMethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngagementMethod", x => x.EngagementMethodId);
                });

            migrationBuilder.CreateTable(
                name: "IndustryClassification",
                schema: "Reference",
                columns: table => new
                {
                    IndustryClassificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryClassification", x => x.IndustryClassificationId);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                schema: "Reference",
                columns: table => new
                {
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRightToLeft = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.LanguageId);
                });

            migrationBuilder.CreateTable(
                name: "LearningCategory",
                schema: "Reference",
                columns: table => new
                {
                    LearningCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LearningCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningCategoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningCategory", x => x.LearningCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "LearningDifficultyLevel",
                schema: "Reference",
                columns: table => new
                {
                    LearningDifficultyLevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningDifficultyLevel", x => x.LearningDifficultyLevelId);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "Person",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PersonPublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "TutorRegistrationStatus",
                schema: "Reference",
                columns: table => new
                {
                    TutorRegistrationStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSkippable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorRegistrationStatus", x => x.TutorRegistrationStatusId);
                });

            migrationBuilder.CreateTable(
                name: "TutorTeachingStyleType",
                schema: "Reference",
                columns: table => new
                {
                    TutorTeachingStyleTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorTeachingStyleType", x => x.TutorTeachingStyleTypeId);
                });

            migrationBuilder.CreateTable(
                name: "WorkType",
                schema: "Reference",
                columns: table => new
                {
                    WorkTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkType", x => x.WorkTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearningSubcategory",
                schema: "Reference",
                columns: table => new
                {
                    LearningSubcategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LearningCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LearningSubcategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningSubcategory", x => x.LearningSubcategoryId);
                    table.ForeignKey(
                        name: "FK_LearningSubcategory_LearningCategory_LearningCategoryId",
                        column: x => x.LearningCategoryId,
                        principalSchema: "Reference",
                        principalTable: "LearningCategory",
                        principalColumn: "LearningCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Person_UserId",
                        column: x => x.UserId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Person_UserId",
                        column: x => x.UserId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthenticationToken",
                schema: "Person",
                columns: table => new
                {
                    AuthenticationTokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationToken", x => x.AuthenticationTokenId);
                    table.ForeignKey(
                        name: "FK_AuthenticationToken_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                schema: "Document",
                columns: table => new
                {
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Document_Person_CreatedByPersonId",
                        column: x => x.CreatedByPersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "Folder",
                schema: "Course",
                columns: table => new
                {
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentFolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OwnerPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.FolderId);
                    table.ForeignKey(
                        name: "FK_Folder_Folder_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalSchema: "Course",
                        principalTable: "Folder",
                        principalColumn: "FolderId");
                    table.ForeignKey(
                        name: "FK_Folder_Person_OwnerPersonId",
                        column: x => x.OwnerPersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateRead = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MessageSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RecipientDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Person_RecipientId",
                        column: x => x.RecipientId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_Person_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonAvailability",
                schema: "Person",
                columns: table => new
                {
                    PersonAvailabilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonAvailability", x => x.PersonAvailabilityId);
                    table.ForeignKey(
                        name: "FK_PersonAvailability_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonDetails",
                schema: "Person",
                columns: table => new
                {
                    PersonDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryOfOriginCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonDetails", x => x.PersonDetailsId);
                    table.ForeignKey(
                        name: "FK_PersonDetails_Country_CountryOfOriginCountryId",
                        column: x => x.CountryOfOriginCountryId,
                        principalSchema: "Reference",
                        principalTable: "Country",
                        principalColumn: "CountryId");
                    table.ForeignKey(
                        name: "FK_PersonDetails_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonEducationInformation",
                schema: "Person",
                columns: table => new
                {
                    PersonEducationInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstitutionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstitutionOfficialWebsite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstitutionAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinorFieldOfStudy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    FinalGrade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonEducationInformation", x => x.PersonEducationInformationId);
                    table.ForeignKey(
                        name: "FK_PersonEducationInformation_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonEmail",
                schema: "Person",
                columns: table => new
                {
                    PersonEmailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonEmail", x => x.PersonEmailId);
                    table.ForeignKey(
                        name: "FK_PersonEmail_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonPassword",
                schema: "Person",
                columns: table => new
                {
                    PersonPasswordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonPassword", x => x.PersonPasswordId);
                    table.ForeignKey(
                        name: "FK_PersonPassword_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonPhoneNumber",
                schema: "Person",
                columns: table => new
                {
                    PersonPhoneNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationalCallingCodeCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonPhoneNumber", x => x.PersonPhoneNumberId);
                    table.ForeignKey(
                        name: "FK_PersonPhoneNumber_Country_NationalCallingCodeCountryId",
                        column: x => x.NationalCallingCodeCountryId,
                        principalSchema: "Reference",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonPhoneNumber_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonPhoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonPhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonPhoto_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonProfilePicture",
                schema: "Person",
                columns: table => new
                {
                    PersonProfilePictureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonProfilePicture", x => x.PersonProfilePictureId);
                    table.ForeignKey(
                        name: "FK_PersonProfilePicture_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonSalt",
                schema: "Person",
                columns: table => new
                {
                    PersonSaltId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfRounds = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonSalt", x => x.PersonSaltId);
                    table.ForeignKey(
                        name: "FK_PersonSalt_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonVerificationCode",
                schema: "Person",
                columns: table => new
                {
                    PersonVerificationCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDateTime = table.Column<long>(type: "bigint", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonVerificationCode", x => x.PersonVerificationCodeId);
                    table.ForeignKey(
                        name: "FK_PersonVerificationCode_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                schema: "Student",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Student_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "Course",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_Tag_Person_CreatedByPersonId",
                        column: x => x.CreatedByPersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginLog",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginLog", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLoginLog_Person_UserId",
                        column: x => x.UserId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK_UserRoles_Person_UserId",
                        column: x => x.UserId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tutor",
                schema: "Tutor",
                columns: table => new
                {
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TutorRegistrationStatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutor", x => x.TutorId);
                    table.ForeignKey(
                        name: "FK_Tutor_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tutor_TutorRegistrationStatus_TutorRegistrationStatusId",
                        column: x => x.TutorRegistrationStatusId,
                        principalSchema: "Reference",
                        principalTable: "TutorRegistrationStatus",
                        principalColumn: "TutorRegistrationStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonCareerInformation",
                schema: "Person",
                columns: table => new
                {
                    PersonCareerInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyWebsite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityOfEmployment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryOfEmployment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmploymentTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Responsibilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Achievements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryClassificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillsUsed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkTypeId = table.Column<int>(type: "int", nullable: true),
                    AdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonCareerInformation", x => x.PersonCareerInformationId);
                    table.ForeignKey(
                        name: "FK_PersonCareerInformation_EmploymentType_EmploymentTypeId",
                        column: x => x.EmploymentTypeId,
                        principalSchema: "Reference",
                        principalTable: "EmploymentType",
                        principalColumn: "EmploymentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonCareerInformation_IndustryClassification_IndustryClassificationId",
                        column: x => x.IndustryClassificationId,
                        principalSchema: "Reference",
                        principalTable: "IndustryClassification",
                        principalColumn: "IndustryClassificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonCareerInformation_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonCareerInformation_WorkType_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalSchema: "Reference",
                        principalTable: "WorkType",
                        principalColumn: "WorkTypeId");
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
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborationDocumentActiveUser", x => x.CollaborationDocumentActiveUserId);
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
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true),
                    DocumentId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                        name: "FK_CollaborationDocumentInvitation_Document_DocumentId1",
                        column: x => x.DocumentId1,
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
                name: "ShoppingCart",
                columns: table => new
                {
                    ShoppingCartID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCart", x => x.ShoppingCartID);
                    table.ForeignKey(
                        name: "FK_ShoppingCart_Student_StudentID",
                        column: x => x.StudentID,
                        principalSchema: "Student",
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentDetails",
                schema: "Student",
                columns: table => new
                {
                    StudentDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentAcademicInstitution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentEducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainAreaOfSpecialisation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDetails", x => x.StudentDetailsId);
                    table.ForeignKey(
                        name: "FK_StudentDetails_Student_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "Student",
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wishlist",
                columns: table => new
                {
                    WishlistID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlist", x => x.WishlistID);
                    table.ForeignKey(
                        name: "FK_Wishlist_Student_StudentID",
                        column: x => x.StudentID,
                        principalSchema: "Student",
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                schema: "Course",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LearningDifficultyLevelId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinNumberOfStudents = table.Column<int>(type: "int", nullable: true),
                    MaxNumberOfStudents = table.Column<int>(type: "int", nullable: true),
                    PublishedStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Course_CourseCategory_CourseCategoryId",
                        column: x => x.CourseCategoryId,
                        principalSchema: "Reference",
                        principalTable: "CourseCategory",
                        principalColumn: "CourseCategoryId");
                    table.ForeignKey(
                        name: "FK_Course_LearningDifficultyLevel_LearningDifficultyLevelId",
                        column: x => x.LearningDifficultyLevelId,
                        principalSchema: "Reference",
                        principalTable: "LearningDifficultyLevel",
                        principalColumn: "LearningDifficultyLevelId");
                    table.ForeignKey(
                        name: "FK_Course_Tutor_TutorId",
                        column: x => x.TutorId,
                        principalSchema: "Tutor",
                        principalTable: "Tutor",
                        principalColumn: "TutorId");
                });

            migrationBuilder.CreateTable(
                name: "TutorTeachingInformation",
                schema: "Tutor",
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
                    ExpectedResponseTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialConsiderations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "CourseDetails",
                schema: "Course",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    LearningSubcategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LearningDifficultyLevelId = table.Column<int>(type: "int", nullable: false),
                    CourseTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDetails", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_CourseDetails_CourseType_CourseTypeId",
                        column: x => x.CourseTypeId,
                        principalSchema: "Reference",
                        principalTable: "CourseType",
                        principalColumn: "CourseTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseDetails_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseDetails_LearningDifficultyLevel_LearningDifficultyLevelId",
                        column: x => x.LearningDifficultyLevelId,
                        principalSchema: "Reference",
                        principalTable: "LearningDifficultyLevel",
                        principalColumn: "LearningDifficultyLevelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseDetails_LearningSubcategory_LearningSubcategoryId",
                        column: x => x.LearningSubcategoryId,
                        principalSchema: "Reference",
                        principalTable: "LearningSubcategory",
                        principalColumn: "LearningSubcategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseEnrollment",
                schema: "Course",
                columns: table => new
                {
                    CourseEnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEnrollment", x => x.CourseEnrollmentId);
                    table.ForeignKey(
                        name: "FK_CourseEnrollment_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrollment_Student_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "Student",
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseLanguage",
                schema: "Course",
                columns: table => new
                {
                    CourseLanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLanguage", x => x.CourseLanguageId);
                    table.ForeignKey(
                        name: "FK_CourseLanguage_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseLanguage_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Reference",
                        principalTable: "Language",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseLesson",
                schema: "Course",
                columns: table => new
                {
                    CourseLessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonSequenceOrder = table.Column<int>(type: "int", nullable: true),
                    PublishedStatus = table.Column<int>(type: "int", nullable: false),
                    StatusChangedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLesson", x => x.CourseLessonId);
                    table.ForeignKey(
                        name: "FK_CourseLesson_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseLesson_Tutor_TutorId",
                        column: x => x.TutorId,
                        principalSchema: "Tutor",
                        principalTable: "Tutor",
                        principalColumn: "TutorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoursePromotion",
                columns: table => new
                {
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursePromotion", x => x.PromotionId);
                    table.ForeignKey(
                        name: "FK_CoursePromotion_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoursePromotionImage",
                schema: "Course",
                columns: table => new
                {
                    CoursePromotionImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursePromotionImage", x => x.CoursePromotionImageId);
                    table.ForeignKey(
                        name: "FK_CoursePromotionImage_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoursePromotionImage_Folder_FolderId",
                        column: x => x.FolderId,
                        principalSchema: "Course",
                        principalTable: "Folder",
                        principalColumn: "FolderId");
                });

            migrationBuilder.CreateTable(
                name: "CourseTag",
                schema: "Course",
                columns: table => new
                {
                    CourseTagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true),
                    TagId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTag", x => x.CourseTagId);
                    table.ForeignKey(
                        name: "FK_CourseTag_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_CourseTag_Tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "Course",
                        principalTable: "Tag",
                        principalColumn: "TagId");
                    table.ForeignKey(
                        name: "FK_CourseTag_Tag_TagId1",
                        column: x => x.TagId1,
                        principalSchema: "Course",
                        principalTable: "Tag",
                        principalColumn: "TagId");
                });

            migrationBuilder.CreateTable(
                name: "CourseTeachingResource",
                schema: "Course",
                columns: table => new
                {
                    CourseTeachingResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ResourceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTeachingResource", x => x.CourseTeachingResourceId);
                    table.ForeignKey(
                        name: "FK_CourseTeachingResource_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTeachingResource_Folder_FolderId",
                        column: x => x.FolderId,
                        principalSchema: "Course",
                        principalTable: "Folder",
                        principalColumn: "FolderId");
                });

            migrationBuilder.CreateTable(
                name: "CourseThumbnail",
                schema: "Course",
                columns: table => new
                {
                    CourseThumbnailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailImageFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseThumbnail", x => x.CourseThumbnailId);
                    table.ForeignKey(
                        name: "FK_CourseThumbnail_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseThumbnail_Folder_FolderId",
                        column: x => x.FolderId,
                        principalSchema: "Course",
                        principalTable: "Folder",
                        principalColumn: "FolderId");
                });

            migrationBuilder.CreateTable(
                name: "CourseViewershipData",
                schema: "Course",
                columns: table => new
                {
                    CourseViewershipDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViewedByPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClickedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnteredDetailsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeftDetailsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserCameFrom = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseViewershipData", x => x.CourseViewershipDataId);
                    table.ForeignKey(
                        name: "FK_CourseViewershipData_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseViewershipData_Person_ViewedByPersonId",
                        column: x => x.ViewedByPersonId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseViewershipDataSnapshot",
                schema: "Course",
                columns: table => new
                {
                    CourseViewershipDataSnapshotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalViews = table.Column<int>(type: "int", nullable: false),
                    NumberOfUniqueVisitors = table.Column<int>(type: "int", nullable: false),
                    CurrentlyViewing = table.Column<int>(type: "int", nullable: false),
                    AverageViewDurationInMinutes = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseViewershipDataSnapshot", x => x.CourseViewershipDataSnapshotId);
                    table.ForeignKey(
                        name: "FK_CourseViewershipDataSnapshot_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItem",
                columns: table => new
                {
                    ShoppingCartItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShoppingCartID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItem", x => x.ShoppingCartItemID);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItem_Course_CourseID",
                        column: x => x.CourseID,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItem_ShoppingCart_ShoppingCartID",
                        column: x => x.ShoppingCartID,
                        principalTable: "ShoppingCart",
                        principalColumn: "ShoppingCartID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentEnrollment",
                columns: table => new
                {
                    StudentEnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentEnrollment", x => x.StudentEnrollmentId);
                    table.ForeignKey(
                        name: "FK_StudentEnrollment_Course_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentEnrollment_Student_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "Student",
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WishlistItems",
                columns: table => new
                {
                    WishtListItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WishListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistItems", x => x.WishtListItemId);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Course_CourseID",
                        column: x => x.CourseID,
                        principalSchema: "Course",
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Wishlist_WishListId",
                        column: x => x.WishListId,
                        principalTable: "Wishlist",
                        principalColumn: "WishlistID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseLessonContent",
                schema: "Course",
                columns: table => new
                {
                    CourseLessonContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseLessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true),
                    RowGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLessonContent", x => x.CourseLessonContentId);
                    table.ForeignKey(
                        name: "FK_CourseLessonContent_CourseLesson_CourseLessonId",
                        column: x => x.CourseLessonId,
                        principalSchema: "Course",
                        principalTable: "CourseLesson",
                        principalColumn: "CourseLessonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseLessonResource",
                schema: "Course",
                columns: table => new
                {
                    CourseLessonResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseLessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ResourceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLessonResource", x => x.CourseLessonResourceId);
                    table.ForeignKey(
                        name: "FK_CourseLessonResource_CourseLesson_CourseLessonId",
                        column: x => x.CourseLessonId,
                        principalSchema: "Course",
                        principalTable: "CourseLesson",
                        principalColumn: "CourseLessonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseLessonResource_Folder_FolderId",
                        column: x => x.FolderId,
                        principalSchema: "Course",
                        principalTable: "Folder",
                        principalColumn: "FolderId");
                });

            migrationBuilder.CreateTable(
                name: "PromotionDuration",
                columns: table => new
                {
                    DurationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<long>(type: "bigint", nullable: false),
                    EndDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionDuration", x => x.DurationId);
                    table.ForeignKey(
                        name: "FK_PromotionDuration_CoursePromotion_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "CoursePromotion",
                        principalColumn: "PromotionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionImages",
                columns: table => new
                {
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsMainImage = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionImages", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_PromotionImages_CoursePromotion_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "CoursePromotion",
                        principalColumn: "PromotionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationToken_PersonId",
                schema: "Person",
                table: "AuthenticationToken",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborationDocumentActiveUser_ActiveUserPersonId",
                schema: "Document",
                table: "CollaborationDocumentActiveUser",
                column: "ActiveUserPersonId");

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
                name: "IX_CollaborationDocumentInvitation_DocumentId1",
                schema: "Document",
                table: "CollaborationDocumentInvitation",
                column: "DocumentId1");

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

            migrationBuilder.CreateIndex(
                name: "IX_Course_CourseCategoryId",
                schema: "Course",
                table: "Course",
                column: "CourseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_LearningDifficultyLevelId",
                schema: "Course",
                table: "Course",
                column: "LearningDifficultyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_TutorId",
                schema: "Course",
                table: "Course",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDetails_CourseTypeId",
                schema: "Course",
                table: "CourseDetails",
                column: "CourseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDetails_LearningDifficultyLevelId",
                schema: "Course",
                table: "CourseDetails",
                column: "LearningDifficultyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDetails_LearningSubcategoryId",
                schema: "Course",
                table: "CourseDetails",
                column: "LearningSubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollment_CourseId",
                schema: "Course",
                table: "CourseEnrollment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollment_StudentId",
                schema: "Course",
                table: "CourseEnrollment",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLanguage_CourseId",
                schema: "Course",
                table: "CourseLanguage",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLanguage_LanguageId",
                schema: "Course",
                table: "CourseLanguage",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLesson_CourseId",
                schema: "Course",
                table: "CourseLesson",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLesson_TutorId",
                schema: "Course",
                table: "CourseLesson",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonContent_CourseLessonId",
                schema: "Course",
                table: "CourseLessonContent",
                column: "CourseLessonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonContent_RowGuid",
                schema: "Course",
                table: "CourseLessonContent",
                column: "RowGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonResource_CourseLessonId",
                schema: "Course",
                table: "CourseLessonResource",
                column: "CourseLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonResource_FolderId",
                schema: "Course",
                table: "CourseLessonResource",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePromotion_CourseId",
                table: "CoursePromotion",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePromotionImage_CourseId",
                schema: "Course",
                table: "CoursePromotionImage",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePromotionImage_FolderId",
                schema: "Course",
                table: "CoursePromotionImage",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTag_CourseId",
                schema: "Course",
                table: "CourseTag",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTag_TagId",
                schema: "Course",
                table: "CourseTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTag_TagId1",
                schema: "Course",
                table: "CourseTag",
                column: "TagId1");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTeachingResource_CourseId",
                schema: "Course",
                table: "CourseTeachingResource",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTeachingResource_FolderId",
                schema: "Course",
                table: "CourseTeachingResource",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseThumbnail_CourseId",
                schema: "Course",
                table: "CourseThumbnail",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseThumbnail_FolderId",
                schema: "Course",
                table: "CourseThumbnail",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseViewershipData_CourseId",
                schema: "Course",
                table: "CourseViewershipData",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseViewershipData_ViewedByPersonId",
                schema: "Course",
                table: "CourseViewershipData",
                column: "ViewedByPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseViewershipDataSnapshot_CourseId",
                schema: "Course",
                table: "CourseViewershipDataSnapshot",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_CreatedByPersonId",
                schema: "Document",
                table: "Document",
                column: "CreatedByPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_OwnerPersonId",
                schema: "Course",
                table: "Folder",
                column: "OwnerPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_ParentFolderId",
                schema: "Course",
                table: "Folder",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningSubcategory_LearningCategoryId",
                schema: "Reference",
                table: "LearningSubcategory",
                column: "LearningCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_RecipientId",
                table: "Message",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Person",
                table: "Person",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Person",
                table: "Person",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PersonAvailability_PersonId",
                schema: "Person",
                table: "PersonAvailability",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCareerInformation_EmploymentTypeId",
                schema: "Person",
                table: "PersonCareerInformation",
                column: "EmploymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCareerInformation_IndustryClassificationId",
                schema: "Person",
                table: "PersonCareerInformation",
                column: "IndustryClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCareerInformation_PersonId",
                schema: "Person",
                table: "PersonCareerInformation",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCareerInformation_WorkTypeId",
                schema: "Person",
                table: "PersonCareerInformation",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonDetails_CountryOfOriginCountryId",
                schema: "Person",
                table: "PersonDetails",
                column: "CountryOfOriginCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonDetails_PersonId",
                schema: "Person",
                table: "PersonDetails",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonEducationInformation_PersonId",
                schema: "Person",
                table: "PersonEducationInformation",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonEmail_PersonId",
                schema: "Person",
                table: "PersonEmail",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonPassword_PersonId",
                schema: "Person",
                table: "PersonPassword",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonPhoneNumber_NationalCallingCodeCountryId",
                schema: "Person",
                table: "PersonPhoneNumber",
                column: "NationalCallingCodeCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonPhoneNumber_PersonId",
                schema: "Person",
                table: "PersonPhoneNumber",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonPhoto_PersonId",
                table: "PersonPhoto",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonProfilePicture_PersonId",
                schema: "Person",
                table: "PersonProfilePicture",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSalt_PersonId",
                schema: "Person",
                table: "PersonSalt",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonVerificationCode_PersonId",
                schema: "Person",
                table: "PersonVerificationCode",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionDuration_PromotionId",
                table: "PromotionDuration",
                column: "PromotionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromotionImages_PromotionId",
                table: "PromotionImages",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_StudentID",
                table: "ShoppingCart",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItem_CourseID",
                table: "ShoppingCartItem",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItem_ShoppingCartID",
                table: "ShoppingCartItem",
                column: "ShoppingCartID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_PersonId",
                schema: "Student",
                table: "Student",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDetails_StudentId",
                schema: "Student",
                table: "StudentDetails",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollment_CourseId",
                table: "StudentEnrollment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollment_StudentId",
                table: "StudentEnrollment",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_CreatedByPersonId",
                schema: "Course",
                table: "Tag",
                column: "CreatedByPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Tutor_PersonId",
                schema: "Tutor",
                table: "Tutor",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Tutor_TutorRegistrationStatusId",
                schema: "Tutor",
                table: "Tutor",
                column: "TutorRegistrationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_PrimaryCommunicationTypeId",
                schema: "Tutor",
                table: "TutorTeachingInformation",
                column: "PrimaryCommunicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_PrimaryEngagementMethodId",
                schema: "Tutor",
                table: "TutorTeachingInformation",
                column: "PrimaryEngagementMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_SecondaryCommunicationTypeId",
                schema: "Tutor",
                table: "TutorTeachingInformation",
                column: "SecondaryCommunicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_SecondaryEngagementMethodId",
                schema: "Tutor",
                table: "TutorTeachingInformation",
                column: "SecondaryEngagementMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_TeachingStyleTypeId",
                schema: "Tutor",
                table: "TutorTeachingInformation",
                column: "TeachingStyleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachingInformation_TutorId",
                schema: "Tutor",
                table: "TutorTeachingInformation",
                column: "TutorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLog_UserId",
                table: "UserLoginLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_PersonId",
                table: "UserRoles",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_StudentID",
                table: "Wishlist",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_CourseID",
                table: "WishlistItems",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_WishListId",
                table: "WishlistItems",
                column: "WishListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuthenticationToken",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "CollaborationDocumentActiveUser",
                schema: "Document");

            migrationBuilder.DropTable(
                name: "CollaborationDocumentParticipant",
                schema: "Document");

            migrationBuilder.DropTable(
                name: "CourseDetails",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CourseEnrollment",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CourseLanguage",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CourseLessonContent",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CourseLessonResource",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CoursePromotionImage",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CourseTag",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CourseTeachingResource",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CourseThumbnail",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CourseViewershipData",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "CourseViewershipDataSnapshot",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "PersonAvailability",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "PersonCareerInformation",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "PersonDetails",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "PersonEducationInformation",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "PersonEmail",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "PersonPassword",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "PersonPhoneNumber",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "PersonPhoto");

            migrationBuilder.DropTable(
                name: "PersonProfilePicture",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "PersonSalt",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "PersonVerificationCode",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "PromotionDuration");

            migrationBuilder.DropTable(
                name: "PromotionImages");

            migrationBuilder.DropTable(
                name: "ShoppingCartItem");

            migrationBuilder.DropTable(
                name: "StudentDetails",
                schema: "Student");

            migrationBuilder.DropTable(
                name: "StudentEnrollment");

            migrationBuilder.DropTable(
                name: "TutorTeachingInformation",
                schema: "Tutor");

            migrationBuilder.DropTable(
                name: "UserLoginLog");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "WishlistItems");

            migrationBuilder.DropTable(
                name: "CollaborationDocumentInvitation",
                schema: "Document");

            migrationBuilder.DropTable(
                name: "CourseType",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "LearningSubcategory",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "Language",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "CourseLesson",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "Tag",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "Folder",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "EmploymentType",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "IndustryClassification",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "WorkType",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "CoursePromotion");

            migrationBuilder.DropTable(
                name: "ShoppingCart");

            migrationBuilder.DropTable(
                name: "CommunicationType",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "EngagementMethod",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "TutorTeachingStyleType",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Wishlist");

            migrationBuilder.DropTable(
                name: "Document",
                schema: "Document");

            migrationBuilder.DropTable(
                name: "LearningCategory",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "Course",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "Student",
                schema: "Student");

            migrationBuilder.DropTable(
                name: "CourseCategory",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "LearningDifficultyLevel",
                schema: "Reference");

            migrationBuilder.DropTable(
                name: "Tutor",
                schema: "Tutor");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "TutorRegistrationStatus",
                schema: "Reference");
        }
    }
}
