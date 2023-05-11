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
        /// ���̃��\�b�h�œ�����ړ���͈ړ��ł��邱�Ƃ��K���m�F����Ă���K�v������
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public abstract List<Movement> GetAllMovements(Board board);
        
        /// <summary>
        /// �ړ���ɓG�̋���邩�m�F����
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
        /// Movement���\���m�F����
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool IsMovementPossible(Movement movement, Board board)
        {
            //�ړ��悪���݂��Ȃ���Έړ��s��
            if(!board.PiecesOnBoard.ContainsKey(movement.Destination))
            {
                return false;
            }
            //�ړ���ɋ�Ȃ��Ȃ�ړ��\
            if(board.PiecesOnBoard[movement.Destination] == null)
            {
                return true;
            }
            //�ړ���̋�G�̋�Ȃ�ړ��\
            if (movement.MovingPiece.IsPlayer != board.PiecesOnBoard[movement.Destination].IsPlayer)
            {
                return true;
            }
            return false;
        }
    }
}