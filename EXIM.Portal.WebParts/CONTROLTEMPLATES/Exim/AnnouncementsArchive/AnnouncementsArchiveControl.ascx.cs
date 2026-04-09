using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.Utils;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.AnnouncementsArchive
{
    public partial class AnnouncementsArchiveControl : UserControl
    {


        // News pages are always filtered on EXIM_ShowInArchive.
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
            SPList list = ResolveTargetList(web);// web.Lists.TryGetList("Pages");

            if (list == null) return;

            int currentPage = LandingPageHelper.GetCurrentPage(Request);
            SPQuery query = BuildPagedQuery(list, currentPage);

            var data = new List<StudyItemModel>();

            foreach (SPListItem item in list.GetItems(query))
            {

                string rawDate = item["ArticleStartDate"]?.ToString() ?? string.Empty;
                string articleDate = string.Empty;
                if (!string.IsNullOrEmpty(rawDate) &&
                    DateTime.TryParse(rawDate, out DateTime parsedDate))
                {
                    articleDate = parsedDate.ToString("dd MMM yyyy");
                }
                data.Add(new StudyItemModel
                {
                    Title = item["Title"]?.ToString() ?? "",
                    Description = item["Comments"]?.ToString() ?? "",
                    ItemUrl = item["FileRef"]?.ToString() ?? "#",
                    ArticleDate = articleDate,
                    ButtonText = LandingPageHelper.IsEnglish() ? "Know more" : "معرفة المزيد"
                });
            }

            rptItems.DataSource = data;
            rptItems.DataBind();
        }

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
                Query =
                    WhereClause + OrderbyClause,
                QueryThrottleMode = SPQueryThrottleOption.Override
            };

            if (page > 1)
                query.ListItemCollectionPosition =
                    LandingPageHelper.GetPagePosition(list, WhereClause, page, OrderbyClause);

            return query;
        }

        private SPList ResolveTargetList(SPWeb web) =>
          LandingPageHelper.TryGetList(web, "Pages")
          ?? LandingPageHelper.TryGetList(web, "الصفحات");

    }
    public class StudyItemModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ArticleDate { get; set; }
        public string ItemUrl { get; set; }
        public string ButtonText { get; set; }
    }
}
