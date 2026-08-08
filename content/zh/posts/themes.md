+++
title = "主题"
description = "Zest 主题系统：目录结构、主题来源、文件合并规则与参数优先级。"
category = "guides"
tags = ["zest", "指南", "主题"]
date = 2026-08-01
+++

# 主题

本文属于 Guides（指南）分类，介绍 Zest 的轻量主题系统。一个主题是 `_themes/` 下的目录（或来自远程来源），提供布局、includes、资产与 ZCSS 样式；主题文件一律充当**回退层**——项目里同名文件优先。

## 目录结构

```
_themes/oxygen/
├── _theme.toml      # 主题清单（元数据、data、filters）
├── _layouts/        # 布局模板（default、post、page …）
├── _includes/       # 局部模板（header、footer、nav …）
├── _locales/        # i18n 字符串（en.toml、zh.toml）
├── _data/           # 主题级全局数据
└── assets/          # 静态资产（css/main.zcss 编译为 .css）
```

在 `_config.toml` 中启用主题：

```toml
[theme]
name = "oxygen"
source = "local"   # local | git | url | path
```

## 主题来源

| `source` | 说明 |
|---|---|
| `local`（默认） | 主题位于 `_themes/{name}/` |
| `git` | 克隆仓库到 `.zest/themes/{name}/`，可指定 `branch` / `tag` |
| `url` | 下载 ZIP 归档并解压到缓存目录 |
| `path` | 引用本地任意目录，适合主题与站点并行开发 |

## 文件合并规则

| 主题文件 | 行为 |
|---|---|
| `_layouts/*` | 项目 `_layouts/` 覆盖同名文件；缺失时回退到主题 |
| `_includes/*` | 同布局：项目优先，主题回退 |
| `assets/*` | 先复制主题资产，项目资产在冲突时覆盖 |
| `_theme.toml` | 声明式清单：元数据暴露为 `site.theme.*`，`[data]` 合并为全局数据 |
| `_theme.zest.fsx` | 主题初始化脚本，在项目 `_init.zest.fsx` **之前**执行 |

## 参数优先级

全局数据按以下顺序合并，后者覆盖前者：

```
主题 _data/  <  项目 _data/  <  _config.toml [params]
```

因此主题作者可以把配色等可定制项做成 `[params]` 键（如 `[params.colors] accent`），用户无需改动主题文件即可覆盖默认值。

## 初始化脚本

`_theme.zest.fsx` 与 `_init.zest.fsx` 使用同一套 API（`addGlobal`、`addFilter`、`loadJson` 等），适合注册模板过滤器与注入全局数据：

```fsharp
// _themes/minima/_theme.zest.fsx
addFilter "excerpt" "truncate(200) | striptags"
addGlobal "total_pages" (site_pages().Length |> box)
```

由于主题脚本先运行，用户脚本可以扩展或覆盖它声明的任何内容。完整配置键见 [配置参考](/zh/posts/config-reference/)，布局渲染细节见 [模板参考](/zh/posts/template-reference/)。
