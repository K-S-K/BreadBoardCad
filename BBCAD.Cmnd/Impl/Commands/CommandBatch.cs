using BBCAD.Cmnd.Common;

namespace BBCAD.Cmnd.Impl.Commands
{
    public class CommandBatch : ICommandBatch
    {
        #region -> Data
        private readonly Dictionary<int, ICommand> _items = new();
        #endregion


        #region -> Properties
        public IEnumerable<ICommand> Commands => _items.Values;

        public int Length => _items.Count;

        public BatchContentBits BatchContent { get; private set; } = default;

        public ICommand this[int i]
        {
            get
            {
                if (_items.TryGetValue(i, out var cmnd) && cmnd != null)
                {
                    return cmnd;
                }
                else
                {
                    throw new Exception($"The command [{i}] not found");
                }
            }
        }
        #endregion


        #region -> Methods
        public override string ToString() => $"[{Length}], {BatchContent}";

        public Guid GetExternalBoardGuid()
        {
            if (!BatchContent.HasFlag(BatchContentBits.DealWithExternalBoard))
            {
                throw new Exception($"The batch does not contain an External Board Id");
            }

            // TODO: Find the External Board Id in the parameters. In must be one one.
            return Guid.Empty;
        }
        #endregion


        #region -> Implementation
        private void Analyse()
        {
            if (Length == 0)
            {
                return;
            }

            if (Commands.Where(x => x.Type == CommandType.CloneBoard).Any())
            {
                throw ScenarioIsNotSupportedYetException.CloneBoard();
            }

            if (Commands.Where(x => x.Type == CommandType.CreateBoard).Any())
            {
                if (Commands.Where(x => x.Type == CommandType.CreateBoard).Count() > 1)
                {
                    throw ScenarioIsNotSupportedYetException.SeveralNewBoards();
                }

                if (this[0].Type != CommandType.CreateBoard)
                {
                    throw ImpossibleScenarioIsException.CreateBoardFirst();
                }

                BatchContent |= BatchContentBits.CreateLocalBoard;

                // TODO: check if commands do not contains board IDs
            }
            else
            {
                // TODO: check if commands contains external board IDs

                BatchContent |= BatchContentBits.DealWithExternalBoard;
            }
        }
        #endregion


        #region -> Ctor
        private CommandBatch() { }

        public CommandBatch(ICommand cmnd) : this()
        {
            _items.Add(0, cmnd);
            Analyse();
        }

        public CommandBatch(IEnumerable<ICommand> items) : this()
        {
            items.ToList().ForEach(
                item => _items.Add(_items.Count, item));

            Analyse();
        }
        #endregion
    }
}
