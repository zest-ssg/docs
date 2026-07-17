# Page DSL Guide

Zest pages are written as `.zest.fsx` F# scripts. The script is evaluated by `dotnet fsi` with the pre-compiled **Zest.Dsl** library loaded.

## Basic Structure

Page metadata is declared as **F# comment headers** at the top of the file using `// @key value` syntax. The `page { }` computation expression (CE) contains the body content.

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

The comment headers are parsed by `MetaParser` and populate the `ContentMeta` record before the `page { }` CE runs. The CE produces the page body HTML.

## Frontmatter (`// @key value` Comment Headers)

Metadata is declared as contiguous `// @key value` lines at the top of the `.zest.fsx` file:

| Key | Type | Example | Description |
|---|---|---|---|
| `@title` | string | `// @title My Post` | Page title (browser tab, SEO, heading) |
| `@layout` | string | `// @layout default` | Layout template name (without extension) |
| `@permalink` | string | `// @permalink /about/` | Custom URL override |
| `@description` | string | `// @description A great post` | Meta description for SEO and social |
| `@date` | datetime | `// @date 2026-01-15` | Publication date |
| `@tags` | list | `// @tags fsharp static-site` | Space-separated tags |
| `@tag` | string | `// @tag fsharp` | Add a single tag (can repeat) |
| `@author` | string | `// @author Jane Doe` | Author name |
| `@updated` | datetime | `// @updated 2026-06-01` | Last modification date |
| `@weight` | int | `// @weight 5` | Sort weight (lower = first) |
| `@template` | string | `// @template custom` | Explicit template override |
| `@collection` | string | `// @collection blog` | Collection / group name |
| `@draft` | bool | `// @draft true` | Skip in production build |
| `@categories` | list | `// @categories tutorial` | Category names |

Multi-line values (e.g., long description) are supported: a `// @key` line with no value continues onto the next non-meta line.

### Alternative: CE Operations

The `page { }` CE also accepts metadata as custom operations, though the **comment header approach is recommended** for cleaner separation of metadata from content:

```fsharp
page {
    title "My Page Title"       // also works, but @title comment is preferred
    layout "default"
    description "SEO description"
    // ...
}
```

The `page { }` CE supports the same custom operations: `title`, `layout`, `permalink`, `slug`, `description`, `date`, `tags`, `tag`, `author`, `category`, `thumbnail`, `source`, `redirect_from`.

## Data (CE-only)

These operations are only available inside `page { }`:

| Operation | Description |
|---|---|
| `data key value` | Add arbitrary key-value data accessible in layout templates |
| `data_from dict` | Import a `IDictionary<string, obj>` of data |

## Content Rendering

| Operation | Description |
|---|---|
| `Yield` | Implicit — any `string`, `HtmlNode`, or `HtmlNode list` is added as content |
| `content nodes` | Explicitly set content from `HtmlNode list` |
| `append nodes` | Append nodes to content |
| `prepend nodes` | Prepend nodes before content |

### Directives

| Operation | Description |
|---|---|
| `output path` | Specify output file path (overrides default routing) |

## HTML Building

Zest provides two layers of HTML builders:

### 1. DSL Layer (Dsl.fs) — Simple string builders for FSI scripts

All functions in `Dsl.fs` return `string` typed HTML. These are always available in `.zest.fsx` scripts.

