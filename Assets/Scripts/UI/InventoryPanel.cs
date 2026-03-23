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
    /// 库存管理面板控制器
    /// </summary>
    public class InventoryPanel : MonoBehaviour
    {
        [Header("UI 引用")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform itemListContainer;
        [SerializeField] private GameObject itemCardPrefab;
        [SerializeField] private TextMeshProUGUI totalItemsText;
        [SerializeField] private TextMeshProUGUI totalValueText;
        [SerializeField] private TextMeshProUGUI capacityText;

        [Header("进货面板")]
        [SerializeField] private GameObject purchasePanel;
        [SerializeField] private Transform purchaseListContainer;
        [SerializeField] private GameObject purchaseCardPrefab;
        [SerializeField] private TextMeshProUGUI purchaseCostText;
        [SerializeField] private Button confirmPurchaseButton;
        [SerializeField] private Button cancelPurchaseButton;

        [Header("按钮")]
        [SerializeField] private Button openInventoryButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button purchaseButton;

        // 系统
        private InventoryManager inventoryManager;
        private InventoryData inventoryData;
        private List<Item> availableProducts;
        
        // 临时购买列表
        private Dictionary<string, int> purchaseList = new Dictionary<string, int>();
        private int totalPurchaseCost = 0;

        private void Awake()
        {
            // 绑定按钮事件
            if (openInventoryButton != null)
                openInventoryButton.onClick.AddListener(OpenPanel);
            if (closeButton != null)
                closeButton.onClick.AddListener(ClosePanel);
            if (purchaseButton != null)
                purchaseButton.onClick.AddListener(OpenPurchasePanel);
            if (confirmPurchaseButton != null)
                confirmPurchaseButton.onClick.AddListener(ConfirmPurchase);
            if (cancelPurchaseButton != null)
                cancelPurchaseButton.onClick.AddListener(ClosePurchasePanel);

            // 初始隐藏
            if (panel != null) panel.SetActive(false);
            if (purchasePanel != null) purchasePanel.SetActive(false);
        }

        private void Start()
        {
            // 初始化系统
            if (GameManager.Instance != null && GameManager.Instance.PlayerData != null)
            {
                inventoryManager = new InventoryManager(GameManager.Instance.PlayerData);
                inventoryData = new InventoryData();
                
                // 添加初始库存
                InitializeStartingInventory();
            }

            // 加载产品列表
            LoadAvailableProducts();
        }

        /// <summary>
        /// 初始化起始库存
        /// </summary>
        private void InitializeStartingInventory()
        {
            // 给玩家一些初始库存
            var products = InitialProducts.CreateDefaultProducts();
            
            // 添加几台入门车作为初始库存
            var entryBikes = products.FindAll(p => p.Tier == ItemTier.Entry);
            foreach (var bike in entryBikes)
            {
                inventoryData.AddItem(bike, 2, bike.PurchasePrice);
            }
        }

        /// <summary>
        /// 加载可购买产品列表
        /// </summary>
        private void LoadAvailableProducts()
        {
            availableProducts = InitialProducts.CreateDefaultProducts();
        }

        #region 面板控制

        public void OpenPanel()
        {
            if (panel != null)
            {
                panel.SetActive(true);
                RefreshInventoryDisplay();
            }
        }

        public void ClosePanel()
        {
            if (panel != null)
                panel.SetActive(false);
        }

        public void OpenPurchasePanel()
        {
            if (purchasePanel != null)
            {
                purchasePanel.SetActive(true);
                purchaseList.Clear();
                totalPurchaseCost = 0;
                RefreshPurchaseDisplay();
            }
        }

        public void ClosePurchasePanel()
        {
            if (purchasePanel != null)
                purchasePanel.SetActive(false);
            purchaseList.Clear();
            totalPurchaseCost = 0;
        }

        #endregion

        #region 库存显示

        private void RefreshInventoryDisplay()
        {
            if (inventoryData == null) return;

            // 更新统计信息
            if (totalItemsText != null)
                totalItemsText.text = $"总数量: {inventoryData.UsedCapacity}";
            
            if (totalValueText != null)
                totalValueText.text = $"总价值: ¥{inventoryData.GetTotalValue():N0}";
            
            if (capacityText != null)
                capacityText.text = $"容量: {inventoryData.UsedCapacity}/{inventoryData.MaxCapacity}";

            // 清空列表
            if (itemListContainer != null)
            {
                foreach (Transform child in itemListContainer)
                {
                    Destroy(child.gameObject);
                }
            }

            // 显示库存物品
            if (itemCardPrefab != null && itemListContainer != null)
            {
                foreach (var item in inventoryData.Items)
                {
                    var card = Instantiate(itemCardPrefab, itemListContainer);
                    SetupItemCard(card, item);
                }
            }
        }

        private void SetupItemCard(GameObject card, InventoryItem item)
        {
            // 设置物品信息
            var nameText = card.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            var quantityText = card.transform.Find("QuantityText")?.GetComponent<TextMeshProUGUI>();
            var priceText = card.transform.Find("PriceText")?.GetComponent<TextMeshProUGUI>();
            var tierText = card.transform.Find("TierText")?.GetComponent<TextMeshProUGUI>();

            if (nameText != null)
                nameText.text = item.ItemData.Name;
            
            if (quantityText != null)
                quantityText.text = $"x{item.Quantity}";
            
            if (priceText != null)
                priceText.text = $"¥{item.ItemData.SellPrice:N0}";
            
            if (tierText != null)
                tierText.text = GetTierLabel(item.ItemData.Tier);

            // 滞销品标记
            var warningIcon = card.transform.Find("WarningIcon")?.GetComponent<GameObject>();
            if (warningIcon != null)
            {
                warningIcon.SetActive(item.IsStagnant);
            }
        }

        #endregion

        #region 进货系统

        private void RefreshPurchaseDisplay()
        {
            // 更新总价
            if (purchaseCostText != null)
                purchaseCostText.text = $"进货总价: ¥{totalPurchaseCost:N0}";

            // 清空列表
            if (purchaseListContainer != null)
            {
                foreach (Transform child in purchaseListContainer)
                {
                    Destroy(child.gameObject);
                }
            }

            // 显示可购买产品
            if (purchaseCardPrefab != null && purchaseListContainer != null)
            {
                foreach (var product in availableProducts)
                {
                    // 检查是否解锁
                    if (product.RequiredReputation > 0 && 
                        GameManager.Instance?.PlayerData?.Reputation < product.RequiredReputation)
                        continue;

                    var card = Instantiate(purchaseCardPrefab, purchaseListContainer);
                    SetupPurchaseCard(card, product);
                }
            }
        }

        private void SetupPurchaseCard(GameObject card, Item product)
        {
            var nameText = card.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            var costText = card.transform.Find("CostText")?.GetComponent<TextMeshProUGUI>();
            var priceText = card.transform.Find("PriceText")?.GetComponent<TextMeshProUGUI>();
            var quantityText = card.transform.Find("QuantityText")?.GetComponent<TextMeshProUGUI>();
            var addButton = card.transform.Find("AddButton")?.GetComponent<Button>();
            var removeButton = card.transform.Find("RemoveButton")?.GetComponent<Button>();

            if (nameText != null)
                nameText.text = product.Name;
            
            if (costText != null)
                costText.text = $"进价: ¥{product.PurchasePrice:N0}";
            
            if (priceText != null)
                priceText.text = $"售价: ¥{product.SellPrice:N0}";

            // 获取当前购买数量
            int currentQty = purchaseList.ContainsKey(product.Id) ? purchaseList[product.Id] : 0;
            if (quantityText != null)
                quantityText.text = currentQty.ToString();

            // 添加按钮
            if (addButton != null)
            {
                addButton.onClick.RemoveAllListeners();
                addButton.onClick.AddListener(() => AddToPurchaseList(product));
            }

            // 减少按钮
            if (removeButton != null)
            {
                removeButton.onClick.RemoveAllListeners();
                removeButton.onClick.AddListener(() => RemoveFromPurchaseList(product));
            }
        }

        private void AddToPurchaseList(Item product)
        {
            if (!purchaseList.ContainsKey(product.Id))
                purchaseList[product.Id] = 0;
            
            purchaseList[product.Id]++;
            totalPurchaseCost += product.PurchasePrice;
            
            RefreshPurchaseDisplay();
        }

        private void RemoveFromPurchaseList(Item product)
        {
            if (purchaseList.ContainsKey(product.Id) && purchaseList[product.Id] > 0)
            {
                purchaseList[product.Id]--;
                totalPurchaseCost -= product.PurchasePrice;
                RefreshPurchaseDisplay();
            }
        }

        private void ConfirmPurchase()
        {
            if (GameManager.Instance == null || inventoryData == null) return;

            // 检查资金
            if (totalPurchaseCost > GameManager.Instance.PlayerData.Money)
            {
                HUDController.Instance?.ShowNotification("资金不足！", NotificationType.Error);
                return;
            }

            // 检查库存容量
            int totalItems = 0;
            foreach (var qty in purchaseList.Values)
                totalItems += qty;
            
            if (!inventoryData.CanAddItem(totalItems))
            {
                HUDController.Instance?.ShowNotification("库存容量不足！", NotificationType.Error);
                return;
            }

            // 执行进货
            foreach (var kvp in purchaseList)
            {
                if (kvp.Value > 0)
                {
                    var product = availableProducts.Find(p => p.Id == kvp.Key);
                    if (product != null)
                    {
                        inventoryData.AddItem(product, kvp.Value, product.PurchasePrice);
                    }
                }
            }

            // 扣款
            GameManager.Instance.AddMoney(-totalPurchaseCost);
            
            HUDController.Instance?.ShowNotification($"进货成功！花费 ¥{totalPurchaseCost:N0}", NotificationType.Success);
            
            ClosePurchasePanel();
            RefreshInventoryDisplay();
        }

        #endregion

        #region 辅助方法

        private string GetTierLabel(ItemTier tier)
        {
            return tier switch
            {
                ItemTier.Entry => "入门",
                ItemTier.Mid => "中端",
                ItemTier.High => "高端",
                ItemTier.Pro => "职业",
                _ => ""
            };
        }

        /// <summary>
        /// 获取库存数据（供其他系统调用）
        /// </summary>
        public InventoryData GetInventoryData()
        {
            return inventoryData;
        }

        #endregion
    }
}