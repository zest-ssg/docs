+++
title = "命令行"
description = "Zest CLI 常用命令总览：build、serve、preview、init、scaffold 与 migrate。"
category = "guides"
tags = ["zest", "指南", "cli"]
date = 2026-08-01
+++

# 命令行

本文属于 Guides（指南）分类，介绍 Zest CLI 的常用命令与主要选项；逐项核实过的完整参考见 [CLI 参考](/zh/posts/cli-reference/)。

## 命令总览

```bash
zest [command] [options]
```

不指定命令（或 `--help`）时打印命令列表；`--verbose/-v`、`--quiet/-q` 与 `--help/-h` 是所有命令通用的选项。

| 命令 | 别名 | 说明 |
|---|---|---|
| `build` | — | 构建静态站点到输出目录 |
| `serve` | `dev` | 构建 + 启动带实时重载的开发服务器 |
| `preview` | — | 直接服务已构建的 `_site/`（不构建） |
| `init` | — | 从内置 starter 脚手架新项目 |
| `scaffold` | — | 从预设（`blog` / `empty`）生成项目 |
| `migrate` | — | 把已有 SSG 项目转换为 Zest 项目 |
| `clean` | — | 清除构建缓存与输出目录 |
| `convert-config` | — | 在 YAML 与 TOML 配置格式之间转换 |

## 构建与开发

```bash
zest build                    # 构建到 _site/
zest build --watch            # 监听变化并自动重建
zest serve                    # 开发服务器（8080，WebSocket 实时重载）
zest serve --port 3000 --open # 自定义端口并自动打开浏览器
zest serve --spa              # SPA 模式：所有路由回退到 index.html
zest preview --watch          # 预览已构建输出，并监听重建
```

`serve` 执行完整构建后启动 HTTP 服务器与 WebSocket 实时重载服务器；`preview` 不触发构建，只服务 `output_dir` 中已有的内容。

## 项目生成与迁移

```bash
zest init my-site               # 复制内置 starter 站点
zest scaffold blog              # 同上（blog 预设 = 内置 starter）
zest scaffold empty             # 仅生成最小 _config.toml + _init.zest.fsx
zest migrate jekyll --from ./blog --to ./zest-blog   # 从 Jekyll/Hexo/Hugo/11ty 迁移
zest migrate hugo --dry-run     # 先打印迁移计划，不写文件
```

## 退出码

| 代码 | 含义 |
|---|---|
| `0` | 成功 |
| `1` | 出错（解析失败、未知命令或选项等） |
