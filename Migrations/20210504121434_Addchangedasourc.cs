using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class Addchangedasourc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlottingType",
                table: "AnalysisSource");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DataDisplay",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "DataDisplay");

            migrationBuilder.AddColumn<string>(
                name: "PlottingType",
                table: "AnalysisSource",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
