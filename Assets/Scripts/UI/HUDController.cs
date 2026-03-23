using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// HUD 控制器 - 显示金钱、口碑、时间等信息
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        public static HUDController Instance { get; private set; }

        [Header("显示组件")]
        public TextMeshProUGUI moneyText;
        public TextMeshProUGUI reputationText;
        public TextMeshProUGUI dayText;
        public Slider reputationSlider;

        [Header("通知")]
        public GameObject notificationPrefab;
        public Transform notificationContainer;

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
            // 订阅事件
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnMoneyChanged += UpdateMoney;
                GameManager.Instance.OnReputationChanged += UpdateReputation;
                
                // 初始化显示
                UpdateMoney(GameManager.Instance.PlayerData.Money);
                UpdateReputation(GameManager.Instance.PlayerData.Reputation);
            }

            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.OnDayStart += UpdateDay;
                UpdateDay(TimeManager.Instance.currentDay);
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnMoneyChanged -= UpdateMoney;
                GameManager.Instance.OnReputationChanged -= UpdateReputation;
            }

            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.OnDayStart -= UpdateDay;
            }
        }

        /// <summary>
        /// 更新金钱显示
        /// </summary>
        public void UpdateMoney(int amount)
        {
            moneyText.text = FormatMoney(amount);
        }

        /// <summary>
        /// 更新口碑显示
        /// </summary>
        public void UpdateReputation(int value)
        {
            reputationText.text = value.ToString();
            if (reputationSlider != null)
            {
                // 口碑上限假设为 1000
                reputationSlider.value = value / 1000f;
            }
        }

        /// <summary>
        /// 更新天数显示
        /// </summary>
        public void UpdateDay(int day)
        {
            if (dayText != null)
                dayText.text = $"第 {day} 天";
        }

        /// <summary>
        /// 显示通知
        /// </summary>
        public void ShowNotification(string message, NotificationType type = NotificationType.Info)
        {
            if (notificationPrefab == null || notificationContainer == null)
            {
                Debug.Log($"[HUD] {message}");
                return;
            }

            var notification = Instantiate(notificationPrefab, notificationContainer);
            var text = notification.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = message;
            }

            // 根据类型设置颜色
            var image = notification.GetComponent<Image>();
            if (image != null)
            {
                image.color = type switch
                {
                    NotificationType.Success => new Color(0.2f, 0.8f, 0.2f),
                    NotificationType.Warning => new Color(0.9f, 0.7f, 0.2f),
                    NotificationType.Error => new Color(0.9f, 0.3f, 0.3f),
                    _ => new Color(0.8f, 0.8f, 0.8f)
                };
            }

            // 3秒后销毁
            Destroy(notification, 3f);
        }

        private string FormatMoney(int amount)
        {
            if (amount >= 10000)
            {
                return $"¥{amount / 10000f:F1}万";
            }
            return $"¥{amount:N0}";
        }
    }

    public enum NotificationType
    {
        Info,
        Success,
        Warning,
        Error
    }
}