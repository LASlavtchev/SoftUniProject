using Microsoft.EntityFrameworkCore.Migrations;

namespace PlanIt.Data.Migrations
{
    public partial class ChangeHour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hours_AspNetUsers_ContractorId",
                table: "Hours");

            migrationBuilder.DropForeignKey(
                name: "FK_Hours_SubProjects_SubProjectId",
                table: "Hours");

            migrationBuilder.DropIndex(
                name: "IX_Hours_ContractorId",
                table: "Hours");

            migrationBuilder.DropIndex(
                name: "IX_Hours_SubProjectId",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "ContractorId",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "PlantItUserId",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "SubProjectId",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "WorkedTime",
                table: "Hours");

            migrationBuilder.AlterColumn<int>(
                name: "ProblemId",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Hours",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hours_UserId",
                table: "Hours",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hours_AspNetUsers_UserId",
                table: "Hours",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hours_AspNetUsers_UserId",
                table: "Hours");

            migrationBuilder.DropIndex(
                name: "IX_Hours_UserId",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Hours");

            migrationBuilder.AlterColumn<int>(
                name: "ProblemId",
                table: "Hours",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "ContractorId",
                table: "Hours",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Hours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PlantItUserId",
                table: "Hours",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubProjectId",
                table: "Hours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "WorkedTime",
                table: "Hours",
                type: "decimal(15,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Hours_ContractorId",
                table: "Hours",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Hours_SubProjectId",
                table: "Hours",
                column: "SubProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hours_AspNetUsers_ContractorId",
                table: "Hours",
                column: "ContractorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hours_SubProjects_SubProjectId",
                table: "Hours",
                column: "SubProjectId",
                principalTable: "SubProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
