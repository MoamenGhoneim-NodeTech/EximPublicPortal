using System;
using System.Web;
using Microsoft.SharePoint;

namespace EXIM.Common.Lib.Utils
{
    /// <summary>
    /// Shared utility class for landing-page and archive web-part controls.
    /// Centralises language detection, SharePoint list resolution,
    /// server-side paging, and pagination HTML generation.
    /// </summary>
    public static class LandingPageHelper
    {
        // ── Constants ────────────────────────────────────────────────────────────
        public const int PageSize = 15;
        private const int LcidEnglish = 1033;

        // ── Language ─────────────────────────────────────────────────────────────
        /// <summary>True when the current UI culture is English (LCID 1033).</summary>
        public static bool IsEnglish() =>
            System.Threading.Thread.CurrentThread.CurrentUICulture.LCID == LcidEnglish;

        public static string ButtonText => IsEnglish() ? "Know more" : "معرفة المزيد";
        public static string NewsCategoryText => IsEnglish() ? "News" : "أخبار";
        public static string PrevText => IsEnglish() ? "Prev." : "السابق";
        public static string NextText => IsEnglish() ? "Next" : "التالي";

        // ── Paging ───────────────────────────────────────────────────────────────
        /// <summary>
        /// Reads the current 1-based page number from the supplied query-string.
        /// Returns 1 when the parameter is absent or invalid.
        /// </summary>
        public static int GetCurrentPage(HttpRequest request)
        {
            int page;
            return int.TryParse(request.QueryString["page"], out page) && page > 0
                ? page : 1;
        }

        public const string DefaultOrderByClause =
            "<OrderBy><FieldRef Name='EXIM_ItemOrder' Ascending='true'/></OrderBy>";

        /// <summary>
        /// Walks through pages to locate the correct
        /// <see cref="SPListItemCollectionPosition"/> for <paramref name="targetPage"/>.
        /// When <paramref name="orderByClause"/> is null or empty the default
        /// EXIM_ItemOrder ascending clause is used.
        /// </summary>
        public static SPListItemCollectionPosition GetPagePosition(
            SPList list, string whereClause, int targetPage,
            string orderByClause = null)
        {
            if (targetPage <= 1) return null;

            string resolvedOrderBy = !string.IsNullOrEmpty(orderByClause)
                ? orderByClause
                : DefaultOrderByClause;

            var query = new SPQuery
            {
                RowLimit = (uint)PageSize,
                Query = whereClause + resolvedOrderBy
            };

            SPListItemCollectionPosition position = null;
            for (int i = 1; i < targetPage; i++)
            {
                SPListItemCollection items = list.GetItems(query);
                if (items.ListItemCollectionPosition == null) break;
                position = items.ListItemCollectionPosition;
                query.ListItemCollectionPosition = position;
            }

            return position;
        }

        /// <summary>
        /// Returns the filtered item count using a minimal (ID-only) CAML query
        /// so no content fields are transferred over the wire.
        /// </summary>
        public static int GetFilteredItemCount(SPList list, string whereClause)
        {
            try
            {
                if (list == null) return 0;

                var query = new SPQuery
                {
                    ViewFields = "<FieldRef Name='ID'/>",
                    ViewFieldsOnly = true,
                    Query = whereClause
                };

                return list.GetItems(query).Count;
            }
            catch { return 0; }
        }

        /// <summary>
        /// Builds the HTML pagination strip and returns it as a string.
        /// Returns <see cref="string.Empty"/> when the total fits on one page.
        /// </summary>
        public static string BuildPaginationHtml(int totalItems, int currentPage)
        {
            int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            if (totalPages <= 1) return string.Empty;

            var html = new System.Text.StringBuilder();

            if (currentPage > 1)
                html.AppendFormat(
                    "<a class='pagingItem' href='?page={0}'>{1}</a>",
                    currentPage - 1, PrevText);

            for (int i = 1; i <= totalPages; i++)
            {
                string active = (i == currentPage) ? " active" : string.Empty;
                html.AppendFormat(
                    "<a class='pagingItem{0}' href='?page={1}'>{1}</a>",
                    active, i);
            }

            if (currentPage < totalPages)
                html.AppendFormat(
                    "<a class='pagingItem' href='?page={0}'>{1}</a>",
                    currentPage + 1, NextText);

            return html.ToString();
        }

        // ── Image helpers ────────────────────────────────────────────────────────
        /// <summary>
        /// Parses the src attribute out of a SharePoint PublishingRollupImage HTML string.
        /// Returns <paramref name="defaultImage"/> when the field is empty or unparseable.
        /// </summary>
        public static string ExtractImageSrc(string rollupImageHtml, string defaultImage)
        {
            if (string.IsNullOrEmpty(rollupImageHtml)) return defaultImage;

            int start = rollupImageHtml.IndexOf("src=\"",
                            StringComparison.OrdinalIgnoreCase);
            if (start < 0) return defaultImage;
            start += 5;

            int end = rollupImageHtml.IndexOf("\"", start);
            if (end < 0) return defaultImage;

            string src = rollupImageHtml.Substring(start, end - start);
            return string.IsNullOrEmpty(src) ? defaultImage : src;
        }

        // ── List resolution ──────────────────────────────────────────────────────
        /// <summary>
        /// Attempts to retrieve a SharePoint list by name.
        /// Returns <c>null</c> and traces an error if the list is not found.
        /// </summary>
        public static SPList TryGetList(SPWeb web, string listName)
        {
            try
            {
                return web.Lists.TryGetList(listName);
            }
            catch (Exception ex)
            {
                LogError($"Could not retrieve list '{listName}': {ex.Message}");
                return null;
            }
        }

        // ── Logging ──────────────────────────────────────────────────────────────
        public static void LogError(string message) =>
            System.Diagnostics.Trace.TraceError($"[EXIM.Portal] {message}");
    }
}
