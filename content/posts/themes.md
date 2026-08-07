# Themes

Zest supports a lightweight theme system. A theme is a directory under `_themes/` (or fetched from a remote source) that provides layouts, includes, assets, ZCSS styles, and a `_theme.zest.fsx` initialization script. Theme files act as **fallbacks**: project-level files with the same name take priority.

---

## Quick Start

1. Add a theme directory under `_themes/`:

```
my-site/
├── _themes/
│   └── minima/
│       ├── _layouts/
│       │   ├── default.njk
│       │   └── post.njk
│       ├── _includes/
│       │   ├── header.njk
│       │   └── footer.njk
│       ├── assets/
│       │   └── css/
│       │       └── main.zcss
│       ├── _theme.zcss
│       ├── _theme.zest.fsx
│       └── theme.toml
├── _config.toml
└── content/
    └── hello.md
```

2. Enable the theme in `_config.toml`:

```toml
[theme]
name = "minima"
```

3. Build — the theme's `default.njk` is used as the default layout, `header.njk`/`footer.njk` are available as includes, and `main.zcss` is compiled into `_site/assets/css/main.css`.

---

## Theme Sources

Zest supports four theme sources, selected via the `source` field.

### Local (`source = "local"`, default)

The theme lives under `_themes/{name}/`. No internet required.

```toml
[theme]
name = "minima"
# source defaults to "local"
```

### Git (`source = "git"`)

Clones the repository into `.zest/themes/{name}/`. The clone is cached — subsequent builds skip the clone step unless you delete `.zest/themes/`.

```toml
[theme]
name = "minima"
source = "git"
git = "https://github.com/zest-ssg/zest-theme-minima.git"
branch = "main"   # optional, defaults to "main"
# tag = "v2.1.0"  # optional, overrides branch
```

### URL (`source = "url"`)

Downloads a ZIP archive and extracts it into `.zest/themes/{name}/`. Supports any public HTTP URL (GitHub Releases, CDN, packagist, your own server).

```toml
[theme]
name = "minima"
source = "url"
url = "https://github.com/zest-ssg/zest-theme-minima/archive/refs/tags/v2.1.0.zip"
```

If the ZIP contains a single top-level folder, it is automatically unwrapped so the cache directory is the actual theme root.

### Path (`source = "path"`)

References any local directory. Useful for developing a theme alongside your site.

```toml
[theme]
name = "minima"
source = "path"
path = "../zest-theme-minima"
```

---

## File Override Rules

| Theme file | Behaviour |
|---|---|
| `_layouts/*` | Project `_layouts/` overwrites. Missing layouts fall back to theme. |
| `_includes/*` | Project `_includes/` overwrites. Missing includes fall back to theme. |
| `assets/*` | Copied first; project `assets/` files overwrite on conflict. |
| `_theme.zcss` | Currently not auto-loaded (use `_theme.zest.fsx` or `_init.zest.fsx` to `@use` manually). |
| `_theme.zest.fsx` | Executed **before** `_init.zest.fsx`. Filters/globals registered here can be extended or overridden by the user script. |
| `theme.toml` | Theme metadata — informational only. |

---

## `_theme.zest.fsx` — Theme Init Script

The theme's init script uses the same API as `_init.zest.fsx` (see [dsl-guide.md](dsl-guide.md)). It runs before the user's `_init.zest.fsx`, so the user can extend or override anything the theme declares.

**Common use cases:**

```fsharp
// _themes/minima/_theme.zest.fsx

// Register a custom Nunjucks filter
addFilter "excerpt" "truncate(200) | striptags"

// Add global data accessible in templates as {{ site.total_pages }}
addGlobal "total_pages" (pages |> Array.length |> box)

// Load external JSON data
let nav = loadJson "data/nav.json"
addGlobal "navigation" nav

// Declare a lazy value — nullary F# values work as globals
addGlobalFunction "current_year" (box (System.DateTime.Now.Year))
```

---

## `theme.toml` — Theme Metadata

Informational. Not consumed by the build system, but useful for theme authors and tooling.

```toml
# theme.toml
name = "minima"
version = "2.1.0"
description = "A clean, minimal blog theme for Zest"
author = "Zest Contributors"
license = "MIT"
homepage = "https://github.com/zest-ssg/zest-theme-minima"
```

---

## Creating a Theme

A minimal theme needs only one file — a default layout:

```
_themes/my-theme/
└── _layouts/
    └── default.njk
```

A full-featured theme would include:

```
_themes/my-theme/
├── _layouts/
│   ├── default.njk
│   ├── post.njk
│   └── page.njk
├── _includes/
│   ├── header.njk
│   ├── footer.njk
│   └── nav.njk
├── assets/
│   ├── css/
│   │   └── main.zcss
│   └── js/
│       └── main.js
├── _theme.zest.fsx
└── theme.toml
```

### Tips

- Use `_theme.zest.fsx` to register filters and helpers that templates expect — this keeps the user's `_config.toml` clean.
- Name layouts intuitively: `default`, `post`, `page`, `home` are standard.
- Theme includes should use generic names (`header`, `footer`, `nav`) so users can override individual partials without touching the theme.
- If your theme uses ZCSS, ship the `.zcss` files in `assets/css/` — they compile during the build automatically.

---

## FAQ

**Q: Can I use multiple themes?**  
A theme is one directory. To combine multiple themes, create your own `_themes/combined/` that merges what you need, or use `_init.zest.fsx` to load data from external sources.

**Q: How do I override a single layout from the theme?**  
Create a file with the same name in your project's `_layouts/`. For example, if the theme has `_layouts/post.njk`, create `_layouts/post.njk` in your project — it will be used instead.

**Q: How do I tell which theme a layout came from?**  
Run `zest serve --verbose` — the build log prints the resolved theme directory path.

**Q: Can I use the theme with `compat` mode?**  
Yes. The `[compat]` flags and `[template]` settings in your `_config.toml` control engine behaviour regardless of the theme source.
