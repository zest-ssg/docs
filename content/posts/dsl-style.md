# CSS DSL (`stylesheet { }`) Reference

The CSS DSL (`DslCss` module) provides an F# computation expression for writing CSS using F#-native syntax. It is available in `.zest.fsx` scripts via `open DslCss`.

## Basic Usage

```fsharp
open DslCss
open DslCss.Selectors  // for pre-defined HTML element selectors

let myCss = stylesheet {
    body [ bg "#f0f0f0"; color "#333"; font_family "'Inter', sans-serif" ]
    a.hover [ color "#0ff" ]
    cls "container" [ max_width "1200px"; margin "0 auto" ]

    h1 [ font_size "2rem"; font_weight "bold" ]
    h2 [ font_size "1.5rem" ]
    p  [ line_height "1.6" ]
}

// Use in a page:
page {
    title "Styled Page"
    styleZcss myCss
    // or: styleFromZcss rules
}
```

## Core Types

### `CssDecl`

```fsharp
type CssDecl = {
    Property : string   // CSS property name, e.g. "background", "color"
    Value   : string    // CSS property value, e.g. "#000", "16px"
}
```

### `CssRule`

```fsharp
type CssRule = {
    Selector     : string
    Declarations : CssDecl list
}
```

### `Sel` (Selector)

```fsharp
type Sel(name: string)
```

A `Sel` wraps a CSS selector string and supports dot-notation for pseudo-classes, pseudo-elements, attribute selectors, and combinators. Calling a `Sel` with a `CssDecl list` produces a `CssRule`.

```fsharp
// Selector + declarations = rule
body [ bg "#fff" ]   // → body { background: #fff; }
```

## `stylesheet { }` Computation Expression

| Method | Signature | Description |
|---|---|---|
| `Yield(rule: CssRule)` | `CssRule list` | Yield a single rule |
| `Yield(rules: CssRule list)` | `CssRule list` | Yield multiple rules |
| `Combine` | `CssRule list -> CssRule list -> CssRule list` | Concatenation (`@`) |
| `Delay` | `(unit -> CssRule list) -> (unit -> CssRule list)` | Lazy evaluation |
| `Zero` | `CssRule list` | Empty list |
| `For` | `'a seq -> ('a -> CssRule list) -> CssRule list` | Iteration |
| `Run` | `CssRule list -> string` | Compile to CSS string |

## Compilation Functions

| Function | Signature | Description |
|---|---|---|
| `compileStylesheet` | `CssRule list -> string` | Compile to pretty-printed CSS |
| `compileStylesheetMinified` | `CssRule list -> string` | Compile to single-line minified CSS |
| `compileStylesheetWithDiagnostics` | `CssRule list -> CssCompileResult` | Compile with warnings for empty rules |

### `CssCompileResult`

```fsharp
type CssCompileResult = {
    Css: string
    Warnings: string list
}
```

---

## CSS Property Functions

Each property is a function `string -> CssDecl` (value → declaration).

### Background

| Function | Property |
|---|---|
| `bg` | `background` |
| `bg_color` | `background-color` |
| `bg_image` | `background-image` |
| `bg_repeat` | `background-repeat` |
| `bg_position` | `background-position` |
| `bg_size` | `background-size` |
| `bg_attachment` | `background-attachment` |
| `bg_clip` | `background-clip` |
| `bg_origin` | `background-origin` |
| `bg_blend_mode` | `background-blend-mode` |

### Color & Text

| Function | Property | Function | Property |
|---|---|---|---|
| `color` | `color` | `opacity` | `opacity` |

### Typography

| Function | Property | Function | Property |
|---|---|---|---|
| `font_family` | `font-family` | `font_size` | `font-size` |
| `font_weight` | `font-weight` | `font_style` | `font-style` |
| `font_variant` | `font-variant` | `font_stretch` | `font-stretch` |
| `line_height` | `line-height` | `letter_spacing` | `letter-spacing` |
| `word_spacing` | `word-spacing` | `text_align` | `text-align` |
| `text_decoration` | `text-decoration` | `text_transform` | `text-transform` |
| `text_indent` | `text-indent` | `text_overflow` | `text-overflow` |
| `text_shadow` | `text-shadow` | `text_wrap` | `text-wrap` |
| `white_space` | `white-space` | `word_break` | `word-break` |
| `overflow_wrap` | `overflow-wrap` | `hyphens` | `hyphens` |
| `vertical_align` | `vertical-align` | | |

