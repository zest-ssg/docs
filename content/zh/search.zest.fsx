// @title 搜索
// @layout default
// @description 在文档全站进行静态全文搜索——无需服务器。
//
// 索引由 base 布局以构建期 JSON（`window.__DOC_INDEX__`）注入到每个页面；
// 下方的纯 JS 过滤器完全在浏览器内运行。

open Zest.Dsl

// Content-page scripts have no `page` binding (only layouts do), so the
// locale is fixed here — this file lives under content/zh/.
let langCode = "zh"

render [
    h1 [ text (t_lang "nav.search" langCode) ]
    pC "tags-page__lead" [ text (t_lang "search.lead" langCode) ]
    divC "search-page" [
        voidElem "input" [ attr "type" "search"
                           attr "name" "q"
                           attr "class" "search-page__input"
                           attr "placeholder" (t_lang "search.placeholder" langCode)
                           attr "aria-label" (t_lang "search.placeholder" langCode)
                           attr "autocomplete" "off" ]
        divC "search-results" []
    ]
    js """
        (function () {
          const index = window.__DOC_INDEX__ || [];
          const input = document.querySelector('.search-page__input');
          const results = document.querySelector('.search-results');
          if (!input || !results) return;
          const esc = (s) => String(s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
          const render = (list) => {
            if (!list.length) {
              results.innerHTML = '<p class="term-list__meta">' + esc('No results') + '</p>';
              return;
            }
            results.innerHTML = list.map((p) =>
              '<article class="search-result">'
                + '<h3 class="search-result__title"><a href="' + esc(p.url) + '">' + esc(p.title) + '</a></h3>'
                + '<div class="search-result__meta">' + esc(p.group || '') + '</div>'
                + (p.description ? '<p class="search-result__meta">' + esc(p.description) + '</p>' : '')
                + '</article>'
            ).join('');
          };
          const search = () => {
            const q = input.value.trim().toLowerCase();
            if (!q) { results.innerHTML = ''; return; }
            const terms = q.split(/\s+/);
            const hits = index.filter((p) => {
              const haystack = (p.title + ' ' + (p.group || '') + ' ' + (p.description || '')).toLowerCase();
              return terms.every((t) => haystack.indexOf(t) !== -1);
            });
            render(hits);
          };
          input.addEventListener('input', search);
          input.addEventListener('search', search); // native clear button
        })();
    """
]
