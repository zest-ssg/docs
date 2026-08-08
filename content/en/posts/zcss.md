+++
title = "ZCSS"
description = "Zest's SCSS-like preprocessor: @use modules, variables, nesting, color functions and build-time compilation."
category = "styling"
tags = ["zest", "zcss", "css", "styling"]
date = 2026-08-01
+++

# ZCSS

ZCSS is the SCSS-like CSS preprocessor built into Zest. `.zcss` files are compiled to plain `.css` during the build — no external toolchain required. This page is the **Styling** section of the documentation and covers the language, the built-in modules, and how the compiler fits into a build.

## Syntax Modes

ZCSS supports three syntax modes, detected automatically.

**Brace mode** (SCSS-style with `{}`):

```scss
$primary: #4a90d9;

body {
  font-family: 'Inter', sans-serif;
  background: #fff;
  color: #333;
}

a {
  color: $primary;
  &:hover { color: darken($primary, 10%); }
}
```

**Indent mode** (Python-style) — used when selector lines contain no `{`:

```scss
$primary: #4a90d9

body
  font-family: 'Inter', sans-serif
  color: #333

a
  color: $primary
  &:hover
    color: darken($primary, 10%)
```

**Bracket mode** (F#-style) — used when a block is opened by a whitespace-prefixed `[`. Brackets are equivalent to `{ }`, and F#-flavored declarations are accepted:

```scss
let primary = #4f46e5
let primaryDark = primary |> darken(8%)

.btn [
  color = white
  background = primary
  &:hover [
    background = primaryDark
  ]
]
```

Only brackets preceded by whitespace and followed by whitespace/end-of-line count as blocks, so attribute selectors such as `a[href]` are never mistaken for block openers.

## Variables and Nesting

```scss
$primary: #4a90d9 !default;   // !default: set only if not already defined
$spacing: 1rem;

.container {
  max-width: 1200px;
  padding: $spacing;
  .item { margin-bottom: $spacing; }
}
```

The `&` parent selector works in every mode, and `@media`, `@if`/`@else`, `@each`, and `@for` are supported:

```scss
@if $theme == "dark" {
  body { background: #111; color: #eee; }
} @else {
  body { background: #fff; color: #333; }
}

@each $color in (red, green, blue) {
  .text-$color { color: $color; }
}
```

## Color Functions

All color functions operate on hex colors and are the primary tool for building palettes:

| Function | Example |
|---|---|
| `lighten(color, pct)` / `darken(color, pct)` | `darken($primary, 10%)` |
| `alpha(color, a)` / `transparentize(color, a)` | `alpha(#000, 0.5)` |
| `mix(c1, c2, pct)` | `mix(#fff, #000, 50%)` |
| `complement(color)` / `invert(color)` / `grayscale(color)` | `grayscale(#f00)` |
| `saturate` / `desaturate` / `adjustHue` | `adjustHue(#f00, 180)` |
| `tint` / `shade` | `tint(#f00, 30%)` |
| `rgba` / `rgb` / `hsl` / `hsla` / `scaleColor` | `rgba(255, 0, 0, 0.5)` |

Bracket mode adds `|>` as a color pipeline — `color |> fn(args)` rewrites to `fn(color, args)`:

```scss
let heroAccent = primary |> lighten(22%)
```

## Math and Functions

Arithmetic is evaluated **only inside `calc(...)`**, matching native CSS and avoiding mis-parses of values like `font: 16px/1.65`:

```scss
.container {
  width: calc($max-width - 2 * $spacing);
  padding: calc($spacing / 2);
}
```

Unit-preserving arithmetic is pre-computed; unit-incompatible `calc()` expressions (for example `calc(100% - 2rem)`) pass through for the browser to evaluate. More than 50 CSS function names (`calc`, `clamp`, `min`, `max`, `color-mix`, gradients, transforms) are recognized and never resolved as variables. Built-in functions include `unit`, `unitless`, `percentage`, `to-upper`, `to-lower`, `str-length`, `list-nth`, and `type-of`.

## Mixins, Extend and Apply

```scss
@mixin card($bg: #fff, $radius: 8px) {
  background: $bg;
  border-radius: $radius;
  padding: 1rem;
}

.featured-card { @include card(#fef3c7, 12px); }

.error { color: red; font-weight: bold; }
.fatal-error { @extend .error; font-size: 1.2rem; }

.card { @apply shadow rounded; padding: 1rem; }
```

Mixins also support `@content` blocks for wrapping media queries.

## Responsive Shorthand

```scss
@md {
  .container { max-width: 720px; }
}
@lg {
  .container { max-width: 960px; }
}
```

Breakpoint shorthands: `@sm`, `@md`, `@lg`, `@xl`, `@2xl`.

## Modules — `@use`

```scss
@use "zest:reset";
@use "zest:utilities";
@import "variables";
```

| Module | Contents |
|---|---|
| `zest:reset` | Minimal CSS reset (box-sizing, margin/padding) |
| `zest:utilities` | Utility classes: display, flex, grid, spacing, text, colors, shadow, transition |
| `zest:palette` | Semantic color variables (all `!default`, overridable) |
| `zest:animations` | Keyframe animations and timing variants |
| `zest:gradients` | Gradient direction and preset classes |
| `zest:filters` | Filter, backdrop-filter, transform utilities |
| `zest:layout` | Container, aspect-ratio, object-fit, column utilities |
| `zest:composition` | Composable layout groups (stack, cluster, sidebar, centered) |
| `zest:all` | Everything above |

## Compiler Behavior

- **Auto vendor prefixes** — 31 common properties are prefixed (`display: flex` also emits `-webkit-box` and `-ms-flexbox`).
- **Minification** — available via the build's minification option.
- **Errors** — malformed input is caught gracefully and reported as a `/* ZCSS ERROR: … */` comment instead of failing the build; compile errors carry source positions (`Error at L12:C5`).
- **Caching** — ZCSS processing is pure, so results are cached by content hash and served from cache during dev-server rebuilds when a `.zcss` file is unchanged.
- **Type checking** — a validator checks property values against expected CSS types for ~175 known properties and reports mismatches as warnings.

## Usage in a Project

`.zcss` files under `assets/css/` compile to `.css` automatically:

```
assets/css/
├── main.zcss      ← compiled to _site/assets/css/main.css
└── components/
    └── buttons.zcss
```

The `stylesheet { }` F# computation expression in `DslCss` compiles the same rules inline from `.zest.fsx` pages. See the [DSL API reference](/en/posts/dsl-api/) for the styling modules, and the [features overview](/en/posts/overview/) for where styling fits in the build pipeline.
