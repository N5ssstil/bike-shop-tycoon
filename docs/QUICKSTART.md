# Bike Shop Tycoon - 快速启动指南

## 🚀 如何运行游戏

### 方法 1：使用编辑器菜单（推荐）

1. 打开 Unity 项目
2. 打开任意场景（或创建新场景）
3. 点击菜单 **BikeShop → 设置场景 → 完整设置**
4. 点击 **Play** 运行

### 方法 2：使用 GameInitializer

1. 创建一个空场景
2. 创建空对象，添加 `GameInitializer` 组件
3. 点击 **Play** 运行

---

## 📋 编辑器菜单功能

| 菜单项 | 功能 |
|--------|------|
| BikeShop/设置场景/完整设置 | 一键创建所有必要对象 |
| BikeShop/设置场景/仅创建管理器 | 只创建 GameManager 等管理器 |
| BikeShop/设置场景/创建 UI | 创建 Canvas 和 UI 元素 |
| BikeShop/创建/商品数据 | 创建 ScriptableObject 商品 |
| BikeShop/创建/品牌数据 | 创建 ScriptableObject 品牌 |
| BikeShop/帮助/检查项目状态 | 检查场景中是否有必要组件 |

---

## ⚙️ 自动创建的系统

运行时自动创建：

- ✅ GameManager（游戏状态管理）
- ✅ TimeManager（时间推进）
- ✅ SettingsManager（设置管理）
- ✅ AudioManager（音效管理）
- ✅ Canvas + HUD（界面）
- ✅ EventSystem（事件系统）

---

## 🎮 游戏玩法

1. **顾客会自动进店**（每 8-20 秒）
2. **点击顾客** 可以进行交易
3. **底部导航**：
   - 📦 库存管理
   - 🔧 维修服务
   - 💾 保存游戏
   - ⏸️ 暂停

---

## 🔧 开发者备注

### 核心脚本位置

```
Assets/Scripts/
├── Core/           # 核心系统（GameManager, SaveSystem, TimeManager）
├── GameSystems/    # 游戏系统（Customer, Inventory, Repair, Event）
├── UI/             # UI 控制器
├── Data/           # 数据结构
└── Editor/         # 编辑器工具
```

### 命名空间

所有代码使用 `BikeShopTycoon.*` 命名空间：
- `BikeShopTycoon.Core`
- `BikeShopTycoon.GameSystems`
- `BikeShopTycoon.UI`
- `BikeShopTycoon.Data`

---

## ❓ 常见问题

### Q: 运行时没有 UI？

确保场景中有 `GameInitializer` 或手动运行菜单命令。

### Q: TextMeshPro 报错？

第一次运行需要导入 TMP：
```
Window → TextMeshPro → Import TMP Essentials
```

### Q: 脚本丢失引用？

检查 `.meta` 文件是否存在，或重新导入项目。

---

**GitHub**: https://github.com/N5ssstil/bike-shop-tycoon