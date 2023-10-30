namespace BBCAD.Cmnd.Common
{
    /// <summary>
    /// The parameters of the command
    /// </summary>
    public class CommandParameter
    {
        /// <summary>
        /// The name of the parameter
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The value of the parameter
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// The type of the parameter
        /// </summary>
        public ParameterType Type { get; private set; }

        /// <summary>
        /// The description of the parameter
        /// </summary>
        public string Description { get; internal set; } = string.Empty;

        /// <summary>
        /// Is the parameter mandatory or optional
        /// </summary>
        public ObligationType Obligation { get; private set; }

        /// <summary>
        /// Is parameter value defined
        /// </summary>
        public bool Defined => !string.IsNullOrEmpty(Value);

        public override string ToString() => Type.MustBeQuoted() ? $"{Name} = \"{Value}\"" : $"{Name} = {Value}";

        /// <summary>
        /// The parameter of the command
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="type">The type of the parameter</param>
        /// <param name="obligation">Is the parameter mandatory or optional</param>
        public CommandParameter(string name, ParameterType type, ObligationType obligation)
        {
            Name = name;
            Type = type;
            Obligation = obligation;
        }
    }
}
