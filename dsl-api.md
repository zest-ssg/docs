# DSL API Reference

This document catalogs every public function, type, and module in the **Zest.Dsl** library. All of these are available inside `.zest.fsx` scripts.

---

## Module: `Dsl` (Core HTML Builders)

Automatically opened in scripts. All functions return `string` typed HTML.

### Text & Encoding

| Function | Signature | Description |
|---|---|---|
| `htmlEncode` | `string -> string` | Encode `&`, `<`, `>`, `"` to HTML entities |
| `text` | `string -> string` | HTML-encoded text content |
| `raw` | `string -> string` | Raw HTML (not encoded) |

### Element Builders

| Function | Signature | Description |
|---|---|---|
| `elem` | `tag: string -> attrs: string list -> children: string list -> string` | Generic element with opening/closing tags |
| `voidElem` | `tag: string -> attrs: string list -> string` | Generic void (self-closing) element |

### Attribute Builder

| Function | Signature | Description |
|---|---|---|
| `attr` | `k: string -> v: string -> string` | HTML attribute as `key="value"` (value HTML-encoded) |

### Inline Elements

| Function | Signature |
|---|---|
| `a` | `url: string -> children: string list -> string` |
| `span` | `children: string list -> string` |
| `code` | `children: string list -> string` |
| `strong` | `children: string list -> string` |
| `em` | `children: string list -> string` |
| `small` | `children: string list -> string` |
| `mark` | `children: string list -> string` |
| `del` | `children: string list -> string` |
| `abbr` | `title: string -> children: string list -> string` |

### Void Elements

| Function | Signature |
|---|---|
| `img` | `src: string -> alt: string -> string` |
| `br` | `unit -> string` |
| `hr` | `unit -> string` |

### Block Elements

| Function | Signature | Function | Signature |
|---|---|---|---|
| `h1`-`h6` | `children: string list -> string` | `div` | `children: string list -> string` |
| `p` | `children: string list -> string` | `section` | `children: string list -> string` |
| `article` | `children: string list -> string` | `nav` | `children: string list -> string` |
| `header` | `children: string list -> string` | `footer` | `children: string list -> string` |
| `main` | `children: string list -> string` | `ul` | `children: string list -> string` |
| `ol` | `children: string list -> string` | `li` | `children: string list -> string` |
| `blockquote` | `children: string list -> string` | `pre` | `children: string list -> string` |

### Table Elements

| Function | Signature |
|---|---|
| `table` | `children: string list -> string` |
| `thead` | `children: string list -> string` |
| `tbody` | `children: string list -> string` |
| `tr` | `children: string list -> string` |
| `th` | `children: string list -> string` |
| `td` | `children: string list -> string` |

### Document Structure

| Function | Signature | Description |
|---|---|---|
| `doctype` | `string` | `"<!DOCTYPE html>"` |
| `html` | `children: string list -> string` | |
| `head` | `children: string list -> string` | |
| `body` | `children: string list -> string` | |
| `title` | `children: string list -> string` | |
| `meta` | `attrs: string list -> string` | |
| `link` | `rel: string -> href: string -> string` | |
| `stylesheet` | `href: string -> string` | Shortcut: `<link rel="stylesheet">` |
| `script` | `src: string -> string` | |
| `scriptInline` | `code: string -> string` | |
| `style` | `css: string -> string` | |
| `styleZcss` | `compiledCss: string -> string` | Shorthand for ZCSS style tag |

### Class-Shortcut Element Builders

Append a CSS class to standard elements. Pattern: `{tag}C`:

| Function | Signature |
|---|---|
| `divC` | `cls: string -> children: string list -> string` |
| `pC` | `cls: string -> children: string list -> string` |
| `spanC` | `cls: string -> children: string list -> string` |
| `sectionC` | `cls: string -> children: string list -> string` |
| `ulC` | `cls: string -> children: string list -> string` |
| `olC` | `cls: string -> children: string list -> string` |
| `liC` | `cls: string -> children: string list -> string` |
| `navC` | `cls: string -> children: string list -> string` |
| `headerC` | `cls: string -> children: string list -> string` |
| `footerC` | `cls: string -> children: string list -> string` |
| `mainC` | `cls: string -> children: string list -> string` |
| `articleC` | `cls: string -> children: string list -> string` |
| `asideC` | `cls: string -> children: string list -> string` |
| `h1C`-`h3C` | `cls: string -> children: string list -> string` |
| `blockquoteC` | `cls: string -> children: string list -> string` |
| `preC` | `cls: string -> children: string list -> string` |
| `codeC` | `cls: string -> children: string list -> string` |
| `tableC` | `cls: string -> children: string list -> string` |
| `imgC` | `cls: string -> src: string -> alt: string -> string` |
| `codeBlock` | `lang: string -> code: string -> string` |

