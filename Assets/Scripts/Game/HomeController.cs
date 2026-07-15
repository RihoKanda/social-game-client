using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SocialGameClient.API;
using SocialGameClient.Auth;

/// ホーム画面コイン表示　
namespace SocialGameClient.Game
{
    public class HomeController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;

        [Header("スタブ(未実装)")]
        [SerializeField] private Button trainingButton;      // 育成
        [SerializeField] private Button shopButton;          // ショップ
        [SerializeField] private Button atkUpButton;         // 攻撃力UP
        [SerializeField] private Button hpUpButton;          // 体力UP
        [SerializeField] private Button healUpButton;        // 回復力UP

        private long currentCoin;

        private void Start()
        {
            trainingButton.onClick.AddListener(() => Debug.Log("育成機能は未実装です"));
            shopButton.onClick.AddListener(() => Debug.Log("ショップ機能は未実装です"));
            atkUpButton.onClick.AddListener(() => Debug.Log("攻撃力UPは未実装です"));
            hpUpButton.onClick.AddListener(() => Debug.Log("体力UPは未実装です"));
            healUpButton.onClick.AddListener(() => Debug.Log("回復力UPは未実装です"));

            RefreshFromServer();
        }

        public void RefreshFromServer()
        {
            SocialGameClient.API.ApiClient.Instance.GetState(
                onSuccess: state => SetCoin(state.coin),
                onError: err => Debug.LogWarning("コイン取得失敗: " + err)
            );
        }

        public void SetCoin(long coin)
        {
            currentCoin = coin;
            coinText.text = $"コイン: {currentCoin}";
        }
    }
}