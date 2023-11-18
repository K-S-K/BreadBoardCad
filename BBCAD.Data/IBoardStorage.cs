using BBCAD.Itself;

namespace BBCAD.Data
{
    public interface IBoardStorage
    {
        /// <summary>
        /// Add the given board to the database
        /// </summary>
        /// <param name="board">Board itself</param>
        /// <exception cref="BoardAlreadyRegisteredException"></exception>
        void RegisterBoard(Board board);

        /// <summary>
        /// Update the board in the database
        /// </summary>
        /// <param name="board">Board to be updated</param>
        /// <exception cref="BoardNotFoundException"></exception>
        void UpdateBoard(Board board);

        /// <summary>
        /// Remove a board from the database by the given id
        /// </summary>
        /// <param name="id">Board id</param>
        void RemoveBoard(Guid id);

        /// <summary>
        /// Get board by Board Id
        /// </summary>
        /// <param name="BoardId">Board Id</param>
        /// <returns>Board with specified Id</returns>
        /// <exception cref="BoardNotFoundException"></exception>
        Board GetBoard(Guid BoardId);

        /// <summary>
        /// Get board list by User Id
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <returns>Board belongs to user with specified Id</returns>
        /// <exception cref="BoardNotFoundException"></exception>
        IEnumerable<Board> GetBoards(Guid UserId);
    }
}
