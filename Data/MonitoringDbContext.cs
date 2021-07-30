using DuneDaqMonitoringPlatform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuneDaqMonitoringPlatform.Data
{
    public class MonitoringDbContext : DbContext
    {
        public MonitoringDbContext(IConfiguration configuration) : base(new DbContextOptionsBuilder<MonitoringDbContext>().UseNpgsql(   configuration["MonitoringDbConnectionParameters:Server"] +
                                                                                                                                        configuration["MonitoringDbConnectionParameters:Port"] +
                                                                                                                                        configuration["MonitoringDbConnectionParameters:Database"] +
                                                                                                                                        configuration["MonitoringDbConnectionParameters:User"] +
                                                                                                                                        configuration["MonitoringDbConnectionParameters:Password"] +
                                                                                                                                        configuration["MonitoringDbConnectionParameters:Security"] +
                                                                                                                                        configuration["MonitoringDbConnectionParameters:Pooling"]
                                                                                                                                    ).Options) { }
        public DbSet<Models.Data> Data { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.DataSource> DataSources { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.Parameter> Parameter { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.AnalysisSource> AnalysisSource { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.DataType> DataType { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.SamplingProfile> SamplingProfile { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.DataDisplay> DataDisplay { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.DataDisplayData> DataDisplayData { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.Analyse> Analyse { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.AnalysisParameter> AnalysisParameter { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.DataPath> DataPaths { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.Pannel> Pannel { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.AnalysisPannel> AnalysisPannel { get; set; }

        public DbSet<DuneDaqMonitoringPlatform.Models.AnalysisResult> AnalysisResult { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AnalysisSource>()
                .HasMany(a => a.Analyses)
                .WithOne(a => a.AnalysisSource)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Analyse>()
                .HasMany(a => a.AnalysisParameters)
                .WithOne(a => a.Analyse)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Analyse>()
                .HasMany(a => a.DataDisplayAnalyses)
                .WithOne(a => a.Analyse)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Models.Data>()
                .HasMany(a => a.DataPaths)
                .WithOne(a => a.Data)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Models.Data>()
                .HasMany(a => a.Analyses)
                .WithOne(a => a.Data)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Models.Data>()
                .HasMany(a => a.DataDisplayDatas)
                .WithOne(a => a.Data)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DataDisplay>()
                .HasMany(a => a.DataDisplayDatas)
                .WithOne(a => a.DataDisplay)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DataDisplay>()
                .HasMany(a => a.DataDisplayAnalyses)
                .WithOne(a => a.DataDisplay)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DataDisplay>()
                .HasMany(a => a.AnalysisPannels)
                .WithOne(a => a.DataDisplay)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DataType>()
                .HasMany(a => a.DataDisplays)
                .WithOne(a => a.DataType)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SamplingProfile>()
                .HasMany(a => a.DataDisplays)
                .WithOne(a => a.SamplingProfile)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Pannel>()
                .HasMany(a => a.AnalysisPannels)
                .WithOne(a => a.Pannel)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Parameter>()
                .HasMany(a => a.AnalysisParameters)
                .WithOne(a => a.Parameter)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DataPath>()
                .HasMany(a => a.AnalysisResults)
                .WithOne(a => a.DataPath)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DataSource>()
                .HasMany(a => a.Datas)
                .WithOne(a => a.DataSource)
                .OnDelete(DeleteBehavior.Cascade);

            //Seeding database
            builder.Entity<DataType>().HasData(new DataType { Id = Guid.NewGuid(), Description = "Default heatmap plotting", Name = "Heatmap plot", PlottingType = "heatmap" });
            builder.Entity<DataType>().HasData(new DataType { Id = Guid.NewGuid(), Description = "Default histogram plotting", Name = "Histogram plot", PlottingType = "histogram" });
            builder.Entity<DataType>().HasData(new DataType { Id = Guid.NewGuid(), Description = "Default scatter plotting, Scatter plot with lines and markers", Name = "Scatter plot with lines and markers", PlottingType = "lines+markers" });
            builder.Entity<DataType>().HasData(new DataType { Id = Guid.NewGuid(), Description = "Scatter plot without markers (lines only)", Name = "Scatter plot with lines", PlottingType="lines" });
            builder.Entity<DataType>().HasData(new DataType { Id = Guid.NewGuid(), Description = "Scatter plot without lines (markers only)", Name = "Scatter plot with markers", PlottingType="markers" });
            builder.Entity<SamplingProfile>().HasData(new SamplingProfile { Id= Guid.NewGuid(), Description = "Default 1:1 sampling", Factor = 1, Name = "Default", PlottingType = "Default" });


        }
    }
}
