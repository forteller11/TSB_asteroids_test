namespace Charly.Common.Utils
{
    public static class MaskUtils
    {
        public static bool ContainsFlags(int field, int mask)
        {
            int bitwiseAnd = field & mask;
            if (bitwiseAnd != 0)
                return true;
            return false;
        }
        
        public static bool HasAllFlags(int self, int other)
        {
            int bitwiseAnd = self & other;
            if (bitwiseAnd == self)
                return true;
            return false;
        }
    }
}