```fsharp
page {
    title "Hello"

    // Text and raw HTML
    text "This is escaped text"
    raw  "<strong>This is raw HTML (not escaped)</strong>"

    // Void elements
    img "/img/photo.jpg" "A beautiful photo"
    br ()
    hr ()

    // Block elements
    h1 [ text "Heading 1" ]
    h2 [ text "Heading 2" ]
    p  [ text "Paragraph text." ]
    div [ text "A div" ]
    section [ text "Section content" ]
    article [ text "Article content" ]
    nav [ text "Nav content" ]
    header [ text "Header content" ]
    footer [ text "Footer content" ]

    // Lists
    ul [
        li [ text "Item 1" ]
        li [ text "Item 2" ]
    ]
    ol [
        li [ text "First" ]
        li [ text "Second" ]
    ]

    // Links
    a "/about/" [ text "About" ]
    aBlank "https://github.com" "GitHub (new tab)"
    aHref "/contact/" "Contact"

    // Tables
    table [
        thead [ tr [ th [ text "Name" ]; th [ text "Age" ] ] ]
        tbody [ tr [ td [ text "Alice" ]; td [ text "30" ] ] ]
    ]

    // Class shortcuts (appends class attribute)
    divC "container" [ text "Content" ]
    spanC "badge" [ text "New" ]
    pC "lead" [ text "Lead paragraph" ]
    sectionC "hero" [ text "Hero Section" ]
    h1C "title" [ text "Title" ]
    imgC "avatar" "/img/me.jpg" "Profile photo"
    aC "btn" "/signup/" [ text "Sign Up" ]

    // Styled text
    strong [ text "Bold text" ]
    em [ text "Italic text" ]
    code [ text "code()" ]
    small [ text "small text" ]
    mark [ text "highlighted" ]
    del [ text "deleted" ]
    abbr "Cascading Style Sheets" [ text "CSS" ]

    // Blockquotes and pre
    blockquote [ text "Someone said this" ]
    pre [ text "preformatted text" ]
    codeBlock "fsharp" "let x = 1"

    // Document structure
    raw doctype         // <!DOCTYPE html>
    html [ ... ]
    head [ ... ]
    body [ ... ]
    title [ text "Page Title" ]
    meta [ attr "name" "viewport"; attr "content" "..." ]
    link "stylesheet" "/css/main.css"
    stylesheet "/css/main.css"       // shorthand
    script "/js/main.js"
    scriptInline "console.log('hi')"
    style "body { margin: 0; }"
}
```

### 2. Engine Layer (HtmlElements/HtmlAttributes/HtmlModifiers) — Rich `HtmlNode` builder

These modules in `Zest.Engine.Html` return `HtmlNode` discriminated unions and are used internally by the engine. They are available when writing `.zest.fsx` scripts that reference the engine assemblies.

---

## Content Control Flow

### Conditionals

```fsharp
// @title Conditional Demo

page {
    when' (showBanner) [
        divC "banner" [ text "Special offer!" ]
    ]

    unless (isDraft) [
        p [ text "Published content" ]
    ]

    choose_content [
        (isLoggedIn, [ p [ text "Welcome back!" ] ])
        (isPublic,   [ p [ text "Public page" ] ])
    ]
}
```

### Loops

```fsharp
// @title Loop Demo

page {
    for_each pages [
        divC "post-card" [
            h2 [ text "{{ page.title }}" ]
            p  [ text "{{ page.description }}" ]
        ]
    ]

    for_range 1 10 [
        p [ text ("Item " + string (i)) ]
    ]

    repeat 3 [
        spanC "star" [ text "*" ]
    ]

    spaced [
        span [ text "A" ]
        span [ text "B" ]
        span [ text "C" ]
    ]
}
```

### Content Matching

```fsharp
// @title Pattern Match

page {
    match_content pageType [
        ("blog", [
            article [ text "Blog layout" ]
        ])
        ("docs", [
            nav [ text "Docs sidebar" ]
            main [ text "Docs content" ]
        ])
    ]
}
```

---

## Styling

Zest offers two approaches to writing CSS: the F#-native `stylesheet { }` CE (in `.zest.fsx` scripts) and the standalone `.zcss` preprocessor language. See [dsl-style.md](dsl-style.md) for the full CSS DSL and [zcss.md](zcss.md) for the ZCSS preprocessor.

### F#-Style ZCSS (Inline)

The `stylesheet { }` CE provides an F# computation expression for writing CSS using F#-native syntax — dot-notation selectors, typed property functions, and at-rules:

