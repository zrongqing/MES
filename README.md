# MES
MES项目

个人的一些[开发过程文档](./doc)

## 解决方案结构

```
MyMES/
├── MyMES.Core/                 # 核心层（领域层 Domain）
│   ├── Entities/               # 实体模型（聚合根、值对象）
│   ├── Interfaces/             # 接口定义（仓储、服务）
│   ├── Enums/                  # 枚举定义
│   ├── DTOs/                   # 数据传输对象（非实体）
│   ├── Exceptions/             # 自定义异常
│   └── Common/                 # 公共工具类（规范、常量等）
│
├── MyMES.Infrastructure/       # 基础设施层（Infrastructure）
│   ├── Persistence/            # EFCore 上下文 & 配置
│   ├── Repositories/           # 实现 Core.Interfaces 中的仓储接口
│   ├── Configurations/         # EF Fluent 配置文件
│   ├── Services/               # 外部服务（如金蝶、PLC、MQ、API）
│   └── Migrations/             # EF Core Migration 文件
│
├── MyMES.AppServices/          # 应用层（Application Service）
│   ├── Services/               # 应用服务（业务用例）
│   ├── DTOs/                   # 输入输出模型（ViewModel专用）
│   ├── Commands/Queries/       # CQRS 分层（可选）
│   └── Validators/             # FluentValidation 校验
│
├── MyMES.Presentation/         # 表现层（WPF/Blazor/ASP.NET）
│   ├── ViewModels/             # MVVM ViewModel 层
│   ├── Views/                  # 界面文件（XAML 或 Razor）
│   ├── Converters/             # 值转换器
│   ├── Controls/               # 自定义控件
│   ├── Resources/              # 资源文件（样式、语言、图片）
│   └── App.xaml.cs             # 程序入口
│
├── MyMES.Shared/               # 跨层共享模块（可选）
│   ├── Constants/              # 全局常量定义
│   ├── Extensions/             # 扩展方法
│   ├── Logging/                # 通用日志接口
│   └── Utilities/              # 通用工具类
│
├── MyMES.API/                  # Web API 层（如有需要）
│   ├── Controllers/            # API 控制器
│   ├── Middlewares/            # 中间件
│   ├── Filters/                # 全局过滤器
│   └── Program.cs / Startup.cs
│
└── MyMES.Tests/                # 单元测试 & 集成测试
    ├── UnitTests/
    └── IntegrationTests/
```

```markdown
Presentation → Application → Core ← Infrastructure
                      ↑
                      └── Shared
```

- Core：独立存在，不依赖任何层。
- Infrastructure：实现 Core 的接口。 
- Application：封装业务逻辑，调用 Core 接口。 
- Presentation：用户交互层（WPF / Web / HMI）。 
- Shared：跨层通用工具类。

## 技术路线

- NET8  
- [Obfuscar](https://zrongqing.github.io/posts/7f4ee469/)
- [NLog](https://github.com/NLog/NLog.Extensions.Logging/wiki/NLog-configuration-with-appsettings.json#loading-from-appsettingsjson)
- [ASP.NET Authentication](https://learn.microsoft.com/zh-cn/aspnet/core/security/authentication/?view=aspnetcore-8.0)  

### 客户端

- NET8
- Syncfusion WPF UI框架
- 

### 服务器

- EFCore

## 功能点

