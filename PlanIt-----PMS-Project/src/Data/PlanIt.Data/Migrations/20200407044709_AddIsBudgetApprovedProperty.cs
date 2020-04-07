using Microsoft.EntityFrameworkCore.Migrations;

namespace PlanIt.Data.Migrations
{
    public partial class AddIsBudgetApprovedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Budjet",
                table: "SubProjects",
                newName: "Budget");

            migrationBuilder.RenameColumn(
                name: "Budjet",
                table: "Projects",
                newName: "Budget");

            migrationBuilder.AddColumn<bool>(
                name: "IsBudgetApproved",
                table: "Projects",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBudgetApproved",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "Budget",
                table: "SubProjects",
                newName: "Budjet");

            migrationBuilder.RenameColumn(
                name: "Budget",
                table: "Projects",
                newName: "Budjet");
        }
    }
}
