using System;
using System.Web;
using Microsoft.SharePoint;

namespace EXIM.Common.Lib.SPHelpers
{
    /// <summary>
    /// Shared utility class for landing-pages 
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
        public static int GetCurrentPage(HttpRequest request)
        {
            int page;
            return int.TryParse(request.QueryString["page"], out page) && page > 0
                ? page : 1;
        }

        public const string DefaultOrderByClause =
            "<OrderBy><FieldRef Name='EXIM_ItemOrder' Ascending='true'/></OrderBy>";

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
