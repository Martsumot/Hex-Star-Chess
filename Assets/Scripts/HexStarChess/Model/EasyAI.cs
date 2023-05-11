using System;
using System.Collections.Generic;
using System.Linq;
namespace HexStarChess.Model
{
    public class EasyAI : IAI
    {
        private readonly bool _isPlayer;
        public bool IsPlayer => _isPlayer;

        public int AILevel { get; private set; } = 2;

        public EasyAI()
        {
            _isPlayer = false;
        }

        public void SetAILevel(int aiLevel)
        {
            AILevel = aiLevel;
        }

        public Movement SelectMovement(Board board)
        {
            List<Movement> allMovements = board.GetAllMovements(_isPlayer);
            allMovements = allMovements.OrderBy(x => Guid.NewGuid()).ToList();
            
            if(AILevel <= 0)
            {
                return allMovements[0];
            }

            List<int> movementScores = new();
            foreach (Movement movement in allMovements)
            {
                if(movement.CapturedPiece is not null)
                {
                    movementScores.Add(board.GetPieceScore(movement.CapturedPiece.Type));
                }
                else 
                {
                    movementScores.Add(0);
                }
                
            }

            if (AILevel == 1)
            {
                for(int i = 0; i < movementScores.Count; i++)
                {
                    if(movementScores[i] > 0)
                    {
                        return allMovements[i];
                    }
                }
                return allMovements[0];
            }

            int bestIndex = 0;
            int bestScore = 0;
            for(int i = 0; i < movementScores.Count; i++)
            {
                if(movementScores[i] > bestScore)
                {
                    bestScore = movementScores[i];
                    bestIndex = i;
                }
            }

            return allMovements[bestIndex];
        }
    }
}