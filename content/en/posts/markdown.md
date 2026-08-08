+++
title = "Markdown"
description = "Author pages in Markdown with TOML frontmatter: headings, fenced code, tables, images and links."
category = "templates"
tags = ["zest", "markdown", "content"]
date = 2026-08-01
+++

# Markdown

Markdown is the primary authoring format for prose content in Zest. A `.md` or `.markdown` file in the content directory is parsed by the built-in Markdown engine, wrapped in its declared layout, and written out as HTML. This page is part of the **Templates** section of the documentation; it describes the supported syntax and the engine's deliberate boundaries.

## Frontmatter

Markdown files declare metadata in TOML frontmatter delimited by `+++` lines. TOML is always tried first by the metadata parser:

```markdown
+++
title = "About"
layout = "default"
date = 2026-01-15
tags = ["meta", "zest"]
description = "A short page description."
+++

# About

Body content starts here.
```

Recognized keys include `title`, `layout`, `permalink`, `date`, `tags`, `draft`, `author`, `updated`, `weight`, `template`, and `collection`; any other key is kept as extra page metadata and exposed as `page.<key>`. If `title` is missing, the first `# Heading` in the body is used.

## Headings

ATX headings produce `<h1>` through `<h6>` with an auto-generated `id` anchor (derived from the heading text) so in-page links work:

```markdown
## A Section
### A Subsection
```

## Fenced Code Blocks

Fenced code blocks with a language label render as `<pre><code class="language-...">`:

````markdown
```fsharp
page {
    h1 [ text "Hello" ]
}
```
````

There is **no built-in syntax highlighting** — the language class is emitted for your theme's client-side highlighter to consume.

## Tables

GFM-style tables are supported:

```markdown
| Command | Description |
|---|---|
| `build` | Build the site |
| `serve` | Run the dev server |
```

## Inline Formatting

Standard inline syntax works: `**bold**`, `*italic*`, `` `code` ``, `[links](/en/posts/configuration/)`, and images:

```markdown
![Alt text](/assets/img/example.png)
```

## Links

Links between pages should use absolute paths as rendered by the permalink router, without the `.md` extension:

```markdown
See the [configuration guide](/en/posts/configuration/).
```

## Engine Boundaries

The built-in Markdown engine is deliberately focused. It supports the constructs above (headings with anchors, paragraphs, fenced code blocks, tables, lists, inline formatting, images, and links) but does not attempt full CommonMark coverage or extended features:

- **No syntax highlighting** — language classes are emitted, highlighting is left to the theme.
- **No built-in table of contents** — this documentation theme generates the right-hand "On this page" TOC with client-side JavaScript from the page's `h2`/`h3` headings.
- **No frontmatter-driven TOC or footnote extensions** — complex content can be built as a [Zest DSL](/en/posts/zest-dsl/) page instead, where the `md` helper lets you embed Markdown inside an F# script.

## Layouts and Partials

The `layout` frontmatter key selects the wrapping layout by name (file stem, extension omitted). If omitted, the site's `default_layout` is used. Markdown content pages can also be routed to specific layouts via `[[defaults]]` in `_config.toml`, as the documentation site does for `content/en/posts/*`.

## When to Use It

Use Markdown for anything prose-heavy: blog posts, guides, documentation pages. Use the [Zest DSL](/en/posts/zest-dsl/) when you need programmatic content, query the site (see [dsl-collections](/en/posts/dsl-collections/)), or want type-safe markup, and [Nunjucks](/en/posts/nunjucks/) when you need template logic in a page.
