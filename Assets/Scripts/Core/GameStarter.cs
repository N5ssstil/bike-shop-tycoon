using UnityEngine;
using BikeShopTycoon.Core;
using BikeShopTycoon.GameSystems;
using BikeShopTycoon.UI;

namespace BikeShopTycoon
{
    /// <summary>
    /// 游戏启动器 - 初始化所有系统
    /// 放置在场景中确保系统正确初始化
    /// </summary>
    public class GameStarter : MonoBehaviour
    {
        [Header("系统引用")]
        public CustomerGeneratorSettings customerSettings;

        [Header("调试")]
        public bool debugMode = true;

        private void Awake()
        {
            Debug.Log("[GameStarter] 初始化游戏系统...");

            // 确保 GameManager 存在
            if (GameManager.Instance == null)
            {
                Debug.LogError("[GameStarter] GameManager 未找到！请确保场景中有 GameManager 对象。");
                return;
            }

            // 确保 TimeManager 存在
            if (TimeManager.Instance == null)
            {
                Debug.LogError("[GameStarter] TimeManager 未找到！请确保场景中有 TimeManager 对象。");
                return;
            }

            // 确保 UIManager 存在
            if (UIManager.Instance == null)
            {
                Debug.LogError("[GameStarter] UIManager 未找到！请确保场景中有 UIManager 对象。");
                return;
            }
        }

        private void Start()
        {
            Debug.Log("[GameStarter] 游戏启动完成！");
            
            if (debugMode)
            {
                Debug.Log($"[GameStarter] 玩家资金: ¥{GameManager.Instance.PlayerData.Money:N0}");
                Debug.Log($"[GameStarter] 玩家口碑: {GameManager.Instance.PlayerData.Reputation}");
                Debug.Log($"[GameStarter] 当前天数: {TimeManager.Instance.currentDay}");
            }
        }

        /// <summary>
        /// 编辑器菜单 - 创建基础场景配置
        /// </summary>
#if UNITY_EDITOR
        [UnityEditor.MenuItem("BikeShop/Setup Scene", false, 100)]
        public static void SetupScene()
        {
            // 创建 GameManager
            var gmGO = new GameObject("[GameManager]");
            gmGO.AddComponent<GameManager>();
            UnityEditor.Undo.RegisterCreatedObjectUndo(gmGO, "Create GameManager");

            // 创建 TimeManager
            var tmGO = new GameObject("[TimeManager]");
            tmGO.AddComponent<TimeManager>();
            UnityEditor.Undo.RegisterCreatedObjectUndo(tmGO, "Create TimeManager");

            // 创建 UIManager
            var uiGO = new GameObject("[UIManager]");
            uiGO.AddComponent<UIManager>();
            UnityEditor.Undo.RegisterCreatedObjectUndo(uiGO, "Create UIManager");

            // 创建 EventSystem
            var eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            UnityEditor.Undo.RegisterCreatedObjectUndo(eventSystemGO, "Create EventSystem");

            Debug.Log("[GameStarter] 场景配置完成！请手动添加 UI 面板引用。");
        }
#endif
    }
}