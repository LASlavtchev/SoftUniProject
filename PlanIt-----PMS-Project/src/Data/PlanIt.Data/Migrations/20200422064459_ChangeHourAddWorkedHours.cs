using Microsoft.EntityFrameworkCore.Migrations;

namespace PlanIt.Data.Migrations
{
    public partial class ChangeHourAddWorkedHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "WorkedHours",
                table: "Hours",
                type: "decimal(15,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkedHours",
                table: "Hours");
        }
    }
}