### Box Model

| Function | Property | Function | Property |
|---|---|---|---|
| `width` | `width` | `height` | `height` |
| `min_width` | `min-width` | `max_width` | `max-width` |
| `min_height` | `min-height` | `max_height` | `max-height` |
| `margin` | `margin` | `margin_top` | `margin-top` |
| `margin_right` | `margin-right` | `margin_bottom` | `margin-bottom` |
| `margin_left` | `margin-left` | `padding` | `padding` |
| `padding_top` | `padding-top` | `padding_right` | `padding-right` |
| `padding_bottom` | `padding-bottom` | `padding_left` | `padding-left` |
| `box_sizing` | `box-sizing` | `box_shadow` | `box-shadow` |

### Border

| Function | Property | Function | Property |
|---|---|---|---|
| `border` | `border` | `border_top` | `border-top` |
| `border_right` | `border-right` | `border_bottom` | `border-bottom` |
| `border_left` | `border-left` | `border_color` | `border-color` |
| `border_width` | `border-width` | `border_style` | `border-style` |
| `border_radius` | `border-radius` | `outline` | `outline` |
| `outline_color` | `outline-color` | `outline_width` | `outline-width` |
| `outline_style` | `outline-style` | `outline_offset` | `outline-offset` |

Individual border-radius corners: `border_top_left_radius`, `border_top_right_radius`, `border_bottom_left_radius`, `border_bottom_right_radius`.

### Display & Positioning

| Function | Property | Function | Property |
|---|---|---|---|
| `display` | `display` | `position` | `position` |
| `top` | `top` | `right` | `right` |
| `bottom` | `bottom` | `left` | `left` |
| `z_index` | `z-index` | `float_` | `float` |
| `clear` | `clear` | `overflow` | `overflow` |
| `overflow_x` | `overflow-x` | `overflow_y` | `overflow-y` |
| `visibility` | `visibility` | `object_fit` | `object-fit` |
| `object_position` | `object-position` | `aspect_ratio` | `aspect-ratio` |

### Flexbox

| Function | Property | Function | Property |
|---|---|---|---|
| `flex` | `flex` | `flex_direction` | `flex-direction` |
| `flex_wrap` | `flex-wrap` | `flex_flow` | `flex-flow` |
| `flex_grow` | `flex-grow` | `flex_shrink` | `flex-shrink` |
| `flex_basis` | `flex-basis` | `justify_content` | `justify-content` |
| `align_items` | `align-items` | `align_content` | `align-content` |
| `align_self` | `align-self` | `justify_items` | `justify-items` |
| `justify_self` | `justify-self` | `order_` | `order` |
| `gap` | `gap` | `row_gap` | `row-gap` |
| `column_gap` | `column-gap` | `place_items` | `place-items` |
| `place_content` | `place-content` | `place_self` | `place-self` |

### Grid

| Function | Property | Function | Property |
|---|---|---|---|
| `grid` | `grid` | `grid_template_columns` | `grid-template-columns` |
| `grid_template_rows` | `grid-template-rows` | `grid_template_areas` | `grid-template-areas` |
| `grid_template` | `grid-template` | `grid_auto_columns` | `grid-auto-columns` |
| `grid_auto_rows` | `grid-auto-rows` | `grid_auto_flow` | `grid-auto-flow` |
| `grid_column` | `grid-column` | `grid_row` | `grid-row` |
| `grid_column_start` | `grid-column-start` | `grid_column_end` | `grid-column-end` |
| `grid_row_start` | `grid-row-start` | `grid_row_end` | `grid-row-end` |
| `grid_area` | `grid-area` | | |

### Transform & Transition

| Function | Property |
|---|---|
| `transform` | `transform` |
| `transform_origin` | `transform-origin` |
| `transition` | `transition` |
| `transition_duration` | `transition-duration` |
| `transition_property` | `transition-property` |
| `transition_timing` | `transition-timing-function` |
| `transition_delay` | `transition-delay` |

