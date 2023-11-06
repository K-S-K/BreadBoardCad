using System.Xml.Linq;

using BBCAD.Cmnd.Common;

namespace BBCAD.Cmnd.Impl.Commands
{
    public class CommandBatch : ICommandBatch
    {
        internal const string XMLNodeName = "Batch";
        internal const string XMLAttrContentTypeName = "content-type";
        internal const string XMLCommandsCollectionName = "Commands";

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

        public XElement XML
        {
            get
            {
                return new XElement(XMLNodeName
                    , new XAttribute(XMLAttrContentTypeName, BatchContent)
                    , new XElement(XMLCommandsCollectionName, Commands.Select(x => x.XML)));
            }
        }
        #endregion


        #region -> Methods
        public Guid GetExternalBoardGuid()
        {
            if (!BatchContent.HasFlag(BatchContentBits.DealWithExternalBoard))
            {
                throw new Exception($"The batch does not contain an External Board Id");
            }

            // TODO: Find the External Board Id in the parameters. In must be one one.
            return Guid.Empty;
        }

        public override string ToString() => $"[{Length}], {BatchContent}";
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
