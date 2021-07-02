using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class updateOnDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Analyse_AnalysisSource_AnalysisSourceId",
                table: "Analyse");

            migrationBuilder.AddForeignKey(
                name: "FK_Analyse_AnalysisSource_AnalysisSourceId",
                table: "Analyse",
                column: "AnalysisSourceId",
                principalTable: "AnalysisSource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Analyse_AnalysisSource_AnalysisSourceId",
                table: "Analyse");

            migrationBuilder.AddForeignKey(
                name: "FK_Analyse_AnalysisSource_AnalysisSourceId",
                table: "Analyse",
                column: "AnalysisSourceId",
                principalTable: "AnalysisSource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
