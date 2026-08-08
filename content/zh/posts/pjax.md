+++
title = "PJAX 导航"
description = "PJAX/AJAX 导航：{{ pjaxScript }} 注入、内容区替换与 pjax:end 事件。"
category = "features"
tags = ["zest", "功能", "pjax"]
date = 2026-08-01
+++

# PJAX 导航

本文属于 Features（功能）分类，介绍 Zest 内置的 PJAX（PushState + AJAX）导航脚本。PJAX 让站内导航只替换页面主体，不整页刷新，显著提升浏览体验。

## 注入方式

自包含脚本（`Zest.Engine.Resources.ZestPjax.script`）有两种注入途径：

- Nunjucks 布局：`{{ pjaxScript | safe }}`
- F# DSL 布局：`raw (siteValue "pjaxScript")`，或直接调用 `pjax_script ()`

`pjaxScript` 作为全局变量注入模板上下文，无需自己复制脚本内容。

## 行为

- **链接拦截**：只处理同源普通左键点击；带修饰键（Ctrl/Cmd/Shift/Alt）、`target="_blank"`、`download`、`mailto:`/`tel:`/`javascript:` 前缀的链接照常整页跳转。
- **内容区替换**：定位 `main` 元素（回退到 `#content`，再回退到 `body`），把响应中的对应节点 `innerHTML` 换入，同时更新 `document.title` 并 `pushState`。
- **内存缓存与预取**：最近访问的 24 个页面缓存在内存中；鼠标悬停链接 150ms 后触发预取。
- **超时与并发保护**：并发锁防止重复请求；8 秒 `AbortController` 超时，超时后回退为整页导航。
- **导航细节**：hash 锚点滚动、`popstate` 前进后退、`prefers-reduced-motion` 下跳过淡入淡出动画。

## `pjax:end` 事件

每次换页完成后，脚本在 `document` 上派发 `CustomEvent('pjax:end')`（`detail.url` 为当前地址）。由于内容区被整体替换，主题里所有绑定到该区域的初始化逻辑都要在事件后重新执行。docs 主题的 `main.js` 模式如下：

```javascript
var reInitTimer = null;
document.addEventListener('pjax:end', function () {
  clearTimeout(reInitTimer);
  reInitTimer = setTimeout(init, 0);   // 重新构建 TOC、搜索、复制按钮等
});
```

初始化器自身需要防重复（守卫重复监听器与失效节点），并在 `pjax:end` 时重新运行。
