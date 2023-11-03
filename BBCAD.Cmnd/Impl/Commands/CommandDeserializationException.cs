using System.Xml.Linq;

namespace BBCAD.Cmnd.Impl.Commands
{
    public class CommandDeserializationException : Exception
    {
        private CommandDeserializationException(string message) : base(message) { }

        public static CommandDeserializationException WrongXmlElementName(XElement xml, string expectedName)
        {
            return new CommandDeserializationException($"Expected xml element name is \"{expectedName}\", expected value is {xml}");
        }

        public static CommandDeserializationException WrongTypeFromXml(string actualType, string expectedType)
        {
            return new CommandDeserializationException($"Expected command type is \"{expectedType}\", actual is {actualType}");
        }

        public static CommandDeserializationException CommandTypeIsNotDefinedInXML(XElement xml)
        {
            return new CommandDeserializationException($"Command type is not defined in xml element {xml}");
        }
    }
}
