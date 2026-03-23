using System;
using System.Collections.Generic;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.GameSystems
{
    /// <summary>
    /// 成就/里程碑系统
    /// </summary>
    public class AchievementSystem
    {
        private PlayerData playerData;
        private List<Achievement> achievements;
        private HashSet<string> unlockedAchievements;

        public event Action<Achievement> OnAchievementUnlocked;

        public AchievementSystem(PlayerData playerData)
        {
            this.playerData = playerData;
            this.achievements = new List<Achievement>();
            this.unlockedAchievements = new HashSet<string>(playerData.CompletedMilestones);

            InitializeAchievements();
        }

        /// <summary>
        /// 检查所有成就
        /// </summary>
        public void CheckAchievements()
        {
            foreach (var achievement in achievements)
            {
                if (!unlockedAchievements.Contains(achievement.Id) && CheckCondition(achievement))
                {
                    UnlockAchievement(achievement);
                }
            }
        }

        /// <summary>
        /// 检查单个成就条件
        /// </summary>
        private bool CheckCondition(Achievement achievement)
        {
            return achievement.Type switch
            {
                AchievementType.Money => playerData.Money >= achievement.RequiredValue,
                AchievementType.Reputation => playerData.Reputation >= achievement.RequiredValue,
                AchievementType.Day => playerData.Day >= achievement.RequiredValue,
                AchievementType.BrandUnlock => playerData.UnlockedBrands.Count >= achievement.RequiredValue,
                AchievementType.Fans => playerData.FansCount >= achievement.RequiredValue,
                _ => false
            };
        }

        /// <summary>
        /// 解锁成就
        /// </summary>
        private void UnlockAchievement(Achievement achievement)
        {
            unlockedAchievements.Add(achievement.Id);
            playerData.CompletedMilestones.Add(achievement.Id);

            // 发放奖励
            if (achievement.RewardMoney > 0)
                playerData.Money += achievement.RewardMoney;
            if (achievement.RewardReputation > 0)
                playerData.Reputation += achievement.RewardReputation;

            OnAchievementUnlocked?.Invoke(achievement);
            Debug.Log($"[AchievementSystem] 成就解锁: {achievement.Name}");
        }

        /// <summary>
        /// 初始化成就列表
        /// </summary>
        private void InitializeAchievements()
        {
            achievements = new List<Achievement>
            {
                // 财富成就
                new Achievement
                {
                    Id = "money_10k",
                    Name = "小本经营",
                    Description = "累计资金达到 1 万元",
                    Type = AchievementType.Money,
                    RequiredValue = 10000,
                    RewardMoney = 500
                },
                new Achievement
                {
                    Id = "money_50k",
                    Name = "生意兴隆",
                    Description = "累计资金达到 5 万元",
                    Type = AchievementType.Money,
                    RequiredValue = 50000,
                    RewardMoney = 2000,
                    RewardReputation = 10
                },
                new Achievement
                {
                    Id = "money_100k",
                    Name = "财源广进",
                    Description = "累计资金达到 10 万元",
                    Type = AchievementType.Money,
                    RequiredValue = 100000,
                    RewardMoney = 5000,
                    RewardReputation = 20
                },
                new Achievement
                {
                    Id = "money_1m",
                    Name = "百万富翁",
                    Description = "累计资金达到 100 万元",
                    Type = AchievementType.Money,
                    RequiredValue = 1000000,
                    RewardMoney = 50000,
                    RewardReputation = 100
                },

                // 口碑成就
                new Achievement
                {
                    Id = "rep_50",
                    Name = "小有名气",
                    Description = "口碑达到 50",
                    Type = AchievementType.Reputation,
                    RequiredValue = 50,
                    RewardReputation = 5
                },
                new Achievement
                {
                    Id = "rep_200",
                    Name = "声名远扬",
                    Description = "口碑达到 200",
                    Type = AchievementType.Reputation,
                    RequiredValue = 200,
                    RewardMoney = 1000,
                    RewardReputation = 20
                },
                new Achievement
                {
                    Id = "rep_500",
                    Name = "行业标杆",
                    Description = "口碑达到 500",
                    Type = AchievementType.Reputation,
                    RequiredValue = 500,
                    RewardMoney = 5000,
                    RewardReputation = 50
                },
                new Achievement
                {
                    Id = "rep_1000",
                    Name = "传奇车店",
                    Description = "口碑达到 1000（满级）",
                    Type = AchievementType.Reputation,
                    RequiredValue = 1000,
                    RewardMoney = 20000,
                    RewardReputation = 100
                },

                // 时间成就
                new Achievement
                {
                    Id = "day_10",
                    Name = "坚持一周",
                    Description = "经营满 10 天",
                    Type = AchievementType.Day,
                    RequiredValue = 10,
                    RewardMoney = 500
                },
                new Achievement
                {
                    Id = "day_30",
                    Name = "月度之星",
                    Description = "经营满 30 天",
                    Type = AchievementType.Day,
                    RequiredValue = 30,
                    RewardMoney = 2000,
                    RewardReputation = 10
                },
                new Achievement
                {
                    Id = "day_100",
                    Name = "百年老店",
                    Description = "经营满 100 天",
                    Type = AchievementType.Day,
                    RequiredValue = 100,
                    RewardMoney = 10000,
                    RewardReputation = 30
                },

                // 品牌成就
                new Achievement
                {
                    Id = "brand_3",
                    Name = "品牌合作",
                    Description = "解锁 3 个品牌授权",
                    Type = AchievementType.BrandUnlock,
                    RequiredValue = 3,
                    RewardMoney = 1000,
                    RewardReputation = 10
                },
                new Achievement
                {
                    Id = "brand_5",
                    Name = "品牌矩阵",
                    Description = "解锁 5 个品牌授权",
                    Type = AchievementType.BrandUnlock,
                    RequiredValue = 5,
                    RewardMoney = 5000,
                    RewardReputation = 20
                },

                // 粉丝成就
                new Achievement
                {
                    Id = "fans_100",
                    Name = "网络达人",
                    Description = "累计粉丝达到 100 人",
                    Type = AchievementType.Fans,
                    RequiredValue = 100,
                    RewardMoney = 500,
                    RewardReputation = 5
                },
                new Achievement
                {
                    Id = "fans_500",
                    Name = "网红车店",
                    Description = "累计粉丝达到 500 人",
                    Type = AchievementType.Fans,
                    RequiredValue = 500,
                    RewardMoney = 2000,
                    RewardReputation = 15
                }
            };
        }

        /// <summary>
        /// 获取所有成就状态
        /// </summary>
        public List<AchievementStatus> GetAchievementStatuses()
        {
            var statuses = new List<AchievementStatus>();
            foreach (var achievement in achievements)
            {
                statuses.Add(new AchievementStatus
                {
                    Achievement = achievement,
                    IsUnlocked = unlockedAchievements.Contains(achievement.Id),
                    Progress = GetProgress(achievement)
                });
            }
            return statuses;
        }

        /// <summary>
        /// 获取成就进度
        /// </summary>
        private float GetProgress(Achievement achievement)
        {
            int current = achievement.Type switch
            {
                AchievementType.Money => playerData.Money,
                AchievementType.Reputation => playerData.Reputation,
                AchievementType.Day => playerData.Day,
                AchievementType.BrandUnlock => playerData.UnlockedBrands.Count,
                AchievementType.Fans => playerData.FansCount,
                _ => 0
            };

            return Mathf.Min(1f, (float)current / achievement.RequiredValue);
        }
    }

    /// <summary>
    /// 成就定义
    /// </summary>
    public class Achievement
    {
        public string Id;
        public string Name;
        public string Description;
        public AchievementType Type;
        public int RequiredValue;
        public int RewardMoney;
        public int RewardReputation;
    }

    /// <summary>
    /// 成就类型
    /// </summary>
    public enum AchievementType
    {
        Money,
        Reputation,
        Day,
        BrandUnlock,
        Fans
    }

    /// <summary>
    /// 成就状态
    /// </summary>
    public class AchievementStatus
    {
        public Achievement Achievement;
        public bool IsUnlocked;
        public float Progress;
    }
}