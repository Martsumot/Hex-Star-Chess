using System.Collections.Generic;
using System.Linq;

namespace HexStarChess.Model
{
    public class Board
    {
        private Dictionary<HexBoardVector2Int, PieceBase> _piecesOnBoard = new()
        {
            { new HexBoardVector2Int(0, 3), null },

            { new HexBoardVector2Int(1, 2), null },
            { new HexBoardVector2Int(1, 3), null },

            { new HexBoardVector2Int(2, 0), null },
            { new HexBoardVector2Int(2, 1), null },
            { new HexBoardVector2Int(2, 2), null },
            { new HexBoardVector2Int(2, 3), null },
            { new HexBoardVector2Int(2, 4), null },
            { new HexBoardVector2Int(2, 5), null },
            { new HexBoardVector2Int(2, 6), null },

            { new HexBoardVector2Int(3, 0), null },
            { new HexBoardVector2Int(3, 1), null },
            { new HexBoardVector2Int(3, 2), null },
            { new HexBoardVector2Int(3, 3), null },
            { new HexBoardVector2Int(3, 4), null },
            { new HexBoardVector2Int(3, 5), null },

            { new HexBoardVector2Int(4, 1), null },
            { new HexBoardVector2Int(4, 2), null },
            { new HexBoardVector2Int(4, 3), null },
            { new HexBoardVector2Int(4, 4), null },
            { new HexBoardVector2Int(4, 5), null },

            { new HexBoardVector2Int(5, 0), null },
            { new HexBoardVector2Int(5, 1), null },
            { new HexBoardVector2Int(5, 2), null },
            { new HexBoardVector2Int(5, 3), null },
            { new HexBoardVector2Int(5, 4), null },
            { new HexBoardVector2Int(5, 5), null },

            { new HexBoardVector2Int(6, 0), null },
            { new HexBoardVector2Int(6, 1), null },
            { new HexBoardVector2Int(6, 2), null },
            { new HexBoardVector2Int(6, 3), null },
            { new HexBoardVector2Int(6, 4), null },
            { new HexBoardVector2Int(6, 5), null },
            { new HexBoardVector2Int(6, 6), null },

            { new HexBoardVector2Int(7, 2), null },
            { new HexBoardVector2Int(7, 3), null },

            { new HexBoardVector2Int(8, 3), null }
        };

        public IReadOnlyDictionary<HexBoardVector2Int, PieceBase> PiecesOnBoard => _piecesOnBoard;

        private List<PieceBase> _playerPieces = new();

        public IReadOnlyList<PieceBase> PlayerPieces => _playerPieces;

        private List<PieceBase> _enemyPieces = new();        

        public IReadOnlyList<PieceBase> EnemyPieces => _enemyPieces;
        
        private List<PieceBase> _movedPawn = new();
        
        public IReadOnlyList<PieceBase> MovedPawn => _movedPawn;

        private Stack<Movement> _history = new();

        public bool IsPlaying;

        private bool _isSimulation = false;

        public Board()
        {
            AddPiece(PieceType.King, true, new HexBoardVector2Int(6, 0));
            AddPiece(PieceType.Knight, true, new HexBoardVector2Int(5, 0));
            AddPiece(PieceType.Bishop, true, new HexBoardVector2Int(4, 1));
            AddPiece(PieceType.Queen, true, new HexBoardVector2Int(3, 0));
            AddPiece(PieceType.Rook, true, new HexBoardVector2Int(2, 0));
            AddPiece(PieceType.Pawn, true, new HexBoardVector2Int(6, 1));
            AddPiece(PieceType.Pawn, true, new HexBoardVector2Int(5, 1));
            AddPiece(PieceType.Pawn, true, new HexBoardVector2Int(4, 2));
            AddPiece(PieceType.Pawn, true, new HexBoardVector2Int(3, 1));
            AddPiece(PieceType.Pawn, true, new HexBoardVector2Int(2, 1));

            AddPiece(PieceType.King, false, new HexBoardVector2Int(2, 6));
            AddPiece(PieceType.Knight, false, new HexBoardVector2Int(3, 5));
            AddPiece(PieceType.Bishop, false, new HexBoardVector2Int(4, 5));
            AddPiece(PieceType.Queen, false, new HexBoardVector2Int(5, 5));
            AddPiece(PieceType.Rook, false, new HexBoardVector2Int(6, 6));
            AddPiece(PieceType.Pawn, false, new HexBoardVector2Int(2, 5));
            AddPiece(PieceType.Pawn, false, new HexBoardVector2Int(3, 4));
            AddPiece(PieceType.Pawn, false, new HexBoardVector2Int(4, 4));
            AddPiece(PieceType.Pawn, false, new HexBoardVector2Int(5, 4));
            AddPiece(PieceType.Pawn, false, new HexBoardVector2Int(6, 5));
            IsPlaying = true;
        }
        public Board(Board board)
        {
            _piecesOnBoard = new(board.PiecesOnBoard);
            _playerPieces = new(board.PlayerPieces);
            _enemyPieces = new(board.EnemyPieces);
            _movedPawn = new(_movedPawn);
            _isSimulation = true;
        }
        
