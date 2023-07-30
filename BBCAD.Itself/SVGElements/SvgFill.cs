using System.Xml.Linq;
using BBCAD.Itself.Common;

namespace BBCAD.Itself.SVGElements
{
    internal class SvgFill : ISvgElement
    {
        public Color Color { get; set; } = Color.Green;
        public IEnumerable<XElement> SVG
        {
            get
            {
                XElement xe = new("rect"
                                    , new XAttribute("width", "100%")
                                    , new XAttribute("height", "100%")
                                    , new XAttribute("fill", Color.Green)
                                    );

                return new XElement[] { xe };
            }
        }

        public override string ToString() => $"{Color}";

        public SvgFill()
        {
        }
    }
}
