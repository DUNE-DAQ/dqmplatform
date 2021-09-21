using DuneDaqMonitoringPlatform.Data;
using DuneDaqMonitoringPlatform.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using DuneDaqMonitoringPlatform.Services;
using DuneDaqMonitoringPlatform.Actions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using System;

namespace DuneDaqMonitoringPlatform
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            new SendMessagesToClients(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddEntityFrameworkNpgsql().AddDbContext<MonitoringDbContext>();

            services.AddEntityFrameworkNpgsql().AddDbContext<UserDbContext>();

            services.AddSignalR();

            services.AddHostedService<KafkaConsumer>();

            var manager = new ConfigurationManager<OpenIdConnectConfiguration>(Configuration.GetValue<string>("LogginService:WellKnown"), new OpenIdConnectConfigurationRetriever());
            /*
            services.AddAuthentication()
                .AddOpenIdConnect("CERN", c => {
                    c.ConfigurationManager = manager;
                    c.ClientId = Configuration.GetValue<string>("LogginService:ClientId");
                    c.ClientSecret = Configuration.GetValue<string>("LogginService:ClientSecret");
                    c.Authority = "https://auth.cern.ch";
                    c.ResponseType = "code";
                });
            */

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                //options.ExcludedHosts.Add("http://test-dunedqm.app.cern.ch");
                //options.ExcludedHosts.Add("http://www.test-dunedqm.app.cern.ch");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });
   
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<ChartHub>("/charthub");
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            
        }

    }
}
