using System.Xml.Linq;
using BBCAD.Itself.Common;

namespace BBCAD.Itself.SVGElements
{
    internal class SvgCircle : ISvgElement
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int R { get; set; }
        public Color Color { get; set; } = Color.White;

        public readonly Guid Id;

        public bool Tagged => Id != Guid.Empty;

        public IEnumerable<XElement> SVG
        {
            get
            {
                XElement xe = new("circle"
                                   , new XAttribute("cx", X)
                                   , new XAttribute("cy", Y)
                                   , new XAttribute("r", R)
                                   , new XAttribute("fill", Color)
                    );

                if (Tagged)
                {
                    xe.Add(new XAttribute("data-type", "point"),
                        new XAttribute("data-id", Id.ToString().ToUpper()));
                }

                // xe.Attributes("xmlns").Remove();
                return new XElement[] { xe };
            }
        }

        public override string ToString() => $"circle({X}, {Y}, {R}, {Color})";

        public SvgCircle(int x, int y, int r, Color color) : this(x, y, r, color, Guid.Empty) { }
        public SvgCircle(int x, int y, int r, Color color, Guid id)
        {
            Id = id;
            X = x;
            Y = y;
            R = r;
            Color = color;
        }
    }
}
