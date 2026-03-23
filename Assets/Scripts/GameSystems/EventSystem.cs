using System;
using System.Collections.Generic;
using BikeShopTycoon.Core;
using BikeShopTycoon.Data;

namespace BikeShopTycoon.GameSystems
{
    /// <summary>
    /// 游戏事件系统 - 管理随机事件、日常事件等
    /// </summary>
    public class EventSystem
    {
        private PlayerData playerData;
        private List<GameEvent> availableEvents;
        private List<GameEvent> activeEvents;
        private Random random;

        public event Action<GameEvent> OnEventTriggered;
        public event Action<GameEvent> OnEventCompleted;
        public event Action<GameEvent, EventChoice> OnEventChoiceMade;

        public EventSystem(PlayerData playerData)
        {
            this.playerData = playerData;
            this.availableEvents = new List<GameEvent>();
            this.activeEvents = new List<GameEvent>();
            this.random = new Random();

            LoadEvents();
        }

        /// <summary>
        /// 检查是否触发事件（每天调用）
        /// </summary>
        public void CheckForEvents(int day, int reputation)
        {
            // 基础事件触发概率
            double baseChance = 0.15; // 15% 基础概率

            // 口碑影响事件概率
            if (reputation > 100) baseChance += 0.05;
            if (reputation > 300) baseChance += 0.05;

            // 随机检查
            if (random.NextDouble() < baseChance)
            {
                var evt = GetRandomEvent(day, reputation);
                if (evt != null)
                {
                    TriggerEvent(evt);
                }
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        public void TriggerEvent(GameEvent evt)
        {
            activeEvents.Add(evt);
            OnEventTriggered?.Invoke(evt);
        }

        /// <summary>
        /// 选择事件选项
        /// </summary>
        public void MakeChoice(GameEvent evt, EventChoice choice)
        {
            if (!activeEvents.Contains(evt)) return;

            // 应用效果
            ApplyChoiceEffect(choice);

            // 移除事件
            activeEvents.Remove(evt);

            OnEventChoiceMade?.Invoke(evt, choice);
            OnEventCompleted?.Invoke(evt);
        }

        /// <summary>
        /// 获取随机事件
        /// </summary>
        private GameEvent GetRandomEvent(int day, int reputation)
        {
            var eligibleEvents = availableEvents.FindAll(e => 
                day >= e.MinDay && 
                reputation >= e.MinReputation &&
                (e.MaxReputation == 0 || reputation <= e.MaxReputation)
            );

            if (eligibleEvents.Count == 0) return null;

            return eligibleEvents[random.Next(eligibleEvents.Count)];
        }

        /// <summary>
        /// 应用选择效果
        /// </summary>
        private void ApplyChoiceEffect(EventChoice choice)
        {
            if (choice.MoneyChange != 0)
            {
                if (choice.MoneyChange > 0)
                    playerData.Money += choice.MoneyChange;
                else
                    playerData.Money = Math.Max(0, playerData.Money + choice.MoneyChange);
            }

            if (choice.ReputationChange != 0)
            {
                playerData.Reputation = Math.Max(0, Math.Min(1000, 
                    playerData.Reputation + choice.ReputationChange));
            }

            if (!string.IsNullOrEmpty(choice.UnlockBrand) && 
                !playerData.UnlockedBrands.Contains(choice.UnlockBrand))
            {
                playerData.UnlockedBrands.Add(choice.UnlockBrand);
            }
        }

        /// <summary>
        /// 加载事件库
        /// </summary>
        private void LoadEvents()
        {
            // 好运事件
            availableEvents.Add(new GameEvent
            {
                Id = "event_lucky_day",
                Title = "好运降临",
                Description = "一位老顾客介绍了一位富豪朋友来买车，他表示对高端车型很感兴趣！",
                MinDay = 1,
                MinReputation = 10,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "热情接待，推荐最顶级的车型",
                        MoneyChange = 5000,
                        ReputationChange = 10
                    },
                    new EventChoice
                    {
                        Text = "稳扎稳打，推荐中端车型",
                        MoneyChange = 2000,
                        ReputationChange = 5
                    }
                }
            });

            availableEvents.Add(new GameEvent
            {
                Id = "event_cycling_club",
                Title = "骑行俱乐部合作",
                Description = "本地骑行俱乐部希望与你的店铺建立长期合作关系，提供会员优惠。",
                MinDay = 5,
                MinReputation = 30,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "同意合作，提供10%折扣",
                        ReputationChange = 15,
                        UnlockBrand = "Specialized"
                    },
                    new EventChoice
                    {
                        Text = "婉拒，保持独立经营",
                        ReputationChange = -5
                    }
                }
            });

