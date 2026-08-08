// @title Zest 文档
// @layout home
// @description Zest 静态站点生成器文档——模板即真正的 F#。
//
// 首页布局会渲染这段 hero 文案，并在下方追加按分类分组的文档索引。

open Zest.Dsl

render [
    divC "hero" [
        h1C "hero__title" [ text "模板即真正的 F# 的静态站点生成器。" ]
        pC "hero__lead" [ text "Zest 将 `.zest.fsx` F# 脚本、Nunjucks、Liquid、Handlebars 等多种模板编译为快速的静态站点。本套文档本身就是一个 Zest 站点——你看到的每一页都由它所描述的工具渲染。" ]
        divC "hero__actions" [
            aC "btn btn--primary" "/zh/posts/getting-started/" [ text "快速开始" ]
            aC "btn" "/zh/posts/nunjucks/" [ text "模板" ]
            aC "btn" "/en/" [ text "English docs" ]
        ]
    ]
]
