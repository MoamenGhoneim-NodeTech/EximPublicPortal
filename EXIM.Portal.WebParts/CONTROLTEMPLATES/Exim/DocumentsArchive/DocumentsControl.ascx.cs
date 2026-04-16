using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;
using EXIM.Common.Lib.Utils;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.DocumentsArchive
{
    public partial class DocumentsControl : UserControl
    {
        // Documents are filtered on EXIM_Visibility.
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
                LogService.LogException("DocumentsControl: target list not found.");
                return;
            }

            try
            {
                int currentPage = LandingPageHelper.GetCurrentPage(Request);
                SPQuery query = BuildPagedQuery(list, currentPage);

                string downloadText = LandingPageHelper.IsEnglish()
                    ? "Download report" : "تحميل التقرير";

                var dataSource = new List<DocumentItemModel>();
                foreach (SPListItem item in list.GetItems(query))
                {
                    dataSource.Add(new DocumentItemModel
                    {
                        Title = item["Title"]?.ToString() ?? string.Empty,
                        FileRef = item["FileRef"]?.ToString() ?? "#",
                        DownloadText = downloadText
                    });
                }

                rptDocumentItems.DataSource = dataSource;
                rptDocumentItems.DataBind();

                RenderPagination(list, currentPage);
            }
            catch (Exception ex)
            {
                LogService.LogException(
                    $"DocumentsControl.BindItems failed: {ex.Message}");
            }
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
    public class DocumentItemModel
    {
        public string Title { get; set; }
        public string FileRef { get; set; }
        public string DownloadText { get; set; }
    }
}
