using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace HexStarChess.View
{
    /// <summary>
    /// æèŒãè‚ğ•Ï‚¦‚éUI
    /// </summary>
    public class FirstMoveChanger : MonoBehaviour
    {
        [SerializeField]
        private Toggle _firstMove;
        [SerializeField]
        private Toggle _secondMove;

        private void Start()
        {
            _firstMove.onValueChanged.AddListener(Change); 
            if(GameDataManager.Entity.IsPlayerFirst)
            {
                _firstMove.isOn = true;
            }
            else
            {
                _secondMove.isOn = true;
            }
        }
        public void Change(bool isUserFirst)
        {
            if(isUserFirst == GameDataManager.Entity.IsPlayerFirst)
            {
                return;
            }
            GameDataManager.Entity.SetFirst(isUserFirst);
        }
    }
}