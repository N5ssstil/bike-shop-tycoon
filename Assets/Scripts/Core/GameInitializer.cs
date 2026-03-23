using UnityEngine;
using UnityEngine.SceneManagement;
using BikeShopTycoon.Core;
using BikeShopTycoon.GameSystems;
using BikeShopTycoon.UI;
using BikeShopTycoon.Data;

namespace BikeShopTycoon
{
    /// <summary>
    /// 游戏初始化器 - 自动创建所有必要的游戏对象和数据
    /// 挂载到场景中即可自动运行
    /// </summary>
    public class GameInitializer : MonoBehaviour
    {
        [Header("初始化设置")]
        [SerializeField] private bool initializeOnAwake = true;
        [SerializeField] private bool createMissingSystems = true;
        [SerializeField] private bool createInitialProducts = true;

        [Header("调试")]
        [SerializeField] private bool debugMode = true;

        private void Awake()
        {
            if (initializeOnAwake)
            {
                InitializeGame();
            }
        }

        /// <summary>
        /// 初始化游戏
        /// </summary>
        [ContextMenu("初始化游戏")]
        public void InitializeGame()
        {
            Log("开始初始化游戏...");

            if (createMissingSystems)
            {
                CreateSystems();
            }

            if (createInitialProducts)
            {
                CreateProducts();
            }

            Log("游戏初始化完成！");
        }

        /// <summary>
        /// 创建系统对象
        /// </summary>
        private void CreateSystems()
        {
            // 1. GameManager
            if (GameManager.Instance == null)
            {
                var gmObj = new GameObject("[GameManager]");
                gmObj.AddComponent<GameManager>();
                DontDestroyOnLoad(gmObj);
                Log("创建 GameManager");
            }

            // 2. TimeManager
            if (TimeManager.Instance == null)
            {
                var tmObj = new GameObject("[TimeManager]");
                tmObj.AddComponent<TimeManager>();
                DontDestroyOnLoad(tmObj);
                Log("创建 TimeManager");
            }

            // 3. SettingsManager
            if (SettingsManager.Instance == null)
            {
                var smObj = new GameObject("[SettingsManager]");
                smObj.AddComponent<SettingsManager>();
                DontDestroyOnLoad(smObj);
                Log("创建 SettingsManager");
            }

            // 4. AudioManager
            if (AudioManager.Instance == null)
            {
                var amObj = new GameObject("[AudioManager]");
                amObj.AddComponent<AudioManager>();
                DontDestroyOnLoad(amObj);
                Log("创建 AudioManager");
            }

            // 5. EventSystem
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                var esObj = new GameObject("EventSystem");
                esObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
                esObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                Log("创建 EventSystem");
            }

            // 6. Canvas 和 UI
            CreateUI();
        }

        /// <summary>
        /// 创建 UI
        /// </summary>
        private void CreateUI()
        {
            var existingCanvas = FindObjectOfType<Canvas>();
            if (existingCanvas != null) return;

            // 创建主画布
            var canvasObj = new GameObject("[Main Canvas]");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            canvasObj.AddComponent<GraphicRaycaster>();

            // 创建 HUD
            CreateHUD(canvasObj.transform);

            // 创建店铺面板
            CreateShopPanel(canvasObj.transform);

            // 创建底部导航
            CreateBottomNav(canvasObj.transform);

            Log("创建 UI 完成");
        }

        /// <summary>
        /// 创建 HUD
        /// </summary>
        private void CreateHUD(Transform parent)
        {
            var hudObj = new GameObject("HUD");
            hudObj.transform.SetParent(parent);

            var hudRect = hudObj.AddComponent<RectTransform>();
            hudRect.anchorMin = new Vector2(0, 1);
            hudRect.anchorMax = new Vector2(1, 1);
            hudRect.pivot = new Vector2(0.5f, 1);
            hudRect.sizeDelta = new Vector2(0, 60);
            hudRect.anchoredPosition = Vector2.zero;

            var hudImage = hudObj.AddComponent<Image>();
            hudImage.color = new Color(0.1f, 0.1f, 0.12f, 0.95f);

            // 添加 HUDController
            var hudController = hudObj.AddComponent<HUDController>();

            // 创建文本元素
            CreateHUDTexts(hudObj.transform, hudController);

            // 创建通知容器
            CreateNotificationContainer(hudObj.transform, hudController);
        }

