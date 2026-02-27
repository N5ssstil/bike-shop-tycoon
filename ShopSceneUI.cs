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
        
        [Header("资金和口碑显示")]
        public TMPro.TextMeshProUGUI moneyText;     // 资金显示
        public TMPro.TextMeshProUGUI reputationText; // 口碑显示

        private void Start()
        {
            // 初始化 UI 状态
            UpdateMoneyDisplay();
            UpdateReputationDisplay();
            
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
        /// 打开库存管理界面
        /// </summary>
        public void OnOpenInventory()
        {
            GameManager.Instance.ChangeState(GameState.Inventory);
            // TODO: 显示库存管理 UI 面板
            Debug.Log("打开库存管理");
        }

        /// <summary>
        /// 打开顾客接待界面
        /// </summary>
        public void OnOpenCustomer()
        {
            GameManager.Instance.ChangeState(GameState.Customer);
            // TODO: 显示顾客接待 UI 面板
            Debug.Log("打开顾客接待");
        }

        /// <summary>
        /// 打开维修/改装界面
        /// </summary>
        public void OnOpenWorkshop()
        {
            GameManager.Instance.ChangeState(GameState.Workshop);
            // TODO: 显示维修/改装 UI 面板
            Debug.Log("打开维修/改装");
        }

        /// <summary>
        /// 打开活动/赛事界面
        /// </summary>
        public void OnOpenEvents()
        {
            GameManager.Instance.ChangeState(GameState.Events);
            // TODO: 显示活动/赛事 UI 面板
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