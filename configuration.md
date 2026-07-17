# Configuration — `_config.toml`

Zest is zero-config: if `_config.toml` is absent, all fields fall back to sensible defaults. Only override what you need.

## Complete Reference

```toml
# ── Site Identity ────────────────────────────────────
title = "My Zest Site"
base_url = "http://localhost:8080"
description = "A site built with Zest SSG"
author = ""
language = "en"
site_version = "1.0"

# ── Directory Structure ──────────────────────────────
root_dir = "content"         # Content root; "." = project root
content_dir = "./content"
output_dir = "./_site"
layouts_dir = "./_layouts"
includes_dir = "./_includes"
data_dir = "./_data"
assets_dir = "./assets"

# ── Defaults ─────────────────────────────────────────
default_layout = "default"
permalink_format = "/:slug/"

# ── Dev Server ───────────────────────────────────────
dev_server_port = 8080
live_reload_port = 35729

# ── Build Options ────────────────────────────────────
enable_minification = false
enable_cache_busting = false
enable_parallel_build = true
enable_incremental_build = true

# ── Template Engine ──────────────────────────────────
# "native" ({{ }} placeholders) or "nunjucks" (Nunjucks-compatible)
template_engine = "native"

# ── Logging ──────────────────────────────────────────
# "Debug" | "Info" | "Warn" | "Error" | "Off"
log_level = "Info"
log_to_file = false
log_timestamps = true

# ── Navigation Menus ─────────────────────────────────
[menu.main]
    [[menu.main]]
    label = "Home"
    url = "/"
    weight = 1

    [[menu.main]]
    label = "Blog"
    url = "/posts/"
    weight = 2

    [[menu.main]]
    label = "About"
    url = "/about/"
    weight = 3

# You can define multiple menus:
# [menu.footer]
#     [[menu.footer]]
#     label = "Privacy"
#     url = "/privacy/"
#     weight = 1

# ── Taxonomies ───────────────────────────────────────
[[taxonomies]]
name = "tag"
plural = "tags"

[[taxonomies]]
name = "category"
plural = "categories"
```

## Configuration Fields

### Site Identity

| Field | Type | Default | Description |
|---|---|---|---|
| `title` | string | `"My Zest Site"` | Site title, available as `{{ site.title }}` in templates |
| `base_url` | string | `"http://localhost:8080"` | Base URL for absolute URL generation |
| `description` | string | `"A site built with Zest SSG"` | Site description for SEO and meta tags |
| `author` | string | `""` | Default author name |
| `language` | string | `"en"` | Site language (HTML `lang` attribute) |
| `site_version` | string | `"1.0"` | Version string for cache busting |

### Directory Structure

| Field | Type | Default | Description |
|---|---|---|---|
| `root_dir` | string | `"content"` | Content root directory. Set to `"."` to use project root. |
| `content_dir` | string | `"./content"` | Content directory path |
| `output_dir` | string | `"./_site"` | Build output directory |
| `layouts_dir` | string | `"./_layouts"` | Layout templates directory |
| `includes_dir` | string | `"./_includes"` | Partial templates directory |
| `data_dir` | string | `"./_data"` | Global data directory (`.toml` files) |
| `assets_dir` | string | `"./assets"` | Static assets directory |

### Build Options

| Field | Type | Default | Description |
|---|---|---|---|
| `enable_minification` | bool | `false` | Minify HTML output |
| `enable_cache_busting` | bool | `false` | Append version hash to asset URLs |
| `enable_parallel_build` | bool | `true` | Process pages in parallel |
| `enable_incremental_build` | bool | `true` | Skip unchanged pages (mtime+hash cache) |

### Template Engine

| Field | Type | Default | Description |
|---|---|---|---|
| `template_engine` | string | `"native"` | `"native"` for `{{ }}` placeholders, `"nunjucks"` for Nunjucks-compatible engine |

### Logging

| Field | Type | Default | Description |
|---|---|---|---|
| `log_level` | string | `"Info"` | Minimum log level: `Debug`, `Info`, `Warn`, `Error`, `Off` |
| `log_to_file` | bool | `false` | Mirror logs to `.zest/logs/zest.log` |
| `log_timestamps` | bool | `true` | Include timestamps in console output |

### Menus

Menus are defined under `[menu.{name}]` sections. Each menu entry is an array of tables with:

| Field | Type | Description |
|---|---|---|
| `label` | string | Display text |
| `url` | string | Link URL |
| `weight` | int | Sort order (lower = first) |

Menu items are exposed in templates as `{{ menu.{name} }}` (JSON array string). In `.zest.fsx` scripts, they are available via `site_data "menu.{name}"`.

### Taxonomies

Define classification systems for content:

| Field | Type | Description |
|---|---|---|
| `name` | string | Singular form (e.g., `"tag"`) |
| `plural` | string | Plural form (e.g., `"tags"`) |

Default taxonomies: `{ name = "tag"; plural = "tags" }`, `{ name = "category"; plural = "categories" }`.

## Global Data (`_data/` directory)

Place `.toml` files in `_data/` to make data available site-wide. Each file becomes a namespace:

```
_data/
├── site.toml        # Accessible as {{ site.social_twitter }}, site_data "site.social_twitter"
└── authors.toml     # Accessible as {{ site.authors_alice_name }}, site_data "authors.alice.name"
```

TOML keys are flattened with `namespace.key` notation. For example:

```toml
# _data/site.toml
[social]
twitter = "@mysite"
github  = "https://github.com/user/repo"
```

Available in templates as `{{ site.site.social_twitter }}` and `{{ site.site.social_github }}`.

## `_init.zest.fsx` — Global Init Script

Place `_init.zest.fsx` at the project root to run custom initialization before the build. The script can:

- Load external data via `loadJson`, `loadToml`, `loadEnv`
- Add global data via `addGlobal`
- Log messages via `console_log`

Available APIs in init scripts:

| Function | Signature | Description |
|---|---|---|
| `addGlobal` | `string * obj -> unit` | Add key-value pair to global site data |
| `loadJson` | `string -> obj` | Load and parse a JSON file |
| `loadToml` | `string -> obj` | Load and parse a TOML file |
| `loadEnv` | `string -> string` | Read an environment variable |
| `console_log` | `string -> unit` | Log a message during init |
| `exec` | `string -> string` | Execute a shell command and return stdout |

Example:

```fsharp
// _init.zest.fsx
let posts = loadJson "data/posts.json"
addGlobal ("total_posts", posts.Length)
console_log ("Loaded " + string posts.Length + " posts")
```
