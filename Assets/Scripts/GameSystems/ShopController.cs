using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using BikeShopTycoon.Core;
using BikeShopTycoon.Data;
using BikeShopTycoon.UI;

namespace BikeShopTycoon.GameSystems
{
    /// <summary>
    /// 店铺主控制器 - 管理顾客生成、交互和交易流程
    /// </summary>
    public class ShopController : MonoBehaviour
    {
        [Header("顾客生成设置")]
        [SerializeField] private float minSpawnInterval = 8f;
        [SerializeField] private float maxSpawnInterval = 20f;
        [SerializeField] private int maxActiveCustomers = 3;

        [Header("UI 引用")]
        [SerializeField] private Transform customerContainer;
        [SerializeField] private GameObject customerCardPrefab;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private GameObject noCustomerText;

        [Header("交易面板")]
        [SerializeField] private GameObject transactionPanel;
        [SerializeField] private TextMeshProUGUI customerNameText;
        [SerializeField] private TextMeshProUGUI customerTypeText;
        [SerializeField] private TextMeshProUGUI budgetText;
        [SerializeField] private TextMeshProUGUI needsText;
        [SerializeField] private TextMeshProUGUI storyText;
        [SerializeField] private Button recommendButton;
        [SerializeField] private Button declineButton;
        [SerializeField] private Button closeButton;

        [Header("推荐面板")]
        [SerializeField] private GameObject recommendPanel;
        [SerializeField] private Transform productContainer;
        [SerializeField] private GameObject productCardPrefab;

        // 系统
        private CustomerManager customerManager;
        private InventoryPanel inventoryPanel;
        private InventoryData inventoryData;
        
        // 状态
        private Customer currentCustomer;
        private float nextSpawnTime;
        private bool isPaused = false;

        private void Awake()
        {
            // 获取库存面板引用
            inventoryPanel = FindObjectOfType<InventoryPanel>();
            if (inventoryPanel != null)
            {
                inventoryData = inventoryPanel.GetInventoryData();
            }
            
            // 如果没有库存面板，创建默认库存
            if (inventoryData == null)
            {
                inventoryData = new InventoryData();
                // 添加初始库存
                var products = InitialProducts.CreateDefaultProducts();
                foreach (var product in products)
                {
                    if (product.Tier == ItemTier.Entry)
                    {
                        inventoryData.AddItem(product, 3, product.PurchasePrice);
                    }
                }
            }

            // 初始化顾客管理系统
            if (GameManager.Instance != null && GameManager.Instance.PlayerData != null)
            {
                var settings = new CustomerGeneratorSettings
                {
                    StudentWeight = 30,
                    CommuterWeight = 35,
                    EnthusiastWeight = 20,
                    RacerWeight = 10,
                    InfluencerWeight = 5
                };
                
                customerManager = new CustomerManager(GameManager.Instance.PlayerData, settings);
            }

            // 绑定事件
            if (customerManager != null)
            {
                customerManager.OnCustomerEnter += OnCustomerEnter;
                customerManager.OnCustomerLeave += OnCustomerLeave;
                customerManager.OnTransactionComplete += OnTransactionComplete;
                customerManager.OnStoryRevealed += OnStoryRevealed;
            }

            // 绑定 UI 事件
            if (recommendButton != null)
                recommendButton.onClick.AddListener(OnRecommendClicked);
            if (declineButton != null)
                declineButton.onClick.AddListener(OnDeclineClicked);
            if (closeButton != null)
                closeButton.onClick.AddListener(CloseTransactionPanel);

            // 初始隐藏面板
            if (transactionPanel != null) transactionPanel.SetActive(false);
            if (recommendPanel != null) recommendPanel.SetActive(false);

            // 设置下次生成时间
            ScheduleNextCustomer();
        }

        private void OnDestroy()
        {
            if (customerManager != null)
            {
                customerManager.OnCustomerEnter -= OnCustomerEnter;
                customerManager.OnCustomerLeave -= OnCustomerLeave;
                customerManager.OnTransactionComplete -= OnTransactionComplete;
                customerManager.OnStoryRevealed -= OnStoryRevealed;
            }
        }

        private void Update()
        {
            if (isPaused) return;

            // 检查是否该生成新顾客
            if (Time.time >= nextSpawnTime && customerManager != null)
            {
                if (customerManager.ActiveCustomerCount < maxActiveCustomers)
                {
                    SpawnCustomer();
                }
                ScheduleNextCustomer();
            }

            // 更新状态文本
            UpdateStatusText();
        }

        /// <summary>
        /// 生成新顾客
        /// </summary>
        private void SpawnCustomer()
        {
            if (customerManager == null) return;
            
            var customer = customerManager.GenerateCustomer();
            Debug.Log($"[ShopController] 新顾客进店: {customer.Name} ({customer.Type}), 预算: ¥{customer.Budget}");
            
            // 显示通知
            if (HUDController.Instance != null)
            {
                HUDController.Instance.ShowNotification($"顾客 {customer.Name} 进店了！", NotificationType.Info);
            }
        }

        /// <summary>
        /// 安排下一个顾客
        /// </summary>
        private void ScheduleNextCustomer()
        {
            nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }

        #region 事件处理

        private void OnCustomerEnter(Customer customer)
        {
            UpdateCustomerDisplay();
        }

        private void OnCustomerLeave(Customer customer)
        {
            UpdateCustomerDisplay();
        }

        private void OnTransactionComplete(Customer customer, bool success)
        {
            if (success)
            {
                // 交易成功，增加资金和口碑
                int reputationGain = 5;
                GameManager.Instance?.AddReputation(reputationGain);
                
                if (HUDController.Instance != null)
                {
                    HUDController.Instance.ShowNotification($"交易成功！口碑 +{reputationGain}", NotificationType.Success);
                }
            }
            else
            {
                if (HUDController.Instance != null)
                {
                    HUDController.Instance.ShowNotification("顾客不满意离开了...", NotificationType.Warning);
                }
            }
        }

