using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using BikeShopTycoon.Core;
using BikeShopTycoon.GameSystems;
using BikeShopTycoon.UI;

namespace BikeShopTycoon.Editor
{
    /// <summary>
    /// 场景设置工具 - 编辑器扩展
    /// </summary>
    public static class SceneSetupTool
    {
        [MenuItem("BikeShop/设置场景/完整设置", false, 1)]
        public static void SetupScene()
        {
            // 创建必要的管理器对象
            CreateManagers();

            // 创建 Canvas 和 UI
            CreateUI();

            // 创建光照
            CreateLighting();

            // 创建相机
            CreateCamera();

            Debug.Log("场景设置完成！按 Play 运行游戏。");
        }

        [MenuItem("BikeShop/设置场景/仅创建管理器", false, 2)]
        public static void CreateManagers()
        {
            // GameManager
            if (Object.FindObjectOfType<GameManager>() == null)
            {
                var gmObj = new GameObject("[GameManager]");
                gmObj.AddComponent<GameManager>();
                Undo.RegisterCreatedObjectUndo(gmObj, "Create GameManager");
            }

            // TimeManager
            if (Object.FindObjectOfType<TimeManager>() == null)
            {
                var tmObj = new GameObject("[TimeManager]");
                tmObj.AddComponent<TimeManager>();
                Undo.RegisterCreatedObjectUndo(tmObj, "Create TimeManager");
            }

            // SettingsManager
            if (Object.FindObjectOfType<SettingsManager>() == null)
            {
                var smObj = new GameObject("[SettingsManager]");
                smObj.AddComponent<SettingsManager>();
                Undo.RegisterCreatedObjectUndo(smObj, "Create SettingsManager");
            }

            // AudioManager
            if (Object.FindObjectOfType<AudioManager>() == null)
            {
                var amObj = new GameObject("[AudioManager]");
                amObj.AddComponent<AudioManager>();
                Undo.RegisterCreatedObjectUndo(amObj, "Create AudioManager");
            }

            // GameInitializer
            if (Object.FindObjectOfType<GameInitializer>() == null)
            {
                var giObj = new GameObject("[GameInitializer]");
                giObj.AddComponent<GameInitializer>();
                Undo.RegisterCreatedObjectUndo(giObj, "Create GameInitializer");
            }

            // EventSystem
            if (Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                var esObj = new GameObject("EventSystem");
                esObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
                esObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                Undo.RegisterCreatedObjectUndo(esObj, "Create EventSystem");
            }

            Debug.Log("管理器创建完成！");
        }

        [MenuItem("BikeShop/设置场景/创建 UI", false, 3)]
        public static void CreateUI()
        {
            // 查找或创建 Canvas
            var canvas = Object.FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                var canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                var scaler = canvasObj.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
                scaler.matchWidthOrHeight = 0.5f;

                canvasObj.AddComponent<GraphicRaycaster>();
                Undo.RegisterCreatedObjectUndo(canvasObj, "Create Canvas");
            }

            // 如果有 GameInitializer，它会自动创建 UI
            var initializer = Object.FindObjectOfType<GameInitializer>();
            if (initializer != null)
            {
                Debug.Log("GameInitializer 将自动创建 UI");
            }
            else
            {
                // 创建 GameInitializer
                var giObj = new GameObject("[GameInitializer]");
                giObj.AddComponent<GameInitializer>();
                Undo.RegisterCreatedObjectUndo(giObj, "Create GameInitializer");
            }

            Debug.Log("UI 设置完成！");
        }

        [MenuItem("BikeShop/设置场景/创建相机和光照", false, 4)]
        public static void CreateCameraAndLighting()
        {
            CreateCamera();
            CreateLighting();
        }

        private static void CreateCamera()
        {
            if (Camera.main == null)
            {
                var camObj = new GameObject("Main Camera");
                camObj.tag = "MainCamera";
                var cam = camObj.AddComponent<Camera>();
                cam.orthographic = true;
                cam.orthographicSize = 5;
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = new Color(0.1f, 0.1f, 0.15f);

                camObj.AddComponent<AudioListener>();
                Undo.RegisterCreatedObjectUndo(camObj, "Create Camera");

                Debug.Log("相机创建完成！");
            }
        }

        private static void CreateLighting()
        {
            // 2D 游戏不需要复杂光照，但可以创建一个方向光
            if (Object.FindObjectOfType<Light>() == null)
            {
                var lightObj = new GameObject("Directional Light");
                var light = lightObj.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1f;
                Undo.RegisterCreatedObjectUndo(lightObj, "Create Light");

                Debug.Log("光照创建完成！");
            }
        }

        [MenuItem("BikeShop/创建/商品数据 (ScriptableObject)", false, 10)]
        public static void CreateProductData()
        {
            var folderPath = "Assets/ScriptableObjects/Products";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Products");
            }

