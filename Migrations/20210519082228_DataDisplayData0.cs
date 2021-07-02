using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class DataDisplayData0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AnalyseId",
                table: "AnalysisPannel",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DataDisplayData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataId = table.Column<Guid>(nullable: true),
                    DataDisplayId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataDisplayData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataDisplayData_DataDisplay_DataDisplayId",
                        column: x => x.DataDisplayId,
                        principalTable: "DataDisplay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataDisplayData_Data_DataId",
                        column: x => x.DataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisPannel_AnalyseId",
                table: "AnalysisPannel",
                column: "AnalyseId");

            migrationBuilder.CreateIndex(
                name: "IX_DataDisplayData_DataDisplayId",
                table: "DataDisplayData",
                column: "DataDisplayId");

            migrationBuilder.CreateIndex(
                name: "IX_DataDisplayData_DataId",
                table: "DataDisplayData",
                column: "DataId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisPannel_Analyse_AnalyseId",
                table: "AnalysisPannel",
                column: "AnalyseId",
                principalTable: "Analyse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisPannel_Analyse_AnalyseId",
                table: "AnalysisPannel");

            migrationBuilder.DropTable(
                name: "DataDisplayData");

            migrationBuilder.DropIndex(
                name: "IX_AnalysisPannel_AnalyseId",
                table: "AnalysisPannel");

            migrationBuilder.DropColumn(
                name: "AnalyseId",
                table: "AnalysisPannel");
        }
    }
}
