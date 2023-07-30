using System.Xml.Linq;

namespace BBCAD.Itself.Common
{
    internal interface ISvgElement
    {
        IEnumerable<XElement> SVG { get; }

        string ToString();
    }
}