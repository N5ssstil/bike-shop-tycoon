using UnityEngine;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// 店铺场景 UI 控制器
    /// 负责处理店铺经营界面的交互
    /// </summary>
    public class ShopSceneUI : MonoBehaviour
    {
        [Header("UI 元素引用")]
        public GameObject inventoryButton;   // 库存管理按钮
        public GameObject customerButton;    // 顾客接待按钮
        public GameObject workshopButton;    // 维修/改装按钮
        public GameObject eventsButton;      // 活动/赛事按钮

        [Header("面板引用")]
        public GameObject inventoryPanel;    // 库存管理面板
        public GameObject customerPanel;     // 顾客接待面板
        public GameObject workshopPanel;     // 维修/改装面板
        public GameObject eventPanel;        // 活动/赛事面板

        [Header("资金和口碑显示")]
        public TMPro.TextMeshProUGUI moneyText;     // 资金显示
        public TMPro.TextMeshProUGUI reputationText; // 口碑显示

        private List<GameObject> allPanels = new List<GameObject>();

        private void Start()
        {
            // 初始化 UI 状态
            UpdateMoneyDisplay();
            UpdateReputationDisplay();

            // 收集所有面板
            if (inventoryPanel != null) allPanels.Add(inventoryPanel);
            if (customerPanel != null) allPanels.Add(customerPanel);
            if (workshopPanel != null) allPanels.Add(workshopPanel);
            if (eventPanel != null) allPanels.Add(eventPanel);

            // 默认只显示店铺主界面，隐藏所有面板
            HideAllPanels();

            // 订阅 GameManager 事件
            GameManager.Instance.OnMoneyChanged += UpdateMoneyDisplay;
            GameManager.Instance.OnReputationChanged += UpdateReputationDisplay;

            // 默认激活店铺经营状态
            GameManager.Instance.ChangeState(GameState.Shop);
        }

        private void OnDestroy()
        {
            // 取消事件订阅
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnMoneyChanged -= UpdateMoneyDisplay;
                GameManager.Instance.OnReputationChanged -= UpdateReputationDisplay;
            }
        }

        /// <summary>
        /// 隐藏所有子面板
        /// </summary>
        private void HideAllPanels()
        {
            foreach (var panel in allPanels)
            {
                if (panel != null) panel.SetActive(false);
            }
        }

        /// <summary>
        /// 打开库存管理界面
        /// </summary>
        public void OnOpenInventory()
        {
            GameManager.Instance.ChangeState(GameState.Inventory);
            HideAllPanels();
            if (inventoryPanel != null) inventoryPanel.SetActive(true);
            Debug.Log("打开库存管理");
        }

        /// <summary>
        /// 打开顾客接待界面
        /// </summary>
        public void OnOpenCustomer()
        {
            GameManager.Instance.ChangeState(GameState.Customer);
            HideAllPanels();
            if (customerPanel != null) customerPanel.SetActive(true);
            Debug.Log("打开顾客接待");
        }

        /// <summary>
        /// 打开维修/改装界面
        /// </summary>
        public void OnOpenWorkshop()
        {
            GameManager.Instance.ChangeState(GameState.Workshop);
            HideAllPanels();
            if (workshopPanel != null) workshopPanel.SetActive(true);
            Debug.Log("打开维修/改装");
        }

        /// <summary>
        /// 打开活动/赛事界面
        /// </summary>
        public void OnOpenEvents()
        {
            GameManager.Instance.ChangeState(GameState.Events);
            HideAllPanels();
            if (eventPanel != null) eventPanel.SetActive(true);
            Debug.Log("打开活动/赛事");
        }

        /// <summary>
        /// 返回主菜单
        /// </summary>
        public void OnReturnToMainMenu()
        {
            // 保存游戏
            GameManager.Instance.SaveGame();
            // 切换到主菜单场景（假设场景名为 "MainMenu"）
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        /// <summary>
        /// 更新资金显示
        /// </summary>
        private void UpdateMoneyDisplay()
        {
            if (moneyText != null)
            {
                moneyText.text = $"资金: {GameManager.Instance.PlayerData.Money:N0} 元";
            }
        }

        /// <summary>
        /// 更新口碑显示
        /// </summary>
        private void UpdateReputationDisplay()
        {
            if (reputationText != null)
            {
                reputationText.text = $"口碑: {GameManager.Instance.PlayerData.Reputation}/1000";
            }
        }
    }
}