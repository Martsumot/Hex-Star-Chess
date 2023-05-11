using System.Collections.Generic;

namespace HexStarChess.Model
{
    public abstract class PieceBase
    {
        public PieceType Type { get; protected set; }

        public bool IsPlayer { get; protected set; }

        public int Forward
        {
            get
            {
                return IsPlayer ? 1 : -1;
            }
        }

        protected readonly int _maxDistance = 7;

        protected List<HexBoardVector2Int> _oddDirections;

        protected List<HexBoardVector2Int> _evenDirections;

        protected PieceBase(bool isPlayer)
        {
            IsPlayer = isPlayer;
        }
        /// <summary>
        /// このメソッドで得られる移動先は移動できることが必ず確認されている必要がある
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public abstract List<Movement> GetAllMovements(Board board);
        
        /// <summary>
        /// 移動先に敵の駒があるか確認する
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool IsEnemyPieceCollide(ref Movement movement, Board board)
        {
            if(!board.PiecesOnBoard.ContainsKey(movement.Destination))
            {
                return false;
            }

            if(board.PiecesOnBoard[movement.Destination] == null)
            {
                return false;
            }

            if(movement.MovingPiece.IsPlayer != board.PiecesOnBoard[movement.Destination].IsPlayer)
            {
                movement.SetCapturedPiece(board.PiecesOnBoard[movement.Destination]);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Movementが可能か確認する
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool IsMovementPossible(Movement movement, Board board)
        {
            //移動先が存在しなければ移動不可
            if(!board.PiecesOnBoard.ContainsKey(movement.Destination))
            {
                return false;
            }
            //移動先に駒がないなら移動可能
            if(board.PiecesOnBoard[movement.Destination] == null)
            {
                return true;
            }
            //移動先の駒が敵の駒なら移動可能
            if (movement.MovingPiece.IsPlayer != board.PiecesOnBoard[movement.Destination].IsPlayer)
            {
                return true;
            }
            return false;
        }
    }
}