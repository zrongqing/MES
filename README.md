# MES
MES项目

## 解决方案结构

MES/
├── MES.sln
├── src/
│   ├── MES.API/                  🌐 Web API 项目
│   ├── MES.Application/         📦 应用层
│   ├── MES.Domain/              🧠 领域模型层
│   ├── MES.Infrastructure/      🏗️ 基础设施层（EF Core、外部服务等）
│   ├── MES.Shared/              🔗 DTOs、通用模型、常量
│   └── MES.Client.WPF/          🖥️ 客户端（WPF + MVVM）
└── tests/
└── MES.Tests/               🧪 单元测试项目

## 技术路线

- [ ] net8

### 客户端

- NET8
- Avaloniaui [avaloniaui](https://docs.avaloniaui.net/zh-Hans/docs/welcome)
- 

### 服务器

- EFCore

## 功能点

- [ ] 登录系统
- [ ] 自动更新模块
- [ ] 接入Github自动化编译系统
- [ ] 角色管理系统