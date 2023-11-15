using BBCAD.Itself;

namespace BBCAD.API.DTO
{
    /// <summary>
    /// The batch responce structure
    /// </summary>
    public class BatchProcessingResponce
    {
        /// <summary>
        /// The list of boards returning by API methods
        /// </summary>
        public Dictionary<Guid, BoardInfo> Boards { get; set; } = new();

        /// <summary>
        /// The error description text
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// The batch responce structure
        /// </summary>
        public BatchProcessingResponce() { }

        /// <summary>
        /// The batch responce structure
        /// </summary>
        /// <param name="error">The error description text</param>
        public BatchProcessingResponce(string error) : this() { Error = error; }

        /// <summary>
        /// The batch responce structure
        /// </summary>
        /// <param name="board">The only board returning by API methods</param>
        public BatchProcessingResponce(Board board) : this(new Board[] { board }) { }

        /// <summary>
        /// The batch responce structure
        /// </summary>
        /// <param name="boards">The list of boards returning by API methods</param>
        public BatchProcessingResponce(IEnumerable<Board> boards) : this()
        {
            foreach (Board board in boards)
            {
                Boards.Add(board.Id, new()
                {
                    Name = board.Name,
                    SixeX = board.SizeX,
                    SixeY = board.SizeY,
                    Svg = board.SVG.ToString().Replace("xmlns=\"\" ", ""),
                    // Description=board.
                }
                );
            }
        }

        /// <summary>
        /// The board  metadata
        /// </summary>
        public class BoardInfo
        {
            /// <summary>
            /// The name of the board
            /// </summary>
            public string Name { get; set; } = null!;

            // public string Description { get; set; }

            /// <summary>
            /// The amount of X holes in the row
            /// </summary>
            public int SixeX { get; set; }

            /// <summary>
            /// The amount of X holes in the column
            /// </summary>
            public int SixeY { get; set; }

            /// <summary>
            /// The SVG image of the board
            /// </summary>
            public string Svg { get; set; } = null!;
        }
    }
}
