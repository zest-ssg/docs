+++
title = "快速开始"
description = "安装 Zest、创建第一个站点并运行构建与开发服务器。"
category = "guides"
tags = ["zest", "指南"]
date = 2026-08-01
+++

# 快速开始

本文是 Zest 文档的起点，位于 Guides（指南）分类。它带你完成从安装到产出第一个静态站点的全过程；更深入的配置、命令与模板细节见同分类下的其他页面。

## 安装

Zest 以 .NET 全局工具（dotnet tool）分发。请先安装 **.NET SDK（10.0 或更高）**——构建 `.zest.fsx` 脚本需要 `dotnet fsi` 参与求值：

```bash
dotnet tool install --global zest-ssg
zest --version
```

## 创建项目

`zest init` 会从内置的 starter 站点脚手架一个新项目：

```bash
zest init my-site
cd my-site
```

生成的项目结构如下（starter 把布局、includes 与资产放进自包含主题）：

```
my-site/
├── _config.toml      # 站点配置（title、目录、构建选项）
├── _init.zest.fsx    # 每次构建前运行的初始化脚本
├── _data/            # 全局数据（.toml）
├── _themes/oxygen/   # 自包含主题
│   ├── _layouts/     #   布局模板
│   ├── _includes/    #   局部模板（partial）
│   ├── _locales/     #   i18n 字符串（en/zh）
│   └── assets/       #   静态资产（.zcss 编译为 .css）
└── content/          # 内容页（.zest.fsx 与 .md）
```

项目也可在根级放置自己的 `_layouts/`、`_includes/` 与 `assets/`，同名文件会覆盖主题文件（见 [主题](/zh/posts/themes/)）。

## 第一个页面

编辑 `content/index.zest.fsx`，元数据用 `// @key value` 注释声明，正文放在 `page { }` 计算表达式内：

```fsharp
// @title 欢迎来到我的站点
// @layout default

page {
    h1 [ text "Hello, Zest!" ]
    p  [ text "这是一个用 F# 构建的静态站点。" ]
}
```

也可以写普通 Markdown 文件 `content/about.md`，元数据用 `+++` TOML frontmatter 声明：

```markdown
+++
title = "关于"
layout = "default"
tags = ["meta"]
+++

# 关于本站

这是由 Zest 生成的静态站点。
```

## 构建与预览

```bash
zest build      # 构建到 _site/
zest serve      # 构建并启动开发服务器（含实时重载）
zest preview    # 直接预览已构建的 _site/，不触发构建
```

`serve` 默认监听 8080 端口，并通过 35729 端口的 WebSocket 在文件变化时推送实时重载；`--port`、`--open` 等选项见 [命令行](/zh/posts/cli/)。

## 下一步

- [配置](/zh/posts/configuration/) — 用 `_config.toml` 配置站点
- [Zest DSL](/zh/posts/dsl-guide/) — `page { }` 计算表达式详解
- [原生模板模式](/zh/posts/native-mode/) — `.zest.fsx` 与 `{{ }}` 占位符
- [ZCSS](/zh/posts/zcss/) — SCSS 风格的内置预处理器
