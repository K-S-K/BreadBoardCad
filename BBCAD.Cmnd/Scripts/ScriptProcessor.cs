using BBCAD.Cmnd.Commands;

namespace BBCAD.Cmnd.Scripts
{
    public class ScriptProcessor : IScriptProcessor
    {
        private readonly ICommandFactory _commandFactory;

        public IEnumerable<ICommand> ExtractCommands(string script)
        {
            if (string.IsNullOrEmpty(script))
            {
                return Enumerable.Empty<ICommand>();
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

            return commands;
        }

        public ScriptProcessor(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory ??
                throw new ArgumentNullException(nameof(commandFactory));
        }
    }
}
