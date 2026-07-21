using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SocialGameClient.API;

namespace SocialGameClient.Game
{
    public class ShopPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private Button backButton;
        [SerializeField] private Button gachaButton;
        [SerializeField] private HomeController homeController;

        [Header("確認ダイアログ")]
        [SerializeField] private GameObject confirmDialog;
        [SerializeField] private TextMeshProUGUI confirmText;
        [SerializeField] private Button confirmYesButton;
        [SerializeField] private Button confirmNoButton;

        [Header("結果ポップアップ")]
        [SerializeField] private GameObject resultPopup;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Button resultCloseButton;

        private const int GachaCost = 100;

        private void Start()
        {
            backButton.onClick.AddListener(Close);
            gachaButton.onClick.AddListener(OpenConfirmDialog);
            confirmYesButton.onClick.AddListener(OnConfirmYes);
            confirmNoButton.onClick.AddListener(() => confirmDialog.SetActive(false));
            resultCloseButton.onClick.AddListener(() => resultPopup.SetActive(false));

            confirmDialog.SetActive(false);
            resultPopup.SetActive(false);
            panelRoot.SetActive(false);
        }

        public void Open() => panelRoot.SetActive(true);
        public void Close() => panelRoot.SetActive(false);

        private void OpenConfirmDialog()
        {
            confirmText.text = $"{GachaCost}コインでガチャを引きますか？";
            confirmDialog.SetActive(true);
        }

        private void OnConfirmYes()
        {
            confirmDialog.SetActive(false);
            gachaButton.interactable = false;

            ApiClient.Instance.DrawGacha(
                onSuccess: result =>
                {
                    gachaButton.interactable = true;
                    resultText.text = $"{result.character_name} を手に入れた";
                    resultPopup.SetActive(true);
                    homeController.RefreshFormServer();
                },
                onError: err =>
                {
                    gachaButton.interactable = true;
                    resultText.text = "ガチャに失敗しました: " + err;
                    resultPopup.SetActive(true);
                }
            );
        }
    }
}