using System.Xml.Linq;
using System.Text.Json;

using BBCAD.Cmnd.Impl.Commands;

namespace BBCAD.Cmnd
{
    /// <summary>
    /// The command type agnostic command 
    /// serialiser to / from XML and JSON
    /// </summary>
    public class CommandTransferObject
    {
        /// <summary>
        /// Command type name
        /// </summary>
        public string Type { get; set; } = "";

        /// <summary>
        /// Parameter values collection
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; } = new();

        /// <summary>
        /// Content representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"Type: {Type}, P.Count: {Parameters?.Count ?? 0}";

        /// <summary>
        /// Serialization to JSON
        /// </summary>
        /// <returns>Serialized command</returns>
        public string ToJson()
            => JsonSerializer.Serialize(this);

        /// <summary>
        /// Deerialization from JSON
        /// </summary>
        /// <param name="json">Serialized command</param>
        /// <returns></returns>
        public static CommandTransferObject? FromJson(string json)
            => JsonSerializer.Deserialize<CommandTransferObject>(json);

        /// <summary>
        /// Serialization to XML
        /// </summary>
        /// <returns>Serialized command</returns>
        public XElement ToXml()
            => new(CommandBase.XMLNodeName
                , new XAttribute(CommandBase.XMLAttrTypeName, Type)
                , Parameters.Select(p => new XAttribute(p.Key, p.Value)));

        /// <summary>
        /// Deerialization from XML
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static CommandTransferObject? FromXml(XElement xml)
            => new()
            {
                Type = xml.Attribute(CommandBase.XMLAttrTypeName)?.Value ?? string.Empty,
                Parameters = xml.Attributes()
                    .Where(a => a.Name != CommandBase.XMLAttrTypeName)
                    .ToList().GroupBy(a => a.Name.ToString())
                    .ToDictionary(b => b.Key, b => b.Select(c => c.Value).Single())
            };
    }

    public static class CommandTransferObjectExtensions
    {
        // Convert CommandTransferObject to Command
        public static ICommand ToCommand(this CommandTransferObject cto, ICommandFactory _commandFactory)
        {
            ICommand command;
            XElement xe;

            try
            {
                xe = cto.ToXml() ??
                throw new Exception($"Can't serialise command {cto} to XML"); ;
            }
            catch (Exception ex)
            {
                throw new Exception($"Can't serialise command {cto} to XML: {ex.Message}");
            }

            try
            {
                command = _commandFactory.DeserializeStatement(xe)
                ?? throw new Exception($"Can't deserialise from XML: {xe}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Can't deserialise command \"{cto}\" from XML: {xe} ({ex.Message})");
            }

            return command;
        }
    }
}
