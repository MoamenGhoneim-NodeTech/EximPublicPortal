using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;
using EXIM.Common.Lib.Utils;

namespace EXIM.Portal.WebParts.WorkshopArchiveWebPart
{
    public partial class WorkshopArchiveWebPartUserControl : UserControl
    {
        public WorkshopArchiveWebPart WebPartRef { get; set; }
        private const string DefaultImage =
      "/PublishingImages/DefaultImages/WorkShopDefaultImg.png";

        // Workshop pages are filtered on EXIM_ShowInArchive.
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
                LogService.LogException("WorkshopArchiveControl: target list not found.");
                return;
            }

            try
            {
                int currentPage = LandingPageHelper.GetCurrentPage(Request);
                SPQuery query = BuildPagedQuery(list, currentPage);

                var dataSource = new List<EventItemModel>();
                foreach (SPListItem item in list.GetItems(query))
                    dataSource.Add(MapItem(item));

                rptItems.DataSource = dataSource;
                rptItems.DataBind();

                RenderPagination(list, currentPage);
            }
            catch (Exception ex)
            {
                LogService.LogException(
                    $"WorkshopArchiveControl.BindItems failed: {ex.Message}");
            }
        }

        private static EventItemModel MapItem(SPListItem item)
        {
            DateTime? eventDate = null;
            if (DateTime.TryParse(item["ArticleStartDate"]?.ToString(), out DateTime parsed))
                eventDate = parsed;

            var status = CalculateEventStatus(eventDate);

            return new EventItemModel
            {
                Title = item["Title"]?.ToString() ?? string.Empty,
                EventType = item["Exim_WorkshopType"]?.ToString() ?? string.Empty,
                EventLocation = item["Exim_WorkshopLocation"]?.ToString() ?? string.Empty,
                EventUrl = item["FileRef"]?.ToString() ?? "#",
                ImgPath = LandingPageHelper.ExtractImageSrc(
                                    item["PublishingRollupImage"]?.ToString(), DefaultImage),
                EventDate = eventDate.HasValue
                                    ? eventDate.Value.ToString("dd MMM yyyy")
                                    : string.Empty,
                StatusText = LandingPageHelper.IsEnglish() ? status.En : status.Ar,
                StatusClass = status.CssClass,
                ButtonText = LandingPageHelper.IsEnglish() ? "Register now" : "سجل الان",
                DisableButton = status.CssClass == "status-inactive"
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
                    "<FieldRef Name='FileRef'/>" +
                    "<FieldRef Name='PublishingRollupImage'/>" +
                    "<FieldRef Name='Exim_WorkshopType'/>" +
                    "<FieldRef Name='Exim_WorkshopLocation'/>" +
                    "<FieldRef Name='ArticleStartDate'/>",
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

        // ── Event status ─────────────────────────────────────────────────────────
        private static (string Ar, string En, string CssClass) CalculateEventStatus(
            DateTime? eventDate)
        {
            if (!eventDate.HasValue)
                return ("غير محدد", "Not specified", "status-active");

            DateTime date = eventDate.Value.Date;
            DateTime today = DateTime.Today;

            if (date < today) return ("انتهت", "Ended", "status-inactive");
            if (date > today) return ("قادمة", "Upcoming", "status-active");
            return ("حالية", "Current", "status-active");
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
    }

    // ── View model ───────────────────────────────────────────────────────────────
    public class EventItemModel
    {
        public string Title { get; set; }
        public string EventType { get; set; }
        public string EventLocation { get; set; }
        public string EventDate { get; set; }
        public string EventUrl { get; set; }
        public string ImgPath { get; set; }
        public string StatusText { get; set; }
        public string StatusClass { get; set; }
        public string ButtonText { get; set; }
        public bool DisableButton { get; set; }
    }
}
