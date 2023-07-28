using BBCAD.Itself;
using System.Diagnostics;
using System.Text;

namespace BBCAD.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var programName = $"Bread Board CAD V.0.0.1 on .Net v.{Environment.Version}";
            Console.WriteLine(programName);
            Console.WriteLine();

          //  var builder = WebApplication.CreateBuilder(args);
          //  var app = builder.Build();
          //
          //  app.MapGet("/", () => programName);
          //
          //  app.Map("/demo-board", CreateDemoBoard);
          //
          //  app.Run();
        }

        private static IResult CreateDemoBoard(HttpContext context)
        {
            string content;

            content = new Board().XML
                .ToString().Replace("xmlns=\"\" ", "");

            context.Response.Headers.CacheControl = "no-cache";

            string mimeType = "image/svg+xml";
            MemoryStream stream = new(Encoding.UTF8.GetBytes(content));
            return Results.File(stream, mimeType, $"board.svg");
        }
    }
}