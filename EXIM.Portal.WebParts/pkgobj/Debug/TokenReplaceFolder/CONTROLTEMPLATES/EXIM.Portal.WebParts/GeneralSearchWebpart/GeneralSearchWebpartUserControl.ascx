<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneralSearchWebpartUserControl.ascx.cs" Inherits="EXIM.Portal.WebParts.GeneralSearchWebpart.GeneralSearchWebpartUserControl" %>

<div id="exim-general-search" class="exim-general-search" dir="<%= PageDir %>">

    <%-- ===== SEARCH BOX ===== --%>
    <%-- The standard SharePoint Search Box Web Part should be placed on this page.
         This web part reads the query from the URL parameter (?k=). --%>

    <%-- ===== RESULTS HEADER ===== --%>
    <div id="gsResultsHeader" class="gs-results-header" style="display:none;">
        <div class="gs-header-right">
            <h2 id="gsResultTitle" class="gs-results-title"></h2>
            <span id="gsResultCount" class="gs-result-count"></span>
        </div>
        <div class="gs-header-left">
            <%-- Sort --%>
            <div class="gs-sort-group">
                <div class="gs-sort-select">
                    <label for="gsSortSelect">
                        <i class="fa fa-sort-amount-desc"></i>
                        <%= IsArabic ? "ترتيب حسب" : "Sort by" %>
                    </label>
                    <select id="gsSortSelect" style="border:none;">
                        <option value=""><%= IsArabic ? "الأكثر صلة" : "Most Relevant" %></option>
                        <option value="LastModifiedTime:descending"><%= IsArabic ? "الأحدث" : "Newest" %></option>
                        <option value="LastModifiedTime:ascending"><%= IsArabic ? "الأقدم" : "Oldest" %></option>
                        <option value="Title:ascending"><%= IsArabic ? "أ - ي" : "A - Z" %></option>
                        <option value="Title:descending"><%= IsArabic ? "ي - أ " : "Z - A" %></option>
                    </select>
                </div>
            </div>
            <%-- Filter --%>
            <div class="gs-filter-group">
                <select id="gsFilterSelect" class="gs-filter-select btn btn-filter">
                    <option value=""><%= IsArabic ? "الكل" : "All" %></option>
                    <option value="type:page"><%= IsArabic ? "الصفحات" : "Pages" %></option>
                    <option value="type:list"><%= IsArabic ? "القوائم" : "Lists" %></option>
                    <option value="type:document"><%= IsArabic ? "المستندات" : "Documents" %></option>
                </select>
                <i class="fa fa-filter"></i>
            </div>
        </div>
    </div>

    <%-- ===== LOADING SPINNER ===== --%>
    <div id="gsLoading" class="gs-loading" style="display:none;">
        <i class="fa fa-spinner fa-spin"></i>
        <%= IsArabic ? " جاري البحث..." : " Searching..." %>
    </div>

    <%-- ===== NO RESULTS / DEFAULT MESSAGE ===== --%>
    <div id="gsNoResults"  style="display:block;">
        
        <%= IsArabic ? "لا توجد نتائج لعرضها. ابدأ بالبحث باستخدام مربع البحث أعلاه." : "No results to display. Start searching using the search box above." %>
    </div>

    <%-- ===== RESULT CARDS ===== --%>
    <div id="gsResultsList" class="gs-results-list" style="display:none;"></div>

    <%-- ===== PAGINATION ===== --%>
    <div id="gsPagination" class="page-pagination" style="display:none;">
        <nav aria-label="<%= IsArabic ? "عدد الصفحات المتاحة" : "Available pages" %>">
            <ul id="gsPaginationUl" class="pagination justify-content-center"></ul>
        </nav>
        <div id="gsPagesTotal" class="pages-total"></div>
    </div>

</div>

