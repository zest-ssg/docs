+++
title = "Template Reference"
description = "Reference for template contexts, engine routing by file extension, layout nesting and the front-matter formats Zest accepts."
category = "reference"
tags = ["zest", "reference", "templates", "layouts", "frontmatter"]
date = 2026-08-01
+++

# Template Reference

This page is the reference for Zest's templating layer: the context objects available to templates, how files are routed to engines by extension, how layouts nest, and the three front-matter formats. The tutorials live in the Templates section — start with [Markdown](/en/posts/markdown/) or [native mode](/en/posts/native-mode/).

## Template Context

Content templates and layouts receive the following bindings.

| Key | Meaning |
|---|---|
| `site.title` | Site title from the config |
| `site.description` | Site description |
| `site.base_url` | Base URL (trailing `/` trimmed) |
| `site.version` | Version string |
| `site.author` | Author name |
| `site.language` | Primary language code |
| `site.params.*` | Values from the `[params]` table; dotted keys traverse nested dictionaries |
| `site.<data>` | Every key from `_data/` and `_init.zest.fsx` globals |
| `page.title` | Page title (front matter or derived) |
| `page.description` | Page description |
| `page.url` | Permalink URL |
| `page.date` | Date string, if set |
| `page.tags` | Array of tag strings |
| `page.<key>` | Any extra front-matter key |
| `content` | Rendered inner HTML (available in layouts) |
| `pages` | All pages as dictionaries (`url`, `title`, `date`, `tags`, `description`, …) |
| `tags` | All tag names |
| `collections` | All collection names |
| `collection` | Current collection name (generated listing pages) |
| `pagination` | Pagination window (see [collections](/en/posts/collections/)) |
| `term`, `term_pages`, `taxonomy` | Taxonomy archive extras (see [tags](/en/posts/tags/)) |

Raw global keys such as `pjaxScript` are also available without the `site.` prefix, both in layouts and in content templates.

## Engine Routing

Routing is decided purely by the file extension; the `template.engine` config value is a label only.

| Extension | Engine |
|---|---|
| `.zest.fsx`, `.fsx` | F# — evaluated by FSI; layouts receive `content`, `page` and `site` bindings |
| `.njk` | Nunjucks |
| `.html`, `.htm` | Nunjucks compat layer |
| `.liquid` | Converted to Nunjucks syntax, then rendered by Nunjucks |
| `.hbs`, `.mustache` | Standalone Hbs engine (native Mustache/Handlebars semantics) |
| `.haml` | Converted to HTML, then rendered by Nunjucks |
| `.pug` | Converted to HTML, then rendered by Nunjucks |
| `.md`, `.markdown` | Markdown converted to HTML (see [Markdown](/en/posts/markdown/)) |
| `.webc` | Recognized; minimal SSR normalization only — full support not implemented (see [WebC](/en/posts/webc/)) |

## Layout Nesting

A layout can declare a parent layout that receives its rendered output. Three directive forms are accepted at the top of the file:

```html
<!-- @layout base -->
```

```fsharp
// @layout base
```

```toml
+++
layout = "base"
+++
```

Layout resolution walks the chain until a layout without a parent is reached, or until the name repeats. The parent's `{{ content }}` placeholder receives the child's rendered HTML.

## Front-Matter Formats

| Format | Used by | Example |
|---|---|---|
| TOML `+++` block | `.md`, `.njk` and other text templates | `+++` `title = "..."` `tags = [...]` `+++` |
| HTML comments | `.html`, `.njk` templates | `<!-- @title Hello -->` `<!-- @layout base -->` |
| F# comments | `.zest.fsx` pages and layouts | `// @title Hello` `// @tags a, b` |

TOML front matter is parsed first; template extensions fall back to HTML comments, and F# scripts fall back to F# comments. Meta keys include `title`, `description`, `layout`, `permalink`, `date`, `draft` and `tags`; any other key is exposed to templates as `page.<key>`.

## Includes

Partials live in the includes dictionary (`_includes/` plus theme includes) and are referenced with Nunjucks `{% include "name" %}` syntax, the legacy `{{ include name }}` placeholder, or `{{> name }}` on the Hbs engine.
