+++
title = "CLI 参考"
description = "Zest CLI 完整参考：命令、选项、退出码与示例。"
category = "reference"
tags = ["zest", "参考", "cli"]
date = 2026-08-01
+++

# CLI 参考

本文属于 Reference（参考）分类，是 Zest 命令行工具的完整参考，选项均按源码核实。日常用法见 [命令行](/zh/posts/cli/)。

## 通用

```bash
zest [command] [options]
zest --version    # 显示版本（-v 顶层含义为版本，命令级 -v 为 --verbose）
zest --help       # 帮助（-h）
```

所有命令接受 `--verbose/-v`、`--quiet/-q`、`--help/-h`。

## `zest build`

```bash
zest build [path] [options]
```

| 选项 | 说明 |
|---|---|
| `path`（参数） | 项目目录，默认当前目录 |
| `--watch`, `-w` | 监听变化并自动重建 |

行为：加载 `_config.toml` → 运行 `_init.zest.fsx` → 发现内容 → 求值 `.zest.fsx` / 渲染 `.md` → 处理模板 → 套用布局 → 写入 `output_dir` → 复制 `assets/`（`.zcss` 编译为 `.css`）。

## `zest serve`（别名 `dev`）

```bash
zest serve [options]
```

| 选项 | 说明 |
|---|---|
| `--port`, `-p PORT` | 端口，默认 `dev_server_port`（8080） |
| `--host HOST` | 绑定主机，默认 `localhost` |
| `--open`, `-o` | 启动时打开浏览器 |
| `--spa` | SPA 模式：所有路由回退到 `index.html` |
| `--dir` | 启用目录列表 |

行为：完整构建 → 启动 HTTP 服务器与 WebSocket 实时重载服务器 → 监听文件变化并推送重载信号。

## `zest preview`

```bash
zest preview [options]
```

| 选项 | 说明 |
|---|---|
| `--port`, `-p PORT` | 端口，默认 8080 |
| `--host HOST` | 绑定主机，默认 `localhost` |
| `--open`, `-o` | 启动时打开浏览器 |
| `--watch`, `-w` | 监听并自动重建 |
| `--livereload`, `-l` | 启用 WebSocket 实时重载 |
| `--spa` / `--dir` | 同 serve |

与 `serve` 不同，`preview` **不触发构建**，只服务 `output_dir` 中已有的输出。

## `zest init` 与 `zest scaffold`

```bash
zest init [path]                    # 目标目录，默认当前目录
zest scaffold <template> [path]     # 预设：blog | empty
```

`init` 复制内置 starter 站点；目标目录非空时询问确认。`scaffold blog` 与 `init` 相同；`scaffold empty` 仅生成最小 `_config.toml` 与 `_init.zest.fsx`。未知预设打印错误并列出可用预设。

## `zest migrate`

```bash
zest migrate <source-ssg> [--from <dir>] [--to <dir>] [--dry-run]
```

| 参数/选项 | 说明 |
|---|---|
| `<source-ssg>` | `jekyll` / `hexo` / `hugo` / `eleventy` |
| `--from <dir>` | 源项目目录，默认当前目录 |
| `--to <dir>` | 目标目录，默认 `<from>/_zest_migrated` |
| `--dry-run` | 只打印迁移计划，不写文件 |

扫描源 SSG 的配置、布局、内容与静态资产，把 frontmatter/配置转换为 Zest 的 TOML 格式，并生成启用对应 `[compat]` 开关的项目结构。

## `zest clean` 与 `zest convert-config`

```bash
zest clean [--cache] [--output]    # 默认同时清除缓存与输出目录
zest convert-config <from> <to>    # from/to: yaml | toml
```

## 退出码

| 代码 | 含义 |
|---|---|
| `0` | 成功 |
| `1` | 出错（解析失败、未知命令/选项、目录不存在等） |
