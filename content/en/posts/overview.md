+++
title = "Features Overview"
description = "A survey of the Zest feature set — template engines, the F# DSL, ZCSS styling, collections, search and developer tooling."
category = "features"
tags = ["zest", "features", "overview"]
date = 2026-08-01
+++

# Features Overview

This page is the starting point for the Features section of the documentation. It summarizes what Zest can do and points to the dedicated page for each capability: [collections](/en/posts/collections/), [tags](/en/posts/tags/), [search](/en/posts/search/) and [PJAX navigation](/en/posts/pjax/). The rest of the documentation — the [guides](/en/posts/getting-started/), the [templates](/en/posts/markdown/) section, the [Zest DSL](/en/posts/dsl-guide/) and [ZCSS](/en/posts/zcss/) — provides the detail behind each item listed here.

## Template Engines

Every content file and layout is routed to an engine by its file extension, so a single site can mix formats freely:

- **`.zest.fsx`** — native F# pages using the `page { }` DSL (see the [DSL guide](/en/posts/dsl-guide/)).
- **`.njk`** — Nunjucks templates, the default engine for HTML-style templates.
- **`.liquid`, `.haml`, `.pug`** — converted to Nunjucks syntax and rendered by the same engine.
- **`.hbs` / `.mustache`** — a standalone Handlebars/Mustache engine with native semantics.
- **`.md` / `.markdown`** — Markdown with TOML front matter (see [Markdown](/en/posts/markdown/)).
- **`.webc`** — recognized but not yet implemented (see [WebC](/en/posts/webc/)).

The [template reference](/en/posts/template-reference/) lists the complete routing table.

## The F# DSL

The signature feature: `.zest.fsx` pages are real F# programs evaluated by `dotnet fsi`. The `page { }` computation expression builds HTML with typed element builders, and supporting modules add collection queries, SEO helpers and XML feed generation. Layouts written in F# receive `content`, `page` and `site` bindings directly.

## ZCSS Styling

[ZCSS](/en/posts/zcss/) is a SCSS-like preprocessor bundled with Zest: variables, nesting, mixins, color functions, `@use` modules and responsive breakpoint shorthands, compiled through a single `.zcss` pipeline with content-hash caching.

## Collections, Tags and Pagination

Pages are grouped into collections by directory and by front-matter tags. Templates query them with filters such as `by_collection` and `pages_by_tag`, while DSL scripts use the `DslCollections` module. [Tag archives](/en/posts/tags/) are generated automatically, and [pagination](/en/posts/collections/) turns an index template into `/posts/`, `/posts/page/2/` and so on.

## Static Search

Zest has no server-side search; the index is built at build time and filtering happens entirely in the browser. The header live search and the standalone [search page](/en/posts/search/) both consume a JSON index injected into the page.

## PJAX Navigation

The [PJAX script](/en/posts/pjax/) intercepts same-origin link clicks, fetches the target page and swaps only the content region, with caching, hover prefetch and full history support.

## Feeds and Internationalization

`DslXml` generates RSS 2.0 and Atom 1.0 feeds from the page collection. Locale files under `_locales/` feed the `t` translation filter, so templates can render in multiple languages from one build.

## Development Workflow

`zest serve` starts a development server with live reload, `zest preview` serves a previously built site, and `zest build --watch` rebuilds on change. Parallel and incremental builds keep iteration fast. See the [CLI guide](/en/posts/cli/) for the full command set.

## Zest Compared to Other SSGs

| Capability | Zest | Jekyll | Hugo | Eleventy | Hexo |
|---|---|---|---|---|---|
| Primary templating | F# DSL + Nunjucks | Liquid | Go templates | Nunjucks / Liquid / JS | EJS |
| Content formats | F# scripts, Markdown, Nunjucks, Liquid, Handlebars, HAML, Pug | Markdown | Markdown | Markdown + anything | Markdown |
| Config format | TOML | YAML | TOML / YAML | JS / JSON / TOML / YAML | YAML |
| Styling pipeline | ZCSS (built-in) | Sass (external) | SCSS (external) | Any | Stylus (external) |
| Search | Build-time index | Plugin | External | Plugin | Plugin |
| Runtime | .NET (F# / C#) | Ruby | Go | Node.js | Node.js |

The comparison is deliberately narrow: Zest's differentiators are the F# DSL, the unified extension-based template pipeline and the zero-configuration build. Zest takes its main inspiration from **Eleventy**: conventions over configuration, swappable template languages and fully static output. On top of that, Zest lets templates themselves become type-safe code via F#, and ships optional `[compat]` switches for Jekyll / Hexo / Hugo / Eleventy to ease migration.