### Link Shortcuts

| Function | Signature | Description |
|---|---|---|
| `aBlank` | `url: string -> text: string -> string` | Opens in new tab (`target="_blank"`) |
| `aHref` | `url: string -> text: string -> string` | Simple text link |
| `aC` | `cls: string -> url: string -> children: string list -> string` | Link with class |

### Conditional Helpers

| Function | Signature | Description |
|---|---|---|
| `showIf` | `cond: bool -> content: string -> string` | Returns content only if `cond` is true |
| `hideIf` | `cond: bool -> content: string -> string` | Returns content only if `cond` is false |
| `render` | `nodes: string list -> unit` | Prints nodes joined by newlines to stdout |

---

## Module: `DslComponents` (Form & Layout Components)

```fsharp
open DslComponents
```

### Form Elements

| Function | Signature |
|---|---|
| `form` | `action: string -> children: string list -> string` |
| `input` | `typ: string -> name: string -> value: string -> string` |
| `button` | `children: string list -> string` |
| `textarea` | `name: string -> children: string list -> string` |
| `select` | `name: string -> children: string list -> string` |
| `option` | `value: string -> children: string list -> string` |
| `label` | `forVal: string -> children: string list -> string` |

With class variants: `formC`, `buttonC`, `labelC`.

### Layout Components

| Function | Signature | Description |
|---|---|---|
| `container` | `children: string list -> string` | `div.container` |
| `row` | `children: string list -> string` | `div.row` |
| `col` | `children: string list -> string` | `div.col` |
| `card` | `children: string list -> string` | `div.card` |
| `badge` | `text: string -> string` | `span.badge` |

### Alert Components

| Function | Signature |
|---|---|
| `alert` | `level: string -> children: string list -> string` |
| `alertInfo` | `children: string list -> string` |
| `alertSuccess` | `children: string list -> string` |
| `alertWarning` | `children: string list -> string` |
| `alertDanger` | `children: string list -> string` |

### Button Variants

| Function | Signature |
|---|---|
| `btnPrimary` | `children: string list -> string` |
| `btnSecondary` | `children: string list -> string` |
| `btnSuccess` | `children: string list -> string` |
| `btnDanger` | `children: string list -> string` |

### Media

| Function | Signature |
|---|---|
| `figure` | `src: string -> alt: string -> caption: string -> string` |
| `details` | `summary: string -> children: string list -> string` |

### Utility

| Function | Signature |
|---|---|
| `each` | `items: 'a list -> f: ('a -> string) -> string` |
| `joinWith` | `sep: string -> items: string list -> string` |
| `opt` | `v: string option -> string` |
| `renderIf` | `cond: bool -> node: string -> fallback: string -> string` |
| `renderOpt` | `v: 'a option -> f: ('a -> string) -> string` |

---

## Module: `DslSugar` (Conditionals, Loops, Pipelines)

```fsharp
open DslSugar
```

### Yield Helpers

| Function | Signature | Description |
|---|---|---|
| `yield_block` | `nodes: string list -> string` | Join with `\n` |
| `yield_inline` | `nodes: string list -> string` | Join with `""` |

### Conditionals

| Function | Signature |
|---|---|
| `cond` | `condition: bool -> ifTrue: string -> ifFalse: string -> string` |
| `default_to` | `fallback: string -> value: string -> string` |
| `coalesce_str` | `values: string list -> string` |
| `when_true` | `cond: bool -> content: string -> string` |
| `unless_true` | `cond: bool -> content: string -> string` |
| `switch_value` | `value: string -> cases: (string * string) list -> defaultCase: string -> string` |
| `match_cond` | `cases: (bool * string) list -> fallback: string -> string` |

### Loops

| Function | Signature |
|---|---|
| `each_with` | `items: 'a list -> separator: string -> f: ('a -> string) -> string` |
| `each_line` | `items: 'a list -> f: ('a -> string) -> string` |
| `each_in_container` | `tag: string -> items: 'a list -> f: ('a -> string) -> string` |
| `repeat_str` | `count: int -> s: string -> string` |
| `numbered_list` | `items: 'a list -> f: (int -> 'a -> string) -> string` |
| `for_range` | `start: int -> endInclusive: int -> f: (int -> string) -> string` |

### Pipelines

| Function | Signature |
|---|---|
| `\|>` | Forward pipe |
| `<\|` | Backward pipe |
| `>>` | Function composition |
| `wrap_in` | `tag: string -> content: string -> string` |
| `add_class` | `cls: string -> element: string -> string` |

