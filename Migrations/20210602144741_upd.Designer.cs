﻿// <auto-generated />
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
    [Migration("20210602144741_upd")]
    partial class upd
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.13")
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

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataDisplay", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AnalyseId")
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

                    b.HasIndex("AnalyseId");

                    b.HasIndex("DataTypeId");

                    b.HasIndex("SamplingProfileId");

                    b.ToTable("DataDisplay");
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

                    b.Property<string>("Path")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("Storage")
                        .HasColumnType("character varying(30)")
                        .HasMaxLength(30);

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
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.Analyse", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.AnalysisSource", "AnalysisSource")
                        .WithMany("Analyses")
                        .HasForeignKey("AnalysisSourceId");

                    b.HasOne("DuneDaqMonitoringPlatform.Models.Data", "Data")
                        .WithMany("Analyses")
                        .HasForeignKey("DataId");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.AnalysisPannel", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.Analyse", "Analyse")
                        .WithMany()
                        .HasForeignKey("AnalyseId");

                    b.HasOne("DuneDaqMonitoringPlatform.Models.DataDisplay", "DataDisplay")
                        .WithMany("AnalysisPannels")
                        .HasForeignKey("DataDisplayId");

                    b.HasOne("DuneDaqMonitoringPlatform.Models.Pannel", "Pannel")
                        .WithMany("AnalysisPannels")
                        .HasForeignKey("PannelId");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.AnalysisParameter", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.Analyse", "Analyse")
                        .WithMany("AnalysisParameters")
                        .HasForeignKey("AnalyseId");

                    b.HasOne("DuneDaqMonitoringPlatform.Models.Parameter", "Parameter")
                        .WithMany("AnalysisParameters")
                        .HasForeignKey("ParameterId");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.AnalysisResult", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.AnalysisParameter", "AnalysisParameter")
                        .WithMany("AnalysisResults")
                        .HasForeignKey("AnalysisParameterId");

                    b.HasOne("DuneDaqMonitoringPlatform.Models.DataPath", "DataPath")
                        .WithMany("AnalysisResults")
                        .HasForeignKey("DataPathId");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.Data", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.DataSource", "DataSource")
                        .WithMany("Datas")
                        .HasForeignKey("DataSourceId");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataDisplay", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.Analyse", "Analyse")
                        .WithMany("DataDisplays")
                        .HasForeignKey("AnalyseId");

                    b.HasOne("DuneDaqMonitoringPlatform.Models.DataType", "DataType")
                        .WithMany("DataDisplays")
                        .HasForeignKey("DataTypeId");

                    b.HasOne("DuneDaqMonitoringPlatform.Models.SamplingProfile", "SamplingProfile")
                        .WithMany("DataDisplays")
                        .HasForeignKey("SamplingProfileId");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataDisplayData", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.DataDisplay", "DataDisplay")
                        .WithMany("DataDisplayDatas")
                        .HasForeignKey("DataDisplayId");

                    b.HasOne("DuneDaqMonitoringPlatform.Models.Data", "Data")
                        .WithMany("DataDisplayDatas")
                        .HasForeignKey("DataId");
                });

            modelBuilder.Entity("DuneDaqMonitoringPlatform.Models.DataPath", b =>
                {
                    b.HasOne("DuneDaqMonitoringPlatform.Models.Data", "Data")
                        .WithMany("DataPaths")
                        .HasForeignKey("DataId");
                });
#pragma warning restore 612, 618
        }
    }
}
