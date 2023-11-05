using System.Xml.Linq;

namespace BBCAD.Cmnd
{
    public interface ICommandFactory
    {
        ICommand ParseStatement(string statement);
        ICommand DeserializeStatement(XElement xe);
    }
}