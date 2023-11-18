using BBCAD.Cmnd;
using BBCAD.Itself;

namespace BBCAD.Core
{
    public interface IBehavior
    {
        Board GetDemoBoard();

        Board ExecuteComandBatch(ICommandBatch batch);
        Board ExecuteComand(ICommand command);
    }
}
