using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class InitialReleaseCBMonitoring : Migration
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

            migrationBuilder.InsertData(
                table: "DataType",
                columns: new[] { "Id", "Description", "Name", "PlottingType" },
                values: new object[,]
                {
                    { new Guid("0083afc7-258b-4b3d-bc23-0c9df3225cb2"), "Default heatmap plotting", "Heatmap plot", "heatmap" },
                    { new Guid("0b8cd0ed-4870-4031-8448-170d0c29fb8b"), "Default histogram plotting", "Histogram plot", "histogram" },
                    { new Guid("8c25fe4d-22c3-4732-914b-142fd5faf712"), "Default scatter plotting, Scatter plot with lines and markers", "Scatter plot with lines and markers", "lines+markers" },
                    { new Guid("ad733dc2-3e92-42d9-9c59-76587507db72"), "Scatter plot without markers (lines only)", "Scatter plot with lines", "lines" },
                    { new Guid("ccd630fd-4d21-40b9-aa3b-4f62b922b5f7"), "Scatter plot without lines (markers only)", "Scatter plot with markers", "markers" }
                });

            migrationBuilder.InsertData(
                table: "SamplingProfile",
                columns: new[] { "Id", "Description", "Factor", "Name", "PlottingType" },
                values: new object[] { new Guid("ed076c41-833d-48b6-9544-4e64cf1da2ce"), "Default 1:1 sampling", 1f, "Default", "Default" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("0083afc7-258b-4b3d-bc23-0c9df3225cb2"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("0b8cd0ed-4870-4031-8448-170d0c29fb8b"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("8c25fe4d-22c3-4732-914b-142fd5faf712"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("ad733dc2-3e92-42d9-9c59-76587507db72"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("ccd630fd-4d21-40b9-aa3b-4f62b922b5f7"));

            migrationBuilder.DeleteData(
                table: "SamplingProfile",
                keyColumn: "Id",
                keyValue: new Guid("ed076c41-833d-48b6-9544-4e64cf1da2ce"));

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
