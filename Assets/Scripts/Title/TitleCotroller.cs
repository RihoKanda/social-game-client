using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using SocialGameClient.Auth;

/// ログイン　処理中ローディング　成功時インゲーム移行
namespace SocialGameClient.Title
{
    public class TitleController : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private GameObject loadingIndicator;
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private string inGameSceneName = "InGameScene";

        private void Start()
        {
            loadingIndicator.SetActive(false);
            if (errorText != null) errorText.text = "";

            startButton.onClick.AddListener(OnStartClicked);
        }

        private void OnStartClicked()
        {
            startButton.interactable = false;
            loadingIndicator.SetActive(true);
            if (errorText != null) errorText.text = "";

            AuthManager.Instance.Login(
                onSuccess: () =>
                {
                    SceneManager.LoadScene(inGameSceneName);
                },
                onError: err =>
                {
                    loadingIndicator.SetActive(false);
                    startButton.interactable = true;
                    if (errorText != null) errorText.text = "ログイン失敗: " + err;
                } 
            );
        }
    }
}


