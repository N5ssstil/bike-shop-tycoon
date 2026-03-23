using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using BikeShopTycoon.Core;
using BikeShopTycoon.GameSystems;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// 事件面板 - 显示游戏事件和选项
    /// </summary>
    public class EventPanel : MonoBehaviour
    {
        [Header("面板引用")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Transform choicesContainer;
        [SerializeField] private GameObject choiceButtonPrefab;

        // 系统
        private EventSystem eventSystem;
        private GameEvent currentEvent;

        private void Awake()
        {
            if (panel != null)
                panel.SetActive(false);
        }

        private void Start()
        {
            if (GameManager.Instance != null && GameManager.Instance.PlayerData != null)
            {
                eventSystem = new EventSystem(GameManager.Instance.PlayerData);
                eventSystem.OnEventTriggered += ShowEvent;
            }

            // 订阅天数变化
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.OnDayStart += OnNewDay;
            }
        }

        private void OnDestroy()
        {
            if (eventSystem != null)
            {
                eventSystem.OnEventTriggered -= ShowEvent;
            }

            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.OnDayStart -= OnNewDay;
            }
        }

        /// <summary>
        /// 新的一天，检查事件
        /// </summary>
        private void OnNewDay(int day)
        {
            if (eventSystem == null || GameManager.Instance?.PlayerData == null) return;

            eventSystem.CheckForEvents(day, GameManager.Instance.PlayerData.Reputation);
        }

        /// <summary>
        /// 显示事件
        /// </summary>
        public void ShowEvent(GameEvent evt)
        {
            if (panel == null) return;

            currentEvent = evt;
            Time.timeScale = 0f; // 暂停游戏

            if (titleText != null)
                titleText.text = $"📢 {evt.Title}";

            if (descriptionText != null)
                descriptionText.text = evt.Description;

            // 清空选项
            if (choicesContainer != null)
            {
                foreach (Transform child in choicesContainer)
                {
                    Destroy(child.gameObject);
                }
            }

            // 创建选项按钮
            if (choiceButtonPrefab != null && choicesContainer != null)
            {
                for (int i = 0; i < evt.Choices.Count; i++)
                {
                    var choice = evt.Choices[i];
                    var button = Instantiate(choiceButtonPrefab, choicesContainer);
                    SetupChoiceButton(button, choice, i);
                }
            }

            panel.SetActive(true);
        }

        private void SetupChoiceButton(GameObject buttonObj, EventChoice choice, int index)
        {
            var buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                string text = choice.Text;
                
                // 显示效果预览
                var effects = new List<string>();
                if (choice.MoneyChange > 0)
                    effects.Add($"💰 +¥{choice.MoneyChange}");
                else if (choice.MoneyChange < 0)
                    effects.Add($"💰 -¥{-choice.MoneyChange}");
                
                if (choice.ReputationChange > 0)
                    effects.Add($"⭐ +{choice.ReputationChange}");
                else if (choice.ReputationChange < 0)
                    effects.Add($"⭐ {-choice.ReputationChange}");
                
                if (!string.IsNullOrEmpty(choice.UnlockBrand))
                    effects.Add($"🔓 解锁 {choice.UnlockBrand}");

                if (effects.Count > 0)
                    text += $"\n<size=18><color=#888>({string.Join(" | ", effects)})</color></size>";

                buttonText.text = text;
            }

            var button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => MakeChoice(choice));
            }
        }

        private void MakeChoice(EventChoice choice)
        {
            if (eventSystem == null || currentEvent == null) return;

            eventSystem.MakeChoice(currentEvent, choice);

            // 显示结果
            ShowChoiceResult(choice);

            // 关闭面板
            ClosePanel();
        }

        private void ShowChoiceResult(EventChoice choice)
        {
            var messages = new List<string>();

            if (choice.MoneyChange != 0)
            {
                string sign = choice.MoneyChange > 0 ? "+" : "";
                messages.Add($"资金 {sign}¥{choice.MoneyChange}");
            }

            if (choice.ReputationChange != 0)
            {
                string sign = choice.ReputationChange > 0 ? "+" : "";
                messages.Add($"口碑 {sign}{choice.ReputationChange}");
            }

            if (!string.IsNullOrEmpty(choice.UnlockBrand))
            {
                messages.Add($"解锁品牌: {choice.UnlockBrand}");
            }

            if (messages.Count > 0 && HUDController.Instance != null)
            {
                HUDController.Instance.ShowNotification(
                    string.Join("\n", messages),
                    choice.MoneyChange >= 0 && choice.ReputationChange >= 0 
                        ? NotificationType.Success 
                        : NotificationType.Warning
                );
            }
        }

        private void ClosePanel()
        {
            if (panel != null)
                panel.SetActive(false);

            currentEvent = null;
            Time.timeScale = 1f; // 恢复游戏
        }
    }
}