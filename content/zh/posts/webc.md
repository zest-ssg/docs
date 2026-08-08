+++
title = "WebC"
description = "WebC 组件格式的现状：兼容层尚未完成，当前请使用 Markdown 或其他模板语言。"
category = "templates"
tags = ["zest", "模板", "webc"]
date = 2026-08-01
+++

# WebC

本文属于 Templates（模板）分类，介绍 WebC 组件格式在 Zest 中的现状。WebC 是 11ty 的组件式 HTML 格式；需要说明的是，**Zest 目前尚未实现 WebC 兼容层。** 在兼容层完成之前，请使用 Markdown 或其他模板语言（Nunjucks、Liquid、Handlebars、HAML、Pug 等）书写内容。

## 当前状态

`.webc` 文件扩展名虽然已在文件类型表中注册，但引擎中只有最小化的清洗逻辑（剥离 `webc:setup` 脚本、规范化 `webc:nocss` 模板标签），WebC 的组件语义——props、slots、组件树解析、`@attributes` 等——尚未实现。因此现在不应把 `.webc` 文件作为内容或布局使用。

## 兼容层完成后的行为

当 WebC 兼容层开发完成后，`.webc` 文件将与其他模板语言一样**按扩展名路由**：内容目录中的 `.webc` 页面与 `_layouts/` 中的 `.webc` 布局都会被识别、渲染并套用布局管线，无需额外配置。

## 建议

- 内容页：使用 `.md`（Markdown，见 [Markdown](/zh/posts/markdown/)）。
- 页面与布局：使用 `.zest.fsx`（见 [原生模板模式](/zh/posts/native-mode/)）或 Nunjucks（`.njk` / `.html`）。
- 需要组件化结构时，可先用 `_includes/` 局部模板拆分。

本页会随兼容层发布而更新。
