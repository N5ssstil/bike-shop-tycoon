using System;
using System.Collections.Generic;

namespace BikeShopTycoon.Data
{
    /// <summary>
    /// 商品类型
    /// </summary>
    public enum ItemType
    {
        Bike,           // 整车
        Frame,          // 车架
        Groupset,       // 套件
        Wheels,         // 轮组
        Accessories,   // 配件（水壶、码表等）
        Tools,          // 维修工具
        Apparel         // 骑行服等
    }

    /// <summary>
    /// 商品等级
    /// </summary>
    public enum ItemTier
    {
        Entry,          // 入门级
        Mid,            // 中端
        High,           // 高端
        Pro             // 竞赛级
    }

    /// <summary>
    /// 商品数据
    /// </summary>
    [Serializable]
    public class Item
    {
        public string Id;
        public string Name;
        public string Description;
        public ItemType Type;
        public ItemTier Tier;
        public string Brand;
        
        [Header("价格")]
        public int PurchasePrice;       // 进货价
        public int SellPrice;           // 售价
        
        [Header("属性")]
        public int Weight;              // 重量（克）
        public int Durability;          // 耐久度
        public int Performance;         // 性能值
        
        [Header("解锁条件")]
        public int RequiredReputation;  // 需要口碑
        public string RequiredBrandUnlock; // 需要品牌授权
        
        [Header("视觉")]
        public string SpritePath;       // 图片路径
    }

    /// <summary>
    /// 自行车整车数据（继承自商品）
    /// </summary>
    [Serializable]
    public class Bike : Item
    {
        public FrameMaterial FrameMaterial;
        public BrakeType BrakeType;
        public int Gears;              // 齿数（如 22速）
        public string GroupsetBrand;   // 套件品牌（Shimano, SRAM, Campagnolo）
        public string Wheelset;        // 轮组型号
        
        // 尺寸
        public List<string> AvailableSizes = new List<string> { "XS", "S", "M", "L", "XL" };
        
        // 适用场景
        public bool IsForRacing;       // 竞赛
        public bool IsForCommuting;    // 通勤
        public bool IsForBeginners;    // 新手
    }

    public enum FrameMaterial
    {
        Aluminum,      // 铝合金
        Steel,         // 钢
        Carbon,        // 碳纤维
        Titanium      // 钛合金
    }

    public enum BrakeType
    {
        RimBrake,      // 圈刹
        DiscBrake      // 碟刹
    }

    /// <summary>
    /// 品牌数据
    /// </summary>
    [Serializable]
    public class Brand
    {
        public string Id;
        public string Name;
        public string Country;          // 原产国
        public int Tier;                // 品牌等级（1-5）
        
        [Header("解锁条件")]
        public int RequiredReputation;
        public int RequiredMoney;       // 授权费
        
        [Header("品牌特性")]
        public float PriceModifier;      // 价格系数
        public float QualityModifier;   // 质量系数
    }

    /// <summary>
    /// 商品配置数据（ScriptableObject用）
    /// </summary>
    [Serializable]
    public class ItemDatabase
    {
        public List<Item> AllItems = new List<Item>();
        public List<Bike> AllBikes = new List<Bike>();
        public List<Brand> AllBrands = new List<Brand>();
        
        public Item GetItemById(string id)
        {
            return AllItems.Find(i => i.Id == id);
        }
        
        public Bike GetBikeById(string id)
        {
            return AllBikes.Find(b => b.Id == id);
        }
        
        public Brand GetBrandById(string id)
        {
            return AllBrands.Find(b => b.Id == id);
        }
        
        public List<Item> GetItemsByType(ItemType type)
        {
            return AllItems.FindAll(i => i.Type == type);
        }
        
        public List<Item> GetItemsByTier(ItemTier tier)
        {
            return AllItems.FindAll(i => i.Tier == tier);
        }
    }
}