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
        /// Remove a board from the database by the given id
        /// </summary>
        /// <param name="id">Board id</param>
        void RemoveBoard(Guid id);

        /// <summary>
        /// Get board by id
        /// </summary>
        /// <param name="id">Board id</param>
        /// <returns></returns>
        /// <exception cref="BoardNotFoundException"></exception>
        Board GetBoard(Guid id);
    }
}