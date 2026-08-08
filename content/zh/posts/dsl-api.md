+++
title = "DSL API 参考"
description = "Zest.Dsl 库的模块参考：DslHtml、DslComponents、DslHelpers、DslSeo、DslXml。"
category = "dsl"
tags = ["zest", "dsl", "参考"]
date = 2026-08-01
+++

# DSL API 参考

本文属于 Zest DSL 分类，是 `Zest.Dsl` 库的模块级参考，所有函数都可在 `.zest.fsx` 脚本中使用。使用思路见 [Zest DSL 指南](/zh/posts/dsl-guide/)。

## DslHtml —— 元素构建器

自动打开的 `Dsl` 模块，所有函数返回 `string` HTML。核心：`elem`、`voidElem`、`attr`、`text`、`raw`、`htmlEncode`。覆盖完整 HTML 元素集：`h1`–`h6`、`div`、`p`、`section`、`article`、`ul` / `ol` / `li`、`table` 系列、`dl` / `dt` / `dd`、`figure` / `figcaption`、`details` / `summary` 等。类快捷方式形如 `divC "cls" [ … ]`、`pC`、`spanC`、`imgC`、`aC`。常用助手：`aBlank`、`aHref`、`codeBlock`、`fragment`、`comment`、`doctype`、`styleZcss`。

## DslComponents —— 组件

```fsharp
open DslComponents
```

表单元素（`form`、`input`、`button`、`select`、`option`、`label`）、布局组件（`container`、`row`、`col`、`card`、`badge`）、按钮变体（`btnPrimary` / `btnSecondary` / `btnSuccess` / `btnDanger`）、提示组件（`alert`、`alertInfo`、`alertSuccess`、`alertWarning`、`alertDanger`）、导航组件（`navLink`、`navList`、`breadcrumb`）、图标与媒体（`icon`、`figureResponsive`、`videoEmbed`）、状态组件（`progressBar`、`meterBar`）、社交组件（`socialLink`、`contactList`）。

## DslHelpers —— 脚本与数据助手

- `js` / `jsModule`：内联 `<script>` / `<script type="module">`，正文自动去缩进。
- `jsonBlock name data`：把 F# 数据序列化为 JSON 并注入 `<script>window.NAME = …</script>`（处理 `</script>` 转义）。
- `md` / `mdDedent` / `dedent`：把 Markdown 字符串渲染进 DSL 树。
- `interp` / `interp_safe`：`{key}` 占位符替换（原始 / 转义）。

## DslSeo —— SEO 标签

```fsharp
open DslSeo
```

`meta_tags`、`open_graph_tags`、`twitter_card_tags` 各返回 `string list`（用 `raw` + `String.concat "\n"` 输出），另有 `canonical_url`、`hreflang_tag`。

## DslXml —— 订阅源

```fsharp
open DslXml
```

`rss_xml`（RSS 2.0）、`atom_xml`（Atom 1.0）、`sitemap_xml`，均接收页面匿名记录数组并返回 XML 字符串。典型用法：配合 `// @output rss.xml` 在 `.zest.fsx` 页面中直接输出订阅源。

## 其他模块

`DslSugar`（条件/循环/管道）、`DslStyle`（`styleCss`、`styleScoped`、`criticalCss` 等）、`ContentGuard`（`validate` / `require` / `trace`）、`StringHelper` / `DateHelper` / `SequenceHelper` 等工具模块。完整签名以 `Zest.Dsl` 程序集为准。
