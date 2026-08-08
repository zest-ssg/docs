+++
title = "CLI Reference"
description = "Complete command-line reference for the zest tool — every command, option, exit code and a set of worked examples."
category = "reference"
tags = ["zest", "reference", "cli", "commands"]
date = 2026-08-01
+++

# CLI Reference

This page is the complete command reference for the `zest` tool. It complements the [CLI guide](/en/posts/cli/), which walks through the common workflows. Every command and option listed here was verified against the tool's argument parser.

## Common Options

| Option | Alias | Meaning |
|---|---|---|
| `--verbose` | `-v` | Enable Debug-level logging |
| `--quiet` | `-q` | Suppress Info-level logs |
| `--help` | `-h` | Show the command's help |

## Global

| Command | Description |
|---|---|
| `zest` | Print the help screen (returns `0`) |
| `zest --version` / `zest -v` | Print the version banner |
| `zest --help` / `zest -h` | Print the help screen |

## `zest build [path] [options]`

Build the site in `path` (default: current directory) into the output directory.

| Option | Alias | Meaning |
|---|---|---|
| `--watch` | `-w` | Watch for changes and rebuild automatically |
| `--verbose` | `-v` | Debug-level logging |
| `--quiet` | `-q` | Suppress Info logs |
| `--help` | `-h` | Show help |

## `zest serve` / `zest dev` [options]

Build the site and start the development server with live reload.

| Option | Alias | Meaning |
|---|---|---|
| `--port` | `-p` | Server port (default `8080`) |
| `--host` | | Bind host (default `localhost`) |
| `--open` | `-o` | Open the browser on start |
| `--spa` | | SPA mode: fall back to `index.html` for every route |
| `--dir` | | Enable directory listing |
| `--verbose` | `-v` | Show detailed FSI output |
| `--quiet` | `-q` | Suppress Info logs |
| `--help` | `-h` | Show help |

## `zest preview [options]`

Serve the already-built `_site/` directory without triggering a build.

| Option | Alias | Meaning |
|---|---|---|
| `--port` | `-p` | Server port (default `8080`) |
| `--host` | | Bind host (default `localhost`) |
| `--open` | `-o` | Open the browser on start |
| `--watch` | `-w` | Watch files and rebuild |
| `--livereload` | `-l` | Enable live reload over WebSocket |
| `--spa` | | SPA fallback |
| `--dir` | | Directory listing |
| `--verbose` | `-v` | Debug-level logging |
| `--quiet` | `-q` | Suppress Info logs |
| `--help` | `-h` | Show help |

## `zest init [path]`

Scaffold a new project from the bundled starter site (default path: current directory).

## `zest scaffold <template> [path]`

Generate a project from a preset. Templates: `blog` — the full bundled starter, shared with `init` — and `empty`, a config-only project with `content/`, `_layouts/` and `assets/`.

## `zest migrate <source-ssg> [options]`

Convert an existing SSG project to Zest. Sources: `jekyll`, `hexo`, `hugo`, `eleventy`.

| Option | Meaning |
|---|---|
| `--from <dir>` | Source directory (default: current directory) |
| `--to <dir>` | Target directory (default: `<from>\_zest_migrated`) |
| `--dry-run` | Print the migration plan without writing files |

## `zest convert-config <from> <to>`

Convert the site config between formats: `yaml`/`yml` to `toml`, or the reverse. Reads `_config.<ext>` (falling back to `config.<ext>`) and writes the converted file next to it.

## `zest clean [--cache] [--output]`

Clear build artifacts. Defaults to clearing both the `.zest/` cache and the output directory; pass `--cache` or `--output` to limit the cleanup.

## Exit Codes

| Code | Meaning |
|---|---|
| `0` | Success |
| `1` | Usage error, unknown command, or build failure |

## Examples

```bash
zest build                      # build the current directory
zest build --watch              # rebuild on every change
zest serve --port 4000 --open   # dev server on port 4000, open browser
zest preview --livereload       # serve the built site with live reload
zest scaffold blog my-blog      # new blog from the starter preset
zest migrate jekyll --from ./jekyll-site --dry-run
zest convert-config yaml toml   # _config.yml -> _config.toml
zest clean --cache              # drop only the build cache
```
