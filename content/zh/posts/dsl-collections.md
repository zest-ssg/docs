+++
title = "DSL 集合"
description = "DslCollections 参考：页面查询、分组、相关推荐、分页与站点数据。"
category = "dsl"
tags = ["zest", "dsl", "集合"]
date = 2026-08-01
+++

# DSL 集合

本文属于 Zest DSL 分类，是 `DslCollections` 模块（`open DslCollections`）的参考，提供页面查询、分组、相关推荐、分页与站点数据访问。集合的完整体系见 [集合与标签](/zh/posts/collections/)。

## 页面记录

查询返回的每个页面是匿名记录：

```fsharp
{|
    url: string          // 如 "/posts/hello/"
    title: string
    date: string         // 无日期时为 ""
    slug: string
    description: string
    tags: string[]
    author: string
    category: string
|}
```

## 查询

```fsharp
let all      = site_pages ()                    // 全站页面
let recent   = recent_pages 5                   // 按日期倒序取最近 5 篇
let tagged   = pages_by_tag "fsharp"            // 带某标签的页面
let inDir    = pages_by_dir "blog"              // URL 含某目录段的页面
let inCol    = pages_by_collection "docs"       // 某集合（URL 首段）的页面
let tags     = all_tags ()                      // 全部标签（去重排序）
let cols     = all_collections ()               // 全部集合名
let hits     = search_pages "tutorial"          // 标题不区分大小写搜索
let count    = page_count ()                    // 页面总数
let byAuthor = pages_by_author "Jane"           // 按作者
let byCat    = pages_by_category "guides"       // 按分类
let byYear   = pages_by_year "2026"             // 按年份前缀
```

## 排序与过滤

```fsharp
sort_pages_by "date" "desc"          // 按 title / date / slug 排序
pages_sorted_by "title" "asc"        // 别名
filter_pages_by (fun p -> p.title.Contains "Zest")
where "category" "guides" pages      // 按属性过滤
pages_limit 5                        // 取前 N 条
pages_offset 10                      // 跳过前 N 条
```

## 分组与标签云

```fsharp
group_pages_by_year ()               // (年份, 页面列表)
group_pages_by_month ()              // (yyyy-MM, 页面列表)
group_pages_by "tag"                 // 按字段分组："tag" / "collection" / "year"
group keyFn items                    // 通用分组
tag_cloud 2                          // (标签, 数量) 对，数量 ≥ 2
tag_cloud_weighted 1.5               // 加权标签云
```

## 相关页面与分页

```fsharp
related_pages "/posts/my-post/" 3              // 共享标签推荐（排除自身）
related_pages_by_category "/posts/my-post/" 3  // 同分类推荐
```

分页返回 `Page<'a>` 记录列表（`Items`、`PageNumber`（1 起）、`TotalPages`、`TotalItems`、`HasPrev`、`HasNext`、`PrevUrl`、`NextUrl`）：

```fsharp
let paged = paginate 10 (fun n -> sprintf "/blog/page/%d/" n) (recent_pages 50)
let page2 = paged.[1]
```

`paginate_pages perPage urlFor` 是按日期倒序对全站页面分页的便捷版。

## 站点数据与 partial

```fsharp
site_data "social.twitter.handle"   // 全局数据取值
site_section "social"               // 某前缀下的键值字典
include_partial "header"            // 渲染 _includes/ 中的 partial
get_page "/about/"                  // 按 URL 查单页（option）
get_collection "posts"              // 集合查询的别名
```

模板侧的等价能力（Nunjucks 上下文 `pages` / `tags` / `collections` 与过滤器）见 [模板参考](/zh/posts/template-reference/)。
