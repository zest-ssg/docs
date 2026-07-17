# Collections, Data & Pagination

Zest provides a rich API for querying pages, grouping them into collections, and working with global data — all accessible from `.zest.fsx` scripts and layout templates.

---

## Page Collections

Pages are automatically grouped into collections based on their URL structure (first URL segment) and their tags.

### In DSL Scripts

```fsharp
open DslCollections

// All pages
let all = site_pages ()

// Recent posts (sorted by date descending)
let recent = recent_pages 5

// Pages by tag
let fsharpPosts = pages_by_tag "fsharp"

// Pages by directory
let blogPosts = pages_by_dir "blog"

// Pages by collection (first URL segment)
let docsPages = pages_by_collection "docs"

// Search pages
let results = search_pages "tutorial"

// All unique tags
let tags = all_tags ()

// All collection names
let collections = all_collections ()

// Total page count
let count = page_count ()
```

### In Nunjucks Layouts

The `pages`, `tags`, and `collections` variables are injected into Nunjucks context:

```njk
{% for page in pages | recent(5) %}
  <article>
    <h2><a href="{{ page.url }}">{{ page.title }}</a></h2>
    <time>{{ page.date }}</time>
  </article>
{% endfor %}
```

Using custom filters:

```njk
{% for page in pages | pages_by_tag("fsharp") %}
  ...
{% endfor %}

{% for page in pages | by_collection("blog") %}
  ...
{% endfor %}
```

---

## Page Record Shape

Each page in the DSL is an anonymous record:

```fsharp
{|
    url: string           // e.g. "/posts/hello-world/"
    title: string         // Page title
    date: string          // Date as string ("" if none)
    slug: string          // URL slug
    description: string   // Meta description
    tags: string[]        // Tag strings array
|}
```

---

## Sorting & Filtering

```fsharp
open DslCollections

// Sort by field
let byTitle = sort_pages_by "title" "asc"
let byDate  = sort_pages_by "date" "desc"
let bySlug  = sort_pages_by "slug" "asc"

// Custom filter
let featured = filter_pages_by (fun p ->
    p.title.Contains("Feature"))
```

---

## Grouping

### Group by Year

```fsharp
open DslCollections

let groups = group_pages_by_year ()
// [(2026, [page1; page2]); (2025, [page3])]

// Render in page:
for (year, pages) in groups do
    h2 [ text year ]
    for p in pages do
        divC "post-item" [
            a p.url [ text p.title ]
        ]
```

### Group by Tags

Group in Nunjucks:

```njk
{% for tag, taggedPages in tags %}
  <section>
    <h2>{{ tag }} ({{ taggedPages | length }})</h2>
    {% for page in taggedPages %}
      <a href="{{ page.url }}">{{ page.title }}</a>
    {% endfor %}
  </section>
{% endfor %}
```

---

## Related Pages

Find pages related by shared tags, excluding the current page:

```fsharp
open DslCollections

let related = related_pages "/posts/my-post/" 3
```

---

## Tag Cloud

```fsharp
open DslCollections

let cloud = tag_cloud 2  // minimum count of 2

// Render:
for (tag, count) in cloud do
    spanC "tag" [
        a ("/tags/" + tag + "/") [ text (tag + " (" + string count + ")") ]
    ]
```

---

## Pagination

### `PaginatedResult`

```fsharp
type PaginatedResult<'a> = {
    Items: 'a list
    CurrentPage: int
    TotalPages: int
    TotalItems: int
    PreviousUrl: string option
    NextUrl: string option
    PageUrl: int -> string
}
```

### `paginate` Function

```fsharp
let paginate (items: 'a list) (pageSize: int) (pageUrl: int -> string) (currentPage: int) : PaginatedResult<'a>
```

### Usage in DSL

```fsharp
page {
    title "Blog — Page 2"

    let allPosts = recent_pages 50
    let paged = paginate (List.ofArray allPosts) 10 (fun p -> "/blog/page/" + string p + "/") 2

    for post in paged.Items do
        divC "post-card" [
            h2 [ a post.url [ text post.title ] ]
            p  [ text post.description ]
        ]

    // Pagination navigation
    match paged.PreviousUrl with
    | Some url -> a url [ text "← Previous" ]
    | None -> raw ""

    match paged.NextUrl with
    | Some url -> a url [ text "Next →" ]
    | None -> raw ""
}
```

### `renderPagination` Helper

```fsharp
renderPagination (paged: PaginatedResult<'a>) (linkClass: string) (activeClass: string) : HtmlNode list
```

Renders a navigation bar with Previous/Next links and numbered page links.

---

## Global Data (`_data/`)

### Loading Data

Data files in `_data/` are TOML files loaded at build time. Each file becomes a namespace:

