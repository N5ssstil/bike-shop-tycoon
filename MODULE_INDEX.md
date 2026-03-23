# 模块索引

## 模块列表

### 核心模块 (core/)
- GameManager.cs
- SaveSystem.cs
- PlayerData.cs
- InventoryManager.cs
- CustomerManager.cs
- RepairService.cs

### UI 模块 (ui/)
- UIManager.cs
- HUDController.cs
- UITheme.cs
- UIPlaceholderGenerator.cs
- UIPresetBuilder.cs
- UIPlaceholderTool.cs
- UI_DESIGN.md

### 数据配置模块 (data-config/)
- BikeBrand.cs
- BikeProduct.cs
- CustomerStory.cs
- DATA_CONFIG_GUIDE.md

### 游戏逻辑模块 (game-logic/)
- 交易流程
- 维修流程
- 顾客生成逻辑

### 场景模块 (scene/)
- 场景搭建

## 上下文管理策略

### 加载规则
- 当前开发哪个模块，只加载对应记忆
- 每次对话保留最近 10-15 条
- 跨模块操作时临时加载相关模块

### 文件更新
- 每个会话结束时更新对应模块记忆
- 重大决策写入主 MEMORY.md