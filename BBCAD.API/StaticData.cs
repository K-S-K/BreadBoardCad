using Microsoft.Extensions.Configuration.Json;
using System.Text;

namespace BBCAD.API
{
    internal static class StaticData
    {
        public const string programName = "Bread Board CAD V.0.0.1";

        public static string Favicon
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

        public static string TestPage
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
                sb.AppendLine($"<BR>");
                sb.AppendLine($"<BR>");

                sb.AppendLine($"<A href = \"/swagger\" target = \"_blank\">Swagger</A>");
                sb.AppendLine($"");
                sb.AppendLine($"</BODY></HTML>");

                return sb.ToString();
            }
        }


        public static void DisplaySettings(WebApplicationBuilder builder)
        {
            var directoryName = Directory.GetCurrentDirectory();
            Console.WriteLine(programName);
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

        public static void DisplayParameters(IEnumerable<IConfigurationSection> parameters, string prefix = "")
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

    }
}
