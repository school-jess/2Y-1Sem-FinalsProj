using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLibraryManagementSystemClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class JoinedReservationTimeAndDateTogether : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationDate",
                table: "Reservation");

            migrationBuilder.RenameColumn(
                name: "ReservationTime",
                table: "Reservation",
                newName: "ReservationDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReservationDateTime",
                table: "Reservation",
                newName: "ReservationTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDate",
                table: "Reservation",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
