using UnityEngine;
using UnityEngine.SceneManagement;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// 主菜单 UI 控制器
    /// 负责处理主菜单按钮事件
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("场景名称")]
        public string shopSceneName = "ShopScene";
        
        [Header("按钮引用")]
        public GameObject continueButton; // 继续游戏按钮

        private void Start()
        {
            // 初始化时检查存档状态，更新继续游戏按钮
            UpdateContinueButton();
        }

        /// <summary>
        /// 开始新游戏
        /// </summary>
        public void OnStartNewGame()
        {
            GameManager.Instance.StartNewGame();
            LoadShopScene();
        }

        /// <summary>
        /// 继续游戏（加载存档）
        /// </summary>
        public void OnContinueGame()
        {
            if (SaveSystem.HasSave())
            {
                var loadResult = SaveSystem.LoadGameWithResult();
                if (loadResult.Success)
                {
                    // 恢复玩家数据
                    GameManager.Instance.PlayerData = loadResult.Data;
                    // 触发状态更新事件
                    GameManager.Instance.OnMoneyChanged?.Invoke(GameManager.Instance.PlayerData.Money);
                    GameManager.Instance.OnReputationChanged?.Invoke(GameManager.Instance.PlayerData.Reputation);
                    
                    LoadShopScene();
                }
                else
                {
                    Debug.LogError("存档加载失败: " + loadResult.ErrorMessage);
                    // 可选：显示错误提示UI
                }
            }
        }

        /// <summary>
        /// 打开设置界面（预留）
        /// </summary>
        public void OnOpenSettings()
        {
            // TODO: 实现设置界面逻辑
            Debug.Log("打开设置");
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void OnQuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        /// <summary>
        /// 加载店铺场景
        /// </summary>
        private void LoadShopScene()
        {
            SceneManager.LoadScene(shopSceneName);
        }

        /// <summary>
        /// 根据存档状态更新继续游戏按钮
        /// </summary>
        private void UpdateContinueButton()
        {
            if (continueButton != null)
            {
                continueButton.SetActive(SaveSystem.HasSave());
            }
        }
    }
}