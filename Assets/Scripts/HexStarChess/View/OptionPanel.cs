using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using TMPro;
using System.Threading.Tasks;

namespace HexStarChess.View
{
    public class OptionPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _panelTitle;

        [SerializeField]
        private TextMeshProUGUI _backTitle;

        private UnityEngine.Localization.Tables.StringTable _stringTable;


        private void Awake()
        {
            string tableName = "TextData";
            _stringTable = LocalizationSettings.StringDatabase.GetTable(tableName);
        }

        private void Start()
        {
            _panelTitle.text = _stringTable.GetEntry("OptionTitle").Value;
            _backTitle.text = _stringTable.GetEntry("BackTitle").Value;
        }

        public void ActivatePanel()
        {
            this.gameObject.SetActive(true);
        }
    }
}