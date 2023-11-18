using System.Xml.Linq;

using BBCAD.Cmnd.Common;
using BBCAD.Cmnd.Commands;

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

            List<Guid> ids = new();
            foreach (ICommand command in Commands)
            {
                switch (command)
                {
                    case CreateBoardCommand _:
                        break;

                    case ResizeBoardCommand resizeBoardCommand:
                        if (resizeBoardCommand.Id.IsConfigured)
                        {
                            ids.Add(resizeBoardCommand.Id.Value);
                        }
                        break;

                    // case CloneBoardCommand cloneBoardCommand:
                    //     if (cloneBoardCommand.Id.Value.IsConfigured)
                    //     {
                    //         ids.Add(cloneBoardCommand.Id.Value);
                    //     }
                    //     break;

                    // case AddLineCommand addLineCommand:
                    //     if (addLineCommand.Id.Value.IsConfigured)
                    //     {
                    //         ids.Add(addLineCommand.Id.Value);
                    //     }
                    //     break;

                    default:
                        throw new NotImplementedException($"{command.GetType().Name}");
                }
            }

            ids = ids.Distinct().ToList();

            if (ids.Count == 0)
            {
                return Guid.Empty;
            }

            if (ids.Count == 1)
            {
                return ids.Single();
            }

            throw new Exception($"There are {ids.Count} Board Ids in the batch. This functionality is not supported yet. In must be only one board Id.");
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

            if (Commands.Where(x => x.CmndType == CommandType.CloneBoard).Any())
            {
                throw ScenarioIsNotSupportedYetException.CloneBoard();
            }

            if (Commands.Where(x => x.CmndType == CommandType.CreateBoard).Any())
            {
                if (Commands.Where(x => x.CmndType == CommandType.CreateBoard).Count() > 1)
                {
                    throw ScenarioIsNotSupportedYetException.SeveralNewBoards();
                }

                if (this[0].CmndType != CommandType.CreateBoard)
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
