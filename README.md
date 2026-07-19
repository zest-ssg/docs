# Zest SSG Documentation

**Zest** (Zealous Efficient Static Toolkit) is a static site generator built on .NET with a unique F#-powered DSL. It compiles `.zest.fsx` F# script files into static HTML pages, offering type-safe templating, a built-in CSS preprocessor (ZCSS), and multi-template-engine support.

## Architecture

Zest consists of four layered assemblies:

| Assembly | Language | Role |
|---|---|---|
| `Zest.App` | C# | CLI entry point, command parsing, controller dispatch |
| `Zest.Infra` | C# | Configuration loading, dev server, file watcher, HTTP infrastructure |
| `Zest.Engine` | F# | Core build pipeline, HTML/DOM types, ZCSS compiler, template engines, scripting, layouts, routing |
| `Zest.Dsl` | F# | Pre-compiled DSL library loaded by F# Interactive (FSI) scripts — provides `page { }` CE, HTML builders, CSS DSL, collections API |

## Key Features

- **F# computation expression DSL** — Write pages with `// @key value` frontmatter headers + `page { }` CE for full IDE support
- **ZCSS preprocessor** — SCSS-like CSS with variables, mixins, `@extend`, `@apply`, color functions, responsive shorthand, and indentation syntax; CSS function passthrough (`calc`/`color-mix`/`clamp`), unit-safe `calc()` evaluation, and content-hash result caching
- **F#-native CSS DSL** — `stylesheet { }` CE with dot-notation selectors, 200+ typed property functions, pseudo-classes, combinators, and at-rules
- **Multi-template support** — Native `{{ placeholder }}` syntax, full Nunjucks engine, plus converters for Liquid, Handlebars/Mustache, HAML, and Pug (with HTML escaping, filter blocks, `{{#with}}`/`{{else if}}`, and conversion caching)
- **Inline JavaScript** — `js """..."""` blocks mirror `md` triple-quote ergonomics; `jsonBlock` injects type-safe F#→JS data via JSON
- **Markdown support** — `.md` files rendered through a built-in Markdown engine with frontmatter; `md`/`mdDedent` helpers for inline Markdown in `.zest.fsx`
- **Collections & pagination** — Page query APIs, tag clouds, related pages, grouped pages by year; `by_collection` with optional index exclusion
- **TOML data** — `_data/*.toml` arrays and nested tables preserved as native types, directly iterable in Nunjucks templates
- **SEO & feeds** — Meta tags, Open Graph, Twitter Cards, RSS 2.0, Atom 1.0, Sitemap XML
- **Scoped CSS** — Component-level style isolation via auto-generated data attributes
- **Dev server** — `zest serve` with live reload via WebSocket, file watching, and auto-rebuild
- **Incremental builds** — Content-hash-based caching for fast rebuilds
- **Zero-config** — Sensible defaults; only specify what you need in `_config.toml`

## Project Layout

```
project/
├── _config.toml          # Site configuration (optional, defaults work)
├── _init.zest.fsx        # Global init script (data loading, API calls)
├── _layouts/             # Layout templates (.njk, .html, etc.)
│   └── default.njk
├── _includes/            # Partial templates included via {{ include name }}
├── _data/                # Global data files (.toml) — loaded as site.* globals
├── content/              # Content directory (configurable via root_dir)
│   ├── index.zest.fsx    # Home page
│   ├── about.md          # Markdown page
│   └── posts/
│       └── hello.zest.fsx
├── assets/               # Static assets (copied to _site/assets/)
│   ├── css/
│   │   └── main.zcss     # ZCSS stylesheet (compiled to .css)
│   └── img/
└── _site/                # Build output
```

## Supported File Formats

| Extension | Processing |
|---|---|
| `.zest.fsx` | F# script via `dotnet fsi` — uses `page { }` CE DSL |
| `.fsx` | Plain F# script (treated as Zest page if it uses Zest DSL) |
| `.md`, `.markdown` | Markdown with TOML/HTML-comment frontmatter |
| `.njk` | Nunjucks-compatible template |
| `.liquid` | Liquid template (converted to Nunjucks) |
| `.hbs`, `.mustache` | Handlebars/Mustache (converted to Nunjucks) |
| `.webc` | WebC template (treated as Nunjucks) |
| `.haml` | HAML (auto-converted to HTML → Nunjucks) |
| `.pug` | Pug/Jade (auto-converted to HTML → Nunjucks) |
| `.html`, `.htm` | Native HTML (preprocessed through Nunjucks if `{{ }}` or `{% %}` detected) |
| `.zcss` | ZCSS stylesheet (compiled to `.css`) |

## Document Index

| Document | Content |
|---|---|
| [getting-started.md](getting-started.md) | Installation, project initialization, first build, dev server |
| [configuration.md](configuration.md) | Complete `_config.toml` reference |
| [cli.md](cli.md) | CLI commands: `build`, `serve`, `preview`, `init` |
| [dsl-guide.md](dsl-guide.md) | Writing pages with `page { }` computation expressions |
| [dsl-api.md](dsl-api.md) | Complete DSL API: HTML builders, components, helpers, collections |
| [dsl-style.md](dsl-style.md) | CSS DSL: `stylesheet { }` CE, selectors, at-rules |
| [zcss.md](zcss.md) | ZCSS preprocessor: syntax, features, built-in styles, color pipeline |
| [templates.md](templates.md) | Template engines, layouts, includes, Nunjucks filters |
| [collections.md](collections.md) | Page collections, queries, pagination, tags, global data |

## Quick Start

```bash
# Install (requires .NET SDK)
dotnet tool install --global zest-ssg

# Create a new project
zest init my-site
cd my-site

# Build
zest build

# Start dev server with live reload
zest serve --open
```
