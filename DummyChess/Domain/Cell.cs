using DummyChess.Common;
using DummyChess.Mvvm;

namespace DummyChess.Domain
{
    public sealed class Cell : BindableBase
    {
        public Cell(bool isWhite, Int2 coordinate)
        {
            IsWhite = isWhite;
            Coordinate = coordinate;
        }

        public bool IsWhite { get; }
        public Int2 Coordinate { get; }

        private bool _isValidTarget;
        public bool IsValidTarget
        {
            get => _isValidTarget;
            set => SetProperty(ref _isValidTarget, value);
        }
    }
}