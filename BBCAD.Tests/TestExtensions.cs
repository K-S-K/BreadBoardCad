using BBCAD.Cmnd;
using BBCAD.Core;
using BBCAD.Data;

namespace BBCAD.Tests
{
    public static class TestExtensions
    {

        public static ServiceProvider CreateCadCoreBehavior()
        {
            var services = new ServiceCollection();
            services.AddBBCadScriptProcessor();
            services.AddBBCadCore();

            ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);

            var provider = services.BuildServiceProvider();

            ICommandFactory commandFactory = provider.GetRequiredService<ICommandFactory>();
            ICommandLibrary commandLibrary = provider.GetRequiredService<ICommandLibrary>();
            IScriptProcessor scriptProcessor = provider.GetService<IScriptProcessor>();

            IBoardStorage boardStorage = provider.GetService<IBoardStorage>();
            IBehavior behavior = provider.GetService<IBehavior>();

            return provider;
        }

        public static IScriptProcessor CreateScriptProcessor()
        {
            var services = new ServiceCollection();
            services.AddBBCadScriptProcessor();

            ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);

            var provider = services.BuildServiceProvider();

            ICommandFactory commandFactory = provider.GetRequiredService<ICommandFactory>();
            ICommandLibrary commandLibrary = provider.GetRequiredService<ICommandLibrary>();
            IScriptProcessor scriptProcessor = provider.GetService<IScriptProcessor>();

            return scriptProcessor;
        }

        public static ICommandLibrary CreateCommandLibrary()
        {
            var services = new ServiceCollection();
            services.AddBBCadCommandLibraryOnly();

            ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);

            var provider = services.BuildServiceProvider();

            ICommandLibrary commandLibrary = provider.GetRequiredService<ICommandLibrary>();

            return commandLibrary;
        }

        public static ICommandFactory CreateCommandFactory()
        {
            var services = new ServiceCollection();
            services.AddBBCadCommandLibraryOnly();

            ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);

            var provider = services.BuildServiceProvider();

            ICommandFactory commandFactory = provider.GetRequiredService<ICommandFactory>();
            ICommandLibrary commandLibrary = provider.GetRequiredService<ICommandLibrary>();

            return commandFactory;
        }
    }
}
