// @title Zest DSL
// @description Zest 的原生模板语言就是真正的 F#。本页是一个真实存在的 .zest.fsx 脚本，在构建时由 FSI 求值。
// @category dsl
// @tags dsl, fsharp
// @date 2026-08-01
//
// 内容页脚本可以访问站点上下文（Context.get()），但没有 `page` 绑定——该绑定只存在于布局中。
// 下面的正文由 FSI 渲染，再由 F# 的 docs 布局包裹。

open Zest.Dsl

/// 输出带主题语言标签的代码示例。
let codeSample (lang: string) (code: string) =
    elem "pre" [] [ elem "code" [ cls ("language-" + lang) ] [ raw code ] ]

let helloSource =
    """render [
    h1 [ text "你好，世界" ]
    p  [ text "页面数：" ; strong [ text "42" ] ]
    a  "/posts/" [ text "阅读文章" ]
]"""

let loopSource =
    """site_pages ()
|> Array.filter (fun p -> p.category <> "" && p.url.StartsWith "/zh/")
|> Array.sortBy (fun p -> p.slug)
|> Array.truncate 6
|> Array.map (fun p -> li [ elem "a" [ href p.url ] [ text p.title ] ])
|> Array.toList
|> ul"""

render [
    h1 [ text "Zest DSL" ]
    p [
        text "Zest 的原生模板语言就是真正的 F#。一个 "
        code [ text ".zest.fsx" ]
        text " 页面就是一份由 F# Interactive 在构建时求值的脚本：DSL 助手把 HTML 组合成字符串，"
        code [ text "render" ]
        text " 把结果输出到 stdout。你获得的是类型、自动补全和完整的 IDE 支持——而引擎只做一件简单的事：捕获脚本的输出。"
    ]
    h2 [ text "组合" ]
    p [ text "元素只是作用于字符串子节点的普通函数：" ]
    codeSample "fsharp" helloSource
    h2 [ text "对站点数据的管道" ]
    p [
        text "DslCollections 把整个站点暴露为带类型的记录，因此数据变换就是普通的 F# 管道。下面这份列表正是由下方脚本生成的："
    ]
    codeSample "fsharp" loopSource
    h2 [ text "实时结果" ]
    p [ text "从构建上下文中取出的六个文档页：" ]
    (site_pages ()
     |> Array.filter (fun p -> p.category <> "" && p.url.StartsWith "/zh/")
     |> Array.sortBy (fun p -> p.slug)
     |> Array.truncate 6
     |> Array.map (fun p -> li [ elem "a" [ href p.url ] [ text p.title ] ])
     |> Array.toList
     |> ul)
    h2 [ text "助手函数" ]
    p [
        text "除元素构建器外，还有针对常见模式的简写——"
        code [ text "divC" ]
        text "、"
        code [ text "aC" ]
        text "、"
        code [ text "ulC" ]
        text "、用于向客户端 JS 注入类型化数据的 "
        code [ text "jsonBlock" ]
        text "、行内脚本 "
        code [ text "js" ]
        text "，以及按语言查找文案的 "
        code [ text "t_lang" ]
        text "。完整清单见 DSL 参考。"
    ]
]
