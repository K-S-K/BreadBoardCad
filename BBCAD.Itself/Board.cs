using System.Xml.Linq;

using BBCAD.Itself.Common;
using BBCAD.Itself.SVGElements;
using BBCAD.Itself.BoardElements;

namespace BBCAD.Itself
{
    public class Board
    {
        #region -> Data
        private const string XMLRootName = "Board";
        private const string XMLLinesName = "Lines";
        #endregion


        #region -> Properties
        public Guid Id { get; set; } = Guid.NewGuid();
        public int SizeX { get; set; } = 13;
        public int SizeY { get; set; } = 8;
        public string Name { get; set; } = string.Empty;

        public List<Row> Rows { get; set; } = new List<Row>();

        public XElement XML
        {
            get
            {
                return new XElement(XMLRootName
                    , new XElement(nameof(Id), Id.ToString().ToUpper())
                    , new XElement(nameof(Name), Name)
                    , new XElement(nameof(SizeX), SizeX)
                    , new XElement(nameof(SizeY), SizeY)
                    , new XElement(XMLLinesName, Rows.Select(r => r.XML))
                    );
            }
            set
            {
                if (value.Name != XMLRootName)
                {
                    throw new ArgumentException(
                        $"Element name \"{value.Name}\" instead of \"{XMLRootName}\"");
                }

                #region -> Metadata
                {
                    if (int.TryParse(value.Element(nameof(SizeX))?.Value, out int x)) SizeX = x; else { throw new InvalidDataException($"Can't parse {nameof(SizeX)} from {{{value.ToString()[..32]}}}"); }
                    if (int.TryParse(value.Element(nameof(SizeY))?.Value, out int y)) SizeY = y; else { throw new InvalidDataException($"Can't parse {nameof(SizeY)} from {{{value.ToString()[..32]}}}"); }
                    if (Guid.TryParse(value.Element(nameof(Id))?.Value, out Guid id)) Id = id; else { throw new InvalidDataException($"Can't parse {nameof(Id)} from {{{value.ToString()[..32]}}}"); }
                    Name = value.Element(nameof(Name))?.Value ?? string.Empty;
                }
                #endregion

                Rows.Clear();
                var items = value?
                    .Element(XMLLinesName)?
                    .Elements(Row.XMLRootName)
                    .Select(xe => new Row(xe));
                if (items != null)
                {
                    Rows.AddRange(items);
                }
            }
        }

        public XElement SVG
        {
            get
            {
                int scale = 20;

                // Prepare the document node
                XNamespace xmlns = "http://www.w3.org/2000/svg";
                XElement xe = new(xmlns + "svg"
                    , new XAttribute("version", "1.1")
                    , new XAttribute("width", (SizeX + 1) * scale)
                    , new XAttribute("height", (SizeY + 1) * scale)
                    );

                // Fill the board color
                xe.Add(new SvgFill().SVG);

                // Draw the rows
                xe.Add(Rows.Select(x => x.SVG));

                // Draw the board hole grid
                for (int x = 0; x < SizeX; x++)
                {
                    for (int y = 0; y < SizeY; y++)
                    {
                        xe.Add(new Hole(x, y).SVG);
                    }
                }

                return xe;
            }
        }
        #endregion


        public override string ToString() => $"[{SizeX}x{SizeY}] \"{Name}\" {{{Id.ToString().ToUpper()}}}";

        public Board()
        {
        }

        public Board(XElement xe) : this() { XML = xe; }

        public static Board Sample
        {
            get
            {
                Board board = new()
                {
                    Id = Guid.Empty,
                    Name = "Demo",
                    SizeX = 13,
                    SizeY = 8
                };

                board.Rows.Add(new Row() { Points = new List<Common.Point> { new Point(1, 7), new Point(1, 5) } });
                board.Rows.Add(new Row() { Points = new List<Common.Point> { new Point(2, 7), new Point(2, 5) } });
                board.Rows.Add(new Row() { Points = new List<Common.Point> { new Point(3, 7), new Point(5, 7), new Point(5, 4), new Point(11, 4), new Point(11, 1) } });
                board.Rows.Add(new Row() { Points = new List<Common.Point> { new Point(5, 0), new Point(2, 0), new Point(2, 1), new Point(2, 0), new Point(0, 0), new Point(0, 3), new Point(10, 3), new Point(10, 1) } });
                board.Rows.Add(new Row() { Points = new List<Common.Point> { new Point(1, 1), new Point(1, 2), new Point(7, 2), new Point(7, 0), new Point(12, 0), new Point(12, 1) } });
                board.Rows.Add(new Row() { Points = new List<Common.Point> { new Point(3, 1), new Point(6, 1), new Point(6, 0) } });
                board.Rows.Add(new Row() { Points = new List<Common.Point> { new Point(7, 7), new Point(7, 6), new Point(9, 6) } });
                board.Rows.Add(new Row() { Points = new List<Common.Point> { new Point(8, 7), new Point(10, 7), new Point(10, 5), new Point(12, 5) } });

                return board;
            }
        }
    }
}
