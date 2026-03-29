using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLibraryManagementSystemClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addedCopiesBorrowed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CopiesBorrowed",
                table: "Catalog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CopiesBorrowed",
                table: "Catalog");
        }
    }
}
