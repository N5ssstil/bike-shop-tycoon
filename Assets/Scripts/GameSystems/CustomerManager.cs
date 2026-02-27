using System;
using System.Collections.Generic;
using BikeShopTycoon.Data;
using BikeShopTycoon.Core;

namespace BikeShopTycoon.GameSystems
{
    /// <summary>
    /// 顾客管理系统
    /// </summary>
    public class CustomerManager
    {
        private CustomerGeneratorSettings settings;
        private List<Customer> activeCustomers;
        private List<CustomerStory> storyDatabase;
        private PlayerData playerData;

        public event Action<Customer> OnCustomerEnter;
        public event Action<Customer> OnCustomerLeave;
        public event Action<Customer, bool> OnTransactionComplete;
        public event Action<CustomerStory> OnStoryRevealed;

        public int ActiveCustomerCount => activeCustomers.Count;

        public CustomerManager(PlayerData playerData, CustomerGeneratorSettings settings = null)
        {
            this.playerData = playerData;
            this.settings = settings ?? new CustomerGeneratorSettings();
            this.activeCustomers = new List<Customer>();
            this.storyDatabase = new List<CustomerStory>();
            
            LoadStories();
        }

        /// <summary>
        /// 生成新顾客
        /// </summary>
        public Customer GenerateCustomer()
        {
            var customer = new Customer
            {
                Name = GenerateName(),
                Type = GetRandomCustomerType(),
                Budget = 0,
                Needs = new CustomerNeeds()
            };

            // 根据顾客类型设置需求和预算
            ConfigureCustomerByType(customer);

            // 随机分配故事
            TryAssignStory(customer);

            activeCustomers.Add(customer);
            customer.State = CustomerState.Entering;
            OnCustomerEnter?.Invoke(customer);

            return customer;
        }

        /// <summary>
        /// 处理顾客交互
        /// </summary>
        public void InteractWithCustomer(Customer customer, string dialogueChoice)
        {
            customer.State = CustomerState.Asking;
            
            // 根据选择更新顾客状态
            if (dialogueChoice == "recommend")
            {
                customer.State = CustomerState.Deciding;
            }
            else if (dialogueChoice == "repair")
            {
                customer.State = CustomerState.Deciding;
            }
        }

        /// <summary>
        /// 推荐商品给顾客
        /// </summary>
        public RecommendationResult RecommendItem(Customer customer, Item item)
        {
            var result = new RecommendationResult
            {
                Item = item,
                MatchScore = CalculateMatchScore(customer, item)
            };

            // 检查预算
            if (item.SellPrice > customer.Budget)
            {
                result.IsOverBudget = true;
                result.BudgetDifference = item.SellPrice - customer.Budget;
                customer.Satisfaction -= 10;
            }

            // 根据匹配度更新满意度
            if (result.MatchScore >= 80)
            {
                customer.Satisfaction += 20;
                result.Feedback = "太棒了，正是我想要的！";
            }
            else if (result.MatchScore >= 50)
            {
                customer.Satisfaction += 10;
                result.Feedback = "还不错，可以考虑。";
            }
            else
            {
                customer.Satisfaction -= 15;
                result.Feedback = "这好像不太适合我...";
            }

            return result;
        }

        /// <summary>
        /// 完成交易
        /// </summary>
        public bool CompleteTransaction(Customer customer, Item item, bool isRepair = false)
        {
            if (customer.State != CustomerState.Purchasing)
                return false;

            // 更新顾客状态
            customer.State = CustomerState.Satisfied;
            customer.Satisfaction += 10;

            // 触发故事（如果有）
            if (!string.IsNullOrEmpty(customer.StoryId) && !customer.StoryRevealed)
            {
                var story = storyDatabase.Find(s => s.Id == customer.StoryId);
                if (story != null)
                {
                    customer.StoryRevealed = true;
                    playerData.Reputation += story.ReputationBonus;
                    OnStoryRevealed?.Invoke(story);
                }
            }

            // 检查是否是网红顾客
            if (customer.Type == CustomerType.Influencer)
            {
                playerData.FansCount += 50;
                // TODO: 触发客流增加效果
            }

            activeCustomers.Remove(customer);
            OnTransactionComplete?.Invoke(customer, true);
            OnCustomerLeave?.Invoke(customer);

            return true;
        }

        /// <summary>
        /// 顾客离开（未购买）
        /// </summary>
        public void CustomerLeaveUnsatisfied(Customer customer)
        {
            customer.State = CustomerState.Unsatisfied;
            playerData.Reputation -= 5;
            
            activeCustomers.Remove(customer);
            OnTransactionComplete?.Invoke(customer, false);
            OnCustomerLeave?.Invoke(customer);
        }

