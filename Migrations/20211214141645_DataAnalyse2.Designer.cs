// <auto-generated />
using System;
using DuneDaqMonitoringPlatform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DuneDaqMonitoringPlatform.Migrations
{
    [DbContext(typeof(MonitoringDbContext))]
    [Migration("20211214141645_DataAnalyse2")]
    partial class DataAnalyse2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.Analyse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AnalysisSourceId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DataId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<float>("Running")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("AnalysisSourceId");

                    b.HasIndex("DataId");

                    b.ToTable("Analyse");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.AnalysisPannel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AnalyseId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DataDisplayId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PannelId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AnalyseId");

                    b.HasIndex("DataDisplayId");

                    b.HasIndex("PannelId");

                    b.ToTable("AnalysisPannel");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.AnalysisParameter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AnalyseId")
                        .HasColumnType("uuid");

                    b.Property<float>("Degree")
                        .HasColumnType("real");

                    b.Property<int>("Interval")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ParameterId")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AnalyseId");

                    b.HasIndex("ParameterId");

                    b.ToTable("AnalysisParameter");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.AnalysisResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AnalysisParameterId")
                        .HasColumnType("uuid");

                    b.Property<float>("Confidence")
                        .HasColumnType("real");

                    b.Property<Guid?>("DataPathId")
                        .HasColumnType("uuid");

                    b.Property<string>("Decision")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AnalysisParameterId");

                    b.HasIndex("DataPathId");

                    b.ToTable("AnalysisResult");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.AnalysisSource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("AnalysisSource");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.Data", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DataSourceId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RententionTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DataSourceId");

                    b.ToTable("Data");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataAnalyse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AnalyseId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DataId")
                        .HasColumnType("uuid");

                    b.Property<int>("channel")
                        .HasColumnType("integer");

                    b.Property<Guid?>("dataDisplayId")
                        .HasColumnType("uuid");

                    b.Property<string>("description")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AnalyseId");

                    b.HasIndex("DataId");

                    b.HasIndex("dataDisplayId");

                    b.ToTable("DataAnalyse");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataDisplay", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DataTypeId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PlotLength")
                        .HasColumnType("integer");

                    b.Property<Guid?>("SamplingProfileId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DataTypeId");

                    b.HasIndex("SamplingProfileId");

                    b.ToTable("DataDisplay");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataDisplayAnalyse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AnalyseId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DataDisplayId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AnalyseId");

                    b.HasIndex("DataDisplayId");

                    b.ToTable("DataDisplayAnalyse");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataDisplayData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DataDisplayId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DataId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DataDisplayId");

                    b.HasIndex("DataId");

                    b.ToTable("DataDisplayData");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataPath", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DataId")
                        .HasColumnType("uuid");

                    b.Property<int>("EventNumber")
                        .HasColumnType("integer");

                    b.Property<string>("Path")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<int>("Run")
                        .HasColumnType("integer");

                    b.Property<string>("Storage")
                        .HasColumnType("character varying(30)")
                        .HasMaxLength(30);

                    b.Property<int>("SubRun")
                        .HasColumnType("integer");

                    b.Property<string>("WriteTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DataId");

                    b.ToTable("DataPaths");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataSource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("DataSources");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("PlottingType")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("DataType");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0b2f3718-f37f-43d4-9feb-42fd1dd77c25"),
                            Description = "Default heatmap plotting",
                            Name = "heatmap",
                            PlottingType = "standard"
                        },
                        new
                        {
                            Id = new Guid("da685200-8463-4263-b64a-f083d95c7ca9"),
                            Description = "Default histogram plotting",
                            Name = "histogram",
                            PlottingType = "standard"
                        },
                        new
                        {
                            Id = new Guid("ad36747d-ffa2-497d-880d-2874c8e1c229"),
                            Description = "Default scatter plotting, Scatter plot with lines and markers",
                            Name = "lines+markers",
                            PlottingType = "standard"
                        },
                        new
                        {
                            Id = new Guid("17e0d1fa-063f-41c6-b868-148e58afa507"),
                            Description = "Scatter plot with lines (no markers)",
                            Name = "lines",
                            PlottingType = "standard"
                        },
                        new
                        {
                            Id = new Guid("2a8ce785-c2f0-4e87-948b-56b01a24571f"),
                            Description = "Scatter plot with markers (no lines)",
                            Name = "markers",
                            PlottingType = "standard"
                        },
                        new
                        {
                            Id = new Guid("d97d4dd7-a818-4ded-add3-897c51e687f1"),
                            Description = "Default scatter plotting, Scatter plot with lines and markers, with log scale",
                            Name = "lines+markers",
                            PlottingType = "log"
                        },
                        new
                        {
                            Id = new Guid("d01159f2-00a0-49fc-9022-b6901939b888"),
                            Description = "Scatter plot with markers (no lines), with log scale",
                            Name = "lines",
                            PlottingType = "log"
                        },
                        new
                        {
                            Id = new Guid("5f2ef2b2-73e7-4bd2-b5d4-a8c14376d517"),
                            Description = "Scatter plot with lines (no markers), with log scale",
                            Name = "markers",
                            PlottingType = "log"
                        });
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.Pannel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Pannel");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.Parameter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<float>("Factor")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Parameter");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.SamplingProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.Property<float>("Factor")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("PlottingType")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("SamplingProfile");

                    b.HasData(
                        new
                        {
                            Id = new Guid("51629972-0c64-4cf0-90fd-3db1a98986d7"),
                            Description = "Default 1:1 sampling",
                            Factor = 1f,
                            Name = "Default",
                            PlottingType = "Default"
                        });
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.Analyse", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.AnalysisSource", "AnalysisSource")
                        .WithMany("Analyses")
                        .HasForeignKey("AnalysisSourceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DuneDaqMonitoringPlatform.Models.Data", "Data")
                        .WithMany("Analyses")
                        .HasForeignKey("DataId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.AnalysisPannel", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.Analyse", "Analyse")
                        .WithMany()
                        .HasForeignKey("AnalyseId");

                    b.HasOne("DuneDaqMonitoringPlatform.Models.DataDisplay", "DataDisplay")
                        .WithMany("AnalysisPannels")
                        .HasForeignKey("DataDisplayId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DuneDaqMonitoringPlatform.Models.Pannel", "Pannel")
                        .WithMany("AnalysisPannels")
                        .HasForeignKey("PannelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.AnalysisParameter", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.Analyse", "Analyse")
                        .WithMany("AnalysisParameters")
                        .HasForeignKey("AnalyseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DuneDaqMonitoringPlatform.Models.Parameter", "Parameter")
                        .WithMany("AnalysisParameters")
                        .HasForeignKey("ParameterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.AnalysisResult", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.AnalysisParameter", "AnalysisParameter")
                        .WithMany("AnalysisResults")
                        .HasForeignKey("AnalysisParameterId");

                    b.HasOne("DuneDaqMonitoringPlatform.Models.DataPath", "DataPath")
                        .WithMany("AnalysisResults")
                        .HasForeignKey("DataPathId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.Data", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.DataSource", "DataSource")
                        .WithMany("Datas")
                        .HasForeignKey("DataSourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataAnalyse", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.Analyse", "Analyse")
                        .WithMany("DataAnalyses")
                        .HasForeignKey("AnalyseId");

                    b.HasOne("DuneDaqMonitoringPlatform.Models.Data", "Data")
                        .WithMany("DataAnalyses")
                        .HasForeignKey("DataId");

                    b.HasOne("DuneDaqMonitoringPlatform.Models.DataDisplay", "dataDisplay")
                        .WithMany("DataAnalyses")
                        .HasForeignKey("dataDisplayId");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataDisplay", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.DataType", "DataType")
                        .WithMany("DataDisplays")
                        .HasForeignKey("DataTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DuneDaqMonitoringPlatform.Models.SamplingProfile", "SamplingProfile")
                        .WithMany("DataDisplays")
                        .HasForeignKey("SamplingProfileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataDisplayAnalyse", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.Analyse", "Analyse")
                        .WithMany("DataDisplayAnalyses")
                        .HasForeignKey("AnalyseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DuneDaqMonitoringPlatform.Models.DataDisplay", "DataDisplay")
                        .WithMany("DataDisplayAnalyses")
                        .HasForeignKey("DataDisplayId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataDisplayData", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.DataDisplay", "DataDisplay")
                        .WithMany("DataDisplayDatas")
                        .HasForeignKey("DataDisplayId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DuneDaqMonitoringPlatform.Models.Data", "Data")
                        .WithMany("DataDisplayDatas")
                        .HasForeignKey("DataId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataPath", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.Data", "Data")
                        .WithMany("DataPaths")
                        .HasForeignKey("DataId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
