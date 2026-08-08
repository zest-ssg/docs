+++
title = "Pug"
description = "Use Pug templates in Zest: indentation-based markup converted to HTML and rendered through Nunjucks."
category = "templates"
tags = ["zest", "pug", "templates"]
date = 2026-08-01
+++

# Pug

Pug is supported through Zest's template compatibility layer: a `.pug` file is converted to HTML by the `PugConverter` and then rendered by the Nunjucks engine. This page is part of the **Templates** section of the documentation, alongside [HAML](/en/posts/haml/), the other indentation-based format.

## How It Works

The converter parses Pug's indentation-based syntax and emits HTML, then hands the result to the Nunjucks engine. Content files and layouts with the `.pug` extension both go through this conversion, so `{{ }}` expressions and `{% %}` tags inside the Pug markup are evaluated after conversion, and the full site context (`site.*`, `page.*`, `pages`, `tags`, `collections`) is available.

## Tags and Nesting

```pug
h1 A heading
p.lead A paragraph with class "lead"
#main-content
  span.badge New
```

- `tag` — an explicit element.
- `.class` and `#id` — shorthand that implies a `div`.
- Indentation expresses nesting: children are indented under their parent.

## Attributes

Parenthesized attribute lists:

```pug
a(href="/about/", title="About") About us
img(src="/img/logo.png", alt="Logo")
```

Attribute values are HTML-escaped for safety.

## Text

- Plain lines are emitted as text content.
- `| text` — an explicit literal-text line.
- `= expr` — evaluates an expression and outputs it, converted to `{{ expr }}` for the Nunjucks engine:

```pug
p= page.title
```

## Includes and Doctype

- `include path` — converted to `{% include "path" %}` so partials resolve from `_includes/`.
- `doctype` — converted to `<!DOCTYPE html>`.

## Mixins

Pug mixins are not executed by the converter. Define reusable markup with Zest includes instead, or fall back to Nunjucks macros:

```njk
{% macro card(title, body) %}
  <div class="card">
    <h3>{{ title }}</h3>
    <p>{{ body }}</p>
  </div>
{% endmacro %}
```

## Example

A complete `.pug` content page:

```pug
<!-- @title Pug Page -->
<!-- @layout default -->
article
  h1= page.title
  p= page.description
  ul
    li First
    li Second
```

Which converts to:

```html
<article>
  <h1>{{ page.title }}</h1>
  <p>{{ page.description }}</p>
  <ul>
    <li>First</li>
    <li>Second</li>
  </ul>
</article>
```

## Notes and Limits

- HTML5 void elements are emitted without a trailing slash.
- Tabs and arbitrary-width indentation are supported.
- The converter covers the structural subset of Pug; complex inline JavaScript and mixin definitions are not executed — only the documented forms are translated.
- Because the converted output is rendered by Nunjucks, remember that Pug's `=` expression syntax becomes a `{{ }}` placeholder: `= page.title` renders `{{ page.title }}`.

Pug is a good choice when porting existing templates. For new projects, the native [Zest DSL](/en/posts/zest-dsl/) or [Nunjucks](/en/posts/nunjucks/) are the primary template languages.
