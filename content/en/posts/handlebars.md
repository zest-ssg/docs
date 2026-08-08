+++
title = "Handlebars"
description = "Render Handlebars templates in Zest with a standalone Hbs engine: helpers, partials and raw output."
category = "templates"
tags = ["zest", "handlebars", "templates"]
date = 2026-08-01
+++

# Handlebars

Handlebars is supported through a **standalone Hbs engine** that renders `.hbs` templates directly — it does not go through the Nunjucks converter. This matters because Handlebars has syntax that Nunjucks cannot express faithfully, such as unescaped triple-stash output, inverted sections, and parent-context lookups. This page is part of the **Templates** section of the documentation; see [Mustache](/en/posts/mustache/) for the logic-less variant, which runs on the same engine.

## Engine Selection

Files and layouts with the `.hbs` extension are routed to the `hbs` engine by file extension. The engine implements native Mustache/Handlebars semantics, including:

- `{{ expr }}` — HTML-escaped output; `{{{ expr }}}` and `{{& expr }}` — unescaped output.
- `{{#section}}…{{else}}…{{/section}}` — blocks with an else branch.
- `{{^inverted}}…{{/inverted}}` — inverted sections.
- `{{#each list}}` with `{{@index}}`, `{{@key}}`, `{{@first}}`, `{{@last}}`.
- `{{this}}`, `.` (current context), `../` parent-context lookups, and `@root`.
- `{{> partial}}` partials, loaded from the site includes dictionary.
- Built-in helpers `{{#if}}`, `{{#unless}}`, and `{{#with}}`.
- `{{! comment }}` and `{{!-- block comment --}}`.

## Variables

```handlebars
<h1>{{ page.title }}</h1>
<p>{{ page.description }}</p>

{{! unescaped HTML, e.g. the rendered page body }}
{{#if content}}{{{ content }}}{{/if}}
```

## Conditionals and Loops

```handlebars
{{#if page.tags}}
  <ul>
    {{#each page.tags}}
      <li>{{@index}}: {{this}}</li>
    {{/each}}
  </ul>
{{else}}
  <p>No tags.</p>
{{/if}}
```

## Partials

Partials resolve by name from `_includes/`:

```handlebars
{{> header}}
<main>{{{ content }}}</main>
{{> footer}}
```

The Hbs engine loads partials directly from the includes dictionary, with no conversion step.

## Context

As with every template format, the full site context is injected: `site.title`, `site.description`, `page.title`, `page.url`, `page.date`, `page.tags`, `pages`, `tags`, and `collections`. Handlebars lookups traverse the nested context, so `{{site.base_url}}{{page.url}}` behaves as expected.

## Differences from Nunjucks

When moving between the two engines, note:

- Handlebars **escapes by default**; `{{{ }}}` is required for raw HTML. Nunjucks requires the `| safe` filter for the same effect.
- Helpers are invoked with `{{#if x}}…{{/if}}` rather than `{% if x %}…{% endif %}`.
- There is no layout inheritance (`{% extends %}`/`{% block %}`) in Handlebars — wrap pages with Zest layouts instead.
- Filter chains are not part of Handlebars; precompute values in the F# DSL or use the helpers Zest registers.

## Frontmatter

`.hbs` files use HTML-comment frontmatter like the other template extensions:

```html
<!-- @title About -->
<!-- @layout default -->
<h1>{{ page.title }}</h1>
```

For new projects, the native [Zest DSL](/en/posts/zest-dsl/) and [Nunjucks](/en/posts/nunjucks/) remain the primary template languages.
