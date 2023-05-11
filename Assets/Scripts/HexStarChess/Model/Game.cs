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
        /// 駒が行える移動と攻撃の範囲を通知
        /// </summary>
        public Action<(List<HexBoardVector2Int>, List<HexBoardVector2Int>)> OnShowRange;

        /// <summary>
        /// 移動したことを通知
        /// </summary>
        public Action<Movement> OnMoved;

        /// <summary>
        /// 駒の昇格候補を通知
        /// </summary>
        public Action<List<Movement>> OnPromotion;

        /// <summary>
        /// Playerの勝利(true)か敗北(false)を通知する
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
        /// 盤の駒を移動させる
        /// 移動を通知する
        /// 手番を次にわたす
        /// </summary>
        /// <param name="movement">移動情報</param>
        public void MovePiece(Movement movement)
        {
            GameBoard.MovePiece(movement);
            _context.Post(_ =>
            {
                OnMoved?.Invoke(movement);
            }, null);
            //Kingが取られると切り替わる
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
        /// Playerの操作から駒の選択か選択した座標への移動を割り振る
        /// </summary>
        /// <param name="hexCoords">選択した座標</param>
        public void Select(HexBoardVector2Int hexCoords)
        {
            //自分のターンでないなら駒は選択できない
            if (!_isPlayerTurn)
            {
                return;
            }

            //タイルの無い場所を選択したら選択解除
            if (hexCoords is null)
            {
                _selectingPiece = null;
                return;
            }

            //存在しない座標を選択したら選択解除
            if(!GameBoard.PiecesOnBoard.ContainsKey(hexCoords))
            {
                _selectingPiece = null;
                return;
            }

            PieceBase piece = GameBoard.PiecesOnBoard[hexCoords];
            //選択している駒と同じ座標を選択したら選択解除
            if(_selectingPiece == piece)
            {
                _selectingPiece = null;
                return;
            }

            //選択した座標にPlayerの駒があれば、駒を選択状態にして移動可能な座標を光らせる
            //既に選択状態でも上書きして行われる
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
                //移動可能な地点がなければ、選択を中止する
                if (movableRange.Count == 0 && attackableRange.Count == 0)
                {
                    return;
                }

                _selectingPiece = piece;
                OnShowRange?.Invoke((movableRange, attackableRange));
                return;
            }

            //駒が選択済みなら選択した移動可能な座標に移動する
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

                //移動の可否に関わらず選択を解除する
                _selectingPiece = null;

                //移動先で昇格する場合は移動を中止し、昇格候補を通知する
                if (promotionMovements.Count > 0)
                {
                    OnPromotion?.Invoke(promotionMovements);
                    return;
                }

                //選択した座標が移動可能な座標でなければ中止する
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