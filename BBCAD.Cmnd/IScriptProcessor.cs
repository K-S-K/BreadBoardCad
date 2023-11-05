namespace BBCAD.Cmnd
{
    public interface IScriptProcessor
    {
        ICommandBatch ExtractCommands(string script);
    }
}