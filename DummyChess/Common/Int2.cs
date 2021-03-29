using System.Collections.Generic;
using System.Linq;
using DummyChess.Mvvm;

namespace DummyChess.Common
{
    public class Int2 : BindableBase
    {
        public static Int2 UnitX = new Int2(1, 0);
        public static Int2 UnitY = new Int2(0, 1);

        public static readonly IReadOnlyCollection<Int2> DiagonalDirections = new[]
        {
            new Int2(1, 1),
            new Int2(-1, 1),
            new Int2(-1, -1),
            new Int2(1, -1)
        };

        public static readonly IReadOnlyCollection<Int2> StraightDirections = new[]
        {
            new Int2(0, 1),
            new Int2(0, -1),
            new Int2(1, 0),
            new Int2(-1, 0)
        };

        public static readonly IReadOnlyCollection<Int2> AllDirections =
            DiagonalDirections.Concat(StraightDirections).ToList();

        private int _x;
        public int X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }

        private int _y;
        public int Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }

        public Int2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Int2 operator +(Int2 lhs, Int2 rhs) => new Int2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        public static Int2 operator -(Int2 lhs, Int2 rhs) => new Int2(lhs.X - rhs.X, lhs.Y - rhs.Y);
        public static bool operator ==(Int2 lhs, Int2 rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
        public static bool operator !=(Int2 lhs, Int2 rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y;
    }
}
