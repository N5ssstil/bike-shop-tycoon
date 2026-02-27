using UnityEngine;

namespace BikeShopTycoon.Core
{
    /// <summary>
    /// 游戏主管理器 - 单例模式
    /// 负责游戏状态管理、场景切换、全局事件
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("游戏状态")]
        public GameState CurrentState;

        [Header("玩家数据")]
        public PlayerData PlayerData;

        // 常量定义
        private const int MIN_MONEY = 0;
        private const int MAX_MONEY = int.MaxValue;
        private const int MIN_REPUTATION = 0;
        private const int MAX_REPUTATION = 1000;

        // 全局事件
        public event System.Action<GameState> OnGameStateChanged;
        public event System.Action<int> OnMoneyChanged;
        public event System.Action<int> OnReputationChanged;
        
        /// <summary>
        /// 资金不足事件
        /// </summary>
        public event System.Action<int> OnMoneyInsufficient;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeGame();
        }

        private void InitializeGame()
        {
            PlayerData = new PlayerData();
            CurrentState = GameState.Shop;

            // 加载存档或初始化新游戏
            if (SaveSystem.HasSave())
            {
                var loadResult = SaveSystem.LoadGameWithResult();
                PlayerData = loadResult.Data;
                
                // 如果是从备份恢复，记录日志（UI层可订阅事件处理）
                if (loadResult.WasRestoredFromBackup)
                {
                    Debug.LogWarning("存档已从备份恢复");
                }
                
                if (!loadResult.Success && !string.IsNullOrEmpty(loadResult.ErrorMessage))
                {
                    Debug.LogError($"存档加载失败: {loadResult.ErrorMessage}");
                }
            }
        }

        public void ChangeState(GameState newState)
        {
            if (CurrentState == newState) return;
            
            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);
        }

        /// <summary>
        /// 修改资金
        /// </summary>
        /// <param name="amount">金额变化量（正数为增加，负数为减少）</param>
        /// <returns>是否成功</returns>
        public bool AddMoney(int amount)
        {
            int newMoney = PlayerData.Money + amount;
            
            // 边界检查
            if (newMoney < MIN_MONEY)
            {
                OnMoneyInsufficient?.Invoke(-amount);
                Debug.LogWarning($"资金不足: 当前 {PlayerData.Money}, 需要 {-amount}");
                return false;
            }
            
            if (newMoney > MAX_MONEY)
            {
                newMoney = MAX_MONEY;
            }
            
            PlayerData.Money = newMoney;
            OnMoneyChanged?.Invoke(PlayerData.Money);
            return true;
        }

        /// <summary>
        /// 尝试消费
        /// </summary>
        /// <param name="cost">消费金额</param>
        /// <returns>是否成功</returns>
        public bool TrySpendMoney(int cost)
        {
            if (cost < 0)
            {
                Debug.LogError("消费金额不能为负数");
                return false;
            }
            
            if (PlayerData.Money < cost)
            {
                OnMoneyInsufficient?.Invoke(cost);
                return false;
            }
            
            return AddMoney(-cost);
        }

        /// <summary>
        /// 修改口碑
        /// </summary>
        /// <param name="amount">口碑变化量</param>
        public void AddReputation(int amount)
        {
            int newReputation = PlayerData.Reputation + amount;
            
            // 边界检查
            newReputation = Mathf.Clamp(newReputation, MIN_REPUTATION, MAX_REPUTATION);
            
            PlayerData.Reputation = newReputation;
            OnReputationChanged?.Invoke(PlayerData.Reputation);
        }

        /// <summary>
        /// 检查是否有足够资金
        /// </summary>
        public bool CanAfford(int cost)
        {
            return PlayerData.Money >= cost && cost >= 0;
        }

        public void SaveGame()
        {
            SaveSystem.SaveGame(PlayerData);
        }

        /// <summary>
        /// 开始新游戏
        /// </summary>
        public void StartNewGame()
        {
            SaveSystem.DeleteSave();
            PlayerData = new PlayerData();
            CurrentState = GameState.Shop;
            OnGameStateChanged?.Invoke(CurrentState);
            OnMoneyChanged?.Invoke(PlayerData.Money);
            OnReputationChanged?.Invoke(PlayerData.Reputation);
        }
    }

    public enum GameState
    {
        Shop,           // 店铺经营
        Inventory,      // 库存管理
        Customer,       // 顾客接待
        Workshop,       // 维修/改装
        Events          // 活动/赛事
    }
}