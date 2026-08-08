// base.zest.fsx
//
// Root layout for every page in the oxygen docs theme. Renders the complete
// document: head (meta, styles, theme bootstrap, search index), site header
// with live search, the page region (<main>) and the footer. The docs, home
// and default layouts nest inside this one via `// @layout base`.
//
// Dependencies: Zest.Dsl (auto-opened in the layout preamble), ZestContext

// ── Context helpers ─────────────────────────────────────

let ctx = Context.get ()

/// Read a flattened site-data key as a string (never throws on missing keys).
let siteValue (key: string) : string =
    match ctx.SiteData.TryGetValue key with
    | true, v ->
        match v with
        | null -> ""
        | :? string as s -> s
        | _ -> v.ToString()
    | _ -> ""

/// Active locale code ("en" / "zh"). Derived from the URL prefix rather than
/// the global site.language config, so both locales share one build.
let langCode = if page.url.StartsWith "/zh/" then "zh" else "en"

let siteTitle   = siteValue "site.title"
let siteDesc    = siteValue "site.description"
let siteBaseUrl = siteValue "site.base_url"
let accentColor = siteValue "params.colors.accent"
let homeUrl     = sprintf "/%s/" langCode

/// Social links injected by `_init.zest.fsx` (label, url, icon).
let socials : (string * string * string) list =
    match ctx.SiteData.TryGetValue "socials" with
    | true, (:? (obj[]) as arr) ->
        arr
        |> Array.choose (fun o ->
            match o with
            | :? System.Collections.Generic.IDictionary<string, obj> as d ->
                let get (k: string) =
                    match d.TryGetValue k with
                    | true, v ->
                        match v with
                        | null -> ""
                        | :? string as s -> s
                        | _ -> v.ToString()
                    | _ -> ""
                Some (get "label", get "url", get "icon")
            | _ -> None)
        |> Array.toList
    | _ -> []

/// Search index for the current language: doc pages only (they carry a category).
let docIndex =
    ctx.Pages
    |> Array.filter (fun (p: PageInfo) -> p.category <> "" && p.url.StartsWith("/" + langCode))
    |> Array.map (fun (p: PageInfo) ->
        {| url = p.url
           title = p.title
           group = t_lang ("sidebar." + p.category) langCode
           description = p.description |})

/// Translate a key for the active locale (explicit lang, falls back to key).
let tKey (key: string) : string = t_lang key langCode

// ── SVG icons (inline, stroke style) ────────────────────

let icon (cls: string) (paths: string) : string =
    raw (sprintf "<svg class=\"%s\" viewBox=\"0 0 24 24\" width=\"18\" height=\"18\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" aria-hidden=\"true\">%s</svg>" cls paths)

// ── Head ────────────────────────────────────────────────

let renderHead =
    head [
        meta [ attr "charset" "utf-8" ]
        meta [ attr "name" "viewport"; attr "content" "width=device-width, initial-scale=1.0" ]
        link "canonical" (siteBaseUrl + page.url)
        link "alternate" (sprintf "%s/%s/rss.xml" siteBaseUrl langCode)
        title [ text (page.title + " | " + siteTitle) ]
        meta [ attr "name" "description"; attr "content" page.description ]
        meta [ attr "name" "theme-color"; attr "content" (if accentColor <> "" then accentColor else "#4f6ef7") ]
        stylesheet "/assets/css/main.css"
        link "preconnect" "https://fonts.googleapis.com"
        link "preconnect" "https://fonts.gstatic.com"
        stylesheet "https://fonts.googleapis.com/css2?family=IBM+Plex+Sans:wght@400;500;600&family=Inconsolata:wght@400&display=swap"
        // User-configurable accent from [params.colors] wins over the token.
        showIf (accentColor <> "") (styleZcss (sprintf ":root { --accent: %s; }" accentColor))
        // Apply the saved / system theme before the first paint to avoid a flash.
        scriptInline """(function(){try{var t=localStorage.getItem('zest-theme');if(!t){t=window.matchMedia&&window.matchMedia('(prefers-color-scheme: dark)').matches?'dark':'light';}document.documentElement.dataset.theme=t;}catch(e){document.documentElement.dataset.theme='light';}})();"""
        jsonBlock "__DOC_INDEX__" docIndex
    ]

