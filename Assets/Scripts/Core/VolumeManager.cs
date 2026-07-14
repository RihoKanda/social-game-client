using UnityEngine;

/// 音量設定　シーンまたいで保持
namespace SocialGameClient.Core
{
    public class VolumeManager : MonoBehaviour
    {
        public static VolumeManager Instance { get; private set; }

        private const string VolumePrefsKey = "master_volume";
        public float CurrentVolume { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CurrentVolume = PlayerPrefs.GetFloat(VolumePrefsKey, 1f);
            AudioListener.volume = CurrentVolume;
        }

        public void SetVolume(float value)
        {
            CurrentVolume = value;
            AudioListener.volume = value;
            PlayerPrefs.SetFloat(VolumePrefsKey, value);
            PlayerPrefs.Save();
        }
    }
}
