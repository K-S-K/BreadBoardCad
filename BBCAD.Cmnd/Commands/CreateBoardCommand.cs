using System.Xml.Linq;

using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Impl.Commands;
using BBCAD.Cmnd.Impl.Parameters;

namespace BBCAD.Cmnd.Commands
{
    public class CreateBoardCommand : CommandBase
    {
        public ParamStr Name { get; private set; } = new();
        public ParamStr Description { get; private set; } = new();
        public ParamInt X { get; private set; } = new();
        public ParamInt Y { get; private set; } = new();
        public ParamGuid User { get; private set; } = new();


        public CreateBoardCommand() : base(CommandType.CreateBoard, "CREATE BOARD")
        {
            AddParameter(new("Name", ParameterType.String, ObligationType.Mandatoty), Name);
            AddParameter(new("X", ParameterType.Integer, ObligationType.Mandatoty), X);
            AddParameter(new("Y", ParameterType.Integer, ObligationType.Mandatoty), Y);
            AddParameter(new("Description", ParameterType.String, ObligationType.Optional), Description);
            AddParameter(new("User", ParameterType.GUID, ObligationType.Optional), User);
        }

        public CreateBoardCommand(XElement xe) : this() { this.XML = xe; }
    }
}
