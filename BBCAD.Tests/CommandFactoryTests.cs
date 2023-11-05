using System.Xml.Linq;

using BBCAD.Cmnd;
using BBCAD.Cmnd.Common;
using Assert = NUnit.Framework.Assert;

namespace BBCAD.Tests
{
    [TestClass]
    public class CommandFactoryTests
    {
        private ICommandFactory commandFactory = null!;


        [OneTimeSetUp]
        public void SetUp()
        {
            commandFactory = TestExtensions.CreateCommandFactory();
        }

        [TestCase(CommandType.CreateBoard, @"CREATE BOARD Name = ""board name"" X = 8 Y = 12 Description = ""Board Description"" User = ""0xB800""", @"CREATE BOARD Name = ""board name"" X = 8 Y = 12 Description = ""Board Description"" User = ""0xB800""")]
        [TestCase(CommandType.CreateBoard, @" CREATE  BOARD   Name =  ""board name""  X  =  8   Y  =  12    Description  =  ""Board Description""  User  =  ""0xB800""", @"CREATE BOARD Name = ""board name"" X = 8 Y = 12 Description = ""Board Description"" User = ""0xB800""")]
        [TestCase(CommandType.CreateBoard, @"CREATE BOARD Name=""board name"" X=8 Y=12 Description=""Board Description"" User=""0xB800""", @"CREATE BOARD Name = ""board name"" X = 8 Y = 12 Description = ""Board Description"" User = ""0xB800""")]
        public void CommandFromTextCommandTest(CommandType type, string txtInput, string txtExpected)
        {
            ICommand? cmnd = commandFactory.ParseStatement(txtInput);
            Assert.IsNotNull(cmnd, $"{type.GetType().Name}.{type}");

            string txtActual = cmnd.ToString();
            Assert.AreEqual(txtExpected, txtActual, "TXT parsing result is different from the expected one");
        }

        [TestCase(@"<Command type=""CreateBoard"" Name=""board name"" X=""8"" Y=""12"" />")]
        public void CommandFromXmlCommandTest(string txtExpectd)
        {
            XElement xe = XElement.Parse(txtExpectd);
            ICommand? cmnd = commandFactory.DeserializeStatement(xe);

            string txtActual = cmnd.XML.ToString();
            Assert.AreEqual(txtExpectd, txtActual, "XML deserialization result is different from the expected one");
        }
    }
}
