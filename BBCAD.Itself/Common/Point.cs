namespace BBCAD.Itself.Common
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString() => $"{X},{Y}";

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
