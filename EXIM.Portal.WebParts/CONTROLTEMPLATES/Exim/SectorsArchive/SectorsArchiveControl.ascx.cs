using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.SectorsArchive
{
    public partial class SectorsArchiveControl : UserControl
    {
        private const string DefaultImage =
    "/PublishingImages/DefaultImages/SectorsDefaultImg.png";

        // Sectors are filtered on EXIM_Visibility.
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
                LandingPageHelper.LogError("SectorsArchiveControl: target list not found.");
                return;
            }

            try
            {
                // Sectors are intentionally not paged — all visible sectors are shown.
                var query = new SPQuery
                {
                    ViewFields =
                        "<FieldRef Name='Title'/>" +
                        "<FieldRef Name='Exim_GeneralLink'/>" +
                        "<FieldRef Name='PublishingRollupImage'/>" +
                        "<FieldRef Name='EXIM_OpenInNewTab'/>",
                    Query = WhereClause + LandingPageHelper.DefaultOrderByClause,
                    QueryThrottleMode = SPQueryThrottleOption.Override
                };

                var dataSource = new List<SectorItemModel>();
                foreach (SPListItem item in list.GetItems(query))
                    dataSource.Add(MapItem(item));

                rptSectorItems.DataSource = dataSource;
                rptSectorItems.DataBind();
            }
            catch (Exception ex)
            {
                LandingPageHelper.LogError(
                    $"SectorsArchiveControl.BindItems failed: {ex.Message}");
            }
        }

        private static SectorItemModel MapItem(SPListItem item)
        {
            // Exim_GeneralLink is a URL field — strip the optional comma-separated description.
            string rawLink = item["Exim_GeneralLink"]?.ToString() ?? string.Empty;
            string itemLink = !string.IsNullOrEmpty(rawLink)
                ? rawLink.Split(',')[0]
                : string.Empty;

            // EXIM_OpenInNewTab is a localised Yes/No choice field.
            // bool.TryParse only handles "True"/"False"; check for the actual
            // field values ("Yes" / "نعم") to mirror EligibilityCheckControl.
            string openInNewTabRaw = item["EXIM_OpenInNewTab"]?.ToString() ?? string.Empty;
            bool openInNewTab = openInNewTabRaw == "Yes" || openInNewTabRaw == "نعم";

            return new SectorItemModel
            {
                Title = item["Title"]?.ToString() ?? string.Empty,
                ItemLink = itemLink,
                ImgPath = LandingPageHelper.ExtractImageSrc(
                                  item["PublishingRollupImage"]?.ToString(), DefaultImage),
                OpenInNewTab = openInNewTab
            };
        }

        // ── List resolution ──────────────────────────────────────────────────────
        private SPList ResolveTargetList(SPWeb web) =>
            LandingPageHelper.TryGetList(web, "Sectors")
            ?? LandingPageHelper.TryGetList(web, "القطاعات");
    }

    // ── View model ───────────────────────────────────────────────────────────────
    public class SectorItemModel
    {
        public string Title { get; set; }
        public string ItemLink { get; set; }
        public string ImgPath { get; set; }
        public bool OpenInNewTab { get; set; }
    }
}

