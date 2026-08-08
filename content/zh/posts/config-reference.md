+++
title = "配置参考"
description = "_config.toml 完整配置参考：site、build、template、theme、defaults、params、compat。"
category = "reference"
tags = ["zest", "参考", "配置"]
date = 2026-08-01
+++

# 配置参考

本文属于 Reference（参考）分类，列出 `_config.toml` 的完整键表。所有字段都有默认值（零配置）；配置支持根级键与 `[site]` / `[build]` 分组表两种写法，分组表存在时优先。组织思路见 [配置](/zh/posts/configuration/)。

## `[site]`（站点身份）

| 键 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `title` | string | `"My Zest Site"` | 站点标题，模板中为 `{{ site.title }}` |
| `url` / `base_url` | string | `"http://localhost:8080"` | 绝对 URL 基准（RSS、canonical 等），末尾斜杠会被去掉 |
| `description` | string | `"A site built with Zest SSG"` | 站点描述 |
| `author` | string | `""` | 默认作者 |
| `language` | string | `"en"` | HTML `lang` 属性 |
| `site_version` | string | `"1.0"` | 版本串（用于缓存破坏） |
| `root_dir` | string | `"content"` | 内容根目录；`"."` 表示项目根 |
| `dev_server_port` | int | `8080` | 开发服务器端口 |
| `live_reload_port` | int | `35729` | WebSocket 实时重载端口 |

## `[build]`（目录与构建）

| 键 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `output` / `output_dir` | string | `"./_site"` | 构建输出目录 |
| `layouts_dir` | string | `"./_layouts"` | 布局目录 |
| `includes_dir` | string | `"./_includes"` | 局部模板目录 |
| `data_dir` | string | `"./_data"` | 全局数据目录 |
| `assets_dir` | string | `"./assets"` | 静态资产目录 |
| `default_layout` | string | `"default"` | 默认布局名 |
| `permalink_format` | string | `"/:slug/"` | 默认永久链接格式 |
| `enable_minification` | bool | `false` | 压缩 HTML 输出 |
| `enable_html_formatting` | bool | `false` | 输出美化缩进 |
| `enable_asset_formatting` | bool | `false` | 资产美化 |
| `enable_cache_busting` | bool | `false` | 资源 URL 追加版本哈希 |
| `enable_parallel_build` | bool | `true` | 并行渲染页面 |
| `enable_incremental_build` | bool | `true` | 跳过未变化页面 |
| `log_level` | string | `"Info"` | `Debug` / `Info` / `Warn` / `Error` / `Off` |
| `log_to_file` | bool | `false` | 镜像日志到 `.zest/logs/zest.log` |
| `log_timestamps` | bool | `true` | 控制台行加时间戳 |
| `include` / `exclude` | string[] | `[]` | 显式包含 / 排除文件的 glob |
| `content_dir` | string | `"./content"` | 兼容别名 |

## `[template]`

| 键 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `engine` | string | `"native"` | **纯注释**，标注主要模板语言，不影响构建路由 |
| `nunjucks_compatibility` | string | `"zest"` | `zest` = 官方 Nunjucks + Zest 扩展过滤器；`strict` = 仅官方 |

嵌套形式：`[template.nunjucks] compatibility = "zest"`。顶层键 `template_engine` 是 `engine` 的旧式写法。

## `[theme]`

| 键 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `name` | string | `""` | 主题目录名；空 = 无主题 |
| `source` | string | `"local"` | `local` / `git` / `url` / `path` |
| `git` | string | `""` | Git 仓库 URL |
| `branch` / `tag` | string | `"main"` / `""` | Git 分支 / 标签 |
| `url` | string | `""` | ZIP 下载 URL |
| `path` | string | `""` | 本地目录路径 |

## `[[defaults]]`、`[params]`、`[compat]` 等

```toml
[[defaults]]              # 按 glob 设置 frontmatter 默认值
path = "posts/*"
[defaults.values]
layout = "post"

[params]                  # 任意主题参数 → {{ site.params.* }}
subtitle = "博客"
[params.colors]
accent = "#4f6ef7"

[compat]                  # 可选 SSG 兼容开关（均默认 false）
jekyll = false
hexo = false
hugo = false
eleventy = false

[[taxonomies]]            # 分类法（默认 tag/tags 与 category/categories）
name = "tag"
plural = "tags"

[menu.main]               # 导航菜单 → {{ menu.main }}
[[menu.main]]
label = "首页"
url = "/"
weight = 1

[pagination]
per_page = 10             # 分页默认每页条数
```

未知键会被忽略；解析失败时回退到默认配置。
