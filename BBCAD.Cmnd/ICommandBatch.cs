using BBCAD.Cmnd.Common;

namespace BBCAD.Cmnd
{
    public interface ICommandBatch
    {
        ICommand this[int i] { get; }

        BatchContentBits BatchContent { get; }
        int Length { get; }
        IEnumerable<ICommand> Commands { get; }

        Guid GetExternalBoardGuid();

        string ToString();
    }
}