+++
title = "标签归档"
description = "TaxonomyGenerator 自动生成 /tags/ 与 /tags/<term>/ 归档页。"
category = "features"
tags = ["zest", "功能", "标签"]
date = 2026-08-01
+++

# 标签归档

本文属于 Features（功能）分类，介绍标签归档页的自动生成机制。`TaxonomyGenerator` 在内容管线之后运行，根据全站页面的标签自动产出归档页——给文章打上标签即可，无需为每个标签手写页面。

## 生成规则

- **索引页**：`/tags/`（`tags/index.html`），默认按 `_layouts/tags.njk` 渲染，回退到 `_layouts/terms.njk` 与内置默认模板。
- **单项页**：`/tags/<term>/`，默认按 `_layouts/tag.njk` 渲染，回退到 `_layouts/taxonomy.njk` 与内置默认模板。
- **内容文件优先**：如果某个 URL（如 `/tags/`）已被内容页占用，生成器会跳过该页——这让你可以用 `.zest.fsx` 自定义标签索引（例如用 `tag_cloud` 画标签云），而不与生成器冲突。
- 每次构建都会重写生成页，模板或配置变化不会留下过期页面。

## 声明方式

标签在页面 frontmatter 中声明（TOML、HTML 注释、F# 注释三种格式均可）：

```markdown
+++
title = "我的文章"
tags = ["fsharp", "static-site"]
+++
```

```fsharp
// @tags fsharp static-site
```

## 生成页上下文

单项归档页的模板上下文除常规的 `site.*`、`pages` 外，还注入：

| 变量 | 说明 |
|---|---|
| `term` | 当前标签名 |
| `term_pages` | 该标签下的页面（按日期倒序） |
| `taxonomy` | `{ name, plural, term }` 字典 |

## 自定义索引页

starter 站点的 `content/tags.zest.fsx` 展示了如何接管 `/tags/` 索引：通过 `// @permalink /tags/` 声明占用该 URL，再在页面内用 `DslCollections.tag_cloud` 渲染标签云。相关查询见 [DSL 集合](/zh/posts/dsl-collections/)，体系说明见 [集合与标签](/zh/posts/collections/)。
