namespace BBCAD.Cmnd.Impl.Parameters
{
    public class ParamGuid : ParamBase
    {
        public Guid Value =>
            Guid.TryParse(Parameter.Value, out Guid v) ? v
            : throw new Exception($"Can't parse");

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        public ParamGuid() { }
    }
}
