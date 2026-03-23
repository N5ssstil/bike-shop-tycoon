using UnityEngine;
using System;

namespace BikeShopTycoon.Core
{
    /// <summary>
    /// 游戏设置 - 可序列化保存
    /// </summary>
    [Serializable]
    public class GameSettings
    {
        // 音频设置
        [Range(0f, 1f)] public float masterVolume = 1f;
        [Range(0f, 1f)] public float musicVolume = 0.7f;
        [Range(0f, 1f)] public float sfxVolume = 0.8f;

        // 游戏设置
        public int dayDurationSeconds = 60;
        public bool autoSave = true;
        public int autoSaveIntervalMinutes = 5;
        public bool showTutorials = true;
        public bool showNotifications = true;

        // 画面设置
        public bool fullscreen = true;
        public int targetFrameRate = 60;

        // 语言
        public string language = "zh-CN";
    }

    /// <summary>
    /// 设置管理器 - 管理游戏设置
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; private set; }

        private const string SETTINGS_KEY = "BikeShopTycoon_Settings";

        public GameSettings Settings { get; private set; }

        // 设置变更事件
        public event Action<GameSettings> OnSettingsChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadSettings();
            ApplySettings();
        }

        /// <summary>
        /// 加载设置
        /// </summary>
        private void LoadSettings()
        {
            if (PlayerPrefs.HasKey(SETTINGS_KEY))
            {
                try
                {
                    string json = PlayerPrefs.GetString(SETTINGS_KEY);
                    Settings = JsonUtility.FromJson<GameSettings>(json);
                }
                catch
                {
                    Settings = new GameSettings();
                }
            }
            else
            {
                Settings = new GameSettings();
            }
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        public void SaveSettings()
        {
            string json = JsonUtility.ToJson(Settings);
            PlayerPrefs.SetString(SETTINGS_KEY, json);
            PlayerPrefs.Save();

            OnSettingsChanged?.Invoke(Settings);
        }

        /// <summary>
        /// 应用设置
        /// </summary>
        private void ApplySettings()
        {
            // 应用音频设置
            AudioListener.volume = Settings.masterVolume;

            // 应用帧率设置
            Application.targetFrameRate = Settings.targetFrameRate;

            // 应用全屏设置
            Screen.fullScreen = Settings.fullscreen;

            // 应用时间设置
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.secondsPerDay = Settings.dayDurationSeconds;
            }
        }

        /// <summary>
        /// 设置主音量
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            Settings.masterVolume = Mathf.Clamp01(volume);
            AudioListener.volume = Settings.masterVolume;
            SaveSettings();
        }

        /// <summary>
        /// 设置音乐音量
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            Settings.musicVolume = Mathf.Clamp01(volume);
            SaveSettings();
        }

        /// <summary>
        /// 设置音效音量
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            Settings.sfxVolume = Mathf.Clamp01(volume);
            SaveSettings();
        }

        /// <summary>
        /// 设置全屏
        /// </summary>
        public void SetFullscreen(bool fullscreen)
        {
            Settings.fullscreen = fullscreen;
            Screen.fullScreen = fullscreen;
            SaveSettings();
        }

        /// <summary>
        /// 设置帧率
        /// </summary>
        public void SetTargetFrameRate(int rate)
        {
            Settings.targetFrameRate = rate;
            Application.targetFrameRate = rate;
            SaveSettings();
        }

        /// <summary>
        /// 重置为默认设置
        /// </summary>
        public void ResetToDefaults()
        {
            Settings = new GameSettings();
            ApplySettings();
            SaveSettings();
        }
    }
}