using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using EXIM.Common.Lib.Utils;
namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.ArticlesArchive
{
    public partial class ArticlesArchiveControl : UserControl
    {
        
        // Pages lists always filter on EXIM_ShowInArchive.
        private const string WhereClause =
            "<Where><Eq><FieldRef Name='EXIM_ShowInArchive'/><Value Type='Boolean'>1</Value></Eq></Where>";
        private const string OrderbyClause = "<OrderBy> <FieldRef Name='ArticleStartDate' Ascending='False' /> </OrderBy>";

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
                LandingPageHelper.LogError(
                    "ArticlesArchiveControl: target list not found.");
                return;
            }

            try
            {
                int currentPage = LandingPageHelper.GetCurrentPage(Request);
                SPQuery query = BuildPagedQuery(list, currentPage);

                var dataSource = new List<object>();
                foreach (SPListItem item in list.GetItems(query))
                {
                    dataSource.Add(new
                    {
                        Title = item["Title"] as string ?? string.Empty,
                        Comments = item["Comments"] as string ?? string.Empty,
                        FileRef = item["FileRef"] as string ?? "#"
                    });
                }

                rptStudyItems.DataSource = dataSource;
                rptStudyItems.DataBind();

                RenderPagination(list, currentPage);
            }
            catch (Exception ex)
            {
                LandingPageHelper.LogError(
                    $"ArticlesArchiveControl.BindItems failed: {ex.Message}");
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
                    "<FieldRef Name='Comments'/>" +
                    "<FieldRef Name='FileRef'/>",
                Query =
                    WhereClause +
                  OrderbyClause,
                QueryThrottleMode = SPQueryThrottleOption.Override
            };

            if (page > 1)
                query.ListItemCollectionPosition =
                    LandingPageHelper.GetPagePosition(list, WhereClause, page ,OrderbyClause);

            return query;
        }

        // ── Pagination ───────────────────────────────────────────────────────────
        private void RenderPagination(SPList list, int currentPage)
        {
            // Use filtered count (not list.ItemCount) for an accurate page total.
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
            LandingPageHelper.TryGetList(web, "Pages")
            ?? LandingPageHelper.TryGetList(web, "الصفحات");
    
}
}
