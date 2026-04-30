using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;
using EXIM.Common.Lib.Utils;


namespace EXIM.Portal.WebParts.EligibilityCheckWebPart
{
    public partial class EligibilityCheckWebPartUserControl : UserControl
    {

        public EligibilityCheckWebPart WebPartRef { get; set; }
        // Pre-application products are filtered on EXIM_Visibility.
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
                LogService.LogException("PreAppControl: target list not found.");
                return;
            }

            try
            {
                int currentPage = LandingPageHelper.GetCurrentPage(Request);
                SPQuery query = BuildPagedQuery(list, currentPage);

                string buttonText = LandingPageHelper.IsEnglish()
                    ? "Choose Product" : "اختر المنتج";

                var dataSource = new List<PreAppItemModel>();
                foreach (SPListItem item in list.GetItems(query))
                    dataSource.Add(MapItem(item, buttonText));

                rptPreAppItems.DataSource = dataSource;
                rptPreAppItems.DataBind();

                RenderPagination(list, currentPage);
            }
            catch (Exception ex)
            {
                LogService.LogException(
                    $"PreAppControl.BindItems failed: {ex.Message}");
            }
        }

        private static PreAppItemModel MapItem(SPListItem item, string buttonText)
        {
            // Exim_GeneralLink is a URL field — strip the optional comma-separated description.
            string rawLink = item["Exim_GeneralLink"]?.ToString() ?? string.Empty;
            string itemLink = !string.IsNullOrEmpty(rawLink)
                ? rawLink.Split(',')[0]
                : string.Empty;

            // EXIM_OpenInNewTab is stored as a localised Yes/No choice field.
            // The JS checks for both the English ("Yes") and Arabic ("نعم") values.
            string openInNewTabRaw = item["EXIM_OpenInNewTab"]?.ToString() ?? string.Empty;
            bool openInNewTab = openInNewTabRaw == "Yes" || openInNewTabRaw == "نعم";

            return new PreAppItemModel
            {
                Title = item["Title"]?.ToString() ?? string.Empty,
                Description = item["_Comments"]?.ToString() ?? string.Empty,
                ItemLink = itemLink,
                OpenInNewTab = openInNewTab,
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
                    "<FieldRef Name='_Comments'/>" +
                    "<FieldRef Name='Exim_GeneralLink'/>" +
                    "<FieldRef Name='EXIM_OpenInNewTab'/>",
                Query =
                    WhereClause + LandingPageHelper.DefaultOrderByClause,
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
            return LandingPageHelper.TryGetList(web, "Products") ;
        }


    }

    // ── View model ───────────────────────────────────────────────────────────────
    public class PreAppItemModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ItemLink { get; set; }
        public bool OpenInNewTab { get; set; }
        public string ButtonText { get; set; }
    }
}
