using BBCAD.Cmnd;
using BBCAD.Data;
using BBCAD.Itself;
using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Commands;
using BBCAD.Data.Exceptions;
using BBCAD.Cmnd.Impl.Parameters;

namespace BBCAD.Core
{
    public class CadCoreBehavior : IBehavior
    {
        private readonly IBoardStorage _boardStorage;
        private readonly ICommandFactory _commandFactory;

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

            if (batch.BatchContent == BatchContentBits.CreateLocalBoard)
            {
                if (batch[0] is not CreateBoardCommand cmnd)
                {
                    throw new Exception($"The first command must be {CommandType.CreateBoard}");
                }

                board = CreateBoard(cmnd);

                for (int i = 1; i < batch.Length; i++)
                {
                    ProcessExistingBoardCommand(board, batch[i]);
                }

                if (commit)
                {
                    _boardStorage.UpdateBoard(board);
                }
            }
            else if (batch.BatchContent == BatchContentBits.DealWithExternalBoard)
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

        public Board ExecuteComand(ICommand command)
        {
            if (!command.Consistent)
            {
                throw new Exception($"The command is not consistent: {command}");
            }

            return command switch
            {
                CreateBoardCommand createBoardCommand => CreateBoard(createBoardCommand),
                ResizeBoardCommand resizeBoardCommand => FindBoardAndProcessCommand(resizeBoardCommand.Id, resizeBoardCommand),
                _ => throw new NotImplementedException($"{command.CmndType}"),
            };
        }

        private Board FindBoardAndProcessCommand(ParamGuid id, ICommand command)
        {
            if (!id.IsConfigured)
            {
                throw new Exception($"This command must have a board ID: {command}");
            }

            Board board = _boardStorage.GetBoard(id.Value);

            ProcessExistingBoardCommand(board, command);

            return board;
        }

        private void ProcessExistingBoardCommand(Board board, ICommand command)
        {
            switch (command)
            {
                case ResizeBoardCommand resizeBoardCommand:
                    ResizeBoard(board, resizeBoardCommand);
                    break;


                default:
                    throw new NotImplementedException($"The \"{command.CmndType}\" command is not implemented yet");
            }
        }

        private void ResizeBoard(Board board, ResizeBoardCommand command)
        {
            if (!command.Consistent)
            {
                throw new Exception($"The command is not consistent: {command}");
            }

            board.SizeX = command.X.Value;
            board.SizeY = command.Y.Value;
        }

        private static Board CreateBoard(CreateBoardCommand command)
        {
            if (!command.Consistent)
            {
                throw new Exception($"The command is not consistent: {command}");
            }

            Board board = new()
            {
                Id = Guid.NewGuid(),
                Name = command.Name.Value,
                SizeX = command.X.Value,
                SizeY = command.Y.Value,
            };

            return board;
        }

        public CadCoreBehavior(IBoardStorage boardStorage, ICommandFactory commandFactory)
        {
            _boardStorage = boardStorage;
            _commandFactory = commandFactory;
        }
    }
}
