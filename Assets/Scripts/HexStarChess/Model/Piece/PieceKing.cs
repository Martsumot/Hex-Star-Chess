using System.Collections.Generic;

namespace HexStarChess.Model
{
    public class PieceKing : PieceBase
    {
        public PieceKing(bool isPlayer) : base(isPlayer)
        {
            Type = PieceType.King;
            _oddDirections = new(HexDirections.RookDirections);
            _oddDirections.AddRange(HexDirections.OddBishopDirections);
            _evenDirections = new(HexDirections.RookDirections);
            _evenDirections.AddRange(HexDirections.EvenBishopDirections);
        }
        public override List<Movement> GetAllMovements(Board board)
        {
            HexBoardVector2Int position = board.GetPiecePosition(this);
            List<Movement> allMovements = new();
            if(position.x % 2 == 1)
            {
                foreach(HexBoardVector2Int direction in _oddDirections)
                {
                    Movement movement = new(this, position, position + direction);
                    IsEnemyPieceCollide(ref movement, board);
                    if (IsMovementPossible(movement, board))
                    {
                        allMovements.Add(movement);
                    }
                }
            }
            else
            {
                foreach (HexBoardVector2Int direction in _evenDirections)
                {
                    Movement movement = new(this, position, position + direction);
                    IsEnemyPieceCollide(ref movement, board);
                    if (IsMovementPossible(movement, board))
                    {
                        allMovements.Add(movement);
                    }
                }
            }
            return allMovements;
        }
    }
}