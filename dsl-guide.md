# Page DSL Guide

Zest pages are `.zest.fsx` F# scripts evaluated by `dotnet fsi` with the pre-compiled **Zest.Dsl** library loaded. Each script is a **page computation expression** (`page { }`) whose operations set metadata and emit an HTML body.

## Structure

```fsharp
// @title  My Page Title
// @layout default
// @description Page description for SEO and social
// @date 2026-01-15
// @tags fsharp static-site

page {
    h1 [ text "Hello World" ]
    p  [ text "This is a paragraph." ]
}
```

Two ways to declare metadata:

1. **F# comment headers** (`// @key value`) — parsed by `MetaParser` for `.zest.fsx`/`.fsx`/`.md` files and populated into the `ContentMeta` record *before* the CE runs. Recommended.
2. **CE operations** (`title "..."`, `layout "default"`, …) — equivalent, but comment headers keep metadata separate from content.

---

## Frontmatter Formats

`MetaParser` recognizes three header formats on every content file:

| Format | Delimiters | Typical files |
|---|---|---|
| **TOML** | `+++` … `+++` on their own lines | `.md`, `.njk`, layouts, data-ish |
| **F# comment** | `// @key value` | `.zest.fsx`, `.fsx` |
| **HTML comment** | `<!-- @key value -->` | `.html`, `.njk`, `.liquid`, `.hbs`, `.mustache`, `.webc`, `.haml`, `.pug` |

**Resolution order:** TOML is always tried first. If no TOML block is found, the parser chosen depends on the file extension — *template* extensions (`.njk`, `.liquid`, `.hbs`, `.mustache`, `.webc`, `.haml`, `.pug`) use the **HTML-comment** parser; all other extensions use the **F#-comment** parser.

### Known keys

`layout`, `title`, `permalink`, `description`, `date`, `tags`/`tag`/`categories`, `draft`, `author`, `updated`, `weight`/`order`, `template`, `collection`. Any unrecognized key is stored in the page's `Extra` map.

| Key | CE op / TOML | Type | Notes |
|---|---|---|---|
| `@title` | `title` | string | Page title |
| `@layout` | `layout` | string | Layout name (no extension) |
| `@permalink` | `permalink` | string | Custom URL override |
| `@description` | `description` | string | SEO/social description |
| `@date` | `date` | date | Parsed via `DateTime.TryParse` |
| `@tags` / `@tag` | `tags` / `tag` | list | Space-comma separated; `@tag` appends |
| `@categories` | (maps into `tags`) | list | Treated as tags |
| `@draft` | — | bool | `true`/`yes`/`1` → skipped in production |
| `@author` | `author` | string | Author name |
| `@updated` | — | date | Last modified |
| `@weight` / `@order` | `weight` | int | Sort order (lower = first) |
| `@template` | `template` | string | Explicit template override |
| `@collection` | `collection` | string | Collection/group name |

Multi-line values are supported in F# headers: a `// @key` line with an empty value continues onto the next non-`//` line.

---

## `page { }` Operations

All operations below are custom operations on the `PageBuilder` computation expression.

### Metadata

| Operation | Signature | Description |
|---|---|---|
| `title` | `string -> ContentPage` | Page title |
| `layout` | `string -> ContentPage` | Layout name |
| `permalink` | `string -> ContentPage` | Custom URL (also sets `Url`) |
| `slug` | `string -> ContentPage` | URL slug |
| `tags` | `string list -> ContentPage` | Replace tags |
| `tag` | `string -> ContentPage` | Append a single tag |
| `date` | `string -> ContentPage` | Publication date |
| `description` | `string -> ContentPage` | Sets `page.description` |
| `author` | `string -> ContentPage` | Sets `page.author` |
| `category` | `string -> ContentPage` | Sets `page.category` |
| `thumbnail` | `string -> ContentPage` | Sets `page.thumbnail` |
| `source` | `string -> ContentPage` | Source path (programmatic pages) |
| `redirect_from` | `string -> ContentPage` | Alias/redirect URL |

### Data (CE-only)

| Operation | Signature | Description |
|---|---|---|
| `data` | `key: string -> value: obj -> ContentPage` | Arbitrary key-value, available as `{{ page.key }}` |
| `data_from` | `(string * obj) list -> ContentPage` | Bulk-import a key-value list |

### Content

| Operation | Signature | Description |
|---|---|---|
| *(implicit yield)* | `string` \| `HtmlNode` \| `HtmlNode list` | Appended as content |
| `content` | `HtmlNode list -> ContentPage` | Set content from a node list |
| `append` | `HtmlNode list -> ContentPage` | Append nodes |
| `prepend` | `HtmlNode list -> ContentPage` | Prepend nodes |
| `if_content` | `cond: bool -> nodes: HtmlNode list -> ContentPage` | Append only if `cond` |
| `match_content` | `(bool * HtmlNode list) list -> ContentPage` | First matching branch |
| `for_each` | `items: 'a list -> render: ('a -> HtmlNode) -> ContentPage` | Map each item to a node |
| `for_pages` | `pages: 'a list -> render: ('a -> HtmlNode) -> ContentPage` | Same as `for_each` (page-oriented) |
| `for_range` | `start: int -> endInclusive: int -> render: (int -> HtmlNode) -> ContentPage` | Integer range |
| `repeat` | `count: int -> nodes: HtmlNode list -> ContentPage` | Repeat nodes `count` times |
| `spaced` | `sep: HtmlNode -> items: HtmlNode list -> ContentPage` | Intersperse `sep` between items |
| `raw_html` | `html: string -> ContentPage` | Inject raw (unescaped) HTML |
| `css` | `cssText: string -> ContentPage` | Inject a `<style>` block |
| `js` | `jsText: string -> ContentPage` | Inject a `<script>` block |

