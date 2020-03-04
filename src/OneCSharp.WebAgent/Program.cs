using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace OneCSharp.WebAgent
{
    public sealed class Program
    {
        public static void Main()
        {
            string url = "http://localhost:5000";
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(url)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Build();
            host.Run();
        }
    }
}