        /// <summary>
        /// 计算商品与顾客需求的匹配度
        /// </summary>
        private int CalculateMatchScore(Customer customer, Item item)
        {
            int score = 50; // 基础分

            // 检查用途匹配
            if (item is Bike bike)
            {
                if (customer.Needs.NeedForRacing && bike.IsForRacing) score += 20;
                if (customer.Needs.NeedForCommuting && bike.IsForCommuting) score += 15;
                if (customer.Needs.NeedForBeginners && bike.IsForBeginners) score += 15;
            }

            // 检查等级偏好
            if (item.Tier == customer.Needs.PreferredTier) score += 15;

            // 检查品牌偏好
            if (!string.IsNullOrEmpty(customer.Needs.PreferredBrand) && 
                item.Brand == customer.Needs.PreferredBrand) score += 10;

            // 检查材质偏好
            if (customer.Needs.PreferredMaterial.HasValue && 
                item is Bike bikeItem && bikeItem.FrameMaterial == customer.Needs.PreferredMaterial)
                score += 10;

            return Math.Min(100, Math.Max(0, score));
        }

        private CustomerType GetRandomCustomerType()
        {
            int totalWeight = settings.StudentWeight + settings.CommuterWeight + 
                              settings.EnthusiastWeight + settings.RacerWeight + settings.InfluencerWeight;
            
            int roll = UnityEngine.Random.Range(0, totalWeight);
            
            if (roll < settings.StudentWeight) return CustomerType.Student;
            if (roll < settings.StudentWeight + settings.CommuterWeight) return CustomerType.Commuter;
            if (roll < settings.StudentWeight + settings.CommuterWeight + settings.EnthusiastWeight) 
                return CustomerType.CyclingEnthusiast;
            if (roll < settings.StudentWeight + settings.CommuterWeight + settings.EnthusiastWeight + settings.RacerWeight)
                return CustomerType.Racer;
            return CustomerType.Influencer;
        }

        private void ConfigureCustomerByType(Customer customer)
        {
            switch (customer.Type)
            {
                case CustomerType.Student:
                    customer.Budget = UnityEngine.Random.Range(1500, 5000);
                    customer.Needs.NeedForBeginners = true;
                    customer.Needs.PreferredTier = ItemTier.Entry;
                    break;
                case CustomerType.Commuter:
                    customer.Budget = UnityEngine.Random.Range(2000, 8000);
                    customer.Needs.NeedForCommuting = true;
                    customer.Needs.PreferredMaterial = FrameMaterial.Aluminum;
                    break;
                case CustomerType.CyclingEnthusiast:
                    customer.Budget = UnityEngine.Random.Range(5000, 30000);
                    customer.Needs.NeedForTraining = true;
                    customer.Needs.PreferredTier = ItemTier.Mid;
                    break;
                case CustomerType.Racer:
                    customer.Budget = UnityEngine.Random.Range(20000, 100000);
                    customer.Needs.NeedForRacing = true;
                    customer.Needs.PreferredTier = ItemTier.Pro;
                    break;
                case CustomerType.Influencer:
                    customer.Budget = UnityEngine.Random.Range(8000, 50000);
                    customer.Needs.NeedHighVisual = true;
                    customer.Needs.PreferredColor = "白色"; // TODO: 随机颜色
                    break;
            }
        }

        private void TryAssignStory(Customer customer)
        {
            // 30% 概率分配故事
            if (UnityEngine.Random.value > 0.3f) return;

            var availableStories = storyDatabase.FindAll(s => s.TargetCustomerType == customer.Type);
            if (availableStories.Count > 0)
            {
                var story = availableStories[UnityEngine.Random.Range(0, availableStories.Count)];
                customer.StoryId = story.Id;
            }
        }

        private string GenerateName()
        {
            string[] firstNames = { "张", "李", "王", "刘", "陈", "杨", "赵", "黄", "周", "吴" };
            string[] lastNames = { "伟", "芳", "娜", "秀英", "敏", "静", "丽", "强", "磊", "军" };
            return firstNames[UnityEngine.Random.Range(0, firstNames.Length)] + 
                   lastNames[UnityEngine.Random.Range(0, lastNames.Length)];
        }

        private void LoadStories()
        {
            // 第一阶段基础故事
            storyDatabase.Add(new CustomerStory
            {
                Id = "story_student_001",
                Title = "校园赛的梦想",
                Description = "一名学生攒了很久的钱，想买第一台公路车备战校园赛",
                TargetCustomerType = CustomerType.Student,
                ReputationBonus = 5
            });
            storyDatabase.Add(new CustomerStory
            {
                Id = "story_commuter_001",
                Title = "为了多陪孩子",
                Description = "一位通勤族想换一辆更轻便的车，每天能多陪孩子半小时",
                TargetCustomerType = CustomerType.Commuter,
                ReputationBonus = 3
            });
            storyDatabase.Add(new CustomerStory
            {
                Id = "story_enthusiast_001",
                Title = "骑游的故事",
                Description = "一位骑行爱好者来修车，分享了他的川藏线骑行经历",
                TargetCustomerType = CustomerType.CyclingEnthusiast,
                ReputationBonus = 4
            });
        }
    }

    public class RecommendationResult
    {
        public Item Item;
        public int MatchScore;
        public bool IsOverBudget;
        public int BudgetDifference;
        public string Feedback;
    }
}