        private void CreateHUDTexts(Transform parent, HUDController controller)
        {
            // 资金文本
            var moneyObj = CreateTextObject("MoneyText", "¥ 10,000", parent);
            var moneyRect = moneyObj.GetComponent<RectTransform>();
            moneyRect.anchorMin = new Vector2(0, 0.5f);
            moneyRect.anchorMax = new Vector2(0, 0.5f);
            moneyRect.pivot = new Vector2(0, 0.5f);
            moneyRect.anchoredPosition = new Vector2(20, 0);
            moneyRect.sizeDelta = new Vector2(250, 40);
            var moneyTmp = moneyObj.GetComponent<TMPro.TextMeshProUGUI>();
            moneyTmp.color = new Color(0.4f, 0.8f, 0.4f);
            moneyTmp.fontSize = 28;
            moneyTmp.fontStyle = TMPro.FontStyles.Bold;

            // 口碑文本
            var repObj = CreateTextObject("ReputationText", "口碑: 10", parent);
            var repRect = repObj.GetComponent<RectTransform>();
            repRect.anchorMin = new Vector2(0.5f, 0.5f);
            repRect.anchorMax = new Vector2(0.5f, 0.5f);
            repRect.pivot = new Vector2(0.5f, 0.5f);
            repRect.anchoredPosition = Vector2.zero;
            repRect.sizeDelta = new Vector2(200, 40);
            var repTmp = repObj.GetComponent<TMPro.TextMeshProUGUI>();
            repTmp.color = new Color(0.9f, 0.7f, 0.2f);
            repTmp.fontSize = 28;
            repTmp.alignment = TMPro.TextAlignmentOptions.Center;

            // 天数文本
            var dayObj = CreateTextObject("DayText", "第 1 天", parent);
            var dayRect = dayObj.GetComponent<RectTransform>();
            dayRect.anchorMin = new Vector2(1, 0.5f);
            dayRect.anchorMax = new Vector2(1, 0.5f);
            dayRect.pivot = new Vector2(1, 0.5f);
            dayRect.anchoredPosition = new Vector2(-20, 0);
            dayRect.sizeDelta = new Vector2(200, 40);
            var dayTmp = dayObj.GetComponent<TMPro.TextMeshProUGUI>();
            dayTmp.color = new Color(0.7f, 0.7f, 0.8f);
            dayTmp.fontSize = 28;
            dayTmp.alignment = TMPro.TextAlignmentOptions.Right;

            // 设置 HUDController 引用
            controller.moneyText = moneyTmp;
            controller.reputationText = repTmp;
            controller.dayText = dayTmp;
        }

        private void CreateNotificationContainer(Transform parent, HUDController controller)
        {
            var containerObj = new GameObject("NotificationContainer");
            containerObj.transform.SetParent(parent);

            var containerRect = containerObj.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.5f, 0);
            containerRect.anchorMax = new Vector2(0.5f, 0);
            containerRect.pivot = new Vector2(0.5f, 0);
            containerRect.anchoredPosition = new Vector2(0, 80);
            containerRect.sizeDelta = new Vector2(400, 200);

            var layoutGroup = containerObj.AddComponent<VerticalLayoutGroup>();
            layoutGroup.spacing = 10;
            layoutGroup.childAlignment = TextAnchor.LowerCenter;
            layoutGroup.childControlWidth = false;
            layoutGroup.childControlHeight = false;

            controller.notificationContainer = containerObj.transform;

