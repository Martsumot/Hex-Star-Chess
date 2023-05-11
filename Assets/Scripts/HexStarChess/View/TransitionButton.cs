using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using TMPro;

namespace HexStarChess.View 
{
    [RequireComponent(typeof(Button))]
    public class TransitionButton : MonoBehaviour
    {
        [SerializeField]
        private SceneName _sceneName;

        [SerializeField]
        private TextMeshProUGUI _text;

        private UnityEngine.Localization.Tables.StringTable _stringTable;

        private void Awake()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => Transition(_sceneName));
            string tableName = "TextData";
            _stringTable = LocalizationSettings.StringDatabase.GetTable(tableName);
            _text.text = _stringTable.GetEntry($"{_sceneName.ToString()}TransitionText").Value;

        }

        private void Transition(SceneName sceneName)
        {
            SceneManager.LoadScene(sceneName.ToString());
        }
    }

    public enum SceneName
    {
        TitleScene,
        GameScene
    }
}