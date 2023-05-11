using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using TMPro;

namespace HexStarChess.View
{
    public class TutorialPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _panelTitle;

        private List<string> _panelTitleTexts = new();

        [SerializeField]
        private TextMeshProUGUI _leftText;

        private List<string> _leftTexts = new();

        [SerializeField]
        private TextMeshProUGUI _rightText;

        private List<string> _rightTexts = new();

        [SerializeField]
        private Image _leftImage;

        [SerializeField]
        private List<Sprite> _leftSprites;

        [SerializeField]
        private Image _rightImage;

        [SerializeField]
        private List<Sprite> _rightSprites;

        private UnityEngine.Localization.Tables.StringTable _stringTable;

        private readonly int _panelCount = 5;

        private int _panelCurrentIndex = 0;
        
        private void Awake()
        {
            string tableName = "TextData";
            _stringTable = LocalizationSettings.StringDatabase.GetTable(tableName);
        }

        private void Start()
        {
            for(int i = 0; i < _panelCount; i++)
            {
                _panelTitleTexts.Add(_stringTable.GetEntry($"TutorialTitle{i}").Value);
                _leftTexts.Add(_stringTable.GetEntry($"TutorialLeftText{i}").Value);
                _rightTexts.Add(_stringTable.GetEntry($"TutorialRightText{i}").Value);
            }
            SetPanel(_panelCurrentIndex);
        }

        public void ChangeLeftPanel()
        {
            if(_panelCurrentIndex == 0)
            {
                _panelCurrentIndex = _panelCount;
            }
            _panelCurrentIndex--;
            SetPanel(_panelCurrentIndex);
        }

        public void ActivatePanel()
        {
            this.gameObject.SetActive(true);
        }

        public void ChangeRightPanel()
        {
            _panelCurrentIndex++;
            _panelCurrentIndex %= _panelCount;
            SetPanel(_panelCurrentIndex);
        }

        private void SetPanel(int index)
        {
            _panelTitle.text = _panelTitleTexts[index];
            _leftText.text = _leftTexts[index];
            _rightText.text = _rightTexts[index];
            _leftImage.sprite = _leftSprites[index];
            _rightImage.sprite = _rightSprites[index];
        }
    }
}