# Configuration — `_config.toml`

Zest is **zero-config**: when `_config.toml` is absent, every field falls back to the defaults shown below. Override only what you need. The file is parsed by `ConfigLoader` (Tomlyn); unknown keys are ignored and missing keys keep their defaults.

> All path fields are resolved relative to the project root. Setting `root_dir = "."` makes the project root itself the content directory (so `index.zest.fsx` may live at the root).

---

## Field Reference

### Site Identity

| Field | Type | Default | Description |
|---|---|---|---|
| `title` | string | `"My Zest Site"` | Site title; available as `{{ site.title }}` |
| `base_url` | string | `"http://localhost:8080"` | Base URL for absolute URLs (RSS, canonical, OG) |
| `description` | string | `"A site built with Zest SSG"` | Default site description |
| `author` | string | `""` | Default author name |
| `language` | string | `"en"` | HTML `lang` attribute |
| `site_version` | string | `"1.0"` | Version string (used for cache busting) |

### Directory Structure

| Field | Type | Default | Description |
|---|---|---|---|
| `root_dir` | string | `"content"` | Content root. `"."` or `""` → project root |
| `content_dir` | string | `"./content"` | Content directory (backward-compat alias) |
| `output_dir` | string | `"./_site"` | Build output directory |
| `layouts_dir` | string | `"./_layouts"` | Layout templates |
| `includes_dir` | string | `"./_includes"` | Partial templates |
| `data_dir` | string | `"./_data"` | Global `.toml` data files |
| `assets_dir` | string | `"./assets"` | Static assets (copied to `_site/assets/`) |

### Build Options

| Field | Type | Default | Description |
|---|---|---|---|
| `enable_minification` | bool | `false` | Minify HTML output |
| `enable_cache_busting` | bool | `false` | Append version hash to asset URLs |
| `enable_parallel_build` | bool | `true` | Render pages in parallel |
| `enable_incremental_build` | bool | `true` | Skip unchanged pages (content-hash + mtime cache) |

### Dev Server

| Field | Type | Default | Description |
|---|---|---|---|
| `dev_server_port` | int | `8080` | HTTP server port for `serve`/`preview` |
| `live_reload_port` | int | `35729` | WebSocket port for live reload |

### Template Engine

| Field | Type | Default | Description |
|---|---|---|---|
| `template_engine` | string | `"native"` | `"native"` → `{{ }}` placeholders only; `"nunjucks"` → full Nunjucks-compatible engine |

> The classic Nunjucks spelling is **`nunjucks`** (and its TOML key `nunjucks_compatibility` below). Layout files using the `.njk` extension are always rendered through the Nunjucks engine regardless of this setting.

### Logging

| Field | Type | Default | Description |
|---|---|---|---|
| `log_level` | string | `"Info"` | `Debug` \| `Info` \| `Warn` \| `Error` \| `Off` |
| `log_to_file` | bool | `false` | Mirror logs to `.zest/logs/zest.log` |
| `log_timestamps` | bool | `true` | Prefix console lines with timestamps |

### Menus — `[menu.<name>]`

Each menu is a TOML **array of tables** under `[menu.<name>]`:

| Field | Type | Description |
|---|---|---|
| `label` | string | Display text |
| `url` | string | Link URL (`"#"` if omitted) |
| `weight` | int | Sort order (lower = first; `0` if omitted) |

Exposed in templates as `{{ menu.<name> }}` (JSON array) and in scripts via `site_data "menu.<name>"`.

### Taxonomies — `[[taxonomies]]`

| Field | Type | Description |
|---|---|---|
| `name` | string | Singular form, e.g. `"tag"` |
| `plural` | string | Plural form, e.g. `"tags"` |

Default taxonomies: `{ name = "tag"; plural = "tags" }`, `{ name = "category"; plural = "categories" }`.

### Compatibility — `[compat]`

Opt-in SSG-compatible behavior. All default to `false`.

| Field | Type | Enables |
|---|---|---|
| `jekyll` | bool | Jekyll-style permalinks, default layout, etc. |
| `hexo` | bool | Hexo-compatible behavior |
| `hugo` | bool | Hugo-compatible behavior |
| `eleventy` | bool | 11ty-compatible collection shape / API |

### Nunjucks Mode — `[template]`

Groups template-engine settings. `engine` overrides the top-level `template_engine`; `nunjucks_compatibility` selects the filter/macro set.

