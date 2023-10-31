using BBCAD.Cmnd.Commands;
using BBCAD.Cmnd.Common;
using NUnit.Framework;

using Assert = NUnit.Framework.Assert;

namespace BBCAD.Tests
{
    [TestFixture]
    public class CommandTests
    {
        [TestCase("board name", 8, 12, null, @"CREATE BOARD Name = ""board name"" X = 8 Y = 12 Description = """" User = """"", @"<Command type=""CreateBoard"" Name=""board name"" X=""8"" Y=""12"" />")]
        public void CommandCreateBoardFromParametersCommandTest(
            string name, int x, int y,
            string? descr, string txtExpected, string xmlExpected)
        {
            CreateBoardCommand cmnd = new();
            cmnd.Parameters["name"].Value = name;
            cmnd.Parameters["x"].Value = x.ToString();
            cmnd.Parameters["y"].Value = y.ToString();
            if (descr != null)
            {
                cmnd.Parameters["Description"].Value = name;
            }

            string txtActual = cmnd.ToString();
            Assert.AreEqual(txtExpected, txtActual, "TXT serialization result is different from the expected one");

            string xmlActual = cmnd.XML.ToString();
            Assert.AreEqual(xmlExpected, xmlActual, "XML serialization result is different from the expected one");
        }

        // [TestCase(CommandType.CreateBoard, @"CREATE BOARD Name = ""board name"" X = 8 Y = 12 Description = ""Board Descriptio"" User = ""0xB800""", @"CREATE BOARD Name = ""board name"" X = 8 Y = 12 Description = """" User = """"")]
        public void CommandFromTextCommandTest(
            CommandType type, string txtInput, string txtExpected)
        {
            ICommand? cmnd =
                type == CommandType.CreateBoard ? new CreateBoardCommand() :
                type == CommandType.ResizeBoard ? new ResizeBoardCommand() :
                type == CommandType.CloneBoard ? new CloneBoardCommand() : null;

            if(cmnd==null)
            {
                throw new NotImplementedException($"{type.GetType().Name}.{type}");
            }

            cmnd.Parse(txtInput);

            string txtActual = cmnd.ToString();
            Assert.AreEqual(txtExpected, txtActual, "TXT parsing result is different from the expected one");
        }
    }
}
