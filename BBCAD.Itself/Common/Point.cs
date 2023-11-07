using System.Xml.Linq;

namespace BBCAD.Itself.Common
{
    public class Point
    {
        #region -> Data
        internal const string XMLRootName = "Point";
        #endregion


        #region -> Properties
        public int X { get; set; }
        public int Y { get; set; }

        internal XElement XML
        {
            get
            {
                return new XElement(XMLRootName
                    , new XAttribute(nameof(X), X)
                    , new XAttribute(nameof(Y), Y)
                    );
            }
            set
            {
                if (value.Name != XMLRootName)
                {
                    throw new ArgumentException(
                        $"Element name \"{value.Name}\" instead of \"{XMLRootName}\"");
                }

                if (int.TryParse(value.Attribute(nameof(X))?.Value, out int x)) X = x;
                else { throw new InvalidDataException($"Can't parse {nameof(X)} from {{{value}}}"); }

                if (int.TryParse(value.Attribute(nameof(Y))?.Value, out int y)) Y = y;
                else { throw new InvalidDataException($"Can't parse {nameof(Y)} from {{{value}}}"); }
            }
        }
        #endregion

        public override string ToString() => $"{X},{Y}";

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        internal Point(XElement xe) { XML = xe; }
    }
}
