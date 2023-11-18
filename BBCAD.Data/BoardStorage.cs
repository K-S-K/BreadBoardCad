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
        /// The index of the boards ownership
        /// </summary>
        private readonly Dictionary<Guid, List<Guid>> _index = new();

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

                #region -> Ownership
                {
                    if (!_index.TryGetValue(board.User, out List<Guid>? index))
                    {
                        index = new List<Guid>();
                        _index.Add(board.User, index);
                    }

                    index.Add(board.Id);
                }
                #endregion
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
                if (_boards.TryGetValue(board.Id, out Board? storedBoard))
                {
                    /// Ib a board ever can be transferred 
                    /// to another user, it must be done here.
                    /// 

                    // Update the board in the storage
                    storedBoard.XML = board.XML;
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
                if (_boards.TryGetValue(id, out Board? board))
                {

                    #region -> Ownership
                    {
                        if (_index.TryGetValue(board.User, out List<Guid>? index))
                        {
                            index.Remove(board.Id);

                            if (!index.Any())
                            {
                                _index.Remove(board.User);
                            }
                        }
                    }
                    #endregion

                    _boards.Remove(id);
                }
            }
        }

        /// <summary>
        /// Get board by Board Id
        /// </summary>
        /// <param name="BoardId">Board Id</param>
        /// <returns>Board with specified Id</returns>
        /// <exception cref="BoardNotFoundException"></exception>
        public Board GetBoard(Guid BoardId)
        {
            lock (_boards)
            {
                if (_boards.TryGetValue(BoardId, out Board? board))
                {
                    return board;
                }
                else
                {
                    throw new BoardNotFoundException(BoardId);
                }
            }
        }

        /// <summary>
        /// Get board list by User Id
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <returns>Board belongs to user with specified Id</returns>
        /// <exception cref="BoardNotFoundException"></exception>
        public IEnumerable<Board> GetBoards(Guid UserId)
        {
            if (_index.TryGetValue(UserId, out List<Guid>? index))
            {
                foreach (Guid boardId in index)
                {
                    if (_boards.TryGetValue(boardId, out Board? board))
                    {
                        yield return board;
                    }
                }
            }
        }

        public BoardStorage() { }
    }
}
