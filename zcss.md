# ZCSS Preprocessor

ZCSS is a SCSS-like CSS preprocessor built into Zest. `.zcss` files are compiled to `.css` during the build. ZCSS supports three syntax modes: **brace mode** (CSS/SCSS style with `{}`), **indent mode** (Python-style indentation), and **bracket mode** (F#-style with `[ ]`).

## Syntax Modes

### Brace Mode (SCSS-like)

```scss
// variables.zcss
$primary: #4a90d9;
$font-stack: 'Inter', sans-serif;

body {
  font-family: $font-stack;
  background: #fff;
  color: #333;
}

a {
  color: $primary;
  &:hover {
    color: darken($primary, 10%);
  }
}
```

### Indent Mode (Python-like)

Detection is automatic — if the file contains no `{` at the end of selector lines, indent mode is used.

```scss
$primary: #4a90d9

body
  font-family: 'Inter', sans-serif
  background: #fff
  color: #333

a
  color: $primary
  &:hover
    color: darken($primary, 10%)
```

### Bracket Mode (F#-style)

Detection is automatic — if the file has **no `{`** but contains a block opened by a
**whitespace-prefixed `[`** (e.g. `selector [ … ]`), bracket mode is used. In this mode
the square brackets are treated as **equivalent to `{ }`** for block delimiters and are
rewritten to braces before reuse of the brace parser, so every feature available in brace
mode (nesting, `&`, `@media`, `@if`, etc.) works identically.

> **Attribute-selector safety:** only brackets *preceded by whitespace* (or at line start)
> and *followed by whitespace / end-of-line* are treated as blocks. `a[href]` is therefore
> left untouched and never mistaken for a block opener.

Bracket mode also accepts F#-flavored declarations:

- `let name = value` — define a variable (equivalent to `$name: value`).
- `prop = value` — property declarations may use `=` instead of `:` (`:` still works).

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

#### Color pipes (`|>`)

`|>` is supported as a color pipeline: `color |> fn(args)` rewrites to `fn(color, args)`,
so you can chain the [color functions](#color-functions) above:

```scss
let heroAccent = primary |> lighten(22%)
.card [
  border-color = primary |> transparentize(0.3)
]
```

> **Pitfall:** the pipe is split on the whole `|>` token and does **not** understand
> nested parentheses. Do **not** place a `|>` inside a function argument, e.g.
> `linear-gradient(135deg, a, b |> lighten(10%))` will be misparsed. Instead precompute the
> value with its own `let` binding first (as shown with `heroAccent` above).

---

## Features

### Variables

```scss
$primary: #4a90d9 !default;  // !default: only set if not already defined
$spacing: 1rem;
$max-width: 1200px;

.container {
  max-width: $max-width;
  padding: $spacing;
}
```

### Let Bindings (Local Variables)

```scss
.header {
  let $bg: #333;
  background: $bg;
  color: contrast($bg);
}
```

### Color Functions

All color functions operate on hex colors:

| Function | Signature | Description |
|---|---|---|
| `lighten(color, pct)` | `#lighten(#333, 10%)` | Lighten by percentage |
| `darken(color, pct)` | `#darken(#fff, 10%)` | Darken by percentage |
| `alpha(color, a)` | `#alpha(#000, 0.5)` | Set alpha (0.0–1.0) |
| `mix(c1, c2, pct)` | `#mix(#fff, #000, 50%)` | Mix two colors |
| `complement(color)` | `#complement(#f00)` | Complementary color |
| `grayscale(color)` | `#grayscale(#f00)` | Convert to grayscale |
| `invert(color)` | `#invert(#000)` | Invert (alias for complement) |
| `saturate(color, pct)` | `#saturate(#f00, 20%)` | Increase saturation |
| `desaturate(color, pct)` | `#desaturate(#f00, 20%)` | Decrease saturation |
| `adjustHue(color, deg)` | `#adjustHue(#f00, 180)` | Rotate hue by degrees |
| `tint(color, pct)` | `#tint(#f00, 30%)` | Mix with white |
| `shade(color, pct)` | `#shade(#f00, 30%)` | Mix with black |
| `transparentize(color, a)` | `#transparentize(#f00, 0.3)` | Add transparency |
| `rgba(r, g, b, a)` | `#rgba(255, 0, 0, 0.5)` | RGBA color |
| `rgb(r, g, b)` | `#rgb(255, 0, 0)` | RGB color |
| `hsl(h, s, l)` | `#hsl(0, 100%, 50%)` | HSL color |
| `hsla(h, s, l, a)` | `#hsla(0, 100%, 50%, 0.5)` | HSLA color |
| `scaleColor(color, satPct, lightPct)` | `#scaleColor(#f00, -10%, +5%)` | Adjust saturation and lightness |

### Math Expressions

Math is evaluated **only inside an explicit `calc(...)` wrapper** — matching
native CSS, where `calc()` is the sole scope for arithmetic and avoids
mis-parsing CSS shorthand separators (e.g. the `/` in `font: 16px/1.65`)
as operators. Every other value is passed through untouched.

```scss
.container {
  width: calc($max-width - 2 * $spacing);
  padding: calc($spacing / 2);
  margin: calc(($spacing * 2) + 1rem);
}
```

Unit-preserving arithmetic: same-unit operations keep the unit, mixed units use the first operand's unit.

### Built-in Functions

| Function | Description |
|---|---|
| `unit(value, unit)` | Append a unit, e.g. `unit(10, px)` → `10px` |
| `unitless(value)` | Strip unit, e.g. `unitless(10px)` → `10` |
| `percentage(value)` | Convert to percentage |
| `str-length(string)` | String character count |
| `to-upper(string)` | Uppercase |
| `to-lower(string)` | Lowercase |
| `quote(string)` | Add quotes |
| `unquote(string)` | Remove quotes |
| `list-length(list)` | Count items |
| `list-nth(list, n)` | Get nth item |
| `type-of(value)` | Return type string |
| `abs(number)` | Absolute value |
| `min(a, b)` | Minimum |
| `max(a, b)` | Maximum |

### Mixins

```scss
@mixin card($bg: #fff, $radius: 8px) {
  background: $bg;
  border-radius: $radius;
  padding: 1rem;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.featured-card {
  @include card(#fef3c7, 12px);
}

.simple-card {
  @include card();  // defaults
}
```

### Mixin with `@content`

```scss
@mixin media-query($breakpoint) {
  @media (min-width: $breakpoint) {
    @content;
  }
}

@include media-query(768px) {
  .sidebar {
    width: 300px;
  }
}
```

### `@extend`

```scss
.error {
  color: red;
  font-weight: bold;
}

.fatal-error {
  @extend .error;
  font-size: 1.2rem;
}
```

### `@apply` (Utility Classes)

Applies declarations from a named utility class:

```scss
.card {
  @apply shadow rounded;
  padding: 1rem;
}
```

### `@each` / `@for`

```scss
// Iterate over a list
@each $color in (red, green, blue) {
  .text-$color {
    color: $color;
  }
}

@each $key, $val in $theme-colors {
  .bg-$key {
    background: $val;
  }
}

// Numeric loop
@for $i from 1 through 5 {
  .col-$i {
    width: $i * 20%;
  }
}
```

### `@if` / `@else`

```scss
@if $theme == "dark" {
  body {
    background: #111;
    color: #eee;
  }
} @else {
  body {
    background: #fff;
    color: #333;
  }
}
```

### Responsive Shorthand

```scss
@sm {
  .container {
    padding: 0.5rem;
  }
}

@md {
  .container {
    max-width: 720px;
  }
}

@lg {
  .container {
    max-width: 960px;
  }
}

@xl {
  .container {
    max-width: 1200px;
  }
}

@2xl {
  .container {
    max-width: 1400px;
  }
}
```

Breakpoints: `@sm`, `@md`, `@lg`, `@xl`, `@2xl`.

### `@import` / `@use`

```scss
// Import another ZCSS file (relative path)
@import "variables";
@import "components/buttons";

// Use built-in or module
@use "zest:utilities";
@use "zest:reset";
@use "zest:palette";
@use "zest:animations";
@use "zest:gradients";
@use "zest:filters";
@use "zest:layout";
@use "zest:composition";
@use "zest:all";
```

---

## Built-in `@use` Modules

### `zest:utilities`
Comprehensive utility classes: display, flexbox, alignment, gap, margin, padding, width/height, text, font, colors, background, border, overflow, position, z-index, opacity, cursor, shadow, transition, transform, grid.

### `zest:reset`
Minimal CSS reset (box-sizing, margin/padding reset).

### `zest:palette`
Color palette with semantic color variables (all marked `!default` so you can override).

### `zest:animations`
Pre-built keyframe animations and animation utility classes with timing variants.

### `zest:gradients`
Gradient direction and preset gradient classes.

### `zest:filters`
Filter, backdrop-filter, transform scale/skew utilities.

### `zest:layout`
Container, aspect-ratio, object-fit, column-count layout utilities.

### `zest:composition`
Composable utility groups (stack, cluster, sidebar, centered) for layout composition.

### `zest:all`
Includes all of the above.

---

## Compiler Features

### Auto-Vendor-Prefix

31 common CSS properties are automatically prefixed:

```scss
.container {
  display: flex;  // → display: -webkit-box; display: -ms-flexbox; display: flex;
  user-select: none;  // → -webkit-user-select: none; -moz-user-select: none; ...
}
```

### Minification

The compiler can produce minified CSS (single line, no whitespace) via `compileStylesheetMinified` or the `--minify` build option.

### Source Positions & Errors

Each AST node carries a `SourcePos` (Line, Col). Compilation errors report exact locations:

```
Error at L12:C5: Undefined variable $primary
```

---

## Usage in Zest

### As a `.zcss` file in `assets/css/`

```
assets/css/
├── main.zcss      ← compiled to _site/assets/css/main.css
├── theme.zcss
└── components/
    └── buttons.zcss
```

### Inline in `.zest.fsx` scripts

```fsharp
page {
    title "Styled Page"

    // Compile inline ZCSS
    let styles = stylesheet {
        body [ bg "#f5f5f5"; font_family "'Inter', sans-serif" ]
        cls "card" [ padding "1rem"; border_radius "8px" ]
    }
    styleZcss styles
}
```

### Via `@use` in ZCSS files

```scss
@use "zest:reset";
@use "zest:utilities";

$primary: #3b82f6;

body {
  font-family: system-ui, sans-serif;
}

.btn {
  background: $primary;
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 4px;

  &:hover {
    background: darken($primary, 10%);
  }
}
```

### Type Checking

ZCSS includes a type checker that validates property values against expected CSS types for ~175 known properties. It detects mismatches like using a length value for a color property and reports warnings.

### Completions

ZCSS provides LSP-compatible completion data for IDE integration: 137 CSS properties, 44 common values, F#-style snippets, and 20 HTML element selectors.
