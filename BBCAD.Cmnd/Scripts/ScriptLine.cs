namespace BBCAD.Cmnd.Scripts
{
    internal class ScriptLine
    {
        public string OriginalLine { get; private set; }
        public string ParsibleLine { get; private set; }

        public ScriptLineType Type { get; private set; }

        public override string ToString() => $"{Type}:\t{OriginalLine}";

        public ScriptLine(string line)
        {
            OriginalLine = line;

            ParsibleLine = line.Trim();

            if (string.IsNullOrEmpty(OriginalLine))
            {
                Type = ScriptLineType.Empty;
                return;
            }

            if (ParsibleLine.StartsWith("//"))
            {
                Type |= ScriptLineType.FullyCommented;
                ParsibleLine = string.Empty;
            }

            if (ParsibleLine.Contains("/*"))
            {
                Type |= ScriptLineType.MultylineCommentStart;
            }

            if (ParsibleLine.Contains("*/"))
            {
                Type |= ScriptLineType.MultylineCommentEnd;
            }

            if (Type == ScriptLineType.FullyCommented) { return; }

            int ixTrim = -1;

            if (ParsibleLine.Contains("//"))
            {
                ixTrim = Math.Min(ixTrim, ParsibleLine.IndexOf("//"));
            }

            if (ParsibleLine.Contains("/*"))
            {
                ixTrim = Math.Min(ixTrim, ParsibleLine.IndexOf("/*"));
            }

            if (ixTrim >= 0)
            {
                ParsibleLine = ParsibleLine[..ixTrim].Trim();
            }

            if (!string.IsNullOrWhiteSpace(ParsibleLine))
            {
                Type |= ScriptLineType.ParsibleContent;
            }
        }
    }
}
