using System.Xml.Linq;

using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Impl.Commands;
using BBCAD.Cmnd.Impl.Parameters;

namespace BBCAD.Cmnd.Commands
{
    public class CloneBoardCommand : CommandBase
    {
        public ParamStr Name { get; private set; } = new();
        public ParamGuid User { get; private set; } = new();
        public ParamGuid Origin { get; private set; } = new();

        public CloneBoardCommand() : base(CommandType.CloneBoard, "CLONE BOARD")
        {
            AddParameter(new("Origin", ParameterType.GUID, ObligationType.Mandatoty), Origin);
            AddParameter(new("Name", ParameterType.String, ObligationType.Optional), Name);
            AddParameter(new("User", ParameterType.GUID, ObligationType.Optional), User);
        }

        public CloneBoardCommand(XElement xe) : this() { this.XML = xe; }
    }
}
