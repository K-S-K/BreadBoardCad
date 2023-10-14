using System.Text;

using Microsoft.OpenApi.Models;

using BBCAD.Core;
using BBCAD.Data;
using BBCAD.Itself;

namespace BBCAD.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            #region -> Register Swagger to Services
            {
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BreadBoard CAD API", Description = "Make your board development documentable", Version = "v1" });
                });
            }
            #endregion


            #region -> Register Configurations to Services
            {
                builder.Configuration.Sources.Clear();
                builder.Configuration
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

                builder.Services.AddSingleton<IBoardStorage, BoardStorage>();
                builder.Services.AddSingleton<IBehavior, CadCoreBehavior>();
            }
            #endregion


            StaticData.DisplaySettings(builder);

            var app = builder.Build();

            #region -> Tell the API project to use Swagger and also where to find the specification file swagger.json.
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BreadBoard CAD API V1");
                });
                app.MapSwagger();
            }
            #endregion

            CreateMapping(app);

            app.Run();
        }

        private static void CreateMapping(WebApplication app)
        {
            // app.MapGet("/", () => $"{programName} on .Net v.{Environment.Version}, Os: {Environment.OSVersion}, PID: {Environment.ProcessId}");
            app.MapGet("/", async (HttpResponse responce, HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8";
                await context.Response.WriteAsync(StaticData.TestPage);
            });

            app.MapGet("/test.htm", async (HttpResponse responce, HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8";
                await context.Response.WriteAsync(StaticData.TestPage);
            });

            app.MapGet("/favicon.ico", async (HttpResponse responce, HttpContext context) =>
            {
                context.Response.ContentType = "image/svg+xml";
                await context.Response.WriteAsync(StaticData.Favicon);
            });

            app.MapGet("/demo-board", CreateDemoBoard);
        }

        private static IResult CreateDemoBoard(HttpResponse responce, HttpContext context, IBehavior _behavior)
        {
            string content;

            Board board = _behavior.GetDemoBoard();

            content = board.SVG
                .ToString().Replace("xmlns=\"\" ", "");

            context.Response.Headers.CacheControl = "no-cache";

            string mimeType = "image/svg+xml";
            MemoryStream stream = new(Encoding.UTF8.GetBytes(content));
            return Results.File(stream, mimeType, $"board.svg");
        }
    }
}
