using System.Xml.Linq;

using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Impl.Commands;
using System.Text.RegularExpressions;

namespace BBCAD.Cmnd.Commands
{
    public partial class CommandFactory : ICommandFactory
    {
        private readonly Dictionary<string, CommandType> _typeData = new();

        [GeneratedRegex("\\s+")]
        private static partial Regex RxNormalization();

        [GeneratedRegex(@"(?<cmnd>[\w\s]+)\s*(?<args>\s+\w+\s*=.*)$")]
        private static partial Regex RxExtractCmnd();

        [GeneratedRegex("(?<prm>\\b\\w*)\\s*=\\s*(?<val>(\\b\\w*)|(\"(\\b\\w*\\s*)*\"))")]
        private static partial Regex RxExtractArgs();

        public ICommand DeserializeStatement(XElement xe)
        {
            if (xe == null)
            {
                throw new ArgumentNullException("Nothing to deserialize");
            }

            if (xe.Name != CommandBase.XMLNodeName)
            {
                throw new ArgumentException($"Wrong {nameof(XElement)} name \"{xe.Name}\" instead of \"{CommandBase.XMLNodeName}\"");
            }

            string cmndName = xe.Attribute(CommandBase.XMLAttrTypeName)?.Value ?? throw new ArgumentException($"Command does not contain \"{CommandBase.XMLAttrTypeName}\" attribute: {xe}");

            if (!Enum.TryParse(cmndName, out CommandType cmndType) || cmndType == default)
            {
                throw new Exception($"There is no command which can be associated with the type name \"{cmndName}\"");
            }

            ICommand cmnd = cmndType switch
            {
                CommandType.CreateBoard => new CreateBoardCommand(xe),
                CommandType.ResizeBoard => new ResizeBoardCommand(xe),
                CommandType.CloneBoard => new CloneBoardCommand(xe),
                CommandType.AddLine => new AddLineCommand(xe),
                _ => throw new NotImplementedException(
                    $"{cmndType.GetType().Name}.{cmndType}"),
            };

            return cmnd;
        }

        public ICommand ParseStatement(string statement)
        {
            MatchCollection statementGroups = RxExtractCmnd().Matches(statement);

            var cmndGroup = statementGroups.Select(x => x.Groups["cmnd"]).FirstOrDefault() ??
                throw new Exception($"There is no command found in the statement: {statement}");

            var argsGroup = statementGroups.Select(x => x.Groups["args"]).FirstOrDefault() ??
                throw new Exception($"There is no any argument found in the statement: {statement}");

            string cmndLine = RxNormalization().Replace(cmndGroup.Value, " ").Trim();
            string cmndArgs = argsGroup.Value;

            if (!_typeData.TryGetValue(cmndLine, out var cmndType) || cmndType == default)
            {
                throw new Exception($"There is no command which can be associated with the statement: {statement}");
            }

            Dictionary<string, string> inputValues = new();

            MatchCollection collectionArgs = RxExtractArgs().Matches(cmndArgs);
            foreach (Match match in collectionArgs.Cast<Match>())
            {
                GroupCollection groups = match.Groups;
                inputValues[groups["prm"].Value.ToUpper()] = groups["val"].Value.Trim('\"');
            }

            ICommand cmnd = cmndType switch
            {
                CommandType.CreateBoard => new CreateBoardCommand(),
                CommandType.ResizeBoard => new ResizeBoardCommand(),
                CommandType.CloneBoard => new CloneBoardCommand(),
                CommandType.AddLine => new AddLineCommand(),
                _ => throw new NotImplementedException(
                    $"{cmndType.GetType().Name}.{cmndType}"),
            };

            (cmnd as CommandBase)?.Parameters.SetValues(inputValues);

            return cmnd;
        }

        internal void AddCommand(ICommand cmnd)
        {
            if (_typeData.ContainsKey(cmnd.CmndName))
            {
                throw new Exception($"The command name\"{cmnd.CmndName}\" is already registered in the {nameof(CommandLibrary)}");
            }

            _typeData.Add(cmnd.CmndName, cmnd.CmndType);
        }
    }
}
