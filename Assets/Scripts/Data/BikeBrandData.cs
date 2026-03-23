using UnityEngine;

namespace BikeShopTycoon.Data
{
    /// <summary>
    /// 自行车品牌数据
    /// </summary>
    [CreateAssetMenu(fileName = "NewBrand", menuName = "BikeShop/Brand")]
    public class BikeBrand : ScriptableObject
    {
        public string brandId;
        public string brandName;
        [TextArea(2, 4)] public string description;
        public BrandTier tier;
        public int reputationBonus;
        public Sprite logo;
    }

    public enum BrandTier
    {
        Entry,      // 入门级
        Mid,        // 中端
        High,       // 高端
        Pro         // 职业级
    }
}