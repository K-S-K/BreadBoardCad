namespace BBCAD.Cmnd.Common
{
    [Flags]
    public enum BatchContentBits
    {
        Empty = 0,
        CreateLocalBoard = 0b_0000_0001,
        DealWithExternalBoard = 0b_0000_0000,
    }
}
