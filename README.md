# Bike Shop Tycoon 🚴

公路车模拟经营游戏 - 从街边车店到环法冠军

## 技术栈

- **引擎**: Unity 2022.3 LTS
- **语言**: C#
- **画面**: 2D 写实风
- **平台**: Steam (PC)

## 开发进度

- [ ] 第一阶段：核心原型（1-2个月）
- [ ] 第二阶段：车店进阶（2-3个月）
- [ ] 第三阶段：社群与车队基础（2-3个月）
- [ ] 第四阶段：车队进阶与赛事（3-4个月）
- [ ] 第五阶段：终极完善与上线（2-3个月）

## 项目结构

```
Assets/
├── Scripts/
│   ├── Core/           # 核心框架（GameManager, EventSystem等）
│   ├── Data/           # 数据结构（商品、顾客、库存）
│   ├── UI/             # UI控制器
│   └── GameSystems/    # 游戏系统（库存、交易、维修）
├── Scenes/             # 游戏场景
├── Prefabs/            # 预制体
├── ScriptableObjects/  # 配置数据
├── Art/
│   ├── Sprites/        # 游戏精灵
│   └── UI/             # UI素材
└── Audio/              # 音效音乐
```

## 文档

- [游戏设计文档](docs/GAME_DESIGN.md)
- [开发路线图](docs/ROADMAP.md)