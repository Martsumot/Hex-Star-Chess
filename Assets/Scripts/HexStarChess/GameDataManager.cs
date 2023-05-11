using System.IO;
using UnityEngine;
namespace HexStarChess
{
    [System.Serializable]
    public class GameDataManager
    {
        private static string _path = Path.Combine(Application.persistentDataPath, "GameData.json");

        public struct GameData
        {
            public bool isUserFirst;
            public int aiLevel;
        }

        private static GameDataManager _entity;
        public static GameDataManager Entity
        {
            get
            {
                if (_entity == null)
                {
                    Load();
                }
                return _entity;
            }
        }

        public bool IsPlayerFirst = true;

        public int AILevel = 0;

        public void SetFirst(bool isPlayerFirst)
        {
            IsPlayerFirst = isPlayerFirst;
            Save();
        }

        public void SetAILevel(int aiLevel)
        {
            AILevel = aiLevel;
            Save();
        }

        public void Save()
        {
            var json = JsonUtility.ToJson(Entity, false);
            File.WriteAllText(_path, json);
        }

        private static void Load()
        {
            if (!File.Exists(_path))
            {
                _entity = new();
                return;
            }

            var json = File.ReadAllText(_path);
            _entity = JsonUtility.FromJson<GameDataManager>(json);
        }
    }
}