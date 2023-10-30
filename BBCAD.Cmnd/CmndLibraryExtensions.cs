using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using BBCAD.Cmnd.Commands;
using BBCAD.Cmnd.Scripts;

namespace BBCAD.Cmnd
{
    public static class CmndLibraryExtensions
    {
        /// <summary>
        /// Register the script processor
        /// with all its dependencies.
        /// </summary>
        /// <param name="services">DI Service Collection</param>
        /// <returns>DI Service Collection</returns>
        /// <remarks>
        /// The only dependency is ICommandLibrary
        /// Added to DI dependency is IScriptProcessor
        /// </remarks>
        public static IServiceCollection AddBBCadScriptProcessor(
            this IServiceCollection services
        )
        {
            services.AddBBCadCommandLibraryOnly();

            services.TryAddSingleton<IScriptProcessor, ScriptProcessor>();

            return services;
        }

        /// <summary>
        /// Register the CommandLibrary only
        /// </summary>
        /// <param name="services">DI Service Collection</param>
        /// <returns>DI Service Collection</returns>
        public static IServiceCollection AddBBCadCommandLibraryOnly(
        this IServiceCollection services
    )
        {
            services.TryAddSingleton<ICommandLibrary, CommandLibrary>();
            return services;
        }
    }
}
