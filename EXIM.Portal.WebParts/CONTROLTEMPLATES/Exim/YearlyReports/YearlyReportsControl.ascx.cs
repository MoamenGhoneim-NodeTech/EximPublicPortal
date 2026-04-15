using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.YearlyReports
{
    public partial class YearlyReportsControl : UserControl
    {
        // Yearly reports are filtered on EXIM_ShowInArchive.
        private const string WhereClause =
            "<Where><Eq><FieldRef Name='EXIM_Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>";

        // ── Lifecycle ────────────────────────────────────────────────────────────
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            BindItems();
        }

        // ── Data binding ─────────────────────────────────────────────────────────
        private void BindItems()
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = ResolveTargetList(web);

            if (list == null)
            {
                LandingPageHelper.LogError("YearlyReportsControl: target list not found.");
                return;
            }

            try
            {
                int currentPage = LandingPageHelper.GetCurrentPage(Request);
                SPQuery query = BuildPagedQuery(list, currentPage);

                var dataSource = new List<ReportItemModel>();
                foreach (SPListItem item in list.GetItems(query))
                    dataSource.Add(MapItem(item));

                rptItems.DataSource = dataSource;
                rptItems.DataBind();

                RenderPagination(list, currentPage);
            }
            catch (Exception ex)
            {
                LandingPageHelper.LogError(
                    $"YearlyReportsControl.BindItems failed: {ex.Message}");
            }
        }

        private static ReportItemModel MapItem(SPListItem item)
        {
            string itemUrl = item["FileRef"]?.ToString() ?? "#";
            string encoded = HttpUtility.UrlEncode(itemUrl);

            // Build the report-details URL relative to the current page location.
            string currentPath = HttpContext.Current.Request.RawUrl.ToLower();
            string reportDetailsUrl = currentPath.Contains("/pages/")
                ? "reportDetails.aspx?src=" + encoded
                : "Reports/Pages/reportDetails.aspx?src=" + encoded;

            return new ReportItemModel
            {
                Title = item["Title"]?.ToString() ?? string.Empty,
                ItemUrl = itemUrl,
                ReportDetailsUrl = reportDetailsUrl,
                DownloadText = LandingPageHelper.IsEnglish() ? "Download report" : "تحميل التقرير",
                ViewText = LandingPageHelper.IsEnglish() ? "View Report" : "عرض التقرير"
            };
        }

        // ── CAML helpers ─────────────────────────────────────────────────────────
        private static SPQuery BuildPagedQuery(SPList list, int page)
        {
            var query = new SPQuery
            {
                RowLimit = (uint)LandingPageHelper.PageSize,
                ViewFields =
                    "<FieldRef Name='Title'/>" +
                    "<FieldRef Name='FileRef'/>",
                // FIXED: WhereClause was missing from the original query — all items
                // were being returned regardless of the EXIM_ShowInArchive flag.
                Query = WhereClause + LandingPageHelper.DefaultOrderByClause,
                QueryThrottleMode = SPQueryThrottleOption.Override
            };

            if (page > 1)
                query.ListItemCollectionPosition =
                    LandingPageHelper.GetPagePosition(list, WhereClause, page);

            return query;
        }

        // ── Pagination ───────────────────────────────────────────────────────────
        private void RenderPagination(SPList list, int currentPage)
        {
            int totalItems = LandingPageHelper.GetFilteredItemCount(list, WhereClause);
            string paginationHtml = LandingPageHelper.BuildPaginationHtml(totalItems, currentPage);

            bool hasMultiplePages = !string.IsNullOrEmpty(paginationHtml);

            litPagination.Text = paginationHtml;
            litPagination.Visible = hasMultiplePages;
            lblPrevText.Visible = hasMultiplePages;
            lblNextText.Visible = hasMultiplePages;

            if (hasMultiplePages)
            {
                lblPrevText.Text = LandingPageHelper.PrevText;
                lblNextText.Text = LandingPageHelper.NextText;
            }
        }

        // ── List resolution ──────────────────────────────────────────────────────
        private SPList ResolveTargetList(SPWeb web) =>
            LandingPageHelper.TryGetList(web, "Documents")
            ?? LandingPageHelper.TryGetList(web, "المستندات");
    }

    // ── View model ───────────────────────────────────────────────────────────────
    public class ReportItemModel
    {
        public string Title { get; set; }
        public string ItemUrl { get; set; }
        public string ReportDetailsUrl { get; set; }
        public string DownloadText { get; set; }
        public string ViewText { get; set; }
    }
}
