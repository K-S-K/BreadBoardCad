using System.Xml.Linq;

namespace BBCAD.Cmnd
{
    public interface IScriptProcessor
    {
        ICommandBatch ExtractCommands(string script);
        ICommandBatch RestoreBatch(XElement xe);
    }
}