using UnityEngine;
using System.IO;
using System;

namespace BikeShopTycoon.Core
{
    /// <summary>
    /// 存档系统
    /// </summary>
    public static class SaveSystem
    {
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "save.json");

        public static bool HasSave()
        {
            return File.Exists(SavePath);
        }

        public static void SaveGame(PlayerData data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(SavePath, json);
                Debug.Log($"游戏已保存到: {SavePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"保存失败: {e.Message}");
            }
        }

        public static PlayerData LoadGame()
        {
            try
            {
                if (File.Exists(SavePath))
                {
                    string json = File.ReadAllText(SavePath);
                    return JsonUtility.FromJson<PlayerData>(json);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"加载存档失败: {e.Message}");
            }
            return new PlayerData();
        }

        public static void DeleteSave()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
                Debug.Log("存档已删除");
            }
        }
    }
}