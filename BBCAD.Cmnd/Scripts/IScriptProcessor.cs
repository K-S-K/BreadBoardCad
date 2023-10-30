using BBCAD.Cmnd.Commands;

namespace BBCAD.Cmnd.Scripts
{
    public interface IScriptProcessor
    {
        IEnumerable<ICommand> ExtractCommands(string script);
    }
}