using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class DuleInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_WishList_WishListId",
                schema: "Course",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "ShoppingCart");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "ShoppingCart");

            migrationBuilder.RenameColumn(
                name: "WishListId",
                table: "WishList",
                newName: "WishlistID");

            migrationBuilder.RenameColumn(
                name: "WishListId",
                schema: "Course",
                table: "Course",
                newName: "WishlistID");

            migrationBuilder.RenameIndex(
                name: "IX_Course_WishListId",
                schema: "Course",
                table: "Course",
                newName: "IX_Course_WishlistID");

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

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollment_CourseId",
                table: "StudentEnrollment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollment_StudentId",
                table: "StudentEnrollment",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_WishList_WishlistID",
                schema: "Course",
                table: "Course",
                column: "WishlistID",
                principalTable: "WishList",
                principalColumn: "WishlistID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_WishList_WishlistID",
                schema: "Course",
                table: "Course");

            migrationBuilder.DropTable(
                name: "StudentEnrollment");

            migrationBuilder.RenameColumn(
                name: "WishlistID",
                table: "WishList",
                newName: "WishListId");

            migrationBuilder.RenameColumn(
                name: "WishlistID",
                schema: "Course",
                table: "Course",
                newName: "WishListId");

            migrationBuilder.RenameIndex(
                name: "IX_Course_WishlistID",
                schema: "Course",
                table: "Course",
                newName: "IX_Course_WishListId");

            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "ShoppingCart",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "ShoppingCart",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_WishList_WishListId",
                schema: "Course",
                table: "Course",
                column: "WishListId",
                principalTable: "WishList",
                principalColumn: "WishListId");
        }
    }
}
