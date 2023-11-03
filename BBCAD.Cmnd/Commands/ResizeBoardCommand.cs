using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Impl.Commands;

namespace BBCAD.Cmnd.Commands
{
    public class ResizeBoardCommand : CommandBase
    {
        public ResizeBoardCommand() : base(CommandType.ResizeBoard, "RESIZE BOARD")
        {
            Parameters.Add(new CommandParameter("ID", ParameterType.GUID, ObligationType.ContextBased));
            Parameters.Add(new CommandParameter("X", ParameterType.Integer, ObligationType.Mandatoty));
            Parameters.Add(new CommandParameter("Y", ParameterType.Integer, ObligationType.Mandatoty));
            Parameters.Add(new CommandParameter("Direction", ParameterType.Direction, ObligationType.Optional));
        }
    }
}
