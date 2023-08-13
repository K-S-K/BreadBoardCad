using System.Text;
using BBCAD.Itself;
using Microsoft.Extensions.Configuration.Json;

namespace BBCAD.API
{
    public class Program
    {
        private const string programName = "Bread Board CAD V.0.0.1";

        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Configuration.Sources.Clear();
            builder.Configuration
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

            DisplaySettings(args, builder);

            var app = builder.Build();

            CreateMapping(app);

            app.Run();
        }

        #region -> Diagnostics
        private static void DisplaySettings(string[] args, WebApplicationBuilder builder)
        {
            var directoryName = Directory.GetCurrentDirectory();
            Console.WriteLine(programName);
            Console.WriteLine();
            Console.WriteLine($"args[{args.Length}]: {string.Join(" ", args.ToList())}");
            Console.WriteLine();
            Console.WriteLine($"at {directoryName}");
            Console.WriteLine();

            Console.WriteLine($"Env={builder.Environment.EnvironmentName}");
            Console.WriteLine($"Configuration.Sources[{builder.Configuration.Sources.Count}]");
            Console.WriteLine($"{{");
            foreach (var cfg in builder.Configuration.Sources)
            {
                if (cfg is JsonConfigurationSource j)
                {
                    Console.WriteLine($" {j.Path}, {(j.Optional ? "Optional" : "Mandatory")}");
                }
            }
            Console.WriteLine($"}}");

            Console.WriteLine($"");

            IEnumerable<IConfigurationSection> parameters = builder.Configuration.GetChildren();
            Console.WriteLine($"Parameters[{parameters.Count()}]:");
            Console.WriteLine($"{{");
            DisplayParameters(parameters);
            Console.WriteLine($"}}");
        }

        private static void DisplayParameters(IEnumerable<IConfigurationSection> parameters, string prefix = "")
        {
            foreach (IConfigurationSection cfg in parameters)
            {
                IEnumerable<IConfigurationSection>? children = null;
                if (cfg is ConfigurationSection sect)
                {
                    children = sect.GetChildren();
                }

                Console.WriteLine($" {prefix}{cfg.Key}: {cfg.Value}");
                // Console.WriteLine($" {prefix}{cfg.Key} ({cfg.Path}): {cfg.Value}");

                if (children != null && children.Count() > 0)
                {
                    DisplayParameters(children, prefix + " ");
                }
            }
        }
        #endregion

        private static void CreateMapping(WebApplication app)
        {
            // app.MapGet("/", () => $"{programName} on .Net v.{Environment.Version}, Os: {Environment.OSVersion}, PID: {Environment.ProcessId}");
            app.MapGet("/", async context =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8";
                await context.Response.WriteAsync(TestPage);
            });

            app.MapGet("/test.htm", async context =>
            {
                context.Response.ContentType = "text/html; charset=UTF-8";
                await context.Response.WriteAsync(TestPage);
            });

            app.MapGet("/favicon.ico", async context =>
            {
                context.Response.ContentType = "image/svg+xml";
                await context.Response.WriteAsync(Favicon);
            });

            app.Map("/demo-board", CreateDemoBoard);
        }

        private static IResult CreateDemoBoard(HttpContext context)
        {
            string content;

            content = Board.Sample.SVG
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
