using System.Xml.Linq;

using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Impl.Commands;
using BBCAD.Cmnd.Impl.Parameters;

namespace BBCAD.Cmnd.Commands
{
    public class ResizeBoardCommand : CommandBase
    {
        public ParamInt X { get; private set; } = new();
        public ParamInt Y { get; private set; } = new();
        public ParamDir Direction { get; private set; } = new();
        public ParamGuid Id { get; private set; } = new();

        public ResizeBoardCommand() : base(CommandType.ResizeBoard, "RESIZE BOARD")
        {
            AddParameter(new("ID", ParameterType.GUID, ObligationType.ContextBased), Id);
            AddParameter(new("X", ParameterType.Integer, ObligationType.Mandatoty), X);
            AddParameter(new("Y", ParameterType.Integer, ObligationType.Mandatoty), Y);
            AddParameter(new("Direction", ParameterType.Direction, ObligationType.Optional), Direction);
        }

        public ResizeBoardCommand(XElement xe) : this() { this.XML = xe; }
    }
}