| Field | Type | Default | Description |
|---|---|---|---|
| `engine` | string | *(top-level value)* | `"native"` \| `"nunjucks"` |
| `nunjucks_compatibility` | string | `"zest"` | `"zest"` = Zest extensions on top of Nunjucks; `"strict"` = official Nunjucks only (Zest custom filters like `pages_by_tag` are skipped) |

Nested form is also accepted:

```toml
[template]
engine = "nunjucks"

[template.nunjucks]
compatibility = "zest"   # "strict" | "zest"
```

### Theme — `[theme]`

Load a theme from `_themes/`, a Git repository, a ZIP URL, or a local path. Theme files act as **fallbacks**: project-level layouts/includes/assets with the same name overwrite theme files. See [themes.md](themes.md) for a detailed guide.

| Field | Type | Default | Description |
|---|---|---|---|
| `name` | string | `""` | Theme directory name (e.g. `"minima"`). Empty = no theme. |
| `source` | string | `"local"` | `"local"` \| `"git"` \| `"url"` \| `"path"` |
| `git` | string | `""` | Git repository URL (`source = "git"`) |
| `branch` | string | `"main"` | Git branch (`source = "git"`) |
| `tag` | string | `""` | Git tag, overrides `branch` (`source = "git"`) |
| `url` | string | `""` | ZIP download URL (`source = "url"`) |
| `path` | string | `""` | Local directory path (`source = "path"`) |

**Examples:**

```toml
# Local theme (from _themes/minima/)
[theme]
name = "minima"

# Git theme (cloned to .zest/themes/minima/)
[theme]
name = "minima"
source = "git"
git = "https://github.com/zest-ssg/zest-theme-minima.git"
tag = "v2.1.0"

# URL theme (ZIP archive)
[theme]
name = "minima"
source = "url"
url = "https://github.com/zest-ssg/zest-theme-minima/archive/refs/tags/v2.1.0.zip"

# Path theme (local development)
[theme]
name = "minima"
source = "path"
path = "../zest-theme-minima"
```

---

## Complete Example

```toml
# ── Site Identity ────────────────────────────────────
title = "My Zest Site"
base_url = "http://localhost:8080"
description = "A site built with Zest SSG"
author = ""
language = "en"
site_version = "1.0"

# ── Directory Structure ──────────────────────────────
root_dir = "content"
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
template_engine = "native"

# ── Logging ──────────────────────────────────────────
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

# ── Taxonomies ───────────────────────────────────────
[[taxonomies]]
name = "tag"
plural = "tags"

[[taxonomies]]
name = "category"
plural = "categories"

# ── Compatibility (opt-in) ──────────────────────────
[compat]
jekyll = false
hexo = false
hugo = false
eleventy = false

# ── Nunjucks mode (only needed when engine = "nunjucks") ─
[template]
engine = "native"

[template.nunjucks]
compatibility = "zest"

# ── Theme ──────────────────────────────────────────────
[theme]
# name = "minima"
# source = "local"       # "local" | "git" | "url" | "path"
# git = ""               # (only when source = "git")
# tag = ""               # (optional, overrides branch)
```

---

## Global Data — `_data/` Directory

Place `.toml` files in `_data/` to make data available site-wide. Each file becomes a namespace; keys are flattened as `namespace.key`.

```toml
# _data/social.toml
[twitter]
handle = "@mysite"
url = "https://twitter.com/mysite"
```

Access in templates: `{{ site.social.twitter.handle }}`. In scripts: `site_data "social.twitter.handle"`.

## `_init.fsx` — Global Init Script

Place `_init.fsx` at the project root to run before every build. It can load external data and inject globals:

| Function | Signature | Description |
|---|---|---|
| `addGlobal` | `string * obj -> unit` | Add a key-value pair to global site data |
| `loadJson` | `string -> obj` | Load and parse a JSON file (or URL) |
| `loadToml` | `string -> obj` | Load and parse a TOML file |
| `loadEnv` | `string -> string` | Read an environment variable (or `""`) |
| `console_log` | `string -> unit` | Log a message during init |
| `exec` | `string -> string` | Execute a shell command, return stdout |

```fsharp
// _init.fsx
let posts = loadJson "data/posts.json"
addGlobal ("total_posts", posts.Length)
console_log ("Loaded " + string posts.Length + " posts")
```
