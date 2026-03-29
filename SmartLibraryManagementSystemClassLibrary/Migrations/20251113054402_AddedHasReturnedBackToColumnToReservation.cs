using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLibraryManagementSystemClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddedHasReturnedBackToColumnToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasReturned",
                table: "Reservation",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasReturned",
                table: "Reservation");
        }
    }
}
