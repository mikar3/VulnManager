using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VulnManager.Migrations
{
    public partial class qwe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Servers_Ip",
                table: "Servers",
                column: "Ip",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Servers_Ip",
                table: "Servers");
        }
    }
}
