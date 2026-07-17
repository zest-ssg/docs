# Templates & Layouts

Zest supports two layout-processing paths selected by `template_engine` in `_config.toml` (see [configuration.md](configuration.md)):

- **`native`** (default): simple `{{ placeholder }}` substitution only.
- **`nunjucks`** (or the `.njk` extension on a layout file): full Nunjucks-compatible engine — tags, blocks, inheritance, filters, macros.

Partials and Markdown/HTML pages are always routed through the active engine. 11ty-style formats (Liquid, Handlebars, Mustache, HAML, Pug, WebC) are auto-converted into Nunjucks input before rendering.

---

## Layouts

Layouts live in `_layouts/` and wrap a page's rendered body. The layout is selected via `page { layout "name" }` or frontmatter `layout = "name"` (the **name** is the file stem, extension omitted).

```
_layouts/
├── default.njk       → layout "default"
├── post.njk          → layout "post"
├── docs.html         → layout "docs"
└── special.haml      → layout "special"
```

Supported layout extensions: `.html`, `.htm`, `.njk`, `.liquid`, `.hbs`, `.mustache`, `.haml`, `.pug`, `.webc`, `.zest.fsx`, `.fsx`.

### Native-layout example

```html
<!-- _layouts/default.html -->
<!DOCTYPE html>
<html lang="{{ site.language }}">
<head>
    <meta charset="utf-8" />
    <title>{{ page.title }} — {{ site.title }}</title>
    <meta name="description" content="{{ page.description }}" />
    <link rel="stylesheet" href="/assets/css/main.css" />
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

### Nunjucks-layout example

```njk
<!-- _layouts/default.njk -->
<!DOCTYPE html>
<html lang="{{ site.language }}">
<head>
    <meta charset="utf-8" />
    <title>{{ page.title }} — {{ site.title }}</title>
    <meta name="description" content="{{ page.description }}" />
</head>
<body>
    {% include "header" %}
    <main>
        {{ content | safe }}
    </main>
    {% include "footer" %}
</body>
</html>
```

### Nested layouts

A layout may itself declare a parent via TOML frontmatter or an HTML-comment header:

```html
<!-- _layouts/post.html -->
---
layout = "default"
---
<article>
    {{ content }}
</article>
```

```html
<!-- _layouts/post.html -->
<!-- @layout default -->
<article>
    {{ content }}
