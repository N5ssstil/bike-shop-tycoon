using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BikeShopTycoon.Core;
using BikeShopTycoon.GameSystems;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// 店铺面板控制器
    /// </summary>
    public class ShopPanelController : MonoBehaviour
    {
        [Header("HUD 显示")]
        public TextMeshProUGUI moneyText;
        public TextMeshProUGUI reputationText;
        public TextMeshProUGUI dayText;

        [Header("顾客区域")]
        public Transform customerArea;
        public Button nextCustomerButton;

        [Header("导航按钮")]
        public Button inventoryButton;
        public Button repairButton;
        public Button saveButton;

        private void Awake()
        {
            // 动态创建 UI 元素
            CreateHUD();
            CreateNavigationButtons();
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

        private void CreateHUD()
        {
            // HUD 容器
            var hudGO = new GameObject("HUD");
            hudGO.transform.SetParent(transform, false);

            var hudRect = hudGO.AddComponent<RectTransform>();
            hudRect.anchorMin = new Vector2(0, 1);
            hudRect.anchorMax = new Vector2(1, 1);
            hudRect.pivot = new Vector2(0.5f, 1);
            hudRect.anchoredPosition = new Vector2(0, -20);
            hudRect.sizeDelta = new Vector2(0, 60);

            var hudImage = hudGO.AddComponent<Image>();
            hudImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            // 资金显示
            moneyText = CreateText(hudGO.transform, "MoneyText", "¥10,000", new Vector2(-200, 0), 24);
            moneyText.alignment = TextAlignmentOptions.Left;

            // 口碑显示
            reputationText = CreateText(hudGO.transform, "ReputationText", "口碑: 10", new Vector2(0, 0), 24);

            // 天数显示
            dayText = CreateText(hudGO.transform, "DayText", "第 1 天", new Vector2(200, 0), 24);
            dayText.alignment = TextAlignmentOptions.Right;
        }

        private void CreateNavigationButtons()
        {
            // 底部导航栏
            var navGO = new GameObject("Navigation");
            navGO.transform.SetParent(transform, false);

            var navRect = navGO.AddComponent<RectTransform>();
            navRect.anchorMin = new Vector2(0, 0);
            navRect.anchorMax = new Vector2(1, 0);
            navRect.pivot = new Vector2(0.5f, 0);
            navRect.anchoredPosition = new Vector2(0, 20);
            navRect.sizeDelta = new Vector2(0, 60);

            var navImage = navGO.AddComponent<Image>();
            navImage.color = new Color(0.15f, 0.15f, 0.15f, 0.9f);

            // 库存按钮
            inventoryButton = CreateNavButton(navGO.transform, "库存", new Vector2(-150, 0), () => 
            {
                Debug.Log("[Shop] 打开库存面板");
            });

            // 维修按钮
            repairButton = CreateNavButton(navGO.transform, "维修", new Vector2(0, 0), () => 
            {
                Debug.Log("[Shop] 打开维修面板");
            });

            // 保存按钮
            saveButton = CreateNavButton(navGO.transform, "保存", new Vector2(150, 0), () => 
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SaveGame();
                    Debug.Log("[Shop] 游戏已保存");
                }
            });
        }

        private TextMeshProUGUI CreateText(Transform parent, string name, string text, Vector2 position, int fontSize)
        {
            var textGO = new GameObject(name);
            textGO.transform.SetParent(parent, false);

            var rectTransform = textGO.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(150, 40);
            rectTransform.anchoredPosition = position;

            var tmp = textGO.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = fontSize;
            tmp.color = Color.white;

            return tmp;
        }

        private Button CreateNavButton(Transform parent, string text, Vector2 position, UnityEngine.Events.UnityAction onClick)
        {
            var buttonGO = new GameObject(text + "Button");
            buttonGO.transform.SetParent(parent, false);

            var rectTransform = buttonGO.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(100, 40);
            rectTransform.anchoredPosition = position;

            var image = buttonGO.AddComponent<Image>();
            image.color = new Color(0.25f, 0.5f, 0.25f, 1f);

            var button = buttonGO.AddComponent<Button>();
            button.onClick.AddListener(onClick);

            var textGO = new GameObject("Text");
            textGO.transform.SetParent(buttonGO.transform, false);
            var textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            var tmp = textGO.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 18;
            tmp.color = Color.white;

            return button;
        }

        public void UpdateMoney(int amount)
        {
            if (moneyText != null)
            {
                moneyText.text = $"¥{amount:N0}";
            }
        }

        public void UpdateReputation(int value)
        {
            if (reputationText != null)
            {
                reputationText.text = $"口碑: {value}";
            }
        }

        public void UpdateDay(int day)
        {
            if (dayText != null)
            {
                dayText.text = $"第 {day} 天";
            }
        }
    }
}