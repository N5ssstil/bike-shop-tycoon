using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// 主菜单控制器
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [Header("按钮")]
        public Button newGameButton;
        public Button continueButton;
        public Button quitButton;

        [Header("标题")]
        public TextMeshProUGUI titleText;

        private void Awake()
        {
            // 如果按钮为空，尝试动态创建
            if (newGameButton == null)
            {
                CreateButton("NewGameButton", "新游戏", new Vector2(0, 50), StartNewGame);
            }
            if (continueButton == null)
            {
                CreateButton("ContinueButton", "继续游戏", new Vector2(0, 0), ContinueGame);
            }
            if (quitButton == null)
            {
                CreateButton("QuitButton", "退出游戏", new Vector2(0, -50), QuitGame);
            }
        }

        private void Start()
        {
            // 检查是否有存档
            if (continueButton != null)
            {
                continueButton.interactable = SaveSystem.HasSave();
            }

            // 创建标题
            if (titleText == null)
            {
                CreateTitle();
            }
        }

        private void CreateButton(string name, string text, Vector2 position, UnityEngine.Events.UnityAction onClick)
        {
            var buttonGO = new GameObject(name);
            buttonGO.transform.SetParent(transform, false);

            var rectTransform = buttonGO.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(200, 50);
            rectTransform.anchoredPosition = position;

            var image = buttonGO.AddComponent<Image>();
            image.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            var button = buttonGO.AddComponent<Button>();
            button.onClick.AddListener(onClick);

            // 添加文本
            var textGO = new GameObject("Text");
            textGO.transform.SetParent(buttonGO.transform, false);
            var textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            var tmp = textGO.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 24;
            tmp.color = Color.white;

            // 保存引用
            if (name == "NewGameButton") newGameButton = button;
            else if (name == "ContinueButton") continueButton = button;
            else if (name == "QuitButton") quitButton = button;
        }

        private void CreateTitle()
        {
            var titleGO = new GameObject("Title");
            titleGO.transform.SetParent(transform, false);

            var rectTransform = titleGO.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0, -100);
            rectTransform.sizeDelta = new Vector2(600, 100);

            titleText = titleGO.AddComponent<TextMeshProUGUI>();
            titleText.text = "🚴 公路车大亨";
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.fontSize = 48;
            titleText.fontStyle = FontStyles.Bold;
            titleText.color = Color.white;
        }

        public void StartNewGame()
        {
            Debug.Log("[MainMenu] 开始新游戏");
            if (UIManager.Instance != null)
            {
                UIManager.Instance.StartNewGame();
            }
        }

        public void ContinueGame()
        {
            Debug.Log("[MainMenu] 继续游戏");
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ContinueGame();
            }
        }

        public void QuitGame()
        {
            Debug.Log("[MainMenu] 退出游戏");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}