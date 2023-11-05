﻿using System.Xml.Linq;

using BBCAD.Cmnd;
using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Commands;

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
