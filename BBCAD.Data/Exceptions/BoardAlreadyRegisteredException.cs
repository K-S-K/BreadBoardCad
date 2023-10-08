namespace BBCAD.Data.Exceptions
{
    public class BoardAlreadyRegisteredException : Exception
    {
        public readonly Guid BoardId;
        public BoardAlreadyRegisteredException(Guid id) : base(FormatMessageWithId(id)) { BoardId = id; }

        private static string FormatMessageWithId(Guid id) => $"The board [{id.ToString().ToUpper()}] is alreadyRegistered in the database";
    }
}
