using System;
using System.Collections;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using SocialGameClient.Auth;

namespace SocialGameClient.API
{
    public class ApiClient : MonoBehaviour
    {
        public static ApiClient Instance { get; private set; }

        // エディタ→localhost　実機→PCのローカルIPに変更
        [SerializeField] private string baseUrl = "http://localhost:8080";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Login(string deviceId, Action<LoginResponse> onSuccess, Action<string> onError)
        {
            var body = JsonUtility.ToJson(new LoginRequest { device_id = deviceId });
            StartCoroutine(PostCoroutine<LoginResponse>("/auth/login", body, withAuth: false, onSuccess, onError));
        }

        public void GetState(Action<StateResponse> onSuccess, Action<string> onError)
        {
            StartCoroutine(GetCoroutine<StateResponse>("/user/state", onSuccess, onError));
        }

        public void Claim(Action<ClaimResponse> onSuccess, Action<string> onError)
        {
            StartCoroutine(PostCoroutine<ClaimResponse>("/user/claim", null, withAuth: true, onSuccess, onError));
        }

        public void DrawGacha(Action<GachaDrawResponse> onSuccess, Action<string> onError)
        {
            StartCoroutine(PostCoroutine<GachaDrawResponse>("/gacha/draw", null, withAuth: true, onSuccess, onError));
        }

        private IEnumerator PostCoroutine<T>(string path, string jsonBody, bool withAuth,
            Action<T> onSuccess, Action<string> onError)
        {
            using var req = new UnityWebRequest(baseUrl + path, "POST");
            if (!string.IsNullOrEmpty(jsonBody))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            if (withAuth) SetAuthHeader(req);

            yield return req.SendWebRequest();
            HandleResponse(req, onSuccess, onError);
        }

        private IEnumerator GetCoroutine<T>(string path, Action<T> onSuccess, Action<string> onError)
        {
            using var req = UnityWebRequest.Get(baseUrl + path);
            SetAuthHeader(req);

            yield return req.SendWebRequest();
            HandleResponse(req, onSuccess, onError);
        }

        private void SetAuthHeader(UnityWebRequest req)
        {
            string token = AuthManager.Instance.Token;
            if (!string.IsNullOrEmpty(token))
            {
                req.SetRequestHeader("Authorization", "Bearer " + token);
            }
        }

        private void HandleResponse<T>(UnityWebRequest req, Action<T> onSuccess, Action<string> onError)
        {
            if (req.result != UnityWebRequest.Result.Success)
            {
                string message = req.downloadHandler != null && !string.IsNullOrEmpty(req.downloadHandler.text)
                    ? TryParseError(req.downloadHandler.text)
                    : req.error;
                onError?.Invoke(message);
                return;
            }

            try
            {
                T parsed = JsonUtility.FromJson<T>(req.downloadHandler.text);
                onSuccess?.Invoke(parsed);
            }
            catch (Exception e)
            {
                onError?.Invoke("failed to parse response: " + e.Message);
            }
        }

        private string TryParseError(string json)
        {
            try
            {
                var err = JsonUtility.FromJson<ErrorResponse>(json);
                return err != null && !string.IsNullOrEmpty(err.error) ? err.error : json;
            }
            catch
            {
                return json;
            }
        }
    }
}
