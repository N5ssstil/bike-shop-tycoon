using UnityEngine;
using System.IO;
using System;

namespace BikeShopTycoon.Core
{
    /// <summary>
    /// 存档系统 - 支持备份和版本控制
    /// </summary>
    public static class SaveSystem
    {
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "save.json");
        private static readonly string BackupPath = Path.Combine(Application.persistentDataPath, "save_backup.json");
        
        // 存档版本号，用于兼容性检查
        private const int SAVE_VERSION = 1;

        /// <summary>
        /// 存档加载结果
        /// </summary>
        public class LoadResult
        {
            public bool Success;
            public PlayerData Data;
            public string ErrorMessage;
            public bool WasRestoredFromBackup;
        }

        public static bool HasSave()
        {
            return File.Exists(SavePath);
        }

        public static void SaveGame(PlayerData data)
        {
            try
            {
                // 确保目录存在
                Directory.CreateDirectory(Application.persistentDataPath);
                
                // 先备份旧存档
                if (File.Exists(SavePath))
                {
                    CreateBackup();
                }

                // 设置存档版本
                data.SaveVersion = SAVE_VERSION;
                data.LastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(SavePath, json);
                Debug.Log($"游戏已保存到: {SavePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"保存失败: {e.Message}");
                throw;
            }
        }

        public static PlayerData LoadGame()
        {
            var result = LoadGameWithResult();
            return result.Data;
        }

        /// <summary>
        /// 加载存档并返回详细结果
        /// </summary>
        public static LoadResult LoadGameWithResult()
        {
            var result = new LoadResult
            {
                Success = false,
                Data = new PlayerData(),
                ErrorMessage = null,
                WasRestoredFromBackup = false
            };

            // 尝试加载主存档
            if (File.Exists(SavePath))
            {
                try
                {
                    string json = File.ReadAllText(SavePath);
                    var data = JsonUtility.FromJson<PlayerData>(json);
                    
                    // 版本兼容性检查
                    if (data.SaveVersion > SAVE_VERSION)
                    {
                        result.ErrorMessage = "存档版本过高，请更新游戏版本";
                        Debug.LogWarning(result.ErrorMessage);
                        return result;
                    }

                    // 数据完整性检查
                    if (ValidateData(data))
                    {
                        // 数据验证通过后，进行修复
                        data.ValidateAndRepair();
                        result.Success = true;
                        result.Data = data;
                        return result;
                    }
                    else
                    {
                        result.ErrorMessage = "存档数据不完整";
                        Debug.LogWarning(result.ErrorMessage);
                    }
                }
                catch (Exception e)
                {
                    result.ErrorMessage = $"加载存档失败: {e.Message}";
                    Debug.LogError(result.ErrorMessage);
                }
            }

            // 主存档加载失败，尝试备份
            if (File.Exists(BackupPath))
            {
                Debug.LogWarning("主存档损坏，尝试从备份恢复...");
                try
                {
                    string json = File.ReadAllText(BackupPath);
                    var data = JsonUtility.FromJson<PlayerData>(json);
                    
                    if (ValidateData(data))
                    {
                        // 数据验证通过后，进行修复
                        data.ValidateAndRepair();
                        result.Success = true;
                        result.Data = data;
                        result.WasRestoredFromBackup = true;
                        Debug.Log("已从备份恢复存档");
                        return result;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"备份恢复失败: {e.Message}");
                    result.ErrorMessage = "存档和备份均损坏";
                }
            }

            // 都失败了，返回新数据
            Debug.LogWarning("无法加载存档，将开始新游戏");
            return result;
        }

        private static void CreateBackup()
        {
            try
            {
                File.Copy(SavePath, BackupPath, true);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"创建备份失败: {e.Message}");
            }
        }

        /// <summary>
        /// 数据完整性验证
        /// </summary>
        private static bool ValidateData(PlayerData data)
        {
            if (data == null) return false;
            
            // 检查关键数据是否有效
            if (data.UnlockedBrands == null) data.UnlockedBrands = new System.Collections.Generic.List<string>();
            if (data.UnlockedItems == null) data.UnlockedItems = new System.Collections.Generic.List<string>();
            if (data.CompletedMilestones == null) data.CompletedMilestones = new System.Collections.Generic.List<string>();
            if (data.CustomerFriends == null) data.CustomerFriends = new System.Collections.Generic.List<string>();
            
            // 基础数据范围检查
            if (data.Money < 0)
            {
                Debug.LogWarning("存档中金钱为负数，已修正为0");
                data.Money = 0;
            }
            
            if (data.Reputation < 0)
            {
                Debug.LogWarning("存档中口碑为负数，已修正为0");
                data.Reputation = 0;
            }

            return true;
        }

        public static void DeleteSave()
        {
            try
            {
                if (File.Exists(SavePath))
                {
                    File.Delete(SavePath);
                }
                if (File.Exists(BackupPath))
                {
                    File.Delete(BackupPath);
                }
                Debug.Log("存档已删除");
            }
            catch (Exception e)
            {
                Debug.LogError($"删除存档失败: {e.Message}");
            }
        }

        /// <summary>
        /// 检查是否存在有效存档
        /// </summary>
        public static bool HasValidSave()
        {
            if (!HasSave()) return false;
            
            var result = LoadGameWithResult();
            return result.Success;
        }
    }
}