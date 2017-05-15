using System.Net.Http;
using AutoMapper;
using DdhpCore.FrontEnd.Configuration;
using DdhpCore.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DdhpCore.FrontEnd
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<DataOptions>(Configuration);
            services.Configure<ApiOptions>(Configuration);
            services.AddMvc();
            services.AddSingleton<CloudStorageAccount>(
                provider =>
                        CloudStorageAccount.Parse(provider.GetService<IOptions<DataOptions>>().Value.StorageConnectionString));
            services.AddScoped<CloudTableClient>(
                provider => provider.GetService<CloudStorageAccount>().CreateCloudTableClient());
            services.AddSingleton<IMapper>(builder =>
            {
                var config = new MapperConfiguration(ClassMaps.BuildMaps);
                var mapper = config.CreateMapper();
                return mapper;
            });
            services.AddScoped<IStorageFacade, StorageFacade>();
            services.AddTransient<HttpClient>((factory) => new HttpClient(new HttpClientHandler()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory, 
            IApplicationLifetime applicationLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddAzureWebAppDiagnostics();

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
            }

            var logger = loggerFactory.CreateLogger<Startup>();
            logger.LogDebug("Configuration starting");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

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
