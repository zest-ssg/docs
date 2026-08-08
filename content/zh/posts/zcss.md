+++
title = "ZCSS"
description = "ZCSS 预处理器：@use 模块、变量、嵌套与颜色函数，构建期编译为 CSS。"
category = "styling"
tags = ["zest", "zcss", "样式"]
date = 2026-08-01
+++

# ZCSS

本文属于 Styling（样式）分类，介绍 Zest 内置的 ZCSS 预处理器。`.zcss` 文件在构建时编译为 `.css`（`assets/` 中的文件输出到 `_site/assets/` 对应路径），无需任何外部工具链。

## 语法模式

ZCSS 支持三种写法，自动检测：

```scss
// 花括号模式（SCSS 风格）
$primary: #4a90d9;
a { color: $primary; &:hover { color: darken($primary, 10%); } }
```

```scss
// 缩进模式（Python 风格）
$primary: #4a90d9
a
  color: $primary
  &:hover
    color: darken($primary, 10%)
```

```scss
// 方括号模式（F# 风格）
let primary = #4f46e5
.btn [
  color = white
  background = primary |> darken(8%)
]
```

方括号模式中 `let name = value` 定义变量、`prop = value` 写属性，并支持 `|>` 颜色管道。

## 变量与嵌套

```scss
$spacing: 1rem !default;   // !default：仅当未定义时设置

.container {
  max-width: 1200px;
  padding: $spacing;
  .card { border: 1px solid #eee; }   // 嵌套
}
```

## 颜色函数

所有颜色函数作用于十六进制颜色：`lighten`、`darken`、`alpha`、`mix`、`complement`、`grayscale`、`invert`、`saturate`、`desaturate`、`adjustHue`、`tint`、`shade`、`transparentize`，以及构造色 `rgba` / `rgb` / `hsl` / `hsla`、综合 `scaleColor`。

## `@use` 内置模块

```scss
@use "zest:reset";
@use "zest:utilities";     // 显示、flex、间距、文本、颜色等工具类
@use "zest:palette";       // 语义色变量（均带 !default，可覆盖）
@use "zest:animations";
@use "zest:gradients";
@use "zest:filters";
@use "zest:layout";
@use "zest:composition";
@use "zest:all";           // 全部以上
```

`@import "variables"` 可引入其他 ZCSS 文件（相对路径）。

## 控制指令与响应式

```scss
@mixin card($bg: #fff) { background: $bg; padding: 1rem; }
.featured { @include card(#fef3c7); }

@if $theme == "dark" { body { background: #111; } } @else { body { background: #fff; } }

@each $c in (red, green, blue) { .text-$c { color: $c; } }

@sm { .container { padding: 0.5rem; } }   // @sm / @md / @lg / @xl / @2xl
```

另有 `@for`、`@extend`、`@apply` 与 `@content`；`calc(...)` 内支持带单位运算。编译器还会为 31 个常用属性自动添加厂商前缀，并可通过构建选项输出压缩版 CSS。

在 `.zest.fsx` 中可用 `stylesheet { }` 计算表达式编写样式并以 `styleZcss` 注入，或用 `styleExternal "css/theme.zcss"` 引用外部文件（见 [Zest DSL 指南](/zh/posts/dsl-guide/)）。
