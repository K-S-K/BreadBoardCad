using BBCAD.Cmnd;
using BBCAD.Cmnd.Scripts;
using BBCAD.Cmnd.Commands;

using Assert = NUnit.Framework.Assert;

namespace BBCAD.Tests.ScriptProcessorTests
{
    [TestFixture]
    public class ScriptProcessorTests
    {
        private IScriptProcessor _scriptProcessor = null!;

        [OneTimeSetUp]
        public void SetUp()
        {
            _scriptProcessor = TestExtensions.CreateScriptProcessor();
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("// Something")]
        [TestCase("/*Delimited;*/ //text")]
        public void ScriptProcessorParseNoScript(string script)
        {
            Assert.IsNotNull(_scriptProcessor, $"{_scriptProcessor} should not be null");

            IEnumerable<ICommand> commands = _scriptProcessor.ExtractCommands(script);

            Assert.IsEmpty(commands);
        }

        [Test]
        public void ScriptProcessorParseScript()
        {
            Assert.IsNotNull(_scriptProcessor, $"{_scriptProcessor} should not be null");

            string script = Resources.Script_01_CRC;

            IEnumerable<ICommand> commands = _scriptProcessor.ExtractCommands(script);

            Assert.AreEqual(2, commands.Count());
        }
    }
}
