namespace HexStarChess.Model
{
    public class Movement
    {
        public PieceBase MovingPiece { get; private set; } = null;
        public HexBoardVector2Int Origin { get; private set; } = null;
        public HexBoardVector2Int Destination { get; private set; } = null;
        public PieceBase CapturedPiece { get; private set; } = null;
        //Pawnが一番最初に動くMovementのみtrue
        //AIのシミュレーションで正しく動かすために必要
        public bool IsFirstMovingPawn { get; private set; } = false;
        public PieceType PromotionType { get; private set; } = PieceType.None;

        public Movement() { }

        public Movement(PieceBase movingPiece, HexBoardVector2Int origin, HexBoardVector2Int destination)
        {
            SetMovement(movingPiece, origin, destination, null, false, PieceType.None);
        }

        public Movement(Movement movement)
        {
            SetMovement(movement.MovingPiece, movement.Origin, movement.Destination, movement.CapturedPiece, movement.IsFirstMovingPawn , movement.PromotionType);
        }

        public Movement(Movement movement, PieceType promotionType)
        {
            SetMovement(movement.MovingPiece, movement.Origin, movement.Destination, movement.CapturedPiece, movement.IsFirstMovingPawn, promotionType);
        }

        private void SetMovement(PieceBase movingPiece, HexBoardVector2Int origin, HexBoardVector2Int destination, PieceBase capturedPiece, bool isFirstMovingPawn, PieceType promotionType)
        {
            MovingPiece = movingPiece;
            Origin = origin;
            Destination = destination;
            CapturedPiece = capturedPiece;
            IsFirstMovingPawn = isFirstMovingPawn;
            if (promotionType == PieceType.King || promotionType == PieceType.Pawn)
            {
                PromotionType = PieceType.None;
            }
            else
            {
                PromotionType = promotionType;
            }
        }
        public void MovePawnFirst()
        {
            IsFirstMovingPawn = true;
        }
        public void SetCapturedPiece(PieceBase piece)
        {
            CapturedPiece = piece;
        }
    }
}
