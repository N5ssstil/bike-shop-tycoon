using UnityEngine;
using System.Collections.Generic;

namespace BikeShopTycoon.Data
{
    /// <summary>
    /// 产品数据库 - 存储所有可销售商品
    /// </summary>
    [CreateAssetMenu(fileName = "ProductDatabase", menuName = "BikeShop/ProductDatabase")]
    public class ProductDatabase : ScriptableObject
    {
        public List<Item> allProducts = new List<Item>();
        public List<Bike> allBikes = new List<Bike>();
        
        /// <summary>
        /// 获取所有公路车
        /// </summary>
        public List<Item> GetRoadBikes()
        {
            return allProducts.FindAll(p => p.Type == ItemType.Bike);
        }
        
        /// <summary>
        /// 按价格范围获取商品
        /// </summary>
        public List<Item> GetProductsByPriceRange(int minPrice, int maxPrice)
        {
            return allProducts.FindAll(p => p.SellPrice >= minPrice && p.SellPrice <= maxPrice);
        }
        
        /// <summary>
        /// 按等级获取商品
        /// </summary>
        public List<Item> GetProductsByTier(ItemTier tier)
        {
            return allProducts.FindAll(p => p.Tier == tier);
        }
    }

    /// <summary>
    /// 初始产品数据 - 用于游戏启动时生成默认商品
    /// </summary>
    public static class InitialProducts
    {
        public static List<Item> CreateDefaultProducts()
        {
            var products = new List<Item>();

            // 入门级公路车
            products.Add(new Bike
            {
                Id = "bike_entry_001",
                Name = "捷安特 Escape 3",
                Description = "入门级铝合金公路车，适合新手通勤",
                Type = ItemType.Bike,
                Tier = ItemTier.Entry,
                Brand = "Giant",
                PurchasePrice = 1800,
                SellPrice = 2500,
                Weight = 10500,
                Durability = 80,
                Performance = 40,
                RequiredReputation = 0,
                FrameMaterial = FrameMaterial.Aluminum,
                BrakeType = BrakeType.RimBrake,
                Gears = 16,
                GroupsetBrand = "Shimano Claris",
                IsForBeginners = true,
                IsForCommuting = true,
                IsForRacing = false
            });

            products.Add(new Bike
            {
                Id = "bike_entry_002",
                Name = "美利达 挑战者 300",
                Description = "入门山地车，适合轻度越野和通勤",
                Type = ItemType.Bike,
                Tier = ItemTier.Entry,
                Brand = "Merida",
                PurchasePrice = 2200,
                SellPrice = 3000,
                Weight = 12000,
                Durability = 90,
                Performance = 35,
                RequiredReputation = 0,
                FrameMaterial = FrameMaterial.Aluminum,
                BrakeType = BrakeType.DiscBrake,
                Gears = 18,
                GroupsetBrand = "Shimano Altus",
                IsForBeginners = true,
                IsForCommuting = true,
                IsForRacing = false
            });

            // 中端公路车
            products.Add(new Bike
            {
                Id = "bike_mid_001",
                Name = "捷安特 TCR Advanced 2",
                Description = "碳纤维公路车，适合训练和业余比赛",
                Type = ItemType.Bike,
                Tier = ItemTier.Mid,
                Brand = "Giant",
                PurchasePrice = 12000,
                SellPrice = 15800,
                Weight = 7800,
                Durability = 75,
                Performance = 75,
                RequiredReputation = 20,
                FrameMaterial = FrameMaterial.Carbon,
                BrakeType = BrakeType.DiscBrake,
                Gears = 22,
                GroupsetBrand = "Shimano 105",
                IsForBeginners = false,
                IsForCommuting = false,
                IsForRacing = true
            });

            products.Add(new Bike
            {
                Id = "bike_mid_002",
                Name = "闪电 Allez Sprint",
                Description = "铝合金竞技公路车，冲刺手的最爱",
                Type = ItemType.Bike,
                Tier = ItemTier.Mid,
                Brand = "Specialized",
                PurchasePrice = 15000,
                SellPrice = 19800,
                Weight = 8200,
                Durability = 80,
                Performance = 80,
                RequiredReputation = 30,
                FrameMaterial = FrameMaterial.Aluminum,
                BrakeType = BrakeType.DiscBrake,
                Gears = 22,
                GroupsetBrand = "Shimano 105",
                IsForBeginners = false,
                IsForCommuting = false,
                IsForRacing = true
            });

            // 高端公路车
            products.Add(new Bike
            {
                Id = "bike_high_001",
                Name = "捷安特 Propel Advanced SL",
                Description = "顶级气动公路车，职业车手的选择",
                Type = ItemType.Bike,
                Tier = ItemTier.High,
                Brand = "Giant",
                PurchasePrice = 45000,
                SellPrice = 58000,
                Weight = 6800,
                Durability = 70,
                Performance = 95,
                RequiredReputation = 100,
                FrameMaterial = FrameMaterial.Carbon,
                BrakeType = BrakeType.DiscBrake,
                Gears = 24,
                GroupsetBrand = "Shimano Dura-Ace",
                IsForBeginners = false,
                IsForCommuting = false,
                IsForRacing = true
            });

            products.Add(new Bike
            {
                Id = "bike_high_002",
                Name = "闪电 Tarmac SL7",
                Description = "环法冠军战车，全能竞赛公路车",
                Type = ItemType.Bike,
                Tier = ItemTier.High,
                Brand = "Specialized",
                PurchasePrice = 55000,
                SellPrice = 72000,
                Weight = 6600,
                Durability = 68,
                Performance = 98,
                RequiredReputation = 150,
                FrameMaterial = FrameMaterial.Carbon,
                BrakeType = BrakeType.DiscBrake,
                Gears = 24,
                GroupsetBrand = "SRAM Red",
                IsForBeginners = false,
                IsForCommuting = false,
                IsForRacing = true
            });

            // 配件
            products.Add(new Item
            {
                Id = "acc_helmet_001",
                Name = "Giro Aether MIPS",
                Description = "高端公路头盔，MIPS安全系统",
                Type = ItemType.Accessories,
                Tier = ItemTier.High,
                Brand = "Giro",
                PurchasePrice = 1800,
                SellPrice = 2500,
                Weight = 280,
                Durability = 100,
                Performance = 90,
                RequiredReputation = 10
            });

            products.Add(new Item
            {
                Id = "acc_computer_001",
                Name = "Garmin Edge 540",
                Description = "专业骑行电脑，GPS导航",
                Type = ItemType.Accessories,
                Tier = ItemTier.Mid,
                Brand = "Garmin",
                PurchasePrice = 2800,
                SellPrice = 3500,
                Weight = 85,
                Durability = 100,
                Performance = 85,
                RequiredReputation = 5
            });

            return products;
        }
    }
}