using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLibraryManagementSystemClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class StreamlinedFineLoanReservationTableColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmtLoanedSinceLastLoaned",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "AmtPayedSinceLastFine",
                table: "Fine");

            migrationBuilder.AddColumn<bool>(
                name: "HasReturned",
                table: "Reservation",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPayed",
                table: "Loan",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPayed",
                table: "Fine",
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

            migrationBuilder.DropColumn(
                name: "HasPayed",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "HasPayed",
                table: "Fine");

            migrationBuilder.AddColumn<int>(
                name: "AmtLoanedSinceLastLoaned",
                table: "Loan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmtPayedSinceLastFine",
                table: "Fine",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
