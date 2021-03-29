using System.Collections.Generic;
using DummyChess.Common;
using DummyChess.Mvvm;

namespace DummyChess.Domain
{
    public sealed class Piece : BindableBase
    {
        public static readonly IReadOnlyDictionary<PieceType, string> CodeMapping = new Dictionary<PieceType, string>
        {
            [PieceType.King] = "\uf43f",
            [PieceType.Queen] = "\uf445",
            [PieceType.Rook] = "\uf447",
            [PieceType.Bishop] = "\uf43a",
            [PieceType.Knight] = "\uf441",
            [PieceType.Pawn] = "\uf443",
        };

        public bool HasMoved { get; set; }
        public Player Owner { get; }
        public PieceType Type { get; }
        public string Code { get; }

        private Int2 _coordinate;
        public Int2 Coordinate
        {
            get => _coordinate;
            set => SetProperty(ref _coordinate, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public Piece(Player owner, PieceType type, Int2 coordinate)
        {
            Owner = owner;
            Code = CodeMapping[type];
            Type = type;
            Coordinate = coordinate;
        }
    }
}