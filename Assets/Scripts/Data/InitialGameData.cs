using UnityEngine;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.Data
{
    /// <summary>
    /// 初始游戏数据配置
    /// 在游戏启动时自动创建默认的商品和品牌数据
    /// </summary>
    [CreateAssetMenu(fileName = "InitialGameData", menuName = "BikeShop/InitialGameData")]
    public class InitialGameData : ScriptableObject
    {
        [Header("初始资金")]
        public int startingMoney = 10000;
        public int startingReputation = 10;

        [Header("顾客生成设置")]
        public CustomerGeneratorSettings customerSettings;
    }

    /// <summary>
    /// 顾客生成设置
    /// </summary>
    [System.Serializable]
    public class CustomerGeneratorSettings
    {
        [Header("顾客类型权重")]
        [Range(0, 100)] public int StudentWeight = 30;
        [Range(0, 100)] public int CommuterWeight = 35;
        [Range(0, 100)] public int EnthusiastWeight = 20;
        [Range(0, 100)] public int RacerWeight = 10;
        [Range(0, 100)] public int InfluencerWeight = 5;

        [Header("生成间隔")]
        public float minSpawnInterval = 5f;
        public float maxSpawnInterval = 15f;

        [Header("同时在线顾客数")]
        public int maxConcurrentCustomers = 3;
    }
}