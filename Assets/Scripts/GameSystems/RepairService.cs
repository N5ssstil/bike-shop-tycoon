using System;
using BikeShopTycoon.Data;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.GameSystems
{
    /// <summary>
    /// 维修服务系统
    /// </summary>
    public class RepairService
    {
        private PlayerData playerData;

        public event Action<RepairJob> OnRepairComplete;
        public event Action<int> OnReputationGained;

        public RepairService(PlayerData playerData)
        {
            this.playerData = playerData;
        }

        /// <summary>
        /// 创建维修工单
        /// </summary>
        public RepairJob CreateJob(RepairType type, Customer customer)
        {
            return new RepairJob
            {
                Id = Guid.NewGuid().ToString(),
                Type = type,
                Customer = customer,
                Status = RepairStatus.Pending,
                CreatedTime = DateTime.Now,
                BaseCost = GetRepairCost(type),
                EstimatedMinutes = GetRepairTime(type),
                RequiredTools = GetRequiredTools(type)
            };
        }

        /// <summary>
        /// 执行维修
        /// </summary>
        public RepairResult ExecuteRepair(RepairJob job)
        {
            var result = new RepairResult
            {
                Job = job,
                Success = true
            };

            // 计算收益
            int baseIncome = job.BaseCost;
            int reputationGain = GetReputationGain(job.Type);

            // 店铺有维修区时，效率提升
            if (playerData.HasWorkshop)
            {
                baseIncome = (int)(baseIncome * 1.2f);
                reputationGain += 1;
            }

            result.Income = baseIncome;
            result.ReputationGain = reputationGain;

            // 更新玩家数据
            playerData.Money += baseIncome;
            playerData.Reputation += reputationGain;

            job.Status = RepairStatus.Completed;
            job.CompletedTime = DateTime.Now;

            OnRepairComplete?.Invoke(job);
            OnReputationGained?.Invoke(reputationGain);

            return result;
        }

        /// <summary>
        /// 获取维修费用
        /// </summary>
        public int GetRepairCost(RepairType type)
        {
            return type switch
            {
                RepairType.FlatTire => 50,          // 爆胎
                RepairType.GearAdjustment => 80,    // 变速调试
                RepairType.BrakeService => 60,      // 刹车保养
                RepairType.WheelTruing => 150,      // 编圈调整
                RepairType.FullService => 300,      // 全车保养
                RepairType.CustomTuning => 500,     // 定制调校
                _ => 50
            };
        }

        /// <summary>
        /// 获取维修时间（游戏分钟）
        /// </summary>
        public int GetRepairTime(RepairType type)
        {
            return type switch
            {
                RepairType.FlatTire => 15,
                RepairType.GearAdjustment => 30,
                RepairType.BrakeService => 20,
                RepairType.WheelTruing => 60,
                RepairType.FullService => 120,
                RepairType.CustomTuning => 90,
                _ => 30
            };
        }

        /// <summary>
        /// 获取所需工具
        /// </summary>
        public string[] GetRequiredTools(RepairType type)
        {
            return type switch
            {
                RepairType.FlatTire => new[] { "撬胎棒", "打气筒", "备用内胎" },
                RepairType.GearAdjustment => new[] { "内六角扳手", "螺丝刀", "变速调试架" },
                RepairType.BrakeService => new[] { "内六角扳手", "刹车油", "注油器" },
                RepairType.WheelTruing => new[] { "编圈台", "辐条扳手", "张力计" },
                RepairType.FullService => new[] { "全套工具", "润滑脂", "清洁剂" },
                RepairType.CustomTuning => new[] { "Fitting工具", "扭矩扳手", "专业测量仪" },
                _ => new[] { "基础工具包" }
            };
        }

        private int GetReputationGain(RepairType type)
        {
            return type switch
            {
                RepairType.FlatTire => 1,
                RepairType.GearAdjustment => 2,
                RepairType.BrakeService => 1,
                RepairType.WheelTruing => 3,
                RepairType.FullService => 4,
                RepairType.CustomTuning => 5,
                _ => 1
            };
        }
    }

    /// <summary>
    /// 维修工单
    /// </summary>
    public class RepairJob
    {
        public string Id;
        public RepairType Type;
        public Customer Customer;
        public RepairStatus Status;
        public DateTime CreatedTime;
        public DateTime? CompletedTime;
        public int BaseCost;
        public int EstimatedMinutes;
        public string[] RequiredTools;
    }

    public enum RepairStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }

    /// <summary>
    /// 维修结果
    /// </summary>
    public class RepairResult
    {
        public RepairJob Job;
        public bool Success;
        public int Income;
        public int ReputationGain;
        public string Message;
    }
}