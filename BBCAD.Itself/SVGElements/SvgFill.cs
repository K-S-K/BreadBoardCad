using System.Xml.Linq;

namespace BBCAD.Itself.SVGElements
{
    internal class SvgFill
    {
        public XElement XML
        {
            get
            {
                return new XElement("rect"
                                   , new XAttribute("width", "100%")
                                   , new XAttribute("height", "100%")
                                   , new XAttribute("fill", Color.Green)
                    );
            }
        }
    }
}