### Shorthand Builders

| Function | Signature |
|---|---|
| `div_text` | `cls: string -> content: string -> string` |
| `span_text` | `cls: string -> content: string -> string` |
| `p_text` | `content: string -> string` |
| `h_text` | `level: int -> content: string -> string` |
| `a_text` | `url: string -> textContent: string -> string` |
| `a_text_c` | `cls: string -> url: string -> textContent: string -> string` |
| `img_c` | `cls: string -> src: string -> alt: string -> string` |
| `ul_from` | `items: 'a list -> f: ('a -> string) -> string` |
| `ol_from` | `items: 'a list -> f: ('a -> string) -> string` |

### Type Conversion

| Function | Signature |
|---|---|
| `str` | `'a -> string` |
| `int_str` | `int -> string` |
| `float_str` | `format: string -> float -> string` |
| `bool_str` | `bool -> string` |

---

## Module: `DslUtilities` (Control Flow & String Interpolation)

```fsharp
open DslUtilities
```

### Inline Markdown

| Function | Signature | Description |
|---|---|---|
| `md` | `markdown: string -> string` | Render a Markdown string to an HTML `string`, for mixing Markdown prose directly into the DSL tree |

`md` returns a plain `string` — identical in kind to every other DSL builder — so it drops straight into a `render [ ... ]` block (or any `string list`). It delegates to `Zest.Engine.Html.MarkdownEngine.toHtml`.

```fsharp
render [
    divC "about" [
        md """
# About

This is a **native template** page written in `.zest.fsx`, where Markdown
and the F# HTML DSL live side by side.
"""
    ]
]
```

### Control Flow

| Function | Signature | Description |
|---|---|---|
| `switch_str` | `string -> (string * string) list -> defaultCase: string -> string` | Match on string value |
| `cond_str` | `(bool * string) list -> fallback: string -> string` | First matching boolean condition |
| `chain_cond` | `(bool * string) list -> fallback: string -> string` | Alias for cond_str |
| `choose` | `bool -> string -> string -> string` | Ternary (if-then-else) |

### String Interpolation

| Function | Signature | Description |
|---|---|---|
| `interp` | `template: string -> vars: (string * string) list -> string` | Replace `{key}` placeholders (raw values) |
| `interp_safe` | `template: string -> vars: (string * string) list -> string` | Replace `{key}` placeholders (HTML-encoded values) |

### Collection Helpers

| Function | Signature |
|---|---|
| `take_n` | `int -> string list -> string list` |
| `skip_n` | `int -> string list -> string list` |
| `filter_by` | `(string -> bool) -> string list -> string list` |
| `map_by` | `(string -> string) -> string list -> string list` |
| `group_by` | `(string -> string) -> string list -> (string * string list) list` |
| `chunk` | `int -> string list -> string list list` |
| `intersperse_str` | `string -> string list -> string list` |
| `zip_lists` | `string list -> string list -> (string * string) list` |

### Math Helpers

| Function | Signature |
|---|---|
| `sum` | `int list -> int` |
| `avg` | `int list -> int` |
| `min_val` | `int list -> int` |
| `max_val` | `int list -> int` |

---

## Module: `DslCollections` (Page Query API)

```fsharp
open DslCollections
```

| Function | Signature | Description |
|---|---|---|
| `site_pages` | `unit -> {| url, title, date, slug, description, tags |}[]` | All pages across the site |
| `recent_pages` | `n: int -> {| ... |}[]` | N most recent pages (by date descending) |
| `pages_by_tag` | `tag: string -> {| ... |}[]` | Pages with a specific tag |
| `pages_by_dir` | `dir: string -> {| ... |}[]` | Pages in a URL directory segment |
| `pages_by_collection` | `col: string -> {| ... |}[]` | Pages in a collection (first URL segment) |
| `all_tags` | `unit -> string[]` | All unique tags, sorted |
| `all_collections` | `unit -> string[]` | All unique collection names, sorted |
| `search_pages` | `query: string -> {| ... |}[]` | Case-insensitive title search |
| `page_count` | `unit -> int` | Total page count |
| `sort_pages_by` | `field: string -> direction: string -> {| ... |}[]` | Sort by `"title"`, `"date"`, `"slug"` |
| `filter_pages_by` | `pred: ({| ... |} -> bool) -> {| ... |}[]` | Custom filter predicate |
| `group_pages_by_year` | `unit -> (string * {| ... |} list) list` | Group by publication year |
| `related_pages` | `page_url: string -> count: int -> {| ... |}[]` | Related pages by shared tags |
| `tag_cloud` | `min_count: int -> (string * int) list` | Tag frequency pairs |
| `include_partial` | `name: string -> string` | Render an include by name |
| `site_data` | `key: string -> string` | Look up global site data value |
| `site_section` | `prefix: string -> IDictionary<string, string>` | Get data keys under a prefix |

