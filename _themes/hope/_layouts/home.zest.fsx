// home.zest.fsx
//
// Landing-page layout: renders the hero copy supplied by the page body and
// appends a documentation index grouped by category, so visitors can jump
// into any guide straight from the home page.
//
// Dependencies: base.zest.fsx (nested), ZestContext

// @layout base

let ctx = Context.get ()

let siteValue (key: string) : string =
    match ctx.SiteData.TryGetValue key with
    | true, v ->
        match v with
        | null -> ""
        | :? string as s -> s
        | _ -> v.ToString()
    | _ -> ""

let langCode = if page.url.StartsWith "/zh/" then "zh" else "en"
let tKey (key: string) : string = t_lang key langCode

let isDocPage (p: PageInfo) = p.category <> "" && p.url.StartsWith("/" + langCode)

let categoryOrder = [ "guides"; "templates"; "dsl"; "styling"; "features"; "reference" ]

let categoryRank (cat: string) =
    match List.tryFindIndex ((=) cat) categoryOrder with
    | Some i -> i
    | None -> categoryOrder.Length

let homeGroups =
    site_pages ()
    |> Array.filter isDocPage
    |> Array.groupBy (fun (p: PageInfo) -> p.category)
    |> Array.sortBy (fun (cat, _) -> categoryRank cat)

let renderGroupCard (cat: string) (pages: PageInfo[]) =
    divC "feature-card" [
        divC "feature-card__title" [ text (tKey ("sidebar." + cat)) ]
        ulC "recent-list" (
            pages
            |> Array.sortBy (fun (p: PageInfo) -> p.slug)
            |> Array.map (fun (p: PageInfo) ->
                liC "recent-list__item" [
                    divC "recent-list__title" [ elem "a" [ href p.url ] [ text p.title ] ]
                ])
            |> Array.toList)
    ]

render [
    divC "home-band" [
        divC "content-card" [
            divC "content-body" [ raw content ]
        ]
        divC "feature-grid" (
            homeGroups
            |> Array.map (fun (cat, pages) -> renderGroupCard cat pages)
            |> Array.toList)
    ]
]
