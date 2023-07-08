using System.Xml.Linq;

using BBCAD.Itself.Common;
using BBCAD.Itself.SVGElements;

namespace BBCAD.Itself.BoardElements
{
    public class Row
    {
        public List<Point> Points { get; set; } = new List<Point>();

        public XElement XE
        {
            get
            {
                int scale = 20;

                return new SvgPLine(Points.Select(p =>
                    new Point((p.X + 1) * scale, (p.Y + 1) * scale)
                    )).XML;
            }
        }

        public void Add(int x, int y) => Points.Add(new Point(x, y));

        public override string ToString() => $"Point count: {Points.Count}";

        public Row()
        {
        }
    }
}
