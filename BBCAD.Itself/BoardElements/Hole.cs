using System.Xml.Linq;

using BBCAD.Itself.Common;
using BBCAD.Itself.SVGElements;

namespace BBCAD.Itself.BoardElements
{
    public class Hole : ISvgElement
    {
        public int X { get; set; }
        public int Y { get; set; }

        public readonly Guid Id;

        public IEnumerable<XElement> SVG
        {
            get
            {
                int scale = 20;

                var inner = new SvgCircle((X + 1) * scale, (Y + 1) * scale, 3, Color.Black).SVG;
                var outer = new SvgCircle((X + 1) * scale, (Y + 1) * scale, 5, Color.Yellow, Id).SVG;

                return outer.Concat(inner);
            }
        }

        public override string ToString() => $"Hole({X}, {Y})";

        public Hole(int x, int y)
        {
            Id = Guid.NewGuid();
            X = x;
            Y = y;
        }
    }
}
