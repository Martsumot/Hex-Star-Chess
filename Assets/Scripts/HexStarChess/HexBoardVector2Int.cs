namespace HexStarChess
{
    public class HexBoardVector2Int
    {
        public int x;
        public int y;

        public static readonly HexBoardVector2Int zero = new(0, 0);
        public static readonly HexBoardVector2Int up = new(0, 1);
        public static readonly HexBoardVector2Int down = new(0, -1);
        public static readonly HexBoardVector2Int left = new(-1, 0);
        public static readonly HexBoardVector2Int right = new(1, 0);
        public static readonly HexBoardVector2Int oddUpLeft = new(-1, 1);
        public static readonly HexBoardVector2Int evenUpLeft = new(-1, 0);
        public static readonly HexBoardVector2Int oddUpRight = new(1, 1);
        public static readonly HexBoardVector2Int evenUpRight = new(1, 0);
        public static readonly HexBoardVector2Int oddDownLeft = new(-1, 0);
        public static readonly HexBoardVector2Int evenDownLeft = new(-1, -1);
        public static readonly HexBoardVector2Int oddDownRight = new(1, 0);
        public static readonly HexBoardVector2Int evenDownRight = new(1, -1);

        public HexBoardVector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static HexBoardVector2Int operator+ (HexBoardVector2Int a, HexBoardVector2Int b)
        {
            return new(a.x + b.x, a.y + b.y);
        }
        public static HexBoardVector2Int operator- (HexBoardVector2Int a, HexBoardVector2Int b)
        {
            return new(a.x - b.x, a.y - b.y);
        }
        public static HexBoardVector2Int operator* (HexBoardVector2Int a, int b)
        {
            return new(a.x * b, a.y * b);
        }

        public static bool operator== (HexBoardVector2Int a, HexBoardVector2Int b)
        {
            if(a.x != b.x || a.y != b.y)
            {
                return false;
            }
            return true;
        }

        public static bool operator !=(HexBoardVector2Int a, HexBoardVector2Int b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }
            return (this == (HexBoardVector2Int)obj);
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }
    }
}
