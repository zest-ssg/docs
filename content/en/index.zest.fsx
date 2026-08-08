// @title Zest Docs
// @layout home
// @description Documentation for Zest — a static site generator where templates are real F#.
//
// The home layout renders this hero copy and appends the grouped
// documentation index below it.

open Zest.Dsl

render [
    divC "hero" [
        h1C "hero__title" [ text "A static site generator where templates are real F#." ]
        pC "hero__lead" [ text "Zest compiles `.zest.fsx` F# scripts, Nunjucks, Liquid, Handlebars and more into a fast static site. This documentation is itself a Zest site — every page you see here is rendered by the tool it describes." ]
        divC "hero__actions" [
            aC "btn btn--primary" "/en/posts/getting-started/" [ text "Get started" ]
            aC "btn" "/en/posts/nunjucks/" [ text "Templates" ]
            aC "btn" "/zh/" [ text "中文文档" ]
        ]
    ]
]
