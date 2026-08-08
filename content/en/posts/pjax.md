+++
title = "PJAX Navigation"
description = "Progressively enhanced navigation: Zest injects a self-contained PJAX script that swaps only the content region on link clicks."
category = "features"
tags = ["zest", "pjax", "navigation", "javascript"]
date = 2026-08-01
+++

# PJAX Navigation

This page describes Zest's PJAX support: how to enable it, what the injected script does, and how to keep page-specific JavaScript working after a swap. It is part of the Features section; the layout structure it relies on is described in the [template reference](/en/posts/template-reference/).

## Enabling PJAX

The script is a site-wide preset exposed as `pjaxScript` in the template context. Inject it once in your base layout, just before the closing `</body>`:

```njk
{{ pjaxScript | safe }}
```

or, from an F# layout:

```fsharp
raw (siteValue "pjaxScript")
```

The script guards itself with a `window.__zestPjaxLoaded` flag, so double injection through a nested layout is harmless. There is no configuration switch — the preset is always available and opt-in by injection.

## How It Works

The script intercepts clicks on same-origin links, fetches the target page with an `X-PJAX` header, and swaps only the content region:

- **Target region** — the first of `main`, `#content` or `body`, so a theme can choose its own container.
- **Interception rules** — modifier-key clicks, `mailto:`, `tel:` and `javascript:` hrefs, `download` attributes, `target="_blank"` links and cross-origin links fall through to normal navigation.
- **Caching and prefetch** — responses are kept in an in-memory cache (24 entries, oldest evicted) and prefetched on hover after a 150 ms delay, so clicks usually resolve instantly.
- **Timeouts** — a request aborts after 8 seconds via `AbortController` and falls back to a full navigation.
- **History** — each visit uses `history.pushState`; the back and forward buttons work through `popstate`, and scroll positions are remembered per page.
- **Accessibility** — `prefers-reduced-motion` disables the fade transition, and hash links scroll to their anchors without a fetch.

The document `title` is updated from the fetched page and the URL changes to the target, so shareable URLs and the back button keep working.

## Structuring the Layout

Give the swappable region a stable selector so the script always finds it:

```html
<main class="content" id="content">{{ content }}</main>
```

The fetched document must be a complete HTML page — the script parses it with `DOMParser` and copies the matching region from it — so layout files should keep the content region structurally identical across pages.

## Re-Initializing Page Scripts

Code that ran on the initial page load does not automatically re-run after a swap. The script dispatches a `pjax:end` event after every successful navigation, with `event.detail.url` holding the new URL. Listen for it and re-initialize:

```javascript
document.addEventListener('pjax:end', function () {
  initCodeBlocks();
  initMenu();
});
```

Because a listener registered this way is never removed, guard the binding with a window flag so it is attached only once:

```javascript
if (!window.__pjaxBound) {
  window.__pjaxBound = true;
  document.addEventListener('pjax:end', init);
}
```

A common failure mode is duplicate global handlers after several swaps; the guard above keeps single-page listeners while letting per-page logic re-run.
