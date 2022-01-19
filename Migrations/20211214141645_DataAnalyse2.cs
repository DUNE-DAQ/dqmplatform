using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class DataAnalyse2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<Guid>(
                name: "dataDisplayId",
                table: "DataAnalyse",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataAnalyse_dataDisplayId",
                table: "DataAnalyse",
                column: "dataDisplayId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataAnalyse_Data_DataId",
                table: "DataAnalyse",
                column: "DataId",
                principalTable: "Data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DataAnalyse_DataDisplay_dataDisplayId",
                table: "DataAnalyse",
                column: "dataDisplayId",
                principalTable: "DataDisplay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataAnalyse_Data_DataId",
                table: "DataAnalyse");

            migrationBuilder.DropForeignKey(
                name: "FK_DataAnalyse_DataDisplay_dataDisplayId",
                table: "DataAnalyse");

            migrationBuilder.DropIndex(
                name: "IX_DataAnalyse_dataDisplayId",
                table: "DataAnalyse");


            migrationBuilder.DropColumn(
                name: "dataDisplayId",
                table: "DataAnalyse");


            migrationBuilder.AddForeignKey(
                name: "FK_DataAnalyse_DataDisplay_DataId",
                table: "DataAnalyse",
                column: "DataId",
                principalTable: "DataDisplay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
