using System.Xml.Linq;

using BBCAD.Cmnd.Common;
namespace BBCAD.Cmnd
{
    public interface ICommand
    {
        string CmndName { get; }
        CommandType CmndType { get; }

        XElement XML { get; }

        /// <summary>
        /// Checks if we have all mandatory parameters defined
        /// </summary>
        bool Consistent { get; }
    }
}
