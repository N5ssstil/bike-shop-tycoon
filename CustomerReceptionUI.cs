using UnityEngine;
using System.Collections.Generic;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// 顾客接待界面控制器
    /// 负责处理三类顾客（学生/通勤族/骑行爱好者）的接待流程
    /// </summary>
    public class CustomerReceptionUI : MonoBehaviour
    {
        [Header("UI 元素引用")]
        public GameObject customerPanel;         // 顾客面板
        public TMPro.TextMeshProUGUI customerNameText;   // 顾客名称
        public TMPro.TextMeshProUGUI customerTypeText;   // 顾客类型
        public TMPro.TextMeshProUGUI customerDemandText; // 需求描述
        public Transform productRecommendationList; // 推荐商品列表容器
        public GameObject productItemPrefab;     // 商品项预制体
        
        [Header("交互按钮")]
        public GameObject sellButton;           // 销售按钮
        public GameObject repairButton;          // 维修按钮
        public GameObject closeButton;           // 关闭按钮

        private CustomerData currentCustomer;
        private List<ProductData> recommendedProducts = new List<ProductData>();

        private void Start()
        {
            // 初始化时生成随机顾客
            GenerateRandomCustomer();
            
            // 订阅口碑变化事件
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnReputationChanged += OnReputationChanged;
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnReputationChanged -= OnReputationChanged;
            }
        }

        /// <summary>
        /// 生成随机顾客（示例逻辑）
        /// </summary>
        private void GenerateRandomCustomer()
        {
            // 实际应从顾客配置数据加载
            string[] names = { "小明", "李华", "王芳", "张伟", "刘洋" };
            CustomerType[] types = { CustomerType.Student, CustomerType.Commuter, CustomerType.CyclingEnthusiast };
            
            System.Random random = new System.Random();
            string name = names[random.Next(names.Length)];
            CustomerType type = types[random.Next(types.Length)];
            
            currentCustomer = new CustomerData
            {
                Name = name,
                Type = type,
                Budget = GetCustomerBudget(type),
                Demand = GetCustomerDemand(type)
            };
            
            // 根据需求推荐商品
            recommendedProducts = RecommendProducts(currentCustomer.Demand);
            
            // 刷新 UI
            RefreshCustomerUI();
        }

        /// <summary>
        /// 获取顾客预算（示例逻辑）
        /// </summary>
        private int GetCustomerBudget(CustomerType type)
        {
            switch (type)
            {
                case CustomerType.Student: return Random.Range(1000, 3000);
                case CustomerType.Commuter: return Random.Range(2000, 5000);
                case CustomerType.CyclingEnthusiast: return Random.Range(5000, 15000);
                default: return 3000;
            }
        }

        /// <summary>
        /// 获取顾客需求（示例逻辑）
        /// </summary>
        private string GetCustomerDemand(CustomerType type)
        {
            switch (type)
            {
                case CustomerType.Student: return "需要一辆性价比高的入门公路车，预算有限";
                case CustomerType.Commuter: return "需要一辆耐用舒适的通勤车，日常代步用";
                case CustomerType.CyclingEnthusiast: return "想要一辆性能优秀的中端公路车，追求速度和颜值";
                default: return "需要一辆自行车";
            }
        }

        /// <summary>
        /// 根据需求推荐商品（示例逻辑）
        /// </summary>
        private List<ProductData> RecommendProducts(string demand)
        {
            List<ProductData> products = new List<ProductData>();
            
            if (demand.Contains("入门") || demand.Contains("性价比"))
            {
                products.Add(new ProductData { Name = "入门铝合金公路车", Price = 2000, Brand = "本地品牌" });
            }
            if (demand.Contains("通勤") || demand.Contains("耐用"))
            {
                products.Add(new ProductData { Name = "通勤车", Price = 1500, Brand = "本地品牌" });
            }
            if (demand.Contains("性能") || demand.Contains("中端"))
            {
                products.Add(new ProductData { Name = "中端碳纤维公路车", Price = 8000, Brand = "速联" });
            }
            
            return products;
        }

        /// <summary>
        /// 刷新顾客 UI
        /// </summary>
        private void RefreshCustomerUI()
        {
            if (customerPanel == null) return;
            
            customerPanel.SetActive(true);
            customerNameText.text = currentCustomer.Name;
            customerTypeText.text = GetCustomerTypeDisplayName(currentCustomer.Type);
            customerDemandText.text = currentCustomer.Demand;
            
            // 清空现有推荐列表
            foreach (Transform child in productRecommendationList)
            {
                Destroy(child.gameObject);
            }
            
            // 生成推荐商品项
            foreach (var product in recommendedProducts)
            {
                if (productItemPrefab != null)
                {
                    GameObject itemObj = Instantiate(productItemPrefab, productRecommendationList);
                    // TODO: 设置商品项的 UI 内容（名称、价格、品牌）
                    // 示例：itemObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"{product.Name} - {product.Price:N0}元";
                }
            }
        }

        /// <summary>
        /// 获取顾客类型显示名称
        /// </summary>
        private string GetCustomerTypeDisplayName(CustomerType type)
        {
            switch (type)
            {
                case CustomerType.Student: return "学生";
                case CustomerType.Commuter: return "通勤族";
                case CustomerType.CyclingEnthusiast: return "骑行爱好者";
                default: return "顾客";
            }
        }

        /// <summary>
        /// 执行销售操作
        /// </summary>
        public void OnSellProduct()
        {
            if (recommendedProducts.Count == 0) return;
            
            // 默认销售第一个推荐商品
            ProductData selectedProduct = recommendedProducts[0];
            
            // 检查资金是否足够（顾客支付）
            if (currentCustomer.Budget >= selectedProduct.Price)
            {
                // 增加店铺资金
                GameManager.Instance.AddMoney(selectedProduct.Price);
                
                // 提升口碑
                int reputationGain = CalculateReputationGain(selectedProduct, currentCustomer);
                GameManager.Instance.AddReputation(reputationGain);
                
                // 显示交易成功提示
                Debug.Log($"成功向{currentCustomer.Name}销售{selectedProduct.Name}！");
                
                // 关闭顾客面板
                CloseCustomerPanel();
            }
            else
            {
                // 顾客预算不足
                Debug.LogWarning($"{currentCustomer.Name}预算不足，无法购买{selectedProduct.Name}");
                // TODO: 显示提示信息
            }
        }

        /// <summary>
        /// 执行维修操作
        /// </summary>
        public void OnRepairBike()
        {
            // 示例维修费用
            int repairCost = 200;
            
            // 检查顾客是否能支付
            if (currentCustomer.Budget >= repairCost)
            {
                // 增加店铺资金
                GameManager.Instance.AddMoney(repairCost);
                
                // 提升口碑
                GameManager.Instance.AddReputation(10);
                
                Debug.Log($"成功为{currentCustomer.Name}维修自行车！");
                CloseCustomerPanel();
            }
            else
            {
                Debug.LogWarning($"{currentCustomer.Name}无法支付维修费用");
            }
        }

        /// <summary>
        /// 计算口碑增益
        /// </summary>
        private int CalculateReputationGain(ProductData product, CustomerData customer)
        {
            // 基础口碑增益
            int baseGain = 20;
            
            // 根据商品价格和顾客满意度调整
            if (product.Price <= customer.Budget * 0.8f) // 顾客觉得物超所值
            {
                baseGain += 10;
            }
            
            return baseGain;
        }

        /// <summary>
        /// 关闭顾客面板
        /// </summary>
        public void OnCloseCustomer()
        {
            CloseCustomerPanel();
        }

        /// <summary>
        /// 实际关闭面板逻辑
        /// </summary>
        private void CloseCustomerPanel()
        {
            customerPanel.SetActive(false);
            // 切换回店铺经营状态
            GameManager.Instance.ChangeState(GameState.Shop);
            // TODO: 隐藏当前界面，显示店铺主界面
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 口碑变化回调
        /// </summary>
        private void OnReputationChanged(int newReputation)
        {
            // 可用于更新 UI 或触发特殊事件
        }
    }

    /// <summary>
    /// 顾客数据结构
    /// </summary>
    [System.Serializable]
    public class CustomerData
    {
        public string Name;
        public CustomerType Type;
        public int Budget;
        public string Demand;
    }

    /// <summary>
    /// 顾客类型枚举
    /// </summary>
    public enum CustomerType
    {
        Student,            // 学生
        Commuter,           // 通勤族
        CyclingEnthusiast   // 骑行爱好者
    }

    /// <summary>
    /// 商品数据结构
    /// </summary>
    [System.Serializable]
    public class ProductData
    {
        public string Name;
        public int Price;
        public string Brand;
    }
}