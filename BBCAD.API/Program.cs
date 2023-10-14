using System.Text;

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

            builder.Configuration.Sources.Clear();
            builder.Configuration
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

            builder.Services.AddSingleton<IBoardStorage, BoardStorage>();
            builder.Services.AddSingleton<IBehavior, CadCoreBehavior>();


            StaticData.DisplaySettings(builder);

            var app = builder.Build();

            CreateMapping(app);

            app.Run();
        }

        private static void CreateMapping(WebApplication app)
        {
            // app.MapGet("/", () => $"{programName} on .Net v.{Environment.Version}, Os: {Environment.OSVersion}, PID: {Environment.ProcessId}");
            app.MapGet("/", async context =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8";
                await context.Response.WriteAsync(StaticData.TestPage);
            });

            app.MapGet("/test.htm", async context =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8";
                await context.Response.WriteAsync(StaticData.TestPage);
            });

            app.MapGet("/favicon.ico", async context =>
            {
                context.Response.ContentType = "image/svg+xml";
                await context.Response.WriteAsync(StaticData.Favicon);
            });

            app.Map("/demo-board", CreateDemoBoard);
        }

        private static IResult CreateDemoBoard(HttpContext context, IBehavior _behavior)
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
