#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using BikeShopTycoon.UI;

namespace BikeShopTycoon.Editor
{
    /// <summary>
    /// UI 占位资源生成工具 - 编辑器窗口
    /// </summary>
    public class UIPlaceholderTool : EditorWindow
    {
        [MenuItem("BikeShop/UI 生成工具")]
        public static void ShowWindow()
        {
            var window = GetWindow<UIPlaceholderTool>("UI 生成工具");
            window.minSize = new Vector2(300, 400);
        }

        private UITheme theme;
        private GameObject generatedObject;

        private void OnGUI()
        {
            GUILayout.Label("UI 占位资源生成器", EditorStyles.boldLabel);
            GUILayout.Space(10);

            // 主题选择
            theme = (UITheme)EditorGUILayout.ObjectField("UI 主题", theme, typeof(UITheme), false);
            if (theme == null)
            {
                if (GUILayout.Button("创建默认主题"))
                {
                    theme = UITheme.GetDefaultTheme();
                    AssetDatabase.CreateAsset(theme, "Assets/ScriptableObjects/DefaultUITheme.asset");
                    AssetDatabase.SaveAssets();
                }
            }

            GUILayout.Space(20);
            GUILayout.Label("界面生成", EditorStyles.boldLabel);

            // 主菜单
            if (GUILayout.Button("生成主菜单"))
            {
                GenerateUI("MainMenu", builder => builder.BuildMainMenu());
            }

            // HUD
            if (GUILayout.Button("生成 HUD"))
            {
                GenerateUI("HUD", builder => builder.BuildHUD());
            }

            // 店铺主界面
            if (GUILayout.Button("生成店铺主界面"))
            {
                GenerateUI("ShopMain", builder => builder.BuildShopMainScreen());
            }

            // 库存界面
            if (GUILayout.Button("生成库存界面"))
            {
                GenerateUI("Inventory", builder => builder.BuildInventoryScreen());
            }

            // 顾客界面
            if (GUILayout.Button("生成顾客接待界面"))
            {
                GenerateUI("Customer", builder => builder.BuildCustomerScreen());
            }

            GUILayout.Space(20);

            // 清除生成的对象
            if (generatedObject != null && GUILayout.Button("清除生成的 UI"))
            {
                DestroyImmediate(generatedObject);
                generatedObject = null;
            }

            GUILayout.Space(20);
            GUILayout.Label("单个元素生成", EditorStyles.boldLabel);

            // 单个元素
            if (GUILayout.Button("生成按钮"))
            {
                var btn = UIPlaceholderGenerator.CreateButton("Button", "按钮", Selection.activeTransform, theme);
                Selection.activeGameObject = btn;
            }

            if (GUILayout.Button("生成面板"))
            {
                var panel = UIPlaceholderGenerator.CreatePanel("Panel", Selection.activeTransform, 400, 300, theme);
                Selection.activeGameObject = panel;
            }

            if (GUILayout.Button("生成进度条"))
            {
                var bar = UIPlaceholderGenerator.CreateProgressBar("ProgressBar", Selection.activeTransform, 200, 20, theme);
                Selection.activeGameObject = bar;
            }

            if (GUILayout.Button("生成商品卡片"))
            {
                var card = UIPlaceholderGenerator.CreateItemCard("ItemCard", Selection.activeTransform, 250, 80, theme);
                Selection.activeGameObject = card;
            }

            if (GUILayout.Button("生成对话气泡"))
            {
                var bubble = UIPlaceholderGenerator.CreateDialogueBubble("DialogueBubble", Selection.activeTransform, theme);
                Selection.activeGameObject = bubble;
            }
        }

        private void GenerateUI(string name, System.Func<UIPresetBuilder, GameObject> buildFunc)
        {
            var builder = new UIPresetBuilder(theme);
            generatedObject = buildFunc(builder);
            generatedObject.name = name;
            Selection.activeGameObject = generatedObject;
            Debug.Log($"已生成: {name}");
        }
    }
}
#endif