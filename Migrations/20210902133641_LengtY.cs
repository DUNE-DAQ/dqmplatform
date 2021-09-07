using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class LengtY : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("0e14499d-8106-4c05-953f-e52a5f91da8b"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("34e44dd0-7219-493e-8cd9-c63d8a0387e3"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("7592e161-e925-4d58-9c68-0f93431e439c"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("8d83a885-c0aa-4d36-ab49-a93c8525d3f5"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("b0ab9f47-bd2a-462b-aa24-fb52b7184885"));

            migrationBuilder.DeleteData(
                table: "SamplingProfile",
                keyColumn: "Id",
                keyValue: new Guid("c7b9ec99-1a44-4ef5-8b89-f6a0404fb4d4"));

            migrationBuilder.DropColumn(
                name: "PlotLength",
                table: "DataDisplay");

            migrationBuilder.AddColumn<int>(
                name: "PlotLengthX",
                table: "DataDisplay",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlotLengthY",
                table: "DataDisplay",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "DataType",
                columns: new[] { "Id", "Description", "Name", "PlottingType" },
                values: new object[,]
                {
                    { new Guid("a2b64743-cb5d-4798-8716-67454ad4deb3"), "Default heatmap plotting", "Heatmap plot", "heatmap" },
                    { new Guid("f29b7c3f-f96b-44ad-98f5-87dd0e506987"), "Default histogram plotting", "Histogram plot", "histogram" },
                    { new Guid("93dbf500-b1c0-41e0-9758-66d968009176"), "Default scatter plotting, Scatter plot with lines and markers", "Scatter plot with lines and markers", "lines+markers" },
                    { new Guid("987ed037-34da-4ce2-a7cc-c50fb881849f"), "Scatter plot without markers (lines only)", "Scatter plot with lines", "lines" },
                    { new Guid("8e8b2ab0-5c04-481e-8b24-d99e3b2fc00e"), "Scatter plot without lines (markers only)", "Scatter plot with markers", "markers" }
                });

            migrationBuilder.InsertData(
                table: "SamplingProfile",
                columns: new[] { "Id", "Description", "Factor", "Name", "PlottingType" },
                values: new object[] { new Guid("b1190749-3b40-4a90-9920-6c822973720c"), "Default 1:1 sampling", 1f, "Default", "Default" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("8e8b2ab0-5c04-481e-8b24-d99e3b2fc00e"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("93dbf500-b1c0-41e0-9758-66d968009176"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("987ed037-34da-4ce2-a7cc-c50fb881849f"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("a2b64743-cb5d-4798-8716-67454ad4deb3"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("f29b7c3f-f96b-44ad-98f5-87dd0e506987"));

            migrationBuilder.DeleteData(
                table: "SamplingProfile",
                keyColumn: "Id",
                keyValue: new Guid("b1190749-3b40-4a90-9920-6c822973720c"));

            migrationBuilder.DropColumn(
                name: "PlotLengthX",
                table: "DataDisplay");

            migrationBuilder.DropColumn(
                name: "PlotLengthY",
                table: "DataDisplay");

            migrationBuilder.AddColumn<int>(
                name: "PlotLength",
                table: "DataDisplay",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}
