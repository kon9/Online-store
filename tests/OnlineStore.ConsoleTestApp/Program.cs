using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OnlineStore.Library.Clients.AspIdentity;
using OnlineStore.Library.Clients.UserManagementService;
using OnlineStore.Library.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OnlineStore.ConsoleTestApp
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient<AspIdentityClient>();
                    services.AddHttpClient<UsersClient>();
                    services.AddHttpClient<RolesClient>();

                    services.AddTransient<AuthenticationServiceTest>();

                    var configurationBuilder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false, true);

                    var configuration = configurationBuilder.Build();

                    services.Configure<AspIdentityApiOptions>
                        (configuration.GetSection(AspIdentityApiOptions.SectionName));

                    services.Configure<ServiceAddressOptions>
                        (configuration.GetSection(ServiceAddressOptions.SectionName));
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .UseConsoleLifetime();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var service = services.GetRequiredService<AuthenticationServiceTest>();

                    var usersResult = await service.RunUserClientTest("testUser");
                    var rolesResult = await service.RunRolesClientTest("testRole");

                    Console.WriteLine($"Roles Client: {rolesResult}");
                    Console.WriteLine($"Users Client: {usersResult}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error occurred: {e.Message}");
                }
            }
            Console.ReadKey();

            return 0;
        }
    }
}