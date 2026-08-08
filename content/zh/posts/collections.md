+++
title = "集合与标签"
description = "集合、标签与分类体系：frontmatter、查询 API 与模板上下文。"
category = "features"
tags = ["zest", "功能", "集合"]
date = 2026-08-01
+++

# 集合与标签

本文属于 Features（功能）分类，介绍 Zest 的集合（collection）、标签（tag）与分类（category）体系。它们都从页面元数据与 URL 自动派生，无需手工维护清单；查询 API 详见 [DSL 集合](/zh/posts/dsl-collections/)。

## 集合（Collection）

集合按页面 URL 的**第一段**自动分组：`content/posts/hello.md` 属于 `posts` 集合，`content/docs/api.md` 属于 `docs` 集合。

```fsharp
open DslCollections

let docsPages  = pages_by_collection "docs"   // 集合内页面
let collections = all_collections ()          // 全部集合名
```

## 标签与分类（Tag / Category）

标签与分类在 frontmatter 中声明，两种格式等价：

```markdown
+++
title = "一篇文章"
tags = ["fsharp", "static-site"]
category = "guides"
+++
```

```fsharp
// @tags fsharp static-site
// @category guides
```

查询与分组：

```fsharp
let fsharpPosts = pages_by_tag "fsharp"        // 单标签
let allTags     = all_tags ()                  // 全部标签
let guides      = pages_by_category "guides"   // 分类
let grouped     = group_pages_by "tag"         // (标签, 页面列表)
let cloud       = tag_cloud 2                  // 标签云（数量 ≥ 2）
```

## 模板上下文

在 Nunjucks 布局中，`pages`、`tags`、`collections` 作为上下文变量直接可用：

```njk
{% for tag, taggedPages in tags %}
  <h2>{{ tag }}</h2>
  {% for page in taggedPages %}
    <a href="{{ page.url }}">{{ page.title }}</a>
  {% endfor %}
{% endfor %}
```

自定义过滤器 `pages_by_tag`、`recent`、`by_collection`、`where` 等仅在 `nunjucks_compatibility = "zest"`（默认）模式下注册。

## 收藏页生成

集合首页与分页既可用 `.zest.fsx` 手写（配合 `paginate`），也可在内容页 frontmatter 声明 `<!-- @paginate posts, 5 -->` 由分页生成器接管渲染。标签归档页由生成器自动产出，见 [标签归档](/zh/posts/tags/)。
