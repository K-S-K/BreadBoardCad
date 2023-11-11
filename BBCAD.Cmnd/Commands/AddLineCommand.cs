using System.Xml.Linq;

using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Impl.Commands;
using BBCAD.Cmnd.Impl.Parameters;

namespace BBCAD.Cmnd.Commands
{
    public class AddLineCommand : CommandBase
    {
        public ParamInt X1 { get; private set; } = new();
        public ParamInt Y1 { get; private set; } = new();
        public ParamInt X2 { get; private set; } = new();
        public ParamInt Y2 { get; private set; } = new();

        public AddLineCommand() : base(CommandType.AddLine, "ADD LINE")
        {
            AddParameter(new("X1", ParameterType.Integer, ObligationType.Mandatoty), X1);
            AddParameter(new("Y1", ParameterType.Integer, ObligationType.Mandatoty), Y1);
            AddParameter(new("X2", ParameterType.Integer, ObligationType.ContextBased), X2);
            AddParameter(new("Y2", ParameterType.Integer, ObligationType.ContextBased), Y2);
        }

        public AddLineCommand(XElement xe) : this() { this.XML = xe; }

        public override bool Consistent
        {
            get
            {
                if (!base.Consistent) { return false; }

                ConsistentState state = default;
                if (Parameters["X2"].Defined) state |= ConsistentState.X2;
                if (Parameters["Y2"].Defined) state |= ConsistentState.Y2;

                switch (state)
                {
                    case ConsistentState.X2:
                    case ConsistentState.Y2:
                        return true;

                    case ConsistentState.No:
                        return false;

                    case ConsistentState.P2:
                        return
                            Parameters["X2"] == Parameters["X1"]
                            ||
                            Parameters["Y2"] == Parameters["Y1"];

                    default: throw new NotImplementedException($"{state.GetType().Name}.{state}");
                }
            }
        }

        [Flags]
        private enum ConsistentState
        {
            No = 0b_00,
            X2 = 0b_01,
            Y2 = 0b_10,
            P2 = 0b_11,
        }
    }
}
