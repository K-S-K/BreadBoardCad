using System.Xml.Linq;

using BBCAD.Cmnd.Impl.Commands;

namespace BBCAD.Cmnd.Impl.Scripts
{
    public class ScriptProcessor : IScriptProcessor
    {
        private readonly ICommandFactory _commandFactory;

        public ICommandBatch ExtractCommands(string script)
        {
            if (string.IsNullOrEmpty(script))
            {
                return new CommandBatch(Enumerable.Empty<ICommand>());
            }

            IEnumerable<ScriptLine> inputLines =
                script.Split(new string[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None).Select(x => new ScriptLine(x));

            bool multyLineComment = false;
            List<string> outputLines = new();
            foreach (ScriptLine line in inputLines)
            {
                if (line.Type == ScriptLineType.Empty)
                {
                    continue;
                }

                if (line.Type.HasFlag(ScriptLineType.FullyCommented))
                {
                    continue;
                }

                if (multyLineComment)
                {
                    if (line.Type.HasFlag(ScriptLineType.MultylineCommentEnd))
                    {
                        multyLineComment = false;
                    }
                    continue;
                }
                else
                {
                    if (line.Type.HasFlag(ScriptLineType.MultylineCommentStart))
                    {
                        multyLineComment = true;
                        continue;
                    }
                }

                outputLines.Add(line.ParsibleLine);
            }

            IEnumerable<ICommand> commands =
                outputLines.Select(x => _commandFactory.ParseStatement(x));

            return new CommandBatch(commands);
        }

        public ICommandBatch RestoreBatch(XElement xe)
        {
            if (xe.Name != CommandBatch.XMLNodeName)
            {
                throw CommandDeserializationException.WrongXmlElementName(xe, CommandBatch.XMLNodeName);
            }

            XElement xeCommands = xe.Element(CommandBatch.XMLCommandsCollectionName) ??
                throw new ArgumentException($"The XML does not contain element \"{CommandBatch.XMLCommandsCollectionName}\"");

            IEnumerable<ICommand> commands =
            xeCommands.Elements(CommandBase.XMLNodeName)
                .Select(xe => _commandFactory.DeserializeStatement(xe)).ToList();

            return new CommandBatch(commands);
        }

        public ScriptProcessor(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory ??
                throw new ArgumentNullException(nameof(commandFactory));
        }
    }
}
