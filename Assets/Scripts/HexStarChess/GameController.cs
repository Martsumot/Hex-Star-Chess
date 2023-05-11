using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
namespace HexStarChess
{
    using Model;
    using View;
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        private Game _game;

        [SerializeField]
        private PlayerInput _playerInput;

        [SerializeField]
        private BoardView _boardView;

        [SerializeField]
        private PiecesView _piecesView;

        [SerializeField]
        private GameUIView _uiView;

        [SerializeField]
        private float _piecePositionZ = 0;

        private Dictionary<PieceBase, GameObject> _upForwardPieceGameObjectPairs = new();
        private Dictionary<PieceBase, GameObject> _downForwardPieceGameObjectPairs = new();

        private bool _isPromotionTime = false;

        private List<Movement> _promotionMovements = null;
        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
        }

        private void Start()
        {
            _playerInput.OnPointerEnterTile += PointerEnterTile;
            _playerInput.OnSelectTile += SelectTile;
            
            _game = new(GameDataManager.Entity.IsPlayerFirst);
            _game.OnShowRange += ShowRange;
            _game.OnMoved += ReflectMovement;
            _game.OnPromotion += ShowPromotion;
            _game.OnWin += End;
            _uiView.OnSelectPromotion += PromotePawn;
            _boardView.CreateBoard(_game.GameBoard.PiecesOnBoard.Keys.ToList());
            foreach(PieceBase piece in _game.GameBoard.PlayerPieces)
            {
                AddPiece(piece);
            }
            foreach (PieceBase piece in _game.GameBoard.EnemyPieces)
            {
                AddPiece(piece);
            }
            _uiView.ChangeTurnText(_game.IsPlayerTurn);
            _ = GameRoop();
        }

        private async Task GameRoop()
        {
            if (!_game.IsPlayerTurn)
            {
                Task t = _game.SelectByEnemy();
                await Task.WhenAll(t);
            }
            await Task.Delay(100);
            if (_game.IsPlaying)
            {
                _ = GameRoop();
            }
        }

        private void ReflectMovement(Movement movement)
        {
            _boardView.ResetHighlight();
            Dictionary<PieceBase, GameObject> targetPieceGameObjectPairs = movement.MovingPiece.IsPlayer ? _upForwardPieceGameObjectPairs : _downForwardPieceGameObjectPairs;
            if(!targetPieceGameObjectPairs.ContainsKey(movement.MovingPiece))
            {
                return;
            }
            targetPieceGameObjectPairs[movement.MovingPiece].transform.position = HexCoordsConverter.ConvertWorldPosition(movement.Destination, _piecePositionZ);
            if (movement.PromotionType != PieceType.None)
            {
                RemovePiece(movement.MovingPiece);
                AddPiece(_game.GameBoard.PiecesOnBoard[movement.Destination]);
            }

            if (movement.CapturedPiece != null)
            {
                RemovePiece(movement.CapturedPiece);
            }
            _uiView.ChangeTurnText(_game.IsPlayerTurn);
        }
        private void AddPiece(PieceBase piece)
        {
            HexBoardVector2Int hexCoords = _game.GameBoard.GetPiecePosition(piece);
            Vector3 position = HexCoordsConverter.ConvertWorldPosition(hexCoords, _piecePositionZ);
            Dictionary<PieceBase, GameObject> targetPieceGameObjectPairs = piece.IsPlayer ? _upForwardPieceGameObjectPairs : _downForwardPieceGameObjectPairs;
            targetPieceGameObjectPairs.Add(piece, _piecesView.CreatePiece(piece.Type, piece.IsPlayer, position));
        }
        
        private void RemovePiece(PieceBase piece)
        {
            Dictionary<PieceBase, GameObject> targetPieceGameObjectPairs = piece.IsPlayer ? _upForwardPieceGameObjectPairs : _downForwardPieceGameObjectPairs;
            GameObject pieceGameObject = targetPieceGameObjectPairs[piece];
            targetPieceGameObjectPairs.Remove(piece);
            Destroy(pieceGameObject);
        }
        private void PointerEnterTile(Tile tile)
        {
            if(_isPromotionTime)
            {
                return;
            }
            _boardView.PointerEnterTile(tile);
        }
        private void SelectTile(Tile tile)
        {
            if(!_game.IsPlayerTurn || !_game.IsPlaying)
            {
                return;
            }

            if(_isPromotionTime)
            {
                return;
            }

            _boardView.ResetHighlight();
            HexBoardVector2Int hexCoords = _boardView.TileToHexCoords(tile);
            _game.Select(hexCoords);
        }

        private void ShowRange((List<HexBoardVector2Int> movableRange, List<HexBoardVector2Int> attackableRange) rangeTuple)
        {
            _boardView.ShowRange(rangeTuple.movableRange, rangeTuple.attackableRange);
        }

        private void ShowPromotion(List<Movement> movements)
        {
            _uiView.ShowPromotionUI();
            _promotionMovements = movements;
            _isPromotionTime = true;
        }

        private void PromotePawn(PieceType type)
        {
            Movement promotionMovement = null;
            foreach(Movement movement in _promotionMovements)
            {
                if(movement.PromotionType == type)
                {
                    promotionMovement = movement;
                }
            }
            
            _game.MovePiece(promotionMovement);
            _promotionMovements = null;
            _isPromotionTime = false;
        }

        private void End(bool isPlayerWin)
        {
            _uiView.ShowEndUI(isPlayerWin);
        }
    }
}