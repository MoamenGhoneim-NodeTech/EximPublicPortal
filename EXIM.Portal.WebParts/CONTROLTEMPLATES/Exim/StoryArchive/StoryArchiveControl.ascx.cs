using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.Utils;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.StoryArchive
{
    public partial class StoryArchiveControl : UserControl
    {


        private const string WhereClause =
            "<Where><Eq><FieldRef Name='EXIM_ShowInArchive'/><Value Type='Boolean'>1</Value></Eq></Where>";
        private const string OrderbyClause = "<OrderBy> <FieldRef Name='EXIM_ItemOrder' Ascending='true' /> <FieldRef Name ='Created' Ascending='False' /> </OrderBy>";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindItems();
        }

        private const string DefaultImage = "/PublishingImages/DefaultImages/SuccessStoryDefaultImg.png";

        private void BindItems()
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = ResolveTargetList(web);// web.Lists.TryGetList("Pages");

            if (list == null) return;

            var data = new List<StoryItemModel>();

            try
            {
                SPQuery query = new SPQuery
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
                    Query= WhereClause+OrderbyClause
                        
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
                LandingPageHelper.LogError("StoryControl.BindItems: " + ex.Message);
            }
        }

        private StoryItemModel MapItem(SPListItem item)
        {
            string img = LandingPageHelper.ExtractImageSrc(
                item["PublishingRollupImage"]?.ToString(), DefaultImage);

            string logo = LandingPageHelper.ExtractImageSrc(
                item["Exim_SecondaryImage"]?.ToString(), DefaultImage);

            return new StoryItemModel
            {
                Title = item["Title"]?.ToString() ?? "",
                Description = item["Comments"]?.ToString() ?? "",
                ItemUrl = item["FileRef"]?.ToString() ?? "#",
                ImgPath = img,
                LogoPath = logo,
                PersonName = item["Exim_Story_PersonOfInterest"]?.ToString() ?? "",
                PersonWord = item["Exim_Story_PersonWord"]?.ToString() ?? "",
                ButtonText = LandingPageHelper.IsEnglish() ? "Learn more" : "معرفة المزيد"
            };
        }
        private SPList ResolveTargetList(SPWeb web) =>
               LandingPageHelper.TryGetList(web, "Pages")
               ?? LandingPageHelper.TryGetList(web, "الصفحات");


    }

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
