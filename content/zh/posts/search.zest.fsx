// @title 搜索
// @description 构建期 JSON 索引加客户端实时过滤：标题栏搜索与独立搜索页。本页本身就是一个 .zest.fsx 脚本。
// @category features
// @tags zest, 功能, 搜索
// @date 2026-08-01
//
// Content-page scripts receive the site context (Context.get()) but not a
// `page` binding — layouts are the only place that binding exists. The body
// below is rendered by FSI and wrapped by the F# docs layout.

open Zest.Dsl

/// 输出带主题语言标签的代码示例。
let codeSample (lang: string) (code: string) =
    elem "pre" [] [ elem "code" [ cls ("language-" + lang) ] [ raw code ] ]

let indexSource =
    """let docIndex =
    ctx.Pages
    |> Array.filter (fun p -> p.category <> "" && p.url.StartsWith "/zh/")
    |> Array.map (fun p ->
        {| url = p.url
           title = p.title
           group = t_lang ("sidebar." + p.category) "zh"
           description = p.description |})

// 在 <head> 中：
jsonBlock "__DOC_INDEX__" docIndex"""

let jsSource =
    """const index = window.__DOC_INDEX__ || [];
const terms = q.split(/\\s+/);
const hits = index.filter(p =>
  terms.every(t => (p.title + ' ' + p.group + ' ' + p.description)
    .toLowerCase().indexOf(t) !== -1));"""

render [
    h1 [ text "搜索" ]
    p [
        text "Zest 站点的搜索完全在浏览器内进行，不需要服务器：构建期把页面索引序列化为 JSON 注入页面，客户端 JS 对索引做实时过滤。本页用 Zest 原生 DSL（"
        code [ text ".zest.fsx" ]
        text "）写成，代码示例展示了本站的实际实现。"
    ]
    h2 [ text "构建期索引" ]
    p [
        text "主题在布局中把搜索索引注入为 "
        code [ text "window.__DOC_INDEX__" ]
        text "。本站的做法是在 "
        code [ text "base.zest.fsx" ]
        text " 布局中筛选出带 "
        code [ text "category" ]
        text " 的文档页，映射为 "
        code [ text "{ url, title, group, description }" ]
        text " 后用 "
        code [ text "jsonBlock" ]
        text " 注入："
    ]
    codeSample "fsharp" indexSource
    p [
        text "jsonBlock"
        text " 会处理 "
        code [ text "</script>" ]
        text " 转义，保证注入安全。索引内容与主题结构有关，可按需自定义。"
    ]
    h2 [ text "标题栏实时搜索" ]
    p [
        text "主题的 "
        code [ text "main.js" ]
        text " 监听搜索输入框，把查询按空白拆成多个词，对每条索引（标题 + 分组 + 描述拼接的小写文本）做多词 AND 匹配，结果渲染为下拉列表："
    ]
    codeSample "javascript" jsSource
    h2 [ text "独立搜索页" ]
    p [
        text "本站还提供独立搜索页："
        elem "a" [ href "/zh/search/" ] [ text "/zh/search/" ]
        text " 一个输入框加结果容器，输入时实时过滤 "
        code [ text "__DOC_INDEX__" ]
        text " 并渲染文章卡片（标题、分组、描述）。它同样是纯静态的，可被搜索引擎正常收录标题与描述文本。"
    ]
]