<%-- ===== STYLES ===== --%>
<style>
    .exim-general-search { font-family: inherit; }

    /* Search box */
    .gs-search-box-wrapper { background: #f4f7fa; padding: 18px 24px; border-radius: 6px; margin-bottom: 28px; }
    .gs-input-group { display: flex; align-items: center; position: relative; }
    .gs-input {
        flex: 1; height: 44px; border: 1px solid #cdd5df;
        border-radius: 4px 0 0 4px; padding: 0 40px 0 14px;
        font-size: 15px; outline: none;
    }
    [dir=rtl] .gs-input { border-radius: 0 4px 4px 0; padding: 0 14px 0 40px; }
    .gs-search-icon { position: absolute; right: 110px; color: #888; pointer-events: none; }
    [dir=rtl] .gs-search-icon { right: auto; left: 110px; }
    .gs-btn-search {
        height: 44px; background: #1a5276; color: #fff; border: none;
        padding: 0 24px; border-radius: 0 4px 4px 0; cursor: pointer; font-size: 15px;
    }
    [dir=rtl] .gs-btn-search { border-radius: 4px 0 0 4px; }
    .gs-btn-search:hover { background: #154360; }

    /* Results header - Improved for RTL/LTR */
    .gs-results-header {
        display: flex;
        align-items: flex-start;
        justify-content: space-between;
        margin-bottom: 20px;
        flex-wrap: wrap;
        gap: 12px;
    }
    
    /* LTR (English) - default */
    .gs-header-right {
        display: flex;
        flex-direction: column;
        gap: 6px;
        order: 1;
        text-align: left;
    }
    .gs-header-left {
        display: flex;
        align-items: center;
        gap: 12px;
        order: 2;
        flex-wrap: wrap;
    }
    
    /* RTL (Arabic) - swap order and right-align */
    [dir=rtl] .gs-results-header {
        flex-direction: row-reverse;
    }
    [dir=rtl] .gs-header-right {
        order: 2;
        align-items: flex-end;
        text-align: right;
    }
    [dir=rtl] .gs-header-left {
        order: 1;
        flex-direction: row-reverse;
    }

    .gs-results-title { 
        font-size: 25px; 
        font-weight: 700; 
        color: black; 
        margin: 0; 
    }
    
    .gs-result-count {
        display: inline-block;
        color: black;
        padding: 2px 12px;
        font-size: 13px;
        text-align: inherit;
    }
    
    /* Ensure count badge aligns properly in RTL */
    [dir=rtl] .gs-result-count {
        align-self: flex-start;
    }
    
    /* Left side controls - LTR */
    .gs-header-left {
        display: flex;
        align-items: center;
        gap: 12px;
        flex-wrap: wrap;
    }
    
    /* For RTL, keep the controls group order but reverse inner elements if needed */
    [dir=rtl] .gs-header-left {
        flex-direction: row-reverse;
    }

    .gs-sort-group, .gs-filter-group { 
        display: flex; 
        align-items: center; 
        gap: 6px; 
        font-size: 14px; 
        color: #555; 
    }
    
    /* RTL adjustments for sort/filter groups */
    [dir=rtl] .gs-sort-group, 
    [dir=rtl] .gs-filter-group {
        flex-direction: row-reverse;
    }
    
    .gs-sort-select, .gs-filter-select {
        border: 1px solid #cdd5df; 
        border-radius: 4px;
        padding: 5px 10px; 
        font-size: 14px; 
        background: #fff; 
        cursor: pointer;
    }
    
    .gs-filter-group {
        background: #1a5276 !important; 
        color: #fff !important;     
        border: 1px solid #cdd5df; 
        border-radius: 4px;
        padding-left: 5px; 
        padding-right: 5px;
    }
    
    .gs-filter-select.btn-filter { 
        background: #1a5276 !important; 
        color: #fff !important; 
        border-color: #1a5276 !important; 
    }

    /* RTL icon adjustments */
    [dir=rtl] .gs-filter-group .fa-filter {
        order: -1;
    }
    
    [dir=rtl] .gs-sort-select .fa-sort-amount-desc {
        order: -1;
    }

    /* Loading */
    .gs-loading { padding: 30px; text-align: center; color: #1a5276; font-size: 16px; }

    /* Result cards */
    .gs-results-list { display: flex; flex-direction: column; gap: 14px; }
    .gs-study-item {
        background: #fff; border: 1px solid #e2e8f0; border-radius: 6px;
        padding: 20px 24px; direction: inherit;
    }
    .gs-result-badge {
        display: inline-block; background: #e8f4fc; color: #1a5276;
        border: 1px solid #aed6f1; border-radius: 10px;
        padding: 2px 12px; font-size: 12px; margin-bottom: 8px;
    }
    .gs-result-title { font-size: 17px; font-weight: 700; margin: 0 0 10px; }
    .gs-result-title a { color: #1a3a5c; text-decoration: none; }
    .gs-result-title a:hover { color: #1a5276; text-decoration: underline; }
    .gs-result-desc { font-size: 14px; color: #555; margin: 0 0 14px; line-height: 1.7; }
    .gs-study-action { }
    .gs-btn-details {
        background: #27ae60; border: none; color: #fff;
        font-size: 13px; padding: 6px 18px; border-radius: 4px;
        text-decoration: none; display: inline-block; cursor: pointer;
    }
    .gs-btn-details:hover { background: #1e8449; color: #fff; }

    /* No results / Default message */
    .gs-no-results { 
        text-align: center; 
        padding: 60px 40px; 
        color: #888; 
        font-size: 18px; 
        background: #f9fafb;
        border-radius: 8px;
        border: 1px dashed #d0d7e2;
    }
    .gs-no-results .fa-search {
        font-size: 48px;
        color: #ccc;
        display: block;
        margin-bottom: 15px;
    }

    /* Pagination */
    .page-pagination { margin-top: 30px; text-align: center; }
    .pagination { 
        display: flex; 
        list-style: none; 
        padding: 0; 
        margin: 0 0 10px; 
        justify-content: center; 
        gap: 4px; 
        flex-wrap: wrap; 
    }
    
    /* RTL pagination arrow directions */
    [dir=rtl] .pagination .fa-angle-right:before {
        content: "\f104"; /* left arrow */
    }
    [dir=rtl] .pagination .fa-angle-left:before {
        content: "\f105"; /* right arrow */
    }
    
    .page-item .page-link {
        display: block; padding: 6px 14px; border: 1px solid #cdd5df;
        border-radius: 4px; color: #1a5276; text-decoration: none; cursor: pointer; background: #fff;
    }
    .page-item.active .page-link { background: #1a5276; color: #fff; border-color: #1a5276; }
    .page-item.disabled .page-link, .page-item.disabled span.page-link { color: #aaa; cursor: default; pointer-events: none; }
    .pages-total { font-size: 13px; color: #888; }
</style>

<%-- ===== CLIENT-SIDE SEARCH SCRIPT ===== --%>
<script type="text/javascript">
    (function () {
        'use strict';

        // ── Config ────────────────────────────────────────────────────────────
        var PAGE_SIZE = 10;
        var isArabic = (typeof _spPageContextInfo !== 'undefined' &&
            _spPageContextInfo.currentLanguage == 1025);
        var moreText = isArabic ? 'تفاصيل الخدمة' : 'More details';

        // ── State ─────────────────────────────────────────────────────────────
        var state = {
            query: '',
            sortQuery: '',
            currentPage: 1,
            totalRows: 0,
            allResults: []   // we fetch up to 500 and page client-side
        };

        // ── DOM refs ──────────────────────────────────────────────────────────
        var elSort = document.getElementById('gsSortSelect');
        var elFilter = document.getElementById('gsFilterSelect');
        var elHeader = document.getElementById('gsResultsHeader');
        var elTitle = document.getElementById('gsResultTitle');
        var elCount = document.getElementById('gsResultCount');
        var elLoading = document.getElementById('gsLoading');
        var elNoResults = document.getElementById('gsNoResults');
        var elList = document.getElementById('gsResultsList');
        var elPagination = document.getElementById('gsPagination');
        var elPagUl = document.getElementById('gsPaginationUl');
        var elPagesTotal = document.getElementById('gsPagesTotal');

        // ── Wire events ───────────────────────────────────────────────────────
        elSort.addEventListener('change', function () {
            if (!state.query) return;
            state.sortQuery = buildSortParam(elSort.value);
            state.currentPage = 1;
            fetchResults();
        });

        elFilter.addEventListener('change', function () {
            state.currentPage = 1;
            renderPage();
        });

        // ── Auto-search from query-string (?k= or ?q=) ────────────────────────
        (function () {
            var qs = getHashParam('k');

            if (qs) {
                state.query = qs;
                state.currentPage = 1;
                state.sortQuery = buildSortParam(elSort.value);
                fetchResults();
            }
        })();

        window.addEventListener('hashchange', function () {
            var qs = getHashParam('k');

            if (qs !== state.query) {
                state.query = qs;
                state.currentPage = 1;
                state.sortQuery = buildSortParam(elSort.value);
                fetchResults();
            }
        });

        // ── Core functions ────────────────────────────────────────────────────

        function triggerSearch() {
            // Kept for backward compatibility. Search is now driven by ?k=.
        }

        function fetchResults() {
            showLoading(true);
            hideAll();

            var sortParam = state.sortQuery;   // e.g. "&sortlist='LastModifiedTime:descending'"

            // Scope 1 – content type (same as the CSWP query on /en):
            //   ContentTypeId:0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF39*
            var CONTENT_TYPE_SCOPE =
                'ContentTypeId:0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF39*';

            // Scope 2 – restrict to /en or /ar and all children.
            // The web part lives under /en/Search so webAbsoluteUrl = http://nodetech:2020/en/Search.
            // We need the language subsite root (/en or /ar), which is always 1 level below the
            // site collection root (siteAbsoluteUrl = http://nodetech:2020).
            // Strategy: strip siteAbsoluteUrl from webAbsoluteUrl, take the first path segment.
            //   webAbsoluteUrl  = http://nodetech:2020/en/Search
            //   siteAbsoluteUrl = http://nodetech:2020
            //   relative        = /en/Search  → first segment = "en"
            //   langRoot        = http://nodetech:2020/en
            var siteUrl = _spPageContextInfo.siteAbsoluteUrl.replace(/\/+$/, '');
            var webUrl = _spPageContextInfo.webAbsoluteUrl.replace(/\/+$/, '');
            var relative = webUrl.replace(siteUrl, '');               // e.g. "/en/Search"
            var firstSeg = relative.replace(/^\//, '').split('/')[0]; // e.g. "en"
            var langRoot = siteUrl + (firstSeg ? '/' + firstSeg : '');// e.g. "http://nodetech:2020/en"
            var PATH_SCOPE = 'path:' + langRoot;

            // KQL structure: each clause separated, keyword wrapped in quotes if it contains spaces,
            // wildcard appended outside the quotes: "multi word"* is valid KQL prefix on a phrase.
            var keyword = state.query.indexOf(' ') !== -1
                ? '"' + state.query + '"*'
                : state.query + '*';

            var fullQuery = CONTENT_TYPE_SCOPE + ' ' + PATH_SCOPE + ' ' + keyword;

            var url = _spPageContextInfo.siteAbsoluteUrl +
                '/_api/search/query?' +
                "querytext='" + encodeURIComponent(fullQuery) + "'" +
                sortParam +
                '&rowlimit=500' +
                "&selectedproperties='Title,Path,CommentsOWSMTXT,ContentClass,Description,FileExtension'";

            // Use XMLHttpRequest for SP on-prem (no fetch polyfill needed)
            var xhr = new XMLHttpRequest();
            xhr.open('GET', url, true);
            xhr.setRequestHeader('Accept', 'application/json;odata=verbose');
            xhr.setRequestHeader('Content-Type', 'application/json;odata=verbose');

            xhr.onload = function () {
                showLoading(false);
                if (xhr.status >= 200 && xhr.status < 300) {
                    try {
                        var data = JSON.parse(xhr.responseText);
                        processResults(data);
                    } catch (e) {
                        showError('JSON parse error: ' + e.message);
                    }
                } else {
                    showError('Search API error: HTTP ' + xhr.status);
                }
            };

            xhr.onerror = function () {
                showLoading(false);
                showError('Network error during search.');
            };

            xhr.send();
        }

        function processResults(data) {
            try {
                var relevant = data.d.query.PrimaryQueryResult.RelevantResults;
                state.totalRows = relevant.TotalRows;
                var rows = relevant.Table.Rows.results;

                state.allResults = rows.map(function (row) {
                    var cells = {};
                    row.Cells.results.forEach(function (c) { cells[c.Key] = c.Value; });
                    return {
                        title: cells['Title'] || '',
                        url: cells['Path'] || '#',
                        description: cells['CommentsOWSMTXT'] || cells['Description'] || '',
                        contentClass: cells['ContentClass'] || '',
                        fileExtension: (cells['FileExtension'] || '').toLowerCase()
                    };
                });
            } catch (e) {
                showError('Could not parse search results: ' + e.message);
                return;
            }

            // Apply client-side sort if needed (title sort can be done server-side,
            // but in case you want client-side as well)
            applySortIfNeeded();

            state.currentPage = 1;
            renderPage();
        }

        function applySortIfNeeded() {
            // Server-side sort handles most cases; add client-side fallback here if required
        }

        function renderPage() {
            // Apply client-side filter.
            // ContentClass alone cannot distinguish "Page" from "Document": SharePoint
            // returns STS_ListItem_DocumentLibrary for BOTH publishing pages and regular
            // library documents. So:
            //   - "Lists"     → matched on ContentClass (STS_ListItem_GenericList etc.)
            //   - "Pages"     → matched on FileExtension === 'aspx'
            //   - "Documents" → ContentClass is a document-library item AND extension != aspx
            var filterType = elFilter.value || ''; // '', 'type:page', 'type:list', 'type:document'

            var filtered = !filterType
                ? state.allResults
                : state.allResults.filter(function (r) {
                    var isList = r.contentClass.indexOf('GenericList') !== -1;
                    var isDocLibItem = r.contentClass.indexOf('DocumentLibrary') !== -1;
                    var isPage = r.fileExtension === 'aspx';

                    if (filterType === 'type:list') return isList;
                    if (filterType === 'type:page') return isPage;
                    if (filterType === 'type:document') return isDocLibItem && !isPage;
                    return true;
                });

            var total = filtered.length;
            var totalPages = total > 0 ? Math.ceil(total / PAGE_SIZE) : 0;
            var page = state.currentPage;
            var start = (page - 1) * PAGE_SIZE;
            var end = Math.min(start + PAGE_SIZE, total);
            var pageItems = filtered.slice(start, end);

            // Show header
            elHeader.style.display = '';
            var titleFmt = isArabic
                ? 'نتيجة البحث عن &ldquo;{q}&rdquo;'
                : 'Search Results for &ldquo;{q}&rdquo;';
            elTitle.innerHTML = titleFmt.replace('{q}', escHtml(state.query));

            var countFmt = isArabic
                ? '{n} نتيجة وجدت'
                : '{n} result(s) found';
            elCount.textContent = countFmt.replace('{n}', total);

            if (pageItems.length === 0) {
                elNoResults.style.display = 'block';
                elNoResults.innerHTML = isArabic
                    ? '<i class="fa fa-search" style="font-size: 48px; color: #ccc; display: block; margin-bottom: 15px;"></i>لا توجد نتائج تطابق بحثك.'
                    : '<i class="fa fa-search" style="font-size: 48px; color: #ccc; display: block; margin-bottom: 15px;"></i>No results found for your search.';
                elList.style.display = 'none';
                elPagination.style.display = 'none';
                return;
            }

            // Render cards
            elNoResults.style.display = 'none';
            elList.style.display = '';
            elList.innerHTML = pageItems.map(renderCard).join('');

            // Render pagination
            if (totalPages > 1) {
                elPagination.style.display = '';
                renderPagination(page, totalPages);
                var pagesLbl = isArabic
                    ? 'إجمالي ' + totalPages + ' صفحات'
                    : 'Total ' + totalPages + ' pages';
                elPagesTotal.textContent = pagesLbl;
            } else {
                elPagination.style.display = 'none';
            }
        }

        function renderCard(item) {
            var badge = mapCategory(item.contentClass);
            var badgeHtml = badge
                ? '<span class="gs-result-badge">' + escHtml(badge) + '</span><br/>'
                : '';
            var desc = item.description
                ? '<p class="gs-result-desc">' + escHtml(item.description) + '</p>'
                : '';
            return '<div class="study-item">' +
                badgeHtml +
                '<h3 class="gs-result-title">' +
                '<a href="' + escAttr(item.url) + '">' + escHtml(item.title) + '</a>' +
                '</h3>' +
                desc +
                '<div class="study-action">' +
                '<a href="' + escAttr(item.url) + '" class="btn btn-primary">' +
                moreText +
                '</a>' +
                '</div>' +
                '</div>';
        }

        function renderPagination(currentPage, totalPages) {
            var MAX_VISIBLE = 5;
            var half = Math.floor(MAX_VISIBLE / 2);
            var start = Math.max(1, currentPage - half);
            var end = Math.min(totalPages, start + MAX_VISIBLE - 1);
            if (end - start < MAX_VISIBLE - 1)
                start = Math.max(1, end - MAX_VISIBLE + 1);

            var html = '';

            // Use different arrow icons based on language direction
            var prevIcon = isArabic ? 'fa-angle-left' : 'fa-angle-right';
            var nextIcon = isArabic ? 'fa-angle-right' : 'fa-angle-left';

            // Prev
            if (currentPage <= 1) {
                html += '<li class="page-item disabled"><span class="page-link"><i class="fas ' + prevIcon + '"></i></span></li>';
            } else {
                html += '<li class="page-item"><a class="page-link" href="#" data-page="' +
                    (currentPage - 1) + '"><i class="fas ' + prevIcon + '"></i></a></li>';
            }

            // Numbered pages
            for (var p = start; p <= end; p++) {
                if (p === currentPage) {
                    html += '<li class="page-item active"><a class="page-link" href="#">' + p + '</a></li>';
                } else {
                    html += '<li class="page-item"><a class="page-link" href="#" data-page="' + p + '">' + p + '</a></li>';
                }
            }

            // Next
            if (currentPage >= totalPages) {
                html += '<li class="page-item disabled"><span class="page-link"><i class="fas ' + nextIcon + '"></i></span></li>';
            } else {
                html += '<li class="page-item"><a class="page-link" href="#" data-page="' +
                    (currentPage + 1) + '"><i class="fas ' + nextIcon + '"></i></a></li>';
            }

            elPagUl.innerHTML = html;

            // Wire page clicks
            elPagUl.querySelectorAll('a[data-page]').forEach(function (a) {
                a.addEventListener('click', function (e) {
                    e.preventDefault();
                    state.currentPage = parseInt(this.getAttribute('data-page'), 10);
                    renderPage();
                    // Scroll to top of results
                    elHeader.scrollIntoView({ behavior: 'smooth', block: 'start' });
                });
            });
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        function buildSortParam(val) {
            // val is e.g. "LastModifiedTime:descending" or "" for relevance
            if (!val) return '';
            var parts = val.split(':');
            if (parts.length !== 2) return '';
            // SP Search REST sort syntax: &sortlist='Property:direction'
            // direction: 0 = ascending, 1 = descending
            var dir = parts[1] === 'descending' ? '1' : '0';
            return "&sortlist='" + encodeURIComponent(parts[0] + ':' + dir) + "'";
        }

        function mapCategory(contentClass) {
            if (!contentClass) return '';
            if (contentClass.indexOf('Pages') !== -1) return isArabic ? 'صفحة' : 'Page';
            if (contentClass.indexOf('Announcements') !== -1) return isArabic ? 'أخبار' : 'News';
            if (contentClass.indexOf('GenericList') !== -1) return isArabic ? 'قائمة' : 'List';
            return '';
        }

        function showLoading(visible) {
            elLoading.style.display = visible ? '' : 'none';
            if (visible) {
                elNoResults.style.display = 'none';
            }
        }

        function hideAll() {
            elHeader.style.display = 'none';
            elList.style.display = 'none';
            elPagination.style.display = 'none';
        }

        function showError(msg) {
            elNoResults.style.display = 'block';
            elNoResults.innerHTML = '<i class="fa fa-exclamation-triangle" style="font-size: 48px; color: #e67e22; display: block; margin-bottom: 15px;"></i>' + escHtml(msg);
        }

        function escHtml(s) {
            return String(s)
                .replace(/&/g, '&amp;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;')
                .replace(/"/g, '&quot;')
                .replace(/'/g, '&#39;');
        }

        function escAttr(s) { return escHtml(s); }

        function getQueryStringParam(name) {
            var match = new RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match ? decodeURIComponent(match[1].replace(/\+/g, ' ')) : null;
        }

        function getHashParam(name) {
            var match = new RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match ? decodeURIComponent(match[1].replace(/\+/g, ' ')) : null;
        }

    })();
</script>
