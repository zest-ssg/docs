# Getting Started

## Prerequisites

- **.NET SDK** (10.0 or later recommended) — Zest requires `dotnet fsi` for evaluating `.zest.fsx` scripts

## Installation

```bash
dotnet tool install --global zest-ssg
```

Verify installation:

```bash
zest --version
```

## Creating a New Project

```bash
zest init my-site
cd my-site
```

This creates a scaffolded project with the default template:

```
my-site/
├── _config.toml
├── _layouts/
│   └── default.njk
├── content/
│   └── index.zest.fsx
└── assets/
    └── css/
        └── main.zcss
```

## Your First Page

Edit `content/index.zest.fsx`:

```fsharp
// @title  Welcome to My Site
// @layout default
// @description A site built with Zest

page {
    h1 [ text "Hello, Zest!" ]
    p  [ text "This is my first static site built with F#." ]
}
```

Metadata (title, layout, description, date, tags, etc.) is declared as `// @key value` comments at the top of the file. The parser reads these F# comment headers and populates the page's frontmatter. The `page { }` block contains only the page body content.

## Building

```bash
zest build
```

Output is written to `_site/`. All `.zest.fsx` scripts in the content directory are evaluated, layout-wrapped, and written as `.html` files.

## Development Server

```bash
zest serve
```

Starts a local HTTP server on port 8080 with:
- Auto-rebuild on file changes (file watcher)
- Live reload via WebSocket (port 35729)
- Directory listing support

```bash
# Custom port and auto-open browser
zest serve --port 3000 --open

# SPA mode (all routes fall back to index.html)
zest serve --spa
```

## Preview Built Output

```bash
zest preview
```

Serves the `_site/` directory without triggering a build. Useful for CI/CD or when you want to serve an existing build.

## Markdown Pages

Create `content/about.md`:

```markdown
+++
title = "About"
layout = "default"
date = 2026-01-15
tags = ["meta"]
+++

# About This Site

This is a Zest-powered static site. Content written in **Markdown**
is automatically rendered to HTML.
```

## How It Works

1. **Discovery** — Zest scans the content directory for `.zest.fsx`, `.fsx`, `.md`, `.markdown`, and template files
2. **Metadata extraction** — Frontmatter is parsed from TOML (`+++` delimiters), F# comments (`// @key value`), or HTML comments (`<!-- @key value -->`)
3. **Evaluation** — `.zest.fsx` scripts run via `dotnet fsi` with the Zest.Dsl pre-compiled library loaded. Markdown files are parsed by the built-in engine.
4. **Layout wrapping** — Each page's content is inserted into its layout template (e.g., `_layouts/default.njk`) with placeholder substitution
5. **Asset copying** — `assets/` is copied to `_site/assets/`, with `.zcss` files compiled to `.css`
6. **Output** — Final HTML files are written to `_site/`

## Next Steps

- [configuration.md](configuration.md) — Configure your site with `_config.toml`
- [dsl-guide.md](dsl-guide.md) — Learn the `page { }` computation expression
- [dsl-style.md](dsl-style.md) — Write CSS with the F#-native `stylesheet { }` CE (dot-notation selectors, typed property functions)
- [zcss.md](zcss.md) — Write CSS with the `.zcss` preprocessor language (SCSS-like, indent or brace syntax)
- [templates.md](templates.md) — Create layout templates
