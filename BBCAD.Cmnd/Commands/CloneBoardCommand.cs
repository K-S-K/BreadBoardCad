using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Impl.Commands;

namespace BBCAD.Cmnd.Commands
{
    public class CloneBoardCommand : CommandBase
    {
        public CloneBoardCommand() : base(CommandType.CloneBoard, "CLONE BOARD")
        {
            Parameters.Add(new CommandParameter("Origin", ParameterType.GUID, ObligationType.Mandatoty));
            Parameters.Add(new CommandParameter("Name", ParameterType.String, ObligationType.Optional));
            Parameters.Add(new CommandParameter("User", ParameterType.GUID, ObligationType.Optional));
        }
    }
}
