using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Commands;

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
            CommandLibrary commandLibrary = new();

            foreach (CommandType type in Enum.GetValues(typeof(CommandType)))
            {
                Assert.IsTrue(commandLibrary.TryGetValue(type, out ICommand? cmnd));
            }
        }
    }
}
