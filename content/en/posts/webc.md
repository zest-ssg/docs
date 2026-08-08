+++
title = "WebC"
description = "WebC support is not yet implemented in Zest. Use Markdown or another template language for now."
category = "templates"
tags = ["zest", "webc", "templates"]
date = 2026-08-01
+++

# WebC

**WebC support is not yet implemented in Zest.** This page documents the current status and the intended behavior so you can plan accordingly. It is part of the **Templates** section of the documentation, alongside [Liquid](/en/posts/liquid/), [Handlebars](/en/posts/handlebars/), [Mustache](/en/posts/mustache/), [HAML](/en/posts/haml/), and [Pug](/en/posts/pug/).

## Current Status

The `.webc` extension is recognized by the content pipeline and the frontmatter parser, but the WebC component model is **not** implemented. There is no component serialization, no isolated component scope, and no `webc:setup` script evaluation. In practice:

- A `.webc` file is not processed as a WebC component.
- Only a minimal server-side normalization exists in the template pipeline: `<script webc:setup>` blocks are stripped and `<template webc:nocss>` tags are normalized before the file is passed to the Nunjucks engine.
- Real-world WebC components (assets hoisting, `@html`/`@css` attributes, slot-like composition) will not work.

## What to Use Instead

For new content, use a fully supported format:

- **[Markdown](/en/posts/markdown/)** for prose and structured content — the primary authoring format.
- **[Nunjucks](/en/posts/nunjucks/)** (`.njk`) for templates that need logic, includes, and filters.
- **[Zest DSL](/en/posts/zest-dsl/)** (`.zest.fsx`) for type-safe, programmable pages and layouts.
- **[Liquid](/en/posts/liquid/)** or **[Handlebars](/en/posts/handlebars/)** when porting existing templates.

If you have `.webc` files in an existing project, convert them to one of the formats above before building.

## Future Behavior

When the WebC compatibility layer is complete, `.webc` files will be handled like every other template language: **routed by file extension** in the content pipeline and layout engine, just as `.njk`, `.liquid`, `.hbs`, and `.pug` files are today. A `.webc` layout under `_layouts/` would then render through the WebC pipeline with the full site context (`site.*`, `page.*`, `pages`, `tags`, `collections`), and `.webc` content files would be processed as components and wrapped in their declared layout.

Because routing is decided solely by file extension, no configuration change is required to opt in or out — a file named `.webc` will start being processed once the layer lands, and existing projects will be able to adopt it incrementally.

## Frontmatter

Once supported, `.webc` files are expected to use HTML-comment frontmatter, consistent with the other template extensions:

```html
<!-- @title Component Demo -->
<!-- @layout default -->
<h1>{{ page.title }}</h1>
```

Until then, keep an eye on the project changelog and this page for updates on implementation progress.
