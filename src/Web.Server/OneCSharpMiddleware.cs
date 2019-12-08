using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OneCSharp.DSL.Model;
using OneCSharp.DSL.Services;
using OneCSharp.Metadata.Model;
using OneCSharp.Metadata.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OneCSharp.Web.Server
{
    public static class OneCSharpMiddlewareExtentions
    {
        public static IApplicationBuilder UseOneCSharpMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OneCSharpMiddleware>();
        }
    }

    public sealed class OneCSharpMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<OneCSharpMiddleware> _logger;
        private Dictionary<string, Request> routes = new Dictionary<string, Request>();
        private readonly IMetadataProvider Metadata = new MetadataProvider();
        private readonly IOneCSharpJsonSerializer Serializer = new OneCSharpJsonSerializer();
        public OneCSharpMiddleware(RequestDelegate next, ILogger<OneCSharpMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "GET" || context.Request.Method == "POST")
            {
                //if (routes.Count == 0)
                //{
                routes = Metadata.GetRequests();
                //}

                Request request = null;
                string json = string.Empty;

                _logger.LogInformation("Request path: {0}", context.Request.Path);

                if (routes.TryGetValue(context.Request.Path, out request))
                {
                    json = request.ParseTree;
                    Procedure query = Serializer.FromJson(json);

                    JObject parameters = this.ReadParameters(context);
                    if (parameters != null && query.Parameters != null && query.Parameters.Count > 0)
                    {
                        this.SetParameters(parameters, query);
                    }

                    QueryExecutor executor = new QueryExecutor(query);
                    try
                    {
                        var result = executor.Build().ExecuteAsRowData();
                        json = JsonConvert.SerializeObject(result);
                    }
                    catch (Exception ex)
                    {
                        json = Program.GetErrorText(ex);
                    }
                }
                else
                {
                    _logger.LogInformation("Requested path not found.");
                    _logger.LogInformation("Available paths are:");
                    foreach (string route in routes.Keys)
                    {
                        _logger.LogInformation(route);
                    }
                }

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            else
            {
                await _next(context);
            }
        }
        private JObject ReadParameters(HttpContext context)
        {
            JObject parameters = null;

            using (var reader = new StreamReader(context.Request.Body))
            {
                string body = reader.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(body))
                {
                    parameters = JsonConvert.DeserializeObject<JObject>(body);
                }
            }
            return parameters;
        }
        private void SetParameters(JObject parameters, Procedure query)
        {
            //Dictionary<string, object> bag = new Dictionary<string, object>();
            //{
            //	"Булево":true,
            //  "Строка":"Тест",
            //	"ЦелоеЧисло":123,
            //	"ДробноеЧисло":123.45,
            //	"Дата":"2019-08-01T19:15:00",
            //	"Неопределено":null
            //}

            JsonSerializer serializer = JsonSerializer.Create();
            foreach (JProperty property in parameters.Properties())
            {
                //bag.Add(property.Name, serializer.Deserialize(property.Value.CreateReader()));

                ParameterExpression parameter = query.Parameters.Where(p => p.Name == property.Name).FirstOrDefault();
                if (parameter != null)
                {
                    parameter.Value = serializer.Deserialize(property.Value.CreateReader());
                }
            }
        }
    }
}
