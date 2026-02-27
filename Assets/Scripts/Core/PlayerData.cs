using System;
using System.Collections.Generic;

namespace BikeShopTycoon.Core
{
    /// <summary>
    /// 玩家数据 - 包含所有存档信息
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        // 基础属性
        public int Money = 10000;           // 初始资金 1万
        public int Reputation = 10;         // 初始口碑
        public int Day = 1;                 // 游戏天数

        // 店铺等级
        public int ShopLevel = 1;
        public bool HasShowroom = false;
        public bool HasWorkshop = false;
        public bool HasCustomStudio = false;

        // 解锁状态
        public List<string> UnlockedBrands = new List<string>();    // 已解锁品牌
        public List<string> UnlockedItems = new List<string>();      // 已解锁商品

        // 里程碑
        public List<string> CompletedMilestones = new List<string>();

        // 社群
        public int FansCount = 0;
        public List<string> CustomerFriends = new List<string>();

        // 车队（第三阶段解锁）
        public bool HasTeam = false;
        public TeamData Team;

        public PlayerData()
        {
            // 初始解锁入门品牌
            UnlockedBrands.Add("本地品牌");
        }
    }

    [Serializable]
    public class TeamData
    {
        public string TeamName;
        public int TeamLevel;
        public List<CyclistData> Cyclists = new List<CyclistData>();
        public List<StaffData> Staff = new List<StaffData>();
    }

    [Serializable]
    public class CyclistData
    {
        public string Name;
        public CyclistType Type;        // 新人/业余赛手/职业赛手/退役老将
        public CyclistRole Role;        // 爬坡手/冲刺手/副将/全能主将

        // 属性
        public int Climbing;             // 爬坡
        public int Sprint;               // 冲刺
        public int TimeTrial;            // 计时赛
        public int Endurance;            // 耐力
        public int Recovery;             // 恢复力
        public int TacticalAwareness;    // 战术意识
        public int Teamwork;             // 团队协作

        // 状态
        public int Fatigue;              // 疲劳度
        public int Morale;               // 士气
        public CyclistPersonality Personality;  // 性格

        public int Salary;               // 工资
    }

    public enum CyclistType
    {
        Novice,         // 新人
        Amateur,        // 业余赛手
        Professional,   // 职业赛手
        Veteran         // 退役老将
    }

    public enum CyclistRole
    {
        Climber,        // 爬坡手
        Sprinter,       // 冲刺手
        Domestique,     // 副将
        AllRounder      // 全能主将
    }

    public enum CyclistPersonality
    {
        Competitive,    // 好胜型
        TeamPlayer,      // 团队型
        ClutchPlayer,    // 关键时刻爆发型
        Stable          // 稳定型
    }

    [Serializable]
    public class StaffData
    {
        public string Name;
        public StaffType Type;
        public int Skill;
        public int Salary;
    }

    public enum StaffType
    {
        Mechanic,      // 机械师
        Coach,         // 教练
        Nutritionist   // 营养师
    }
}