</article>
```

---

## Includes

Partials live in `_includes/` and are referenced by file stem (extension optional). Include resolution is recursive up to **10 levels** deep.

```
_includes/
├── header.html
├── footer.html
├── nav.html
└── analytics.njk
```

| Syntax | Form |
|---|---|
| Native | `{{ include header }}` |
| Nunjucks | `{% include "header" %}` |

---

## Placeholder Reference

### `page.*`

| Placeholder | Source | Description |
|---|---|---|
| `{{ page.title }}` | `page { title "..." }` or frontmatter | Page title |
| `{{ page.url }}` | Permalink router | Page URL, e.g. `/posts/hello/` |
| `{{ page.slug }}` | Derived from filename | URL slug |
| `{{ page.date }}` | `page { date ... }` or frontmatter | Date (`yyyy-MM-dd`) |
| `{{ page.tags }}` | `page { tags [...] }` or frontmatter | Tags (array) |
| `{{ page.description }}` | `page { description "..." }` or frontmatter | Meta description |
| `{{ page.content }}` / `{{ content }}` | Auto-generated | Rendered page body |

Custom data set via `page { data "key" value }` is available as `{{ page.key }}`.

### `site.*`

| Placeholder | Source |
|---|---|
| `{{ site.title }}` | `_config.toml` → `title` |
| `{{ site.description }}` | `_config.toml` → `description` |
| `{{ site.base_url }}` | `_config.toml` → `base_url` |
| `{{ site.version }}` | `_config.toml` → `site_version` |
| `{{ site.author }}` | `_config.toml` → `author` |
| `{{ site.language }}` | `_config.toml` → `language` |
| `{{ site.{namespace}.{key} }}` | `_data/{namespace}.toml` |
| `{{ menu.{name} }}` | `_config.toml` → `[menu.{name}]` |

---

## Nunjucks Engine Reference

The built-in engine (`NunjucksEngine`) implements a compatible subset of Nunjucks syntax. Its behavior is governed by `nunjucks_compatibility` in `_config.toml`:

- **`zest`** (default): official Nunjucks tags/filters **plus** Zest's custom page-query filters (`pages_by_tag`, `recent`, `by_collection`, `search`, `where`).
- **`strict`**: official Nunjucks only — Zest custom filters are skipped.

### Tags

| Tag | Syntax | Description |
|---|---|---|
| `if` | `{% if c %}…{% elif c %}…{% else %}…{% endif %}` | Conditional |
| `for` | `{% for item in list %}…{% endfor %}` | Loop (`loop.index`, `loop.first`, `loop.last`) |
| `block` | `{% block name %}…{% endblock %}` | Named block (inheritance) |
| `extends` | `{% extends "layout.html" %}` | Template inheritance |
| `include` | `{% include "partial.html" %}` | Include partial |
| `set` | `{% set name = value %}` | Set variable |
| `macro` | `{% macro name(args) %}…{% endmacro %}` | Define macro |
| `call` | `{% call macro(args) %}…{% endcall %}` | Call macro with body |
| `import` | `{% import "macros.html" as name %}` | Import macros |
| `from` | `{% from "macros.html" import name %}` | Import one macro |
| `raw` | `{% raw %}…{% endraw %}` | Literal, unparsed content |
| `filter` | `{% filter name %}…{% endfilter %}` | Apply filter to block |

### Built-in Filters

`capitalize`, `lower`, `upper`, `title`, `trim`, `safe`, `escape`/`e`, `striptags`, `truncate(n)`, `wordcount`, `replace(a, b)`, `slugify`, `urlencode`, `format`, `indent(n)`, `center(n)`, `int`, `float`, `abs`, `round`, `length`, `reverse`, `first`, `last`, `join(sep)`, `sort`, `slice(start, end)`, `batch(n)`, `groupby(attr)`, `selectattr(attr, test)`, `rejectattr(attr, test)`, `items`, `dictsort`, `default(d)`/`d`, `urlize`.

### Custom Filters (Zest, `zest` mode only)

| Filter | Description |
|---|---|
| `pages_by_tag(tag)` | All pages carrying `tag` |
| `recent(n)` | `n` most recent pages (by date desc) |
| `by_collection(name)` | Pages in collection `name` (first URL segment) |
| `search(query)` | Case-insensitive title search |
| `where(attr, value)` | Generic attribute filter |

### Context Variables

When rendering a layout, the engine injects:

| Variable | Type | Description |
|---|---|---|
| `content` / `page.content` | string | Page body HTML |
| `page.url` / `page.date` / `page.tags` / `page.title` / `page.description` | varies | Page metadata |
| `pages` | object[] | All site pages |
| `tags` | object | All site tags → pages |
| `collections` | object | All collections → pages |
| `site.*` | varies | Site config + global data |
| include names | string | Each include as its own variable |

---

## Template Compatibility Layer

Zest converts several formats to Nunjucks input automatically (see `TemplateCompat`, `HamlConverter`, `PugConverter`, `HandlebarsMustacheConverter`):

| Format | Extension | Conversion |
|---|---|---|
| Liquid | `.liquid` | `{{ }}` keeps; `{% %}` maps to Nunjucks tags |
| Handlebars | `.hbs` | `{{#each}}`, `{{#if}}`, `{{> partial}}` → Nunjucks |
| Mustache | `.mustache` | `{{#section}}`, `{{^inverted}}`, `{{> partial}}` → Nunjucks |
| HAML | `.haml` | Indentation-based → HTML, then Nunjucks |
| Pug | `.pug` | Indentation-based → HTML, then Nunjucks |
| WebC | `.webc` | Treated as Nunjucks directly |

> HAML/Pug conversion supports implicit `div` (`.class`, `#id`), inline classes/ids, inline `= expr` → `{{ expr }}`, and real tag-name closing for nested structure.

---

## Per-Extension Strategy

| Extension | Strategy |
|---|---|
| `.md`, `.markdown` | Rendered through the Markdown engine (frontmatter stripped) |
| `.html`, `.htm` | Passed through directly; Nunjucks-preprocessed only if `{{ }}`/`{% %}` detected |
| `.njk`, `.liquid`, `.hbs`, `.mustache`, `.webc` | Rendered through the Nunjucks engine |
| `.haml`, `.pug` | Converted, then rendered through the Nunjucks engine |

### `.html` Preprocessing (11ty-style)

Like Eleventy preprocesses `.html` through Liquid, Zest preprocesses `.html` files through the **Nunjucks** engine when they contain `{{ }}` or `{% %}` syntax. This lets content-directory `.html` files use template variables and includes directly.
