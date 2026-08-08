+++
title = "Zest DSL Guide"
description = "Write pages with the F# DSL: render, element builders, conditions, loops and common patterns."
category = "dsl"
tags = ["zest", "dsl", "fsharp"]
date = 2026-08-01
+++

# Zest DSL Guide

The Zest DSL is Zest's native template language: real F#. A `.zest.fsx` page is a script evaluated by `dotnet fsi` with the pre-compiled `Zest.Dsl` library loaded. The DSL composes HTML as plain strings, and `render` prints the result to stdout, which becomes the page body. This guide is the entry point for the **Zest DSL** section; the [API reference](/en/posts/dsl-api/) catalogs every function, and [DslCollections](/en/posts/dsl-collections/) covers the page-query API.

## Page Structure

Metadata is declared as `// @key value` comment headers; the body is built in a `page { }` computation expression:

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

Comment headers are parsed by `MetaParser` and populate the page's metadata *before* the CE runs. The same keys can be set with CE operations (`title "..."`, `layout "default"`) when you prefer metadata alongside content.

## Building HTML

Element builders are plain functions over string children:

```fsharp
page {
    title "Hello"

    // Text and raw HTML
    text "This is escaped text"
    raw  "<strong>Raw, not escaped</strong>"

    // Block and void elements
    h1  [ text "Heading 1" ]
    p   [ text "Paragraph" ]
    img "/img/photo.jpg" "A beautiful photo"
    br ()

    // Class shortcuts append a class attribute
    divC "container" [ text "Content" ]
    spanC "badge" [ text "New" ]

    // Lists and links
    ul [ li [ text "A" ]; li [ text "B" ] ]
    a "/about/" [ text "About" ]
    aBlank "https://github.com" "GitHub (new tab)"
}
```

- `text` HTML-encodes its argument; `raw` emits it verbatim.
- `elem "tag" attrs children` is the generic builder; `voidElem "br" attrs` covers void elements.
- Attribute helpers such as `cls`, `id'`, `href`, `data'`, and `aria` produce `key="value"` strings.

## Inline Markdown

The `md` helper renders a Markdown string to HTML, so prose can sit directly inside a DSL tree:

```fsharp
page {
    divC "about" [
        md """
# About

This page mixes **Markdown** with the F# HTML DSL.
"""
    ]
}
```

## Conditions and Loops

The `page { }` builder provides CE operations for control flow, and ordinary F# `for`/`if` also work inside the CE:

```fsharp
page {
    when' (showBanner) [ divC "banner" [ text "Special offer!" ] ]
    unless (isDraft)   [ p [ text "Published content" ] ]

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
    spaced (hr ()) [ span [ text "A" ]; span [ text "B" ] ]
}
```

## Common Patterns

**Custom page data** — set via `data` and read in templates as `{{ page.<key> }}`:

```fsharp
page {
    data "priority" 5
    data "custom_field" "my value"
}
```

**SEO and social tags** (see `DslSeo`):

```fsharp
open DslSeo

page {
    raw (String.concat "\n" (meta_tags "My Page" "Description" "https://mysite.com/page/" "/img/og.png" "My Site"))
    raw (String.concat "\n" (open_graph_tags "My Page" "Description" "https://mysite.com/page/" "/img/og.png" "article"))
}
```

**Inline styles and scripts**:

```fsharp
page {
    styleCss ".hero { background: #667eea; padding: 2rem; }"
    js "document.querySelector('.btn').addEventListener('click', () => alert('hi'))"
}
```

**XML feeds** via `DslXml`:

```fsharp
open DslXml

// @output rss.xml
page {
    let pages = recent_pages 20 |> Array.map (fun p ->
        {| url = p.url; title = p.title; date = p.date; description = p.description |})
    raw (rss_xml siteTitle siteUrl siteDescription pages)
}
```

## Frontmatter Formats

`MetaParser` recognizes three header formats on every content file. TOML (`+++`) is always tried first; template extensions fall back to HTML comments, everything else to F# comments:

```markdown
+++
title = "My Post"
date = 2026-01-15
tags = ["fsharp", "static-site"]
layout = "post"
+++
```

```html
<!-- @title My Page -->
<!-- @layout default -->
```

The known keys are `layout`, `title`, `permalink`, `description`, `date`, `tags`/`tag`/`categories`, `draft`, `author`, `updated`, `weight`/`order`, `template`, and `collection`; any other key lands in the page's `Extra` map.

For the full function catalog — `DslHtml` element builders, `DslComponents` (alerts, badges, icons), `DslHelpers` (`js`, `jsonBlock`, `md`), `DslSeo`, `DslXml`, and the styling modules — see the [DSL API reference](/en/posts/dsl-api/).
