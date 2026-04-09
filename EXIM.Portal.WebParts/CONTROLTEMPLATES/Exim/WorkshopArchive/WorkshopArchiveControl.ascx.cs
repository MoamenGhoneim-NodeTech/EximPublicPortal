using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.Utils;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.WorkshopArchive
{
    public partial class WorkshopArchiveControl : UserControl
    {
        private const string DefaultImage = "/PublishingImages/DefaultImages/WorkShopDefaultImg.png";
        private const string WhereClause =
            "<Where><Eq><FieldRef Name='EXIM_ShowInArchive'/><Value Type='Boolean'>1</Value></Eq></Where>";
        private const string OrderbyClause = "<OrderBy> <FieldRef Name='ArticleStartDate' Ascending='False' /> </OrderBy>";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindItems();
        }
        
        private void BindItems()
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = ResolveTargetList(web);

            if (list == null) return;

            var data = new List<EventItemModel>();

            try
            {
                SPQuery query = new SPQuery
                {
                    RowLimit = (uint)LandingPageHelper.PageSize,
                    ViewFields =
                        "<FieldRef Name='Title'/>" +
                        "<FieldRef Name='FileRef'/>" +
                        "<FieldRef Name='PublishingRollupImage'/>" +
                        "<FieldRef Name='Exim_WorkshopType'/>" +
                        "<FieldRef Name='Exim_WorkshopLocation'/>" +
                        "<FieldRef Name='ArticleStartDate'/>",
                         Query = WhereClause + OrderbyClause

                };

                foreach (SPListItem item in list.GetItems(query))
                {
                    data.Add(MapItem(item));
                }

                rptItems.DataSource = data;
                rptItems.DataBind();
            }
            catch (Exception ex)
            {
                LandingPageHelper.LogError("EventControl.BindItems: " + ex.Message);
            }
        }

        private SPList ResolveTargetList(SPWeb web) =>
              LandingPageHelper.TryGetList(web, "Pages")
              ?? LandingPageHelper.TryGetList(web, "الصفحات");



        private EventItemModel MapItem(SPListItem item)
        {
            DateTime? eventDate = null;

            if (DateTime.TryParse(item["ArticleStartDate"]?.ToString(), out DateTime parsed))
                eventDate = parsed;

            var status = CalculateEventStatus(eventDate);

            return new EventItemModel
            {
                Title = item["Title"]?.ToString() ?? "",
                EventType = item["Exim_WorkshopType"]?.ToString() ?? "",
                EventLocation = item["Exim_WorkshopLocation"]?.ToString() ?? "",
                EventUrl = item["FileRef"]?.ToString() ?? "#",
                ImgPath = LandingPageHelper.ExtractImageSrc(
                    item["PublishingRollupImage"]?.ToString(), DefaultImage),

                EventDate = eventDate.HasValue
                    ? eventDate.Value.ToString("dd MMM yyyy")
                    : "",

                StatusText = LandingPageHelper.IsEnglish() ? status.En : status.Ar,
                StatusClass = status.CssClass,

                ButtonText = LandingPageHelper.IsEnglish() ? "Register now" : "سجل الان",
                DisableButton = status.CssClass == "status-inactive"
            };
        }

        private (string Ar, string En, string CssClass) CalculateEventStatus(DateTime? eventDate)
        {
            if (!eventDate.HasValue)
            {
                return ("غير محدد", "Not specified", "status-active");
            }

            DateTime today = DateTime.Today;
            DateTime date = eventDate.Value.Date;

            if (date < today)
            {
                return ("انتهت", "Ended", "status-inactive");
            }
            else if (date > today)
            {
                return ("قادمة", "Upcoming", "status-active");
            }
            else
            {
                return ("حالية", "Current", "status-active");
            }
        }
    }

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
