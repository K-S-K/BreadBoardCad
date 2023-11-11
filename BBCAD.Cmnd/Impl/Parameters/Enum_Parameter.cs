using System;

namespace BBCAD.Cmnd.Impl.Parameters
{
    /// <summary>
    /// Type of the commands parameter
    /// </summary>
    public enum ParameterType
    {
        GUID,
        String,
        Integer,
        Direction,
    }

    internal static class ParameterType_Aux
    {
        public static bool MustBeQuoted(this ParameterType type)
        {
            switch (type)
            {
                case ParameterType.GUID:
                case ParameterType.String:

                    return true;
                case ParameterType.Integer:
                case ParameterType.Direction:
                    return false;

                default:
                    throw new NotImplementedException($"{typeof(ParameterType)}.{type}");
            }
        }
    }
}
