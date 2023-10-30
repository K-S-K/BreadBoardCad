using BBCAD.Cmnd.Common;

namespace BBCAD.Cmnd.Commands
{
    public class CommandLibrary : ICommandLibrary
    {
        private readonly Dictionary<CommandType, ICommand> _commands = new();

        public IEnumerable<ICommand> Commands => _commands.Values.ToList();


        private void AddCommand(ICommand cmnd)
        {
            if (_commands.ContainsKey(cmnd.Type))
            {
                throw new Exception($"The command \"{cmnd}\" is already registered in the {nameof(CommandLibrary)}");
            }

            _commands.Add(cmnd.Type, cmnd);
        }

        public bool TryGetValue(CommandType type, out ICommand? cmnd)
        {
            if (_commands.TryGetValue(type, out cmnd))
            {
                return true;
            }
            else
            {
                cmnd = null;
                return false;
            }
        }


        public CommandLibrary()
        {
            AddCommand(new CreateBoardCommand());
            AddCommand(new CloneBoardCommand());
            AddCommand(new ResizeBoardCommand());
        }
    }
}
