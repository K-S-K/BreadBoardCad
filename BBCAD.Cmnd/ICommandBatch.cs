using System.Xml.Linq;

using BBCAD.Cmnd.Common;

namespace BBCAD.Cmnd
{
    public interface ICommandBatch
    {
        ICommand this[int i] { get; }

        BatchContentBits BatchContent { get; }
        int Length { get; }
        IEnumerable<ICommand> Commands { get; }

        XElement XML { get; }

        Guid GetExternalBoardGuid();

        string ToString();
    }
}
