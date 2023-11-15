using System.Text;
using System.Reflection;

using Microsoft.OpenApi.Models;

using BBCAD.Core;
using BBCAD.Cmnd;
using BBCAD.Itself;
using BBCAD.API.DTO;

namespace BBCAD.API
{
    /// <summary>
    /// The main working class of the service
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The service entry point
        /// </summary>
        /// <param name="args">The entry point command line arguments</param>
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            #region -> Register Swagger to Services
            {
                string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BreadBoard CAD API", Description = "Make your board development documentable", Version = "v1" });
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                });
            }
            #endregion


            #region -> Register Configurations to Services
            {
                builder.Configuration.Sources.Clear();
                builder.Configuration
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

                builder.Services.AddBBCadCore();
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
            app.MapGet("/", ServiceStatusPage);
            app.MapGet("/favicon.ico", FavIcon);
            app.MapPost("/CreateBoard", CreateBoard);
            app.MapGet("/demo-board", CreateDemoBoard);
        }

        /// <summary>
        /// Retreive the favicon for the service status page.
        /// </summary>
        /// <param name="responce"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task FavIcon(HttpResponse responce, HttpContext context)
        {
            context.Response.ContentType = "image/svg+xml";
            await context.Response.WriteAsync(StaticData.Favicon);
        }

        /// <summary>
        /// Display the status page of the service
        /// </summary>
        /// <remarks>
        /// The simple diagnostic page 
        /// with the environment description 
        /// and the service status parameters.
        /// </remarks>
        /// <param name="responce"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task ServiceStatusPage(HttpResponse responce, HttpContext context)
        {
            context.Response.ContentType = "text/html; charset=UTF-8";
            await context.Response.WriteAsync(StaticData.TestPage);
        }

        /// <summary>
        /// Create a New Board
        /// </summary>
        /// <param name="cto" cref="CommandTransferObject">Input data example: { "Type": "CreateBoard", "Parameters": { "X": "8", "Y": "13", "Name": "Amazing device", "Description": "An exciting hardware project" } }</param>
        /// <param name="context"></param>
        /// <param name="_commandFactory"></param>
        /// <param name="_behavior"></param>
        /// <example>
        /// </example>
        /// <remarks>
        /// The normal response example:
        /// {
        ///     "Boards": {
        ///         "6637eeec-cab5-44c0-9a79-41661acbfe94": {
        ///             "Name": "Amazing device",
        ///             "SixeX": 8,
        ///             "SixeY": 13,
        ///             "svg": "&lt;svg version="1.1" width="180" height="280" ... &gt;"
        ///         }
        ///     },
        ///     "Error": null
        /// }
        /// 
        /// The abnormal response example:
        /// {
        ///     "Boards": {},
        ///     "Error": "The command is not consistent: CREATE BOARD Name = \"Amazing device\" X =  Y = 13 Description = \"An exciting hardware project\" User = \"\""
        /// }
        /// </remarks>
        /// <returns></returns>
        private static async Task CreateBoard(CommandTransferObject cto, HttpContext context, ICommandFactory _commandFactory, IBehavior _behavior)
        {
            context.Response.Headers.CacheControl = "no-cache";
            context.Response.ContentType = "application/json";

            ICommand command;

            try
            {
                command = cto.ToCommand(_commandFactory);
            }
            catch (Exception ex)
            {
                await PublishResponce(context, new(ex.Message));
                return;
            }

            Board board = _behavior.ExecuteComand(command);

            await PublishResponce(context, new(new Board[] { board }));
        }

        private static async Task PublishResponce(HttpContext context, BatchProcessingResponce responceObj)
        {
            string responceStr = System.Text.Json.JsonSerializer.Serialize(responceObj);

            await context.Response.WriteAsync(responceStr);
        }

        /// <summary>
        /// Create a Demo Board
        /// </summary>
        /// <param name="responce"></param>
        /// <param name="context"></param>
        /// <param name="_behavior"></param>
        /// <returns></returns>
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
