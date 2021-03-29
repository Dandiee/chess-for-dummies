using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DummyChess.Common;
using DummyChess.Mvvm;

namespace DummyChess.Domain
{
    public sealed class Game : BindableBase
    {
        public ICommand PieceSelectedCommand { get; }
        public ICommand MoveCommand { get; }


        private Player _player1;
        public Player Player1
        {
            get => _player1;
            set => SetProperty(ref _player1, value);
        }

        private Player _player2;
        public Player Player2
        {
            get => _player2;
            set => SetProperty(ref _player2, value);
        }

        public List<Cell> CellList { get; }
        private readonly Cell[,] _cellsMatrix;
        private readonly List<Cell> _validTargetCells;

        private Piece _selectedPiece;

        public Game()
        {
            PieceSelectedCommand = new DelegateCommand<Piece>(OnPieceSelectedCommand);
            MoveCommand = new DelegateCommand<Cell>(OnMoveCommand);

            _validTargetCells = new List<Cell>();
            _cellsMatrix = new Cell[8, 8];

            CellList = new List<Cell>();

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var cellColor = i % 2 == 0
                        ? (j % 2 == 0 ? Colors.White : Colors.Black)
                        : (j % 2 == 0 ? Colors.Black : Colors.White);

                    var cell = new Cell(cellColor == Colors.White, new Int2(i, j));
                    _cellsMatrix[i, j] = cell;
                    CellList.Add(cell);
                }
            }

            Initialize();
        }

        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set => SetProperty(ref _currentPlayer, value);
        }

        private Player _enemyPlayer;
        public Player EnemyPlayer
        {
            get => _enemyPlayer;
            set => SetProperty(ref _enemyPlayer, value);
        }

        private void SetSelectedPiece(Piece piece)
        {
            // reset the current selection on second click
            if (piece == _selectedPiece || piece == null)
            {
                _selectedPiece.IsSelected = false;
                _selectedPiece = null;
                ClearTargetCells();
                return;
            }

            // remove the previous selection indicator flag
            if (_selectedPiece != null)
            {
                _selectedPiece.IsSelected = false;
            }

            // set the new selection, update the indicator
            _selectedPiece = piece;
            _selectedPiece.IsSelected = true;
        }

        private void OnMoveCommand(Cell obj)
        {
            // move the selected piece
            _selectedPiece.Coordinate = obj.Coordinate;
            _selectedPiece.HasMoved = true;

            var enemyPiece = EnemyPlayer.Pieces.SingleOrDefault(p => p.Coordinate == obj.Coordinate);
            if (enemyPiece != default)
            {
                // KILL IT
                EnemyPlayer.Pieces.Remove(enemyPiece);
                EnemyPlayer.Graveyard.Add(enemyPiece);
            }

            SetSelectedPiece(null);
            ClearTargetCells();
            
            // check for check
            EnemyPlayer.IsInCheck = MovementHelper.GetIsEnemyInCheck(CurrentPlayer, this);

            SwapPlayers();
        }

        public void OnPieceSelectedCommand(Piece piece)
        {
            // the current player cannot move with the enemy player's pieces
            if (piece.Owner != CurrentPlayer)
            {
                return;
            }

            SetSelectedPiece(piece);
            UpdateTargetCells();
        }

        private void UpdateTargetCells()
        {
            ClearTargetCells();

            if (_selectedPiece != null)
            {
                var validMovements = MovementHelper.GetValidMoves(CurrentPlayer, _selectedPiece);
                foreach (var validMovement in validMovements)
                {
                    var validTargetCell = _cellsMatrix[validMovement.X, validMovement.Y];
                    validTargetCell.IsValidTarget = true;
                    _validTargetCells.Add(validTargetCell);
                }
            }
        }

        private void ClearTargetCells()
        {
            foreach (var previousTargetCell in _validTargetCells)
            {
                previousTargetCell.IsValidTarget = false;
            }
            _validTargetCells.Clear();
        }

        private void SwapPlayers()
        {
            var originalPlayer = CurrentPlayer;
            CurrentPlayer = EnemyPlayer;
            EnemyPlayer = originalPlayer;
            CurrentPlayer.IsActive = !CurrentPlayer.IsActive;
            EnemyPlayer.IsActive = !EnemyPlayer.IsActive;
        }

        public void Initialize()
        {
            Player1 = new Player(this, true);
            Player2 = new Player(this, false);
            CurrentPlayer = Player1;
            EnemyPlayer = Player2;
        }
    }
}
