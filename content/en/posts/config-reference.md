+++
title = "Configuration Reference"
description = "Complete reference for _config.toml — every table, key, type and default in Zest's site configuration."
category = "reference"
tags = ["zest", "reference", "configuration", "toml"]
date = 2026-08-01
+++

# Configuration Reference

This page is the definitive reference for `_config.toml`. It lists every recognized table and key with its type, default and meaning, and is the companion to the hands-on [configuration guide](/en/posts/configuration/).

## File Location and Parsing

The configuration file is `_config.toml` at the project root, parsed by the Tomlyn TOML parser. Zest is zero-config: when the file is absent every field keeps its default, unknown keys are ignored, and only explicitly set fields override the defaults. Path fields resolve relative to the project root, and the file's last-write time is tracked so repeated loads reuse a cached parse.

## The `[site]` Table

| Key | Type | Default | Description |
|---|---|---|---|
| `title` | string | `"My Zest Site"` | Site title; exposed as `site.title` |
| `url` | string | `"http://localhost:8080"` | Base URL (trailing `/` trimmed); exposed as `site.base_url` |
| `description` | string | `"A site built with Zest SSG"` | Default site description |
| `author` | string | `""` | Default author name |
| `language` | string | `"en"` | Primary language code |
| `site_version` | string | `"1.0"` | Version string, used for cache busting |
| `root_dir` | string | `"content"` | Content root; `"."` makes the project root the content directory |
| `content_dir` | string | `"./content"` | Content directory (backward-compatible alias) |
| `default_layout` | string | `"default"` | Layout applied when a page declares none |
| `permalink_format` | string | `"/:slug/"` | Default URL pattern |
| `dev_server_port` | int | `8080` | Port for `serve` and `preview` |
| `live_reload_port` | int | `35729` | WebSocket port for live reload |
| `enable_minification` | bool | `false` | Minify HTML output |
| `enable_asset_formatting` | bool | `false` | Pretty-print CSS/JS output |
| `enable_html_formatting` | bool | `false` | Pretty-print HTML output |
| `enable_cache_busting` | bool | `false` | Append a version hash to asset URLs |
| `enable_parallel_build` | bool | `true` | Render pages in parallel |
| `enable_incremental_build` | bool | `true` | Skip unchanged pages via content-hash and mtime cache |
| `log_level` | string | `"Info"` | `Debug` \| `Info` \| `Warn` \| `Error` \| `Off` |
| `log_to_file` | bool | `false` | Mirror logs to `.zest/logs/zest.log` |
| `log_timestamps` | bool | `true` | Prefix console lines with timestamps |

## The `[build]` Table

| Key | Type | Default | Description |
|---|---|---|---|
| `output` | string | `"./_site"` | Build output directory |
| `layouts_dir` | string | `"./_layouts"` | Layout templates |
| `includes_dir` | string | `"./_includes"` | Partial templates |
| `data_dir` | string | `"./_data"` | Global `.toml` data files |
| `assets_dir` | string | `"./assets"` | Static assets, copied to `_site/assets/` |

Top-level keys such as `base_url`, `output_dir`, `layouts_dir`, `includes_dir`, `data_dir`, `assets_dir`, `default_layout` and `permalink_format` remain supported fallbacks when no table is present.

## The `[template]` Table

| Key | Type | Default | Description |
|---|---|---|---|
| `engine` | string | `"native"` | Labels the primary template language; a **pure annotation** — routing is decided by file extension |
| `nunjucks_compatibility` | string | `"zest"` | `"zest"` enables Zest extension filters; `"strict"` matches official Nunjucks (extension filters such as `pages_by_tag` are skipped) |

`engine` overrides the top-level `template_engine` key. The nested form `[template.nunjucks]` with `compatibility = "..."` is equivalent to the flat key.

## The `[compat]` Table

Opt-in SSG compatibility flags, all defaulting to `false`: `jekyll`, `hexo`, `hugo`, `eleventy`.

## `[[taxonomies]]` Array of Tables

Each entry has `name` (singular) and `plural`, e.g. `{ name = "tag"; plural = "tags" }`. Defaults: `tag`/`tags` and `category`/`categories`.

## `[menu.<name>]` Arrays of Tables

Each named menu holds an array of tables with `label`, `url` (default `"#"`) and `weight` (default `0`); entries are sorted by weight and exposed to templates as `site.menu.<name>`.

## The `[theme]` Table

| Key | Type | Default | Description |
|---|---|---|---|
| `name` | string | `""` | Theme directory; empty means no theme |
| `source` | string | `"local"` | `local` \| `git` \| `url` \| `path` |
| `git` | string | `""` | Repository URL (`source = "git"`) |
| `branch` | string | `"main"` | Git branch (`source = "git"`) |
| `tag` | string | `""` | Git tag, overrides `branch` |
| `url` | string | `""` | ZIP download URL (`source = "url"`) |
| `path` | string | `""` | Local directory (`source = "path"`) |

## `[[defaults]]` Page Defaults

Each entry has `path`, a glob pattern, and a `values` table of front-matter defaults:

```toml
[[defaults]]
path = "content/en/posts/*"
[defaults.values]
layout = "docs"
```

Lower-index entries win; the first matching pattern applies. Matching uses the file name, the relative path, `*.ext` suffixes and `dir/*` prefixes.

## The `[params]` Table

Arbitrary theme parameters. Nested tables become nested dictionaries and arrays stay arrays, so both Nunjucks and DSL scripts can traverse them. Exposed as `site.params.*` with a deep merge over `_data/params.toml` values.

## The `[pagination]` Table

`per_page` (int, default `10`) — the page size for paginated listings; a content file overrides it per page with `@paginate`.

## `include` and `exclude` Arrays

Glob patterns of files to force-include (even with a leading `_` or `.`) or to exclude from the content pipeline.

## Complete Example

```toml
[site]
title = "My Zest Site"
url = "https://example.com"
description = "A site built with Zest SSG"
language = "en"

[build]
output = "_site"

[template]
engine = "nunjucks"

[theme]
name = "minima"
source = "git"
git = "https://github.com/zest-ssg/zest-theme-minima.git"

[[defaults]]
path = "posts/*"
[defaults.values]
layout = "post"

[params.colors]
accent = "#4f6ef7"

[[taxonomies]]
name = "tag"
plural = "tags"

[pagination]
per_page = 10

exclude = ["README.md", "drafts/*"]
```
