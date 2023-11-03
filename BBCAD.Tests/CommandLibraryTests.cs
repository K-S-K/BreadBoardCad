using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Commands;

using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BBCAD.Tests
{
    [TestClass]
    public class CommandLibraryTests
    {
        /// <summary>
        /// Checks if all command types are registered in the library
        /// </summary>
        [TestMethod]
        public void CheckCommandLibraryComplete()
        {
            ICommandLibrary commandLibrary = TestExtensions.CreateCommandLibrary();

            foreach (CommandType type in Enum.GetValues(typeof(CommandType)))
            {
                if (type == default)
                {
                    continue;
                }

                Assert.IsTrue(commandLibrary.TryGetValue(type, out _));
            }
        }
    }
}
