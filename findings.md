# 发现与调研记录

> 项目: Bike Shop Tycoon
> 最后更新: 2026-06-12

---

## 项目现状分析

### 已实现的核心系统
| 系统 | 文件数 | 完成度 | 备注 |
|------|--------|--------|------|
| GameManager | 1 | ✅ 完成 | 单例、状态管理、资金/口碑、存档 |
| SaveSystem | 1 | ✅ 完成 | 支持备份恢复 |
| TimeManager | 1 | ✅ 完成 | 游戏时间推进 |
| PlayerData | 1 | ✅ 完成 | 玩家数据模型 |
| CustomerManager | 1 | ✅ 完成 | 顾客生成、交互、交易 |
| InventoryManager | 1 | ✅ 完成 | 进货、销售、滞销处理 |
| RepairService | 1 | ⚠️ 待验证 | 维修服务逻辑 |
| ShopController | 1 | ⚠️ 部分 TODO | ShowAvailableProducts 空方法 |
| AchievementSystem | 1 | ⚠️ 待验证 | 成就系统框架 |
| EventSystem | 1 | ⚠️ 待连接 | 随机事件系统 |

### UI 面板状态
| 面板 | 文件 | 状态 |
|------|------|------|
| HUDController | Assets/Scripts/UI/ | ✅ 完成 |
| UIManager | Assets/Scripts/UI/ | ✅ 完成 |
| MainMenuController | Assets/Scripts/UI/ | ✅ 完成 |
| ShopPanelController | Assets/Scripts/UI/ | ✅ 完成 |
| InventoryPanel | Assets/Scripts/UI/ | ✅ 完成 |
| CustomerDetailPanel | Assets/Scripts/UI/ | ✅ 完成 |
| RepairPanel | Assets/Scripts/UI/ | ✅ 完成 |
| EventPanel | Assets/Scripts/UI/ | ✅ 完成 |
| CustomerReceptionUI | 根目录 ⚠️ | 有重复类型定义 |
| InventoryUI | 根目录 ⚠️ | 有重复类型定义 |
| WorkshopUI | 根目录 ⚠️ | 部分 TODO |
| MainMenuUI | 根目录 ⚠️ | 待审查 |
| ShopSceneUI | 根目录 ⚠️ | 待审查 |

### 场景状态
| 场景 | 状态 |
|------|------|
| MainMenu.unity | ✅ 已创建 |
| MainScene.unity | ✅ 已创建 |
| Shop.unity | ✅ 已创建 |

### ScriptableObject 配置
| 类型 | 已有 | 缺失 |
|------|------|------|
| 品牌 (Brands) | Giant, Specialized | Trek, 本地品牌, 禧玛诺, 速联 等 |
| 商品 (Products) | Entry/Mid/Pro 各1个 | 需要完整商品库 |
| 顾客故事 | 0 | 需要创建 |

---

## 已发现的问题

### 🔴 严重 — 代码重复定义
1. `CustomerReceptionUI.cs`(根目录) 定义了 `CustomerData` 类 — 与 `Assets/Scripts/Data/CustomerData.cs` 冲突
2. `CustomerReceptionUI.cs`(根目录) 定义了 `ProductData` 类 — 与 `BikeProduct.cs` 功能重叠
3. `InventoryUI.cs`(根目录) 定义了 `InventoryItemData` 类 — 与 `InventoryData.cs` 中的 `InventoryItem` 冲突

### 🟡 中等 — 文件位置错误
4. 5 个 .cs 文件放在项目根目录，应在 `Assets/Scripts/UI/`
5. `.meta` 文件缺失（Unity 需要）

### 🟢 轻微 — 代码待完善
6. 多处硬编码示例数据（品牌名、价格、商品名）
7. 多处 `// TODO:` 注释未实现
8. `ShopController.ShowAvailableProducts()` 为空方法

---

## 技术决策

- 命名空间: `BikeShopTycoon.Core` / `.GameSystems` / `.UI` / `.Data`
- 数据配置: 使用 Unity ScriptableObject
- UI 框架: Unity uGUI + TextMeshPro
- 存档: JSON 文件 + 备份机制
