/**
 * 紙芝居用 breadcrambにtitleを挿入するためのscript
 * 本番では不要
 */
(function ($) {
  $(function () {
    var pageTitle = $('#breadcrumb-outer h1').text();
    (window.onload = function () {
      $('.bredcrumb__list__item__page-title').text(pageTitle);
    })();
  });
})(jQuery);
