using System.Xml.Linq;

using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Impl.Commands;

namespace BBCAD.Cmnd
{
    public interface ICommand
    {
        string Name { get; }
        CommandType Type { get; }
        ParameterCollection Parameters { get; }

        XElement XML { get; }

        /// <summary>
        /// Checks if we have all mandatory parameters defined
        /// </summary>
        bool Consistent { get; }
    }
}