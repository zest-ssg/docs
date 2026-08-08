+++
title = "Native Template Mode"
description = "Zest's native template mode: .zest.fsx F# DSL pages plus {{ placeholder }} substitution in layouts."
category = "templates"
tags = ["zest", "native-mode", "templates", "fsharp"]
date = 2026-08-01
+++

# Native Template Mode

Native template mode is Zest's default authoring model: **pages are real F# scripts** (`.zest.fsx`) evaluated by F# Interactive, and **layouts are HTML templates** that receive the rendered page body through `{{ placeholder }}` substitution. This page is part of the **Templates** section of the documentation.

## The Mental Model

If you know Eleventy (11ty), the model is familiar. In Eleventy, a `.njk` or `.md` file is a template that receives data (`page`, `site`, `collections`) and renders to HTML. In Zest's native mode, a `.zest.fsx` file plays that role: it is a script that produces HTML, with the same data available through the DSL context — `page.*`, `site.*`, and the page-query API from [DslCollections](/en/posts/dsl-collections/).

The template engine label `engine = "native"` under `[template]` in `_config.toml` names this primary language. As with every engine label, it is a **pure annotation**: routing is decided by file extension, so `.njk`, `.liquid`, and `.hbs` files keep using their own engines regardless of the label.

## Pages: `.zest.fsx`

A native page is an F# script. Metadata comes from `// @key value` comment headers; the body is built with the DSL and printed with `render`:

```fsharp
// @title Welcome
// @layout default
// @description A native-mode page

open Zest.Dsl

render [
    h1 [ text "Hello" ]
    p  [ text "Pages: " ; strong [ text (string (page_count ())) ] ]
]
```

The DSL is type-safe and programmable — loops, conditionals, queries over the site's pages, and inline Markdown via the `md` helper are all ordinary F#. See the [Zest DSL guide](/en/posts/zest-dsl/) for the full language.

## Layouts: `{{ placeholder }}`

Layouts live in `_layouts/` and are selected by name via `// @layout default` or frontmatter `layout = "default"`. HTML layouts use placeholder substitution:

```html
<!-- _layouts/default.html -->
<!DOCTYPE html>
<html lang="{{ site.language }}">
<head>
    <meta charset="utf-8" />
    <title>{{ page.title }} — {{ site.title }}</title>
</head>
<body>
    {{ include header }}
    <main>
        {{ content }}
    </main>
    {{ include footer }}
</body>
</html>
```

The available placeholders are:

| Placeholder | Source |
|---|---|
| `{{ content }}` | The page's rendered body HTML |
| `{{ page.title }}` / `{{ page.url }}` / `{{ page.date }}` / `{{ page.slug }}` / `{{ page.tags }}` / `{{ page.description }}` | Page metadata |
| `{{ site.title }}` / `{{ site.base_url }}` / `{{ site.author }}` / `{{ site.language }}` / `{{ site.version }}` | Site configuration |
| `{{ site.<namespace>.<key> }}` | Global data from `_data/` and `_init.zest.fsx` |
| `{{ include name }}` | Include a partial from `_includes/` |

Any `{{ page.<key> }}` for custom data set via `page { data "key" value }` also resolves.

## Layouts Can Be F# Too

A layout itself may be a `.zest.fsx` script. It runs in its own FSI session with `content`, `page`, and `site` injected as top-level bindings, and its stdout becomes the rendered page:

```fsharp
// _layouts/page.zest.fsx
html [ lang site.language ] [
    head [] [ title [] [ str page.title ] ]
    body [] [ main [] [ raw content ] ]
]
```

The documentation site you are reading uses this approach — the three-column docs layout is a `.zest.fsx` script.

## Relationship to the F# DSL

Native mode and the F# DSL are the same thing viewed from two angles:

- The **DSL** is the toolset — element builders (`h1`, `divC`, `a`), helpers (`js`, `jsonBlock`, `md`), and the page-query API.
- **Native mode** is the workflow built on it — `.zest.fsx` pages, placeholder layouts, and the F# Interactive evaluation loop.

Markdown, [Nunjucks](/en/posts/nunjucks/), and the other template formats coexist with native mode: route content to whichever format fits the task, or mix them in one build.
