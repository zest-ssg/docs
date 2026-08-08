+++
title = "Zest DSL 指南"
description = "使用 page { } 计算表达式构建页面：元素、文本、条件与循环。"
category = "dsl"
tags = ["zest", "dsl", "fsharp"]
date = 2026-08-01
+++

# Zest DSL 指南

本文属于 Zest DSL 分类，是 `.zest.fsx` 页面脚本的使用指南；全部函数签名见 [DSL API 参考](/zh/posts/dsl-api/)，页面查询 API 见 [DSL 集合](/zh/posts/dsl-collections/)。

## 页面结构

`.zest.fsx` 页面由 F# 注释形式的元数据头与 `page { }` 计算表达式组成：

```fsharp
// @title 我的页面
// @layout default
// @date 2026-08-01

page {
    h1 [ text "你好" ]
    p  [ text "这是正文。" ]
}
```

## 文本与原始 HTML

`text` 对内容做 HTML 转义，`raw` 原样注入，`elem` 生成任意元素：

```fsharp
page {
    text "被转义 <b>的内容</b>"
    raw  "<strong>不转义</strong>"
    elem "custom-tag" [ attr "data-x" "1" ] [ text "任意元素" ]
}
```

## 元素组合

所有块级/行内构建器都接收 `string list` 作为子节点，类快捷方式在元素名后加 `C`：

```fsharp
page {
    divC "container" [
        h1C "title" [ text "标题" ]
        p [ text "段落" ]
        a "/about/" [ text "关于" ]
        aBlank "https://github.com" "GitHub（新标签页）"
    ]
    ul [ li [ text "A" ]; li [ text "B" ] ]
    img "/img/photo.jpg" "照片说明"
}
```

## 条件与循环

`page { }` 内置条件与循环助手（`DslSugar`）：

```fsharp
page {
    when' (showBanner) [ divC "banner" [ text "特惠" ] ]
    unless (isDraft)   [ p [ text "已发布内容" ] ]

    choose_content (isLoggedIn)
        [ p [ text "欢迎回来" ] ]
        [ p [ text "公开页" ] ]

    match_content [
        (kind = "blog", [ article [ text "博客布局" ] ])
        (kind = "docs", [ main [ text "文档正文" ] ])
    ]

    for_each (recent_pages 5) (fun p ->
        divC "post-card" [ h2 [ text p.title ]; p [ text p.description ] ])

    for_range 1 10 (fun i -> p [ text ("第 " + string i + " 项") ])
    repeat 3 [ spanC "star" [ text "*" ] ]
}
```

F# 自身的 `for` / `if … then … else` 也可直接在 CE 内使用。

## 常用模式

- **混合 Markdown**：`md` / `mdDedent` 助手把 Markdown 字符串渲染进 DSL 树。
- **注入脚本与样式**：`js` 生成 `<script>` 块，`jsonBlock` 把 F# 数据序列化为 `window.NAME` 供前端使用。
- **SEO 与订阅源**：`DslSeo` 的 `meta_tags` / `open_graph_tags`，`DslXml` 的 `rss_xml` / `sitemap_xml`。
- **页面数据**：`data "key" value` 写入自定义字段，模板中以 `{{ page.key }}` 读取。

更多细节见 [DSL API 参考](/zh/posts/dsl-api/) 与 [DSL 集合](/zh/posts/dsl-collections/)。
