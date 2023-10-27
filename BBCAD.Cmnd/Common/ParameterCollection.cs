namespace BBCAD.Cmnd.Common
{
    /// <summary>
    /// The collection of the parameters of the command
    /// </summary>
    public class ParameterCollection
    {
        private readonly Dictionary<string, CommandParameter> commands = new();

        /// <summary>
        /// List of the parameters of the command
        /// </summary>
        public IEnumerable<CommandParameter> Items => commands.Values.ToArray();

        /// <summary>
        /// Checks if we have all mandatory parameters defined
        /// </summary>
        public bool Consistent => commands.Values
            .Where(x => x.Obligation == ObligationType.Mandatoty && !x.Defined).Any();

        /// <summary>
        /// Add a parameter to the collection.
        /// Must be called from the parameter constructor
        /// </summary>
        /// <param name="prm">Parameter</param>
        /// <exception cref="Exception">It is a developer level exceptions:
        /// - for the no-name parameter insertion attempt
        /// - for the parameter duplication attempt prevention
        /// </exception>
        /// <remarks>
        /// For inside usage only
        /// </remarks>
        internal void Add(CommandParameter prm)
        {
            string name = NormalizedName(prm.Name);

            lock (commands)
            {
                if (commands.ContainsKey(name))
                {
                    throw new Exception($"The command \"{prm}\" is already registered in the {nameof(ParameterCollection)}");
                }

                commands.Add(name, prm);
            }
        }

        /// <summary>
        /// Tty to get the parameter by it's name
        /// </summary>
        /// <param name="name">A parameter's name</param>
        /// <param name="cmnd">The parameter with given name</param>
        /// <returns>The result of the parameter find attempt</returns>
        public bool TryGetValue(string name, out CommandParameter? cmnd)
        {
            lock (commands)
            {
                if (commands.TryGetValue(NormalizedName(name), out cmnd))
                {
                    return true;
                }
                else
                {
                    cmnd = null;
                    return false;
                }
            }
        }


        /// <summary>
        /// Gets the normalized form of name for parsing
        /// </summary>
        /// <param name="originalName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string NormalizedName(string originalName)
        {
            string name = originalName.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("The command name wasn't provided");
            }

            return name;
        }
    }
}
