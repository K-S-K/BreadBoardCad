using BBCAD.Cmnd;
using BBCAD.Cmnd.Common;

using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BBCAD.Tests
{
    [TestClass]
    public class CommandLibraryTests
    {
        /// <summary>
        /// Checks if all command types are registered in the library
        /// </summary>
        /// <remarks>
        /// If the test fails, it probably means that 
        /// CommandLibrary constructor does not contains
        /// a new added command type registration statement.
        /// </remarks>
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

                Assert.IsTrue(commandLibrary.TryGetValue(type, out _),
                    $"The command type {type} must be registered in the {nameof(Cmnd.Impl.Commands.CommandLibrary)}");
            }
        }
    }
}
