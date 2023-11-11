using System.Xml.Linq;

using BBCAD.Cmnd;
using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Commands;

using Assert = NUnit.Framework.Assert;

namespace BBCAD.Tests
{
    [TestFixture]
    public class CommandTests
    {
        [TestCase(CommandType.CreateBoard, @"<Command type=""CreateBoard"" Name=""board name"" X=""8"" Y=""12"" />")]
        public void CommandSerializationTest(CommandType cmndType, string txtExpectd)
        {
            XElement xe = XElement.Parse(txtExpectd);

            ICommand cmnd = cmndType switch
            {
                CommandType.CreateBoard => new CreateBoardCommand(xe),
                CommandType.ResizeBoard => new ResizeBoardCommand(xe),
                CommandType.CloneBoard => new CloneBoardCommand(xe),
                CommandType.AddLine => new AddLineCommand(xe),
                _ => throw new NotImplementedException(
                    $"{cmndType.GetType().Name}.{cmndType}"),
            };

            string txtActual = cmnd.XML.ToString();
            Assert.AreEqual(txtExpectd, txtActual, "XML deserialization result is different from the expected one");
        }
    }
}
