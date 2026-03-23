using UnityEngine;
using UnityEngine.SceneManagement;
using BikeShopTycoon.Core;
using BikeShopTycoon.UI;
using BikeShopTycoon.GameSystems;

namespace BikeShopTycoon
{
    /// <summary>
    /// 场景启动器 - 确保场景包含所有必要组件
    /// </summary>
    public class SceneBootstrapper : MonoBehaviour
    {
        [Header("启动场景")]
        [SerializeField] private string gameSceneName = "Shop";

        private void Awake()
        {
            EnsureSystemsExist();
        }

        private void EnsureSystemsExist()
        {
            // 确保 GameManager 存在
            if (GameManager.Instance == null)
            {
                var gmPrefab = Resources.Load<GameObject>("Prefabs/[GameManager]");
                if (gmPrefab != null)
                {
                    Instantiate(gmPrefab);
                }
                else
                {
                    var gmObj = new GameObject("[GameManager]");
                    gmObj.AddComponent<GameManager>();
                    DontDestroyOnLoad(gmObj);
                }
            }

            // 确保 TimeManager 存在
            if (TimeManager.Instance == null)
            {
                var tmObj = new GameObject("[TimeManager]");
                tmObj.AddComponent<TimeManager>();
                DontDestroyOnLoad(tmObj);
            }

            // 确保 EventSystem 存在
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                var esObj = new GameObject("EventSystem");
                esObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
                esObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            // 确保有 Canvas 和 UI
            if (FindObjectOfType<Canvas>() == null)
            {
                var uiBuilderObj = new GameObject("[UIBuilder]");
                uiBuilderObj.AddComponent<UIBuilder>();
            }

            Debug.Log("[SceneBootstrapper] 所有系统初始化完成");
        }

        /// <summary>
        /// 加载游戏场景
        /// </summary>
        public void LoadGameScene()
        {
            SceneManager.LoadScene(gameSceneName);
        }

        /// <summary>
        /// 加载主菜单
        /// </summary>
        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void QuitGame()
        {
            GameManager.Instance?.SaveGame();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}