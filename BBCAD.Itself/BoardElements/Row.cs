using System.Xml.Linq;

using BBCAD.Itself.Common;
using BBCAD.Itself.SVGElements;

namespace BBCAD.Itself.BoardElements
{
    public class Row : ISvgElement
    {
        #region -> Data
        internal const string XMLRootName = "Line";
        private const string XMLPointsName = "Points";
        #endregion


        #region -> Properties
        public List<Point> Points { get; set; } = new List<Point>();

        internal XElement XML
        {
            get
            {
                return new XElement(XMLRootName
                    , new XElement(XMLPointsName, Points.Select(p => p.XML)));
            }
            set
            {
                if (value.Name != XMLRootName)
                {
                    throw new ArgumentException(
                        $"Element name \"{value.Name}\" instead of \"{XMLRootName}\"");
                }

                Points.Clear();
                var items = value?
                    .Element(XMLPointsName)?
                    .Elements(Point.XMLRootName)
                    .Select(xe => new Point(xe));
                if (items != null) { Points.AddRange(items); }
            }
        }

        public IEnumerable<XElement> SVG
        {
            get
            {
                int scale = 20;

                return new SvgPLine(Points.Select(
                    p => new Point((p.X + 1) * scale, (p.Y + 1) * scale))).SVG;
            }
        }
        #endregion


        public void Add(int x, int y) => Points.Add(new Point(x, y));

        public override string ToString() => $"Point count: {Points.Count}";

        public Row()
        {
        }

        internal Row(XElement xe) : this() { XML = xe; }
    }
}
