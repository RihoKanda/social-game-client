using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SocialGameClient.API;
using SocialGameClient.Auth;

namespace SocialGameClient.Game
{
    public class HomeController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI pendingText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Button claimButton;

        private void Start()
        {
            claimButton.interactable = false;
            SetStatus("ログイン中...");

            AuthManager.Instance.Login(
                onSuccess: RefreshState,
                onError: err => SetStatus("ログイン失敗: " + err)
            );

            claimButton.onClick.AddListener(OnClaimClicked);
        }

        private void RefreshState()
        {
            SetStatus("読み込み中...");

            ApiClient.Instance.GetState(
                onSuccess: state =>
                {
                    coinText.text = $"コイン: {state.coin}";
                    pendingText.text = $"未受取り: {state.pending_gain}";
                    claimButton.interactable = true;
                    SetStatus("");
                },
                onError: err => SetStatus("取得失敗: " + err)
            );
        }

        private void OnClaimClicked()
        {
            claimButton.interactable = false;
            SetStatus("受け取り中...");

            ApiClient.Instance.Claim(
                onSuccess: result =>
                {
                    coinText.text = $"コイン: {ReadResult.new_coin}";
                    pendingText.text = "未受取り: 0";
                    SetStatus($"{result.gained} コイン獲得");
                    claimButton.interactable = true;
                },
                onError: err =>
                {
                    SetStatus("受け取り失敗: " + err);
                    claimButton.interactable = true;
                }
            );
        }

        private void SetStatus(string message)
        {
            if (statusText != null) statusText.text = message;
        }
    }
}