using BBCAD.Cmnd.Common;

namespace BBCAD.Cmnd.Commands
{
    public interface ICommand
    {
        string Name { get; }
        CommandType Type { get; }
        ParameterCollection Parameters { get; }
    }
}