// ── Header ──────────────────────────────────────────────

/// Link to the same page in another language, falling back to its home page.
let counterpart (fromLang: string) (toLang: string) (url: string) : string =
    let prefix = sprintf "/%s/" fromLang
    let target = sprintf "/%s/" toLang
    if url.StartsWith prefix then
        let rel = url.Substring prefix.Length
        let candidate = target + rel
        if ctx.Pages |> Array.exists (fun (x: PageInfo) -> x.url = candidate) then candidate
        else target
    else target

let renderLangSwitch =
    divC "lang-switch" [
        elem "a" [ cls (if langCode = "en" then "is-active" else ""); href (counterpart "zh" "en" page.url) ] [ text "EN" ]
        spanC "lang-switch__sep" [ text "·" ]
        elem "a" [ cls (if langCode = "zh" then "is-active" else ""); href (counterpart "en" "zh" page.url) ] [ text "中文" ]
    ]

let renderHeader =
    headerC "site-header" [
        divC "site-header__inner" [
            elem "button" [ cls "icon-btn sidebar-toggle"; type' "button"; aria "label" (tKey "nav.menu") ] [
                icon "icon-menu" "<path d=\"M4 6h16M4 12h16M4 18h16\"/>"
            ]
            elem "a" [ cls "brand"; href homeUrl ] [
                spanC "brand__mark" [ text "Z" ]
                spanC "brand__text" [ text siteTitle ]
            ]
            divC "header-search" [
                spanC "header-search__icon" [ icon "icon-search" "<circle cx=\"11\" cy=\"11\" r=\"7\"/><path d=\"M21 21l-4.3-4.3\"/>" ]
                voidElem "input" [ attr "type" "search"
                                   attr "class" "header-search__input"
                                   attr "placeholder" (tKey "search.placeholder")
                                   attr "aria-label" (tKey "search.placeholder")
                                   attr "autocomplete" "off" ]
                divC "header-search__results" [ data' "empty" (tKey "search.empty") ]
            ]
            divC "header-actions" [
                elem "button" [ cls "icon-btn"; data' "theme-toggle" ""; type' "button"; aria "label" (tKey "theme.toggle") ] [
                    icon "icon-sun" "<circle cx=\"12\" cy=\"12\" r=\"4\"/><path d=\"M12 2v2M12 20v2M4.9 4.9l1.4 1.4M17.7 17.7l1.4 1.4M2 12h2M20 12h2M4.9 19.1l1.4-1.4M17.7 6.3l1.4-1.4\"/>"
                    icon "icon-moon" "<path d=\"M21 12.8A9 9 0 1 1 11.2 3a7 7 0 0 0 9.8 9.8z\"/>"
                ]
                renderLangSwitch
                elem "a" [ href "https://github.com/zest-ssg/zest"; attr "rel" "me"; aria "label" "GitHub" ] [ text "GitHub" ]
            ]
        ]
    ]

// ── Footer ──────────────────────────────────────────────

let renderFooter =
    footerC "site-footer" [
        divC "site-footer__inner" [
            spanC "site-footer__brand" [ text (siteTitle + " — " + siteDesc) ]
            ulC "site-footer__links" (socials |> List.map (fun (label, url, _) ->
                li [ elem "a" [ href url; attr "rel" "me" ] [ text label ] ]))
        ]
    ]

// ── Document ────────────────────────────────────────────

render [
    doctype
    elem "html" [ lang (if langCode = "zh" then "zh-CN" else "en") ] [
        renderHead
        body [
            divC "site-wrapper" [
                renderHeader
                mainC "main" [ raw content ]
                renderFooter
                elem "button" [ cls "back-to-top"; type' "button"; aria "label" (tKey "nav.back_top") ] [
                    icon "icon-top" "<path d=\"M18 15l-6-6-6 6\"/>"
                ]
            ]
            elem "div" [ cls "sidebar-backdrop" ] []
            // External script needs an explicit closing tag (script is not void).
            elem "script" [ attr "src" "/assets/js/main.js" ] []
            raw (siteValue "pjaxScript")
        ]
    ]
]
