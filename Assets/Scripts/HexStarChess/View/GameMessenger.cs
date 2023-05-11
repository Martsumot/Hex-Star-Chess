using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;
namespace HexStarChess.View
{
    public class GameMessenger : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _gameMessage;

        private List<string> _messages = new();

        private UnityEngine.Localization.Tables.StringTable _stringTable;
        private void Awake()
        {
            string tableName = "TextData";
            _stringTable = LocalizationSettings.StringDatabase.GetTable(tableName);
            for (GameMessageType i = 0; i < GameMessageType.Max; i++)
            {
                _messages.Add(_stringTable.GetEntry($"GameMessage{(int)i}").Value);
            }
        }

        public void SetMessage(GameMessageType gameMessageType)
        {
            _gameMessage.text = _messages[(int)gameMessageType];
        }

    }

    public enum GameMessageType
    {
        PlayerTurn,
        EnemyTurn,
        PlayerWin,
        EnemyWin,
        Promotion,
        Max
    }
}