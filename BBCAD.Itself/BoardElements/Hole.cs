using System.Xml.Linq;

using BBCAD.Itself.SVGElements;

namespace BBCAD.Itself.BoardElements
{
    public class Hole
    {
        public int X { get; set; }
        public int Y { get; set; }

        public readonly Guid Id;

        public IEnumerable<XElement> XE
        {
            get
            {
                int scale = 20;

                return new XElement[]{
                    new SvgCircle((X + 1) * scale, (Y + 1) * scale, 5, Color.Yellow, Id).XML,
                    new SvgCircle((X + 1) * scale, (Y + 1) * scale, 3, Color.Black).XML
                };
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
