// @permalink /404.html
// @layout default
// @title 404 — Page Not Found
// @description The page you were looking for could not be found.
//
// Custom 404 page. The dev server and preview server serve this file
// automatically when a route is not found.

open Zest.Dsl

render [
    divC "not-found" [
        h1 [ text "404" ]
        p [ text "Page not found. " ]
        pC "not-found__text" [ text "The page you were looking for could not be found, or it only exists in the other language." ]
        aC "btn btn--primary" "/en/" [ text "Back to English docs" ]
        span [ text " " ]
        aC "btn" "/zh/" [ text "返回中文文档" ]
    ]
]
