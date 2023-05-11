using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HexStarChess.Model
{
    public class Game
    {
        public Board GameBoard;

        private readonly IAI _enemy;

        public bool IsPlaying => GameBoard.IsPlaying;

        public bool IsPlayerTurn => _isPlayerTurn;

        private bool _isPlayerTurn;

        private PieceBase _selectingPiece = null;

        private SynchronizationContext _context;
        /// <summary>
        /// ��s����ړ��ƍU���͈̔͂�ʒm
        /// </summary>
        public Action<(List<HexBoardVector2Int>, List<HexBoardVector2Int>)> OnShowRange;

        /// <summary>
        /// �ړ��������Ƃ�ʒm
        /// </summary>
        public Action<Movement> OnMoved;

        /// <summary>
        /// ��̏��i����ʒm
        /// </summary>
        public Action<List<Movement>> OnPromotion;

        /// <summary>
        /// Player�̏���(true)���s�k(false)��ʒm����
        /// </summary>
        public Action<bool> OnWin;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isPlayerFirst"></param>
        /// <param name="upForwardPlayer"></param>
        /// <param name="downForwardPlayer"></param>
        public Game(bool isPlayerFirst)
        {
            GameBoard = new();
            _enemy = new EasyAI();
            _enemy.SetAILevel(GameDataManager.Entity.AILevel);
            _isPlayerTurn = isPlayerFirst;
            _context = SynchronizationContext.Current;
        }

        public async Task SelectByEnemy()
        {
            Movement enemyMovement = _enemy.SelectMovement(GameBoard);
            PieceBase piece = enemyMovement.MovingPiece;
            List<Movement> allMovements = piece.GetAllMovements(GameBoard);
            List<HexBoardVector2Int> movableRange = new();
            List<HexBoardVector2Int> attackableRange = new();
            foreach (Movement movement in allMovements)
            {
                if (movement.CapturedPiece is null)
                {
                    movableRange.Add(movement.Destination);
                }
                if (movement.CapturedPiece is not null)
                {
                    attackableRange.Add(movement.Destination);
                }
            }
            

            _context.Post(_ =>
            {
                OnShowRange?.Invoke((movableRange,attackableRange));
            }, null);

            await Task.Delay(1000);

            MovePiece(enemyMovement);
        }

        /// <summary>
        /// �Ղ̋���ړ�������
        /// �ړ���ʒm����
        /// ��Ԃ����ɂ킽��
        /// </summary>
        /// <param name="movement">�ړ����</param>
        public void MovePiece(Movement movement)
        {
            GameBoard.MovePiece(movement);
            _context.Post(_ =>
            {
                OnMoved?.Invoke(movement);
            }, null);
            //King�������Ɛ؂�ւ��
            if(!IsPlaying)
            {
                _context.Post(_ =>
                {
                    OnWin?.Invoke(_isPlayerTurn);
                }, null);
                return;
            }

            _isPlayerTurn = !_isPlayerTurn;
        }


        /// <summary>
        /// Player�̑��삩���̑I�����I���������W�ւ̈ړ�������U��
        /// </summary>
        /// <param name="hexCoords">�I���������W</param>
        public void Select(HexBoardVector2Int hexCoords)
        {
            //�����̃^�[���łȂ��Ȃ��͑I���ł��Ȃ�
            if (!_isPlayerTurn)
            {
                return;
            }

            //�^�C���̖����ꏊ��I��������I������
            if (hexCoords is null)
            {
                _selectingPiece = null;
                return;
            }

            //���݂��Ȃ����W��I��������I������
            if(!GameBoard.PiecesOnBoard.ContainsKey(hexCoords))
            {
                _selectingPiece = null;
                return;
            }

            PieceBase piece = GameBoard.PiecesOnBoard[hexCoords];
            //�I�����Ă����Ɠ������W��I��������I������
            if(_selectingPiece == piece)
            {
                _selectingPiece = null;
                return;
            }

            //�I���������W��Player�̋����΁A���I����Ԃɂ��Ĉړ��\�ȍ��W�����点��
            //���ɑI����Ԃł��㏑�����čs����
            if (piece is not null && GameBoard.PlayerPieces.Contains(piece))
            {
                _selectingPiece = null;
                List<Movement> allMovements = piece.GetAllMovements(GameBoard);
                List<HexBoardVector2Int> movableRange = new();
                List<HexBoardVector2Int> attackableRange = new();
                foreach (Movement movement in allMovements)
                {
                    if (movement.CapturedPiece is null)
                    {
                        movableRange.Add(movement.Destination);
                    }
                    if (movement.CapturedPiece is not null)
                    {
                        attackableRange.Add(movement.Destination);
                    }
                }
                //�ړ��\�Ȓn�_���Ȃ���΁A�I���𒆎~����
                if (movableRange.Count == 0 && attackableRange.Count == 0)
                {
                    return;
                }

                _selectingPiece = piece;
                OnShowRange?.Invoke((movableRange, attackableRange));
                return;
            }

            //��I���ς݂Ȃ�I�������ړ��\�ȍ��W�Ɉړ�����
            if (_selectingPiece is not null)
            {
                List<Movement> allMovements = _selectingPiece.GetAllMovements(GameBoard);
                
                List<Movement> promotionMovements = new();
                Movement selectedMovement = null;
                foreach(Movement movement in allMovements)
                {
                    if(hexCoords == movement.Destination)
                    {
                        if (movement.PromotionType == PieceType.None)
                        {
                            selectedMovement = movement;
                            break;
                        }

                        promotionMovements.Add(movement);
                    }
                }

                //�ړ��̉ۂɊւ�炸�I������������
                _selectingPiece = null;

                //�ړ���ŏ��i����ꍇ�͈ړ��𒆎~���A���i����ʒm����
                if (promotionMovements.Count > 0)
                {
                    OnPromotion?.Invoke(promotionMovements);
                    return;
                }

                //�I���������W���ړ��\�ȍ��W�łȂ���Β��~����
                if (selectedMovement is null)
                {
                    return;
                }

                MovePiece(selectedMovement);
                return;
            }

            
        
        
        }
    }
}