        private void OnStoryRevealed(CustomerStory story)
        {
            if (HUDController.Instance != null)
            {
                HUDController.Instance.ShowNotification($"📖 {story.Title}", NotificationType.Info);
            }
        }

        #endregion

        #region UI 更新

        private void UpdateStatusText()
        {
            if (statusText != null && customerManager != null)
            {
                int count = customerManager.ActiveCustomerCount;
                statusText.text = count > 0 
                    ? $"当前顾客: {count}/{maxActiveCustomers}" 
                    : "等待顾客光临...";
            }

            if (noCustomerText != null && customerManager != null)
            {
                noCustomerText.SetActive(customerManager.ActiveCustomerCount == 0);
            }
        }

        private void UpdateCustomerDisplay()
        {
            // 更新顾客列表显示
            // 简化版本：直接显示第一个等待的顾客
        }

        #endregion

        #region 交易交互

        /// <summary>
        /// 选择顾客进行交互
        /// </summary>
        public void SelectCustomer(Customer customer)
        {
            currentCustomer = customer;
            ShowTransactionPanel(customer);
        }

        private void ShowTransactionPanel(Customer customer)
        {
            if (transactionPanel == null) return;

            transactionPanel.SetActive(true);
            
            if (customerNameText != null)
                customerNameText.text = customer.Name;
            
            if (customerTypeText != null)
                customerTypeText.text = GetCustomerTypeLabel(customer.Type);
            
            if (budgetText != null)
                budgetText.text = $"预算: ¥{customer.Budget:N0}";
            
            if (needsText != null)
            {
                string needs = GetNeedsDescription(customer);
                needsText.text = needs;
            }

            if (storyText != null)
            {
                storyText.text = !string.IsNullOrEmpty(customer.StoryId) 
                    ? "📝 这位顾客似乎有故事..." 
                    : "";
            }
        }

        private void CloseTransactionPanel()
        {
            if (transactionPanel != null)
                transactionPanel.SetActive(false);
            if (recommendPanel != null)
                recommendPanel.SetActive(false);
            currentCustomer = null;
        }

        private void OnRecommendClicked()
        {
            // 显示推荐面板
            if (recommendPanel != null)
            {
                recommendPanel.SetActive(true);
                ShowAvailableProducts();
            }
        }

        private void OnDeclineClicked()
        {
            if (currentCustomer != null && customerManager != null)
            {
                customerManager.CustomerLeaveUnsatisfied(currentCustomer);
            }
            CloseTransactionPanel();
        }

        private void ShowAvailableProducts()
        {
            // 显示可销售的商品列表
            // 简化版本：显示几条示例商品
        }

        /// <summary>
        /// 推荐商品给当前顾客
        /// </summary>
        public void RecommendProduct(Item item)
        {
            if (currentCustomer == null || customerManager == null) return;

            var result = customerManager.RecommendItem(currentCustomer, item);
            
            if (result.MatchScore >= 80)
            {
                // 高匹配度，直接成交
                CompleteTransaction(item);
            }
            else if (result.MatchScore >= 50)
            {
                // 中等匹配度，询问是否成交
                // 简化：直接成交
                CompleteTransaction(item);
            }
            else
            {
                // 低匹配度
                if (HUDController.Instance != null)
                {
                    HUDController.Instance.ShowNotification(result.Feedback, NotificationType.Warning);
                }
            }
        }

        private void CompleteTransaction(Item item)
        {
            if (currentCustomer == null || inventoryData == null) return;

            // 检查库存
            var invItem = inventoryData.GetItem(item.Id);
            if (invItem == null || invItem.Quantity < 1)
            {
                if (HUDController.Instance != null)
                {
                    HUDController.Instance.ShowNotification("库存不足！", NotificationType.Error);
                }
                return;
            }

            // 扣除库存
            inventoryData.RemoveItem(item.Id, 1);
            
            // 增加金钱
            GameManager.Instance?.AddMoney(item.SellPrice);
            
            // 完成交易
            currentCustomer.State = CustomerState.Purchasing;
            customerManager.CompleteTransaction(currentCustomer, item);
            
            CloseTransactionPanel();
            
            Debug.Log($"[ShopController] 交易完成: {item.Name}, 售价: ¥{item.SellPrice}");
        }

        #endregion

        #region 辅助方法

        private string GetCustomerTypeLabel(CustomerType type)
        {
            return type switch
            {
                CustomerType.Student => "🎓 学生",
                CustomerType.Commuter => "🚴 通勤族",
                CustomerType.CyclingEnthusiast => "💪 骑行爱好者",
                CustomerType.Racer => "🏆 竞赛车手",
                CustomerType.Influencer => "📱 网红",
                _ => "顾客"
            };
        }

        private string GetNeedsDescription(Customer customer)
        {
            var needs = new System.Collections.Generic.List<string>();
            
            if (customer.Needs.NeedForRacing) needs.Add("竞赛用车");
            if (customer.Needs.NeedForCommuting) needs.Add("通勤代步");
            if (customer.Needs.NeedForTraining) needs.Add("训练健身");
            if (customer.Needs.NeedForBeginners) needs.Add("新手入门");
            if (customer.Needs.NeedHighVisual) needs.Add("高颜值");
            
            return needs.Count > 0 ? "需求: " + string.Join(", ", needs) : "暂无明确需求";
        }

        /// <summary>
        /// 暂停/恢复顾客生成
        /// </summary>
        public void SetPaused(bool paused)
        {
            isPaused = paused;
        }

        #endregion
    }
}