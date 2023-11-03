namespace BBCAD.Cmnd.Scripts
{
    [Flags]
    public enum ScriptLineType
    {
        Empty = 0,
        ParsibleContent = 0b_00_0001,
        FullyCommented = 0b_00_0010,
        ContainsComment = 0b_00_0100,
        MiddlelineComment = 0b_11_0000,
        MultylineCommentEnd = 0b_10_0000,
        MultylineCommentStart = 0b_10_0000,
    }
}
