using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace OneCSharp.Web.Server
{
    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        CreateHostBuilder(args).Build().Run();
    //    }

    //    public static IHostBuilder CreateHostBuilder(string[] args) =>
    //        Host.CreateDefaultBuilder(args)
    //            .ConfigureWebHostDefaults(webBuilder =>
    //            {
    //                webBuilder.UseStartup<Startup>();
    //            });
    //}
    public sealed class Program
    {
        private const string Setting_ServerURL = "ServerURL";

        public static void Main(string[] args)
        {
            string url = string.Empty; //ConfigurationManager.AppSettings[Setting_ServerURL];

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

        public static string GetErrorText(Exception ex)
        {
            string errorText = string.Empty;
            Exception error = ex;
            while (error != null)
            {
                errorText += (errorText == string.Empty) ? error.Message : Environment.NewLine + error.Message;
                error = error.InnerException;
            }
            return errorText;
        }
    }
}
