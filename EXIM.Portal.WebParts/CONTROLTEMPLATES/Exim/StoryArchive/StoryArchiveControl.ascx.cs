using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;
using EXIM.Common.Lib.Utils;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.StoryArchive
{
    public partial class StoryArchiveControl : UserControl
    {
        private const string DefaultImage =
    "/PublishingImages/DefaultImages/SuccessStoryDefaultImg.png";

        // Success-story pages are filtered on EXIM_ShowInArchive.
        private const string WhereClause =
            "<Where><Eq><FieldRef Name='EXIM_ShowInArchive'/><Value Type='Boolean'>1</Value></Eq></Where>";

        private const string OrderByClause =
            "<OrderBy>" +
                "<FieldRef Name='EXIM_ItemOrder' Ascending='False'/>" +
                "<FieldRef Name='Created'        Ascending='False'/>" +
            "</OrderBy>";

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
                LogService.LogException("StoryArchiveControl: target list not found.");
                return;
            }

            try
            {
                int currentPage = LandingPageHelper.GetCurrentPage(Request);
                SPQuery query = BuildPagedQuery(list, currentPage);

                string buttonText = LandingPageHelper.ButtonText;

                var dataSource = new List<StoryItemModel>();
                foreach (SPListItem item in list.GetItems(query))
                    dataSource.Add(MapItem(item, buttonText));

                rptItems.DataSource = dataSource;
                rptItems.DataBind();

                RenderPagination(list, currentPage);
            }
            catch (Exception ex)
            {
                LogService.LogException(
                    $"StoryArchiveControl.BindItems failed: {ex.Message}");
            }
        }

        private static StoryItemModel MapItem(SPListItem item, string buttonText)
        {
            return new StoryItemModel
            {
                Title = item["Title"]?.ToString() ?? string.Empty,
                Description = item["Comments"]?.ToString() ?? string.Empty,
                ItemUrl = item["FileRef"]?.ToString() ?? "#",
                ImgPath = LandingPageHelper.ExtractImageSrc(
                                 item["PublishingRollupImage"]?.ToString(), DefaultImage),
                LogoPath = LandingPageHelper.ExtractImageSrc(
                                 item["Exim_SecondaryImage"]?.ToString(), DefaultImage),
                PersonName = item["Exim_Story_PersonOfInterest"]?.ToString() ?? string.Empty,
                PersonWord = item["Exim_Story_PersonWord"]?.ToString() ?? string.Empty,
                ButtonText = buttonText
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
                    "<FieldRef Name='PublishingRollupImage'/>" +
                    "<FieldRef Name='Exim_SecondaryImage'/>" +
                    "<FieldRef Name='Exim_Story_PersonOfInterest'/>" +
                    "<FieldRef Name='Exim_Story_PersonWord'/>",
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
    public class StoryItemModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ItemUrl { get; set; }
        public string ImgPath { get; set; }
        public string LogoPath { get; set; }
        public string PersonName { get; set; }
        public string PersonWord { get; set; }
        public string ButtonText { get; set; }
    }
}
