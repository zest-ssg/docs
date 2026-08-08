+++
title = "Mustache"
description = "Logic-less Mustache templates in Zest, rendered by the standalone Hbs engine."
category = "templates"
tags = ["zest", "mustache", "templates"]
date = 2026-08-01
+++

# Mustache

Mustache is the logic-less template language, and Zest renders `.mustache` files with the same standalone Hbs engine used for [Handlebars](/en/posts/handlebars/). This page is part of the **Templates** section of the documentation.

## How It Works

A `.mustache` file is routed to the `hbs` engine by file extension. The engine implements native Mustache semantics: variables, sections, inverted sections, and partials — with no programming constructs such as `if` or loops in the template itself. All logic lives in the data that Zest injects into the context.

## Variables

```mustache
<h1>{{ page.title }}</h1>
<p>{{ page.description }}</p>
```

`{{ }}` output is HTML-escaped by default. Use the triple-stash `{{{ }}}` or the ampersand form `{{& }}` for unescaped output, for example to splice the rendered page body:

```mustache
{{{ content }}}
```

## Sections

A section renders its block once for each item in a list, or once if the value is truthy. With `page.tags` set to `["fsharp", "static-site"]`:

```mustache
{{#page.tags}}
  <span>{{.}}</span>
{{/page.tags}}
```

`{{.}}` refers to the current context item. An empty or missing value renders nothing — no `else` branch is needed, which is exactly the point of a logic-less template.

## Inverted Sections

The caret form `{{^section}}…{{/section}}` renders only when the section value is empty, missing, or false:

```mustache
{{^page.tags}}
  <p>This page has no tags.</p>
{{/page.tags}}
```

## Partials

Partials load by name from `_includes/`:

```mustache
{{> header}}
<main>{{{ content }}}</main>
{{> footer}}
```

## Context Data

Mustache resolves dotted paths against the injected context, so the same data available to every Zest template is available here: `{{ site.title }}`, `{{ site.description }}`, `{{ page.url }}`, `{{ page.date }}`, `{{ page.tags }}`, `{{ pages }}`, and `{{ collections }}`. Because the site context is nested (for example `site.base_url`), dotted lookups traverse it directly.

## Differences from Handlebars and Nunjucks

- Mustache has no helpers: `{{#if}}` and `{{#each}}` do not exist. Handlebars syntax works in `.mustache` files only if the engine's Handlebars-mode tokens happen to be used; prefer pure Mustache for portable templates.
- There are no filters. Precompute values in the F# DSL, via `_init.zest.fsx` globals, or with the page-query API in `DslCollections`.
- Nunjucks tags (`{% %}`) are not evaluated in Mustache files — everything goes through the Hbs engine.

## Frontmatter

`.mustache` files use HTML-comment frontmatter:

```html
<!-- @title Home -->
<!-- @layout default -->
{{> header}}
<main>{{{ content }}}</main>
{{> footer}}
```

Mustache is most useful when porting templates from logic-less tooling. For new projects, the native [Zest DSL](/en/posts/zest-dsl/) or [Nunjucks](/en/posts/nunjucks/) are the primary template languages.
