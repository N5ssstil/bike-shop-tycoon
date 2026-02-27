using UnityEngine;
using System.Collections.Generic;

namespace BikeShop.Data
{
    /// <summary>
    /// 顾客故事模板
    /// </summary>
    [CreateAssetMenu(fileName = "NewStory", menuName = "BikeShop/Customer Story")]
    public class CustomerStory : ScriptableObject
    {
        [Header("顾客画像")]
        public string storyId;
        public string customerType;
        [TextArea(3, 6)] public string storyText;

        [Header("偏好")]
        public string preferredBikeType;
        public string preferredColor;
        public string preferredBrand;
        public int maxBudget;
        public int minBudget;

        [Header="匹配规则"]
        public float colorMatchWeight = 0.3f;
        public float brandMatchWeight = 0.2f;
        public float typeMatchWeight = 0.3f;
        public float priceMatchWeight = 0.2f;

        [Header("对话")]
        public List<string> openingDialogues = new List<string>();
        public List<string> successDialogues = new List<string>();
        public List<string> rejectDialogues = new List<string>();

        [Header("特殊属性")]
        public bool isInfluencer;
        public int trafficBoostAmount = 0;
    }
}