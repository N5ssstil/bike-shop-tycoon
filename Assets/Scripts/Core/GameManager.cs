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

        // 全局事件
        public event System.Action<GameState> OnGameStateChanged;
        public event System.Action<int> OnMoneyChanged;
        public event System.Action<int> OnReputationChanged;

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
                PlayerData = SaveSystem.LoadGame();
            }
        }

        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);
        }

        public void AddMoney(int amount)
        {
            PlayerData.Money += amount;
            OnMoneyChanged?.Invoke(PlayerData.Money);
        }

        public void AddReputation(int amount)
        {
            PlayerData.Reputation += amount;
            OnReputationChanged?.Invoke(PlayerData.Reputation);
        }

        public void SaveGame()
        {
            SaveSystem.SaveGame(PlayerData);
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