using System.Xml.Linq;

using BBCAD.Cmnd;

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

            ICommandBatch batch = _scriptProcessor.ExtractCommands(script);

            Assert.IsEmpty(batch.Commands);
        }

        [Test]
        public void ScriptBatchingTest()
        {
            // TODO: Input - script, output - serialized batch
            Assert.IsNotNull(_scriptProcessor, $"{_scriptProcessor} should not be null");

            string strExpected = Resources.Batch_01_CRC;
            XElement xeBatch = XElement.Parse(strExpected);
            strExpected = xeBatch.ToString();

            string script = Resources.Script_01_CRC;

            ICommandBatch batch = _scriptProcessor.ExtractCommands(script);

            string txtActual = batch.XML.ToString();
            Assert.AreEqual(strExpected, txtActual, "XML deserialization result is different from the expected one");
        }

        [Test]
        public void BatchSerializationTest()
        {
            string strExpected = Resources.Batch_01_CRC;
            XElement xeBatch = XElement.Parse(strExpected);
            strExpected = xeBatch.ToString();

            ICommandBatch batch = _scriptProcessor.RestoreBatch(xeBatch);

            string txtActual = batch.XML.ToString();
            Assert.AreEqual(strExpected, txtActual, "XML deserialization result is different from the expected one");
        }

        [Test]
        public void ScriptProcessorParseScript()
        {
            Assert.IsNotNull(_scriptProcessor, $"{_scriptProcessor} should not be null");

            string script = Resources.Script_01_CRC;

            ICommandBatch batch = _scriptProcessor.ExtractCommands(script);

            Assert.AreEqual(2, batch.Length);
        }
    }
}
