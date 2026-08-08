+++
title = "Tags and Taxonomies"
description = "How Zest auto-generates /tags/ and /tags/<term>/ archive pages from page front matter, and how content files override generated URLs."
category = "features"
tags = ["zest", "tags", "taxonomies", "archives"]
date = 2026-08-01
+++

# Tags and Taxonomies

This page covers Zest's taxonomy system: declaring tags on pages, the automatic generation of `/tags/` and `/tags/<term>/` archive pages, and the templates that render them. It builds on the collection concepts from [collections](/en/posts/collections/) and the front-matter conventions described in the [template reference](/en/posts/template-reference/).

## Declaring Tags

A page joins a tag group by declaring it in front matter. Every front-matter format works:

```toml
+++
title = "Writing F# Pages"
tags = ["fsharp", "dsl"]
date = 2026-08-01
+++
```

```html
<!-- @tags fsharp, dsl -->
```

## Automatic Archive Pages

After the content pipeline finishes, `TaxonomyGenerator` scans every page's tags and generates two kinds of output for the default `tag` taxonomy:

- `/tags/` — the terms index, listing every tag with a link to its archive.
- `/tags/<term>/` — one listing per term, newest first, containing all tagged pages.

Generated pages are rewritten on every build, so removing a tag removes its archive, and template or config changes never leave stale pages behind. Adding a tag to a post is enough — no hand-written archive file is required.

## Templates for Tag Pages

The generator renders through your theme's layouts, looked up by key in this order:

| Page type | Layout lookup keys | Built-in fallback |
|---|---|---|
| Term listing | `_layouts/tag.njk` (the taxonomy's `name`), then `_layouts/taxonomy.njk` | `{{ term }}` heading plus the tagged post list |
| Terms index | `_layouts/tags.njk` (the taxonomy's `plural`), then `_layouts/terms.njk` | Tag cloud with per-term links |

The fallbacks are self-contained, so generation works even without a theme template. A custom template receives the standard context plus:

- `term` — the current term name.
- `term_pages` — the tagged pages, newest first.
- `taxonomy` — a dictionary with `name` and `plural`.

```njk
<!-- _layouts/tag.njk -->
<h1>Posts tagged {{ term }}</h1>
{% for p in term_pages %}
  <p><a href="{{ p.url }}">{{ p.title }}</a></p>
{% endfor %}
```

## Content Files Win

A hand-authored content file always takes precedence: if a real page already produces `/tags/foo/`, the generator skips that URL and keeps the content version. This lets you customize a specific archive without disabling generation for the rest.

## Configuration

Taxonomies come from the `[[taxonomies]]` table, each entry being a `name`/`plural` pair. The defaults are `tag`/`tags` and `category`/`categories`. Term extraction is currently implemented for the `tag` taxonomy only, so a custom taxonomy can be declared but is not yet auto-generated.
