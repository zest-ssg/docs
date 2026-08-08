+++
title = "Markdown"
description = "Markdown 内容支持：+++ TOML frontmatter、渲染能力与边界。"
category = "templates"
tags = ["zest", "模板", "markdown"]
date = 2026-08-01
+++

# Markdown

本文属于 Templates（模板）分类，介绍 Markdown 内容文件的书写方式与引擎能力边界。Markdown 是博客文章等内容的默认书写格式；`.md` / `.markdown` 文件由内置 Markdown 引擎渲染为 HTML，再套用布局。

## Frontmatter

文件顶部用 `+++` 包裹的 TOML 块声明元数据：

```markdown
+++
title = "我的文章"
description = "文章摘要，用于搜索与 RSS"
date = 2026-08-01
tags = ["fsharp", "static-site"]
layout = "post"
draft = false
+++

# 正文从这里开始
```

除 `layout`、`title`、`permalink`、`tags`、`date`、`description`、`draft` 等已知键外，未识别的键会存入页面的 `Extra` 元数据（可在模板中以 `page.<key>` 访问）。

## 支持的特性

- **标题**：`#` 到 `######`，自动生成稳定的锚点 id（用于"本页目录"链接）。
- **围栏代码块**：``` `` ``` 围栏并带语言名（如 `fsharp`），输出 `class="language-…"`。
- **表格**：管道式表格（含分隔行）。
- **图片与链接**：`![alt](src)`、`[text](url)`。
- **行内样式**：粗体、斜体、删除线、行内代码。
- **其他**：有序/无序列表、引用块、分隔线，以及**原始 HTML 直通**。

## 能力边界

- **无代码高亮**：代码块只输出语言类名，不内嵌高亮脚本；高亮需由主题自行实现。
- **无内置 TOC**：引擎不生成目录。本文档主题用客户端 JS 从页面 `h2` / `h3` 标题动态生成"本页目录"。
- 页面渲染完成后会作为 `content` 变量注入布局，与 `page.*`、`site.*` 等上下文一并使用，见 [模板参考](/zh/posts/template-reference/)。

## 与 F# 的关系

在 `.zest.fsx` 页面中也可用 `md` / `mdDedent` 助手直接渲染 Markdown 字符串，让两种书写方式并存——见 [Zest DSL 指南](/zh/posts/dsl-guide/)。
