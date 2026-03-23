using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BikeShopTycoon.Core;
using BikeShopTycoon.Data;
using BikeShopTycoon.GameSystems;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// 顾客详情面板 - 显示顾客信息和交互选项
    /// </summary>
    public class CustomerDetailPanel : MonoBehaviour
    {
        [Header("面板引用")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Button closeButton;

        [Header("顾客信息")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI typeText;
        [SerializeField] private Image avatarImage;
        [SerializeField] private TextMeshProUGUI budgetText;
        [SerializeField] private TextMeshProUGUI satisfactionText;

        [Header("需求显示")]
        [SerializeField] private Transform needsContainer;
        [SerializeField] private GameObject needItemPrefab;

        [Header("故事")]
        [SerializeField] private GameObject storySection;
        [SerializeField] private TextMeshProUGUI storyTitleText;
        [SerializeField] private TextMeshProUGUI storyDescText;
        [SerializeField] private Button revealStoryButton;

        [Header("操作按钮")]
        [SerializeField] private Button sellButton;
        [SerializeField] private Button repairButton;
        [SerializeField] private Button dismissButton;

        // 当前顾客
        private Customer currentCustomer;
        private ShopController shopController;

        // 事件
        public event System.Action<Customer> OnSellRequested;
        public event System.Action<Customer> OnRepairRequested;
        public event System.Action<Customer> OnDismissed;

        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(ClosePanel);
            if (sellButton != null)
                sellButton.onClick.AddListener(OnSellClicked);
            if (repairButton != null)
                repairButton.onClick.AddListener(OnRepairClicked);
            if (dismissButton != null)
                dismissButton.onClick.AddListener(OnDismissClicked);
            if (revealStoryButton != null)
                revealStoryButton.onClick.AddListener(OnRevealStory);

            if (panel != null)
                panel.SetActive(false);

            shopController = FindObjectOfType<ShopController>();
        }

        /// <summary>
        /// 显示顾客详情
        /// </summary>
        public void ShowCustomer(Customer customer)
        {
            if (customer == null) return;

            currentCustomer = customer;
            UpdateDisplay();
            
            if (panel != null)
                panel.SetActive(true);
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void ClosePanel()
        {
            if (panel != null)
                panel.SetActive(false);
            currentCustomer = null;
        }

        private void UpdateDisplay()
        {
            if (currentCustomer == null) return;

            // 基本信息
            if (nameText != null)
                nameText.text = currentCustomer.Name;

            if (typeText != null)
                typeText.text = GetCustomerTypeDisplay(currentCustomer.Type);

            if (budgetText != null)
                budgetText.text = $"预算: ¥{currentCustomer.Budget:N0}";

            if (satisfactionText != null)
            {
                satisfactionText.text = $"满意度: {currentCustomer.Satisfaction}%";
                // 根据满意度设置颜色
                satisfactionText.color = currentCustomer.Satisfaction switch
                {
                    >= 80 => new Color(0.2f, 0.8f, 0.2f),
                    >= 50 => new Color(0.9f, 0.7f, 0.2f),
                    _ => new Color(0.9f, 0.3f, 0.3f)
                };
            }

            // 需求列表
            UpdateNeedsDisplay();

            // 故事部分
            UpdateStoryDisplay();

            // 按钮状态
            UpdateButtonStates();
        }

        private void UpdateNeedsDisplay()
        {
            if (needsContainer == null) return;

            // 清空现有内容
            foreach (Transform child in needsContainer)
            {
                Destroy(child.gameObject);
            }

            if (currentCustomer?.Needs == null) return;

            // 显示需求
            var needs = currentCustomer.Needs;

            if (needs.NeedForRacing)
                AddNeedItem("🏁 竞赛用车");
            if (needs.NeedForCommuting)
                AddNeedItem("🚲 通勤代步");
            if (needs.NeedForTraining)
                AddNeedItem("💪 训练健身");
            if (needs.NeedForBeginners)
                AddNeedItem("🎓 新手入门");
            if (needs.NeedHighVisual)
                AddNeedItem("✨ 高颜值");
            if (!string.IsNullOrEmpty(needs.PreferredBrand))
                AddNeedItem($"🏷️ 偏好品牌: {needs.PreferredBrand}");
            if (!string.IsNullOrEmpty(needs.PreferredColor))
                AddNeedItem($"🎨 偏好颜色: {needs.PreferredColor}");
        }

        private void AddNeedItem(string text)
        {
            if (needItemPrefab == null || needsContainer == null) return;

            var item = Instantiate(needItemPrefab, needsContainer);
            var tmp = item.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = text;
        }

        private void UpdateStoryDisplay()
        {
            if (storySection == null) return;

            bool hasStory = !string.IsNullOrEmpty(currentCustomer?.StoryId);
            storySection.SetActive(hasStory);

            if (!hasStory) return;

            // 如果故事已揭示，显示内容
            if (currentCustomer.StoryRevealed)
            {
                var story = GetStoryById(currentCustomer.StoryId);
                if (story != null)
                {
                    if (storyTitleText != null)
                        storyTitleText.text = $"📖 {story.Title}";
                    if (storyDescText != null)
                        storyDescText.text = story.Description;
                }
                if (revealStoryButton != null)
                    revealStoryButton.gameObject.SetActive(false);
            }
            else
            {
                if (storyTitleText != null)
                    storyTitleText.text = "📝 这位顾客似乎有故事...";
                if (storyDescText != null)
                    storyDescText.text = "完成交易后可能揭示";
                if (revealStoryButton != null)
                    revealStoryButton.gameObject.SetActive(false); // 交易后自动揭示
            }
        }

        private void UpdateButtonStates()
        {
            if (sellButton != null)
                sellButton.interactable = currentCustomer != null;

            if (repairButton != null)
                repairButton.interactable = currentCustomer?.Needs?.RepairNeed != null;
        }

        #region 按钮事件

        private void OnSellClicked()
        {
            if (currentCustomer == null) return;

            currentCustomer.CurrentNeed = NeedType.BuyBike;
            OnSellRequested?.Invoke(currentCustomer);
            ClosePanel();
        }

        private void OnRepairClicked()
        {
            if (currentCustomer == null) return;

            currentCustomer.CurrentNeed = NeedType.Repair;
            OnRepairRequested?.Invoke(currentCustomer);
            ClosePanel();
        }

        private void OnDismissClicked()
        {
            if (currentCustomer == null) return;

            OnDismissed?.Invoke(currentCustomer);
            ClosePanel();
        }

        private void OnRevealStory()
        {
            if (currentCustomer == null || string.IsNullOrEmpty(currentCustomer.StoryId)) return;

            currentCustomer.StoryRevealed = true;
            UpdateStoryDisplay();
        }

        #endregion

        #region 辅助方法

        private string GetCustomerTypeDisplay(CustomerType type)
        {
            return type switch
            {
                CustomerType.Student => "🎓 学生党",
                CustomerType.Commuter => "🚴 通勤族",
                CustomerType.CyclingEnthusiast => "💪 骑行爱好者",
                CustomerType.Racer => "🏆 竞赛车手",
                CustomerType.Influencer => "📱 网红博主",
                _ => "顾客"
            };
        }

        private CustomerStory GetStoryById(string storyId)
        {
            // 简化版本，实际应从数据库获取
            return new CustomerStory
            {
                Id = storyId,
                Title = "特殊顾客",
                Description = "这位顾客有着不寻常的故事..."
            };
        }

        #endregion
    }
}