+++
title = "原生模板模式"
description = "Zest 原生模板模式：.zest.fsx F# DSL 与 {{ placeholder }} 占位符替换。"
category = "templates"
tags = ["zest", "模板", "fsharp"]
date = 2026-08-01
+++

# 原生模板模式

本文属于 Templates（模板）分类，介绍 Zest 的原生模板模式（`template.engine = "native"`，默认值）。原生模式包含两条互补的书写路径：`.zest.fsx` F# DSL 页面，以及 `{{ placeholder }}` 占位符替换的 HTML 布局。

## `.zest.fsx` F# DSL

页面是真正的 F# 脚本，由 `dotnet fsi` 求值。元数据用 `// @key value` 注释声明，正文放在 `page { }` 计算表达式里：

```fsharp
// @title 我的页面
// @layout default
// @date 2026-08-01

page {
    h1 [ text "你好，Zest" ]
    p  [ text "正文……" ]
}
```

`page { }` 内的构建器返回 HTML 字符串，可使用完整的 F# 语言能力（列表推导、模式匹配、.NET 库）。API 见 [DSL API 参考](/zh/posts/dsl-api/)。

## `{{ placeholder }}` 占位符

布局（与原生模式下的 `.html` 页面）使用占位符替换，心智模型与 11ty/Eleventy 的模板占位符一致：构建时把上下文值填入双花括号：

```html
<!DOCTYPE html>
<html lang="{{ site.language }}">
<head>
  <meta charset="utf-8" />
  <title>{{ page.title }} — {{ site.title }}</title>
</head>
<body>
  {{ include header }}
  <main>{{ content }}</main>
  {{ include footer }}
</body>
</html>
```

常用占位符：`{{ content }}`（页面正文）、`{{ page.title }}`、`{{ page.url }}`、`{{ site.title }}`、`{{ include header }}`。完整列表见 [模板参考](/zh/posts/template-reference/)。

## 两种路径的关系

- **布局**可以是 `.html`（占位符替换）或 `.zest.fsx`（DSL 脚本，直接输出 HTML）。
- **页面**可以是 `.zest.fsx`（DSL）或 `.md`（Markdown）；渲染后的正文统一作为 `content` 注入布局。
- `template.engine` 只是标注主要语言，不影响路由——布局按文件扩展名选择引擎（见 [配置](/zh/posts/configuration/) 与 [模板参考](/zh/posts/template-reference/)）。

## 与 F# DSL 的关系

F# DSL 是原生模式的"内容面"：它把 HTML 构建收进 F# 代码，让页面获得类型安全与 IDE 支持；而占位符替换是"布局面"，负责把页面套进站点外壳。二者互补，可混用。
