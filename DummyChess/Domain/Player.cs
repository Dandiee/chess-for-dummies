using System.Collections.ObjectModel;
using DummyChess.Common;
using DummyChess.Mvvm;

namespace DummyChess.Domain
{
    public sealed class Player : BindableBase
    {
        public static readonly PieceType[] StartingState = 
        {
            PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen, PieceType.King,  PieceType.Bishop, PieceType.Knight, PieceType.Rook
        };

        public bool IsWhite { get; }
        public Game Game { get; }

        public Piece King { get; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        private bool _isInCheck;
        public bool IsInCheck
        {
            get => _isInCheck;
            set => SetProperty(ref _isInCheck, value);
        }

        public string Name { get; }

        public ObservableCollection<Piece> Graveyard { get; }

        public Player Enemy => Game.Player1 == this ? Game.Player2 : Game.Player1;

        public Player(Game game, bool isWhite)
        {
            Game = game;
            IsWhite = isWhite;
            IsActive = isWhite;
            Name = isWhite ? "White" : "Black";
            Graveyard = new ObservableCollection<Piece>();
            Pieces = new ObservableCollection<Piece>();

            for (var i = 0; i < 8; i++)
            {
                Pieces.Add(new Piece(this, PieceType.Pawn, new Int2(i, IsWhite ? 6 : 1)));
                var piece = new Piece(this, StartingState[i], new Int2(i, IsWhite ? 7 : 0));
                Pieces.Add(piece);

                if (piece.Type == PieceType.King)
                {
                    King = piece;
                }
            }
        }

        private ObservableCollection<Piece> _pieces;
        public ObservableCollection<Piece> Pieces
        {
            get => _pieces;
            set => SetProperty(ref _pieces, value);
        }
    }
}