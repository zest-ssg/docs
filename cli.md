# CLI Reference

## Overview

```bash
zest [command] [options]
```

If no command is given, prints help.

| Command | Description |
|---|---|
| `build` | Build the static site |
| `serve` (alias: `dev`) | Build + start dev server with live reload |
| `preview` | Serve built `_site/` directory (no build) |
| `init` | Scaffold a new project from the default template |
| `--version` (`-v`) | Show version |
| `--help` (`-h`) | Show help |

---

## `zest build`

```bash
zest build [path] [options]
```

### Arguments

| Argument | Description |
|---|---|
| `path` | Project directory (default: current directory) |

### Options

| Option | Description |
|---|---|
| `--watch`, `-w` | Watch for changes and auto-rebuild |
| `--verbose`, `-v` | Enable Debug-level logging |
| `--quiet`, `-q` | Suppress Info-level logs |
| `--help`, `-h` | Show build command help |

### Behavior

1. Loads `_config.toml` (if present)
2. Executes `_init.zest.fsx` (if present)
3. Discovers all content files in the content directory
4. Evaluates `.zest.fsx`/`.fsx` scripts via `dotnet fsi` (batch mode for performance)
5. Renders `.md`/`.markdown` files through Markdown engine
6. Preprocesses `.html` files through Nunjucks if template syntax detected
7. Applies layouts and writes output to `_site/`
8. Copies `assets/` with `.zcss` → `.css` compilation

With `--watch`, Zest monitors the content, layouts, includes, data, and assets directories for changes and re-triggers the build automatically.

---

## `zest serve` / `zest dev`

```bash
zest serve [options]
```

### Options

| Option | Description |
|---|---|
| `--port`, `-p PORT` | Dev server port (default: config value or 8080) |
| `--host HOST` | Bind to host (default: `localhost`) |
| `--open`, `-o` | Open browser on start |
| `--spa` | SPA mode: fallback to `index.html` for all unmatched routes |
| `--dir` | Enable directory listing |
| `--verbose`, `-v` | Show detailed FSI output |
| `--quiet`, `-q` | Suppress Info-level logs |
| `--help`, `-h` | Show serve command help |

### Behavior

1. Performs a full build (same as `zest build`)
2. Starts an HTTP server on the configured port
3. Starts a WebSocket server on `live_reload_port` (35729)
4. Watches for file changes and:
   - Rebuilds changed pages
   - Pushes live-reload signal to connected browsers via WebSocket
5. Supports SPA routing when `--spa` is passed

---

## `zest preview`

```bash
zest preview [options]
```

### Options

| Option | Description |
|---|---|
| `--port`, `-p PORT` | Preview server port (default: 8080) |
| `--host HOST` | Bind to host (default: `localhost`) |
| `--open`, `-o` | Open browser on start |
| `--watch`, `-w` | Watch files and auto-rebuild |
| `--livereload`, `-l` | Enable live reload via WebSocket |
| `--spa` | SPA mode: fallback to `index.html` |
| `--dir` | Enable directory listing |
| `--verbose`, `-v` | Enable Debug-level logging |
| `--quiet`, `-q` | Suppress Info-level logs |
| `--help`, `-h` | Show preview command help |

### Behavior

Unlike `serve`, `preview` does **not** trigger a build. It serves whatever is already in `_site/`. Use `--watch` to add auto-rebuild, or `--livereload` for WebSocket-based live reload.

---

## `zest init`

```bash
zest init [path]
```

### Arguments

| Argument | Description |
|---|---|
| `path` | Target directory (default: current directory) |

### Behavior

1. Copies the default project template to the target directory
2. If target directory is not empty, prompts for confirmation
3. Prints next-step instructions

---

## Common Options

These work with any command that accepts `CommandOptions`:

| Option | Description |
|---|---|
| `--verbose`, `-v` | Enable verbose/debug output |
| `--quiet`, `-q` | Suppress informational output |
| `--help`, `-h` | Show command-specific help |

## Exit Codes

| Code | Meaning |
|---|---|
| `0` | Success |
| `1` | Error (build failure, parse error, etc.) |