---

## Module: `DslSeo` (SEO Meta Tags)

```fsharp
open DslSeo
```

| Function | Signature | Description |
|---|---|---|
| `meta_tags` | `title: string -> description: string -> url: string -> image: string -> siteName: string -> string list` | Charset, viewport, title, description, canonical, image meta tags |
| `open_graph_tags` | `title: string -> description: string -> url: string -> image: string -> ogType: string -> string list` | `og:title`, `og:description`, `og:url`, `og:type`, `og:image`, `og:image:alt` |
| `twitter_card_tags` | `cardType: string -> title: string -> description: string -> image: string -> site: string -> string list` | `twitter:card`, `twitter:title`, `twitter:description`, `twitter:image`, `twitter:site` |
| `canonical_url` | `url: string -> string` | `<link rel="canonical">` tag |
| `hreflang_tag` | `lang: string -> url: string -> string` | `<link rel="alternate" hreflang="...">` tag |

---

## Module: `DslXml` (Feed Generation)

```fsharp
open DslXml
```

| Function | Signature | Description |
|---|---|---|
| `rss_xml` | `siteTitle: string -> siteUrl: string -> siteDescription: string -> pages: {| url, title, date, description |}[] -> string` | RSS 2.0 feed XML |
| `atom_xml` | `siteTitle: string -> siteUrl: string -> siteDescription: string -> authorName: string -> pages: {| url, title, date, description |}[] -> string` | Atom 1.0 feed XML |
| `sitemap_xml` | `baseUrl: string -> pages: {| url, date, priority: float |}[] -> string` | Sitemap XML |

---

## Module: `DslStyle` (Style Integration)

```fsharp
open DslStyle
```

| Function | Signature | Description |
|---|---|---|
| `styleZcss` | `compiledCss: string -> string` | Wrap ZCSS output in `<style>` tag |
| `styleFromZcss` | `cssRules: CssRule list -> string` | Compile `CssRule` list to `<style>` (pretty) |
| `styleFromZcssMinified` | `cssRules: CssRule list -> string` | Compile `CssRule` list to `<style>` (minified) |
| `styleCss` | `css: string -> string` | Raw CSS in `<style>` with optional validation |
| `styleScoped` | `css: string -> string * string` | Scoped CSS with auto-generated scope attribute. Returns `(styleTag, scopeAttr)` |
| `styleScopedOnly` | `css: string -> string` | Scoped `<style>` tag only (no scope attribute) |
| `inlineZcss` | `compiledCss: string -> string` | Alias for `styleZcss` |
| `styleExternal` | `zcssPath: string -> string` | `<link rel="stylesheet">` for `.zcss` file (auto `.zcss` → `.css`) |
| `styleInlineExternal` | `zcssPath: string -> string` | Load and inline a `.zcss` file |
| `criticalCss` | `fullCss: string -> criticalSelectors: string list -> string` | Extract above-the-fold CSS by selector matching |

---

## Module: `StringHelper`

```fsharp
open StringHelper
```

| Function | Signature | Description |
|---|---|---|
| `slugify` | `string -> string` | URL-safe slug with diacritic normalization |
| `truncate` | `maxLen: int -> string -> string` | Truncate with ellipsis |
| `strip_html` | `string -> string` | Remove all HTML tags |
| `reading_time` | `string -> int` | Estimate reading time (200 wpm) |
| `word_count` | `string -> int` | Count words |
| `excerpt` | `maxLen: int -> html: string -> string` | Strip HTML then truncate |
| `capitalize` | `string -> string` | Capitalize first character |
| `title_case` | `string -> string` | Convert to Title Case |
| `default_value` | `fallback: string -> value: string -> string` | Return fallback if value is null/empty |
| `coalesce` | `values: string list -> string` | Return first non-null/non-empty |

---

## Module: `DateHelper`

```fsharp
open DateHelper
```

| Function | Signature | Description |
|---|---|---|
| `format_date` | `dateStr: string -> string` | Format to `yyyy-MM-dd` |
| `format_date_custom` | `dateStr: string -> fmt: string -> string` | Format with custom format string |
| `format_date_iso` | `dateStr: string -> string` | Format to ISO 8601 |
| `format_date_rfc` | `dateStr: string -> string` | Format to RFC 2822 |
| `date_add_days` | `dateStr: string -> days: int -> string` | Add days |
| `date_diff` | `date1: string -> date2: string -> int` | Days between dates |
| `now` | `unit -> string` | Current date as `yyyy-MM-dd` |
| `current_year` | `unit -> string` | Current year |
| `url_encode` | `string -> string` | URL encode |
| `url_decode` | `string -> string` | URL decode |

