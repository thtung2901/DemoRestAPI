using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductLibrary.API.Migrations
{
    public partial class AddDateToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                table: "Categories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Categories");
        }
    }
}
