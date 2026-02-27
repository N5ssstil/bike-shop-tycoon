using UnityEngine;
using System.Collections.Generic;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.UI
{
    /// <summary>
    /// 库存管理界面控制器
    /// 负责显示商品列表、进货操作和库存管理
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Header("UI 元素引用")]
        public Transform inventoryListContainer; // 商品列表容器（用于动态生成商品项）
        public GameObject inventoryItemPrefab;   // 商品项预制体
        public TMPro.TextMeshProUGUI totalValueText; // 库存总价值显示
        
        [Header("进货相关")]
        public GameObject purchasePanel;        // 进货面板
        public TMPro.TMP_Dropdown brandDropdown; // 品牌下拉菜单
        public TMPro.TMP_Dropdown itemDropdown;  // 商品下拉菜单
        public TMPro.TMP_InputField quantityInput; // 数量输入框
        public TMPro.TextMeshProUGUI costText;   // 预估成本显示

        private List<InventoryItemData> currentInventory = new List<InventoryItemData>();
        private Dictionary<string, List<string>> brandItemsMap = new Dictionary<string, List<string>>();

        private void Start()
        {
            // 初始化品牌-商品映射（示例数据，实际应从配置加载）
            InitializeBrandItemsMap();
            
            // 刷新进货面板
            RefreshPurchasePanel();
            
            // 加载当前库存（示例：从 GameManager 获取）
            LoadInventoryData();
            
            // 订阅资金变化事件（用于更新可购买状态）
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
        /// 初始化品牌和商品映射关系
        /// </summary>
        private void InitializeBrandItemsMap()
        {
            // 示例数据：实际应从 ScriptableObject 配置加载
            brandItemsMap.Clear();
            brandItemsMap.Add("本地品牌", new List<string> { "入门铝合金公路车", "通勤车", "基础维修工具包" });
            brandItemsMap.Add("禧玛诺", new List<string> { "变速套件", "刹车系统" });
            brandItemsMap.Add("速联", new List<string> { "高端轮组", "碳纤维车架" });
            
            // 更新品牌下拉菜单
            if (brandDropdown != null)
            {
                brandDropdown.ClearOptions();
                brandDropdown.AddOptions(new List<string>(brandItemsMap.Keys));
                brandDropdown.onValueChanged.AddListener(OnBrandSelected);
            }
        }

        /// <summary>
        /// 品牌选择回调
        /// </summary>
        private void OnBrandSelected(int index)
        {
            string selectedBrand = brandDropdown.options[index].text;
            if (brandItemsMap.TryGetValue(selectedBrand, out var items))
            {
                itemDropdown.ClearOptions();
                itemDropdown.AddOptions(items);
                OnItemSelected(0); // 默认选中第一个商品
            }
        }

        /// <summary>
        /// 商品选择回调
        /// </summary>
        private void OnItemSelected(int index)
        {
            UpdateCostDisplay();
        }

        /// <summary>
        /// 数量输入变化回调
        /// </summary>
        public void OnQuantityChanged()
        {
            UpdateCostDisplay();
        }

        /// <summary>
        /// 更新预估成本显示
        /// </summary>
        private void UpdateCostDisplay()
        {
            if (costText == null) return;
            
            int quantity = 1;
            if (!string.IsNullOrEmpty(quantityInput.text))
            {
                quantity = int.TryParse(quantityInput.text, out int q) ? q : 1;
            }
            
            // 示例价格逻辑：实际应从商品配置获取
            string selectedItem = itemDropdown.options[itemDropdown.value].text;
            int basePrice = GetBasePrice(selectedItem);
            int totalCost = basePrice * quantity;
            
            costText.text = $"预估成本: {totalCost:N0} 元";
            
            // 检查是否可购买
            bool canAfford = GameManager.Instance.CanAfford(totalCost);
            // TODO: 更新购买按钮状态（禁用/启用）
        }

        /// <summary>
        /// 获取商品基础价格（示例逻辑）
        /// </summary>
        private int GetBasePrice(string itemName)
        {
            // 实际应从商品配置数据获取
            switch (itemName)
            {
                case "入门铝合金公路车": return 2000;
                case "通勤车": return 1500;
                case "基础维修工具包": return 300;
                case "变速套件": return 800;
                case "刹车系统": return 600;
                case "高端轮组": return 2500;
                case "碳纤维车架": return 5000;
                default: return 1000;
            }
        }

        /// <summary>
        /// 执行进货操作
        /// </summary>
        public void OnPurchaseItem()
        {
            if (itemDropdown == null || quantityInput == null) return;
            
            int quantity = 1;
            if (!string.IsNullOrEmpty(quantityInput.text))
            {
                quantity = int.TryParse(quantityInput.text, out int q) ? q : 1;
            }
            
            string selectedItem = itemDropdown.options[itemDropdown.value].text;
            int basePrice = GetBasePrice(selectedItem);
            int totalCost = basePrice * quantity;
            
            // 检查资金是否足够
            if (!GameManager.Instance.TrySpendMoney(totalCost))
            {
                // TODO: 显示资金不足提示
                Debug.LogWarning("资金不足，无法进货");
                return;
            }
            
            // 添加到库存（示例逻辑）
            AddToInventory(selectedItem, quantity, basePrice);
            
            // 刷新库存显示
            RefreshInventoryList();
            
            // 保存游戏
            GameManager.Instance.SaveGame();
        }

        /// <summary>
        /// 添加商品到库存
        /// </summary>
        private void AddToInventory(string itemName, int quantity, int price)
        {
            // 查找是否已有该商品
            var existingItem = currentInventory.Find(item => item.Name == itemName);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                currentInventory.Add(new InventoryItemData
                {
                    Name = itemName,
                    Quantity = quantity,
                    Price = price
                });
            }
        }

        /// <summary>
        /// 刷新库存列表显示
        /// </summary>
        private void RefreshInventoryList()
        {
            // 清空现有列表
            foreach (Transform child in inventoryListContainer)
            {
                Destroy(child.gameObject);
            }
            
            // 重新生成商品项
            int totalValue = 0;
            foreach (var item in currentInventory)
            {
                if (inventoryItemPrefab != null)
                {
                    GameObject itemObj = Instantiate(inventoryItemPrefab, inventoryListContainer);
                    // TODO: 设置商品项的 UI 内容（名称、数量、单价）
                    // 示例：itemObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"{item.Name} x{item.Quantity}";
                    totalValue += item.Quantity * item.Price;
                }
            }
            
            // 更新总价值显示
            if (totalValueText != null)
            {
                totalValueText.text = $"库存总价值: {totalValue:N0} 元";
            }
        }

        /// <summary>
        /// 加载库存数据（示例：从内存加载）
        /// </summary>
        private void LoadInventoryData()
        {
            // 实际应从存档或数据库加载
            currentInventory.Clear();
            currentInventory.Add(new InventoryItemData { Name = "入门铝合金公路车", Quantity = 2, Price = 2000 });
            currentInventory.Add(new InventoryItemData { Name = "通勤车", Quantity = 3, Price = 1500 });
            RefreshInventoryList();
        }

        /// <summary>
        /// 资金变化回调
        /// </summary>
        private void OnMoneyChanged(int newMoney)
        {
            // 更新购买能力状态
            UpdateCostDisplay();
        }

        /// <summary>
        /// 返回店铺主界面
        /// </summary>
        public void OnReturnToShop()
        {
            // 切换回店铺经营状态
            GameManager.Instance.ChangeState(GameState.Shop);
            // TODO: 隐藏当前库存界面，显示店铺主界面
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 库存商品数据结构
    /// </summary>
    [System.Serializable]
    public class InventoryItemData
    {
        public string Name;
        public int Quantity;
        public int Price;
    }
}