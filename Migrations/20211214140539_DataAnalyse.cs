using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class DataAnalyse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "DataAnalyse",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AnalyseId = table.Column<Guid>(nullable: true),
                    DataId = table.Column<Guid>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    channel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataAnalyse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataAnalyse_Analyse_AnalyseId",
                        column: x => x.AnalyseId,
                        principalTable: "Analyse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataAnalyse_DataDisplay_DataId",
                        column: x => x.DataId,
                        principalTable: "DataDisplay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataAnalyse_AnalyseId",
                table: "DataAnalyse",
                column: "AnalyseId");

            migrationBuilder.CreateIndex(
                name: "IX_DataAnalyse_DataId",
                table: "DataAnalyse",
                column: "DataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataAnalyse");
        }
    }
}
