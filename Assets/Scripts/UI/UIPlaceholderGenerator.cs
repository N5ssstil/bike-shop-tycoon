using UnityEngine;
using UnityEngine.UI;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// UI 占位资源生成器
    /// 通过代码创建基础 UI 元素，用于开发阶段
    /// 后期可替换为正式美术资源
    /// </summary>
    public static class UIPlaceholderGenerator
    {
        /// <summary>
        /// 创建圆角矩形按钮
        /// </summary>
        public static GameObject CreateButton(string name, string text, Transform parent, UITheme theme = null)
        {
            theme ??= UITheme.GetDefaultTheme();

            var buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent, false);

            // 添加 RectTransform
            var rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(theme.ButtonWidth, theme.ButtonHeight);

            // 添加 Image（按钮背景）
            var image = buttonObj.AddComponent<Image>();
            image.sprite = CreateRoundedRectSprite(theme.ButtonWidth, theme.ButtonHeight, theme.CornerRadius, theme.ButtonNormal);
            image.type = Image.Type.Sliced;

            // 添加 Button 组件
            var button = buttonObj.AddComponent<Button>();
            button.targetGraphic = image;

            // 创建文字
            var textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);

            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            var textComponent = textObj.AddComponent<Text>();
            textComponent.text = text;
            textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            textComponent.fontSize = 18;
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.color = theme.TextLight;

            // 添加按钮颜色过渡
            var colors = button.colors;
            colors.normalColor = theme.ButtonNormal;
            colors.highlightedColor = theme.ButtonHover;
            colors.pressedColor = theme.ButtonPressed;
            colors.disabledColor = theme.ButtonDisabled;
            button.colors = colors;

            return buttonObj;
        }

        /// <summary>
        /// 创建面板
        /// </summary>
        public static GameObject CreatePanel(string name, Transform parent, float width, float height, UITheme theme = null)
        {
            theme ??= UITheme.GetDefaultTheme();

            var panelObj = new GameObject(name);
            panelObj.transform.SetParent(parent, false);

            var rectTransform = panelObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);

            var image = panelObj.AddComponent<Image>();
            image.sprite = CreateRoundedRectSprite(width, height, theme.CornerRadius, theme.PanelColor);
            image.type = Image.Type.Sliced;

            return panelObj;
        }

        /// <summary>
        /// 创建图标按钮
        /// </summary>
        public static GameObject CreateIconButton(string name, Transform parent, float size, UITheme theme = null)
        {
            theme ??= UITheme.GetDefaultTheme();

            var buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent, false);

            var rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(size, size);

            var image = buttonObj.AddComponent<Image>();
            image.sprite = CreateCircleSprite(size, theme.SecondaryColor);

            var button = buttonObj.AddComponent<Button>();
            button.targetGraphic = image;

            return buttonObj;
        }

        /// <summary>
        /// 创建进度条
        /// </summary>
        public static GameObject CreateProgressBar(string name, Transform parent, float width, float height, UITheme theme = null)
        {
            theme ??= UITheme.GetDefaultTheme();

            var barObj = new GameObject(name);
            barObj.transform.SetParent(parent, false);

            var rectTransform = barObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);

            // 背景
            var bgImage = barObj.AddComponent<Image>();
            bgImage.sprite = CreateRoundedRectSprite(width, height, height / 2, theme.SecondaryColor);
            bgImage.type = Image.Type.Sliced;

            // 填充
            var fillObj = new GameObject("Fill");
            fillObj.transform.SetParent(barObj.transform, false);

            var fillRect = fillObj.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = new Vector2(0.5f, 1f);  // 初始 50%
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            var fillImage = fillObj.AddComponent<Image>();
            fillImage.sprite = CreateRoundedRectSprite(width, height, height / 2, theme.PrimaryColor);
            fillImage.type = Image.Type.Sliced;

            // 添加 Slider 组件
            var slider = barObj.AddComponent<Slider>();
            slider.targetGraphic = fillImage;

            return barObj;
        }

        /// <summary>
        /// 创建 HUD 信息框
        /// </summary>
        public static GameObject CreateHUDBox(string name, Transform parent, string iconText, string valueText, UITheme theme = null)
        {
            theme ??= UITheme.GetDefaultTheme();

            var boxObj = new GameObject(name);
            boxObj.transform.SetParent(parent, false);

            var rectTransform = boxObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150, 60);

            // 背景
            var bgImage = boxObj.AddComponent<Image>();
            bgImage.sprite = CreateRoundedRectSprite(150, 60, 8, theme.SecondaryColor);
            bgImage.type = Image.Type.Sliced;

            // 水平布局
            var layout = boxObj.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 10, 10);
            layout.spacing = 10;
            layout.childAlignment = TextAnchor.MiddleLeft;

            // 图标文字（临时用 emoji 替代）
            var iconObj = new GameObject("Icon");
            iconObj.transform.SetParent(boxObj.transform, false);
            var iconTextComp = iconObj.AddComponent<Text>();
            iconTextComp.text = iconText;
            iconTextComp.fontSize = 24;
            iconTextComp.alignment = TextAnchor.MiddleCenter;

            // 数值文字
            var valueObj = new GameObject("Value");
            valueObj.transform.SetParent(boxObj.transform, false);
            var valueTextComp = valueObj.AddComponent<Text>();
            valueTextComp.text = valueText;
            valueTextComp.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            valueTextComp.fontSize = 18;
            valueTextComp.fontStyle = FontStyle.Bold;
            valueTextComp.alignment = TextAnchor.MiddleLeft;
            valueTextComp.color = theme.TextLight;

            return boxObj;
        }

        /// <summary>
        /// 创建商品卡片
        /// </summary>
        public static GameObject CreateItemCard(string name, Transform parent, float width, float height, UITheme theme = null)
        {
            theme ??= UITheme.GetDefaultTheme();

            var cardObj = new GameObject(name);
            cardObj.transform.SetParent(parent, false);

            var rectTransform = cardObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);

            // 背景
            var bgImage = cardObj.AddComponent<Image>();
            bgImage.sprite = CreateRoundedRectSprite(width, height, theme.CornerRadius, Color.white);
            bgImage.type = Image.Type.Sliced;

            // 添加阴影效果
            var shadow = cardObj.AddComponent<Shadow>();
            shadow.effectColor = new Color(0, 0, 0, 0.1f);
            shadow.effectDistance = new Vector2(2, -2);

            // 垂直布局
            var layout = cardObj.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 10, 10);
            layout.spacing = 5;

            return cardObj;
        }

        /// <summary>
        /// 创建对话气泡
        /// </summary>
        public static GameObject CreateDialogueBubble(string name, Transform parent, UITheme theme = null)
        {
            theme ??= UITheme.GetDefaultTheme();

            var bubbleObj = new GameObject(name);
            bubbleObj.transform.SetParent(parent, false);

            var rectTransform = bubbleObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(600, 100);

            // 背景
            var bgImage = bubbleObj.AddComponent<Image>();
            bgImage.sprite = CreateRoundedRectSprite(600, 100, 12, Color.white);
            bgImage.type = Image.Type.Sliced;

            // 边框（Outline）
            var outline = bubbleObj.AddComponent<Outline>();
            outline.effectColor = new Color(0.8f, 0.8f, 0.8f);
            outline.effectDistance = new Vector2(1, -1);

            // 添加文字子对象
            var textObj = new GameObject("Text");
            textObj.transform.SetParent(bubbleObj.transform, false);

            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(15, 10);
            textRect.offsetMax = new Vector2(-15, -10);

            var textComp = textObj.AddComponent<Text>();
            textComp.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            textComp.fontSize = 16;
            textComp.color = theme.TextPrimary;
            textComp.alignment = TextAnchor.MiddleLeft;
            textComp.lineSpacing = 1.2f;

            return bubbleObj;
        }

        #region Sprite 生成工具

        /// <summary>
        /// 创建圆角矩形 Sprite
        /// </summary>
        private static Sprite CreateRoundedRectSprite(float width, float height, float radius, Color color)
        {
            int texWidth = Mathf.CeilToInt(width);
            int texHeight = Mathf.CeilToInt(height);

            Texture2D texture = new Texture2D(texWidth, texHeight);
            Color[] pixels = new Color[texWidth * texHeight];

            for (int y = 0; y < texHeight; y++)
            {
                for (int x = 0; x < texWidth; x++)
                {
                    float dist = DistanceToRoundedRect(x, y, texWidth, texHeight, radius);
                    if (dist <= 0)
                    {
                        pixels[y * texWidth + x] = color;
                    }
                    else if (dist < 1)
                    {
                        // 抗锯齿
                        pixels[y * texWidth + x] = new Color(color.r, color.g, color.b, color.a * (1 - dist));
                    }
                    else
                    {
                        pixels[y * texWidth + x] = Color.clear;
                    }
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();

            // 添加边框（用于 Sliced 模式）
            Vector4 border = new Vector4(radius, radius, radius, radius);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texWidth, texHeight), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, border);

            return sprite;
        }

        /// <summary>
        /// 创建圆形 Sprite
        /// </summary>
        private static Sprite CreateCircleSprite(float size, Color color)
        {
            int texSize = Mathf.CeilToInt(size);
            Texture2D texture = new Texture2D(texSize, texSize);
            Color[] pixels = new Color[texSize * texSize];

            float center = texSize / 2f;
            float radius = texSize / 2f;

            for (int y = 0; y < texSize; y++)
            {
                for (int x = 0; x < texSize; x++)
                {
                    float dist = Mathf.Sqrt((x - center) * (x - center) + (y - center) * (y - center));
                    if (dist <= radius)
                    {
                        // 抗锯齿
                        float alpha = Mathf.Clamp01((radius - dist) * 2);
                        pixels[y * texSize + x] = new Color(color.r, color.g, color.b, color.a * alpha);
                    }
                    else
                    {
                        pixels[y * texSize + x] = Color.clear;
                    }
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();

            return Sprite.Create(texture, new Rect(0, 0, texSize, texSize), new Vector2(0.5f, 0.5f));
        }

        /// <summary>
        /// 计算点到圆角矩形的距离
        /// </summary>
        private static float DistanceToRoundedRect(float x, float y, float width, float height, float radius)
        {
            // 四个圆角的圆心
            float minX = radius;
            float maxX = width - radius;
            float minY = radius;
            float maxY = height - radius;

            // 如果在内部矩形区域内
            if (x >= minX && x <= maxX && y >= minY && y <= maxY)
            {
                return -1; // 内部
            }

            // 计算到最近角的距离
            float nearestX = Mathf.Clamp(x, minX, maxX);
            float nearestY = Mathf.Clamp(y, minY, maxY);

            // 四个角的圆心
            float cornerX = x < minX ? minX : (x > maxX ? maxX : x);
            float cornerY = y < minY ? minY : (y > maxY ? maxY : y);

            // 选择正确的圆角圆心
            if (x < minX && y < minY)
            {
                // 左下角
                return Mathf.Sqrt((x - minX) * (x - minX) + (y - minY) * (y - minY)) - radius;
            }
            else if (x > maxX && y < minY)
            {
                // 右下角
                return Mathf.Sqrt((x - maxX) * (x - maxX) + (y - minY) * (y - minY)) - radius;
            }
            else if (x < minX && y > maxY)
            {
                // 左上角
                return Mathf.Sqrt((x - minX) * (x - minX) + (y - maxY) * (y - maxY)) - radius;
            }
            else if (x > maxX && y > maxY)
            {
                // 右上角
                return Mathf.Sqrt((x - maxX) * (x - maxX) + (y - maxY) * (y - maxY)) - radius;
            }
            else if (x < minX || x > maxX || y < minY || y > maxY)
            {
                // 在边框外部
                return 1;
            }

            return -1;
        }

        #endregion
    }
}