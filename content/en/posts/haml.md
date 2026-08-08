+++
title = "HAML"
description = "Write HAML templates in Zest: indentation-based markup converted to HTML and rendered through Nunjucks."
category = "templates"
tags = ["zest", "haml", "templates"]
date = 2026-08-01
+++

# HAML

HAML is supported through Zest's template compatibility layer: a `.haml` file is converted to HTML by the `HamlConverter` and then rendered by the Nunjucks engine, so template variables and includes keep working inside the converted markup. This page is part of the **Templates** section of the documentation, alongside [Pug](/en/posts/pug/), the other indentation-based format.

## How It Works

The converter parses HAML's indentation-based syntax and emits plain HTML, then hands the result to the Nunjucks engine. The same conversion applies to `.haml` content files and `.haml` layouts. Because the converted markup is rendered by Nunjucks, `{{ }}` expressions and `{% %}` tags inside the HAML are evaluated after conversion.

## Tag, Class and ID Syntax

```haml
%h1 A heading
%p.lead A paragraph with class "lead"
#main-content
  %span.badge New
```

- `%tag` — an explicit element.
- `.class` and `#id` — shorthand that implies a `div`.
- `.class` and `#id` can be combined and attached to any tag: `%a.btn#cta`.

## Attributes

Inline attributes use Ruby-style hashes:

```haml
%a{ href: "/about/", title: "About" } About us
%img{ src: "/img/logo.png", alt: "Logo" }
```

Attribute values are HTML-escaped (XSS-safe), as is text content.

## Expressions and Code Lines

- `= expr` — evaluates an expression and outputs the result, converted to `{{ expr }}` for the Nunjucks engine:

```haml
%p= page.title
```

- `- code` — a line of code; these lines are stripped during conversion (used for flow control that Nunjucks tags replace).
- `/ comment` — an HTML comment, converted to `<!-- -->`.

## Text

Plain text lines are emitted as text content:

```haml
%p
  This is paragraph text.
```

## Filters

The converter understands the classic HAML filters:

- `:css` — wrapped in `<style>`.
- `:javascript` — wrapped in `<script>`.
- `:markdown` — passed through (handled downstream).

## Example

A complete `.haml` content page:

```haml
<!-- @title HAML Page -->
<!-- @layout default -->
%article
  %h1= page.title
  %p= page.description
  %ul
    %li First
    %li Second
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

- HTML5 void elements are emitted without a trailing slash (`<br>` not `<br />`).
- Tabs and arbitrary-width indentation are supported.
- The converter focuses on the structural subset of HAML; exotic Ruby constructs are not executed — only the documented forms are translated.
- The full site context (`site.*`, `page.*`, `pages`, `tags`, `collections`) is available to the Nunjucks pass, so `= site.title` works.

HAML is a pragmatic choice when porting existing templates. For new projects, the native [Zest DSL](/en/posts/zest-dsl/) or [Nunjucks](/en/posts/nunjucks/) are the primary template languages.
