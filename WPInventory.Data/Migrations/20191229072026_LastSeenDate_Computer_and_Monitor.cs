using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WPInventory.Data.Migrations
{
    public partial class LastSeenDate_Computer_and_Monitor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeen",
                table: "ScanDates",
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "ScanDate",
                table: "Monitors",
                newName: "LastSeen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSeen",
                table: "ScanDates");

            migrationBuilder.RenameColumn(
                name: "LastSeen",
                table: "Monitors",
                newName: "ScanDate");
        }
    }
}
