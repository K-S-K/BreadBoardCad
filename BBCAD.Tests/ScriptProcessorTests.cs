using Microsoft.Extensions.DependencyInjection;

using BBCAD.Cmnd;
using BBCAD.Cmnd.Scripts;
using BBCAD.Cmnd.Commands;

namespace BBCAD.Tests
{
    [TestClass]
    public class ScriptProcessorTests
    {

        [TestMethod]
        public void ScriptProcessorParseNoScript()
        {
            IScriptProcessor scriptProcessor = CreateScriptProcessor();
            Assert.IsNotNull(scriptProcessor);

            string script=null;

            IEnumerable<ICommand> commands = scriptProcessor.ExtractCommands(script);
        }

        private static IScriptProcessor CreateScriptProcessor()
        {
            var services = new ServiceCollection();
            services.AddBBCadScriptProcessor();

            ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);

            var provider = services.BuildServiceProvider();

            IScriptProcessor scriptProcessor = provider.GetRequiredService<IScriptProcessor>();
            return scriptProcessor;
        }
    }
}
