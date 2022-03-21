namespace Charly.Common.Utils
{
    public static class MaskUtils
    {
        public static bool ContainsAtLeastOneFlag(int value, int mask)
        {
            int bitwiseAnd = value & mask;
            return bitwiseAnd != 0;
        }
        
        public static bool HasAllFlags(int value, int mask)
        {
            int bitwiseAnd = value & mask;
            return bitwiseAnd == value;
        }
    }
}