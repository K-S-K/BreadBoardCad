using System.Xml.Linq;

using BBCAD.Itself.Common;

namespace BBCAD.Itself.SVGElements
{
    internal class SvgPLine
    {
        IEnumerable<Point> Points { get; set; } = new List<Point>();
        public int W { get; set; } = 6;
        public Color Color { get; set; } = Color.White;

        public readonly Guid Id;

        public XElement XML
        {
            get
            {
                return new XElement("polyline"
                                   , new XAttribute("points", string.Join(" ", Points.Select(x => x.ToString())))
                                   , new XAttribute("style", $"fill:none;stroke:rgb(255,255,255);stroke-width:{W}")
                                   , new XAttribute("data-type", "wire")
                                   , new XAttribute("data-id", Id.ToString().ToUpper())
                                   );
            }
        }

        public SvgPLine(IEnumerable<Point> points)
        {
            Id = Guid.NewGuid();
            Points = points;
        }
    }
}
