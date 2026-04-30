using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;
using EXIM.Common.Lib.Utils;

namespace EXIM.Portal.WebParts.BoardMembersWebPart
{
    public partial class BoardMembersWebPartUserControl : UserControl
    {
        public BoardMembersWebPart WebPartRef { get; set; }

        private const string DefaultImage =
        "/PublishingImages/DefaultImages/BoardMemberDefaultImg.png";

        // Board members are filtered on EXIM_Visibility.
        // The list is intentionally not paged — all visible members are shown
        // so the carousel always has the full set of slides.
        private const string WhereClause =
            "<Where><Eq><FieldRef Name='EXIM_Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>";

        // ── Lifecycle ────────────────────────────────────────────────────────────
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            SetHeadings();
            BindItems();
        }

        // ── Headings ─────────────────────────────────────────────────────────────
        /// <summary>
        /// Renders the section headings in the current UI language,
        /// matching the renderHeader() output of the original CSR script.
        /// </summary>
        private void SetHeadings()
        {
            litAboutText.Text = LandingPageHelper.IsEnglish()
                ? "About the bank" : "عن البنك";

            litBoardText.Text = LandingPageHelper.IsEnglish()
                ? "Board of Directors" : "مجلس الإدارة";
        }

        // ── Data binding ─────────────────────────────────────────────────────────
        private void BindItems()
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = ResolveTargetList(web);

            if (list == null)
            {
                LogService.LogException("BoardMembersControl: target list not found.");
                return;
            }

            try
            {
                SPQuery query = BuildQuery();

                var dataSource = new List<BoardMemberModel>();
                foreach (SPListItem item in list.GetItems(query))
                    dataSource.Add(MapItem(item));

                rptBoardMembers.DataSource = dataSource;
                rptBoardMembers.DataBind();
            }
            catch (Exception ex)
            {
                LogService.LogException(
                    $"BoardMembersControl.BindItems failed: {ex.Message}");
            }
        }

        private static BoardMemberModel MapItem(SPListItem item)
        {
            string title = item["Title"]?.ToString() ?? string.Empty;
            string name = item["Name"]?.ToString() ?? string.Empty;
            string jobTitle = item["JobTitle"]?.ToString() ?? string.Empty;
            string imgPath = LandingPageHelper.ExtractImageSrc(
                                  item["PublishingRollupImage"]?.ToString(), DefaultImage);

            return new BoardMemberModel
            {
                Title = title,
                Name = name,
                JobTitle = jobTitle,
                ImgPath = imgPath,
                // Use name as the primary alt text, fall back to title —
                // mirrors the accessibility fix applied in the JS version.
                AltText = !string.IsNullOrEmpty(name) ? name : title
            };
        }

        // ── CAML query ───────────────────────────────────────────────────────────
        private static SPQuery BuildQuery()
        {
            // No RowLimit — all visible board members are fetched so the
            // carousel always has the complete slide set.
            return new SPQuery
            {
                ViewFields =
                    "<FieldRef Name='Title'/>" +
                    "<FieldRef Name='Name'/>" +
                    "<FieldRef Name='JobTitle'/>" +
                    "<FieldRef Name='PublishingRollupImage'/>",
                Query = WhereClause + LandingPageHelper.DefaultOrderByClause,
                QueryThrottleMode = SPQueryThrottleOption.Override
            };
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
            return LandingPageHelper.TryGetList(web, "BoardMembers")
            ?? LandingPageHelper.TryGetList(web, "أعضاء مجلس الإدارة");
        }
    }

    // ── View model ───────────────────────────────────────────────────────────────
    public class BoardMemberModel
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string ImgPath { get; set; }
        public string AltText { get; set; }
    }

}
