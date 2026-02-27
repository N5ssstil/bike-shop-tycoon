using UnityEngine;
using UnityEngine.Events;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// UI 管理器 - 管理所有 UI 面板
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("UI 面板")]
        public GameObject mainMenuPanel;
        public GameObject shopPanel;
        public GameObject inventoryPanel;
        public GameObject customerPanel;
        public GameObject repairPanel;
        public GameObject pausePanel;

        [Header("HUD")]
        public HUDController hudController;

        [Header("事件")]
        public UnityEvent<GameState> OnPanelChanged;

        private GameObject currentPanel;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            // 初始显示主菜单
            ShowPanel(mainMenuPanel);
        }

        /// <summary>
        /// 显示指定面板
        /// </summary>
        public void ShowPanel(GameObject panel)
        {
            if (currentPanel != null)
                currentPanel.SetActive(false);

            currentPanel = panel;
            currentPanel.SetActive(true);
        }

        /// <summary>
        /// 切换到店铺界面
        /// </summary>
        public void ShowShop()
        {
            ShowPanel(shopPanel);
            GameManager.Instance.ChangeState(GameState.Shop);
            OnPanelChanged?.Invoke(GameState.Shop);
        }

        /// <summary>
        /// 切换到库存界面
        /// </summary>
        public void ShowInventory()
        {
            ShowPanel(inventoryPanel);
            GameManager.Instance.ChangeState(GameState.Inventory);
            OnPanelChanged?.Invoke(GameState.Inventory);
        }

        /// <summary>
        /// 切换到顾客接待界面
        /// </summary>
        public void ShowCustomer()
        {
            ShowPanel(customerPanel);
            GameManager.Instance.ChangeState(GameState.Customer);
            OnPanelChanged?.Invoke(GameState.Customer);
        }

        /// <summary>
        /// 切换到维修界面
        /// </summary>
        public void ShowRepair()
        {
            ShowPanel(repairPanel);
            GameManager.Instance.ChangeState(GameState.Workshop);
            OnPanelChanged?.Invoke(GameState.Workshop);
        }

        /// <summary>
        /// 显示/隐藏暂停菜单
        /// </summary>
        public void TogglePause()
        {
            if (pausePanel.activeSelf)
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        /// <summary>
        /// 开始新游戏
        /// </summary>
        public void StartNewGame()
        {
            SaveSystem.DeleteSave();
            GameManager.Instance.PlayerData = new PlayerData();
            ShowShop();
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        public void ContinueGame()
        {
            ShowShop();
        }

        /// <summary>
        /// 保存并退出
        /// </summary>
        public void SaveAndQuit()
        {
            GameManager.Instance.SaveGame();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}