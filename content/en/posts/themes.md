+++
title = "Themes"
description = "Package layouts, includes, assets and locale strings into a reusable theme with _theme.toml."
category = "guides"
tags = ["zest", "themes", "theme"]
date = 2026-08-01
+++

# Themes

This guide explains Zest's theme system. A theme is a self-contained directory under `_themes/` (or fetched from a remote source) that provides layouts, includes, assets, ZCSS styles, locale strings, and a declarative `_theme.toml` manifest. Theme files act as **fallbacks**: project-level files with the same name take priority.

## Quick Start

Place a theme directory under `_themes/` and enable it in `_config.toml`:

```toml
[theme]
name = "oxygen"
source = "local"
```

The starter project ships with the self-contained `oxygen` theme:

```
_themes/oxygen/
├── _theme.toml        # manifest: metadata, data, filters, afterBuild hooks
├── _layouts/          # default.html, home.html, post.html, tags.html ...
├── _includes/         # header.html, footer.html, page-shell.html ...
├── _locales/          # en.toml, zh.toml (i18n strings)
└── assets/css/        # main.zcss → compiled to _site/assets/css/main.css
```

After a build, the theme's layouts are available for `layout` frontmatter, its includes for `{{ include name }}` / `{% include "name" %}`, and its ZCSS files compile into the output just like project assets.

## Theme Sources

The `source` field selects where the theme is loaded from:

| Source | Description |
|---|---|
| `local` (default) | The theme lives under `_themes/{name}/`; no network required |
| `git` | Clones a Git repository into `.zest/themes/{name}/` (cached between builds); `branch` defaults to `main`, `tag` overrides the branch |
| `url` | Downloads a ZIP archive (GitHub Releases, CDN, your own server) and extracts it; a single top-level folder is unwrapped automatically |
| `path` | References any local directory — handy when developing a theme alongside the site |

## `_theme.toml` — Theme Manifest

The declarative manifest replaces the legacy `_theme.zest.fsx` approach. Supported sections:

| Section | Purpose |
|---|---|
| `[theme]` (or `[meta]`) | Metadata: `name`, `version`, `description`, `author`, `license` |
| `[data]` | Arbitrary key/value pairs exposed as global template data |
| `[filters]` | Filter declarations (`name = "module::function"` spec) |
| `[[afterBuild]]` | Post-build commands (`cmd` + `args`) |

```toml
# _themes/oxygen/_theme.toml
name = "oxygen"
version = "2.4.0"
description = "A quiet, minimal blog theme."

[theme]
author = "Zest"
license = "MIT"

[data]
reading_time_label = "min read"
```

Loaded data is merged into the global data as `site.theme.*`, and top-level keys become template-accessible globals. A legacy `_theme.zest.fsx` init script, if present, still runs after the manifest and may register additional globals and filters.

## Merge and Override Rules

Project files win over theme files, mirroring the layouts/includes/assets pattern:

| Theme file | Behavior |
|---|---|
| `_layouts/*` | Project `_layouts/` overwrites; missing layouts fall back to the theme |
| `_includes/*` | Project `_includes/` overwrites; missing includes fall back to the theme |
| `_data/*` | Project `_data/` keys overwrite theme keys |
| `assets/*` | Theme assets are copied first; project assets overwrite on conflict |
| `_locales/*` | Locale strings; used by the `t` filter and `t_lang` helper |

Theme data is merged **before** the user's `_init.zest.fsx` runs, so user scripts can extend or override anything the theme declares.

## Parameters Priority

Theme parameters resolve with this priority (lowest to highest):

1. Theme `_data/params.toml` — defaults shipped by the theme.
2. Project `_data/params.toml` — site-level defaults.
3. `_config.toml [params]` — the highest priority, deep-merged so nested tables such as `[params.colors]` replace only the keys they specify.

Templates read the result as `{{ site.params.colors.accent }}` and DSL scripts as `site_data "params.colors.accent"`. The starter theme documents its supported keys (`accent`, `background`, `color`) in its config comments, so users override the palette from `_config.toml` instead of editing the theme.

## Creating a Theme

A minimal theme needs just a default layout:

```
_themes/my-theme/
└── _layouts/
    └── default.njk
```

Practical tips:

- Name layouts intuitively: `default`, `post`, `page`, `home`, and `tags` are conventional.
- Use generic include names (`header`, `footer`, `nav`) so users can override individual partials.
- Ship ZCSS files in `assets/css/` — they compile during the build automatically.
- Keep theme-level parameters under `[params]` so users can restyle the theme from `_config.toml`.

See [template-reference](/en/posts/template-reference/) for the layout context variables and routing rules that themes rely on.
