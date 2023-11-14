using BBCAD.Itself;

namespace BBCAD.API.DTO
{
    public class BatchProcessingResponce
    {
        public Dictionary<Guid, BoardInfo> Boards { get; set; } = new();
        public string? Error { get; set; }

        public BatchProcessingResponce() { }
        public BatchProcessingResponce(string error) { Error = error; }
        public BatchProcessingResponce(IEnumerable<Board> boards)
        {
            foreach (Board board in boards)
            {
                Boards.Add(board.Id, new()
                {
                    Name = board.Name,
                    SixeX = board.SizeX,
                    SixeY = board.SizeY,
                    svg = board.SVG.ToString().Replace("xmlns=\"\" ", ""),
                    // Description=board.
                }
                );
            }
        }

        public class BoardInfo
        {
            public string Name { get; set; } = null!;
            // public string Description { get; set; }
            public int SixeX { get; set; }
            public int SixeY { get; set; }

            public string svg { get; set; } = null!;
        }
    }
}
