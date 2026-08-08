// _init.zest.fsx — runs once before every build.
//
// Inject global site data, register custom filters, and schedule
// post-build tasks. All data exposed as `{{ site.<key> }}` in templates.

// ── Site metadata ──────────────────────────────────
// Social links displayed in the footer.
addGlobal "socials" [|
    {| label = "GitHub";  url = "https://github.com/zest-ssg";  icon = "github" |}
    {| label = "X";       url = "https://x.com/zest_ssg";        icon = "x" |}
|]

// Build timestamp for cache-busting query strings.
addGlobal "build_time" (System.DateTime.UtcNow.ToString("yyyyMMddHHmmss"))
