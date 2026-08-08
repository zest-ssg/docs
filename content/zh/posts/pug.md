+++
title = "Pug"
description = "Pug 模板支持：缩进式语法转换为带 Nunjucks 指令的 HTML 后渲染。"
category = "templates"
tags = ["zest", "模板", "pug"]
date = 2026-08-01
+++

# Pug

本文属于 Templates（模板）分类，介绍 Pug 模板语言在 Zest 中的支持方式。Pug（旧称 Jade）以缩进定义嵌套；`.pug` 文件由 `PugConverter` 先构建缩进树，再转换为带 Nunjucks 指令的 HTML，交给 Nunjucks 引擎渲染。

## 基本语法

```pug
doctype html
html(lang=site.language)
  body
    header
      h1= page.title
    main
      each post in posts
        article.post
          h2: a(href=post.url)= post.title
          p= post.description
      else
        p 暂无文章。
    include footer.pug
```

| 写法 | 含义 |
|---|---|
| `tag` / `tag.class` / `tag#id` | 标签与类/id |
| `tag(attr="v")` | 括号属性列表 |
| `.class` / `#id` | `div` 简写 |
| `| text` | 字面文本（支持 `#{expr}` 插值） |
| `= expr` / `!= expr` | 输出表达式（转义 / 不转义） |
| `- var x = expr` | 转译为 `{% set x = expr %}` |

## 控制流与组件

- **条件**：`if` / `else if` / `else` / `unless` 转译为 `{% if %}` / `{% elif %}` / `{% else %}` / `{% if not %}`。
- **遍历**：`each item in list` 与 `each val, key in obj` 转译为 `{% for %}`（对象形式键值互换）。
- **混入**：`mixin name(args)` 与 `+name(args)` 转译为 Nunjucks 的 `{% macro %}` / `{{ name() }}`。
- **继承**：`extends` / `block` 转译为 `{% extends %}` / `{% block %}`。
- **include**：`include path` 转译为 `{% include "path" %}`，用于引用 `_includes/`。
- **doctype**：输出 `<!DOCTYPE html>`。

## 转换细节

- `tag.`（如 `script.`、`style.`）表示原始文本块。
- `// comment` 转译为 HTML 注释，`//-` 则直接剥离。
- 文本与属性值做 HTML 转义；void 元素输出 HTML5 形式。
- 转换结果按内容哈希缓存，开发服务器重建时未变化的文件不会重复转换。

模板上下文（`page.*`、`site.*`、`posts` 等）见 [模板参考](/zh/posts/template-reference/)。
