using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Impl.Commands;

namespace BBCAD.Cmnd.Commands
{
    public class CreateBoardCommand : CommandBase
    {
        public CreateBoardCommand() : base(CommandType.CreateBoard, "CREATE BOARD")
        {
            Parameters.Add(new CommandParameter("Name", ParameterType.String, ObligationType.Mandatoty));
            Parameters.Add(new CommandParameter("X", ParameterType.Integer, ObligationType.Mandatoty));
            Parameters.Add(new CommandParameter("Y", ParameterType.Integer, ObligationType.Mandatoty));
            Parameters.Add(new CommandParameter("Description", ParameterType.String, ObligationType.Optional));
            Parameters.Add(new CommandParameter("User", ParameterType.GUID, ObligationType.Optional));
        }
    }
}
