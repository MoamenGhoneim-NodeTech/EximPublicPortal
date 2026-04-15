using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Web.UI.WebControls.WebParts;
using EXIM.Common.Lib.SPHelpers;

namespace EXIM.Portal.WebParts.LandingPageWebpart
{
    public partial class LandingPageWebpartUserControl : UserControl
    {
       private const string DefaultImage =
             "/PublishingImages/DefaultImages/LandingDefaultImg.png";
        public LandingPageWebpart WebPartRef { get; set; }
        private SPList _list;

        // ── Lifecycle ────────────────────────────────────────────────────────────
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack) return;

            _list = ResolveTargetList(SPContext.Current.Web);

            if (_list == null)
            {
                LandingPageHelper.LogError("LandingPageControl: target list not found.");
                return;
            }

            BindItems();
            RenderPagination();
        }

        // ── Data binding ─────────────────────────────────────────────────────────
        private void BindItems()
        {
            rptItems.DataSource = GetItems();
            rptItems.DataBind();
        }

        private List<ListItemModel> GetItems()
        {
            var result = new List<ListItemModel>();
            if (_list == null) return result;

            try
            {
                string whereClause = WhereClause;
                int currentPage = LandingPageHelper.GetCurrentPage(Request);

                var query = new SPQuery
                {
                    RowLimit = (uint)LandingPageHelper.PageSize,
                    Query = whereClause +
                               "<OrderBy><FieldRef Name='EXIM_ItemOrder' Ascending='true'/></OrderBy>",
                    QueryThrottleMode = SPQueryThrottleOption.Override
                };

                if (currentPage > 1)
                    query.ListItemCollectionPosition =
                        LandingPageHelper.GetPagePosition(_list, whereClause, currentPage);

                foreach (SPListItem item in _list.GetItems(query))
                    result.Add(MapItem(item));
            }
            catch (Exception ex)
            {
                LandingPageHelper.LogError(
                    $"LandingPageControl.GetItems failed: {ex.Message}");
            }

            return result;
        }

        private ListItemModel MapItem(SPListItem item)
        {
            string listKey = _list.Title.ToLowerInvariant();
            bool isSiteOrSvc = listKey == "subsites" || listKey == "services";

            string descField = isSiteOrSvc
                ? (listKey == "subsites" ? "_Comments" : "EXIM_SrvDescription")
                : "Comments";

            string linkField = isSiteOrSvc ? "Exim_GeneralLink" : "FileRef";
            string generalLink = item[linkField]?.ToString() ?? string.Empty;
            string fileRef = item["FileRef"]?.ToString() ?? string.Empty;

            return new ListItemModel
            {
                Title = item["Title"]?.ToString() ?? string.Empty,
                Description = item[descField]?.ToString() ?? string.Empty,
                ItemLink = !string.IsNullOrEmpty(generalLink)
                                  ? generalLink.Split(',')[0]
                                  : fileRef,
                ImgPath = LandingPageHelper.ExtractImageSrc(
                                  item["PublishingRollupImage"]?.ToString(), DefaultImage),
                ButtonText = LandingPageHelper.ButtonText
            };
        }

        // ── Pagination ───────────────────────────────────────────────────────────
        private void RenderPagination()
        {
            int totalItems = LandingPageHelper.GetFilteredItemCount(_list, WhereClause);
            int currentPage = LandingPageHelper.GetCurrentPage(Request);
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

        // ── CAML helpers ─────────────────────────────────────────────────────────
        /// <summary>
        /// Returns the CAML Where clause appropriate for the resolved list type.
        /// Subsites/Services filter by EXIM_Visibility; Pages lists by EXIM_ShowInArchive.
        /// </summary>
        private string WhereClause
        {
            get
            {
                string key = _list?.Title?.ToLowerInvariant() ?? string.Empty;
                return (key == "subsites" || key == "services")
                    ? "<Where><Eq><FieldRef Name='EXIM_Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>"
                    : "<Where><Eq><FieldRef Name='EXIM_ShowInArchive'/><Value Type='Boolean'>1</Value></Eq></Where>";
            }
        }

        // ── List resolution ──────────────────────────────────────────────────────
        private SPList ResolveTargetList(SPWeb web)
        {
            // If TargetListTitle property is set, try it first
            if (!string.IsNullOrEmpty(WebPartRef.TargetlistTitle))
            {
                var explicitList = LandingPageHelper.TryGetList(web, WebPartRef.TargetlistTitle);
                if (explicitList != null)
                    return explicitList;
            }

            // Fall back to default list names
            return LandingPageHelper.TryGetList(web, "Pages")
                ?? LandingPageHelper.TryGetList(web, "الصفحات");
        }
        // ── Repeater hook ────────────────────────────────────────────────────────
        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Reserved for future per-item server-side logic.
        }
    }

    // ── View model ───────────────────────────────────────────────────────────────
    public class ListItemModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ItemLink { get; set; }
        public string ImgPath { get; set; }
        public string ButtonText { get; set; }
    }
}
