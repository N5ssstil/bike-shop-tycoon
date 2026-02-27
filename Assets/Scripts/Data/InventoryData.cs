using System;
using System.Collections.Generic;

namespace BikeShopTycoon.Data
{
    /// <summary>
    /// 库存项
    /// </summary>
    [Serializable]
    public class InventoryItem
    {
        public string ItemId;
        public Item ItemData;
        public int Quantity;
        public int DaysInStock;        // 库存天数（用于滞销品检测）
        public DateTime PurchaseDate;
        public int PurchasePrice;       // 入库时的进价
        
        public bool IsStagnant => DaysInStock > 30;  // 30天以上视为滞销
        
        public InventoryItem(Item item, int quantity, int purchasePrice)
        {
            ItemId = item.Id;
            ItemData = item;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
            PurchaseDate = DateTime.Now;
            DaysInStock = 0;
        }
    }

    /// <summary>
    /// 库存数据
    /// </summary>
    [Serializable]
    public class InventoryData
    {
        public List<InventoryItem> Items = new List<InventoryItem>();
        public int MaxCapacity = 50;    // 最大库存容量
        public int UsedCapacity = 0;
        
        public event Action<InventoryItem> OnItemAdded;
        public event Action<InventoryItem> OnItemRemoved;
        public event Action OnInventoryChanged;

        public bool CanAddItem(int quantity = 1)
        {
            return UsedCapacity + quantity <= MaxCapacity;
        }

        public void AddItem(Item item, int quantity, int purchasePrice)
        {
            if (!CanAddItem(quantity))
            {
                throw new Exception("库存已满！");
            }

            var existing = Items.Find(i => i.ItemId == item.Id);
            if (existing != null)
            {
                existing.Quantity += quantity;
                existing.PurchasePrice = purchasePrice; // 更新进价
            }
            else
            {
                var newItem = new InventoryItem(item, quantity, purchasePrice);
                Items.Add(newItem);
                OnItemAdded?.Invoke(newItem);
            }

            UsedCapacity += quantity;
            OnInventoryChanged?.Invoke();
        }

        public bool RemoveItem(string itemId, int quantity)
        {
            var item = Items.Find(i => i.ItemId == itemId);
            if (item == null || item.Quantity < quantity)
                return false;

            item.Quantity -= quantity;
            UsedCapacity -= quantity;

            if (item.Quantity <= 0)
            {
                Items.Remove(item);
                OnItemRemoved?.Invoke(item);
            }

            OnInventoryChanged?.Invoke();
            return true;
        }

        public InventoryItem GetItem(string itemId)
        {
            return Items.Find(i => i.ItemId == itemId);
        }

        public List<InventoryItem> GetStagnantItems()
        {
            return Items.FindAll(i => i.IsStagnant);
        }

        public List<InventoryItem> GetItemsByType(ItemType type)
        {
            return Items.FindAll(i => i.ItemData.Type == type);
        }

        public int GetTotalValue()
        {
            int total = 0;
            foreach (var item in Items)
            {
                total += item.PurchasePrice * item.Quantity;
            }
            return total;
        }

        public void UpdateDaysInStock()
        {
            foreach (var item in Items)
            {
                item.DaysInStock++;
            }
        }
    }
}