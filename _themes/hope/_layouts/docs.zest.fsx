// docs.zest.fsx
//
// Three-column documentation layout used by all reference pages. Builds the
// left sidebar tree from page front matter (category + title), renders the
// body in a centred card with breadcrumb and prev/next pager, and reserves a
// right-hand "On this page" column that client-side JS fills from the
// article headings.
//
// The layout script runs in its own FSI session, so the context helpers from
// base.zest.fsx are re-declared here rather than shared.
//
// Dependencies: base.zest.fsx (nested via `// @layout base`), ZestContext

// @layout base

// ── Context helpers ─────────────────────────────────

let ctx = Context.get ()

let siteValue (key: string) : string =
    match ctx.SiteData.TryGetValue key with
    | true, v ->
        match v with
        | null -> ""
        | :? string as s -> s
        | _ -> v.ToString()
    | _ -> ""

// Active locale is derived from the URL prefix so both languages share one
// build; site.language only labels the primary language in _config.toml.
let langCode = if page.url.StartsWith "/zh/" then "zh" else "en"
let homeUrl = sprintf "/%s/" langCode
let tKey (key: string) : string = t_lang key langCode

// ── Sidebar tree ────────────────────────────────────
// Doc pages carry a non-empty `category` front-matter value; grouping and
// ordering are derived from that key so the navigation stays in sync with
// the content without a hand-maintained nav file.

let isDocPage (p: PageInfo) = p.category <> "" && p.url.StartsWith("/" + langCode)

let categoryOrder = [ "guides"; "templates"; "dsl"; "styling"; "features"; "reference" ]

let categoryRank (cat: string) =
    match List.tryFindIndex ((=) cat) categoryOrder with
    | Some i -> i
    | None -> categoryOrder.Length

let docPages =
    site_pages ()
    |> Array.filter isDocPage
    // Slug equals the file stem, so intra-group order is stable and matches
    // across languages (both locales share identical file names).
    |> Array.sortBy (fun (p: PageInfo) -> categoryRank p.category, p.slug)

let sidebarGroups =
    docPages
    |> Array.groupBy (fun (p: PageInfo) -> p.category)
    |> Array.sortBy (fun (cat, _) -> categoryRank cat)

let renderSidebarItem (p: PageInfo) =
    li [ elem "a" [ cls (if p.url = page.url then "is-active" else ""); href p.url ] [ text p.title ] ]

let renderSidebar =
    asideC "sidebar" (
        sidebarGroups
        |> Array.map (fun (cat, pages) ->
            divC "sidebar__group" [
                divC "sidebar__title" [ text (tKey ("sidebar." + cat)) ]
                ulC "sidebar__list" (pages |> Array.map renderSidebarItem |> Array.toList)
            ])
        |> Array.toList)

// ── Breadcrumb ───────────────────────────────────────

let renderBreadcrumb =
    navC "breadcrumb" [
        elem "a" [ href homeUrl ] [ text (tKey "nav.home") ]
        spanC "breadcrumb__sep" [ text "/" ]
        span [ text (tKey ("sidebar." + page.category)) ]
        spanC "breadcrumb__sep" [ text "/" ]
        span [ text page.title ]
    ]

// ── Prev / next pager ────────────────────────────────
// Neighbours come from the flat, category-ordered page list, so browsing
// the docs follows the sidebar order.

let renderNavCard (dir: string) (p: PageInfo) =
    elem "a" [ cls ("page-nav__card page-nav__card--" + dir); href p.url ] [
        divC "page-nav__label" [ text (tKey ("nav." + dir)) ]
        divC "page-nav__title" [ text p.title ]
    ]

let renderPageNav =
    match Array.tryFindIndex (fun (p: PageInfo) -> p.url = page.url) docPages with
    | Some i when docPages.Length > 1 ->
        let prevPage = if i > 0 then Some docPages.[i - 1] else None
        let nextPage = if i < docPages.Length - 1 then Some docPages.[i + 1] else None
        navC "page-nav" [
            match prevPage with
            | Some p -> renderNavCard "prev" p
            | None -> elem "span" [ cls "page-nav__card" ] []
            match nextPage with
            | Some p -> renderNavCard "next" p
            | None -> elem "span" [ cls "page-nav__card" ] []
        ]
    | _ -> ""

// ── Right-hand TOC (filled by main.js) ───────────────

let renderToc =
    asideC "toc" [
        divC "toc__title" [ text (tKey "toc.title") ]
        divC "toc__body" []
    ]

// ── Document body ────────────────────────────────────

render [
    divC "page-band" [
        divC "page-band__content" [
            renderSidebar
            articleC "content-card" [
                renderBreadcrumb
                divC "content-body" [ raw content ]
                renderPageNav
            ]
        ]
        renderToc
    ]
]
