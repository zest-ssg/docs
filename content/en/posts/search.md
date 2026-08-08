+++
title = "Static Search"
description = "Build-time search indexes and client-side filtering — header live search and standalone search pages with no server required."
category = "features"
tags = ["zest", "search", "static", "index"]
date = 2026-08-01
+++

# Static Search

This page explains how search works in Zest sites. It covers the build-time index, the two common patterns — a header live search and a standalone search page — and the client-side filtering logic. It is part of the Features section; the templates that host the markup are described in the [template reference](/en/posts/template-reference/).

## How It Works

Zest provides no server-side search endpoint. Instead, a JSON index is produced during the build, embedded into the page and filtered entirely in the browser. The approach keeps the site fully static: no external search service, no database and no runtime cost.

## Injecting the Index

A layout injects the index with the `jsonBlock` DSL helper, which serializes data into a script block:

```fsharp
let docIndex =
    ctx.Pages
    |> Array.filter (fun p -> p.category <> "" && p.url.StartsWith("/" + langCode))
    |> Array.map (fun p ->
        {| url = p.url
           title = p.title
           group = t_lang ("sidebar." + p.category) langCode
           description = p.description |})

// in the <head>:
jsonBlock "__DOC_INDEX__" docIndex
```

Templates written in Nunjucks can build the same index with the `searchIndex` filter:

```njk
<script>
  window.__DOC_INDEX__ = {{ pages | searchIndex | dump | safe }};
</script>
```

The index is a plain array of objects; `url`, `title` and `description` are the fields the built-in client reads. Keep the index scoped (for example, documentation pages only) — it is embedded in every page that hosts the search widget, so size matters.

## The Client-Side Filter

The docs theme ships a small vanilla-JS matcher: the query is split into whitespace-separated terms, and a page is a hit when every term appears in its title, group or description. Results are escaped before insertion:

```javascript
const hits = index.filter(function (p) {
  const haystack = (p.title + ' ' + (p.group || '') + ' ' + (p.description || '')).toLowerCase();
  return terms.every(function (t) { return haystack.indexOf(t) !== -1; });
});
```

## A Standalone Search Page

For a dedicated search page, add a content file that renders an input and a results container, then attach the filter script:

```fsharp
// content/search.zest.fsx
render [
    h1 [ text "Search" ]
    divC "search-page" [
        voidElem "input" [ attr "type" "search"; attr "name" "q"; attr "class" "search-page__input" ]
        divC "search-results" []
    ]
    js """
        const index = window.__DOC_INDEX__ || [];
        const input = document.querySelector('.search-page__input');
        // Listen to the 'input' and 'search' events and render hits.
    """
]
```

The `search` event handles the browser's native clear button, and an empty query should clear the results container rather than show a spurious "no results" message.

## The search Filter

For build-time filtering inside templates, the `search` filter applies the same matching against the page collection:

```njk
{{ pages | search("pagination") }}
```

It searches title, content, excerpt and description, and returns every page when the query is empty.
