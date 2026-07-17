# Templates & Layouts

Zest supports multiple template engines. The default engine uses simple `{{ placeholder }}` syntax, and a full Nunjucks-compatible engine is available.

## Template Engines

### Native Mode (Default)

Set `template_engine = "native"` in `_config.toml` (default). This engine supports:

- `{{ page.title }}` — page metadata placeholders
- `{{ site.title }}` — site-level placeholders
- `{{ content }}` — page body content
- `{{ include name }}` — include partials

### Nunjucks Mode

Set `template_engine = "nunjucks"` for a full Nunjucks-compatible engine with tags, blocks, inheritance, filters, and macros. Also available for individual layout files by using `.njk` extension.

---

## Layouts

Layouts are templates in `_layouts/` that wrap page content. The layout to use is specified in the page:

```fsharp
page {
    layout "default"
    // ...
}
```

Or in frontmatter:

```markdown
+++
layout = "default"
+++
```

### Layout File Naming

Layout files are identified by name (without extension):

```
_layouts/
├── default.njk       → layout "default"
├── post.njk          → layout "post"
├── docs.html         → layout "docs"
└── special.haml      → layout "special"
```

Supported layout extensions: `.html`, `.htm`, `.njk`, `.liquid`, `.hbs`, `.mustache`, `.zest.fsx`, `.fsx`.

### Layout Example (Native Syntax)

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

### Layout Example (Nunjucks)

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

### Nested Layouts

Layouts can chain via TOML frontmatter or HTML comments:

```html
<!-- _layouts/post.html -->
---
layout = "default"
---
<article>
    {{ content }}
</article>
```

Or:

```html
<!-- @layout default -->
<article>
    {{ content }}
</article>
```

---

## Includes

Partials are placed in `_includes/` and referenced by filename (with or without extension):

```
_includes/
├── header.html
├── footer.html
├── nav.html
└── analytics.njk
```

### Native Syntax

```
{{ include header }}
{{ include footer }}
```

Includes support recursive resolution up to 10 levels deep.

### Nunjucks Syntax

```njk
{% include "header" %}
{% include "footer" %}
```

---

## Placeholder Reference

### `page.*` Placeholders

| Placeholder | Source | Description |
|---|---|---|
| `{{ page.title }}` | `page { title "..." }` or frontmatter | Page title |
| `{{ page.url }}` | Computed by permalink router | Page URL, e.g. `/posts/hello/` |
| `{{ page.slug }}` | Derived from filename | URL slug |
| `{{ page.date }}` | `page { date ... }` or frontmatter | Date as `yyyy-MM-dd` |
| `{{ page.tags }}` | `page { tags [...] }` or frontmatter | Tags as comma-separated string |
| `{{ page.description }}` | `page { description "..." }` or frontmatter | Meta description |
| `{{ page.content }}` | Auto-generated | Rendered page content (alias for `{{ content }}`) |
| `{{ content }}` | Auto-generated | Rendered page content |

Custom data set via `page { data "key" value }` is available as `{{ page.key }}`.

### `site.*` Placeholders

| Placeholder | Source |
|---|---|
| `{{ site.title }}` | `_config.toml` → `title` |
| `{{ site.description }}` | `_config.toml` → `description` |
| `{{ site.base_url }}` | `_config.toml` → `base_url` |
| `{{ site.version }}` | `_config.toml` → `site_version` |
| `{{ site.author }}` | `_config.toml` → `author` |
| `{{ site.language }}` | `_config.toml` → `language` |
| `{{ site.{namespace}.{key} }}` | `_data/{namespace}.toml` files |
| `{{ menu.{name} }}` | `_config.toml` → `[menu.{name}]` |

---

## Nunjucks Engine Reference

The built-in Nunjucks engine (`NunjucksEngine`) implements a compatible subset of Nunjucks syntax.

### Tags

| Tag | Syntax | Description |
|---|---|---|
| `if` | `{% if cond %}...{% elif cond %}...{% else %}...{% endif %}` | Conditional |
| `for` | `{% for item in list %}...{% endfor %}` | Loop (with `loop.index`, `loop.first`, `loop.last`) |
| `block` | `{% block name %}...{% endblock %}` | Named block (for template inheritance) |
| `extends` | `{% extends "layout.html" %}` | Template inheritance |
| `include` | `{% include "partial.html" %}` | Include partial |
| `set` | `{% set name = value %}` | Set variable |
| `macro` | `{% macro name(args) %}...{% endmacro %}` | Define macro |
| `call` | `{% call macro(args) %}...{% endcall %}` | Call macro with content |
| `import` | `{% import "macros.html" as name %}` | Import macros |
| `from` | `{% from "macros.html" import name %}` | Import specific macro |
| `raw` | `{% raw %}...{% endraw %}` | Literal content (no parsing) |
| `filter` | `{% filter name %}...{% endfilter %}` | Apply filter to block |