```fsharp
open DslCss
open DslCss.Selectors

let themeStyles = stylesheet {
    body [ bg "#f0f0f0"; color "#333"; font_family "'Inter', sans-serif" ]

    a.hover [ color "#0ff" ]
    p.first_line [ font_weight "bold" ]

    cls "container" [ max_width "1200px"; margin "0 auto"; padding "0 1rem" ]

    h1 [ font_size "2rem"; font_weight "700" ]
    h2 [ font_size "1.5rem"; font_weight "600" ]

    (input.attr_eq "type" "text") [ border "1px solid #ccc"; border_radius "4px" ]
    (div.descendant p) [ line_height "1.6" ]

    cls "card" [
        bg "#fff"
        border_radius "8px"
        box_shadow "0 2px 8px rgba(0,0,0,0.1)"
        padding "1.5rem"
    ]
}

page {
    styleZcss themeStyles
    // ...
}
```

### Inline CSS

```fsharp
page {
    styleCss ".hero { background: #667eea; padding: 2rem; }"
}
```

### Scoped CSS

```fsharp
page {
    let styleBlock, scopeAttr = styleScoped ".title { color: red; }"
    div [attr scopeAttr ""] [
        h1C "title" [ text "This heading is red" ]
    ]
}
```

### External Stylesheets

```fsharp
page {
    // Reference .zcss file (compiled to .css automatically)
    styleExternal "css/theme.zcss"

    // Or use the shorthand
    stylesheet "/css/main.css"
}
```

---

## SEO & Social Tags

```fsharp
open DslSeo

// @title My Page
// @layout default

page {
    // Meta tags
    raw (String.concat "\n" (meta_tags "My Page" "Description" "https://mysite.com/page/" "/img/og.png" "My Site"))

    // Open Graph
    raw (String.concat "\n" (open_graph_tags "My Page" "Description" "https://mysite.com/page/" "/img/og.png" "article"))

    // Twitter Card
    raw (String.concat "\n" (twitter_card_tags "summary_large_image" "My Page" "Description" "/img/twitter.png" "@mysite"))

    // Canonical URL
    raw (canonical_url "https://mysite.com/page/")

    // hreflang tags
    raw (hreflang_tag "en" "https://mysite.com/page/")
    raw (hreflang_tag "zh" "https://mysite.com/page/zh/")
}
```

---

## Data & Content Attributes

### Page-Level Data

```fsharp
// @title Data Demo

page {
    data "custom_field" "my value"
    data "priority" 5
    data "featured" true

    // These are accessible in layouts:
    // {{ page.custom_field }}
    // {{ page.priority }}
    // {{ page.featured }}
}
```

### Site Data Access

```fsharp
// @title Site Data

page {
    // Read global data directly
    let twitter = site_data "site.social_twitter"
    p [ text ("Follow us: " + twitter) ]

    // Get all data under a prefix
    let social = site_section "site.social"
    for kv in social do
        p [ text (kv.Key + ": " + kv.Value) ]
}
```

---

## Collections & Queries

See [collections.md](collections.md) for comprehensive collections documentation. Quick reference:

```fsharp
open DslCollections

// @title Blog Index

page {
    // Recent posts
    for p in recent_pages 5 do
        divC "post-item" [
            h2 [ text p.title ]
            p  [ text p.description ]
            a p.url [ text "Read more" ]
        ]

    // Pages by tag
    for p in pages_by_tag "fsharp" do
        divC "tagged-item" [ text p.title ]

    // All tags
    for tag in all_tags () do
        spanC "tag" [ text tag ]
}
```

---

## XML Feeds

```fsharp
open DslXml

// @title RSS Feed
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

// @title Debug Demo

page {
    // Validate a condition
    validate (title <> "") "Title is required" ""

    // Require a field
    require "author" authorName

    // Debug trace
    trace "page_count" (page_count ())
}
```
