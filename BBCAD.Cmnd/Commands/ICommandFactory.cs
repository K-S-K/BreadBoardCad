namespace BBCAD.Cmnd.Commands
{
    public interface ICommandFactory
    {
        void AddCommand(ICommand cmnd);
        ICommand ParseStatement(string statement);
    }
}