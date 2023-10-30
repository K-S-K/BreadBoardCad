using System.Xml.Linq;
using BBCAD.Cmnd.Common;

namespace BBCAD.Cmnd.Commands
{
    /// <summary>
    /// The collection of the parameters of the command
    /// </summary>
    public class ParameterCollection
    {
        private readonly Dictionary<string, CommandParameter> _parameters = new();

        /// <summary>
        /// List of the parameters of the command
        /// </summary>
        public IEnumerable<CommandParameter> Items => _parameters.Values.ToArray();

        /// <summary>
        /// Checks if we have all mandatory parameters defined
        /// </summary>
        public bool Consistent => !_parameters.Values
            .Where(x => x.Obligation == ObligationType.Mandatoty && !x.Defined).Any();

        /// <summary>
        /// Try to get the parameter by it's name
        /// </summary>
        /// <param name="name">A parameter's name</param>
        /// <param name="prm">The parameter with given name</param>
        /// <returns>The result of the parameter find attempt</returns>
        public bool TryGetValue(string name, out CommandParameter? prm)
        {
            lock (_parameters)
            {
                if (_parameters.TryGetValue(NormalizedName(name), out prm))
                {
                    return true;
                }
                else
                {
                    prm = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Get the parameter by it's name
        /// </summary>
        /// <param name="name">A parameter's name</param>
        /// <returns>The parameter that was foun by the given name</returns>
        /// <exception cref="Exception">Throws if a parameter wasn't found</exception>
        public CommandParameter this[string name]
        {
            get
            {
                if (TryGetValue(name, out var prm) && prm != null)
                {
                    return prm;
                }
                else
                {
                    throw new Exception($"The parameter \"{name}\" not found");
                }
            }
        }

        /// <summary>
        /// The list of defined command's parameters in the XAttribute form
        /// </summary>
        internal IEnumerable<XAttribute> XMLAttributes
        {
            get
            {
                foreach (CommandParameter prm in _parameters.Values
                    .Where(x => x.Defined))
                {
                    if (prm.Value != null)
                    {
                        XAttribute attr = new(prm.Name, prm.Value);
                        yield return attr;
                    }
                }
            }
            set
            {
                foreach (XAttribute attr in value)
                {
                    if (TryGetValue(attr.Name.LocalName, out CommandParameter? prm))
                    {
                        if (prm != null)
                        {
                            prm.Value = attr.Value;
                        }
                    }
                }
            }
        }

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

            lock (_parameters)
            {
                if (_parameters.ContainsKey(name))
                {
                    throw new Exception($"The command \"{prm}\" is already registered in the {nameof(ParameterCollection)}");
                }

                _parameters.Add(name, prm);
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
