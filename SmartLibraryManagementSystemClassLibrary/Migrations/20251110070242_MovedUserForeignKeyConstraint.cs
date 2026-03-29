using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLibraryManagementSystemClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class MovedUserForeignKeyConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Faculty_FacultyId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Student_StudentId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_FacultyId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_StudentId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsFaculty",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Faculty",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Student_UserId",
                table: "Student",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculty_UserId",
                table: "Faculty",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Faculty_User_UserId",
                table: "Faculty",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_User_UserId",
                table: "Student",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faculty_User_UserId",
                table: "Faculty");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_User_UserId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_UserId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Faculty_UserId",
                table: "Faculty");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Faculty");

            migrationBuilder.AddColumn<int>(
                name: "FacultyId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFaculty",
                table: "User",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_FacultyId",
                table: "User",
                column: "FacultyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_StudentId",
                table: "User",
                column: "StudentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Faculty_FacultyId",
                table: "User",
                column: "FacultyId",
                principalTable: "Faculty",
                principalColumn: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Student_StudentId",
                table: "User",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentId");
        }
    }
}
