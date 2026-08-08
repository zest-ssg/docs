+++
title = "Configuration"
description = "Configure a Zest site with _config.toml: the site, build, template, theme, defaults and params tables."
category = "guides"
tags = ["zest", "configuration", "config"]
date = 2026-08-01
+++

# Configuration

This guide explains the `_config.toml` file that configures a Zest site. Zest is zero-config: when `_config.toml` is absent, every field falls back to a sensible default, so you only override what you need. The file is parsed by `ConfigLoader` (Tomlyn); unknown keys are ignored and missing keys keep their defaults. A full key-by-key reference is available in [config-reference](/en/posts/config-reference/).

## File Layout

Configuration is organised into TOML tables. The starter project uses the conventional form:

```toml
[site]
title = "My Zest Blog"
description = "A fast, minimal blog built with Zest SSG."
url = "https://example.com"
author = "Zest User"
language = "en"

[build]
output = "_site"

[template]
engine = "native"

[theme]
name = "oxygen"
source = "local"
```

Root-level keys (`title = "..."`, `output_dir = "./_site"`, `template_engine = "native"`) remain supported as a flat fallback; the table form takes precedence when both are present.

## The `[site]` Table

Site identity. Common keys are `title`, `description`, `url` (the base URL used for absolute URLs such as RSS and canonical links), `author`, and `language`. These are exposed to templates as `{{ site.title }}`, `{{ site.description }}`, `{{ site.base_url }}`, and so on.

## The `[build]` Table

Build behaviour. `output` selects the output directory (default `_site`). Directory overrides such as `layouts_dir`, `includes_dir`, `data_dir`, and `assets_dir` also live here. Formatting and optimisation flags such as `enable_html_formatting`, `enable_asset_formatting`, `enable_minification`, and `enable_cache_busting` are recognised in both forms.

## The `[template]` Table

```toml
[template]
# The `engine` value is a PURE ANNOTATION for the primary template language.
# It has NO effect on the build — layout routing is decided by file extension:
#   native   → primary language is .zest.fsx (F# script templates)
#   nunjucks → primary language is Nunjucks (.njk / .html layouts)
#   liquid   → primary language is Liquid (.liquid layouts)
engine = "native"
```

The `engine` value is **purely informational**. Routing is decided by file extension in the layout and content pipeline: a `.zest.fsx` layout is evaluated by F# Interactive, a `.njk` layout by the Nunjucks engine, a `.hbs` layout by the Handlebars engine, and so on. You can set the label to anything; it does not change the build.

The related `nunjucks_compatibility` option selects between `"zest"` (Zest's extended filters on top of Nunjucks, the default) and `"strict"` (official Nunjucks only).

## The `[theme]` Table

Loads a theme from `_themes/`, a Git repository, a ZIP URL, or a local path:

```toml
[theme]
name = "minima"
source = "git"
git = "https://github.com/zest-ssg/zest-theme-minima.git"
tag = "v2.1.0"
```

Theme files act as **fallbacks**. Project-level layouts, includes, and assets with the same name overwrite theme files; anything missing falls back to the theme. See [themes](/en/posts/themes/) for details.

## `[[defaults]]` — Page Defaults

Applies default frontmatter to files matching a glob path. Lower-index entries win (first match):

```toml
[[defaults]]
path = "posts/*"
[defaults.values]
layout = "post"
```

## `[params]` — Theme Parameters

Arbitrary key/value parameters surfaced to templates as `site.params.*`. Nested tables become nested objects, so `site.params.colors.accent` resolves correctly:

```toml
[params.colors]
accent = "#4f6ef7"
```

Merge priority is **theme `_data/params.toml` < project `_data/params.toml` < `_config.toml [params]`** (highest). The merge is deep: `[params.colors]` replaces only the keys it specifies, leaving the rest of the sub-table intact.

## Other Tables

- `[[taxonomies]]` — taxonomy definitions (singular `name` + `plural`); the defaults are `tag`/`tags` and `category`/`categories`.
- `[menu.<name>]` — navigation menus exposed as `{{ menu.<name> }}` and `site_data "menu.<name>"`.
- `[pagination]` — `per_page` default for `@paginate` listing pages.
- `[compat]` — opt-in SSG compatibility flags (`jekyll`, `hexo`, `hugo`, `eleventy`), all off by default.
- `include` / `exclude` — glob patterns for files that the default underscore/prefix exclusion rules would otherwise skip.

## Global Data — `_data/`

TOML files in `_data/` become site-wide data namespaces. Each file's keys are flattened as `namespace.key` and read in templates as `{{ site.<namespace>.<key> }}` or from DSL scripts via `site_data`.

## `_init.zest.fsx`

Runs once before every build. It can load external data and inject globals via `addGlobal`, `loadJson`, `loadToml`, `loadEnv`, and `console_log` — everything added is exposed to templates as `{{ site.<key> }}`. The full examples are on the [dsl-collections](/en/posts/dsl-collections/) page.
