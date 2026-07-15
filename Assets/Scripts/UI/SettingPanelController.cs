using UnityEngine;
using UnityEngine.UI;
using SocialGameClient.Core;

/// 設定パネルの開閉　音量調節バー
namespace SocialGameClient.UI
{
    public class SettingPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Slider volumeSlider;

        private void Start()
        {
            settingsPanel.SetActive(false);

            openButton.onClick.AddListener(OpenPanel);
            closeButton.onClick.AddListener(ClosePanel);
            volumeSlider.onValueChanged.AddListener(VolumeManager.Instance.SetVolume);
        }

        private void OpenPanel()
        {
            volumeSlider.SetValueWithoutNotify(VolumeManager.Instance.CurrentVolume);
            settingsPanel.SetActive(true);
        }

        private void ClosePanel()
        {
            settingsPanel.SetActive(false);
        }
    }
}

