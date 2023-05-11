namespace HexStarChess.Model
{
    public interface IAI
    {
        public abstract Movement SelectMovement(Board board);

        public abstract void SetAILevel(int aiLevel);
    }
}