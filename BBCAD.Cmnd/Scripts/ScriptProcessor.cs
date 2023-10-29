using BBCAD.Cmnd.Commands;

namespace BBCAD.Cmnd.Scripts
{
    public class ScriptProcessor : IScriptProcessor
    {
        private readonly ICommandLibrary _commandLibrary;

        public IEnumerable<ICommand> ExtractCommands(string script)
        {
            List<ICommand> commands = new();

            // TODO: parse script and extract commands

            return commands;
        }

        public ScriptProcessor(ICommandLibrary commandLibrary)
        {
            _commandLibrary = commandLibrary ?? throw new ArgumentNullException(nameof(commandLibrary));
        }
    }
}
