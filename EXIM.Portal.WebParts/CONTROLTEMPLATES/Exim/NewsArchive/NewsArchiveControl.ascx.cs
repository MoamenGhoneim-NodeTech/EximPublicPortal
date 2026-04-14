using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.NewsArchive
{
    public partial class NewsArchiveControl : UserControl
    {
        private const string DefaultImage =
    "/PublishingImages/DefaultImages/NewsDefaultImg.png";

        // News pages are always filtered on EXIM_ShowInArchive.
        private const string WhereClause =
            "<Where><Eq><FieldRef Name='EXIM_ShowInArchive'/><Value Type='Boolean'>1</Value></Eq></Where>";

        private const string OrderByClause =
            "<OrderBy><FieldRef Name='ArticleStartDate' Ascending='False'/></OrderBy>";

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
                LandingPageHelper.LogError("NewsArchiveControl: target list not found.");
                return;
            }

            try
            {
                int currentPage = LandingPageHelper.GetCurrentPage(Request);
                SPQuery query = BuildPagedQuery(list, currentPage);

                var dataSource = new List<NewsItemModel>();
                foreach (SPListItem item in list.GetItems(query))
                    dataSource.Add(MapItem(item));

                rptNewsItems.DataSource = dataSource;
                rptNewsItems.DataBind();

                RenderPagination(list, currentPage);
            }
            catch (Exception ex)
            {
                LandingPageHelper.LogError(
                    $"NewsArchiveControl.BindItems failed: {ex.Message}");
            }
        }

        private static NewsItemModel MapItem(SPListItem item)
        {
            string rawDate = item["ArticleStartDate"]?.ToString() ?? string.Empty;
            string articleDate = string.Empty;
            if (!string.IsNullOrEmpty(rawDate) &&
                DateTime.TryParse(rawDate, out DateTime parsedDate))
            {
                articleDate = parsedDate.ToString("dd MMM yyyy");
            }

            return new NewsItemModel
            {
                Title = item["Title"]?.ToString() ?? string.Empty,
                Comments = item["Comments"]?.ToString() ?? string.Empty,
                FileRef = item["FileRef"]?.ToString() ?? "#",
                ArticleDate = articleDate,
                ImgPath = LandingPageHelper.ExtractImageSrc(
                                  item["PublishingRollupImage"]?.ToString(), DefaultImage),
                // Use the centralised helper instead of a hardcoded string.
                ButtonText = LandingPageHelper.NewsCategoryText
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
                    "<FieldRef Name='Comments'/>" +
                    "<FieldRef Name='FileRef'/>" +
                    "<FieldRef Name='ArticleStartDate'/>" +
                    "<FieldRef Name='PublishingRollupImage'/>",
                Query = WhereClause + OrderByClause,
                QueryThrottleMode = SPQueryThrottleOption.Override
            };

            if (page > 1)
                query.ListItemCollectionPosition =
                    LandingPageHelper.GetPagePosition(list, WhereClause, page, OrderByClause);

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
            LandingPageHelper.TryGetList(web, "Pages")
            ?? LandingPageHelper.TryGetList(web, "الصفحات");
    }

    // ── View model ───────────────────────────────────────────────────────────────
    public class NewsItemModel
    {
        public string Title { get; set; }
        public string Comments { get; set; }
        public string FileRef { get; set; }
        public string ArticleDate { get; set; }
        public string ImgPath { get; set; }
        public string ButtonText { get; set; }
    }
}

