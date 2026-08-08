// @permalink /zh/rss.xml
// @layout none
// @title RSS Feed (Chinese)
//
// Generates the Chinese documentation feed. Rendered with no layout so the
// output is raw XML at /zh/rss.xml, matching the <link rel="alternate"> tag
// that the base layout emits for Chinese pages.

open System
open System.Globalization
open System.Text
open Zest.Dsl

let inv = CultureInfo.InvariantCulture

let xe (s: string) =
    if String.IsNullOrEmpty s then "" else
    s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;")
     .Replace("\"", "&quot;").Replace("'", "&apos;")

let data = Context.get().SiteData
let opt k = if data.ContainsKey(k) then data.[k].ToString() else ""

// TrimEnd guards against a trailing slash in site.base_url producing "//" links.
let siteUrl   = (let u = opt "site.base_url" in if u <> "" then u else "https://example.com").TrimEnd('/')
let siteTitle = opt "site.title"
let siteDesc  = opt "site.description"

let posts =
    site_pages ()
    |> Array.filter (fun p -> p.url.StartsWith "/zh/posts/" && p.url <> "/zh/posts/")
    |> Array.sortByDescending (fun p -> p.date)

let sb = StringBuilder()
sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>") |> ignore
sb.AppendLine("<rss version=\"2.0\" xmlns:atom=\"http://www.w3.org/2005/Atom\">") |> ignore
sb.AppendLine("  <channel>") |> ignore
sb.AppendFormat("    <title>{0}</title>\n", xe siteTitle) |> ignore
sb.AppendFormat("    <link>{0}</link>\n", xe siteUrl) |> ignore
sb.AppendFormat("    <description>{0}</description>\n", xe siteDesc) |> ignore
sb.AppendFormat("    <atom:link href=\"{0}/zh/rss.xml\" rel=\"self\" type=\"application/rss+xml\" />\n", xe siteUrl) |> ignore
sb.AppendLine("    <language>zh-CN</language>") |> ignore
sb.AppendFormat("    <lastBuildDate>{0}</lastBuildDate>\n", DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", inv)) |> ignore
for p in posts do
    let full = if p.url.StartsWith "/" then siteUrl + p.url else p.url
    let pub =
        match DateTime.TryParse(p.date, inv, DateTimeStyles.None) with
        | true, d -> d.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", inv)
        | _ -> DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", inv)
    sb.AppendLine("    <item>") |> ignore
    sb.AppendFormat("      <title>{0}</title>\n", xe p.title) |> ignore
    sb.AppendFormat("      <link>{0}</link>\n", xe full) |> ignore
    sb.AppendFormat("      <guid>{0}</guid>\n", xe full) |> ignore
    sb.AppendFormat("      <pubDate>{0}</pubDate>\n", pub) |> ignore
    sb.AppendFormat("      <description>{0}</description>\n", xe p.description) |> ignore
    sb.AppendLine("    </item>") |> ignore
sb.AppendLine("  </channel>") |> ignore
sb.AppendLine("</rss>") |> ignore

printfn "%s" (sb.ToString())
