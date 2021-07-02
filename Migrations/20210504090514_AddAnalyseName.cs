using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class AddAnalyseName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Analyse",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Analyse",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Analyse");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Analyse");
        }
    }
}
