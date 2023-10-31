using System.Xml.Linq;
using System.Text.RegularExpressions;

using BBCAD.Cmnd.Common;

namespace BBCAD.Cmnd.Commands
{
    /// <summary>
    /// Board design command
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        private const string XMLNodeName = "Command";
        /// <summary>
        /// The type of the command
        /// </summary>
        public CommandType Type { get; private set; }

        /// <summary>
        /// The name of the command used in the CLI
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The list of the parameters of the command
        /// </summary>
        public ParameterCollection Parameters { get; private set; }

        /// <summary>
        /// The XML representation of the command
        /// </summary>
        public XElement XML
        {
            get
            {
                if (!Parameters.Consistent)
                {
                    throw new InvalidOperationException("The command is not consistent");
                }

                return new XElement(XMLNodeName,
                    new XAttribute("type", Type), Parameters.XMLAttributes);
            }
            set
            {
                if (value.Name != XMLNodeName)
                {
                    throw CommandDeserializationException.WrongXmlElementName(value, XMLNodeName);
                }

                XAttribute? xaType = value.Attribute("type");
                if (xaType == null)
                {
                    throw CommandDeserializationException.CommandTypeIsNotDefinedInXML(value);

                }

                if (xaType.Value != Type.ToString())
                {
                    throw CommandDeserializationException.WrongTypeFromXml(xaType.Value, Type.ToString());
                }

                Parameters.XMLAttributes = value.Attributes();
            }
        }

        public void Parse(string statement)
        {
            int ixTrim = 0;
            string patternCmnd = @"([\w\s]+)\s+\w+\s*=.*$";
            string patternArgs = "(\\b\\w*\\s)\\s*=\\s*((\\b\\w*)|(\"(\\b\\w*\\s*)*\"))";

            MatchCollection collectionCmnd = Regex.Matches(statement, patternCmnd);
            foreach (Match match in collectionCmnd)
            {
                var v = match.Groups[1].Value;
            }
            if (collectionCmnd.Count == 1)
            {
                if (collectionCmnd[0].Groups.Count == 2)
                {
                    string cmndLine = collectionCmnd[0].Groups[1].Value.Trim().ToUpper();
                    ixTrim = collectionCmnd[0].Groups[1].Length;
                    if (cmndLine != Name)
                    {
                        throw new Exception($"The command line \"{cmndLine}\" instead of \"{Name}\"");
                    }
                }
            }

            string statementArgs = statement.Substring(ixTrim);

            MatchCollection collectionArgs = Regex.Matches(statementArgs, patternArgs);
            foreach (Match match in collectionArgs)
            {
                var v = match.Groups[1].Value;
            }
        }

        public override string ToString() => $"{Name} {string.Join(" ", Parameters.Items)}";

        /// <summary>
        /// Board design command
        /// </summary>
        /// <param name="type">The type of the command</param>
        /// <param name="name">The name of the command used in the CLI</param>
        protected CommandBase(CommandType type, string name)
        {
            Type = type;
            Name = name;
            Parameters = new();
        }
    }
}