```toml
# _data/social.toml
[twitter]
handle = "@mysite"
url = "https://twitter.com/mysite"

[github]
url = "https://github.com/user/repo"
```

### In DSL Scripts

```fsharp
open DslCollections

// Single value
let twitter = site_data "social.twitter.handle"

// All values under a prefix
let social = site_section "social"
// → dict ["twitter.handle", "@mysite"; "twitter.url", "https://..."; ...]

for kv in social do
    p [ text (kv.Key + ": " + kv.Value) ]
```

### In Templates

```
{{ site.social.twitter.handle }}
{{ site.social.github.url }}
```

### In Nunjucks

```njk
<a href="{{ site.social.twitter.url }}">{{ site.social.twitter.handle }}</a>
```

---

## Includes as Data

Include partials are accessible in `.zest.fsx` scripts:

```fsharp
open DslCollections

let headerHtml = include_partial "header"
let footerHtml = include_partial "footer"
```

Includes are also available as direct variables in layouts:

```
{{ include header }}
```

---

## `_init.zest.fsx` — Global Initialization

The init script runs before each build and can populate global data programmatically:

```fsharp
// _init.zest.fsx

// Load external JSON
let apiData = loadJson "https://api.example.com/stats"

// Load local TOML
let config = loadToml "config/site.toml"

// Environment variable
let buildEnv = loadEnv "BUILD_ENV"

// Add to global data
addGlobal ("stats.visitors", apiData.visitors)
addGlobal ("build.env", buildEnv)
addGlobal ("build.time", System.DateTime.UtcNow.ToString("o"))

console_log ("Init complete. Build env: " + buildEnv)
```

All data added via `addGlobal` is available as `{{ site.stats.visitors }}`, etc.

---

## Menu Data

Menus defined in `_config.toml` are available as JSON string data:

```fsharp
// Access in DSL
let mainMenu = site_data "menu.main"
// → [{"label":"Home","url":"/","weight":1},{"label":"Blog","url":"/posts/","weight":2}]

// Parse and render
let menuItems = System.Text.Json.JsonSerializer.Deserialize<{| label: string; url: string; weight: int |}[]>(mainMenu)
```

In templates:

```
{{ menu.main }}
```

---

## `ContentMeta` Fields (Frontmatter)

Page metadata is parsed by `MetaParser` from three formats. **TOML (`+++`) is always tried first.** If no TOML block is present, the fallback parser depends on the file extension: *template* extensions (`.njk`, `.liquid`, `.hbs`, `.mustache`, `.webc`, `.haml`, `.pug`) use the **HTML-comment** parser; all other extensions (`.zest.fsx`, `.fsx`, `.md`, …) use the **F#-comment** parser. For `.zest.fsx` files the **F# comment header** format is therefore the primary and recommended approach.

| Field | TOML Key | F# Comment Key | Description |
|---|---|---|---|
| `Layout` | `layout` | `@layout` | Layout template name |
| `Title` | `title` | `@title` | Page title |
| `Permalink` | `permalink` | `@permalink` | Custom permalink |
| `Tags` | `tags` (array) | `@tags` / `@tag` | Content tags (space-separated or repeated) |
| `Date` | `date` | `@date` | Publication date |
| `Description` | `description` | `@description` | SEO/social description |
| `Draft` | `draft` (bool) | `@draft` | Draft status (skipped in production) |
| `Author` | `author` | `@author` | Author name |
| `Updated` | `updated` | `@updated` | Last modified date |
| `Weight` | `weight` (int) | `@weight` | Sort weight (lower = first) |
| `Template` | `template` | `@template` | Explicit template override |
| `Collection` | `collection` | `@collection` | Collection name |
| `Extra` | *(any unrecognized)* | *(any unrecognized)* | Arbitrary extra metadata |

### Frontmatter Formats

**F# Comment Headers** (`.zest.fsx` files — recommended):

```fsharp
// @title  My Page Title
// @layout default
// @permalink /about/
// @description About this site
// @date 2026-01-15
// @tags fsharp static-site
// @draft false
// @author Jane Doe

page {
    h1 [ text "About" ]
    p  [ text "Content goes here." ]
}
```

**TOML** (`.md`, `.njk`, layout files):

```markdown
+++
title = "My Post"
date = 2026-01-15
tags = ["fsharp", "static-site"]
layout = "post"
draft = false
+++

# Content starts here
```

**HTML Comments** (`.html`, `.njk` template files):

```html
<!-- @title My Page -->
<!-- @layout default -->
<!-- @description A page built with Zest -->

<article>
    {{ content }}
</article>
```

The parser is called in `ContentPipeline` and tries TOML first, then HTML comments for template files, then F# comment headers for everything else (`.zest.fsx`, `.fsx`, etc.).
