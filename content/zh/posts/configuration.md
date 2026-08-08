+++
title = "配置"
description = "介绍 _config.toml 的顶层结构与关键行为。"
category = "guides"
tags = ["zest", "指南", "配置"]
date = 2026-08-01
+++

# 配置

本文介绍站点配置文件 `_config.toml` 的组织方式与关键行为，属于 Guides（指南）分类；完整的逐键参考见 [配置参考](/zh/posts/config-reference/)。

## 零配置设计

Zest 是零配置（zero-config）的：当 `_config.toml` 不存在时，每个字段都回退到内置默认值。配置文件由 `ConfigLoader` 解析（基于 Tomlyn）：未知键被忽略，缺失键保留默认值。所有路径字段都相对于项目根目录解析；将 `root_dir = "."` 可让项目根目录直接充当内容目录。

## 顶层表

配置文件同时接受根级键与 `[site]` / `[build]` 分组表两种写法，分组表存在时优先。常用顶层表如下：

| 表 | 作用 |
|---|---|
| `[site]` | 站点身份：`title`、`url`（即 `base_url`）、`description`、`author`、`language`、`root_dir` 等 |
| `[build]` | 构建输出：`output`（即 `output_dir`）、`layouts_dir`、`includes_dir`、`data_dir`、`assets_dir` 等 |
| `[template]` | 模板引擎相关设置，见下 |
| `[theme]` | 主题来源：`name`、`source`（`local` / `git` / `url` / `path`）等 |
| `[[defaults]]` | 按 glob 路径为文件批量设置 frontmatter 默认值，如把 `posts/*` 全部路由到 `post` 布局 |
| `[params]` | 任意主题参数，暴露为 `site.params.*` |
| `[compat]` | 可选的 SSG 兼容开关（`jekyll` / `hexo` / `hugo` / `eleventy`） |
| `[pagination]` | 分页默认每页条数（`per_page`） |

## `template.engine` 是纯注释

`[template]` 表（或顶层键 `template_engine`）中的 `engine` 值**只是标注**站点的主要模板语言（`native` / `nunjucks` / `liquid` …），**不影响构建路由**——布局按文件扩展名决定由哪个引擎处理（见 [模板参考](/zh/posts/template-reference/)）。默认值为 `native`，对应 `.zest.fsx` 页面与 `{{ }}` 占位符替换。

## 主题合并优先级

主题文件是回退层：项目里同名的布局、includes、资产会覆盖主题文件。全局数据按以下优先级合并，后者覆盖前者：

```
主题 _data/  <  项目 _data/  <  _config.toml [params]
```

`[params]` 还会与 `_data/params.toml` 深度合并，并以 `site.params.<key>` 暴露给模板。

## 示例

```toml
[site]
title = "My Zest Site"
url = "https://example.com"
language = "zh-CN"

[build]
output = "_site"

[template]
engine = "native"

[theme]
name = "oxygen"
source = "local"

[[defaults]]
path = "posts/*"
[defaults.values]
layout = "post"

[params.colors]
accent = "#4f6ef7"
```

主题合并规则详见 [主题](/zh/posts/themes/)。
