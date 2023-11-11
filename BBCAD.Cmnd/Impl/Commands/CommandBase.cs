using System.Xml.Linq;
using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Impl.Parameters;

namespace BBCAD.Cmnd.Impl.Commands
{
    /// <summary>
    /// Board design command
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        internal const string XMLNodeName = "Command";
        internal const string XMLAttrTypeName = "type";
        /// <summary>
        /// The type of the command
        /// </summary>
        public CommandType CmndType { get; private set; }

        /// <summary>
        /// The name of the command used in the CLI
        /// </summary>
        public string CmndName { get; private set; }

        /// <summary>
        /// The list of the parameters of the command
        /// </summary>
        internal ParameterCollection Parameters { get; private set; }

        /// <summary>
        /// Checks if we have all mandatory parameters defined
        /// </summary>
        public virtual bool Consistent => Parameters.Consistent;

        /// <summary>
        /// The XML representation of the command
        /// </summary>
        public XElement XML
        {
            get
            {
                if (!Consistent)
                {
                    throw new InvalidOperationException("The command is not consistent");
                }

                return new XElement(XMLNodeName,
                    new XAttribute(XMLAttrTypeName, CmndType), Parameters.XMLAttributes);
            }
            set
            {
                if (value.Name != XMLNodeName)
                {
                    throw CommandDeserializationException.WrongXmlElementName(value, XMLNodeName);
                }

                XAttribute? xaType = value.Attribute(XMLAttrTypeName);
                if (xaType == null)
                {
                    throw CommandDeserializationException.CommandTypeIsNotDefinedInXML(value);

                }

                if (xaType.Value != CmndType.ToString())
                {
                    throw CommandDeserializationException.WrongTypeFromXml(xaType.Value, CmndType.ToString());
                }

                Parameters.XMLAttributes = value.Attributes();
            }
        }

        protected void AddParameter(CommandParameter parameter, ParamBase pb)
        {
            Parameters.Add(parameter);
            pb.Init(parameter);
        }

        public override string ToString() => $"{CmndName} {string.Join(" ", Parameters.Items)}";

        /// <summary>
        /// Board design command
        /// </summary>
        /// <param name="type">The type of the command</param>
        /// <param name="name">The name of the command used in the CLI</param>
        protected CommandBase(CommandType type, string name)
        {
            CmndType = type;
            CmndName = name;
            Parameters = new();
        }
    }
}
