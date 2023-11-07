using BBCAD.Itself;
using BBCAD.Data.Exceptions;

namespace BBCAD.Data
{
    public class BoardStorage : IBoardStorage
    {
        /// <summary>
        /// The cache of loaded boards
        /// </summary>
        private readonly Dictionary<Guid, Board> _boards = new();

        /// <summary>
        /// Add the given board to the database
        /// </summary>
        /// <param name="board">Board itself</param>
        /// <exception cref="BoardAlreadyRegisteredException"></exception>
        public void RegisterBoard(Board board)
        {
            lock (_boards)
            {
                if (_boards.ContainsKey(board.Id))
                {
                    throw new BoardAlreadyRegisteredException(board.Id);
                }

                _boards.Add(board.Id, board);
            }
        }

        /// <summary>
        /// Update the board in the database
        /// </summary>
        /// <param name="board">Board to be updated</param>
        /// <exception cref="BoardNotFoundException"></exception>
        public void UpdateBoard(Board board)
        {
            lock (_boards)
            {
                if (_boards.ContainsKey(board.Id))
                {
                    // It is nothing to to actually in this implementation.
                    // But if we have some dtorage, board must be updated there.
                }
                else
                {
                    throw new BoardNotFoundException(board.Id);
                }
            }
        }

        /// <summary>
        /// Remove a board from the database by the given id
        /// </summary>
        /// <param name="id">Board id</param>
        public void RemoveBoard(Guid id)
        {
            lock (_boards)
            {
                _boards.Remove(id);
            }
        }

        /// <summary>
        /// Get board by id
        /// </summary>
        /// <param name="id">Board id</param>
        /// <returns></returns>
        /// <exception cref="BoardNotFoundException"></exception>
        public Board GetBoard(Guid id)
        {
            lock (_boards)
            {
                if (_boards.TryGetValue(id, out Board? board))
                {
                    return board;
                }
                else
                {
                    throw new BoardNotFoundException(id);
                }
            }
        }

        public BoardStorage() { }
    }
}
