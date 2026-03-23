using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BikeShopTycoon.Core;
using BikeShopTycoon.GameSystems;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// UI 构建器 - 在运行时构建完整的游戏 UI
    /// </summary>
    public class UIBuilder : MonoBehaviour
    {
        [Header("主画布")]
        [SerializeField] private Canvas mainCanvas;

        // 主 UI 元素
        private GameObject hudPanel;
        private GameObject shopPanel;
        private GameObject inventoryPanel;
        private GameObject repairPanel;
        private GameObject eventPanel;
        private GameObject customerDetailPanel;

        // HUD 元素
        private TextMeshProUGUI moneyText;
        private TextMeshProUGUI reputationText;
        private TextMeshProUGUI dayText;

        // 店铺面板元素
        private Transform customerContainer;
        private TextMeshProUGUI shopStatusText;

        private void Start()
        {
            BuildUI();
        }

        /// <summary>
        /// 构建完整 UI
        /// </summary>
        public void BuildUI()
        {
            if (mainCanvas == null)
            {
                mainCanvas = FindObjectOfType<Canvas>();
                if (mainCanvas == null)
                {
                    CreateMainCanvas();
                }
            }

            BuildHUD();
            BuildShopPanel();
            BuildBottomNavigation();
        }

        /// <summary>
        /// 创建主画布
        /// </summary>
        private void CreateMainCanvas()
        {
            var canvasObj = new GameObject("Main Canvas");
            canvasObj.transform.SetParent(transform);

            mainCanvas = canvasObj.AddComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            canvasObj.AddComponent<GraphicRaycaster>();

            // 创建 EventSystem
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                var eventSystemObj = new GameObject("EventSystem");
                eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
        }

        /// <summary>
        /// 构建 HUD
        /// </summary>
        private void BuildHUD()
        {
            hudPanel = UIPrefabCreator.CreatePanel("HUD", mainCanvas.transform);
            var hudRect = hudPanel.GetComponent<RectTransform>();
            hudRect.anchorMin = new Vector2(0, 1);
            hudRect.anchorMax = new Vector2(1, 1);
            hudRect.pivot = new Vector2(0.5f, 1);
            hudRect.sizeDelta = new Vector2(0, 60);
            hudRect.anchoredPosition = new Vector2(0, 0);

            // 背景
            var hudImage = hudPanel.GetComponent<Image>();
            hudImage.color = new Color(0.1f, 0.1f, 0.12f, 0.9f);

            // 资金
            var moneyObj = UIPrefabCreator.CreateText("MoneyText", "¥ 10,000", hudPanel.transform);
            var moneyRect = moneyObj.GetComponent<RectTransform>();
            moneyRect.anchorMin = new Vector2(0, 0.5f);
            moneyRect.anchorMax = new Vector2(0, 0.5f);
            moneyRect.pivot = new Vector2(0, 0.5f);
            moneyRect.anchoredPosition = new Vector2(20, 0);
            moneyRect.sizeDelta = new Vector2(250, 40);
            moneyObj.GetComponent<TextMeshProUGUI>().color = new Color(0.4f, 0.8f, 0.4f);
            moneyObj.GetComponent<TextMeshProUGUI>().fontSize = 28;
            moneyText = moneyObj.GetComponent<TextMeshProUGUI>();

            // 口碑
            var repObj = UIPrefabCreator.CreateText("ReputationText", "口碑: 10", hudPanel.transform);
            var repRect = repObj.GetComponent<RectTransform>();
            repRect.anchorMin = new Vector2(0.5f, 0.5f);
            repRect.anchorMax = new Vector2(0.5f, 0.5f);
            repRect.pivot = new Vector2(0.5f, 0.5f);
            repRect.anchoredPosition = new Vector2(0, 0);
            repRect.sizeDelta = new Vector2(200, 40);
            repObj.GetComponent<TextMeshProUGUI>().color = new Color(0.9f, 0.7f, 0.2f);
            repObj.GetComponent<TextMeshProUGUI>().fontSize = 28;
            reputationText = repObj.GetComponent<TextMeshProUGUI>();

            // 天数
            var dayObj = UIPrefabCreator.CreateText("DayText", "第 1 天", hudPanel.transform);
            var dayRect = dayObj.GetComponent<RectTransform>();
            dayRect.anchorMin = new Vector2(1, 0.5f);
            dayRect.anchorMax = new Vector2(1, 0.5f);
            dayRect.pivot = new Vector2(1, 0.5f);
            dayRect.anchoredPosition = new Vector2(-20, 0);
            dayRect.sizeDelta = new Vector2(200, 40);
            dayObj.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
            dayObj.GetComponent<TextMeshProUGUI>().fontSize = 28;
            dayText = dayObj.GetComponent<TextMeshProUGUI>();

            // 添加 HUDController 组件
            var hudController = hudPanel.AddComponent<HUDController>();
            hudController.moneyText = moneyText;
            hudController.reputationText = reputationText;
            hudController.dayText = dayText;
        }

        /// <summary>
        /// 构建店铺面板
        /// </summary>
        private void BuildShopPanel()
        {
            shopPanel = UIPrefabCreator.CreatePanel("ShopPanel", mainCanvas.transform);
            var shopRect = shopPanel.GetComponent<RectTransform>();
            shopRect.anchorMin = new Vector2(0, 0);
            shopRect.anchorMax = new Vector2(1, 1);
            shopRect.sizeDelta = new Vector2(0, -120); // 底部留导航空间
            shopRect.anchoredPosition = new Vector2(0, -60);

            var shopImage = shopPanel.GetComponent<Image>();
            shopImage.color = new Color(0.12f, 0.12f, 0.15f);

            // 标题
            var titleObj = UIPrefabCreator.CreateText("Title", "🚴 自行车店", shopPanel.transform);
            var titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.9f);
            titleRect.anchorMax = new Vector2(0.5f, 0.9f);
            titleRect.sizeDelta = new Vector2(400, 60);
            titleObj.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            titleObj.GetComponent<TextMeshProUGUI>().fontSize = 36;

            // 状态文本
            var statusObj = UIPrefabCreator.CreateText("StatusText", "等待顾客光临...", shopPanel.transform);
            var statusRect = statusObj.GetComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0.5f, 0.5f);
            statusRect.anchorMax = new Vector2(0.5f, 0.5f);
            statusRect.sizeDelta = new Vector2(600, 100);
            statusObj.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            statusObj.GetComponent<TextMeshProUGUI>().color = new Color(0.6f, 0.6f, 0.7f);
            shopStatusText = statusObj.GetComponent<TextMeshProUGUI>();

            // 顾客容器
            var containerObj = new GameObject("CustomerContainer");
            containerObj.transform.SetParent(shopPanel.transform);
            var containerRect = containerObj.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.1f, 0.2f);
            containerRect.anchorMax = new Vector2(0.9f, 0.8f);
            containerRect.sizeDelta = Vector2.zero;

            // 添加垂直布局
            var layoutGroup = containerObj.AddComponent<VerticalLayoutGroup>();
            layoutGroup.spacing = 10;
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.childControlWidth = false;
            layoutGroup.childControlHeight = false;

            customerContainer = containerObj.transform;

            // 添加 ShopController 组件
            var shopController = shopPanel.AddComponent<ShopController>();
            // 通过反射或序列化设置字段...
        }

        /// <summary>
        /// 构建底部导航
        /// </summary>
        private void BuildBottomNavigation()
        {
            var navPanel = UIPrefabCreator.CreatePanel("BottomNav", mainCanvas.transform);
            var navRect = navPanel.GetComponent<RectTransform>();
            navRect.anchorMin = new Vector2(0, 0);
            navRect.anchorMax = new Vector2(1, 0);
            navRect.pivot = new Vector2(0.5f, 0);
            navRect.sizeDelta = new Vector2(0, 60);
            navRect.anchoredPosition = new Vector2(0, 0);

            var navImage = navPanel.GetComponent<Image>();
            navImage.color = new Color(0.08f, 0.08f, 0.1f, 0.95f);

            // 水平布局
            var layoutGroup = navPanel.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.spacing = 20;
            layoutGroup.padding = new RectOffset(50, 50, 10, 10);
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.childControlWidth = false;
            layoutGroup.childControlHeight = false;

            // 库存按钮
            var inventoryBtn = UIPrefabCreator.CreateIconButton("InventoryBtn", "📦", navPanel.transform);
            inventoryBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 45);

            // 维修按钮
            var repairBtn = UIPrefabCreator.CreateIconButton("RepairBtn", "🔧", navPanel.transform);
            repairBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 45);

            // 保存按钮
            var saveBtn = UIPrefabCreator.CreateIconButton("SaveBtn", "💾", navPanel.transform);
            saveBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 45);

            // 暂停按钮
            var pauseBtn = UIPrefabCreator.CreateIconButton("PauseBtn", "⏸️", navPanel.transform);
            pauseBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 45);

            // 绑定按钮事件
            inventoryBtn.GetComponent<Button>().onClick.AddListener(OpenInventory);
            repairBtn.GetComponent<Button>().onClick.AddListener(OpenRepair);
            saveBtn.GetComponent<Button>().onClick.AddListener(SaveGame);
            pauseBtn.GetComponent<Button>().onClick.AddListener(TogglePause);
        }

        #region 按钮事件

        private void OpenInventory()
        {
            Debug.Log("[UIBuilder] 打开库存面板");
            // TODO: 显示库存面板
        }

        private void OpenRepair()
        {
            Debug.Log("[UIBuilder] 打开维修面板");
            // TODO: 显示维修面板
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

        #endregion
    }
}