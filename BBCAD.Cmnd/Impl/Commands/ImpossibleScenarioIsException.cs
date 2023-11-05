using BBCAD.Cmnd.Common;

namespace BBCAD.Cmnd.Impl.Commands
{
    public class ImpossibleScenarioIsException : Exception
    {
        private ImpossibleScenarioIsException(string message) : base(message) { }

        internal static ImpossibleScenarioIsException CreateBoardFirst()
        {
            return new ImpossibleScenarioIsException($"The first command command in the batch must be \"{CommandType.CreateBoard}\".");
        }
    }
}
