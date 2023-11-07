using BBCAD.Cmnd;
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

        public Board ExecuteComandBatch(ICommandBatch batch, bool commit = false)
        {
            Board board;

            if (batch == null)
            {
                throw new Exception($"The {nameof(batch)} was not provided to {nameof(ExecuteComandBatch)} method");
            }

            if (batch.BatchContent == Cmnd.Common.BatchContentBits.CreateLocalBoard)
            {
                board = CreateBoard(batch[0]);

                for (int i = 1; i < batch.Length; i++)
                {
                    ProcessBoardCommand(board, batch[i]);
                }

                if (commit)
                {
                    _boardStorage.UpdateBoard(board);
                }
            }
            else if (batch.BatchContent == Cmnd.Common.BatchContentBits.DealWithExternalBoard)
            {
                try
                {
                    board = _boardStorage.GetBoard(batch.GetExternalBoardGuid());
                }
                catch (BoardNotFoundException)
                {
                    throw new Exception($"Can't found the board {{{batch.GetExternalBoardGuid().ToString().ToUpper()}}}.");
                }
            }

            else
            {
                throw new Exception("Inconsistent batch: it mist be local or external deal");
            }

            return board;
        }

        private void ProcessBoardCommand(Board board, ICommand command)
        {
            switch (command.Type)
            {
                case Cmnd.Common.CommandType.ResizeBoard:
                    ResizeBoard(board, command); break;

                default:
                    throw new NotImplementedException($"The \"{command.Type}\" command is not implemented yet");
            }
        }

        private void ResizeBoard(Board board, ICommand command)
        {
            if (
                !int.TryParse(command.Parameters["X"].Value, out int x)
                ||
                !int.TryParse(command.Parameters["Y"].Value, out int y)
                )
            {
                throw new Exception($"Can't parse parameter from the command {command}");
            }

            board.SizeX = x;
            board.SizeY = y;
        }

        private static Board CreateBoard(ICommand command)
        {
            if (
                !int.TryParse(command.Parameters["X"].Value, out int x)
                ||
                !int.TryParse(command.Parameters["Y"].Value, out int y)
                )
            {
                throw new Exception($"Can't parse parameter from the command {command}");
            }

            Board board = new()
            {
                Id = Guid.NewGuid(),
                Name = command.Parameters["Name"].Value,
                SizeX = x,
                SizeY = y,
            };

            return board;
        }

        public CadCoreBehavior(IBoardStorage boardStorage)
        {
            _boardStorage = boardStorage;
        }
    }
}
