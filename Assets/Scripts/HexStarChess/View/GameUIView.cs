using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HexStarChess.View
{
    public class GameUIView : MonoBehaviour
    {
        [SerializeField]
        private GameMessenger _gameMessanger;
        
        [SerializeField]
        private TutorialPanel _tutorialPanel;
        [SerializeField]
        private OptionPanel _optionPanel;
        [SerializeField]
        private GameObject _promotionButtons;
        [SerializeField]
        private GameObject _backTitleButton;


        public Action<PieceType> OnSelectPromotion;
        public void ShowTutorialUI()
        {
            _tutorialPanel.ActivatePanel();
        }
        public void ShowOptionUI()
        {
            _optionPanel.ActivatePanel();
        }
        public void ShowPromotionUI()
        {
            _promotionButtons.SetActive(true);
            _gameMessanger.SetMessage(GameMessageType.Promotion);
        }

        public void SelectPromotion(int promotionNumber)
        {
            PieceType type;
            switch(promotionNumber)
            {
                case 0:
                    type = PieceType.Queen;
                    break;
                case 1:
                    type = PieceType.Bishop;
                    break;
                case 2:
                    type = PieceType.Rook;
                    break;
                case 3:
                    type = PieceType.Knight;
                    break;
                default:
                    return;
            }
            _promotionButtons.SetActive(false);
            OnSelectPromotion?.Invoke(type);
        }

        public void ChangeTurnText(bool isPlayerTurn)
        {
            _gameMessanger.SetMessage(isPlayerTurn ? GameMessageType.PlayerTurn : GameMessageType.EnemyTurn);
        }

        public void ShowEndUI(bool isPlayerWin)
        {
            _backTitleButton.SetActive(true);
            _gameMessanger.SetMessage(isPlayerWin ? GameMessageType.PlayerWin : GameMessageType.EnemyWin);
        }
    }
}