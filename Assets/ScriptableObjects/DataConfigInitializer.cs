using UnityEngine;
using BikeShop.Data;

namespace BikeShop.Initializer
{
    /// <summary>
    /// 数据配置初始化工具（编辑器脚本）
    /// 创建初始的品牌、商品、顾客故事配置
    /// </summary>
    public class DataConfigInitializer : MonoBehaviour
    {
        [Header("品牌配置")]
        public BikeBrand localBrand;
        public BikeBrand trekBrand;
        public BikeBrand giantBrand;

        [Header("商品配置")]
        public BikeProduct entryRoadBike;
        public BikeProduct midRoadBike;
        public BikeProduct proRoadBike;

        [Header("顾客故事")]
        public CustomerStory commuterStory;
        public CustomerStory athleteStory;
        public CustomerStory influencerStory;

        void Start()
        {
            Debug.Log("DataConfigInitializer - 这是初始化工具，实际配置请在 Unity 编辑器中创建 ScriptableObject");
        }
    }
}