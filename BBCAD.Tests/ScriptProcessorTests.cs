using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

using BBCAD.Cmnd;
using BBCAD.Cmnd.Scripts;
using BBCAD.Cmnd.Commands;

using Assert = NUnit.Framework.Assert;

namespace BBCAD.Tests
{
    [TestFixture]
    public class ScriptProcessorTests
    {
        private IScriptProcessor _scriptProcessor = null!;

        [SetUp]
        public void SetUp()
        {
            var services = new ServiceCollection();
            services.AddBBCadScriptProcessor();

            ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);

            var provider = services.BuildServiceProvider();

            _scriptProcessor = provider.GetRequiredService<IScriptProcessor>();
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("Something")]
        [TestCase("Delimited; text")]
        public void ScriptProcessorParseNoScript(string script)
        {
            Assert.IsNotNull(_scriptProcessor, $"{_scriptProcessor} should not be null");

            IEnumerable<ICommand> commands = _scriptProcessor.ExtractCommands(script);

            Assert.IsEmpty(commands);
        }
    }
}
