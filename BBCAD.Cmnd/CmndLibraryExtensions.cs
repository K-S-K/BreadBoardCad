using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using BBCAD.Cmnd.Commands;
using BBCAD.Cmnd.Scripts;

namespace BBCAD.Cmnd
{
    public static class CmndLibraryExtensions
    {
        public static IServiceCollection AddBBCadScriptProcessor(
            this IServiceCollection services
        )
        {
            services.AddBBCadCommandLibraryOnly();

            services.TryAddSingleton<IScriptProcessor, ScriptProcessor>();

            return services;
        }

        public static IServiceCollection AddBBCadCommandLibraryOnly(
        this IServiceCollection services
    )
        {
            services.TryAddSingleton<ICommandLibrary, CommandLibrary>();
            return services;
        }
    }
}
