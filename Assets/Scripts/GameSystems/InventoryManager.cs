using System;
using System.Collections.Generic;
using BikeShopTycoon.Data;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.GameSystems
{
    /// <summary>
    /// 库存管理系统
    /// </summary>
    public class InventoryManager
    {
        private InventoryData inventory;
        private PlayerData playerData;

        public event Action<string> OnStockWarning;      // 库存预警（库存不足）
        public event Action<string> OnStagnantWarning;   // 滞销预警
        public event Action<InventoryItem, int> OnItemSold;  // 商品售出

        public InventoryManager(PlayerData playerData)
        {
            this.playerData = playerData;
            this.inventory = new InventoryData();
        }

        /// <summary>
        /// 进货
        /// </summary>
        public bool PurchaseStock(Item item, int quantity, int unitPrice)
        {
            int totalCost = unitPrice * quantity;

            // 检查资金
            if (playerData.Money < totalCost)
            {
                OnStockWarning?.Invoke("资金不足，无法进货！");
                return false;
            }

            // 检查库存容量
            if (!inventory.CanAddItem(quantity))
            {
                OnStockWarning?.Invoke("库存已满，请先清理库存！");
                return false;
            }

            // 检查品牌授权
            if (!string.IsNullOrEmpty(item.RequiredBrandUnlock) && 
                !playerData.UnlockedBrands.Contains(item.RequiredBrandUnlock))
            {
                OnStockWarning?.Invoke($"尚未获得 {item.RequiredBrandUnlock} 的品牌授权！");
                return false;
            }

            // 扣款并入库
            playerData.Money -= totalCost;
            inventory.AddItem(item, quantity, unitPrice);

            return true;
        }

        /// <summary>
        /// 出售商品
        /// </summary>
        public bool SellItem(string itemId, int quantity, int sellPrice)
        {
            var invItem = inventory.GetItem(itemId);
            if (invItem == null || invItem.Quantity < quantity)
            {
                return false;
            }

            inventory.RemoveItem(itemId, quantity);
            playerData.Money += sellPrice * quantity;
            
            // 根据商品类型增加口碑
            int reputationGain = CalculateReputationGain(invItem.ItemData);
            playerData.Reputation += reputationGain;

            OnItemSold?.Invoke(invItem, quantity);
            return true;
        }

        /// <summary>
        /// 清理滞销品（打折出售）
        /// </summary>
        public bool ClearStagnantItem(string itemId, float discount = 0.5f)
        {
            var invItem = inventory.GetItem(itemId);
            if (invItem == null || !invItem.IsStagnant)
                return false;

            int discountedPrice = (int)(invItem.ItemData.SellPrice * discount);
            SellItem(itemId, invItem.Quantity, discountedPrice);
            
            OnStagnantWarning?.Invoke($"已清理滞销品: {invItem.ItemData.Name}");
            return true;
        }

        /// <summary>
        /// 获取库存报告
        /// </summary>
        public InventoryReport GetReport()
        {
            return new InventoryReport
            {
                TotalItems = inventory.UsedCapacity,
                TotalValue = inventory.GetTotalValue(),
                StagnantItems = inventory.GetStagnantItems(),
                ItemsByType = GetItemCountByType()
            };
        }

        private int CalculateReputationGain(Item item)
        {
            return item.Tier switch
            {
                ItemTier.Entry => 1,
                ItemTier.Mid => 2,
                ItemTier.High => 3,
                ItemTier.Pro => 5,
                _ => 1
            };
        }

        private Dictionary<ItemType, int> GetItemCountByType()
        {
            var counts = new Dictionary<ItemType, int>();
            foreach (var item in inventory.Items)
            {
                if (!counts.ContainsKey(item.ItemData.Type))
                    counts[item.ItemData.Type] = 0;
                counts[item.ItemData.Type] += item.Quantity;
            }
            return counts;
        }
    }

    public class InventoryReport
    {
        public int TotalItems;
        public int TotalValue;
        public List<InventoryItem> StagnantItems;
        public Dictionary<ItemType, int> ItemsByType;
    }
}