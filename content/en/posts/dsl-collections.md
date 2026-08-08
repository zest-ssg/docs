+++
title = "DslCollections"
description = "The page-query API: site_pages, recent_pages, pages_by_tag, paginate, site_data and related functions."
category = "dsl"
tags = ["zest", "dsl", "collections", "pagination"]
date = 2026-08-01
+++

# DslCollections

`DslCollections` exposes the whole site to F# scripts as typed data, and provides the query, grouping, and pagination functions used to build listing pages, archives, tag clouds, and feeds. This page is part of the **Zest DSL** section; it documents each function with its signature and typical usage. Open the module with:

```fsharp
open DslCollections
```

## Page Record Shape

Every query returns the `PageInfo` anonymous record:

```fsharp
{| url: string          // e.g. "/posts/hello-world/"
   title: string        // page title
   date: string         // date as string ("" if none)
   slug: string         // URL slug
   description: string  // meta description
   tags: string[]       // tag strings
   author: string       // front-matter author ("" if absent)
   category: string     // front-matter category ("" if absent) |}
```

## Core Queries

| Function | Signature | Description |
|---|---|---|
| `site_pages` | `unit -> PageInfo[]` | All pages across the site |
| `recent_pages` | `n: int -> PageInfo[]` | N most recent pages (by date descending) |
| `pages_by_tag` | `tag: string -> PageInfo[]` | Pages carrying the tag (case-insensitive) |
| `pages_by_dir` | `dir: string -> PageInfo[]` | Pages whose URL contains the directory segment |
| `pages_by_collection` | `col: string -> PageInfo[]` | Pages in a collection (first URL segment) |
| `all_tags` | `unit -> string[]` | All unique tags, sorted |
| `all_collections` | `unit -> string[]` | All unique collection names, sorted |
| `search_pages` | `query: string -> PageInfo[]` | Case-insensitive title search |
| `page_count` | `unit -> int` | Total page count |

```fsharp
let all = site_pages ()
let recent = recent_pages 5
let fsharpPosts = pages_by_tag "fsharp"
let results = search_pages "tutorial"
let tags = all_tags ()
```

## Sorting, Filtering and Grouping

| Function | Signature | Description |
|---|---|---|
| `sort_pages_by` | `field: string -> direction: string -> PageInfo[]` | Sort by `"title"`, `"date"`, or `"slug"`; direction `"asc"`/`"desc"` |
| `filter_pages_by` | `pred: (PageInfo -> bool) -> PageInfo[]` | Custom predicate filter |
| `group_pages_by_year` | `unit -> (string * PageInfo list) list` | Group by publication year |
| `group_pages_by_month` | `unit -> (string * PageInfo list) list` | Group by `yyyy-MM` |
| `group_pages_by` | `field: string -> (string * obj list) list` | Group by `"tag"`, `"collection"`, or `"year"` |
| `pages_limit` | `n: int -> PageInfo[]` | First N pages |
| `pages_offset` | `n: int -> PageInfo[]` | Skip the first N pages |

```fsharp
let byTitle = sort_pages_by "title" "asc"
let featured = filter_pages_by (fun p -> p.title.Contains("Feature"))
let groups = group_pages_by_year ()
// [(2026, [page1; page2]); (2025, [page3])]
```

## Related Pages and Tags

| Function | Signature | Description |
|---|---|---|
| `related_pages` | `page_url: string -> count: int -> PageInfo[]` | Pages sharing tags with the given page (excluding itself) |
| `related_pages_by_category` | `page_url: string -> count: int -> PageInfo[]` | Pages in the same category |
| `tag_cloud` | `min_count: int -> (string * int) list` | Tag frequency pairs, filtered by minimum count |
| `tag_cloud_weighted` | `weight: float -> (string * float * int) list` | Weighted tag cloud |

```fsharp
let related = related_pages "/posts/my-post/" 3
let cloud = tag_cloud 2
for (tag, count) in cloud do
    spanC "tag" [ a ("/tags/" + tag + "/") [ text (tag + " (" + string count + ")") ] ]
```

## Pagination

`paginate` splits a sequence into pages with navigation metadata:

```fsharp
let paginate (perPage: int) (urlFor: int -> string) (items: 'a seq) : Page<'a> list
```

The `Page<'a>` record carries `Items`, `PageNumber` (1-based), `TotalPages`, `TotalItems`, `HasPrev`, `HasNext`, `PrevUrl`, and `NextUrl`.

```fsharp
page {
    title "Blog"

    let paged = paginate 10 (fun p -> "/blog/page/" + string p + "/")
                    (recent_pages 50 |> Array.toList)

    for page in paged do
        for post in page.Items do
            divC "post-card" [ h2 [ a post.url [ text post.title ] ] ]
}
```

`paginate_pages perPage urlFor` is a convenience wrapper that pages the site's pages in date-descending order. There is also a `renderPagination` helper that renders Previous/Next plus numbered page links.

## Data Lookup

| Function | Signature | Description |
|---|---|---|
| `site_data` | `key: string -> string` | Look up a global data value (`"social.twitter.handle"`) |
| `site_section` | `prefix: string -> IDictionary<string, string>` | All data keys under a prefix |
| `include_partial` | `name: string -> string` | Render an include partial by name |
| `get_page` | `url: string -> PageInfo option` | Look up a single page by URL |
| `get_collection` | `name: string -> PageInfo[]` | Alias for `pages_by_collection` |

```fsharp
let twitter = site_data "social.twitter.handle"
let social = site_section "social"
let headerHtml = include_partial "header"
```

## Templates View

The same data is injected into Nunjucks layouts as `pages`, `tags`, and `collections`, with matching custom filters (`pages_by_tag`, `recent`, `by_collection`, `search`, `where`). The [collections](/en/posts/collections/) page in **Features** shows the template-side usage; the full function signatures live in the [DSL API reference](/en/posts/dsl-api/).
