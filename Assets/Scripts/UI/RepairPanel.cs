using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using BikeShopTycoon.Core;
using BikeShopTycoon.Data;
using BikeShopTycoon.GameSystems;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// 维修服务面板控制器
    /// </summary>
    public class RepairPanel : MonoBehaviour
    {
        [Header("UI 引用")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform jobListContainer;
        [SerializeField] private GameObject jobCardPrefab;
        [SerializeField] private TextMeshProUGUI totalJobsText;
        [SerializeField] private TextMeshProUGUI todayIncomeText;

        [Header("新建工单")]
        [SerializeField] private GameObject newJobPanel;
        [SerializeField] private TMP_Dropdown repairTypeDropdown;
        [SerializeField] private TextMeshProUGUI costPreviewText;
        [SerializeField] private TextMeshProUGUI timePreviewText;
        [SerializeField] private TextMeshProUGUI toolsPreviewText;
        [SerializeField] private Button createJobButton;
        [SerializeField] private Button cancelNewJobButton;

        [Header("按钮")]
        [SerializeField] private Button openRepairButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button newJobButton;

        // 系统
        private RepairService repairService;
        private List<RepairJob> activeJobs = new List<RepairJob>();
        private List<RepairJob> completedJobs = new List<RepairJob>();
        private int todayIncome = 0;

        // 当前等待维修的顾客
        private Customer waitingCustomer;

        private void Awake()
        {
            // 绑定按钮事件
            if (openRepairButton != null)
                openRepairButton.onClick.AddListener(OpenPanel);
            if (closeButton != null)
                closeButton.onClick.AddListener(ClosePanel);
            if (newJobButton != null)
                newJobButton.onClick.AddListener(OpenNewJobPanel);
            if (createJobButton != null)
                createJobButton.onClick.AddListener(CreateRepairJob);
            if (cancelNewJobButton != null)
                cancelNewJobButton.onClick.AddListener(CloseNewJobPanel);

            // 初始隐藏
            if (panel != null) panel.SetActive(false);
            if (newJobPanel != null) newJobPanel.SetActive(false);

            // 设置下拉菜单选项
            SetupRepairTypeDropdown();
        }

        private void Start()
        {
            if (GameManager.Instance != null && GameManager.Instance.PlayerData != null)
            {
                repairService = new RepairService(GameManager.Instance.PlayerData);
                repairService.OnRepairComplete += OnRepairComplete;
            }
        }

        private void OnDestroy()
        {
            if (repairService != null)
            {
                repairService.OnRepairComplete -= OnRepairComplete;
            }
        }

        #region 面板控制

        public void OpenPanel()
        {
            if (panel != null)
            {
                panel.SetActive(true);
                RefreshJobDisplay();
            }
        }

        public void ClosePanel()
        {
            if (panel != null)
                panel.SetActive(false);
        }

        public void OpenNewJobPanel()
        {
            if (newJobPanel != null)
            {
                newJobPanel.SetActive(true);
                UpdateRepairPreview();
            }
        }

        public void CloseNewJobPanel()
        {
            if (newJobPanel != null)
                newJobPanel.SetActive(false);
        }

        #endregion

        #region 工单管理

        private void SetupRepairTypeDropdown()
        {
            if (repairTypeDropdown == null) return;

            repairTypeDropdown.ClearOptions();
            var options = new List<string>
            {
                "爆胎修补 - ¥50",
                "变速调试 - ¥80",
                "刹车保养 - ¥60",
                "编圈调整 - ¥150",
                "全车保养 - ¥300",
                "定制调校 - ¥500"
            };
            repairTypeDropdown.AddOptions(options);
            repairTypeDropdown.onValueChanged.AddListener(OnRepairTypeChanged);
        }

        private void OnRepairTypeChanged(int index)
        {
            UpdateRepairPreview();
        }

        private void UpdateRepairPreview()
        {
            if (repairTypeDropdown == null || repairService == null) return;

            var repairType = (RepairType)repairTypeDropdown.value;
            
            if (costPreviewText != null)
                costPreviewText.text = $"费用: ¥{repairService.GetRepairCost(repairType)}";
            
            if (timePreviewText != null)
                timePreviewText.text = $"预计时间: {repairService.GetRepairTime(repairType)} 分钟";
            
            if (toolsPreviewText != null)
            {
                var tools = repairService.GetRequiredTools(repairType);
                toolsPreviewText.text = $"所需工具: {string.Join(", ", tools)}";
            }
        }

        /// <summary>
        /// 接收维修顾客（由 ShopController 调用）
        /// </summary>
        public void ReceiveRepairCustomer(Customer customer, RepairType repairType)
        {
            waitingCustomer = customer;
            CreateRepairJobInternal(repairType, customer);
        }

        private void CreateRepairJob()
        {
            if (repairService == null) return;

            var repairType = (RepairType)repairTypeDropdown.value;
            
            // 创建模拟顾客（如果没有人等待）
            var customer = waitingCustomer ?? new Customer
            {
                Name = "维修顾客",
                Type = CustomerType.Commuter
            };

            CreateRepairJobInternal(repairType, customer);
            
            waitingCustomer = null;
            CloseNewJobPanel();
            RefreshJobDisplay();
            
            HUDController.Instance?.ShowNotification("维修工单已创建！", NotificationType.Success);
        }

        private void CreateRepairJobInternal(RepairType repairType, Customer customer)
        {
            var job = repairService.CreateJob(repairType, customer);
            activeJobs.Add(job);
        }

        /// <summary>
        /// 完成维修工单
        /// </summary>
        public void CompleteJob(string jobId)
        {
            var job = activeJobs.Find(j => j.Id == jobId);
            if (job == null || repairService == null) return;

            var result = repairService.ExecuteRepair(job);
            
            if (result.Success)
            {
                activeJobs.Remove(job);
                completedJobs.Add(job);
                todayIncome += result.Income;
                
                // 通过 GameManager 增加金钱
                GameManager.Instance?.AddMoney(result.Income);
                GameManager.Instance?.AddReputation(result.ReputationGain);
                
                HUDController.Instance?.ShowNotification(
                    $"维修完成！收入 ¥{result.Income}，口碑 +{result.ReputationGain}", 
                    NotificationType.Success
                );
                
                RefreshJobDisplay();
            }
        }

        /// <summary>
        /// 取消工单
        /// </summary>
        public void CancelJob(string jobId)
        {
            var job = activeJobs.Find(j => j.Id == jobId);
            if (job != null)
            {
                job.Status = RepairStatus.Cancelled;
                activeJobs.Remove(job);
                RefreshJobDisplay();
            }
        }

        #endregion

        #region 显示更新

        private void RefreshJobDisplay()
        {
            // 更新统计
            if (totalJobsText != null)
                totalJobsText.text = $"进行中: {activeJobs.Count}";
            
            if (todayIncomeText != null)
                todayIncomeText.text = $"今日收入: ¥{todayIncome}";

            // 清空列表
            if (jobListContainer != null)
            {
                foreach (Transform child in jobListContainer)
                {
                    Destroy(child.gameObject);
                }
            }

            // 显示活动工单
            if (jobCardPrefab != null && jobListContainer != null)
            {
                foreach (var job in activeJobs)
                {
                    var card = Instantiate(jobCardPrefab, jobListContainer);
                    SetupJobCard(card, job);
                }

                // 显示空状态
                if (activeJobs.Count == 0)
                {
                    var emptyText = new GameObject("EmptyText");
                    emptyText.transform.SetParent(jobListContainer);
                    var tmp = emptyText.AddComponent<TextMeshProUGUI>();
                    tmp.text = "暂无进行中的维修工单";
                    tmp.alignment = TextAlignmentOptions.Center;
                    tmp.color = new Color(0.6f, 0.6f, 0.6f);
                }
            }
        }

        private void SetupJobCard(GameObject card, RepairJob job)
        {
            var typeText = card.transform.Find("TypeText")?.GetComponent<TextMeshProUGUI>();
            var customerText = card.transform.Find("CustomerText")?.GetComponent<TextMeshProUGUI>();
            var costText = card.transform.Find("CostText")?.GetComponent<TextMeshProUGUI>();
            var statusText = card.transform.Find("StatusText")?.GetComponent<TextMeshProUGUI>();
            var completeButton = card.transform.Find("CompleteButton")?.GetComponent<Button>();
            var cancelButton = card.transform.Find("CancelButton")?.GetComponent<Button>();

            if (typeText != null)
                typeText.text = GetRepairTypeLabel(job.Type);
            
            if (customerText != null)
                customerText.text = job.Customer?.Name ?? "匿名顾客";
            
            if (costText != null)
                costText.text = $"¥{job.BaseCost}";
            
            if (statusText != null)
            {
                statusText.text = job.Status switch
                {
                    RepairStatus.Pending => "等待中",
                    RepairStatus.InProgress => "进行中",
                    RepairStatus.Completed => "已完成",
                    RepairStatus.Cancelled => "已取消",
                    _ => ""
                };
            }

            // 绑定完成按钮
            if (completeButton != null)
            {
                completeButton.onClick.RemoveAllListeners();
                completeButton.onClick.AddListener(() => CompleteJob(job.Id));
            }

            // 绑定取消按钮
            if (cancelButton != null)
            {
                cancelButton.onClick.RemoveAllListeners();
                cancelButton.onClick.AddListener(() => CancelJob(job.Id));
            }
        }

        #endregion

        #region 事件处理

        private void OnRepairComplete(RepairJob job)
        {
            Debug.Log($"[RepairPanel] 维修完成: {job.Type}, 收入: ¥{job.BaseCost}");
        }

        #endregion

        #region 辅助方法

        private string GetRepairTypeLabel(RepairType type)
        {
            return type switch
            {
                RepairType.FlatTire => "🔧 爆胎修补",
                RepairType.GearAdjustment => "⚙️ 变速调试",
                RepairType.BrakeService => "🛑 刹车保养",
                RepairType.WheelTruing => "⭕ 编圈调整",
                RepairType.FullService => "🔩 全车保养",
                RepairType.CustomTuning => "🎯 定制调校",
                _ => "维修"
            };
        }

        #endregion
    }
}