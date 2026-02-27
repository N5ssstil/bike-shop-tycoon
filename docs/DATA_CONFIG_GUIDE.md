# 数据配置创建指南

## 创建步骤

### 1. 创建品牌 (Brand)

在 Unity 编辑器中：
1. 右键 Project 窗口 → Create → BikeShop → Brand
2. 命名：`LocalBrand_Local`
3. 配置示例：
```
Brand ID: local_001
Brand Name: 本地品牌
Description: 本地小型自行车制造商，性价比高
Base Reputation: 5
Unlock Cost: 0
Quality Multiplier: 0.8
Price Multiplier: 0.7
Target Customer Types: 通勤族, 学生
```

### 2. 创建商品 (Product)

**入门公路车：**
```
Product ID: road_001
Product Name: 城市行者 C1
Description: 适合城市通勤的入门公路车
Brand: LocalBrand
Base Cost: 2000
Base Price: 3500
Reputation Gain: 3
Quality Level: 1
Bike Type: RoadBike
Available Colors: 白色, 黑色, 红色
Preferred Customer Type: 通勤族
```

**中端公路车：**
```
Product ID: road_002
Product Name: 极速 R200
Description: 轻量化铝合金车架，适合运动骑行
Base Cost: 5000
Base Price: 8000
Reputation Gain: 5
Quality Level: 2
Bike Type: RoadBike
Available Colors: 蓝色, 黑色, 银色
Preferred Customer Type: 运动族
```

### 3. 创建顾客故事 (Customer Story)

**通勤族故事：**
```
Story ID: story_commuter_001
Customer Type: 通勤族
Story Text: "我每天上下班都要骑15公里，需要一辆舒适耐用的车..."
Preferred Bike Type: RoadBike
Preferred Color: 黑色
Preferred Brand: LocalBrand
Max Budget: 5000
Min Budget: 2000
```

**网红顾客：**
```
Story ID: story_influencer_001
Customer Type: 网红
Story Text: "我是骑行博主，想要一辆有格调的车来拍摄视频..."
Preferred Bike Type: RoadBike
Preferred Color: 白色
Preferred Brand: Trek
Max Budget: 15000
Min Budget: 8000
Is Influencer: true
Traffic Boost Amount: 10
```

## 文件组织

```
Assets/ScriptableObjects/
├── Brands/
│   ├── LocalBrand.asset
│   ├── TrekBrand.asset
│   └── GiantBrand.asset
├── Products/
│   ├── RoadBike_Entry.asset
│   ├── RoadBike_Mid.asset
│   └── RoadBike_Pro.asset
└── CustomerStories/
    ├── Commuter_001.asset
    ├── Athlete_001.asset
    └── Influencer_001.asset
```