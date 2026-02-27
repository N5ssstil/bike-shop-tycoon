using UnityEngine;
using System.Collections.Generic;

namespace BikeShop.Data
{
    /// <summary>
    /// 自行车商品配置
    /// </summary>
    [CreateAssetMenu(fileName = "NewProduct", menuName = "BikeShop/Product")]
    public class BikeProduct : ScriptableObject
    {
        [Header("基础信息")]
        public string productId;
        public string productName;
        [TextArea(2, 4)] public string description;
        public BikeBrand brand;

        [Header("属性")]
        public int baseCost;           // 进货成本
        public int basePrice;          // 销售价
        public int reputationGain;     // 交易获得口碑
        public int qualityLevel = 1;   // 1-5
        public BikeType bikeType;

        [Header("颜色变体")]
        public List<string> availableColors = new List<string>();

        [Header("顾客偏好")]
        public string preferredCustomerType;
        public string preferredPriceRange = "中等";
    }

    public enum BikeType
    {
        RoadBike,       // 公路车
        MountainBike,   // 山地车
        HybridBike,     // 混合车
        CityBike        // 城市车
    }
}