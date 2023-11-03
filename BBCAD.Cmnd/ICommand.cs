using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Impl.Commands;

namespace BBCAD.Cmnd
{
    public interface ICommand
    {
        string Name { get; }
        CommandType Type { get; }
        ParameterCollection Parameters { get; }
    }
}