using System.Collections.Generic;

namespace DummyChess.Common
{
    public sealed class Int2Comparer : IEqualityComparer<Int2>
    {
        public static readonly Int2Comparer Instance = new Int2Comparer();

        public bool Equals(Int2 lhs, Int2 rhs)
        {
            if (lhs == null || rhs == null) return false;
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        public int GetHashCode(Int2 obj)
        {
            int hash = 17;
            hash = hash * 23 + obj.X.GetHashCode();
            hash = hash * 23 + obj.Y.GetHashCode();
            return hash;
        }
    }
}