---

## Module: `SequenceHelper`

```fsharp
open SequenceHelper
```

| Function | Signature |
|---|---|
| `kv` | `string -> obj -> string * obj` |
| `kv_list` | `(string * obj) list -> (string * obj) list` |
| `kv_get` | `string -> (string * obj) list -> obj option` |
| `range` | `int -> int -> string list` |
| `repeat` | `int -> string -> string` |
| `url_join` | `string -> string -> string` |
| `is_absolute_url` | `string -> bool` |
| `to_string` | `obj -> string` |
| `to_int` | `int -> string -> int` |
| `to_bool` | `bool -> string -> bool` |
| `ternary` | `bool -> 'T -> 'T -> 'T` |
| `as_option` | `string -> string option` |
| `apply_when` | `bool -> ('T -> 'T) -> 'T -> 'T` |
| `apply_unless` | `bool -> ('T -> 'T) -> 'T -> 'T` |
| `try_with` | `(unit -> 'T) -> 'T -> 'T` |
| `tap` | `('T -> unit) -> 'T -> 'T` |
| `(=>)` | `string -> obj -> (string * obj)` |
| `dict_of` | `(string * obj) list -> IDictionary<string, obj>` |
| `take_while` | `('a -> bool) -> 'a list -> 'a list` |
| `skip_while` | `('a -> bool) -> 'a list -> 'a list` |
| `partition` | `('a -> bool) -> 'a list -> 'a list * 'a list` |
| `sort_by` | `('a -> 'b) -> 'a list -> 'a list` |
| `sort_by_desc` | `('a -> 'b) -> 'a list -> 'a list` |
| `dedup` | `'a list -> 'a list` |
| `flat_map` | `('a -> 'b list) -> 'a list -> 'b list` |

---

## Module: `ContentGuard`

```fsharp
open ContentGuard
```

| Function | Signature | Description |
|---|---|---|
| `guard` | `value: string -> render: (string -> string) -> fallback: string -> string` | Guard against null/empty value |
| `guard_opt` | `value: string option -> render: (string -> string) -> fallback: string -> string` | Guard for option values |
| `guard_list` | `items: 'a list -> render: ('a list -> string) -> fallback: string -> string` | Guard against empty list |
| `validate` | `condition: bool -> message: string -> content: string -> string` | Emit HTML comment error if condition fails |
| `require` | `fieldName: string -> value: string -> string` | Emit error comment if field missing |
| `warn_if` | `condition: bool -> message: string -> content: string -> string` | Emit HTML comment warning |
| `debug` | `format: string -> [<ParamArray>] args: obj[] -> unit` | Print debug message to stderr |
| `trace` | `label: string -> value: 'a -> 'a` | Trace value (returns it unchanged) |

---

## Module: `CompoundBuilder`

```fsharp
open CompoundBuilder
```

| Function | Signature |
|---|---|
| `media_object` | `imgSrc: string -> imgAlt: string -> title: string -> desc: string -> string` |
| `card_component` | `title: string -> body: string -> linkUrl: string -> linkText: string -> string` |
| `hero_section` | `title: string -> subtitle: string -> ctaUrl: string -> ctaText: string -> string` |
| `card_grid` | `items: 'a list -> cardFn: ('a -> string) -> string` |

---

## Module: `CssValidator`

```fsharp
open CssValidator
```

| Type / Function | Description |
|---|---|
| `CssValidationLevel` | `Strict`, `Warn`, `Off` |
| `setLevel` | `CssValidationLevel -> unit` — set global validation behavior |
| `getLevel` | `unit -> CssValidationLevel` |
| `validate` | `string -> string` — validate and possibly annotate CSS |
| `validateDetailed` | `string -> CssValidationIssue list` — detailed issue list |
| `checkBrackets` | `string -> bool` — check curly bracket balance |

---

## Module: `CssScoper`

```fsharp
open CssScoper
```

| Function | Signature | Description |
|---|---|---|
| `generateScope` | `unit -> string` | Generate unique 8-char scope ID |
| `applyScope` | `scopeAttr: string -> css: string -> string` | Prepend scope attribute to all selectors |
| `applyScopeWithAttr` | `scopeId: string -> css: string -> string * string` | Scope CSS + return attribute name |
| `scopedStyleBlock` | `css: string -> string * string` | Generate scoped `<style>` + scope attribute |
