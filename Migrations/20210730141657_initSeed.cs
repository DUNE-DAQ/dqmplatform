using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class initSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalysisSource",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataSources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Source = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PlottingType = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pannel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pannel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parameter",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Factor = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SamplingProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PlottingType = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    Factor = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplingProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Data",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataSourceId = table.Column<Guid>(nullable: true),
                    RententionTime = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Data_DataSources_DataSourceId",
                        column: x => x.DataSourceId,
                        principalTable: "DataSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataDisplay",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataTypeId = table.Column<Guid>(nullable: true),
                    SamplingProfileId = table.Column<Guid>(nullable: true),
                    PlotLength = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataDisplay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataDisplay_DataType_DataTypeId",
                        column: x => x.DataTypeId,
                        principalTable: "DataType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataDisplay_SamplingProfile_SamplingProfileId",
                        column: x => x.SamplingProfileId,
                        principalTable: "SamplingProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Analyse",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AnalysisSourceId = table.Column<Guid>(nullable: true),
                    DataId = table.Column<Guid>(nullable: true),
                    Running = table.Column<float>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analyse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Analyse_AnalysisSource_AnalysisSourceId",
                        column: x => x.AnalysisSourceId,
                        principalTable: "AnalysisSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Analyse_Data_DataId",
                        column: x => x.DataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataPaths",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataId = table.Column<Guid>(nullable: true),
                    WriteTime = table.Column<string>(nullable: false),
                    Path = table.Column<string>(maxLength: 256, nullable: true),
                    Storage = table.Column<string>(maxLength: 30, nullable: true),
                    Run = table.Column<int>(nullable: false),
                    SubRun = table.Column<int>(nullable: false),
                    EventNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataPaths_Data_DataId",
                        column: x => x.DataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataDisplayData_Data_DataId",
                        column: x => x.DataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisPannel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PannelId = table.Column<Guid>(nullable: true),
                    AnalyseId = table.Column<Guid>(nullable: true),
                    DataDisplayId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisPannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisPannel_Analyse_AnalyseId",
                        column: x => x.AnalyseId,
                        principalTable: "Analyse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalysisPannel_DataDisplay_DataDisplayId",
                        column: x => x.DataDisplayId,
                        principalTable: "DataDisplay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnalysisPannel_Pannel_PannelId",
                        column: x => x.PannelId,
                        principalTable: "Pannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisParameter",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParameterId = table.Column<Guid>(nullable: true),
                    AnalyseId = table.Column<Guid>(nullable: true),
                    Degree = table.Column<float>(nullable: false),
                    Interval = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisParameter_Analyse_AnalyseId",
                        column: x => x.AnalyseId,
                        principalTable: "Analyse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnalysisParameter_Parameter_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "Parameter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataDisplayAnalyse",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AnalyseId = table.Column<Guid>(nullable: true),
                    DataDisplayId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataDisplayAnalyse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataDisplayAnalyse_Analyse_AnalyseId",
                        column: x => x.AnalyseId,
                        principalTable: "Analyse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataDisplayAnalyse_DataDisplay_DataDisplayId",
                        column: x => x.DataDisplayId,
                        principalTable: "DataDisplay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AnalysisParameterId = table.Column<Guid>(nullable: true),
                    DataPathId = table.Column<Guid>(nullable: true),
                    Decision = table.Column<string>(nullable: true),
                    Confidence = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisResult_AnalysisParameter_AnalysisParameterId",
                        column: x => x.AnalysisParameterId,
                        principalTable: "AnalysisParameter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalysisResult_DataPaths_DataPathId",
                        column: x => x.DataPathId,
                        principalTable: "DataPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DataType",
                columns: new[] { "Id", "Description", "Name", "PlottingType" },
                values: new object[,]
                {
                    { new Guid("8d83a885-c0aa-4d36-ab49-a93c8525d3f5"), "Default heatmap plotting", "Heatmap plot", "heatmap" },
                    { new Guid("b0ab9f47-bd2a-462b-aa24-fb52b7184885"), "Default histogram plotting", "Histogram plot", "histogram" },
                    { new Guid("7592e161-e925-4d58-9c68-0f93431e439c"), "Default scatter plotting, Scatter plot with lines and markers", "Scatter plot with lines and markers", "lines+markers" },
                    { new Guid("34e44dd0-7219-493e-8cd9-c63d8a0387e3"), "Scatter plot without markers (lines only)", "Scatter plot with lines", "lines" },
                    { new Guid("0e14499d-8106-4c05-953f-e52a5f91da8b"), "Scatter plot without lines (markers only)", "Scatter plot with markers", "markers" }
                });

            migrationBuilder.InsertData(
                table: "SamplingProfile",
                columns: new[] { "Id", "Description", "Factor", "Name", "PlottingType" },
                values: new object[] { new Guid("c7b9ec99-1a44-4ef5-8b89-f6a0404fb4d4"), "Default 1:1 sampling", 1f, "Default", "Default" });

            migrationBuilder.CreateIndex(
                name: "IX_Analyse_AnalysisSourceId",
                table: "Analyse",
                column: "AnalysisSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Analyse_DataId",
                table: "Analyse",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisPannel_AnalyseId",
                table: "AnalysisPannel",
                column: "AnalyseId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisPannel_DataDisplayId",
                table: "AnalysisPannel",
                column: "DataDisplayId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisPannel_PannelId",
                table: "AnalysisPannel",
                column: "PannelId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisParameter_AnalyseId",
                table: "AnalysisParameter",
                column: "AnalyseId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisParameter_ParameterId",
                table: "AnalysisParameter",
                column: "ParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResult_AnalysisParameterId",
                table: "AnalysisResult",
                column: "AnalysisParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResult_DataPathId",
                table: "AnalysisResult",
                column: "DataPathId");

            migrationBuilder.CreateIndex(
                name: "IX_Data_DataSourceId",
                table: "Data",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataDisplay_DataTypeId",
                table: "DataDisplay",
                column: "DataTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataDisplay_SamplingProfileId",
                table: "DataDisplay",
                column: "SamplingProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_DataDisplayAnalyse_AnalyseId",
                table: "DataDisplayAnalyse",
                column: "AnalyseId");

            migrationBuilder.CreateIndex(
                name: "IX_DataDisplayAnalyse_DataDisplayId",
                table: "DataDisplayAnalyse",
                column: "DataDisplayId");

            migrationBuilder.CreateIndex(
                name: "IX_DataDisplayData_DataDisplayId",
                table: "DataDisplayData",
                column: "DataDisplayId");

            migrationBuilder.CreateIndex(
                name: "IX_DataDisplayData_DataId",
                table: "DataDisplayData",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_DataPaths_DataId",
                table: "DataPaths",
                column: "DataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysisPannel");

            migrationBuilder.DropTable(
                name: "AnalysisResult");

            migrationBuilder.DropTable(
                name: "DataDisplayAnalyse");

            migrationBuilder.DropTable(
                name: "DataDisplayData");

            migrationBuilder.DropTable(
                name: "Pannel");

            migrationBuilder.DropTable(
                name: "AnalysisParameter");

            migrationBuilder.DropTable(
                name: "DataPaths");

            migrationBuilder.DropTable(
                name: "DataDisplay");

            migrationBuilder.DropTable(
                name: "Analyse");

            migrationBuilder.DropTable(
                name: "Parameter");

            migrationBuilder.DropTable(
                name: "DataType");

            migrationBuilder.DropTable(
                name: "SamplingProfile");

            migrationBuilder.DropTable(
                name: "AnalysisSource");

            migrationBuilder.DropTable(
                name: "Data");

            migrationBuilder.DropTable(
                name: "DataSources");
        }
    }
}
