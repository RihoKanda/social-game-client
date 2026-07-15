using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SocialGameClient.API;
using System.Collections;

/// インゲーム移行後放置報酬パネル提示　受け取る→確定後ホーム画面に切替
namespace SocialGameClient.Game
{
    public class RewardController : MonoBehaviour
    {
        [SerializeField] private GameObject rewardPanel;
        [SerializeField] private TextMeshProUGUI pendingText;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private Button claimButton;
        [SerializeField] private HomeController homeController;

        [Header("獲得ポップアップ")]
        [SerializeField] private GameObject gainPopup;
        [SerializeField] private TextMeshProUGUI gainPopupText;
        [SerializeField] private float popupDuration = 1f;

        private void Start()
        {
            claimButton.interactable = false;
            coinText.text = "";
            if (gainPopup != null) gainPopup.SetActive(false);

            claimButton.onClick.AddListener(OnClaimClicked);

            // 表示用　確定前の放置分を取得
            ApiClient.Instance.GetState(
                onSuccess: state =>
                {
                    pendingText.text = $"おかえりなさい \n{state.pending_gain} コインがたまっています";
                    coinText.text = $"コイン: {state.coin}";
                    claimButton.interactable = true;
                },
                onError: err =>
                {
                    pendingText.text = "取得に失敗しました: " + err;
                }
            );
        }

        private void OnClaimClicked()
        {
            claimButton.interactable = false;

            ApiClient.Instance.Claim(
                onSuccess: result =>
                {
                    homeController.SetCoin(result.new_coin);   // ホーム画面のコインを加算後の値に更新
                    StartCoroutine(ShowGainPopupThenClose(result.gained));
                },
                onError: err =>
                {
                    pendingText.text = "受け取りに失敗しました: " + err;
                    claimButton.interactable = true;
                }
            );
        }

        private IEnumerator ShowGainPopupThenClose(long gained)
        {
            if (gainPopup != null)
            {
                gainPopupText.text = $"+{gained} コイン";
                gainPopup.SetActive(true);
            }

            yield return new WaitForSeconds(popupDuration);

            rewardPanel.SetActive(false); // 受け取ったらパネルを閉じる
        }
    }
};