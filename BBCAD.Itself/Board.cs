using System.Xml.Linq;

using BBCAD.Itself.Common;
using BBCAD.Itself.SVGElements;
using BBCAD.Itself.BoardElements;

namespace BBCAD.Itself
{
    public class Board
    {
        public int Id { get; set; }
        public int SizeX { get; set; } = 13;
        public int SizeY { get; set; } = 8;

        public List<Row> Rows { get; set; } = new List<Row>();

        public XElement XML
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
                xe.Add(new SvgFill().XML);

                // Draw the rows
                xe.Add(Rows.Select(x => x.XE));

                // Draw the board hole grid
                for (int x = 0; x < SizeX; x++)
                {
                    for (int y = 0; y < SizeY; y++)
                    {
                        xe.Add(new Hole(x, y).XE);
                    }
                }

                return xe;
            }
        }

        public Board()
        {
            SizeX = 13;
            SizeY = 8;

            Rows.Add(new Row() { Points = new List<Common.Point> { new Point(1, 7), new Point(1, 5) } });
            Rows.Add(new Row() { Points = new List<Common.Point> { new Point(2, 7), new Point(2, 5) } });
            Rows.Add(new Row() { Points = new List<Common.Point> { new Point(3, 7), new Point(5, 7), new Point(5, 4), new Point(11, 4), new Point(11, 1) } });
            Rows.Add(new Row() { Points = new List<Common.Point> { new Point(5, 0), new Point(2, 0), new Point(2, 1), new Point(2, 0), new Point(0, 0), new Point(0, 3), new Point(10, 3), new Point(10, 1) } });
            Rows.Add(new Row() { Points = new List<Common.Point> { new Point(1, 1), new Point(1, 2), new Point(7, 2), new Point(7, 0), new Point(12, 0), new Point(12, 1) } });
            Rows.Add(new Row() { Points = new List<Common.Point> { new Point(3, 1), new Point(6, 1), new Point(6, 0) } });
            Rows.Add(new Row() { Points = new List<Common.Point> { new Point(7, 7), new Point(7, 6), new Point(9, 6) } });
            Rows.Add(new Row() { Points = new List<Common.Point> { new Point(8, 7), new Point(10, 7), new Point(10, 5), new Point(12, 5) } });
        }
    }
}
