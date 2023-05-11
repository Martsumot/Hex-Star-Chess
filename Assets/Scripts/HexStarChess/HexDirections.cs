using System.Collections.Generic;
namespace HexStarChess
{
    public static class HexDirections
    {
        public static List<HexBoardVector2Int> RookDirections = new()
        {
            HexBoardVector2Int.up,
            HexBoardVector2Int.down
        };

        public static List<HexBoardVector2Int> OddBishopDirections = new()
        {
            HexBoardVector2Int.oddUpLeft,
            HexBoardVector2Int.oddUpRight,
            HexBoardVector2Int.oddDownLeft,
            HexBoardVector2Int.oddDownRight
        };

        public static List<HexBoardVector2Int> EvenBishopDirections = new()
        {
            HexBoardVector2Int.evenUpLeft,
            HexBoardVector2Int.evenUpRight,
            HexBoardVector2Int.evenDownLeft,
            HexBoardVector2Int.evenDownRight
        };

        public static List<HexBoardVector2Int> OddKnightDirection = new()
        {
            HexBoardVector2Int.up * 2 + HexBoardVector2Int.left * 2,
            HexBoardVector2Int.up * 2 + HexBoardVector2Int.right * 2,
            HexBoardVector2Int.down * 2 + HexBoardVector2Int.left * 2,
            HexBoardVector2Int.down * 2 + HexBoardVector2Int.right * 2,

            HexBoardVector2Int.up * 2 + HexBoardVector2Int.oddUpLeft,
            HexBoardVector2Int.up * 2 + HexBoardVector2Int.oddUpRight,
            HexBoardVector2Int.down * 2 + HexBoardVector2Int.oddDownLeft,
            HexBoardVector2Int.down * 2 + HexBoardVector2Int.oddDownRight,
            HexBoardVector2Int.left * 2 + HexBoardVector2Int.oddUpLeft,
            HexBoardVector2Int.left * 2 + HexBoardVector2Int.oddDownLeft,
            HexBoardVector2Int.right * 2 + HexBoardVector2Int.oddUpRight,
            HexBoardVector2Int.right * 2 + HexBoardVector2Int.oddDownRight
        };

        public static List<HexBoardVector2Int> EvenKnightDirections = new()
        {
            HexBoardVector2Int.up * 2 + HexBoardVector2Int.left * 2,
            HexBoardVector2Int.up * 2 + HexBoardVector2Int.right * 2,
            HexBoardVector2Int.down * 2 + HexBoardVector2Int.left * 2,
            HexBoardVector2Int.down * 2 + HexBoardVector2Int.right * 2,

            HexBoardVector2Int.up * 2 + HexBoardVector2Int.evenUpLeft,
            HexBoardVector2Int.up * 2 + HexBoardVector2Int.evenUpRight,
            HexBoardVector2Int.down * 2 + HexBoardVector2Int.evenDownLeft,
            HexBoardVector2Int.down * 2 + HexBoardVector2Int.evenDownRight,
            HexBoardVector2Int.left * 2 + HexBoardVector2Int.evenUpLeft,
            HexBoardVector2Int.left * 2 + HexBoardVector2Int.evenDownLeft,
            HexBoardVector2Int.right * 2 + HexBoardVector2Int.evenUpRight,
            HexBoardVector2Int.right * 2 + HexBoardVector2Int.evenDownRight

        };

        public static List<HexBoardVector2Int> OddPlayerPawnCaptureDirections = new()
        {
            HexBoardVector2Int.oddUpLeft,
            HexBoardVector2Int.oddUpRight
        };

        public static List<HexBoardVector2Int> EvenPlayerPawnCaptureDirections = new()
        {
            HexBoardVector2Int.evenUpLeft,
            HexBoardVector2Int.evenUpRight
        };

        public static List<HexBoardVector2Int> OddEnemyPawnCaptureDirections = new()
        {
            HexBoardVector2Int.oddDownLeft,
            HexBoardVector2Int.oddDownRight
        };

        public static List<HexBoardVector2Int> EvenEnemyPawnCaptureDirections = new()
        {
            HexBoardVector2Int.evenDownLeft,
            HexBoardVector2Int.evenDownRight
        };
    }
}
