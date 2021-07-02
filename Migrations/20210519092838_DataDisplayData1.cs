using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class DataDisplayData1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataDisplay_Data_DataId",
                table: "DataDisplay");

            migrationBuilder.DropIndex(
                name: "IX_DataDisplay_DataId",
                table: "DataDisplay");

            migrationBuilder.DropColumn(
                name: "DataId",
                table: "DataDisplay");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DataId",
                table: "DataDisplay",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataDisplay_DataId",
                table: "DataDisplay",
                column: "DataId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataDisplay_Data_DataId",
                table: "DataDisplay",
                column: "DataId",
                principalTable: "Data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
