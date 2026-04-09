using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.Utils;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.YearlyReports
{
    public partial class YearlyReportsControl : UserControl
    {
        private const string WhereClause =
           "<Where><Eq><FieldRef Name='EXIM_ShowInArchive'/><Value Type='Boolean'>1</Value></Eq></Where>";
        
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

            var data = new List<ReportItemModel>();

            try
            {
                SPQuery query = new SPQuery
                {
                    RowLimit = (uint)LandingPageHelper.PageSize,
                    ViewFields =
                        "<FieldRef Name='Title'/>" +
                        "<FieldRef Name='FileRef'/>",
                    Query = LandingPageHelper.DefaultOrderByClause
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
                LandingPageHelper.LogError("ReportControl.BindItems: " + ex.Message);
            }
        }

        private ReportItemModel MapItem(SPListItem item)
        {
            string itemUrl = item["FileRef"]?.ToString() ?? "#";

            string encoded = HttpUtility.UrlEncode(itemUrl);

            string currentPath = HttpContext.Current.Request.Url.AbsolutePath.ToLower();

            string reportDetailsUrl;

            if (currentPath.Contains("/pages/"))
            {
                reportDetailsUrl = "reportDetails.aspx?src=" + encoded;
            }
            else
            {
                reportDetailsUrl = "Reports/Pages/reportDetails.aspx?src=" + encoded;
            }

            return new ReportItemModel
            {
                Title = item["Title"]?.ToString() ?? "",
                ItemUrl = itemUrl,
                ReportDetailsUrl = reportDetailsUrl,
                DownloadText = LandingPageHelper.IsEnglish() ? "Download report" : "تحميل التقرير",
                ViewText = LandingPageHelper.IsEnglish() ? "View Report" : "عرض التقرير"
            };
        }
        private SPList ResolveTargetList(SPWeb web) =>
             LandingPageHelper.TryGetList(web, "Documents")
             ?? LandingPageHelper.TryGetList(web, "المستندات");


    }

    public class ReportItemModel
    {
        public string Title { get; set; }
        public string ItemUrl { get; set; }
        public string DownloadText { get; set; }
        public string ViewText { get; set; }
        public string ReportDetailsUrl { get; set; }
    }
}
