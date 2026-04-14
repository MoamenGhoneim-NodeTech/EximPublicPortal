using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.FAQ
{
    public partial class FAQControl : UserControl
    {
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
                LandingPageHelper.LogError("FAQControl: target list not found.");
                return;
            }

            try
            {
                int currentPage = LandingPageHelper.GetCurrentPage(Request);
                SPQuery query = BuildPagedQuery(list, currentPage);

                var dataSource = new List<FAQItemModel>();
                foreach (SPListItem item in list.GetItems(query))
                {
                    dataSource.Add(new FAQItemModel
                    {
                        ItemId = item.ID,
                        Title = item["Title"]?.ToString() ?? string.Empty,
                        Answer = item["Answer"]?.ToString() ?? string.Empty
                    });
                }

                rptFAQItems.DataSource = dataSource;
                rptFAQItems.DataBind();

                RenderPagination(list, currentPage);
            }
            catch (Exception ex)
            {
                LandingPageHelper.LogError(
                    $"FAQControl.BindItems failed: {ex.Message}");
            }
        }

        // ── CAML helpers ─────────────────────────────────────────────────────────
        private static SPQuery BuildPagedQuery(SPList list, int page)
        {
            var query = new SPQuery
            {
                RowLimit = (uint)LandingPageHelper.PageSize,
                ViewFields =
                    "<FieldRef Name='ID'/>" +
                    "<FieldRef Name='Title'/>" +
                    "<FieldRef Name='Answer'/>",
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
        private SPList ResolveTargetList(SPWeb web) =>
            LandingPageHelper.TryGetList(web, "FAQ")
            ?? LandingPageHelper.TryGetList(web, "الأسئلة الشائعة");
    }

    // ── View model ───────────────────────────────────────────────────────────────
    public class FAQItemModel
    {
        public int ItemId { get; set; }
        public string Title { get; set; }
        public string Answer { get; set; }
    }
}