        public int GetBoardScore(bool isUpForward)
        {
            int upForwardScore = 0;
            int downForwardScore = 0;
            foreach(PieceBase piece in _playerPieces)
            {
                upForwardScore += GetPieceScore(piece.Type);
            }
            foreach (PieceBase piece in _enemyPieces)
            {
                downForwardScore += GetPieceScore(piece.Type);
            }
            return isUpForward ? upForwardScore - downForwardScore : downForwardScore - upForwardScore;
        }

        public int GetPieceScore(PieceType pieceType)
        {
            return pieceType switch
            {
                PieceType.King => 1000,
                PieceType.Queen => 9,
                PieceType.Rook or PieceType.Bishop or PieceType.Knight => 3,
                PieceType.Pawn => 1,
                _ => 0,
            };
        }

        private PieceBase AddPiece(PieceType pieceType, bool isUpForward, HexBoardVector2Int position)
        {
            if (!_piecesOnBoard.ContainsKey(position) || _piecesOnBoard[position] != null)
            {
                return null;
            }

            PieceBase piece = null;
            switch (pieceType)
            {
                case PieceType.King:
                    piece = new PieceKing(isUpForward);
                    break;
                case PieceType.Queen:
                    piece = new PieceQueen(isUpForward);
                    break;
                case PieceType.Rook:
                    piece = new PieceRook(isUpForward);
                    break;
                case PieceType.Bishop:
                    piece = new PieceBishop(isUpForward);
                    break;
                case PieceType.Knight:
                    piece = new PieceKnight(isUpForward);
                    break;
                case PieceType.Pawn:
                    piece = new PiecePawn(isUpForward);
                    break;
            }

            _piecesOnBoard[position] = piece;
            if(isUpForward)
            {
                _playerPieces.Add(piece);
            }
            else
            {
                _enemyPieces.Add(piece);
            }

            return piece;
        }
        
        public void MovePiece(Movement movement)
        {
            if(!_piecesOnBoard.ContainsKey(movement.Origin) || !_piecesOnBoard.ContainsKey(movement.Destination))
            {
                return;
            }

            if(_piecesOnBoard[movement.Destination] != null)
            {
                movement.SetCapturedPiece(_piecesOnBoard[movement.Destination]);
                CapturePeice(movement.CapturedPiece);
            }

            PieceBase piece = movement.MovingPiece;

            if (movement.PromotionType != PieceType.None)
            {
                piece = PromotePawn(piece, movement.PromotionType);
            }

            _piecesOnBoard[movement.Origin] = null;
            _piecesOnBoard[movement.Destination] = piece;
            if(piece.Type == PieceType.Pawn && !_movedPawn.Contains(piece))
            {
                _movedPawn.Add(piece);
                movement.MovePawnFirst();
            }
            _history.Push(movement);
        }

        public void CancelMovement()
        {
            if(_history.Count == 0)
            {
                return;
            }

            Movement movement = _history.Pop();
            if (movement.PromotionType != PieceType.None)
            {
                if(movement.MovingPiece.IsPlayer)
                {
                    _playerPieces.Remove(_piecesOnBoard[movement.Destination]);
                }
                else
                {
                    _enemyPieces.Remove(_piecesOnBoard[movement.Destination]);
                }
            }
            _piecesOnBoard[movement.Destination] = null;
            _piecesOnBoard[movement.Origin] = movement.MovingPiece;

            

            if(movement.CapturedPiece != null)
            {
                _piecesOnBoard[movement.Destination] = movement.CapturedPiece;
                if(movement.CapturedPiece.IsPlayer)
                {
                    _playerPieces.Add(movement.CapturedPiece);
                }
                else
                {
                    _enemyPieces.Add(movement.CapturedPiece);
                }
            }

            if(movement.IsFirstMovingPawn)
            {
                _movedPawn.Remove(movement.MovingPiece);
            }
        }

        public HexBoardVector2Int GetPiecePosition(PieceBase piece)
        {
            return _piecesOnBoard.FirstOrDefault(x => x.Value == piece).Key;
        }

        private void CapturePeice(PieceBase piece)
        {
            if(piece.Type == PieceType.King && !_isSimulation)
            {
                IsPlaying = false;
            }
            RemovePiece(piece);
        }

        private PieceBase PromotePawn(PieceBase pawn, PieceType promotionType)
        {
            HexBoardVector2Int position = GetPiecePosition(pawn);
            RemovePiece(pawn);
            PieceBase piece = AddPiece(promotionType, pawn.IsPlayer, position);
            return piece;
        }

        private void RemovePiece(PieceBase piece)
        {
            _piecesOnBoard[GetPiecePosition(piece)] = null;
            if(piece.IsPlayer)
            {
                _playerPieces.Remove(piece);
            }
            else
            {
                _enemyPieces.Remove(piece);
            }

            if(piece.Type == PieceType.Pawn && _movedPawn.Contains(piece))
            {
                _movedPawn.Remove(piece);
            }
        }

        public List<Movement> GetAllMovements(bool isUpForward)
        {
            List<Movement> allMovements = new();
            List<PieceBase> pieces = isUpForward ? _playerPieces : _enemyPieces;
            
            foreach(PieceBase piece in pieces)
            {
                allMovements.AddRange(piece.GetAllMovements(this));
            }

            return allMovements;            
        }
    }
}