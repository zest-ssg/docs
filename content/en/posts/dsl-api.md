+++
title = "DSL API Reference"
description = "The public API of Zest.Dsl: DslHtml element builders, DslComponents, helpers, DslSeo and DslXml."
category = "dsl"
tags = ["zest", "dsl", "api", "fsharp"]
date = 2026-08-01
+++

# DSL API Reference

This page catalogs the public surface of the **Zest.Dsl** library. Every function below is available inside `.zest.fsx` scripts; the module list in the left sidebar's **Zest DSL** group is completed by the [guide](/en/posts/dsl-guide/) and [DslCollections](/en/posts/dsl-collections/). All functions return `string`-typed HTML.

## `Dsl` — Core HTML Builders

Auto-opened in scripts.

**Text & encoding:** `text` (HTML-encoded content), `raw` (verbatim HTML), `htmlEncode`.

**Generic builders:** `elem tag attrs children`, `voidElem tag attrs`, `attr key value`, and the general-purpose `el tag pairs children` / `elVoid tag pairs` / `attrsOf pairs`.

**Element builders** — `h1`–`h6`, `p`, `div`, `section`, `article`, `nav`, `header`, `footer`, `main`, `ul`/`ol`/`li`, `blockquote`, `pre`, `table`/`thead`/`tbody`/`tr`/`th`/`td`, `dl`/`dt`/`dd`, `figure`/`figcaption`, `details`/`summary`, plus inline elements `a`, `span`, `code`, `strong`, `em`, `small`, `mark`, `del`, `abbr`, `sub`, `sup`, `kbd`, `time`, and void elements `img`, `br`, `hr`, `wbr`.

**Attribute sugar:** `cls` (`class=`), `id'`, `role`, `href`, `src`, `type'`, `name'`, `value'`, `placeholder'`, `title'`, `alt`, `lang`, `data' key value`, `aria key value`, and `boolAttr name on`.

**Class-shortcut builders** (`{tag}C`): `divC`, `pC`, `spanC`, `sectionC`, `ulC`, `liC`, `navC`, `headerC`, `footerC`, `mainC`, `articleC`, `asideC`, `h1C`–`h3C`, `blockquoteC`, `preC`, `codeC`, `tableC`, `imgC`, and `codeBlock lang code`.

**Link shortcuts:** `aBlank` (new tab), `aHref`, `aC`.

**Conditional helpers:** `showIf cond content`, `hideIf cond content`, and `render nodes` (prints nodes joined by newlines — the terminal operation of a page).

**Document structure:** `doctype`, `html`, `head`, `body`, `title`, `meta`, `link`, `stylesheet`, `script`, `scriptInline`, `style`, `styleZcss`; plus `comment`, `nbsp`, `fragment`, `fragmentLines`.

## `DslComponents` — Components

Open with `open DslComponents`.

- **Layout:** `container`, `row`, `col`, `card`, `badge`.
- **Alerts:** `alert level children`, `alertInfo`, `alertSuccess`, `alertWarning`, `alertDanger`.
- **Buttons:** `btnPrimary`, `btnSecondary`, `btnSuccess`, `btnDanger`; form elements `form`, `input`, `button`, `textarea`, `select`, `option`, `label`.
- **Navigation:** `navLink url label isActive`, `navList`, `breadcrumb`.
- **Tags & media:** `tagBadges baseUrl tags`, `badgeC variant text`, `icon name` (renders `<span class="icon icon-NAME" aria-hidden="true">`), `figureResponsive`, `videoEmbed`.
- **Status:** `progressBar percent label`, `meterBar value optimum`.
- **Utility:** `each`, `joinWith`, `opt`, `renderIf`, `renderOpt`.

```fsharp
open DslComponents

page {
    alertInfo [ text "Heads up: the build is in 10 minutes." ]
    icon "star"
}
```

## `DslSugar` — Conditionals, Loops, Pipelines

Open with `open DslSugar`.

- **Yield:** `yield_block` (join with newlines), `yield_inline` (join with empty string).
- **Conditionals:** `cond`, `default_to`, `coalesce_str`, `when_true`, `unless_true`, `switch_value`, `match_cond`.
- **Loops:** `each_with items sep f`, `each_line`, `each_in_container tag items f`, `numbered_list`, `for_range`.
- **Shorthand builders:** `div_text cls content`, `span_text`, `p_text`, `h_text level content`, `a_text`, `a_text_c`, `img_c`, `ul_from`, `ol_from`.
- **Formatting:** `truncate_str`, `pad_right`, `pad_left`, `pluralize`, `titleize`, `capitalize`.
- **Option rendering:** `opt_str`, `opt_or`, `opt_map`, `opt_when`.

## `DslUtilities` — Markdown, JS, JSON, Collections

Open with `open DslUtilities`.

- `md` — render a Markdown string to HTML, for mixing prose into the DSL tree.
- `mdDedent` / `dedent` — strip common leading indentation before rendering.
- `js` — inline `<script>` block; `jsModule` — `<script type="module">`.
- `jsonBlock name data` — serialise F# data to JSON and inject as `<script>window.NAME = JSON</script>` (uses `System.Text.Json` with `</script>`-safe escaping):

```fsharp
jsonBlock "__CFG__" {| theme = "dark"; count = 10 |}
// → <script>window.__CFG__ = {"theme":"dark","count":10}</script>
```

- Control flow: `switch_str`, `cond_str`, `choose`.
- Collection helpers: `take_n`, `skip_n`, `filter_by`, `map_by`, `group_by`, `chunk`, `intersperse_str`, `zip_lists`.

## `DslSeo` — SEO Tags

Open with `open DslSeo`.

| Function | Emits |
|---|---|
| `meta_tags title description url image siteName` | Charset, viewport, title, description, canonical, image meta |
| `open_graph_tags title description url image ogType` | `og:*` tags |
| `twitter_card_tags cardType title description image site` | `twitter:*` tags |
| `canonical_url url` | `<link rel="canonical">` |
| `hreflang_tag lang url` | `<link rel="alternate" hreflang="...">` |

## `DslXml` — Feeds

Open with `open DslXml`.

- `rss_xml siteTitle siteUrl siteDescription pages` — RSS 2.0 feed.
- `atom_xml siteTitle siteUrl siteDescription authorName pages` — Atom 1.0 feed.
- `sitemap_xml baseUrl pages` — sitemap XML.

The starter project's `content/rss.zest.fsx` and `content/sitemap.zest.fsx` are working examples.

## Styling Modules

`DslCss` provides the `stylesheet { }` computation expression and typed property functions (`bg`, `color`, `font_size`, `margin`, `flex`, `grid` …). `DslStyle` integrates styles into pages: `styleZcss`, `styleCss`, `styleScoped`, `styleExternal`, and `criticalCss`. The CSS DSL is covered in detail by the [zcss](/en/posts/zcss/) page and the legacy `dsl-style` notes.

## Supporting Modules

- `StringHelper` — `slugify`, `truncate`, `strip_html`, `reading_time`, `word_count`, `excerpt`, `title_case`, `coalesce`.
- `DateHelper` — `format_date`, `format_date_custom`, `format_date_iso`, `format_date_rfc`, `date_add_days`, `date_diff`, `now`, `current_year`, `url_encode`.
- `ContentGuard` — `guard`, `validate`, `require`, `warn_if`, `debug`, `trace`.

For page queries over the site — `site_pages`, `recent_pages`, `pages_by_tag`, `paginate`, `site_data` and friends — see [dsl-collections](/en/posts/dsl-collections/).
