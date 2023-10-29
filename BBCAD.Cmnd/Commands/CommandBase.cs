using BBCAD.Cmnd.Common;

namespace BBCAD.Cmnd.Commands
{
    /// <summary>
    /// Board design command
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        /// The type of the command
        /// </summary>
        public CommandType Type { get; private set; }

        /// <summary>
        /// The name of the command used in the CLI
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The list of the parameters of the command
        /// </summary>
        public ParameterCollection Parameters { get; private set; }


        /// <summary>
        /// Board design command
        /// </summary>
        /// <param name="type">The type of the command</param>
        /// <param name="name">The name of the command used in the CLI</param>
        protected CommandBase(CommandType type, string name)
        {
            Type = type;
            Name = name;
            Parameters = new();
        }
    }
}
