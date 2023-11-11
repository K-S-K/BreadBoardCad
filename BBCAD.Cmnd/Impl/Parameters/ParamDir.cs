using BBCAD.Cmnd.Common;

namespace BBCAD.Cmnd.Impl.Parameters
{
    public class ParamDir : ParamBase
    {
        public Direction Value => 
            Enum.TryParse(Parameter.Value, out Direction v) ? v 
            : throw new Exception($"Can't parse");

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        public ParamDir() { }
    }
}
