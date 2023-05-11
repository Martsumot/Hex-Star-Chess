using System.Collections.Generic;
using System.Linq;

namespace HexStarChess.Model
{
    public class PiecePawn : PieceBase
    {
        private List<HexBoardVector2Int> _lastRanks;

        private static readonly PieceType[] _promotionTypes =
        {
            PieceType.Queen,
            PieceType.Rook,
            PieceType.Bishop,
            PieceType.Knight
        };

        public PiecePawn(bool isPlayer) : base(isPlayer)
        {
            Type = PieceType.Pawn;
            _oddDirections = isPlayer ? new(HexDirections.OddPlayerPawnCaptureDirections) : new(HexDirections.OddEnemyPawnCaptureDirections);
            _evenDirections = isPlayer ? new(HexDirections.EvenPlayerPawnCaptureDirections): new(HexDirections.EvenEnemyPawnCaptureDirections);
            if (isPlayer)
            {
                _lastRanks = new()
                {
                    new HexBoardVector2Int(2, 6),
                    new HexBoardVector2Int(3, 5),
                    new HexBoardVector2Int(4, 5),
                    new HexBoardVector2Int(5, 5),
                    new HexBoardVector2Int(6, 6)
                };
            }
            else
            {
                _lastRanks = new()
                {
                    new HexBoardVector2Int(2, 0),
                    new HexBoardVector2Int(3, 0),
                    new HexBoardVector2Int(4, 1),
                    new HexBoardVector2Int(5, 0),
                    new HexBoardVector2Int(6, 0),
                };
            }
        }
        public override List<Movement> GetAllMovements(Board board)
        {
            HexBoardVector2Int position = board.GetPiecePosition(this);
            List<Movement> allMovements = new();
            Movement movement = new(this, position, position + HexBoardVector2Int.up * Forward);
            if(!IsEnemyPieceCollide(ref movement, board) && IsMovementPossible(movement, board))
            {
                allMovements.Add(movement);


                movement = new(this, position, position + HexBoardVector2Int.up * Forward * 2);
                if (!board.MovedPawn.Contains(this) && !IsEnemyPieceCollide(ref movement, board)) 
                {
                    if (IsMovementPossible(movement, board))
                    {
                        allMovements.Add(movement);
                    }
                }
                
            }
            
            if(position.x % 2 == 1)
            {
                foreach(HexBoardVector2Int direction in _oddDirections)
                {
                    movement = new(this, position, position + direction);
                    if(IsEnemyPieceCollide(ref movement, board))
                    {
                        allMovements.Add(movement);
                    }
                }
            }
            else
            {
                foreach (HexBoardVector2Int direction in _evenDirections)
                {
                    movement = new(this, position, position + direction);
                    if (IsEnemyPieceCollide(ref movement, board))
                    {
                        allMovements.Add(movement);
                    }
                }
            }
            List<Movement> promotionMovements = new();
            for(int i = allMovements.Count - 1; i >= 0; i--)
            {
                if (_lastRanks.Contains(allMovements[i].Destination))
                {
                    movement = new(allMovements[i]);
                    allMovements.RemoveAt(i);
                    foreach (PieceType promotionType in _promotionTypes)
                    {
                        promotionMovements.Add(new Movement(movement, promotionType));
                    }
                }
            }
            if(promotionMovements.Count > 0)
            {
                allMovements.AddRange(promotionMovements);
            }
            
            return allMovements;
        }
    }
}