            // 创建通知预制体
            var notifPrefab = CreateNotificationPrefab();
            controller.notificationPrefab = notifPrefab;
        }

        private GameObject CreateNotificationPrefab()
        {
            var notifObj = new GameObject("Notification");
            
            var notifRect = notifObj.AddComponent<RectTransform>();
            notifRect.sizeDelta = new Vector2(350, 45);

            var notifImage = notifObj.AddComponent<Image>();
            notifImage.color = new Color(0.2f, 0.2f, 0.25f, 0.9f);

            var textObj = CreateTextObject("Text", "通知内容", notifObj.transform);
            var textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = new Vector2(-20, 0);
            var tmp = textObj.GetComponent<TMPro.TextMeshProUGUI>();
            tmp.alignment = TMPro.TextAlignmentOptions.Center;
            tmp.fontSize = 18;

            return notifObj;
        }

        /// <summary>
        /// 创建店铺面板
        /// </summary>
        private void CreateShopPanel(Transform parent)
        {
            var shopObj = new GameObject("ShopPanel");
            shopObj.transform.SetParent(parent);

            var shopRect = shopObj.AddComponent<RectTransform>();
            shopRect.anchorMin = new Vector2(0, 0);
            shopRect.anchorMax = new Vector2(1, 1);
            shopRect.sizeDelta = new Vector2(0, -120);
            shopRect.anchoredPosition = new Vector2(0, -60);

            var shopImage = shopObj.AddComponent<Image>();
            shopImage.color = new Color(0.12f, 0.12f, 0.15f);

            // 标题
            var titleObj = CreateTextObject("Title", "🚴 自行车店", shopObj.transform);
            var titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.9f);
            titleRect.anchorMax = new Vector2(0.5f, 0.9f);
            titleRect.sizeDelta = new Vector2(400, 60);
            var titleTmp = titleObj.GetComponent<TMPro.TextMeshProUGUI>();
            titleTmp.alignment = TMPro.TextAlignmentOptions.Center;
            titleTmp.fontSize = 36;
            titleTmp.fontStyle = TMPro.FontStyles.Bold;

            // 状态文本
            var statusObj = CreateTextObject("StatusText", "等待顾客光临...", shopObj.transform);
            var statusRect = statusObj.GetComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0.5f, 0.5f);
            statusRect.anchorMax = new Vector2(0.5f, 0.5f);
            statusRect.sizeDelta = new Vector2(600, 100);
            var statusTmp = statusObj.GetComponent<TMPro.TextMeshProUGUI>();
            statusTmp.alignment = TMPro.TextAlignmentOptions.Center;
            statusTmp.color = new Color(0.6f, 0.6f, 0.7f);
            statusTmp.fontSize = 24;

            // 添加 ShopController
            var shopController = shopObj.AddComponent<ShopController>();
        }

        /// <summary>
        /// 创建底部导航
        /// </summary>
        private void CreateBottomNav(Transform parent)
        {
            var navObj = new GameObject("BottomNav");
            navObj.transform.SetParent(parent);

            var navRect = navObj.AddComponent<RectTransform>();
            navRect.anchorMin = new Vector2(0, 0);
            navRect.anchorMax = new Vector2(1, 0);
            navRect.pivot = new Vector2(0.5f, 0);
            navRect.sizeDelta = new Vector2(0, 60);
            navRect.anchoredPosition = Vector2.zero;

            var navImage = navObj.AddComponent<Image>();
            navImage.color = new Color(0.08f, 0.08f, 0.1f, 0.95f);

            var layoutGroup = navObj.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.spacing = 20;
            layoutGroup.padding = new RectOffset(50, 50, 10, 10);
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.childControlWidth = false;
            layoutGroup.childControlHeight = false;

            // 创建导航按钮
            CreateNavButton("📦", navObj.transform);
            CreateNavButton("🔧", navObj.transform);
            CreateNavButton("💾", navObj.transform, SaveGame);
            CreateNavButton("⏸️", navObj.transform, TogglePause);
        }

        private void CreateNavButton(string icon, Transform parent, UnityEngine.Events.UnityAction onClick = null)
        {
            var btnObj = new GameObject("NavButton");
            btnObj.transform.SetParent(parent);

            var btnRect = btnObj.AddComponent<RectTransform>();
            btnRect.sizeDelta = new Vector2(100, 45);

            var btnImage = btnObj.AddComponent<Image>();
            btnImage.color = new Color(0.2f, 0.2f, 0.25f);

            var button = btnObj.AddComponent<Button>();

            // 图标文本
            var iconObj = CreateTextObject("Icon", icon, btnObj.transform);
            var iconRect = iconObj.GetComponent<RectTransform>();
            iconRect.anchorMin = Vector2.zero;
            iconRect.anchorMax = Vector2.one;
            iconRect.sizeDelta = Vector2.zero;
            var iconTmp = iconObj.GetComponent<TMPro.TextMeshProUGUI>();
            iconTmp.alignment = TMPro.TextAlignmentOptions.Center;
            iconTmp.fontSize = 24;

            if (onClick != null)
            {
                button.onClick.AddListener(onClick);
            }
        }

        /// <summary>
        /// 创建产品数据
        /// </summary>
        private void CreateProducts()
        {
            Log("创建初始商品数据...");
            // ProductDatabase 会通过代码创建默认商品
            // 这里不需要额外操作，InitialProducts.CreateDefaultProducts() 已包含所有商品
        }

        /// <summary>
        /// 创建文本对象
        /// </summary>
        private GameObject CreateTextObject(string name, string text, Transform parent)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent);

            var rect = obj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(200, 40);

            var tmp = obj.AddComponent<TMPro.TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = 24;
            tmp.color = Color.white;

            return obj;
        }

        private void SaveGame()
        {
            GameManager.Instance?.SaveGame();
            HUDController.Instance?.ShowNotification("游戏已保存！", NotificationType.Success);
        }

        private void TogglePause()
        {
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.SetPaused(!TimeManager.Instance.isPaused);
                string status = TimeManager.Instance.isPaused ? "已暂停" : "继续游戏";
                HUDController.Instance?.ShowNotification(status, NotificationType.Info);
            }
        }

        private void Log(string message)
        {
            if (debugMode)
            {
                Debug.Log($"[GameInitializer] {message}");
            }
        }
    }
}