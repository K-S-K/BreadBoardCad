namespace BBCAD.Cmnd
{
    public interface ICommandFactory
    {
        ICommand ParseStatement(string statement);
    }
}