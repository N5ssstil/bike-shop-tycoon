using UnityEngine;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// UI 主题配置 - 定义全局颜色和样式
    /// </summary>
    [CreateAssetMenu(fileName = "UITheme", menuName = "BikeShop/UI Theme")]
    public class UITheme : ScriptableObject
    {
        [Header("主色调")]
        public Color PrimaryColor = new Color(0.18f, 0.8f, 0.44f);      // 骑行绿 #2ECC71
        public Color SecondaryColor = new Color(0.1f, 0.1f, 0.1f);       // 碳黑 #1A1A1A
        public Color AccentColor = new Color(0.9f, 0.5f, 0.13f);         // 警示橙 #E67E22

        [Header("背景色")]
        public Color BackgroundColor = new Color(0.96f, 0.96f, 0.96f);   // 浅灰 #F5F5F5
        public Color PanelColor = new Color(0.95f, 0.95f, 0.95f);        // 面板背景

        [Header("文字色")]
        public Color TextPrimary = new Color(0.2f, 0.2f, 0.2f);          // 深灰 #333333
        public Color TextSecondary = new Color(0.5f, 0.5f, 0.5f);        // 次要文字
        public Color TextLight = new Color(1f, 1f, 1f);                  // 浅色文字

        [Header("状态色")]
        public Color SuccessColor = new Color(0.18f, 0.8f, 0.44f);       // 成功绿
        public Color WarningColor = new Color(0.9f, 0.7f, 0.2f);         // 警告黄
        public Color ErrorColor = new Color(0.9f, 0.3f, 0.24f);          // 错误红
        public Color InfoColor = new Color(0.2f, 0.6f, 0.86f);           // 信息蓝

        [Header("特殊色")]
        public Color MoneyColor = new Color(0.95f, 0.77f, 0.06f);        // 金币金 #F1C40F
        public Color ReputationColor = new Color(0.2f, 0.6f, 0.86f);     // 口碑蓝 #3498DB

        [Header("按钮颜色")]
        public Color ButtonNormal = new Color(0.18f, 0.8f, 0.44f);
        public Color ButtonHover = new Color(0.22f, 0.85f, 0.5f);
        public Color ButtonPressed = new Color(0.15f, 0.7f, 0.38f);
        public Color ButtonDisabled = new Color(0.5f, 0.5f, 0.5f);

        [Header("尺寸设置")]
        public float ButtonHeight = 50f;
        public float ButtonWidth = 200f;
        public float CornerRadius = 8f;
        public float BorderWidth = 2f;

        /// <summary>
        /// 获取默认主题
        /// </summary>
        public static UITheme GetDefaultTheme()
        {
            var theme = CreateInstance<UITheme>();
            return theme;
        }
    }
}