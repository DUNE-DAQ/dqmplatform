using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class updateOnDelete3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Data_DataSources_DataSourceId",
                table: "Data");

            migrationBuilder.AddForeignKey(
                name: "FK_Data_DataSources_DataSourceId",
                table: "Data",
                column: "DataSourceId",
                principalTable: "DataSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Data_DataSources_DataSourceId",
                table: "Data");

            migrationBuilder.AddForeignKey(
                name: "FK_Data_DataSources_DataSourceId",
                table: "Data",
                column: "DataSourceId",
                principalTable: "DataSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
