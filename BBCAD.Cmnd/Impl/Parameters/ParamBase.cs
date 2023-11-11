namespace BBCAD.Cmnd.Impl.Parameters
{
    public abstract class ParamBase
    {
        protected CommandParameter Parameter { get; private set; } = null!;

        public string Name => Parameter.Name ?? string.Empty;

        internal void Init(CommandParameter parameter)
        {
            this.Parameter = parameter;
        }
    }
}
