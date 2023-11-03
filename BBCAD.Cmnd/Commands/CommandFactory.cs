using BBCAD.Cmnd.Common;
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
                _ => throw new NotImplementedException(
                    $"{cmndType.GetType().Name}.{cmndType}"),
            };

            cmnd.Parameters.SetValues(inputValues);

            return cmnd;
        }

        public void AddCommand(ICommand cmnd)
        {
            if (_typeData.ContainsKey(cmnd.Name))
            {
                throw new Exception($"The command name\"{cmnd.Name}\" is already registered in the {nameof(CommandLibrary)}");
            }

            _typeData.Add(cmnd.Name, cmnd.Type);
        }
    }
}
