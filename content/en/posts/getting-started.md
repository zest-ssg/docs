+++
title = "Getting Started"
description = "Install Zest, scaffold a project, and write your first page with the F# DSL or Markdown."
category = "guides"
tags = ["zest", "getting-started", "installation", "cli"]
date = 2026-08-01
+++

# Getting Started

This guide is the entry point to the Zest documentation. It covers installing the tool, creating a project, understanding the standard directory layout, and writing your first page — both as a native `.zest.fsx` F# script and as a Markdown file. The [configuration](/en/posts/configuration/) and [CLI](/en/posts/cli/) guides build on the workflow introduced here.

## Prerequisites

Zest is a .NET tool. You need:

- **.NET SDK** (10.0 or later recommended). Zest evaluates `.zest.fsx` scripts with `dotnet fsi`, so an SDK — not just a runtime — is required.

## Installation

Install the tool globally:

```bash
dotnet tool install --global zest-ssg
```

Verify the installation:

```bash
zest --version
```

## Creating a Project

Scaffold a new site from the bundled starter project:

```bash
zest init my-site
cd my-site
```

`zest init` copies the starter site into the target directory (see `zest scaffold` for the `empty` preset, which writes only a minimal `_config.toml` plus `_init.zest.fsx`).

## Directory Structure

A freshly initialised project looks like this:

```
my-site/
├── _config.toml            # site metadata, build options, defaults
├── _init.zest.fsx          # runs before every build; injects global data
├── _data/
│   └── nav.toml            # global data, exposed as site.nav
├── _themes/
│   └── oxygen/             # self-contained theme (layouts, includes, assets)
│       ├── _theme.toml     # theme manifest (metadata, data, filters)
│       ├── _layouts/       # default.html, post.html, ...
│       ├── _includes/      # header.html, footer.html
│       ├── _locales/       # en.toml, zh.toml (i18n strings)
│       └── assets/css/     # main.zcss → compiled to CSS at build time
├── content/
│   ├── index.zest.fsx      # home page (F# DSL)
│   ├── about.zest.fsx      # static page
│   └── posts/
│       ├── index.njk       # paginated post listing
│       └── *.md            # blog posts (Markdown)
├── assets/                 # project-level assets (override theme files)
└── _site/                  # build output (generated)
```

Content lives in `content/`, configuration in `_config.toml`, layouts in `_layouts/`, partials in `_includes/`, and global data in `_data/`. Themes live under `_themes/` and act as fallbacks: project-level files with the same name take priority.

## Your First Page

Pages are `.zest.fsx` F# scripts evaluated by F# Interactive at build time. Metadata is declared as `// @key value` comment headers, and the `page { }` computation expression builds the HTML body:

```fsharp
// @title  Welcome to My Site
// @layout default
// @description A site built with Zest

page {
    h1 [ text "Hello, Zest!" ]
    p  [ text "This is my first static site built with F#." ]
}
```

## Markdown Pages

Markdown files use `+++` TOML frontmatter:

```markdown
+++
title = "About"
layout = "default"
date = 2026-01-15
tags = ["meta"]
+++

# About This Site

This is a Zest-powered static site. Content written in **Markdown**
is rendered to HTML automatically.
```

See the [markdown](/en/posts/markdown/) and [Zest DSL](/en/posts/zest-dsl/) pages for the full capabilities of each format.

## Building

```bash
zest build
```

The site is written to `_site/`. Every `.zest.fsx` script and Markdown file in the content directory is rendered, wrapped in its layout, and emitted as `.html`. Add `--watch` to rebuild on file changes.

## Development Server

```bash
zest serve
```

Starts a local HTTP server (default port 8080) with auto-rebuild on file changes, live reload over WebSocket (default port 35729), and optional directory listing:

```bash
# Custom port and auto-open browser
zest serve --port 3000 --open

# SPA mode (all routes fall back to index.html)
zest serve --spa
```

## Previewing Built Output

```bash
zest preview
```

Serves the already-built `_site/` directory **without triggering a build** — useful in CI/CD or when you only want to inspect a previous build. Add `--watch` or `--livereload` for live behavior.

## How the Build Works

1. **Discovery** — Zest scans the content directory for `.zest.fsx`, `.fsx`, `.md`, `.markdown`, and template files.
2. **Metadata extraction** — Frontmatter is parsed from TOML (`+++` delimiters), F# comments (`// @key value`), or HTML comments (`<!-- @key value -->`).
3. **Evaluation** — `.zest.fsx` scripts run via `dotnet fsi` with the `Zest.Dsl` library loaded; Markdown is parsed by the built-in engine; template formats are routed by extension to the matching engine.
4. **Layout wrapping** — Each page's body is inserted into its layout (for example `_layouts/default.njk`).
5. **Assets** — `assets/` is copied to `_site/assets/`, with `.zcss` files compiled to `.css`.
6. **Output** — Final HTML is written to `_site/`.

## Next Steps

- [configuration](/en/posts/configuration/) — configure your site with `_config.toml`
- [Zest DSL](/en/posts/zest-dsl/) — learn the `page { }` computation expression
- [zcss](/en/posts/zcss/) — write styles with the `.zcss` preprocessor
- [templates](/en/posts/template-reference/) — create layout templates
