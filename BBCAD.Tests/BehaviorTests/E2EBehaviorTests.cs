using System.Xml.Linq;

using BBCAD.Cmnd;
using BBCAD.Core;
using BBCAD.Data;
using BBCAD.Itself;

using Assert = NUnit.Framework.Assert;

namespace BBCAD.Tests.BehaviorTests
{
    [TestFixture]
    public class E2EBehaviorTests
    {
        private ServiceProvider _serviceProvider = null!;

        [OneTimeSetUp]
        public void SetUp()
        {
            _serviceProvider = TestExtensions.CreateCadCoreBehavior();
        }

        [Test]
        public void ScriptProcessorParseTest()
        {
            IBehavior behavior = _serviceProvider.GetService<IBehavior>();
            Assert.IsNotNull(behavior);

            IScriptProcessor scriptProcessor = _serviceProvider.GetService<IScriptProcessor>();
            Assert.IsNotNull(behavior);

            IBoardStorage boardStorage = _serviceProvider.GetService<IBoardStorage>();
            Assert.IsNotNull(boardStorage);

            string script = Resources.Script_01_CRC;
            ICommandBatch batch = scriptProcessor.ExtractCommands(script);
            Assert.IsNotNull(batch);

            Board board = behavior.ExecuteComandBatch(batch);
            Assert.IsNotNull(board);

            XElement xeBoard = XElement.Parse(Resources.Board_01_CRC);
            xeBoard.Element(nameof(board.Id)).Value = board.Id.ToString().ToUpper();
            string strActual = board.XML.ToString();
            string strExpctd = xeBoard.ToString();
            Assert.AreEqual(strExpctd, strActual);
        }

        [Test]
        public void BoardSerializationTest()
        {
            // TODO: Move it to board test

            Board board = Board.Sample;

            string strExpctd = board.XML.ToString();
            string strActual = new Board(board.XML).XML.ToString();

            Assert.AreEqual(strExpctd, strActual);
        }
    }
}
