+++
title = "Liquid"
description = "Use Shopify Liquid templates in Zest: converted to Nunjucks and rendered by the built-in engine."
category = "templates"
tags = ["zest", "liquid", "templates"]
date = 2026-08-01
+++

# Liquid

Liquid is supported in Zest as part of the template compatibility layer. A `.liquid` file is converted to Nunjucks syntax and then rendered by the built-in Nunjucks engine — you get Liquid's familiar syntax without a second runtime. This page sits in the **Templates** section of the documentation, alongside [Handlebars](/en/posts/handlebars/), [Mustache](/en/posts/mustache/), [HAML](/en/posts/haml/), and [Pug](/en/posts/pug/).

## How It Works

Content files and layouts with the `.liquid` extension are routed to the Nunjucks engine. Before rendering, the `LiquidConverter` rewrites Liquid constructs to their Nunjucks equivalents:

- `{{ }}` output expressions are kept as-is.
- `{% %}` tags are mapped to Nunjucks tags (`assign`, `capture`, `unless`, `case`, `if`, `for`).
- Liquid filter names are aliased to their Nunjucks counterparts (for example `downcase` → `lower`, `upcase` → `upper`), and filter argument syntax `name: a, b` is rewritten as `name(a, b)`.
- `for` modifiers such as `limit`, `offset`, and `reversed` become `| take(offset, limit) | reverse`.
- `nil` is normalized to `null`.

Because `.liquid` layouts are rendered through the same pipeline as `.njk` layouts, the full site context is available: `site.*`, `page.*`, `pages`, `tags`, and `collections`.

## Variables

```liquid
{{ page.title }}
{{ site.title }}
{{ page.tags | join: ", " }}
```

## Assign and Logic

```liquid
{% assign count = site_pages | size %}
{% if count > 3 %}
  <p>More than three pages.</p>
{% else %}
  <p>Three or fewer pages.</p>
{% endif %}
```

## Loops

```liquid
<ul>
{% for post in site_pages limit: 5 %}
  <li><a href="{{ post.url }}">{{ post.title }}</a></li>
{% endfor %}
</ul>
```

Inside loops, `forloop.index`, `forloop.first`, and `forloop.last` map to the equivalent loop metadata provided by the rendering engine.

## Filters

Most standard Liquid filters work through the alias/rewrite layer, and the full set of Nunjucks built-in filters is available as well: `capitalize`, `lower`, `upper`, `title`, `trim`, `safe`, `escape`, `striptags`, `truncate`, `replace`, `slugify`, `join`, `sort`, `default`, and others. Zest's custom page-query filters (`pages_by_tag`, `recent`, `by_collection`, `search`, `where`) are also registered, so collection logic written in Liquid can query pages directly.

## Differences from Nunjucks

Liquid is deliberately a more restricted language than Nunjucks. When writing `.liquid` templates, keep in mind:

- There is no `extends`/`block` inheritance in Liquid; use Zest layouts and the `{{ content }}` placeholder instead.
- Macros are not available — reuse markup with includes.
- Whitespace-control delimiters (`{%- -%}`) are handled by the converter, but complex nesting is best kept simple.
- Any Liquid tag the converter does not recognize is passed through and may surface as a Nunjucks template error — prefer the constructs listed above.

## Frontmatter

Liquid files support HTML-comment frontmatter, the same as other template extensions:

```html
<!-- @title Products -->
<!-- @layout default -->
<h1>{{ page.title }}</h1>
```

## When to Use It

Liquid is a good choice when porting content from Jekyll, 11ty, or Shopify-based tooling. For new projects, the native [Zest DSL](/en/posts/zest-dsl/) or [Nunjucks](/en/posts/nunjucks/) are the primary template languages, and [Markdown](/en/posts/markdown/) covers most prose content.