            // 创建入门公路车
            var entryBike = ScriptableObject.CreateInstance<BikeProduct>();
            entryBike.productId = "bike_entry_001";
            entryBike.productName = "捷安特 Escape 3";
            entryBike.description = "入门级铝合金公路车，适合新手通勤";
            entryBike.baseCost = 1800;
            entryBike.basePrice = 2500;
            entryBike.reputationGain = 1;
            entryBike.qualityLevel = 1;
            entryBike.bikeType = BikeType.RoadBike;

            AssetDatabase.CreateAsset(entryBike, $"{folderPath}/EntryBike_Giant_Escape3.asset");

            // 创建中端公路车
            var midBike = ScriptableObject.CreateInstance<BikeProduct>();
            midBike.productId = "bike_mid_001";
            midBike.productName = "捷安特 TCR Advanced 2";
            midBike.description = "碳纤维公路车，适合训练和业余比赛";
            midBike.baseCost = 12000;
            midBike.basePrice = 15800;
            midBike.reputationGain = 3;
            midBike.qualityLevel = 3;
            midBike.bikeType = BikeType.RoadBike;

            AssetDatabase.CreateAsset(midBike, $"{folderPath}/MidBike_Giant_TCR_Advanced2.asset");

            // 创建高端公路车
            var highBike = ScriptableObject.CreateInstance<BikeProduct>();
            highBike.productId = "bike_high_001";
            highBike.productName = "捷安特 Propel Advanced SL";
            highBike.description = "顶级气动公路车，职业车手的选择";
            highBike.baseCost = 45000;
            highBike.basePrice = 58000;
            highBike.reputationGain = 5;
            highBike.qualityLevel = 5;
            highBike.bikeType = BikeType.RoadBike;

            AssetDatabase.CreateAsset(highBike, $"{folderPath}/HighBike_Giant_Propel_SL.asset");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("商品数据创建完成！路径: " + folderPath);
        }

        [MenuItem("BikeShop/创建/品牌数据 (ScriptableObject)", false, 11)]
        public static void CreateBrandData()
        {
            var folderPath = "Assets/ScriptableObjects/Brands";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Brands");
            }

            // 捷安特
            var giant = ScriptableObject.CreateInstance<BikeBrand>();
            giant.brandId = "brand_giant";
            giant.brandName = "Giant (捷安特)";
            giant.description = "台湾自行车品牌，全球最大的自行车制造商";
            giant.baseReputation = 10;
            giant.qualityMultiplier = 1.0f;
            giant.priceMultiplier = 1.0f;

            AssetDatabase.CreateAsset(giant, $"{folderPath}/Brand_Giant.asset");

            // Specialized
            var specialized = ScriptableObject.CreateInstance<BikeBrand>();
            specialized.brandId = "brand_specialized";
            specialized.brandName = "Specialized (闪电)";
            specialized.description = "美国高端自行车品牌，环法冠军战车";
            specialized.baseReputation = 20;
            specialized.qualityMultiplier = 1.1f;
            specialized.priceMultiplier = 1.2f;
            specialized.unlockCost = 5000;

            AssetDatabase.CreateAsset(specialized, $"{folderPath}/Brand_Specialized.asset");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("品牌数据创建完成！路径: " + folderPath);
        }

        [MenuItem("BikeShop/帮助/打开文档", false, 100)]
        public static void OpenDocumentation()
        {
            Application.OpenURL("https://github.com/N5ssstil/bike-shop-tycoon");
        }

        [MenuItem("BikeShop/帮助/检查项目状态", false, 101)]
        public static void CheckProjectStatus()
        {
            Debug.Log("=== 项目状态检查 ===");
            Debug.Log($"GameManager: {(Object.FindObjectOfType<GameManager>() != null ? "✅ 存在" : "❌ 缺失")}");
            Debug.Log($"TimeManager: {(Object.FindObjectOfType<TimeManager>() != null ? "✅ 存在" : "❌ 缺失")}");
            Debug.Log($"SettingsManager: {(Object.FindObjectOfType<SettingsManager>() != null ? "✅ 存在" : "❌ 缺失")}");
            Debug.Log($"AudioManager: {(Object.FindObjectOfType<AudioManager>() != null ? "✅ 存在" : "❌ 缺失")}");
            Debug.Log($"Canvas: {(Object.FindObjectOfType<Canvas>() != null ? "✅ 存在" : "❌ 缺失")}");
            Debug.Log($"EventSystem: {(Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() != null ? "✅ 存在" : "❌ 缺失")}");
            Debug.Log($"Camera: {(Camera.main != null ? "✅ 存在" : "❌ 缺失")}");
            Debug.Log("===================");
        }
    }
}