# Bike Shop Tycoon — 完善计划

> **项目**: 公路车模拟经营游戏
> **仓库**: https://github.com/N5ssstil/bike-shop-tycoon
> **引擎**: Unity 2022.3 LTS / C# / 2D
> **当前阶段**: 第一阶段 — 核心原型

---

## 目标

完善项目结构，修复已发现的问题，使代码可编译、可运行，为后续开发打好基础。

---

## 阶段概览

| 阶段 | 状态 | 内容 |
|------|------|------|
| Phase 1 | ✅ complete | 项目结构整理 — 松散文件归位 |
| Phase 2 | ✅ complete | 代码整合 — 消除重复定义 |
| Phase 3 | ✅ complete | 补充核心逻辑 — 修复 TODO/空方法 |
| Phase 4 | ✅ complete | 数据层完善 — ScriptableObject 体系 |
| Phase 5 | ✅ complete | 功能增强 — 事件系统、成就系统 |
| Phase 6 | ✅ complete | 验证 & 文档更新 |

---

## Phase 1: 项目结构整理

**目标**: 将根目录下的松散 .cs 文件移到正确位置

### 任务
- [x] 移动 `CustomerReceptionUI.cs` → `Assets/Scripts/UI/`
- [x] 移动 `InventoryUI.cs` → `Assets/Scripts/UI/`
- [x] 移动 `MainMenuUI.cs` → `Assets/Scripts/UI/`
- [x] 移动 `ShopSceneUI.cs` → `Assets/Scripts/UI/`
- [x] 移动 `WorkshopUI.cs` → `Assets/Scripts/UI/`
- [x] 确认命名空间与文件位置一致

## Phase 2: 代码整合 — 消除重复

**目标**: 解决重复定义问题，确保代码可编译

### 任务
- [x] 审查 `CustomerReceptionUI.cs` 中的 `CustomerData` vs `Assets/Scripts/Data/CustomerData.cs`
- [x] 审查 `CustomerReceptionUI.cs` 中的 `ProductData` vs 其他 Product 类型
- [x] 审查 `InventoryUI.cs` 中的 `InventoryItemData` vs `Assets/Scripts/Data/InventoryData.cs`
- [x] 统一使用 Data 命名空间下的类型，删除松散文件中的重复定义
- [x] 确保所有引用正确更新

## Phase 3: 补充核心逻辑

**目标**: 修复代码中的 TODO 和空方法，让游戏流程可运行

### 任务
- [x] `ShopController.ShowAvailableProducts()` — 从库存加载商品列表
- [x] `CustomerReceptionUI` — 商品项 UI 内容设置
- [x] `InventoryUI` — 商品项 UI 刷新、购买按钮状态
- [x] `WorkshopUI` — 完成工具选择、维修流程
- [x] 各面板的资金不足/成功提示
- [x] `MainMenuUI` — 主菜单完整功能

## Phase 4: 数据层完善

**目标**: 建立完整的 ScriptableObject 配置体系

### 任务
- [x] 完善 `BikeBrand` ScriptableObject，填充更多品牌数据
- [x] 完善 `BikeProduct` ScriptableObject，建立完整商品数据库
- [x] 创建 `CustomerStory` ScriptableObject 配置
- [x] 创建 `ProductDatabase` 统一加载机制
- [x] 替换各 UI 中的硬编码数据为 ScriptableObject 引用

## Phase 5: 功能增强

**目标**: 连接事件系统和成就系统

### 任务
- [x] 连接 `EventSystem` 到 GameManager 事件
- [x] 完善 `AchievementSystem` 的里程碑检测
- [x] 完善 `RepairService` 与 `WorkshopUI` 的连接
- [x] 实现口碑值对客流的影响

## Phase 6: 验证 & 文档

**目标**: 确保项目可编译，文档完善

### 任务
- [x] 检查所有脚本的命名空间一致性
- [x] 更新 README.md 中的开发进度
- [x] 更新 ROADMAP.md
- [x] 生成 `.meta` 文件清单

---

## 错误记录

| 错误 | 尝试 | 解决方案 |
|------|------|----------|
| (暂无) | - | - |
