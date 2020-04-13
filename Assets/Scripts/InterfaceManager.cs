using UnityEditor;
using UnityEngine; 
using UnityEngine.UI;

namespace GameManagers
{
    public class InterfaceManager : MonoBehaviour
    {
        [SerializeField] private Text _versionText;
        [SerializeField] private Text _platformText;

        private void Start() {
            MainGameManager.OnProjectVersionChanged += OnVersionChanged;
        }

        private void OnVersionChanged() {
            _versionText.text = string.Format("Build Version: {0}", Application.version);

            string _currentPlatform = "";
            switch (Application.platform)
            {
                case RuntimePlatform.Android: _currentPlatform = "Android";
                    break;
                case RuntimePlatform.WindowsPlayer: _currentPlatform = "Windows";
                    break;
                case RuntimePlatform.WindowsEditor: _currentPlatform = "Windows Editor";
                    break;
            }

            _platformText.text = string.Format("This is an {0} build!", _currentPlatform);
        }
    }
}
