using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PlanIt.Data.Migrations.InvitesDb
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invites",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    SecurityValue = table.Column<string>(nullable: false),
                    ExpiredOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invites", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invites_Email_SecurityValue",
                table: "Invites",
                columns: new[] { "Email", "SecurityValue" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invites");
        }
    }
}
