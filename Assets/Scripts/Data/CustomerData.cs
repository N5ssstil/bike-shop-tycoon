using System;
using System.Collections.Generic;

namespace BikeShopTycoon.Data
{
    /// <summary>
    /// 顾客类型
    /// </summary>
    public enum CustomerType
    {
        Student,            // 学生
        Commuter,           // 通勤族
        CyclingEnthusiast,  // 普通骑行爱好者
        Racer,              // 竞赛车手（第二阶段）
        Influencer          // 网红/拍照党（第二阶段）
    }

    /// <summary>
    /// 顾客状态
    /// </summary>
    public enum CustomerState
    {
        Entering,           // 进店
        Browsing,           // 浏览
        Asking,             // 咨询
        Deciding,           // 决定中
        Purchasing,         // 购买
        Leaving,            // 离开
        Satisfied,          // 满意离开
        Unsatisfied         // 不满意离开
    }

    /// <summary>
    /// 顾客需求类型
    /// </summary>
    public enum NeedType
    {
        BuyBike,            // 买车
        BuyAccessories,     // 买配件
        Repair,             // 维修
        Customize,          // 改装/定制（第二阶段）
        Consult             // 咨询
    }

    /// <summary>
    /// 顾客数据
    /// </summary>
    [Serializable]
    public class Customer
    {
        public string Id;
        public string Name;
        public CustomerType Type;
        public CustomerState State;
        
        [Header("需求")]
        public NeedType CurrentNeed;
        public CustomerNeeds Needs;
        
        [Header("属性")]
        public int Budget;              // 预算
        public int Patience;            // 耐心值（100满）
        public int Satisfaction;        // 满意度
        
        [Header("故事")]
        public string StoryId;          // 故事ID
        public bool StoryRevealed;      // 故事是否已揭示
        
        [Header("视觉")]
        public string AvatarPath;
        
        public Customer()
        {
            Id = Guid.NewGuid().ToString();
            State = CustomerState.Entering;
            Patience = 100;
            Satisfaction = 50;
        }
    }

    /// <summary>
    /// 顾客需求
    /// </summary>
    [Serializable]
    public class CustomerNeeds
    {
        // 预算范围
        public int MinBudget;
        public int MaxBudget;
        
        // 用途
        public bool NeedForRacing;      // 竞赛需求
        public bool NeedForCommuting;   // 通勤需求
        public bool NeedForTraining;    // 训练需求
        public bool NeedForBeginners;   // 新手入门
        
        // 偏好
        public ItemTier PreferredTier;  // 偏好等级
        public string PreferredBrand;   // 偏好品牌
        public FrameMaterial? PreferredMaterial;  // 偏好材质
        
        // 维修需求
        public RepairType? RepairNeed;  // 维修类型
        
        // 配件需求
        public List<ItemType> AccessoryNeeds = new List<ItemType>();
        
        // 特殊需求（网红类）
        public bool NeedHighVisual;     // 需要高颜值
        public string PreferredColor;   // 偏好颜色
    }

    /// <summary>
    /// 维修类型
    /// </summary>
    public enum RepairType
    {
        FlatTire,           // 爆胎
        GearAdjustment,     // 变速调试
        BrakeService,       // 刹车保养
        WheelTruing,        // 编圈调整
        FullService,        // 全车保养
        CustomTuning        // 定制调校
    }

    /// <summary>
    /// 顾客故事
    /// </summary>
    [Serializable]
    public class CustomerStory
    {
        public string Id;
        public string Title;
        public string Description;      // 故事描述
        public string[] Dialogues;      // 对话内容
        public CustomerType TargetCustomerType;
        public string Reward;           // 完成后的奖励描述
        public int ReputationBonus;     // 口碑奖励
    }

    /// <summary>
    /// 顾客生成器配置
    /// </summary>
    [Serializable]
    public class CustomerGeneratorSettings
    {
        public float BaseSpawnInterval = 30f;       // 基础生成间隔（秒）
        public float MinSpawnInterval = 10f;        // 最小间隔
        public float MaxSpawnInterval = 120f;       // 最大间隔
        
        // 各类型顾客生成权重
        public int StudentWeight = 30;
        public int CommuterWeight = 30;
        public int EnthusiastWeight = 30;
        public int RacerWeight = 5;
        public int InfluencerWeight = 5;
        
        // 口碑对客流的影响系数
        public float ReputationMultiplier = 0.1f;
    }
}