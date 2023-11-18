using System.Xml.Linq;

using BBCAD.Cmnd;
using BBCAD.Core;
using BBCAD.Data;
using BBCAD.Itself;
using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Commands;

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
        public void ExecuteComandTest()
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

            var cmndCrt = batch.Commands
                .Where(x => x.CmndType == CommandType.CreateBoard).Single() as CreateBoardCommand;
            Assert.IsNotNull(cmndCrt);

            var cmndRsz = batch.Commands
                .Where(x => x.CmndType == CommandType.ResizeBoard).Single() as ResizeBoardCommand;
            Assert.IsNotNull(cmndRsz);

            Board board1 = behavior.ExecuteComand(cmndCrt);
            Assert.IsNotNull(board1);
            Assert.AreEqual(board1.Name, cmndCrt.Name.Value);
            Assert.AreEqual(board1.SizeX, cmndCrt.X.Value);
            Assert.AreEqual(board1.SizeY, cmndCrt.Y.Value);

            // Propagate new board Id to the next command
            XElement xeCmndRsz = cmndRsz.XML;
            xeCmndRsz.SetAttributeValue("Id", board1.Id);
            cmndRsz.XML = xeCmndRsz;

            Board board2 = behavior.ExecuteComand(cmndRsz);
            Assert.IsNotNull(board2);
            Assert.AreEqual(board2.Name, cmndCrt.Name.Value);
            Assert.AreEqual(board2.SizeX, cmndRsz.X.Value);
            Assert.AreEqual(board2.SizeY, cmndRsz.Y.Value);
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
