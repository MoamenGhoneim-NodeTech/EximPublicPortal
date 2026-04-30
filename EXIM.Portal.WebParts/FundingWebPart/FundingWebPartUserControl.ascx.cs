using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;
using EXIM.Common.Lib.Utils;

namespace EXIM.Portal.WebParts.FundingWebPart
{
    public partial class FundingWebPartUserControl : UserControl
    {
        public FundingWebPart WebPartRef { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindItems();
        }


        private void BindItems()
        {
            SPWeb web = SPContext.Current.Web;

            // Try both English & Arabic list names like your pattern
            SPList list = ResolveTargetList(web);

            if (list == null) return;

            var data = new List<FundingItemModel>();

            try
            {
                SPQuery query = new SPQuery
                {
                    RowLimit = (uint)LandingPageHelper.PageSize,
                    ViewFields =
                        "<FieldRef Name='Title'/>" +
                        "<FieldRef Name='URL'/>" +
                        "<FieldRef Name='_Comments'/>"
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
                LogService.LogException("FundingControl.BindItems: " + ex.Message);
            }
        }
        private FundingItemModel MapItem(SPListItem item)
        {
            string urlField = item["URL"]?.ToString() ?? "";

            // Handle hyperlink field (same as your other controls)
            string serviceUrl = !string.IsNullOrEmpty(urlField)
                ? urlField.Split(',')[0]
                : "#";

            return new FundingItemModel
            {
                ServiceTitle = item["Title"]?.ToString() ?? "",
                ServiceDescription = item["_Comments"]?.ToString() ?? "",
                ServiceURL = serviceUrl
            };
        }

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
            return LandingPageHelper.TryGetList(web, "Financing");
        }

    }
    public class FundingItemModel
    {
        public string ServiceURL { get; set; }
        public string ServiceTitle { get; set; }
        public string ServiceDescription { get; set; }
    }
}

