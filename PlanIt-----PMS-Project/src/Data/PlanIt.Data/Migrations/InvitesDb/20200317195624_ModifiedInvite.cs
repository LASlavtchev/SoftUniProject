using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PlanIt.Data.Migrations.InvitesDb
{
    public partial class ModifiedInvite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invites_Email_SecurityValue",
                table: "Invites");

            migrationBuilder.AlterColumn<string>(
                name: "SecurityValue",
                table: "Invites",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiredOn",
                table: "Invites",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsInvited",
                table: "Invites",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "Invites",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestExpiredOn",
                table: "Invites",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Invites_Email_SecurityValue",
                table: "Invites",
                columns: new[] { "Email", "SecurityValue" },
                unique: true,
                filter: "[SecurityValue] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invites_Email_SecurityValue",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "IsInvited",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "RequestExpiredOn",
                table: "Invites");

            migrationBuilder.AlterColumn<string>(
                name: "SecurityValue",
                table: "Invites",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiredOn",
                table: "Invites",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invites_Email_SecurityValue",
                table: "Invites",
                columns: new[] { "Email", "SecurityValue" },
                unique: true);
        }
    }
}