### Built-in Filters

| Filter | Description |
|---|---|
| `capitalize` | Capitalize first character |
| `lower` / `upper` | Case conversion |
| `title` | Title case |
| `trim` | Strip whitespace |
| `safe` | Mark as safe (no escaping) |
| `escape` / `e` | HTML-escape |
| `striptags` | Remove HTML tags |
| `truncate(n)` | Truncate to N characters |
| `wordcount` | Count words |
| `replace(a, b)` | String replace |
| `slugify` | URL-safe slug |
| `urlencode` | URL-encode |
| `format` | String format |
| `indent(n)` | Indent lines |
| `center(n)` | Center text |
| `int` / `float` | Type conversion |
| `abs` | Absolute value |
| `length` | List length |
| `reverse` | Reverse list |
| `first` / `last` | First/last item |
| `join(sep)` | Join list with separator |
| `sort` | Sort list |
| `slice(start, end)` | Slice list |
| `batch(n)` | Batch list into chunks of N |
| `groupby(attr)` | Group by attribute |
| `selectattr(attr, test)` | Filter list by attribute |
| `rejectattr(attr, test)` | Reject by attribute |
| `items` | Dict to (key, value) pairs |
| `dictsort` | Sort dict by key |
| `default(d)` / `d` | Default value |
| `date(format)` | Date formatting |
| `urlize` | Convert URLs to links |

### Custom Filters (Registered by Zest)

| Filter | Description |
|---|---|
| `pages_by_tag(tag)` | Filter all pages by tag |
| `recent(n)` | Get N most recent pages |
| `by_collection(name)` | Filter by collection name |
| `search(query)` | Full-text search across pages |
| `where(attr, value)` | Generic attribute filter |

### Context Variables in Nunjucks

When using Nunjucks layouts, the following context variables are available:

| Variable | Type | Description |
|---|---|---|
| `content` | string | Page body HTML |
| `page.content` | string | Same as content |
| `page.url` | string | Page URL |
| `page.date` | string | Page date |
| `page.tags` | string[] | Page tags as array |
| `pages` | object[] | All site pages (for iteration) |
| `tags` | object | All site tags (for iteration) |
| `collections` | object | All collections (for iteration) |
| `site.*` | varies | Site config and global data |
| Include names | string | Each include as its own variable |

---

## Template Compatibility

Zest can convert several template formats to Nunjucks automatically:

| Format | Extension | Conversion |
|---|---|---|
| Liquid | `.liquid` | `{{ }}` → same, `{% %}` → same, basic tag mapping |
| Handlebars | `.hbs` | `{{#each}}`, `{{#if}}`, `{{> partial}}` → Nunjucks |
| Mustache | `.mustache` | `{{#section}}`, `{{^inverted}}`, `{{> partial}}` → Nunjucks |
| HAML | `.haml` | Experimental: indentation-based → HTML |
| Pug/Jade | `.pug` | Experimental: indentation-based → HTML |
| WebC | `.webc` | Treated as Nunjucks directly |

Conversion is applied before the Nunjucks engine renders the template. The mapping is defined in `TemplateCompat` and handled by `HamlConverter`, `PugConverter`, and `HandlebarsMustacheConverter`.

---

## Markdown-Only and HTML-Only Strategies

| Extension | Strategy |
|---|---|
| `.md`, `.markdown` | `MarkdownOnly` — rendered through the Markdown engine |
| `.html`, `.htm` | `HtmlOnly` — passed through directly (Nunjucks-preprocessed if `{{ }}` detected) |
| `.njk`, `.liquid`, `.hbs`, `.mustache`, `.webc` | `Nunjucks` — rendered through Nunjucks engine |
| `.haml`, `.pug` | `ConvertThenNunjucks` — converted then rendered through Nunjucks |

---

## `.html` File Preprocessing (11ty-Style)

Like Eleventy preprocesses `.html` through Liquid, Zest preprocesses `.html` files through Nunjucks when they contain `{{ }}` or `{% %}` syntax. This allows `.html` files in the content directory to use template variables and includes directly.
