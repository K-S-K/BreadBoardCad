using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using BBCAD.Cmnd;
using BBCAD.Data;

namespace BBCAD.Core
{
    public static class CoreLibraryExtensions
    {
        public static IServiceCollection AddBBCadCore(this IServiceCollection services)
        {
            services.AddBBCadScriptProcessor();

            services.TryAddSingleton<IBoardStorage, BoardStorage>();
            services.TryAddSingleton<IBehavior, CadCoreBehavior>();

            return services;
        }
    }
}
