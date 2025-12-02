using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLibraryManagementSystemClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class RemovedSessionDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionData",
                columns: table => new
                {
                    Key = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Expiry = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionData", x => x.Key);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
