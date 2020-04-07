using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PlanIt.Data.Migrations
{
    public partial class AddClientsProjectReqiurementsProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ClientBudget",
                table: "Projects",
                type: "decimal(15,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClientDueDate",
                table: "Projects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientBudget",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ClientDueDate",
                table: "Projects");
        }
    }
}
