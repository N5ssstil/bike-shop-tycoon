using UnityEngine;
using UnityEngine.UI;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// UI 预设构建器 - 快速生成完整界面
    /// </summary>
    public class UIPresetBuilder
    {
        private UITheme theme;
        private GameObject canvas;

        public UIPresetBuilder(UITheme theme = null)
        {
            this.theme = theme ?? UITheme.GetDefaultTheme();
        }

        /// <summary>
        /// 创建主菜单
        /// </summary>
        public GameObject BuildMainMenu()
        {
            // 创建 Canvas
            canvas = CreateCanvas("MainMenuCanvas");

            // 背景
            var bgObj = new GameObject("Background");
            bgObj.transform.SetParent(canvas.transform, false);
            var bgRect = bgObj.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            var bgImage = bgObj.AddComponent<Image>();
            bgImage.color = theme.SecondaryColor;

            // 标题区域
            var titlePanel = UIPlaceholderGenerator.CreatePanel("TitlePanel", canvas.transform, 400, 150, theme);
            var titleRect = titlePanel.GetComponent<RectTransform>();
            titleRect.anchoredPosition = new Vector2(0, 200);

            // 标题文字
            var titleText = CreateText("Title", "Bike Shop Tycoon", titlePanel.transform, 48, FontStyle.Bold);
            titleText.alignment = TextAnchor.MiddleCenter;
            titleText.color = theme.PrimaryColor;

            var subtitleText = CreateText("Subtitle", "车店大亨", titlePanel.transform, 24, FontStyle.Normal);
            subtitleText.alignment = TextAnchor.MiddleCenter;
            subtitleText.color = theme.TextLight;

            // 按钮区域
            var buttonContainer = new GameObject("ButtonContainer");
            buttonContainer.transform.SetParent(canvas.transform, false);
            var containerRect = buttonContainer.AddComponent<RectTransform>();
            containerRect.anchoredPosition = new Vector2(0, -50);

            var vGroup = buttonContainer.AddComponent<VerticalLayoutGroup>();
            vGroup.spacing = 15;
            vGroup.childAlignment = TextAnchor.MiddleCenter;
            vGroup.childForceExpandWidth = false;
            vGroup.childForceExpandHeight = false;

            // 按钮
            UIPlaceholderGenerator.CreateButton("StartButton", "开始游戏", buttonContainer.transform, theme);
            UIPlaceholderGenerator.CreateButton("ContinueButton", "继续游戏", buttonContainer.transform, theme);
            UIPlaceholderGenerator.CreateButton("SettingsButton", "设置", buttonContainer.transform, theme);
            UIPlaceholderGenerator.CreateButton("QuitButton", "退出", buttonContainer.transform, theme);

            // 版本信息
            var versionText = CreateText("Version", "v0.1", canvas.transform, 14, FontStyle.Normal);
            versionText.alignment = TextAnchor.LowerLeft;
            var versionRect = versionText.GetComponent<RectTransform>();
            versionRect.anchorMin = Vector2.zero;
            versionRect.anchorMax = Vector2.zero;
            versionRect.anchoredPosition = new Vector2(20, 20);

            return canvas;
        }

        /// <summary>
        /// 创建 HUD
        /// </summary>
        public GameObject BuildHUD()
        {
            var hudObj = new GameObject("HUD");
            hudObj.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            hudObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            hudObj.AddComponent<GraphicRaycaster>();

            // HUD 背景
            var bgObj = new GameObject("Background");
            bgObj.transform.SetParent(hudObj.transform, false);
            var bgRect = bgObj.AddComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0, 1);
            bgRect.anchorMax = Vector2(1, 1);
            bgRect.pivot = new Vector2(0.5f, 1);
            bgRect.sizeDelta = new Vector2(0, 80);
            var bgImage = bgObj.AddComponent<Image>();
            bgImage.color = new Color(theme.SecondaryColor.r, theme.SecondaryColor.g, theme.SecondaryColor.b, 0.9f);

            // 水平布局
            var hGroup = new GameObject("Content");
            hGroup.transform.SetParent(bgObj.transform, false);
            var contentRect = hGroup.AddComponent<RectTransform>();
            contentRect.anchorMin = Vector2.zero;
            contentRect.anchorMax = Vector2.one;
            contentRect.sizeDelta = Vector2.zero;
            var layout = hGroup.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(20, 20, 10, 10);
            layout.spacing = 30;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childForceExpandWidth = false;

            // 金钱
            UIPlaceholderGenerator.CreateHUDBox("MoneyBox", hGroup.transform, "💰", "¥10,000", theme);

            // 口碑
            UIPlaceholderGenerator.CreateHUDBox("ReputationBox", hGroup.transform, "⭐", "50", theme);

            // 天数
            UIPlaceholderGenerator.CreateHUDBox("DayBox", hGroup.transform, "📅", "第 1 天", theme);

            // 右侧按钮
            var rightContainer = new GameObject("RightButtons");
            rightContainer.transform.SetParent(hGroup.transform, false);
            var rightRect = rightContainer.AddComponent<RectTransform>();
            rightRect.anchorMin = new Vector2(1, 0.5f);
            rightRect.anchorMax = new Vector2(1, 0.5f);
            rightRect.pivot = new Vector2(1, 0.5f);
            rightRect.anchoredPosition = new Vector2(-20, 0);
            var rightLayout = rightContainer.AddComponent<HorizontalLayoutGroup>();
            rightLayout.spacing = 10;
            rightLayout.childForceExpandWidth = false;

            UIPlaceholderGenerator.CreateIconButton("SettingsBtn", rightContainer.transform, 40, theme);
            UIPlaceholderGenerator.CreateIconButton("PauseBtn", rightContainer.transform, 40, theme);

            return hudObj;
        }

        /// <summary>
        /// 创建店铺主界面
        /// </summary>
        public GameObject BuildShopMainScreen()
        {
            canvas = CreateCanvas("ShopCanvas");

            // 店铺背景（占位）
            var bgObj = new GameObject("ShopBackground");
            bgObj.transform.SetParent(canvas.transform, false);
            var bgRect = bgObj.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            var bgImage = bgObj.AddComponent<Image>();
            bgImage.color = new Color(0.9f, 0.88f, 0.85f); // 温暖的米色背景

            // 店铺标题
            var titleText = CreateText("ShopTitle", "我的车店", canvas.transform, 32, FontStyle.Bold);
            titleText.alignment = TextAnchor.MiddleCenter;
            var titleRect = titleText.GetComponent<RectTransform>();
            titleRect.anchoredPosition = new Vector2(0, 350);

            // 可点击区域提示
            var showroomArea = CreateClickableArea("ShowroomArea", "🚲 商品展示区", canvas.transform, new Vector2(300, 200), new Vector2(-400, 0), theme);
            var repairArea = CreateClickableArea("RepairArea", "🔧 维修工作台", canvas.transform, new Vector2(250, 150), new Vector2(500, -100), theme);
            var customerArea = CreateClickableArea("CustomerArea", "👥 顾客入口", canvas.transform, new Vector2(150, 200), new Vector2(-500, -50), theme);

            // 底部导航
            CreateBottomNavigation(canvas.transform);

            return canvas;
        }

        /// <summary>
        /// 创建库存界面
        /// </summary>
        public GameObject BuildInventoryScreen()
        {
            canvas = CreateCanvas("InventoryCanvas");

            // 主面板
            var mainPanel = UIPlaceholderGenerator.CreatePanel("MainPanel", canvas.transform, 1800, 900, theme);
            mainPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            // 标题栏
            var headerPanel = UIPlaceholderGenerator.CreatePanel("Header", mainPanel.transform, 1800, 60, theme);
            var headerRect = headerPanel.GetComponent<RectTransform>();
            headerRect.anchorMin = new Vector2(0, 1);
            headerRect.anchorMax = new Vector2(1, 1);
            headerRect.anchoredPosition = new Vector2(0, -30);

            var titleText = CreateText("Title", "库存管理", headerPanel.transform, 24, FontStyle.Bold);
            titleText.alignment = TextAnchor.MiddleCenter;

            // 返回按钮
            var backBtn = UIPlaceholderGenerator.CreateButton("BackBtn", "← 返回", headerPanel.transform, theme);
            var backBtnRect = backBtn.GetComponent<RectTransform>();
            backBtnRect.anchorMin = new Vector2(0, 0.5f);
            backBtnRect.anchorMax = new Vector2(0, 0.5f);
            backBtnRect.anchoredPosition = new Vector2(120, 0);
            backBtnRect.sizeDelta = new Vector2(100, 40);

            // 左侧分类
            var categoryPanel = UIPlaceholderGenerator.CreatePanel("CategoryPanel", mainPanel.transform, 200, 800, theme);
            var categoryRect = categoryPanel.GetComponent<RectTransform>();
            categoryRect.anchorMin = new Vector2(0, 0.5f);
            categoryRect.anchorMax = new Vector2(0, 0.5f);
            categoryRect.anchoredPosition = new Vector2(110, 0);

            // 右侧商品列表
            var itemListPanel = UIPlaceholderGenerator.CreatePanel("ItemListPanel", mainPanel.transform, 1500, 800, theme);
            var itemRect = itemListPanel.GetComponent<RectTransform>();
            itemRect.anchorMin = new Vector2(1, 0.5f);
            itemRect.anchorMax = new Vector2(1, 0.5f);
            itemRect.anchoredPosition = new Vector2(-780, 0);

            return canvas;
        }

        /// <summary>
        /// 创建顾客接待界面
        /// </summary>
        public GameObject BuildCustomerScreen()
        {
            canvas = CreateCanvas("CustomerCanvas");

            // 主面板
            var mainPanel = UIPlaceholderGenerator.CreatePanel("MainPanel", canvas.transform, 1000, 600, theme);
            mainPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            // 顾客信息区域
            var customerPanel = UIPlaceholderGenerator.CreatePanel("CustomerInfo", mainPanel.transform, 960, 100, theme);
            var customerRect = customerPanel.GetComponent<RectTransform>();
            customerRect.anchorMin = new Vector2(0.5f, 1);
            customerRect.anchorMax = new Vector2(0.5f, 1);
            customerRect.anchoredPosition = new Vector2(0, -30);

            // 对话气泡
            var dialogueBubble = UIPlaceholderGenerator.CreateDialogueBubble("DialogueBubble", mainPanel.transform, theme);
            var bubbleRect = dialogueBubble.GetComponent<RectTransform>();
            bubbleRect.anchorMin = new Vector2(0.5f, 0.5f);
            bubbleRect.anchorMax = new Vector2(0.5f, 0.5f);
            bubbleRect.anchoredPosition = new Vector2(0, 100);

            // 推荐商品区域
            var recommendPanel = UIPlaceholderGenerator.CreatePanel("RecommendPanel", mainPanel.transform, 960, 200, theme);
            var recommendRect = recommendPanel.GetComponent<RectTransform>();
            recommendRect.anchorMin = new Vector2(0.5f, 0);
            recommendRect.anchorMax = new Vector2(0.5f, 0);
            recommendRect.anchoredPosition = new Vector2(0, 30);

            // 水平布局
            var hGroup = recommendPanel.AddComponent<HorizontalLayoutGroup>();
            hGroup.padding = new RectOffset(20, 20, 20, 20);
            hGroup.spacing = 20;
            hGroup.childAlignment = TextAnchor.MiddleCenter;
            hGroup.childForceExpandWidth = false;

            // 推荐商品卡片
            for (int i = 0; i < 3; i++)
            {
                var card = UIPlaceholderGenerator.CreateItemCard($"ItemCard{i}", hGroup.transform, 180, 160, theme);
                var cardLayout = card.AddComponent<VerticalLayoutGroup>();
                cardLayout.padding = new RectOffset(10, 10, 10, 10);
                cardLayout.spacing = 5;
            }

            return canvas;
        }

        #region 私有工具方法

        private GameObject CreateCanvas(string name)
        {
            var canvasObj = new GameObject(name);
            var canvasComp = canvasObj.AddComponent<Canvas>();
            canvasComp.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            canvasObj.AddComponent<GraphicRaycaster>();

            return canvasObj;
        }

        private Text CreateText(string name, string content, Transform parent, int fontSize, FontStyle style)
        {
            var textObj = new GameObject(name);
            textObj.transform.SetParent(parent, false);

            var rect = textObj.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;

            var text = textObj.AddComponent<Text>();
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.fontStyle = style;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = theme.TextLight;

            return text;
        }

        private GameObject CreateClickableArea(string name, string label, Transform parent, Vector2 size, Vector2 position, UITheme theme)
        {
            var areaObj = new GameObject(name);
            areaObj.transform.SetParent(parent, false);

            var rect = areaObj.AddComponent<RectTransform>();
            rect.sizeDelta = size;
            rect.anchoredPosition = position;

            var image = areaObj.AddComponent<Image>();
            image.sprite = UIPlaceholderGenerator.CreateRoundedRectSprite(size.x, size.y, 12, new Color(0.3f, 0.3f, 0.3f, 0.5f));
            image.type = Image.Type.Sliced;

            // 添加标签
            var labelObj = new GameObject("Label");
            labelObj.transform.SetParent(areaObj.transform, false);
            var labelRect = labelObj.AddComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.sizeDelta = Vector2.zero;

            var labelText = labelObj.AddComponent<Text>();
            labelText.text = label;
            labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            labelText.fontSize = 18;
            labelText.alignment = TextAnchor.MiddleCenter;
            labelText.color = Color.white;

            // 添加按钮组件
            var button = areaObj.AddComponent<Button>();
            button.targetGraphic = image;

            return areaObj;
        }

        private void CreateBottomNavigation(Transform parent)
        {
            var navObj = new GameObject("BottomNavigation");
            navObj.transform.SetParent(parent, false);

            var navRect = navObj.AddComponent<RectTransform>();
            navRect.anchorMin = new Vector2(0, 0);
            navRect.anchorMax = new Vector2(1, 0);
            navRect.sizeDelta = new Vector2(0, 80);

            var navImage = navObj.AddComponent<Image>();
            navImage.color = theme.SecondaryColor;

            // 水平布局
            var layout = navObj.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(50, 50, 15, 15);
            layout.spacing = 50;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childForceExpandWidth = false;

            // 导航按钮
            UIPlaceholderGenerator.CreateButton("InventoryBtn", "📦 库存", navObj.transform, theme);
            UIPlaceholderGenerator.CreateButton("CustomerBtn", "👥 顾客", navObj.transform, theme);
            UIPlaceholderGenerator.CreateButton("RepairBtn", "🔧 维修", navObj.transform, theme);
            UIPlaceholderGenerator.CreateButton("UpgradeBtn", "⬆️ 升级", navObj.transform, theme);
        }

        #endregion
    }
}