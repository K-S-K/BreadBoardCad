namespace BBCAD.Data.Exceptions
{
    public class BoardNotFoundException : Exception
    {
        public readonly Guid BoardId;
        public BoardNotFoundException(Guid id) : base(FormatMessageWithId(id)) { BoardId = id; }
        public BoardNotFoundException(Guid id, string reason) : base($"{FormatMessageWithId(id)} {reason}") { BoardId = id; }
        public BoardNotFoundException(Guid id, Exception innerException) : base(FormatMessageWithId(id), innerException) { BoardId = id; }

        private static string FormatMessageWithId(Guid id) => $"The board [{id.ToString().ToUpper()}] was not found.";
    }
}
