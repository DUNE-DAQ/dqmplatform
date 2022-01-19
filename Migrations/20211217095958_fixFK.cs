using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DuneDaqMonitoringPlatform.Migrations
{
    public partial class fixFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("0b2f3718-f37f-43d4-9feb-42fd1dd77c25"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("17e0d1fa-063f-41c6-b868-148e58afa507"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("2a8ce785-c2f0-4e87-948b-56b01a24571f"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("5f2ef2b2-73e7-4bd2-b5d4-a8c14376d517"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("ad36747d-ffa2-497d-880d-2874c8e1c229"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("d01159f2-00a0-49fc-9022-b6901939b888"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("d97d4dd7-a818-4ded-add3-897c51e687f1"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("da685200-8463-4263-b64a-f083d95c7ca9"));

            migrationBuilder.DeleteData(
                table: "SamplingProfile",
                keyColumn: "Id",
                keyValue: new Guid("51629972-0c64-4cf0-90fd-3db1a98986d7"));

            migrationBuilder.InsertData(
                table: "DataType",
                columns: new[] { "Id", "Description", "Name", "PlottingType" },
                values: new object[,]
                {
                    { new Guid("78704bbe-cfa1-4cc2-af3d-c05fb2e5a2f8"), "Default heatmap plotting", "heatmap", "standard" },
                    { new Guid("5e6e0a5f-7263-4193-a335-2c7e27ac69c1"), "Default histogram plotting", "histogram", "standard" },
                    { new Guid("c5818fd6-3442-4e7c-a6c7-dfa9d4710e63"), "Default scatter plotting, Scatter plot with lines and markers", "lines+markers", "standard" },
                    { new Guid("746448f1-6e53-4c83-9484-f6abc2f07431"), "Scatter plot with lines (no markers)", "lines", "standard" },
                    { new Guid("49684b8d-b2f4-4cdc-aba9-a023626d15ac"), "Scatter plot with markers (no lines)", "markers", "standard" },
                    { new Guid("b6131d73-e825-492c-a0df-a43fa53ad09a"), "Default scatter plotting, Scatter plot with lines and markers, with log scale", "lines+markers", "log" },
                    { new Guid("2269af8d-94ad-4c5e-a999-6fb51968e9cd"), "Scatter plot with markers (no lines), with log scale", "lines", "log" },
                    { new Guid("92478ff8-3168-4410-ae9e-560420486dc8"), "Scatter plot with lines (no markers), with log scale", "markers", "log" }
                });

            migrationBuilder.InsertData(
                table: "SamplingProfile",
                columns: new[] { "Id", "Description", "Factor", "Name", "PlottingType" },
                values: new object[] { new Guid("18a73f0e-5b9f-4ab0-b210-7386fac7d05a"), "Default 1:1 sampling", 1f, "Default", "Default" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("2269af8d-94ad-4c5e-a999-6fb51968e9cd"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("49684b8d-b2f4-4cdc-aba9-a023626d15ac"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("5e6e0a5f-7263-4193-a335-2c7e27ac69c1"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("746448f1-6e53-4c83-9484-f6abc2f07431"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("78704bbe-cfa1-4cc2-af3d-c05fb2e5a2f8"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("92478ff8-3168-4410-ae9e-560420486dc8"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("b6131d73-e825-492c-a0df-a43fa53ad09a"));

            migrationBuilder.DeleteData(
                table: "DataType",
                keyColumn: "Id",
                keyValue: new Guid("c5818fd6-3442-4e7c-a6c7-dfa9d4710e63"));

            migrationBuilder.DeleteData(
                table: "SamplingProfile",
                keyColumn: "Id",
                keyValue: new Guid("18a73f0e-5b9f-4ab0-b210-7386fac7d05a"));

            migrationBuilder.InsertData(
                table: "DataType",
                columns: new[] { "Id", "Description", "Name", "PlottingType" },
                values: new object[,]
                {
                    { new Guid("0b2f3718-f37f-43d4-9feb-42fd1dd77c25"), "Default heatmap plotting", "heatmap", "standard" },
                    { new Guid("da685200-8463-4263-b64a-f083d95c7ca9"), "Default histogram plotting", "histogram", "standard" },
                    { new Guid("ad36747d-ffa2-497d-880d-2874c8e1c229"), "Default scatter plotting, Scatter plot with lines and markers", "lines+markers", "standard" },
                    { new Guid("17e0d1fa-063f-41c6-b868-148e58afa507"), "Scatter plot with lines (no markers)", "lines", "standard" },
                    { new Guid("2a8ce785-c2f0-4e87-948b-56b01a24571f"), "Scatter plot with markers (no lines)", "markers", "standard" },
                    { new Guid("d97d4dd7-a818-4ded-add3-897c51e687f1"), "Default scatter plotting, Scatter plot with lines and markers, with log scale", "lines+markers", "log" },
                    { new Guid("d01159f2-00a0-49fc-9022-b6901939b888"), "Scatter plot with markers (no lines), with log scale", "lines", "log" },
                    { new Guid("5f2ef2b2-73e7-4bd2-b5d4-a8c14376d517"), "Scatter plot with lines (no markers), with log scale", "markers", "log" }
                });

            migrationBuilder.InsertData(
                table: "SamplingProfile",
                columns: new[] { "Id", "Description", "Factor", "Name", "PlottingType" },
                values: new object[] { new Guid("51629972-0c64-4cf0-90fd-3db1a98986d7"), "Default 1:1 sampling", 1f, "Default", "Default" });
        }
    }
}
