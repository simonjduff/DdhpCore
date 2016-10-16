using System;
using Amazon.DynamoDBv2;
using DdhpCore.Micros.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DdhpCore.FrontEnd
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            UseLocalDynamoDb = env.IsEnvironment("development");

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private bool UseLocalDynamoDb{get;set;}

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            if (!UseLocalDynamoDb)
            {
                services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
                services.AddAWSService<IAmazonDynamoDB>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var worker = new MicrosRunner();
            applicationLifetime.ApplicationStopping.Register(worker.StopApplication);
            worker.Run();

            app.UseMvc(BuildRoutes);
        }

        private void BuildRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
