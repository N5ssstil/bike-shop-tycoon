using UnityEngine;
using System.Collections.Generic;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// 维修界面控制器
    /// 负责处理基础维修操作（爆胎/变速/刹车）和改装服务
    /// </summary>
    public class WorkshopUI : MonoBehaviour
    {
        [Header("UI 元素引用")]
        public GameObject repairPanel;            // 维修面板
        public Transform serviceListContainer;    // 服务列表容器
        public GameObject serviceItemPrefab;      // 服务项预制体
        public TMPro.TextMeshProUGUI totalCostText; // 总费用显示
        
        [Header("工具按钮")]
        public GameObject tireToolButton;        // 轮胎工具
        public GameObject gearToolButton;         // 变速工具
        public GameObject brakeToolButton;        // 刹车工具
        public GameObject confirmRepairButton;    // 确认维修按钮

        private List<RepairServiceData> availableServices = new List<RepairServiceData>();
        private List<RepairServiceData> selectedServices = new List<RepairServiceData>();
        private Dictionary<string, GameObject> toolButtons = new Dictionary<string, GameObject>();

        private void Start()
        {
            // 初始化可用服务
            InitializeAvailableServices();
            
            // 初始化工具按钮映射
            InitializeToolButtons();
            
            // 刷新服务列表
            RefreshServiceList();
            
            // 订阅资金变化事件
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnMoneyChanged += OnMoneyChanged;
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnMoneyChanged -= OnMoneyChanged;
            }
        }

        /// <summary>
        /// 初始化可用维修服务
        /// </summary>
        private void InitializeAvailableServices()
        {
            availableServices.Clear();
            availableServices.Add(new RepairServiceData 
            { 
                Name = "爆胎更换", 
                Description = "更换内胎和外胎", 
                Cost = 150, 
                RequiredTool = "TireTool",
                ServiceType = RepairServiceType.Tire
            });
            availableServices.Add(new RepairServiceData 
            { 
                Name = "变速调试", 
                Description = "调整前后变速器", 
                Cost = 200, 
                RequiredTool = "GearTool",
                ServiceType = RepairServiceType.Gear
            });
            availableServices.Add(new RepairServiceData 
            { 
                Name = "刹车保养", 
                Description = "更换刹车片和调试", 
                Cost = 180, 
                RequiredTool = "BrakeTool",
                ServiceType = RepairServiceType.Brake
            });
            availableServices.Add(new RepairServiceData 
            { 
                Name = "基础保养套餐", 
                Description = "包含轮胎、变速、刹车全面检查", 
                Cost = 450, 
                RequiredTool = "AllTools",
                ServiceType = RepairServiceType.Full
            });
        }

        /// <summary>
        /// 初始化工具按钮映射
        /// </summary>
        private void InitializeToolButtons()
        {
            toolButtons.Clear();
            toolButtons.Add("TireTool", tireToolButton);
            toolButtons.Add("GearTool", gearToolButton);
            toolButtons.Add("BrakeTool", brakeToolButton);
        }

        /// <summary>
        /// 刷新服务列表显示
        /// </summary>
        private void RefreshServiceList()
        {
            // 清空现有列表
            foreach (Transform child in serviceListContainer)
            {
                Destroy(child.gameObject);
            }
            
            // 生成服务项
            foreach (var service in availableServices)
            {
                if (serviceItemPrefab != null)
                {
                    GameObject itemObj = Instantiate(serviceItemPrefab, serviceListContainer);
                    // TODO: 设置服务项的 UI 内容（名称、描述、价格）
                    // 示例：itemObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"{service.Name} - {service.Cost:N0}元";
                    
                    // 添加点击事件（选择/取消选择服务）
                    var button = itemObj.GetComponent<UnityEngine.UI.Button>();
                    if (button != null)
                    {
                        string serviceName = service.Name;
                        button.onClick.AddListener(() => ToggleServiceSelection(serviceName));
                    }
                }
            }
            
            UpdateTotalCost();
        }

        /// <summary>
        /// 切换服务选择状态
        /// </summary>
        private void ToggleServiceSelection(string serviceName)
        {
            var service = availableServices.Find(s => s.Name == serviceName);
            if (service == null) return;
            
            if (selectedServices.Contains(service))
            {
                selectedServices.Remove(service);
                // 取消高亮
                // TODO: 更新 UI 选中状态
            }
            else
            {
                selectedServices.Add(service);
                // 高亮选中
                // TODO: 更新 UI 选中状态
            }
            
            UpdateTotalCost();
            UpdateToolButtons();
        }

        /// <summary>
        /// 更新总费用显示
        /// </summary>
        private void UpdateTotalCost()
        {
            int totalCost = 0;
            foreach (var service in selectedServices)
            {
                totalCost += service.Cost;
            }
            
            if (totalCostText != null)
            {
                totalCostText.text = $"总费用: {totalCost:N0} 元";
            }
        }

        /// <summary>
        /// 更新工具按钮状态
        /// </summary>
        private void UpdateToolButtons()
        {
            // 重置所有工具按钮状态
            foreach (var button in toolButtons.Values)
            {
                if (button != null)
                {
                    var image = button.GetComponent<UnityEngine.UI.Image>();
                    if (image != null)
                    {
                        image.color = Color.white; // 默认颜色
                    }
                }
            }
            
            // 高亮需要的工具
            foreach (var service in selectedServices)
            {
                if (service.RequiredTool == "AllTools")
                {
                    // 高亮所有工具
                    foreach (var button in toolButtons.Values)
                    {
                        HighlightToolButton(button);
                    }
                    break;
                }
                else if (toolButtons.ContainsKey(service.RequiredTool))
                {
                    HighlightToolButton(toolButtons[service.RequiredTool]);
                }
            }
        }

        /// <summary>
        /// 高亮工具按钮
        /// </summary>
        private void HighlightToolButton(GameObject button)
        {
            if (button != null)
            {
                var image = button.GetComponent<UnityEngine.UI.Image>();
                if (image != null)
                {
                    image.color = Color.yellow; // 高亮颜色
                }
            }
        }

        /// <summary>
        /// 执行维修操作
        /// </summary>
        public void OnConfirmRepair()
        {
            if (selectedServices.Count == 0)
            {
                Debug.LogWarning("请至少选择一项维修服务");
                return;
            }
            
            // 计算总费用
            int totalCost = 0;
            foreach (var service in selectedServices)
            {
                totalCost += service.Cost;
            }
            
            // 检查资金是否足够
            if (!GameManager.Instance.TrySpendMoney(totalCost))
            {
                Debug.LogWarning("资金不足，无法完成维修");
                return;
            }
            
            // 执行维修逻辑
            ExecuteRepairServices();
            
            // 提升口碑
            int reputationGain = CalculateReputationGain(selectedServices);
            GameManager.Instance.AddReputation(reputationGain);
            
            // 显示成功提示
            Debug.Log($"成功完成 {selectedServices.Count} 项维修服务！");
            
            // 重置选择
            selectedServices.Clear();
            RefreshServiceList();
            UpdateToolButtons();
            
            // 保存游戏
            GameManager.Instance.SaveGame();
        }

        /// <summary>
        /// 执行具体维修服务
        /// </summary>
        private void ExecuteRepairServices()
        {
            // TODO: 实际维修逻辑（如播放动画、更新自行车状态等）
            foreach (var service in selectedServices)
            {
                switch (service.ServiceType)
                {
                    case RepairServiceType.Tire:
                        // 执行爆胎更换逻辑
                        break;
                    case RepairServiceType.Gear:
                        // 执行变速调试逻辑
                        break;
                    case RepairServiceType.Brake:
                        // 执行刹车保养逻辑
                        break;
                    case RepairServiceType.Full:
                        // 执行全面保养逻辑
                        break;
                }
            }
        }

        /// <summary>
        /// 计算口碑增益
        /// </summary>
        private int CalculateReputationGain(List<RepairServiceData> services)
        {
            // 基础口碑增益
            int baseGain = services.Count * 15;
            
            // 如果包含全面保养，额外奖励
            if (services.Exists(s => s.ServiceType == RepairServiceType.Full))
            {
                baseGain += 20;
            }
            
            return baseGain;
        }

        /// <summary>
        /// 资金变化回调
        /// </summary>
        private void OnMoneyChanged(int newMoney)
        {
            // 更新维修能力状态
            bool canAfford = true;
            int totalCost = 0;
            foreach (var service in selectedServices)
            {
                totalCost += service.Cost;
            }
            canAfford = GameManager.Instance.CanAfford(totalCost);
            
            // TODO: 更新确认按钮状态（禁用/启用）
        }

        /// <summary>
        /// 返回店铺主界面
        /// </summary>
        public void OnReturnToShop()
        {
            // 切换回店铺经营状态
            GameManager.Instance.ChangeState(GameState.Shop);
            // TODO: 隐藏当前维修界面，显示店铺主界面
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 维修服务数据结构
    /// </summary>
    [System.Serializable]
    public class RepairServiceData
    {
        public string Name;
        public string Description;
        public int Cost;
        public string RequiredTool;
        public RepairServiceType ServiceType;
    }

    /// <summary>
    /// 维修服务类型枚举
    /// </summary>
    public enum RepairServiceType
    {
        Tire,   // 轮胎
        Gear,   // 变速
        Brake,  // 刹车
        Full    // 全面保养
    }
}