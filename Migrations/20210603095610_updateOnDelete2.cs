using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class updateOnDelete2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Analyse_Data_DataId",
                table: "Analyse");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisPannel_DataDisplay_DataDisplayId",
                table: "AnalysisPannel");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisPannel_Pannel_PannelId",
                table: "AnalysisPannel");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisParameter_Analyse_AnalyseId",
                table: "AnalysisParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisParameter_Parameter_ParameterId",
                table: "AnalysisParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResult_DataPaths_DataPathId",
                table: "AnalysisResult");

            migrationBuilder.DropForeignKey(
                name: "FK_DataDisplay_Analyse_AnalyseId",
                table: "DataDisplay");

            migrationBuilder.DropForeignKey(
                name: "FK_DataDisplay_DataType_DataTypeId",
                table: "DataDisplay");

            migrationBuilder.DropForeignKey(
                name: "FK_DataDisplay_SamplingProfile_SamplingProfileId",
                table: "DataDisplay");

            migrationBuilder.DropForeignKey(
                name: "FK_DataDisplayData_DataDisplay_DataDisplayId",
                table: "DataDisplayData");

            migrationBuilder.DropForeignKey(
                name: "FK_DataDisplayData_Data_DataId",
                table: "DataDisplayData");

            migrationBuilder.DropForeignKey(
                name: "FK_DataPaths_Data_DataId",
                table: "DataPaths");

            migrationBuilder.AddForeignKey(
                name: "FK_Analyse_Data_DataId",
                table: "Analyse",
                column: "DataId",
                principalTable: "Data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisPannel_DataDisplay_DataDisplayId",
                table: "AnalysisPannel",
                column: "DataDisplayId",
                principalTable: "DataDisplay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisPannel_Pannel_PannelId",
                table: "AnalysisPannel",
                column: "PannelId",
                principalTable: "Pannel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisParameter_Analyse_AnalyseId",
                table: "AnalysisParameter",
                column: "AnalyseId",
                principalTable: "Analyse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisParameter_Parameter_ParameterId",
                table: "AnalysisParameter",
                column: "ParameterId",
                principalTable: "Parameter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResult_DataPaths_DataPathId",
                table: "AnalysisResult",
                column: "DataPathId",
                principalTable: "DataPaths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataDisplay_Analyse_AnalyseId",
                table: "DataDisplay",
                column: "AnalyseId",
                principalTable: "Analyse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataDisplay_DataType_DataTypeId",
                table: "DataDisplay",
                column: "DataTypeId",
                principalTable: "DataType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataDisplay_SamplingProfile_SamplingProfileId",
                table: "DataDisplay",
                column: "SamplingProfileId",
                principalTable: "SamplingProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataDisplayData_DataDisplay_DataDisplayId",
                table: "DataDisplayData",
                column: "DataDisplayId",
                principalTable: "DataDisplay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataDisplayData_Data_DataId",
                table: "DataDisplayData",
                column: "DataId",
                principalTable: "Data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataPaths_Data_DataId",
                table: "DataPaths",
                column: "DataId",
                principalTable: "Data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Analyse_Data_DataId",
                table: "Analyse");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisPannel_DataDisplay_DataDisplayId",
                table: "AnalysisPannel");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisPannel_Pannel_PannelId",
                table: "AnalysisPannel");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisParameter_Analyse_AnalyseId",
                table: "AnalysisParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisParameter_Parameter_ParameterId",
                table: "AnalysisParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResult_DataPaths_DataPathId",
                table: "AnalysisResult");

            migrationBuilder.DropForeignKey(
                name: "FK_DataDisplay_Analyse_AnalyseId",
                table: "DataDisplay");

            migrationBuilder.DropForeignKey(
                name: "FK_DataDisplay_DataType_DataTypeId",
                table: "DataDisplay");

            migrationBuilder.DropForeignKey(
                name: "FK_DataDisplay_SamplingProfile_SamplingProfileId",
                table: "DataDisplay");

            migrationBuilder.DropForeignKey(
                name: "FK_DataDisplayData_DataDisplay_DataDisplayId",
                table: "DataDisplayData");

            migrationBuilder.DropForeignKey(
                name: "FK_DataDisplayData_Data_DataId",
                table: "DataDisplayData");

            migrationBuilder.DropForeignKey(
                name: "FK_DataPaths_Data_DataId",
                table: "DataPaths");

            migrationBuilder.AddForeignKey(
                name: "FK_Analyse_Data_DataId",
                table: "Analyse",
                column: "DataId",
                principalTable: "Data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisPannel_DataDisplay_DataDisplayId",
                table: "AnalysisPannel",
                column: "DataDisplayId",
                principalTable: "DataDisplay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisPannel_Pannel_PannelId",
                table: "AnalysisPannel",
                column: "PannelId",
                principalTable: "Pannel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisParameter_Analyse_AnalyseId",
                table: "AnalysisParameter",
                column: "AnalyseId",
                principalTable: "Analyse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisParameter_Parameter_ParameterId",
                table: "AnalysisParameter",
                column: "ParameterId",
                principalTable: "Parameter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResult_DataPaths_DataPathId",
                table: "AnalysisResult",
                column: "DataPathId",
                principalTable: "DataPaths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DataDisplay_Analyse_AnalyseId",
                table: "DataDisplay",
                column: "AnalyseId",
                principalTable: "Analyse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DataDisplay_DataType_DataTypeId",
                table: "DataDisplay",
                column: "DataTypeId",
                principalTable: "DataType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DataDisplay_SamplingProfile_SamplingProfileId",
                table: "DataDisplay",
                column: "SamplingProfileId",
                principalTable: "SamplingProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DataDisplayData_DataDisplay_DataDisplayId",
                table: "DataDisplayData",
                column: "DataDisplayId",
                principalTable: "DataDisplay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DataDisplayData_Data_DataId",
                table: "DataDisplayData",
                column: "DataId",
                principalTable: "Data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DataPaths_Data_DataId",
                table: "DataPaths",
                column: "DataId",
                principalTable: "Data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
