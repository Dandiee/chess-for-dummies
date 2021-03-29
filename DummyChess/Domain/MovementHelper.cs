using System;
using System.Collections.Generic;
using System.Linq;
using DummyChess.Common;

namespace DummyChess.Domain
{
    public static class MovementHelper
    {
        // instead of a switch case we can use dictionary (if it is O(1) on lookup)
        private static readonly IReadOnlyDictionary<PieceType, Func<Piece, Player, IReadOnlyCollection<Int2>>>
            ValidMovesByPieceType = new Dictionary<PieceType, Func<Piece, Player, IReadOnlyCollection<Int2>>>
            {
                [PieceType.King] = (s, p) => GetValidKingMoves(p, s).ToList(),
                [PieceType.Queen] = (s, p) => GetValidQueenMoves(p, s).ToList(),
                [PieceType.Rook] = (s, p) => GetValidRookMoves(p, s).ToList(),
                [PieceType.Bishop] = (s, p) => GetValidBishopMoves(p, s).ToList(),
                [PieceType.Knight] = (s, p) => GetValidKnightMoves(p, s).ToList(),
                [PieceType.Pawn] = (s, p) => GetValidPawnMoves(p, s).ToList(),
            };

        private static readonly IReadOnlyCollection<Int2> ValidKnightTranslations = new[]
        {
            new Int2(-2, 1),
            new Int2(-2, -1),
            new Int2(2, 1),
            new Int2(2, -1),
            new Int2(1, 2),
            new Int2(-1, 2),
            new Int2(1, -2),
            new Int2(-1, -2)
        };

        public static IReadOnlyCollection<Int2> GetValidMoves(Player currentPlayer, Piece state)
            => ValidMovesByPieceType[state.Type](state, currentPlayer).ToList();

        public static bool GetIsEnemyInCheck(Player currentPlayer, Game game)
        {
            foreach (var enemyPiece in game.CurrentPlayer.Pieces)
            {
                var validEnemyMoves = GetValidMoves(currentPlayer, enemyPiece);
                foreach (var validEnemyMove in validEnemyMoves)
                {
                    if (validEnemyMove == game.EnemyPlayer.King.Coordinate)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        
        private static IEnumerable<Int2> GetValidKnightMoves(Player currentPlayer, Piece piece)
        {
            foreach (var valid in ValidKnightTranslations)
            {
                var target = new Int2(piece.Coordinate.X + valid.X, piece.Coordinate.Y + valid.Y);
                if (IsValidTarget(currentPlayer, target))
                {
                    yield return target;
                }
            }
        }

        private static IEnumerable<Int2> GetValidRookMoves(Player currentPlayer, Piece state)
        {
            var results = new List<Int2>();
            foreach (var direction in Int2.StraightDirections)
            {
                results.AddRange(GetValidMovesByDirection(currentPlayer, state, direction));
            }

            return results;
        }

        private static IEnumerable<Int2> GetValidQueenMoves(Player currentPlayer, Piece state)
        {
            var results = new List<Int2>();
            foreach (var direction in Int2.StraightDirections.Concat(Int2.DiagonalDirections))
            {
                results.AddRange(GetValidMovesByDirection(currentPlayer, state, direction));
            }

            return results;
        }

        private static IEnumerable<Int2> GetValidBishopMoves(Player currentPlayer, Piece piece)
        {
            var results = new List<Int2>();
            foreach (var direction in Int2.DiagonalDirections)
            {
                results.AddRange(GetValidMovesByDirection(currentPlayer, piece, direction));
            }

            return results;
        }

        private static IEnumerable<Int2> GetValidPawnMoves(Player currentPlayer, Piece state)
        {
            var validDirection = state.Owner.IsWhite
                ? new Int2(0, -1)
                : new Int2(0, 1);

            var targetCell = state.Coordinate + validDirection;

            if (IsInBoundary(targetCell) && !IsOccupied(currentPlayer, targetCell))
            {
                yield return targetCell;
            }

            if (!state.HasMoved)
            {
                var firstMoveTarget = targetCell + validDirection;
                if (IsInBoundary(firstMoveTarget) && !IsOccupied(currentPlayer, firstMoveTarget))
                {
                    yield return firstMoveTarget;
                }
            }

            var leftTarget = targetCell - Int2.UnitX;
            if (IsInBoundary(leftTarget) && IsOccupiedByEnemy(currentPlayer, leftTarget))
            {
                yield return leftTarget;
            }

            var rightTarget = targetCell + Int2.UnitX;
            if (IsInBoundary(rightTarget) && IsOccupiedByEnemy(currentPlayer, rightTarget))
            {
                yield return rightTarget;
            }
        }

        private static IEnumerable<Int2> GetValidKingMoves(Player currentPlayer, Piece state)
        {
            foreach (var direction in Int2.AllDirections)
            {
                var targetCell = state.Coordinate + direction;
                if (IsValidTarget(currentPlayer, targetCell))
                {
                    yield return targetCell;
                }
            }
        }

        private static IEnumerable<Int2> GetValidMovesByDirection(Player currentPlayer, Piece piece, Int2 step)
        {
            for (var nextStep = piece.Coordinate + step;
                IsValidTarget(currentPlayer, nextStep);
                nextStep += step)
            {
                yield return nextStep;

                if (IsOccupiedByEnemy(currentPlayer, nextStep))
                {
                    yield break;
                }
            }
        }

        private static bool IsInBoundary(Int2 targetCell)
            => targetCell.X >= 0 && targetCell.X < 8 && targetCell.Y >= 0 && targetCell.Y < 8;

        private static bool IsValidTarget(Player currentPlayer, Int2 targetCell)
        {
            if (!IsInBoundary(targetCell))
            {
                return false;
            }

            return !IsOccupiedByOwner(currentPlayer, targetCell);
        }

        private static bool IsOccupiedByOwner(Player currentPlayer, Int2 targetCell)
            => currentPlayer.Pieces.Any(p =>
                p.Coordinate.X == targetCell.X && p.Coordinate.Y == targetCell.Y);
        private static bool IsOccupiedByEnemy(Player currentPlayer, Int2 targetCell)
            => currentPlayer.Enemy.Pieces.Any(p =>
                p.Coordinate.X == targetCell.X && p.Coordinate.Y == targetCell.Y);

        private static bool IsOccupied(Player currentPlayer, Int2 targetCell)
            => IsOccupiedByEnemy(currentPlayer, targetCell) || IsOccupiedByOwner(currentPlayer, targetCell);
    }
}