            // 挑战事件
            availableEvents.Add(new GameEvent
            {
                Id = "event_complaint",
                Title = "顾客投诉",
                Description = "有顾客投诉售后问题，声称之前买的自行车有质量问题，要求退款。",
                MinDay = 3,
                MinReputation = 10,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "全额退款，赠送小礼品",
                        MoneyChange = -500,
                        ReputationChange = 5
                    },
                    new EventChoice
                    {
                        Text = "提供免费维修服务",
                        MoneyChange = -100,
                        ReputationChange = 0
                    },
                    new EventChoice
                    {
                        Text = "拒绝退款，坚持售后政策",
                        ReputationChange = -15
                    }
                }
            });

            availableEvents.Add(new GameEvent
            {
                Id = "event_supplier_issue",
                Title = "供应商问题",
                Description = "主要供应商通知供货延迟，可能影响接下来的销售。",
                MinDay = 7,
                MinReputation = 20,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "寻找替代供应商（需要资金）",
                        MoneyChange = -1000,
                        ReputationChange = 5
                    },
                    new EventChoice
                    {
                        Text = "等待原供应商恢复",
                        ReputationChange = -5
                    }
                }
            });

            // 特殊事件
            availableEvents.Add(new GameEvent
            {
                Id = "event_race_winner",
                Title = "环法冠军光临",
                Description = "一位环法自行车赛冠军来到你的店里！他对你店铺的口碑早有耳闻。",
                MinDay = 30,
                MinReputation = 200,
                MaxReputation = 600,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "邀请他签名合影，发到社交媒体",
                        ReputationChange = 30,
                        MoneyChange = 3000
                    },
                    new EventChoice
                    {
                        Text = "低调服务，展现专业态度",
                        ReputationChange = 20,
                        UnlockBrand = "Pinarello"
                    }
                }
            });

            availableEvents.Add(new GameEvent
            {
                Id = "event_influencer",
                Title = "网红打卡",
                Description = "一位拥有百万粉丝的骑行网红来到店里，准备拍摄视频。",
                MinDay = 10,
                MinReputation = 50,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "全力配合，提供最好的展示",
                        ReputationChange = 25,
                        MoneyChange = 2000
                    },
                    new EventChoice
                    {
                        Text = "正常服务，不打扰拍摄",
                        ReputationChange = 10
                    }
                }
            });
        }
    }

    /// <summary>
    /// 游戏事件
    /// </summary>
    public class GameEvent
    {
        public string Id;
        public string Title;
        public string Description;
        public int MinDay;
        public int MinReputation;
        public int MaxReputation;  // 0 表示无上限
        public List<EventChoice> Choices;
        public EventType Type;

        public GameEvent()
        {
            Choices = new List<EventChoice>();
            Type = EventType.Random;
        }
    }

    /// <summary>
    /// 事件选项
    /// </summary>
    public class EventChoice
    {
        public string Text;
        public int MoneyChange;
        public int ReputationChange;
        public string UnlockBrand;
        public string UnlockItem;
    }

    /// <summary>
    /// 事件类型
    /// </summary>
    public enum EventType
    {
        Random,         // 随机事件
        Story,          // 剧情事件
        Milestone,      // 里程碑事件
        Challenge       // 挑战事件
    }
}