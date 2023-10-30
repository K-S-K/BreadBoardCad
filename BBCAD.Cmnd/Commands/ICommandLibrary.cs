using BBCAD.Cmnd.Common;

namespace BBCAD.Cmnd.Commands
{
    public interface ICommandLibrary
    {
        IEnumerable<ICommand> Commands { get; }

        bool TryGetValue(CommandType type, out ICommand? cmnd);
    }
}
