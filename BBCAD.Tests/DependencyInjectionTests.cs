using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using BBCAD.Cmnd;

namespace BBCAD.Tests
{
    [TestClass]
    public class DependencyInjectionTests
    {
        [TestMethod]
        public void CommandLibraryDITest_From_ScriptProcessor()
        {
            var services = new ServiceCollection();
            services.AddBBCadScriptProcessor();

            ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);

            var provider = services.BuildServiceProvider();

            ICommandFactory commandFactory = provider.GetRequiredService<ICommandFactory>();
            Assert.IsNotNull(commandFactory);

            ICommandLibrary commandLibrary = provider.GetRequiredService<ICommandLibrary>();
            Assert.IsNotNull(commandLibrary);

            IScriptProcessor scriptProcessor = provider.GetRequiredService<IScriptProcessor>();
            Assert.IsNotNull(scriptProcessor);
        }

        [TestMethod]
        public void CommandLibraryDITest_Only()
        {
            var services = new ServiceCollection();
            services.AddBBCadCommandLibraryOnly();

            ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);

            var provider = services.BuildServiceProvider();

            ICommandFactory commandFactory = provider.GetRequiredService<ICommandFactory>();
            Assert.IsNotNull(commandFactory);

            ICommandLibrary commandLibrary = provider.GetRequiredService<ICommandLibrary>();
            Assert.IsNotNull(commandLibrary);
        }
    }
}
