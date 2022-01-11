using AspNetCoreRateLimit;
using DogsAppAPI.DB.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace App_TestAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var ipPolicyStore = scope.ServiceProvider.GetRequiredService<IIpPolicyStore>();
                await ipPolicyStore.SeedAsync();

                try
                {
                    var context = services.GetRequiredService<DogsDbContext>();
                    SampleDogs.Initialize(context);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
