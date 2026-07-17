# CLI Reference

## Synopsis

```bash
zest [command] [options]
```

Run `zest` with no command (or `--help`) to print the command list. Global common options `--verbose`/`-v`, `--quiet`/`-q`, and `--help`/`-h` are accepted by every command.

| Command | Alias | Description |
|---|---|---|
| `build` | — | Build the static site to `output_dir` |
| `serve` | `dev` | Build + start dev server with live reload |
| `preview` | — | Serve already-built `_site/` (no build) |
| `init` | — | Scaffold a new project from the default template |
| `scaffold` | — | Scaffold a project from a preset template (`blog`/`docs`/`portfolio`/`empty`) |
| `migrate` | — | Convert an existing SSG project into a Zest project |

---

## `zest build`

```bash
zest build [path] [options]
```

| Argument | Description |
|---|---|
| `path` | Project directory (default: current directory) |

| Option | Description |
|---|---|
| `--watch`, `-w` | Watch for changes and auto-rebuild |
| `--verbose`, `-v` | Enable `Debug`-level logging |
| `--quiet`, `-q` | Suppress `Info`-level logs |
| `--help`, `-h` | Show build command help |

**Behavior:** load `_config.toml` → run `_init.fsx` → discover content files → evaluate `.zest.fsx`/`.fsx` via `dotnet fsi` (batch mode) → render `.md` → preprocess `.html`/`.njk` through the template engine → apply layouts → write `output_dir` → copy `assets/` (compiling `.zcss` → `.css`). With `--watch`, re-builds on changes to content/layouts/includes/data/assets.

---

## `zest serve` / `zest dev`

```bash
zest serve [options]
```

| Option | Description |
|---|---|
| `--port`, `-p PORT` | Dev server port (default: `dev_server_port` or `8080`) |
| `--host HOST` | Bind host (default: `localhost`) |
| `--open`, `-o` | Open browser on start |
| `--spa` | SPA mode: fall back to `index.html` for all routes |
| `--dir` | Enable directory listing |
| `--verbose`, `-v` / `--quiet`, `-q` | Log verbosity |

**Behavior:** full build → start HTTP server → start WebSocket live-reload server (`live_reload_port`, default `35729`) → watch files and push reload signals on change.

---

## `zest preview`

```bash
zest preview [options]
```

| Option | Description |
|---|---|
| `--port`, `-p PORT` | Preview port (default: `8080`) |
| `--host HOST` | Bind host (default: `localhost`) |
| `--open`, `-o` | Open browser on start |
| `--watch`, `-w` | Watch files and auto-rebuild |
| `--livereload`, `-l` | Enable live reload via WebSocket |
| `--spa` | SPA mode: fall back to `index.html` |
| `--dir` | Enable directory listing |
| `--verbose`, `-v` / `--quiet`, `-q` | Log verbosity |

Unlike `serve`, `preview` **does not** trigger a build — it serves whatever is already in `output_dir`. Add `--watch` or `--livereload` for live behavior.

---

## `zest init`

```bash
zest init [path]
```

| Argument | Description |
|---|---|
| `path` | Target directory (default: current directory) |

Copies the default project template into the target. If the target already contains files, prompts for confirmation, then prints next-step instructions.

---

## `zest scaffold`

```bash
zest scaffold <template> [path]
```

| Argument | Description |
|---|---|
| `<template>` | Preset: `blog`, `docs`, `portfolio`, `empty` |
| `[path]` | Target directory (default: current directory) |

Generates a standard Zest project (config, `_layouts/`, `content/`, `assets/`) from a bundled preset. Unknown templates fall back to a minimal structure. Prints next steps on success.

---

## `zest migrate`

```bash
zest migrate <source-ssg> [--from <dir>] [--to <dir>] [--dry-run]
```

| Argument | Description |
|---|---|
| `<source-ssg>` | Source system: `jekyll`, `hexo`, `hugo`, `eleventy` |

| Option | Description |
|---|---|
| `--from <dir>` | Source project directory (default: current directory) |
| `--to <dir>` | Target Zest directory (default: `<from>/_zest_migrated`) |
| `--dry-run` | Print the migration plan without writing files |

Scans the source SSG (config, layouts, content, static assets), converts frontmatter/configuration to Zest's TOML format, and emits a Zest project structure.

---

## Exit Codes

| Code | Meaning |
|---|---|
| `0` | Success |
| `1` | Error (parse failure, unknown command/option, etc.) |
