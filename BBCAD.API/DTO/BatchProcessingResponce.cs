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
        /// <param name="condition">The responce content specifier</param>
        public BatchProcessingResponce(Board board, Condition condition) : this(new Board[] { board }, condition) { }

        /// <summary>
        /// The batch responce structure
        /// </summary>
        /// <param name="boards">The list of boards returning by API methods</param>
        /// <param name="condition">The responce content specifier</param>
        public BatchProcessingResponce(IEnumerable<Board> boards, Condition condition) : this()
        {
            foreach (Board board in boards)
            {
                Boards.Add(board.Id, new()
                {
                    Name = board.Name,
                    SizeX = board.SizeX,
                    SizeY = board.SizeY,
                    Description = board.Description,
                    User = board.User,
                    Svg = condition == Condition.Complete ?
                    board.SVG.ToString().Replace("xmlns=\"\" ", "") : null,
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

            /// <summary>
            /// The amount of X holes in the row
            /// </summary>
            public int SizeX { get; set; }

            /// <summary>
            /// The amount of X holes in the column
            /// </summary>
            public int SizeY { get; set; }

            /// <summary>
            /// The Id of the User who the board belongs to
            /// </summary>
            public Guid User { get; set; } = Guid.Empty;

            /// <summary>
            /// The description of the board
            /// </summary>
            public string? Description { get; set; }

            /// <summary>
            /// The SVG image of the board
            /// </summary>
            public string? Svg { get; set; }
        }

        /// <summary>
        /// The responce content specifier
        /// </summary>
        [Flags]
        public enum Condition
        {
            /// <summary>
            /// Whole board content
            /// </summary>
            Complete = 0b11,

            /// <summary>
            /// Only metadata without content
            /// </summary>
            Metadata = 0b01,
        }
    }
}