### Animation

| Function | Property |
|---|---|
| `animation` | `animation` |
| `animation_name` | `animation-name` |
| `animation_duration` | `animation-duration` |
| `animation_timing` | `animation-timing-function` |
| `animation_delay` | `animation-delay` |
| `animation_iteration` | `animation-iteration-count` |
| `animation_direction` | `animation-direction` |
| `animation_fill_mode` | `animation-fill-mode` |
| `animation_play_state` | `animation-play-state` |

### Filters & Effects

| Function | Property |
|---|---|
| `filter` | `filter` |
| `backdrop_filter` | `backdrop-filter` |
| `clip_path` | `clip-path` |
| `mix_blend_mode` | `mix-blend-mode` |
| `isolation_` | `isolation` |

### Cursor & Interaction

| Function | Property |
|---|---|
| `cursor` | `cursor` |
| `pointer_events` | `pointer-events` |
| `user_select` | `user-select` |
| `resize` | `resize` |
| `caret_color` | `caret-color` |
| `scroll_behavior` | `scroll-behavior` |
| `scrollbar_width` | `scrollbar-width` |
| `scrollbar_color` | `scrollbar-color` |

### Lists & Tables

`list_style`, `list_style_type`, `list_style_position`, `list_style_image`, `counter_reset`, `counter_increment`, `counter_set`, `table_layout`, `border_collapse`, `border_spacing`, `caption_side`, `empty_cells`.

### Content & Print

`content_`, `quotes`, `page_break_before`, `page_break_after`, `page_break_inside`.

### Modern CSS

`will_change`, `contain`, `contain_intrinsic_size`, `content_visibility`.

### Custom Properties

| Function | Signature | Description |
|---|---|---|
| `var_` | `name: string -> value: string -> CssDecl` | CSS custom property `--name: value` |
| `prop` | `name: string -> value: string -> CssDecl` | Explicit property name (for unrecognized properties) |

### Validation

| Function | Signature | Description |
|---|---|---|
| `validateValue` | `propName: string -> value: string -> string option` | Lightweight value format check; returns warning or `None` |

---

## Selectors (`DslCss.Selectors`)

Open explicitly: `open DslCss.Selectors`

### Pre-defined HTML Element Selectors

All standard HTML elements are available as `Sel` instances: `body`, `div`, `span`, `a`, `p`, `h1`-`h6`, `header`, `footer`, `nav`, `main`, `section`, `article`, `aside`, `ul`, `ol`, `li`, `table`, `thead`, `tbody`, `tr`, `th`, `td`, `img`, `input`, `button`, `form`, `label`, `select`, `textarea`, `pre`, `code`, `blockquote`, `figure`, `figcaption`, `details`, `summary`, `dialog`, `video`, `audio`, `canvas`, `iframe`, etc.

### ID and Class Selectors

| Function | Signature | Description |
|---|---|---|
| `cls` | `name: string -> Sel` | `.className` |
| `id` | `name: string -> Sel` | `#idName` |
| `attr_sel` | `name: string -> Sel` | `[attr]` |
| `selectors` | `sels: Sel list -> Sel` | Comma-separated selectors, e.g. `h1, h2, h3` |
| `raw_sel` | `selector: string -> Sel` | Arbitrary raw selector string |

---

## Pseudo-Class Dot Notation

`Sel` supports dot-notation for CSS pseudo-classes:

| Member | CSS | Member | CSS |
|---|---|---|---|
| `.hover` | `:hover` | `.active` | `:active` |
| `.focus` | `:focus` | `.visited` | `:visited` |
| `.checked_` | `:checked` | `.disabled` | `:disabled` |
| `.enabled` | `:enabled` | `.required` | `:required` |
| `.optional` | `:optional` | `.read_only` | `:read-only` |
| `.read_write` | `:read-write` | `.valid` | `:valid` |
| `.invalid` | `:invalid` | `.default_` | `:default` |
| `.in_range` | `:in-range` | `.out_of_range` | `:out-of-range` |
| `.placeholder_shown` | `:placeholder-shown` | `.autofill` | `:autofill` |
| `.target` | `:target` | `.root_` | `:root` |
| `.empty` | `:empty` | `.blank` | `:blank` |
| `.first_child` | `:first-child` | `.last_child` | `:last-child` |
| `.only_child` | `:only-child` | `.first_of_type` | `:first-of-type` |
| `.last_of_type` | `:last-of-type` | `.only_of_type` | `:only-of-type` |

