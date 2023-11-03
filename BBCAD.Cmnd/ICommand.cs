using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Impl.Commands;

namespace BBCAD.Cmnd
{
    public interface ICommand
    {
        string Name { get; }
        CommandType Type { get; }
        ParameterCollection Parameters { get; }

        /// <summary>
        /// Checks if we have all mandatory parameters defined
        /// </summary>
        bool Consistent { get; }
    }
}