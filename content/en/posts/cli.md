+++
title = "Command Line"
description = "The day-to-day Zest commands: build, serve, preview, init, scaffold and migrate."
category = "guides"
tags = ["zest", "cli", "commands"]
date = 2026-08-01
+++

# Command Line

This guide covers the everyday Zest commands and their main options. The exact option names, arguments, and exit codes are verified against the CLI source and collected in the [CLI reference](/en/posts/cli-reference/).

## Synopsis

```bash
zest [command] [options]
```

Run `zest` with no command (or `--help`) to print the command list. Global options `--verbose`/`-v`, `--quiet`/`-q`, and `--help`/`-h` are accepted by every command.

| Command | Alias | Description |
|---|---|---|
| `build` | — | Build the static site into the output directory |
| `serve` | `dev` | Build + start a dev server with live reload |
| `preview` | — | Serve the already-built `_site/` (no build) |
| `init` | — | Scaffold a new project from the bundled starter site |
| `scaffold` | — | Scaffold a project from a preset (`blog` or `empty`) |
| `migrate` | — | Convert an existing SSG project into a Zest project |
| `clean` | — | Clear build cache and/or output |
| `convert-config` | — | Convert `_config` between YAML and TOML |

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
| `--help`, `-h` | Show build help |

The build loads `_config.toml`, runs `_init.zest.fsx`, discovers content, evaluates `.zest.fsx` scripts via `dotnet fsi`, renders Markdown and template files, applies layouts, copies assets (compiling `.zcss` to `.css`), and writes the result to the output directory.

## `zest serve` / `zest dev`

```bash
zest serve [options]
```

| Option | Description |
|---|---|
| `--port`, `-p PORT` | Dev server port (default: `dev_server_port` or 8080) |
| `--host HOST` | Bind host (default: `localhost`) |
| `--open`, `-o` | Open the browser on start |
| `--spa` | SPA mode: fall back to `index.html` for all routes |
| `--dir` | Enable directory listing |
| `--verbose`, `-v` / `--quiet`, `-q` | Log verbosity |

`serve` runs a full build, starts the HTTP server, starts the WebSocket live-reload server (default port 35729), and watches files so edits push a reload signal immediately.

## `zest preview`

```bash
zest preview [options]
```

| Option | Description |
|---|---|
| `--port`, `-p PORT` | Preview port (default: 8080) |
| `--host HOST` | Bind host (default: `localhost`) |
| `--open`, `-o` | Open the browser on start |
| `--watch`, `-w` | Watch files and auto-rebuild |
| `--livereload`, `-l` | Enable live reload over WebSocket |
| `--spa` / `--dir` | SPA fallback / directory listing |

Unlike `serve`, `preview` **does not trigger a build** — it serves whatever is already in the output directory. Add `--watch` or `--livereload` for live behavior.

## `zest init`

```bash
zest init [path]
```

Copies the bundled starter site into the target directory (default: current directory). If the current directory is not empty, it asks for confirmation, then prints next-step instructions.

## `zest scaffold`

```bash
zest scaffold <template> [path]
```

Generates a project from a preset:

- `blog` — the full bundled starter site (the same one `zest init` uses).
- `empty` — a minimal config-only project (`_config.toml` + `_init.zest.fsx`).

An unknown template prints an error listing the valid presets.

## `zest migrate`

```bash
zest migrate <source-ssg> [--from <dir>] [--to <dir>] [--dry-run]
```

Converts an existing SSG project into a Zest project. The source system is one of `jekyll`, `hexo`, `hugo`, or `eleventy`. It scans the source project, converts YAML frontmatter to TOML, generates a `_config.toml` with the matching `[compat]` flag enabled, and emits the Zest project structure under `--to` (default `<from>/_zest_migrated`). Use `--dry-run` to print the migration plan without writing files.

## Exit Codes

| Code | Meaning |
|---|---|
| `0` | Success |
| `1` | Error (parse failure, unknown command/option, aborted init, failed build) |

For a complete option reference including `clean` and `convert-config`, see the [CLI reference](/en/posts/cli-reference/).
