using UnityEngine; using UnityEngine.UI;

namespace GameManagers
{
    public class InterfaceManager : MonoBehaviour
    {
        [SerializeField] private Text _versionText;
        [SerializeField] private Text _platformText;

        private void Awake() {
            MainGameManager.OnProjectVersionChanged += OnVersionChanged;
        }

        private void OnVersionChanged() {
            _versionText.text = string.Format("v{0}", MainGameManager.GameVersion.ToString("0.000"));
            _platformText.text = string.Format("This is an {0} build!", Application.platform);
        }
    }
}
