using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// UI 预制体创建器 - 运行时动态创建 UI 元素
    /// </summary>
    public static class UIPrefabCreator
    {
        // 默认字体（使用 Unity 内置）
        private static TMP_FontAsset defaultFont;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            // 尝试加载默认字体
            defaultFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        }

        #region 基础 UI 元素

        /// <summary>
        /// 创建基础面板
        /// </summary>
        public static GameObject CreatePanel(string name, Transform parent = null)
        {
            var panel = new GameObject(name);
            panel.transform.SetParent(parent);

            var rectTransform = panel.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;

            var image = panel.AddComponent<Image>();
            image.color = new Color(0.12f, 0.12f, 0.15f, 0.95f);

            return panel;
        }

        /// <summary>
        /// 创建按钮
        /// </summary>
        public static GameObject CreateButton(string name, string text, Transform parent = null)
        {
            var buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent);

            var rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200, 50);

            var image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.2f, 0.4f, 0.6f);

            var button = buttonObj.AddComponent<Button>();

            // 添加文本
            var textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform);

            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            var tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 20;
            tmp.color = Color.white;

            return buttonObj;
        }

        /// <summary>
        /// 创建文本
        /// </summary>
        public static GameObject CreateText(string name, string text, Transform parent = null, int fontSize = 24)
        {
            var textObj = new GameObject(name);
            textObj.transform.SetParent(parent);

            var rectTransform = textObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200, 40);

            var tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = fontSize;
            tmp.color = Color.white;

            return textObj;
        }

        /// <summary>
        /// 创建图标按钮
        /// </summary>
        public static GameObject CreateIconButton(string name, string icon, Transform parent = null)
        {
            var buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent);

            var rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(60, 60);

            var image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.25f);

            var button = buttonObj.AddComponent<Button>();

            // 添加图标文本
            var iconObj = new GameObject("Icon");
            iconObj.transform.SetParent(buttonObj.transform);

            var iconRect = iconObj.AddComponent<RectTransform>();
            iconRect.anchorMin = Vector2.zero;
            iconRect.anchorMax = Vector2.one;
            iconRect.sizeDelta = Vector2.zero;

            var tmp = iconObj.AddComponent<TextMeshProUGUI>();
            tmp.text = icon;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 28;

            return buttonObj;
        }

        #endregion

        #region 游戏特定 UI

        /// <summary>
        /// 创建顾客卡片
        /// </summary>
        public static GameObject CreateCustomerCard(Transform parent = null)
        {
            var card = new GameObject("CustomerCard");
            card.transform.SetParent(parent);

            var rectTransform = card.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 120);

            var image = card.AddComponent<Image>();
            image.color = new Color(0.15f, 0.15f, 0.18f);

            // 添加边框效果
            var outline = card.AddComponent<Outline>();
            outline.effectColor = new Color(0.3f, 0.3f, 0.35f);
            outline.effectDistance = new Vector2(2, -2);

            // 名字
            var nameText = CreateText("NameText", "顾客名称", card.transform);
            var nameRect = nameText.GetComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0, 0.7f);
            nameRect.anchorMax = new Vector2(1, 1);
            nameRect.sizeDelta = new Vector2(-20, 0);
            nameRect.anchoredPosition = new Vector2(10, 0);

            // 类型
            var typeText = CreateText("TypeText", "类型", card.transform);
            var typeRect = typeText.GetComponent<RectTransform>();
            typeRect.anchorMin = new Vector2(0, 0.4f);
            typeRect.anchorMax = new Vector2(0.5f, 0.7f);
            typeRect.sizeDelta = new Vector2(-10, 0);
            typeText.GetComponent<TextMeshProUGUI>().fontSize = 18;
            typeText.GetComponent<TextMeshProUGUI>().color = new Color(0.7f, 0.7f, 0.8f);

            // 预算
            var budgetText = CreateText("BudgetText", "¥0", card.transform);
            var budgetRect = budgetText.GetComponent<RectTransform>();
            budgetRect.anchorMin = new Vector2(0.5f, 0.4f);
            budgetRect.anchorMax = new Vector2(1, 0.7f);
            budgetRect.sizeDelta = new Vector2(-10, 0);
            budgetText.GetComponent<TextMeshProUGUI>().fontSize = 18;
            budgetText.GetComponent<TextMeshProUGUI>().color = new Color(0.4f, 0.8f, 0.4f);
            budgetText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;

            // 交互按钮
            var interactBtn = CreateButton("InteractBtn", "接待", card.transform);
            var btnRect = interactBtn.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0, 0);
            btnRect.anchorMax = new Vector2(1, 0.35f);
            btnRect.sizeDelta = new Vector2(-20, 0);
            btnRect.anchoredPosition = new Vector2(10, 5);

            return card;
        }

        /// <summary>
        /// 创建商品卡片
        /// </summary>
        public static GameObject CreateProductCard(Transform parent = null)
        {
            var card = new GameObject("ProductCard");
            card.transform.SetParent(parent);

            var rectTransform = card.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(280, 100);

            var image = card.AddComponent<Image>();
            image.color = new Color(0.18f, 0.18f, 0.2f);

            // 名称
            var nameText = CreateText("NameText", "商品名称", card.transform);
            var nameRect = nameText.GetComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0, 0.6f);
            nameRect.anchorMax = new Vector2(1, 1);
            nameRect.sizeDelta = new Vector2(-10, 0);

            // 进价
            var costText = CreateText("CostText", "进价: ¥0", card.transform);
            var costRect = costText.GetComponent<RectTransform>();
            costRect.anchorMin = new Vector2(0, 0.3f);
            costRect.anchorMax = new Vector2(0.5f, 0.6f);
            costRect.sizeDelta = new Vector2(-5, 0);
            costText.GetComponent<TextMeshProUGUI>().fontSize = 16;
            costText.GetComponent<TextMeshProUGUI>().color = new Color(0.9f, 0.5f, 0.3f);

            // 售价
            var priceText = CreateText("PriceText", "售价: ¥0", card.transform);
            var priceRect = priceText.GetComponent<RectTransform>();
            priceRect.anchorMin = new Vector2(0.5f, 0.3f);
            priceRect.anchorMax = new Vector2(1, 0.6f);
            priceRect.sizeDelta = new Vector2(-5, 0);
            priceText.GetComponent<TextMeshProUGUI>().fontSize = 16;
            priceText.GetComponent<TextMeshProUGUI>().color = new Color(0.4f, 0.8f, 0.4f);

            // 数量控制
            var quantityText = CreateText("QuantityText", "0", card.transform);
            var qtyRect = quantityText.GetComponent<RectTransform>();
            qtyRect.anchorMin = new Vector2(0, 0);
            qtyRect.anchorMax = new Vector2(0.3f, 0.3f);
            qtyRect.sizeDelta = Vector2.zero;
            quantityText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

            // 添加按钮
            var addBtn = CreateButton("AddBtn", "+", card.transform);
            var addRect = addBtn.GetComponent<RectTransform>();
            addRect.anchorMin = new Vector2(0.7f, 0);
            addRect.anchorMax = new Vector2(0.85f, 0.3f);
            addRect.sizeDelta = Vector2.zero;

            // 减少按钮
            var removeBtn = CreateButton("RemoveBtn", "-", card.transform);
            var removeRect = removeBtn.GetComponent<RectTransform>();
            removeRect.anchorMin = new Vector2(0.85f, 0);
            removeRect.anchorMax = new Vector2(1, 0.3f);
            removeRect.sizeDelta = Vector2.zero;

            return card;
        }

        /// <summary>
        /// 创建维修工单卡片
        /// </summary>
        public static GameObject CreateRepairJobCard(Transform parent = null)
        {
            var card = new GameObject("RepairJobCard");
            card.transform.SetParent(parent);

            var rectTransform = card.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(350, 80);

            var image = card.AddComponent<Image>();
            image.color = new Color(0.15f, 0.18f, 0.2f);

            // 类型
            var typeText = CreateText("TypeText", "维修类型", card.transform);
            var typeRect = typeText.GetComponent<RectTransform>();
            typeRect.anchorMin = new Vector2(0, 0.5f);
            typeRect.anchorMax = new Vector2(0.4f, 1);
            typeRect.sizeDelta = new Vector2(-10, 0);

            // 顾客
            var customerText = CreateText("CustomerText", "顾客", card.transform);
            var customerRect = customerText.GetComponent<RectTransform>();
            customerRect.anchorMin = new Vector2(0.4f, 0.5f);
            customerRect.anchorMax = new Vector2(0.7f, 1);
            customerRect.sizeDelta = new Vector2(-5, 0);
            customerText.GetComponent<TextMeshProUGUI>().fontSize = 18;
            customerText.GetComponent<TextMeshProUGUI>().color = new Color(0.7f, 0.7f, 0.8f);

            // 费用
            var costText = CreateText("CostText", "¥0", card.transform);
            var costRect = costText.GetComponent<RectTransform>();
            costRect.anchorMin = new Vector2(0.7f, 0.5f);
            costRect.anchorMax = new Vector2(1, 1);
            costRect.sizeDelta = new Vector2(-10, 0);
            costText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
            costText.GetComponent<TextMeshProUGUI>().color = new Color(0.4f, 0.8f, 0.4f);

            // 完成按钮
            var completeBtn = CreateButton("CompleteBtn", "完成", card.transform);
            var compRect = completeBtn.GetComponent<RectTransform>();
            compRect.anchorMin = new Vector2(0, 0);
            compRect.anchorMax = new Vector2(0.5f, 0.45f);
            compRect.sizeDelta = new Vector2(-5, 0);

            // 取消按钮
            var cancelBtn = CreateButton("CancelButton", "取消", card.transform);
            cancelBtn.GetComponent<Image>().color = new Color(0.6f, 0.2f, 0.2f);
            var cancRect = cancelBtn.GetComponent<RectTransform>();
            cancRect.anchorMin = new Vector2(0.5f, 0);
            cancRect.anchorMax = new Vector2(1, 0.45f);
            cancRect.sizeDelta = new Vector2(-5, 0);

            return card;
        }

        /// <summary>
        /// 创建通知条
        /// </summary>
        public static GameObject CreateNotification(Transform parent = null)
        {
            var notification = new GameObject("Notification");
            notification.transform.SetParent(parent);

            var rectTransform = notification.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(400, 50);

            var image = notification.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.25f);

            // 文本
            var textObj = CreateText("Text", "通知内容", notification.transform);
            var textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = new Vector2(-20, 0);
            textObj.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            textObj.GetComponent<TextMeshProUGUI>().fontSize = 18;

            return notification;
        }

        #endregion
    }
}