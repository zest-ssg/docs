// default.zest.fsx
//
// Single-column fallback layout for standalone pages that do not belong to
// the documentation tree (about, search, 404, tag archives). Keeps the same
// card styling as the docs layout so pages feel consistent.
//
// Dependencies: base.zest.fsx (nested)

// @layout base

render [
    divC "page-band page-band--solo" [
        articleC "content-card" [
            divC "content-body" [ raw content ]
        ]
    ]
]