### Parameterized pseudo-classes

| Member | CSS |
|---|---|
| `.nth_child(n)` | `:nth-child(n)` |
| `.nth_last_child(n)` | `:nth-last-child(n)` |
| `.nth_of_type(n)` | `:nth-of-type(n)` |
| `.nth_last_of_type(n)` | `:nth-last-of-type(n)` |
| `.not_(sel)` | `:not(sel)` |
| `.lang(code)` | `:lang(code)` |
| `.is_(sel)` | `:is(sel)` |
| `.where_(sel)` | `:where(sel)` |
| `.has_(sel)` | `:has(sel)` |

```fsharp
a.hover [ color "#0ff" ]
li.nth_child(2) [ bg "#eee" ]
div.not_(".excluded") [ display "block" ]
div.has_("img") [ padding "1rem" ]
```

---

## Pseudo-Element Dot Notation

| Member | CSS |
|---|---|
| `.before` | `::before` |
| `.after` | `::after` |
| `.first_letter` | `::first-letter` |
| `.first_line` | `::first-line` |
| `.selection` | `::selection` |
| `.placeholder` | `::placeholder` |
| `.backdrop` | `::backdrop` |
| `.marker` | `::marker` |
| `.spelling_error` | `::spelling-error` |
| `.grammar_error` | `::grammar-error` |

```fsharp
p.first_letter [ font_size "2em" ]
input.placeholder [ color "#999" ]
```

---

## Attribute Selectors

| Member | CSS |
|---|---|
| `.attr(name)` | `[name]` |
| `.attr_eq(name, value)` | `[name="value"]` |
| `.attr_contains(name, value)` | `[name~="value"]` |
| `.attr_dash(name, value)` | `[name\|="value"]` |
| `.attr_starts(name, value)` | `[name^="value"]` |
| `.attr_ends(name, value)` | `[name$="value"]` |
| `.attr_substr(name, value)` | `[name*="value"]` |

```fsharp
(input.attr_eq "type" "text") [ border "1px solid #ccc" ]
(a.attr_starts "href" "https") [ color "green" ]
```

---

## Combinators

| Member | CSS |
|---|---|
| `.descendant(child: Sel)` | ` ` (space) |
| `.child(child: Sel)` | ` > ` |
| `.adjacent(sib: Sel)` | ` + ` |
| `.sibling(sib: Sel)` | ` ~ ` |

```fsharp
(div.descendant p) [ margin "0" ]
(ul.child li) [ list_style "none" ]
(h1.adjacent p) [ margin_top "0" ]
```

---

## At-Rule Functions

### `media`

```fsharp
media (query: string) (rules: CssRule list) : string
```

```fsharp
let mobileStyles = media "(max-width: 768px)" [
    body [ font_size "14px" ]
    cls "container" [ padding "1rem" ]
]
```

### `keyframes`

```fsharp
keyframes (name: string) (frames: (string * CssDecl list) list) : string
```

```fsharp
let fadeIn = keyframes "fadeIn" [
    "from", [ opacity "0" ]
    "to",   [ opacity "1" ]
]
```

### `supports`

```fsharp
supports (condition: string) (rules: CssRule list) : string
```

```fsharp
let gridStyles = supports "(display: grid)" [
    cls "grid-container" [ display "grid" ]
]
```

### `fontFace`

```fsharp
fontFace (decls: CssDecl list) : string
```

```fsharp
let inter = fontFace [
    prop "font-family" "\"Inter\""
    prop "src" "url('/fonts/inter.woff2') format('woff2')"
    prop "font-display" "swap"
]
```

### `cssImport` / `cssImportMedia`

```fsharp
cssImport (url: string) : string
cssImportMedia (url: string) (mediaQuery: string) : string
```

```fsharp
cssImport "url('/css/reset.css')"
cssImportMedia "url('/css/print.css')" "print"
```
