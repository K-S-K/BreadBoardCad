using BBCAD.Data;
using BBCAD.Itself;
using BBCAD.Data.Exceptions;

namespace BBCAD.Core
{
    public class CadCoreBehavior : IBehavior
    {
        private readonly IBoardStorage _boardStorage;

        public Board GetDemoBoard()
        {
            try
            {
                return _boardStorage.GetBoard(Guid.Empty);
            }
            catch (BoardNotFoundException)
            {
                Board board = Board.Sample;
                try
                {
                    _boardStorage.RegisterBoard(board);
                }
                catch (BoardAlreadyRegisteredException) {; }
                return board;
            }
        }

        public CadCoreBehavior(IBoardStorage boardStorage)
        {
            _boardStorage = boardStorage;
        }
    }
}