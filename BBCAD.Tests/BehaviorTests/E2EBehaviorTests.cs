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
            IBehavior? behavior = _serviceProvider.GetService<IBehavior>();
            Assert.IsNotNull(behavior);

            IScriptProcessor? scriptProcessor = _serviceProvider.GetService<IScriptProcessor>();
            Assert.IsNotNull(behavior);

            IBoardStorage? boardStorage = _serviceProvider.GetService<IBoardStorage>();
            Assert.IsNotNull(boardStorage);

            string script = Resources.Script_01_CRC;
            ICommandBatch? batch = scriptProcessor?.ExtractCommands(script);
            Assert.IsNotNull(batch);

            Board? board = behavior?.ExecuteComandBatch(batch);
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
            IBehavior? behavior = _serviceProvider.GetService<IBehavior>();
            Assert.IsNotNull(behavior);

            IScriptProcessor? scriptProcessor = _serviceProvider.GetService<IScriptProcessor>();
            Assert.IsNotNull(behavior);

            IBoardStorage? boardStorage = _serviceProvider.GetService<IBoardStorage>();
            Assert.IsNotNull(boardStorage);

            string script = Resources.Script_01_CRC;
            ICommandBatch? batch = scriptProcessor?.ExtractCommands(script);
            Assert.IsNotNull(batch);

            var cmndCrt = batch?.Commands
                .Where(x => x.CmndType == CommandType.CreateBoard).Single() as CreateBoardCommand;
            Assert.IsNotNull(cmndCrt);

            var cmndRsz = batch?.Commands
                .Where(x => x.CmndType == CommandType.ResizeBoard).Single() as ResizeBoardCommand;
            Assert.IsNotNull(cmndRsz);

            Board? board1 = behavior?.ExecuteComand(cmndCrt);
            Assert.IsNotNull(board1);
            Assert.AreEqual(board1.Name, cmndCrt.Name.Value);
            Assert.AreEqual(board1.Description, cmndCrt.Description.Value);
            Assert.AreEqual(board1.SizeX, cmndCrt.X.Value);
            Assert.AreEqual(board1.SizeY, cmndCrt.Y.Value);

            // Propagate new board Id to the next command
            XElement? xeCmndRsz = cmndRsz?.XML;
            Assert.IsNotNull(xeCmndRsz);
            xeCmndRsz.SetAttributeValue("Id", board1.Id);
            cmndRsz.XML = xeCmndRsz;

            Board? board2 = behavior?.ExecuteComand(cmndRsz);
            Assert.IsNotNull(board2);
            Assert.AreEqual(board2.Name, cmndCrt.Name.Value);
            Assert.AreEqual(board2.SizeX, cmndRsz.X.Value);
            Assert.AreEqual(board2.SizeY, cmndRsz.Y.Value);
        }

        [Test]
        public void BoardStorageTest()
        {
            IBoardStorage? boardStorage = _serviceProvider.GetService<IBoardStorage>();
            Assert.IsNotNull(boardStorage);

            ICommandFactory? commandFactory = _serviceProvider.GetService<ICommandFactory>();
            Assert.IsNotNull(commandFactory);

            IBehavior? behavior = _serviceProvider.GetService<IBehavior>();
            Assert.IsNotNull(behavior);

            Guid u1 = Guid.NewGuid();
            Guid u2 = Guid.NewGuid();

            string txtBoard1 = $"CREATE BOARD Name = \"B1\" X = 8 Y = 12 User = \"{u1}\"\"";
            string txtBoard2 = $"CREATE BOARD Name = \"B2\" X = 8 Y = 12 User = \"{u2}\"\"";

            ICommand? cmnd1 = commandFactory?.ParseStatement(txtBoard1);
            ICommand? cmnd2 = commandFactory?.ParseStatement(txtBoard2);
            Assert.IsNotNull(cmnd1);
            Assert.IsNotNull(cmnd2);

            Board? boardIn11 = behavior?.ExecuteComand(cmnd1);
            Board? boardIn12 = behavior?.ExecuteComand(cmnd1);
            Board? boardIn21 = behavior?.ExecuteComand(cmnd2);
            Board? boardIn22 = behavior?.ExecuteComand(cmnd2);

            Assert.IsNotNull(boardIn11);
            Assert.IsNotNull(boardIn12);
            Assert.IsNotNull(boardIn21);
            Assert.IsNotNull(boardIn22);

            Assert.AreEqual(u1, boardIn11?.User);
            Assert.AreEqual(u1, boardIn12?.User);
            Assert.AreEqual(u2, boardIn21?.User);
            Assert.AreEqual(u2, boardIn22?.User);

            IEnumerable<Board>? boards1 = boardStorage?.GetBoards(u1);
            IEnumerable<Board>? boards2 = boardStorage?.GetBoards(u2);

            Assert.IsNotNull(boards1);
            Assert.IsNotNull(boards2);

            Assert.AreEqual(2, boards1?.Count());
            Assert.AreEqual(2, boards2?.Count());

            Assert.AreEqual(2, boards1?.Where(x => x.User == u1).Count());
            Assert.AreEqual(2, boards2?.Where(x => x.User == u2).Count());

            Assert.AreEqual(2, boards1?.Select(x => x.Id).Distinct().Count());
            Assert.AreEqual(2, boards2?.Select(x => x.Id).Distinct().Count());

            Board? boardOut11 = boardStorage?.GetBoard(boardIn11.Id);
            Board? boardOut12 = boardStorage?.GetBoard(boardIn12.Id);
            Board? boardOut21 = boardStorage?.GetBoard(boardIn21.Id);
            Board? boardOut22 = boardStorage?.GetBoard(boardIn22.Id);

            Assert.IsNotNull(boardOut11);
            Assert.IsNotNull(boardOut12);
            Assert.IsNotNull(boardOut21);
            Assert.IsNotNull(boardOut22);

            Assert.AreEqual(boardIn11?.XML.ToString(), boardOut11?.XML.ToString());
            Assert.AreEqual(boardIn12?.XML.ToString(), boardOut12?.XML.ToString());
            Assert.AreEqual(boardIn21?.XML.ToString(), boardOut21?.XML.ToString());
            Assert.AreEqual(boardIn22?.XML.ToString(), boardOut22?.XML.ToString());
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
