+++
title = "功能总览"
description = "Zest 功能总览：多模板引擎、F# DSL、ZCSS、分页、搜索、PJAX 等。"
category = "features"
tags = ["zest", "功能"]
date = 2026-08-01
+++

# 功能总览

本文属于 Features（功能）分类，概述 Zest 的核心能力，并给出各功能的文档入口。

## 内容与模板

- **原生 F# DSL**：`.zest.fsx` 页面是真正的 F# 脚本，类型安全地构建 HTML——见 [Zest DSL 指南](/zh/posts/dsl-guide/)。
- **多模板引擎**：Nunjucks（`.njk` / `.html`）、Liquid、Handlebars、Mustache、HAML、Pug 均可用于内容与布局；其中 Liquid/HAML/Pug 转换后由 Nunjucks 渲染，Hbs 系由独立引擎渲染。WebC 兼容层尚未完成——见 [模板](/zh/posts/markdown/) 分类各页。
- **原生模板模式**：`{{ placeholder }}` 占位符替换，心智模型兼容 11ty/Eleventy——见 [原生模板模式](/zh/posts/native-mode/)。

## 样式与构建

- **ZCSS 预处理器**：变量、嵌套、颜色函数、`@use` 模块，构建期编译为 CSS——见 [ZCSS](/zh/posts/zcss/)。
- **构建优化**：并行构建、增量构建（内容哈希 + mtime 缓存）、HTML 压缩、缓存破坏（`site_version`）、厂商前缀与资产格式化。

## 内容组织

- **集合与分页**：按 URL 首段分集合，`paginate` 生成多页列表——见 [集合与标签](/zh/posts/collections/)。
- **标签与分类**：frontmatter 声明，`TaxonomyGenerator` 自动生成归档页——见 [标签归档](/zh/posts/tags/)。
- **RSS/Atom 与站点地图**：`DslXml` 生成订阅源与 sitemap；starter 自带示例。

## 站点能力

- **搜索**：构建期 JSON 索引 + 客户端实时过滤，纯静态——见 [搜索](/zh/posts/search/)。
- **PJAX 导航**：AJAX 换页、hover 预取、`pjax:end` 事件——见 [PJAX 导航](/zh/posts/pjax/)。
- **i18n**：`_locales/` 字符串表，`t` / `t_lang` 辅助函数按语言取词。
- **实时重载**：开发服务器通过 WebSocket（35729 端口）推送页面刷新。
- **主题系统**：本地 / Git / URL / 路径四种来源，项目文件覆盖主题——见 [主题](/zh/posts/themes/)。

## 与常见 SSG 的对比

| 能力 | Zest | Jekyll | Hugo | 11ty |
|---|---|---|---|---|
| 模板即代码 | ✅ F# DSL | ❌ | 部分（模板函数） | ❌ |
| 多模板引擎 | ✅ 六种以上 | Liquid | Go 模板 | 多种 |
| 零配置 | ✅ | 部分 | 部分 | ✅ |
| 迁移工具 | ✅ `zest migrate` | — | — | — |
| 兼容模式 | ✅ `[compat]` 逐项开启 | — | — | — |

Zest 对 Jekyll / Hexo / Hugo / 11ty 提供可选的 `[compat]` 兼容开关，便于从既有项目迁移。
