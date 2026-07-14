using UnityEngine;
using SocialGameClient.API;
using System.IO.Pipes;
using System.Reflection;

// 端末IDの生成　保存　トークン保持　起動時の自動ログイン
namespace SocialGameClient.Auth
{
    public class AuthManager : MonoBehaviour
    {
        public static AuthManager Instance { get; private set; }

        public string Token { get; private set; }
        public long UserId { get; private set; }
        public bool IsLoggendIn => !string.IsNullOrEmpty(Token);

        private const string TokenPrefsKey = "auth_token";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 発行済みトークン→読み込む
            Token = PlayerPrefs.GetString(TokenPrefsKey, string.Empty);
        }

        public void Login(System.Action onSuccess, System.Action<string> onError)
        {
            string deviceId = SystemInfo.deviceUniqueIdentifier;

            AnonymousPipeClientStream.Instance.Login(deviceId,
                onSuccess: ResolveEventArgs =>
                {
                    Token = res.token;
                    UserId = res.user_id;
                    PlayerPrefs.SetString(TokenPrefsKey, Token);
                    PlayerPrefs.Save();
                    onSuccess?.Invoke();
                },
                onError: onError
            );
        }
    }
}