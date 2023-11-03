namespace BBCAD.Cmnd
{
    public interface IScriptProcessor
    {
        IEnumerable<ICommand> ExtractCommands(string script);
    }
}