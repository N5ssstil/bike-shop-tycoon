using UnityEngine;
using System;

namespace BikeShopTycoon.Core
{
    /// <summary>
    /// 时间管理系统 - 管理游戏天数推进
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        [Header("时间设置")]
        [Tooltip("一个游戏日的现实秒数")]
        public float secondsPerDay = 60f;
        
        [Header("当前状态")]
        public int currentDay = 1;
        public float dayProgress = 0f;
        public bool isPaused = false;

        // 事件
        public event Action<int> OnDayStart;
        public event Action<int> OnDayEnd;
        public event Action<float> OnDayProgress;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            if (GameManager.Instance != null && GameManager.Instance.PlayerData != null)
            {
                currentDay = GameManager.Instance.PlayerData.Day;
            }
            OnDayStart?.Invoke(currentDay);
        }

        private void Update()
        {
            if (isPaused) return;

            dayProgress += Time.deltaTime / secondsPerDay;
            OnDayProgress?.Invoke(dayProgress);

            if (dayProgress >= 1f)
            {
                AdvanceDay();
            }
        }

        /// <summary>
        /// 推进到下一天
        /// </summary>
        public void AdvanceDay()
        {
            OnDayEnd?.Invoke(currentDay);
            
            currentDay++;
            dayProgress = 0f;

            // 更新玩家数据
            if (GameManager.Instance != null && GameManager.Instance.PlayerData != null)
            {
                GameManager.Instance.PlayerData.Day = currentDay;
            }

            OnDayStart?.Invoke(currentDay);
        }

        /// <summary>
        /// 手动跳过指定天数
        /// </summary>
        public void SkipDays(int days)
        {
            for (int i = 0; i < days; i++)
            {
                AdvanceDay();
            }
        }

        /// <summary>
        /// 暂停/恢复游戏时间
        /// </summary>
        public void SetPaused(bool paused)
        {
            isPaused = paused;
        }

        /// <summary>
        /// 获取当天剩余时间比例
        /// </summary>
        public float GetRemainingDayRatio()
        {
            return 1f - dayProgress;
        }

        /// <summary>
        /// 获取当天剩余秒数（现实时间）
        /// </summary>
        public float GetRemainingSeconds()
        {
            return GetRemainingDayRatio() * secondsPerDay;
        }
    }
}