### Syntactic Sugar

| Operation | Signature | Description |
|---|---|---|
| `when'` | `cond: bool -> nodes: HtmlNode list -> ContentPage` | Append when true (alias of `if_content`) |
| `unless` | `cond: bool -> nodes: HtmlNode list -> ContentPage` | Append when false |
| `choose_content` | `cond: bool -> ifTrue: HtmlNode list -> ifFalse: HtmlNode list -> ContentPage` | Ternary content |
| `output` | `path: string -> ContentPage` | Override output file path |

> `for x in items do …` (F# `for`) and `if … then … else …` also work directly inside the CE via the builder's `For`/`If` support.

---

## HTML Building

Two layers are available (details in [dsl-api.md](dsl-api.md) and [dsl-style.md](dsl-style.md)):

- **`Dsl` (string builders)** — always open; every function returns `string` HTML.
- **Engine HTML modules** (`HtmlElements`/`HtmlAttributes`) — return `HtmlNode` discriminated unions; used internally and when you reference the engine assemblies.

```fsharp
page {
    title "Hello"

    // Text and raw HTML
    text "This is escaped text"
    raw  "<strong>Raw, not escaped</strong>"

    // Block + void elements
    h1 [ text "Heading 1" ]
    p  [ text "Paragraph" ]
    img "/img/photo.jpg" "A beautiful photo"
    br ()

    // Class shortcuts (append a class attribute)
    divC "container" [ text "Content" ]
    spanC "badge" [ text "New" ]

    // Lists and links
    ul [ li [ text "A" ]; li [ text "B" ] ]
    a "/about/" [ text "About" ]
    aBlank "https://github.com" "GitHub (new tab)"
}
```

---

## Control Flow & Loops

```fsharp
// @title Control Flow

page {
    when' (showBanner) [
        divC "banner" [ text "Special offer!" ]
    ]

    unless (isDraft) [
        p [ text "Published content" ]
    ]

    choose_content (isLoggedIn)
        [ p [ text "Welcome back!" ] ]
        [ p [ text "Public page" ] ]

    match_content [
        (pageType = "blog", [ article [ text "Blog layout" ] ])
        (pageType = "docs", [ nav [ text "Docs sidebar" ]; main [ text "Docs content" ] ])
    ]

    for_each (recent_pages 5) (fun p ->
        divC "post-card" [ h2 [ text p.title ]; p [ text p.description ] ])

    for_range 1 10 (fun i ->
        p [ text ("Item " + string i) ])

    repeat 3 [ spanC "star" [ text "*" ] ]

    spaced (hr ()) [ span [ text "A" ]; span [ text "B" ]; span [ text "C" ] ]
}
```

---

## Styling

Two CSS authoring paths (see [dsl-style.md](dsl-style.md) and [zcss.md](zcss.md)):

### F#-native stylesheet CE

```fsharp
open DslCss
open DslCss.Selectors

let theme = stylesheet {
    body [ bg "#f0f0f0"; color "#333"; font_family "'Inter', sans-serif" ]
    a.hover [ color "#0ff" ]
    cls "container" [ max_width "1200px"; margin "0 auto" ]
}

page {
    styleZcss theme
}
```

### Inline / scoped / external

```fsharp
page {
    styleCss ".hero { background: #667eea; padding: 2rem; }"   // raw CSS in <style>
    let styleBlock, scopeAttr = styleScoped ".title { color: red; }"
    div [ attr scopeAttr "" ] [ h1C "title" [ text "Red heading" ] ]
    styleExternal "css/theme.zcss"   // <link> to a .zcss (compiled to .css)
}
```

---

## SEO & Social Tags

```fsharp
open DslSeo

page {
    raw (String.concat "\n" (meta_tags "My Page" "Description" "https://mysite.com/page/" "/img/og.png" "My Site"))
    raw (String.concat "\n" (open_graph_tags "My Page" "Description" "https://mysite.com/page/" "/img/og.png" "article"))
    raw (String.concat "\n" (twitter_card_tags "summary_large_image" "My Page" "Description" "/img/twitter.png" "@mysite"))
    raw (canonical_url "https://mysite.com/page/")
}
```

---

## Site & Page Data

```fsharp
page {
    // Custom page data → {{ page.custom_field }}
    data "custom_field" "my value"
    data "priority" 5

    // Read global data
    let twitter = site_data "site.social_twitter"
    p [ text ("Follow us: " + twitter) ]

    let social = site_section "site.social"
    for kv in social do
        p [ text (kv.Key + ": " + kv.Value) ]
}
```

See [collections.md](collections.md) for the page-query API (`recent_pages`, `pages_by_tag`, `search_pages`, `paginate`, …) and [dsl-api.md](dsl-api.md) for the full module reference.

---

## XML Feeds

```fsharp
open DslXml

// @output rss.xml
page {
    let pages = recent_pages 20 |> Array.map (fun p ->
        {| url = p.url; title = p.title; date = p.date; description = p.description |})
    raw (rss_xml siteTitle siteUrl siteDescription pages)
}
```

---

## Debugging & Guards

```fsharp
open ContentGuard

page {
    validate (title <> "") "Title is required" ""
    require "author" authorName
    trace "page_count" (page_count ())
}
```
