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
        [Header("显示组件")]
        public TextMeshProUGUI moneyText;
        public TextMeshProUGUI reputationText;
        public TextMeshProUGUI dayText;
        public Slider reputationSlider;

        [Header("通知")]
        public GameObject notificationPrefab;
        public Transform notificationContainer;

        private void Start()
        {
            // 订阅事件
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnMoneyChanged += UpdateMoney;
                GameManager.Instance.OnReputationChanged += UpdateReputation;
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnMoneyChanged -= UpdateMoney;
                GameManager.Instance.OnReputationChanged -= UpdateReputation;
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
            dayText.text = $"第 {day} 天";
        }

        /// <summary>
        /// 显示通知
        /// </summary>
        public void ShowNotification(string message, NotificationType type = NotificationType.Info)
        {
            if (notificationPrefab == null || notificationContainer == null)
                return;

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
                    NotificationType.Success => Color.green,
                    NotificationType.Warning => Color.yellow,
                    NotificationType.Error => Color.red,
                    _ => Color.white
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