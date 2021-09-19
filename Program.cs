using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Authentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel(options => {
                            options.Listen(IPAddress.Loopback, 5002, listnerOption => {
                                listnerOption.UseHttps("localhost.pfx", "1234");
                            });
                        })
                        .UseStartup<Startup>();
                });
    }
}
