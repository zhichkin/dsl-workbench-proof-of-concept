using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OneCSharp.WebAgent
{
    public static class WebAgentMiddlewareExtentions
    {
        public static IApplicationBuilder UseWebAgentMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebAgentMiddleware>();
        }
    }
    public sealed class WebAgentMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<WebAgentMiddleware> _logger;
        private Dictionary<string, object> _services = new Dictionary<string, object>();
        public WebAgentMiddleware(RequestDelegate next, IWebHostEnvironment environment, ILogger<WebAgentMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            InitializeServices();
            string rootPath = environment.ContentRootPath;
        }
        private void InitializeServices()
        {
            _services.Add("/deploy", new DeployService());
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "POST")
            {
                string json;
                try
                {
                    if (GetService(context.Request.Path) is IDeployService service)
                    {
                        json = service.Deploy();
                    }
                    else
                    {
                        json = await GetBody(context);
                    }
                }
                catch (Exception ex)
                {
                    json = GetErrorText(ex);
                }
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            else
            {
                await _next(context);
            }
        }
        private object GetService(string path)
        {
            object service;
            _services.TryGetValue(path, out service);
            return service;
        }
        private async Task<string> GetBody(HttpContext context)
        {
            string message;
            using (var reader = new StreamReader(context.Request.Body))
            {
                string body = await reader.ReadToEndAsync();
                if (string.IsNullOrWhiteSpace(body))
                {
                    message = "{ \"message\" : \"body is empty\" }";
                }
                else
                {
                    message = body;
                }
            }
            return message;
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