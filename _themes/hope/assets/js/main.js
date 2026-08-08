// main.js
//
// Client-side behaviour for the oxygen docs theme:
//   - theme toggle (light / dark, persisted, respects prefers-color-scheme)
//   - on-this-page TOC built from the article headings with scroll-spy
//   - header search against the build-time JSON index
//   - mobile sidebar drawer, back-to-top button, copy buttons on <pre>
//
// The page region swaps via PJAX, so every initializer runs again on
// `pjax:end` (guarded against duplicate listeners and stale nodes).

(function () {
  'use strict';

  var THEME_KEY = 'zest-theme';

  // ── Theme ──────────────────────────────────────────────────────────

  function applyTheme(theme) {
    document.documentElement.dataset.theme = theme;
    try { localStorage.setItem(THEME_KEY, theme); } catch (e) { /* private mode */ }
    // Sun/moon icons are swapped by CSS (html[data-theme]); the button is
    // left untouched so the inline SVGs from the layout stay in place.
  }

  function initTheme() {
    var btn = document.querySelector('[data-theme-toggle]');
    if (!btn) return;
    btn.addEventListener('click', function () {
      var next = document.documentElement.dataset.theme === 'dark' ? 'light' : 'dark';
      applyTheme(next);
    });
    // Keep the icon in sync if the toggle button survived a PJAX swap.
    applyTheme(document.documentElement.dataset.theme || 'light');
  }

  // ── TOC ────────────────────────────────────────────────────────────

  function buildToc() {
    var nav = document.querySelector('.toc__body');
    if (!nav) return;
    nav.innerHTML = '';

    var article = document.querySelector('.content-body');
    if (!article) return;

    var heads = Array.prototype.slice.call(article.querySelectorAll('h2, h3'));
    if (!heads.length) return;

    // Ensure stable fragment ids for headings the Markdown renderer missed.
    var used = {};
    heads.forEach(function (h, i) {
      if (!h.id) {
        var base = (h.textContent || 'section-' + i)
          .toLowerCase().replace(/[^\w\u4e00-\u9fa5-]+/g, '-').replace(/^-+|-+$/g, '');
        var id = base || 'section-' + i;
        var n = used[id] || 0;
        used[id] = n + 1;
        h.id = n ? id + '-' + n : id;
      }
    });

    var ul = document.createElement('ul');
    ul.className = 'toc__list';
    nav.appendChild(ul);
    var parent = { el: ul, level: 2 };

    heads.forEach(function (h) {
      var level = Number(h.tagName[1]);
      while (level > parent.level) {
        var nested = document.createElement('ul');
        nested.className = 'toc__list toc__list--nested';
        parent.el.appendChild(nested);
        parent = { el: nested, level: level };
      }
      while (level < parent.level && parent !== rootLevel()) { /* not tracked */ }
      var li = document.createElement('li');
      var a = document.createElement('a');
      a.href = '#' + h.id;
      a.className = 'toc__link';
      a.textContent = h.textContent;
      li.appendChild(a);
      parent.el.appendChild(li);
    });

    function rootLevel() { return { el: ul, level: 2 }; }

    // Scroll-spy: highlight the heading currently near the viewport top.
    var links = nav.querySelectorAll('.toc__link');
    var map = [];
    heads.forEach(function (h) {
      map.push({ head: h, link: null });
    });
    links.forEach(function (a) {
      var id = a.getAttribute('href').slice(1);
      map.forEach(function (m) { if (m.head.id === id) m.link = a; });
    });

    var active = null;
    function onScroll() {
      var top = window.pageYOffset + 96; // offset for the sticky header
      var current = null;
      for (var i = 0; i < map.length; i++) {
        if (map[i].head.getBoundingClientRect().top + window.pageYOffset <= top) current = map[i];
      }
      if (current !== active) {
        if (active && active.link) active.link.classList.remove('is-active');
        active = current;
        if (active && active.link) active.link.classList.add('is-active');
      }
    }
    window.addEventListener('scroll', onScroll, { passive: true });
    onScroll();
  }

  // ── Copy buttons on code blocks ────────────────────────────────────

  function initCopyButtons() {
    document.querySelectorAll('pre').forEach(function (pre) {
      if (pre.querySelector('.code-copy')) return;
      var btn = document.createElement('button');
      btn.className = 'code-copy';
      btn.type = 'button';
      btn.textContent = 'Copy';
      btn.addEventListener('click', function () {
        var text = pre.innerText.replace(/\n$/, '');
        if (navigator.clipboard && navigator.clipboard.writeText) {
          navigator.clipboard.writeText(text).then(flash, function () { flash(); });
        } else {
          flash();
        }
        function flash() {
          btn.textContent = 'Copied';
          setTimeout(function () { btn.textContent = 'Copy'; }, 1500);
        }
      });
      pre.appendChild(btn);
    });
  }

  // ── Header search ──────────────────────────────────────────────────

  function initSearch() {
    var input = document.querySelector('.header-search__input');
    var box = document.querySelector('.header-search__results');
    if (!input || !box) return;
    var index = window.__DOC_INDEX__ || [];

    function esc(s) {
      return String(s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
        .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    function render(list) {
      box.innerHTML = '';
      if (!list.length) {
        var empty = document.createElement('div');
        empty.className = 'header-search__empty';
        empty.textContent = box.getAttribute('data-empty') || 'No results';
        box.appendChild(empty);
        return;
      }
      list.slice(0, 8).forEach(function (p) {
        var a = document.createElement('a');
        a.className = 'header-search__item';
        a.href = p.url;
        var g = document.createElement('div');
        g.className = 'header-search__item-group';
        g.textContent = p.group || '';
        var t = document.createElement('div');
        t.className = 'header-search__item-title';
        t.textContent = p.title;
        a.appendChild(g);
        a.appendChild(t);
        box.appendChild(a);
      });
    }

    function search() {
      var q = input.value.trim().toLowerCase();
      if (!q) {
        box.classList.remove('is-open');
        return;
      }
      var terms = q.split(/\s+/);
      var hits = index.filter(function (p) {
        var hay = (p.title + ' ' + (p.group || '') + ' ' + (p.description || '')).toLowerCase();
        return terms.every(function (t) { return hay.indexOf(t) !== -1; });
      });
      box.classList.add('is-open');
      render(hits);
    }

    input.addEventListener('input', search);
    input.addEventListener('focus', function () { if (input.value.trim()) search(); });
    document.addEventListener('click', function (e) {
      if (!e.target.closest('.header-search')) box.classList.remove('is-open');
    });
  }

  // ── Sidebar drawer (mobile) ────────────────────────────────────────

  function initDrawer() {
    var toggle = document.querySelector('.sidebar-toggle');
    var sidebar = document.querySelector('.sidebar');
    var backdrop = document.querySelector('.sidebar-backdrop');
    if (!toggle || !sidebar) return;
    function close() {
      sidebar.classList.remove('is-open');
      if (backdrop) backdrop.classList.remove('is-open');
    }
    toggle.addEventListener('click', function () {
      sidebar.classList.toggle('is-open');
      if (backdrop) backdrop.classList.toggle('is-open');
    });
    if (backdrop) backdrop.addEventListener('click', close);
    sidebar.addEventListener('click', function (e) {
      if (e.target.closest('a')) close();
    });
  }

  // ── Back to top ────────────────────────────────────────────────────

  function initBackToTop() {
    var btn = document.querySelector('.back-to-top');
    if (!btn) return;
    function onScroll() {
      btn.classList.toggle('is-visible', window.pageYOffset > 480);
    }
    btn.addEventListener('click', function () {
      window.scrollTo({ top: 0, behavior: 'smooth' });
    });
    window.addEventListener('scroll', onScroll, { passive: true });
    onScroll();
  }

  // ── Boot ───────────────────────────────────────────────────────────

  function init() {
    initTheme();
    buildToc();
    initCopyButtons();
    initSearch();
    initDrawer();
    initBackToTop();
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }

  // PJAX swaps only the page region; re-run everything that touches it.
  var reInitTimer = null;
  document.addEventListener('pjax:end', function () {
    clearTimeout(reInitTimer);
    reInitTimer = setTimeout(init, 0);
  });
})();
