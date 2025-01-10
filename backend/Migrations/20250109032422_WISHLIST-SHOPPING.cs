using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class WISHLISTSHOPPING : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Course");

            migrationBuilder.CreateTable(
                name: "ShoppingCart",
                columns: table => new
                {
                    ShoppingCartID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "WishList",
                columns: table => new
                {
                    WishListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishList", x => x.WishListId);
                    table.ForeignKey(
                        name: "FK_WishList_Student_StudentID",
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
                    TutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetailedDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseField = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniqueInstance = table.Column<bool>(type: "bit", nullable: false),
                    PlannedNumberOfClasses = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedAt = table.Column<long>(type: "bigint", nullable: true),
                    ShoppingCartID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WishListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Course_ShoppingCart_ShoppingCartID",
                        column: x => x.ShoppingCartID,
                        principalTable: "ShoppingCart",
                        principalColumn: "ShoppingCartID");
                    table.ForeignKey(
                        name: "FK_Course_Tutor_TutorId",
                        column: x => x.TutorId,
                        principalSchema: "Tutor",
                        principalTable: "Tutor",
                        principalColumn: "TutorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Course_WishList_WishListId",
                        column: x => x.WishListId,
                        principalTable: "WishList",
                        principalColumn: "WishListId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_ShoppingCartID",
                schema: "Course",
                table: "Course",
                column: "ShoppingCartID");

            migrationBuilder.CreateIndex(
                name: "IX_Course_TutorId",
                schema: "Course",
                table: "Course",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_WishListId",
                schema: "Course",
                table: "Course",
                column: "WishListId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_StudentID",
                table: "ShoppingCart",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_WishList_StudentID",
                table: "WishList",
                column: "StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Course",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "ShoppingCart");

            migrationBuilder.DropTable(
                name: "WishList");
        }
    }
}
