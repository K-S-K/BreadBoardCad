using System.Text;
using BBCAD.Itself;

namespace BBCAD.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var programName = $"Bread Board CAD V.0.0.1 on .Net v.{Environment.Version}";
            var directoryName = Directory.GetCurrentDirectory();
            Console.WriteLine(programName);
            Console.WriteLine();
            Console.WriteLine($"args[{args.Count()}]: {string.Join(" ", args.ToList())}");
            Console.WriteLine();
            Console.WriteLine($"at {directoryName}");
            Console.WriteLine();

            // return;

            WebApplicationOptions options = new()
            {
                WebRootPath = directoryName
            };

            var builder = WebApplication.CreateBuilder(options);

            var app = builder.Build();

            app.MapGet("/", () => programName);

            app.MapGet("/favicon.ico", async context =>
            {
                context.Response.ContentType = "image/svg+xml";
                await context.Response.WriteAsync(Favicon);
            });

            app.Map("/demo-board", CreateDemoBoard);

            app.Run();
        }

        private static async void GetFavIcon(HttpContext context)
        {
            context.Response.ContentType = "image/svg+xml";
            await context.Response.WriteAsync(Favicon);
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

        private static string Favicon
        {
            get
            {
                StringBuilder sb = new();

                sb.AppendLine(@"<svg version=""1.1""");
                sb.AppendLine(@"width=""16"" height=""16""");
                sb.AppendLine(@"xmlns=""http://www.w3.org/2000/svg"">");
                sb.AppendLine(@"<rect width=""16"" height=""16"" rx=""3"" fill=""green"" />");
                sb.AppendLine(@"<circle cx=""8"" cy=""8"" r=""5"" fill=""yellow"" />");
                sb.AppendLine(@"<circle cx=""8"" cy=""8"" r=""3"" fill=""black"" />");
                sb.AppendLine(@"</svg>");

                return sb.ToString();
            }
        }
    }
}