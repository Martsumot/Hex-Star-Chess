using System.Collections.Generic;
namespace HexStarChess.Model
{
    public class PieceRook : PieceBase
    {
        public PieceRook(bool isPlayer) : base(isPlayer)
        {
            Type = PieceType.Rook;
            _oddDirections = new(HexDirections.RookDirections);
            _evenDirections = new(HexDirections.RookDirections);
        }
        public override List<Movement> GetAllMovements(Board board)
        {
            HexBoardVector2Int position = board.GetPiecePosition(this);
            List<Movement> allMovements = new();
            List<HexBoardVector2Int> directions = position.x % 2 == 1 ? _oddDirections : _evenDirections;
            HexBoardVector2Int temporaryPosition = position;
            for (int i = 0; i < directions.Count; i++)
            {
                for (int j = 1; j < _maxDistance; j++)
                {
                    directions = temporaryPosition.x % 2 == 1 ? _oddDirections : _evenDirections;
                    temporaryPosition += directions[i];
                    Movement movement = new(this, position, temporaryPosition);
                    if (IsEnemyPieceCollide(ref movement, board))
                    {
                        allMovements.Add(movement);
                        break;
                    }
                    if (IsMovementPossible(movement, board))
                    {
                        allMovements.Add(movement);
                    }
                    else
                    {
                        break;
                    }
                }
                temporaryPosition = position;
            }

            return allMovements;
        }
    }
}