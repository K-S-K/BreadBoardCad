namespace BBCAD.Cmnd.Impl.Parameters
{
    public class ParamInt : ParamBase
    {
        public int Value => 
            int.TryParse(Parameter.Value, out int v) ? v 
            : throw new Exception($"Can't parse");

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        public ParamInt() { }
    }
}
