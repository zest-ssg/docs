+++
title = "Collections"
description = "How Zest groups pages into collections, queries them from templates and the F# DSL, and renders paginated listings."
category = "features"
tags = ["zest", "collections", "pagination", "filters"]
date = 2026-08-01
+++

# Collections

This page explains Zest's collection system: how pages are grouped, how templates and DSL scripts query them, and how an index template becomes a paginated listing. It is part of the Features section — see [tags](/en/posts/tags/) for auto-generated tag archives and [search](/en/posts/search/) for querying content. The full function catalog lives in the [DslCollections reference](/en/posts/dsl-collections/).

## What a Collection Is

A collection is simply a group of pages. Two groupings exist implicitly:

- **By directory** — every page under `content/posts/` belongs to the `posts` collection.
- **By tag** — every page whose front matter declares `tags` joins the corresponding tag groups.

Templates receive three ready-made arrays: `pages` (all pages), `tags` (all tag names) and `collections` (all collection names). On generated listing pages the `collection` context key holds the current collection's name.

## Tagging Pages

Add tags to any page in front matter — the format depends on the file type:

```toml
+++
title = "Hello World"
tags = ["announcements", "zest"]
date = 2026-08-01
+++
```

```html
<!-- @title Hello World -->
<!-- @tags announcements, zest -->
```

## Querying Collections

Nunjucks templates use the built-in filters:

```njk
{% set posts = pages | by_collection('posts', 'true') %}
{% for p in posts | recent(5) %}
  <a href="{{ p.url }}">{{ p.title }}</a>
{% endfor %}
```

- `by_collection` — pages whose URL starts with the collection directory; a second `true` argument excludes the index page.
- `pages_by_tag` / `by_tag` — pages carrying a given tag (case-insensitive).
- `recent` — the N newest pages by date.
- `where` — generic Liquid-style attribute filter.

In F# DSL pages the same queries go through `DslCollections` (`site_pages`, `pages_by_collection`, `pages_by_tag`, `all_tags` and friends) — see the [collection DSL page](/en/posts/dsl-collections/).

## Paginated Listings

To turn a listing template into multiple pages, add the `@paginate` directive to its front matter:

```html
<!-- @paginate posts, 5 -->
```

The first argument is the collection name (defaults to the file's directory) and the second is the page size (defaults to `[pagination] per_page`). The generator then takes over the URL: `/posts/` holds the first window, `/posts/page/2/` and later windows hold the rest. The content pipeline skips the file, so no output conflict occurs.

The template reads the current window from the `pagination` object:

```njk
{% for p in pagination.items %}
  <article><a href="{{ p.url }}">{{ p.title }}</a></article>
{% endfor %}
{% if pagination.prevUrl %}<a href="{{ pagination.prevUrl }}">Previous</a>{% endif %}
{% if pagination.nextUrl %}<a href="{{ pagination.nextUrl }}">Next</a>{% endif %}
```

| Key | Meaning |
|---|---|
| `pagination.currentPage` | 1-based index of the current window |
| `pagination.totalPages` | Total number of windows |
| `pagination.totalItems` | Total items in the collection |
| `pagination.perPage` | Items per window |
| `pagination.items` | Page dictionaries for this window |
| `pagination.prevUrl` | URL of the previous window (empty on the first) |
| `pagination.nextUrl` | URL of the next window (empty on the last) |

Items are ordered newest first by date. The per-page directory is cleared before regeneration, so incremental builds never leave orphaned `/posts/page/N/` outputs behind.
