+++
title = "模板参考"
description = "模板上下文参考：可用变量、布局扩展名路由、布局嵌套与 frontmatter 语法。"
category = "reference"
tags = ["zest", "参考", "模板"]
date = 2026-08-01
+++

# 模板参考

本文属于 Reference（参考）分类，汇总布局渲染所需的上下文变量、路由规则与 frontmatter 语法。各模板语言的特性见 Templates 分类下的页面。

## 上下文变量

布局渲染时注入以下变量：

| 变量 | 类型 | 说明 |
|---|---|---|
| `content` / `page.content` | string | 页面已渲染的正文 HTML |
| `page.title` / `page.url` / `page.slug` | string | 页面元数据 |
| `page.date` | string | 日期（`yyyy-MM-dd`） |
| `page.tags` | string[] | 标签数组 |
| `page.description` | string | 描述 |
| `page.<extra>` | varies | frontmatter 未识别键 |
| `site.title` / `site.description` / `site.base_url` | string | 来自 `_config.toml` |
| `site.version` / `site.author` / `site.language` | string | 来自配置 |
| `site.{namespace}.{key}` | varies | `_data/` 下的 TOML 数据 |
| `site.params.*` | varies | `[params]` 表 |
| `menu.{name}` | JSON 数组 | `[menu.{name}]` 配置 |
| `pages` | object[] | 全站页面 |
| `tags` | object | 标签 → 页面 |
| `collections` | object | 集合 → 页面 |
| `pjaxScript` | string | 内置 PJAX 脚本（配合 safe 过滤器输出） |
| include 名 | string | 每个 `_includes/` partial 作为独立变量 |

`.zest.fsx` 布局中则以顶层绑定形式提供 `content`、`page`、`site` 匿名记录（见 [原生模板模式](/zh/posts/native-mode/)）。

## 布局扩展名路由

布局按扩展名选择处理引擎：

| 扩展名 | 处理方式 |
|---|---|
| `.html` / `.htm` | 直接通过；含 `{{ }}` / `{% %}` 时经 Nunjucks 预处理 |
| `.njk` | Nunjucks 引擎 |
| `.liquid` | LiquidConverter 转换后经 Nunjucks 渲染 |
| `.hbs` / `.mustache` | 独立 Hbs 引擎（原生语义） |
| `.haml` / `.pug` | 转换后经 Nunjucks 渲染 |
| `.webc` | 兼容层尚未实现，见 [WebC](/zh/posts/webc/) |
| `.zest.fsx` / `.fsx` | F# 脚本布局，`dotnet fsi` 求值，stdout 即页面 |

内容页同理：`.md` 经 Markdown 引擎，`.html` 直通（含模板语法时预处理），其余模板扩展名按上表路由。

## 布局嵌套

布局可通过 frontmatter 声明父布局（HTML 注释形式或 TOML 形式均可；`.zest.fsx` 布局用 F# 注释 `// @layout base`）：

```html
<!-- _layouts/post.html -->
<!-- @layout default -->
<article>
    {{ content }}
</article>
```

```html
---
layout = "default"
---
<article>
    {{ content }}
</article>
```

## Frontmatter 语法

`MetaParser` 识别三种格式，解析顺序为：**TOML 优先**；无 TOML 块时，模板扩展名（`.njk`、`.liquid`、`.hbs`、`.mustache`、`.webc`、`.haml`、`.pug`）用 HTML 注释解析器，其余（`.zest.fsx`、`.fsx`、`.md` 等）用 F# 注释解析器。

```markdown
+++
title = "文章"
layout = "post"
tags = ["fsharp"]
+++
```

```html
<!-- @title 页面 -->
<!-- @layout default -->
```

```fsharp
// @title 页面
// @layout default
```

已知键：`layout`、`title`、`permalink`、`description`、`date`、`tags` / `tag`、`draft`、`author`、`updated`、`weight`、`template`、`collection`；未识别键存入 `Extra`。
