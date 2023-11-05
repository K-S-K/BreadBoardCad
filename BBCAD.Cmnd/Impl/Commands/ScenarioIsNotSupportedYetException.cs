namespace BBCAD.Cmnd.Impl.Commands
{
    public class ScenarioIsNotSupportedYetException : Exception
    {
        private ScenarioIsNotSupportedYetException(string message) : base(message) { }

        internal static ScenarioIsNotSupportedYetException SeveralNewBoards()
        {
            return new ScenarioIsNotSupportedYetException($"The batches with several board creating is not supported yet");
        }

        internal static Exception CloneBoard()
        {
            return new ScenarioIsNotSupportedYetException($"The batches with board cloning is not supported yet");
        }
    }
}
