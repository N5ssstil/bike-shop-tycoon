using UnityEngine;
using System.Collections.Generic;

namespace BikeShopTycoon.Core
{
    /// <summary>
    /// 音效管理器 - 管理游戏音效和音乐
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("音频源")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("音效库")]
        [SerializeField] private AudioClip buttonClickSfx;
        [SerializeField] private AudioClip transactionSfx;
        [SerializeField] private AudioClip notificationSfx;
        [SerializeField] private AudioClip customerEnterSfx;
        [SerializeField] private AudioClip successSfx;
        [SerializeField] private AudioClip errorSfx;

        // 音效缓存
        private Dictionary<string, AudioClip> sfxLibrary;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeAudioSources();
            InitializeSfxLibrary();
        }

        private void Start()
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.OnSettingsChanged += OnSettingsChanged;
            }
        }

        private void OnDestroy()
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.OnSettingsChanged -= OnSettingsChanged;
            }
        }

        private void InitializeAudioSources()
        {
            if (musicSource == null)
            {
                var musicObj = new GameObject("MusicSource");
                musicObj.transform.SetParent(transform);
                musicSource = musicObj.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.playOnAwake = false;
            }

            if (sfxSource == null)
            {
                var sfxObj = new GameObject("SFXSource");
                sfxObj.transform.SetParent(transform);
                sfxSource = sfxObj.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
            }
        }

        private void InitializeSfxLibrary()
        {
            sfxLibrary = new Dictionary<string, AudioClip>
            {
                { "button_click", buttonClickSfx },
                { "transaction", transactionSfx },
                { "notification", notificationSfx },
                { "customer_enter", customerEnterSfx },
                { "success", successSfx },
                { "error", errorSfx }
            };
        }

        private void OnSettingsChanged(GameSettings settings)
        {
            if (musicSource != null)
                musicSource.volume = settings.musicVolume * settings.masterVolume;
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (musicSource == null || clip == null) return;

            if (SettingsManager.Instance != null)
            {
                musicSource.volume = SettingsManager.Instance.Settings.musicVolume 
                    * SettingsManager.Instance.Settings.masterVolume;
            }

            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void StopMusic()
        {
            if (musicSource != null)
                musicSource.Stop();
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlaySFX(AudioClip clip)
        {
            if (sfxSource == null || clip == null) return;

            float volume = 1f;
            if (SettingsManager.Instance != null)
            {
                volume = SettingsManager.Instance.Settings.sfxVolume 
                    * SettingsManager.Instance.Settings.masterVolume;
            }

            sfxSource.PlayOneShot(clip, volume);
        }

        /// <summary>
        /// 播放命名音效
        /// </summary>
        public void PlaySFX(string sfxName)
        {
            if (sfxLibrary != null && sfxLibrary.TryGetValue(sfxName, out var clip))
            {
                PlaySFX(clip);
            }
            else
            {
                Debug.LogWarning($"音效未找到: {sfxName}");
            }
        }

        /// <summary>
        /// 播放按钮点击音效
        /// </summary>
        public void PlayButtonClick()
        {
            PlaySFX("button_click");
        }

        /// <summary>
        /// 播放交易音效
        /// </summary>
        public void PlayTransaction()
        {
            PlaySFX("transaction");
        }

        /// <summary>
        /// 播放成功音效
        /// </summary>
        public void PlaySuccess()
        {
            PlaySFX("success");
        }

        /// <summary>
        /// 播放错误音效
        /// </summary>
        public void PlayError()
        {
            PlaySFX("error");
        }

        /// <summary>
        /// 播放顾客进店音效
        /// </summary>
        public void PlayCustomerEnter()
        {
            PlaySFX("customer_enter");
        }

        /// <summary>
        /// 播放通知音效
        /// </summary>
        public void PlayNotification()
        {
            PlaySFX("notification");
        }
    }
}