using UnityEngine;
using TMPro;
namespace HexStarChess.View
{
    public class AILevelChanger : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<TMP_Dropdown>().value = GameDataManager.Entity.AILevel;
        }
        public void Change(int aiLevel)
        {
            if(aiLevel == GameDataManager.Entity.AILevel)
            {
                return;
            }

            GameDataManager.Entity.SetAILevel(aiLevel);
        }
    }
}