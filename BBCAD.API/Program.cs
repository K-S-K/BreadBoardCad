using System.Text;
using BBCAD.Itself;

namespace BBCAD.API
{
    public class Program
    {
        private const string programName = "Bread Board CAD V.0.0.1";

        public static void Main(string[] args)
        {
            var directoryName = Directory.GetCurrentDirectory();
            Console.WriteLine(programName);
            Console.WriteLine();
            Console.WriteLine($"args[{args.Length}]: {string.Join(" ", args.ToList())}");
            Console.WriteLine();
            Console.WriteLine($"at {directoryName}");
            Console.WriteLine();

            WebApplicationOptions options = new()
            {
                Args = args,
                WebRootPath = directoryName
            };

            var app = WebApplication.CreateBuilder(options).Build();

            CreateMapping(app);

            app.Run();
        }

        private static void CreateMapping(WebApplication app)
        {
            app.MapGet("/", () => $"{programName} on .Net v.{Environment.Version}, Os: {Environment.OSVersion}, PID: {Environment.ProcessId}");

            app.MapGet("/favicon.ico", async context =>
            {
                context.Response.ContentType = "image/svg+xml";
                await context.Response.WriteAsync(Favicon);
            });

            app.MapGet("/test.htm", async context =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8";
                await context.Response.WriteAsync(TestPage);
            });

            app.Map("/demo-board", CreateDemoBoard);
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

        private static string TestPage
        {
            get
            {
                StringBuilder sb = new();

                sb.AppendLine($"<HTML><BODY>");
                sb.AppendLine($"");
                sb.AppendLine($"{programName}<BR>");
                sb.AppendLine($"<BR>");
                sb.AppendLine($"<img src=\"/demo-board\" />");
                sb.AppendLine($"<BR>");
                sb.AppendLine($"<BR>");

                sb.AppendLine($"<TABLE>");
                sb.AppendLine($"<TR><TD>.Net Version</TD><TD>{Environment.Version}</TD></TR>");
                sb.AppendLine($"<TR><TD>OSVersion</TD><TD>{Environment.OSVersion}</TD></TR>");
                sb.AppendLine($"<TR><TD>CurrentDirectory</TD><TD>{Environment.CurrentDirectory}</TD></TR>");
                sb.AppendLine($"<TR><TD>ProcessPath</TD><TD>{Environment.ProcessPath}</TD></TR>");
                sb.AppendLine($"<TR><TD>MachineName</TD><TD>{Environment.MachineName}</TD></TR>");
                sb.AppendLine($"<TR><TD>UserName</TD><TD>{Environment.UserName}</TD></TR>");
                sb.AppendLine($"<TR><TD>ProcessorCount</TD><TD>{Environment.ProcessorCount}</TD></TR>");
                sb.AppendLine($"<TR><TD>ProcessId</TD><TD>{Environment.ProcessId}</TD></TR>");
                sb.AppendLine($"<TR><TD>WorkingSet</TD><TD>{Environment.WorkingSet}</TD></TR>");
                sb.AppendLine($"</TABLE>"); 
                sb.AppendLine($"");
                sb.AppendLine($"</BODY></HTML>");

                return sb.ToString();
            }
        }
    }
}
