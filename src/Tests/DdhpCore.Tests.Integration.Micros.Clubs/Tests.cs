using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace DdhpCore.Tests.Integration.Micros.Clubs
{
    public class Tests
    {
        private readonly HttpClient _client;
        private static readonly Regex AppRootRegex = new Regex("FrontEnd$", RegexOptions.Compiled);

        public Tests(ITestOutputHelper output)
        {
            var appSettings = new AppSettings(new ConfigurationBuilder()
                                                    .AddJsonFile("appsettings.json")
                                                    .AddEnvironmentVariables()
                                                    .Build());

            var testServer = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseContentRoot(ApplicationRoot().FullName)
                .UseStartup<TestStartup>());
            _client = testServer.CreateClient();
        }

        private DirectoryInfo ApplicationRoot()
        {
            var root = Directory.GetCurrentDirectory();
            var rootInfo = new DirectoryInfo(root);
            var src = rootInfo.Parent.Parent.EnumerateDirectories();
            var appRoot = src.Single(q => AppRootRegex.IsMatch(q.Name));
            return appRoot;
        }

        [Fact]
        public async Task Test1() 
        {
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            Assert.True(true);
        }
    }
}
