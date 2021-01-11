using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;

using WebApp.Models;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            
            // Application Insights
            // Enable Application Insights telemetry collection
            services.AddApplicationInsightsTelemetry();

            // Configure EventCounters (PerfCounters)
            // Collects every 60 seconds. Not configurable.
            services.ConfigureTelemetryModule<EventCounterCollectionModule>((module,o ) => 
              {
                // Remove default counters.
                // module.Counters.Clear();

                // Add user-defined counter
                module.Counters.Add(new EventCounterCollectionRequest("MyEventSource","MyCounter"));
                // Add the System Counter "gen-0-size" from System.Runtime
                module.Counters.Add(new EventCounterCollectionRequest("System.Runtime","gen-0-size"));
              });
            // Disable EventCounter collection Module
            var applicationInsightsServiceOptions = new ApplicationInsightsServiceOptions();
            // applicationInsightsServiceOptions.EnableEventCounterCollectionModule = false;
            // Disable adaptive sampling
            applicationInsightsServiceOptions.EnableAdaptiveSampling = true;
            // More: https://docs.microsoft.com/de-de/azure/azure-monitor/app/asp-net-core
            services.AddApplicationInsightsTelemetry(applicationInsightsServiceOptions);

            // Telemetry Initializer (simple)
            //services.AddSingleton<ITelemetryInitializer, MyCustomTelemetryInitializer()>();
            // Advanced
            services.AddSingleton(new MyCustomTelemetryInitializer() {
              fieldName = "myFieldName"
            });

            // Remove all initializers
            // This requires importing namespace by using Microsoft.Extensions.DependencyInjection.Extensions;
            //services.RemoveAll(typeof(ITelemetryInitializer));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
