using System;
using UnityEngine;

namespace GameManagers
{
    public class MainGameManager : MonoBehaviour
    {
        public static float GameVersion;
        private const float _versionIncrement = 0.001f;

        [SerializeField] private string _dataPath = "Assets/Data/GameVersion.json";

        public delegate void ProjectVersionChanged();
        public static event ProjectVersionChanged OnProjectVersionChanged;

        private void Start() {
            JsonReadWrite.CreateJsonData(_dataPath, _versionIncrement.ToString("0.000"));
            GameVersion = float.Parse(JsonReadWrite.GetJsonString(_dataPath));
            OnProjectVersionChanged();
        }

        public void PerformBuild() {
            GameVersion += _versionIncrement;
            JsonReadWrite.SetJsonString(_dataPath, GameVersion.ToString("0.000"));
            OnProjectVersionChanged();
        }
    }
}
