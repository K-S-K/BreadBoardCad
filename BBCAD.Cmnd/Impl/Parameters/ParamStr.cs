namespace BBCAD.Cmnd.Impl.Parameters
{
    public class ParamStr : ParamBase
    {
        public string Value => Parameter.Value ?? string.Empty;

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        public ParamStr() { }
    }
}
