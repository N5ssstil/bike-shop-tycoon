using UnityEngine;
using System.Collections.Generic;

namespace BikeShop.Data
{
    /// <summary>
    /// 自行车品牌配置
    /// </summary>
    [CreateAssetMenu(fileName = "NewBrand", menuName = "BikeShop/Brand")]
    public class BikeBrand : ScriptableObject
    {
        [Header("基础信息")]
        public string brandId;
        public string brandName;
        [TextArea(2, 4)] public string description;
        public Sprite brandLogo;

        [Header("属性")]
        public int baseReputation = 10;
        public int unlockCost = 0;
        public float qualityMultiplier = 1.0f;
        public float priceMultiplier = 1.0f;

        [Header("顾客偏好")]
        public List<string> targetCustomerTypes = new List<string> { "通勤族", "运动族", "学生" };
    }
}