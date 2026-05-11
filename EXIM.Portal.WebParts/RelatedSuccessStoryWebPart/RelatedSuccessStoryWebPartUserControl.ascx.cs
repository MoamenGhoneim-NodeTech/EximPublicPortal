using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;
using System.Linq;

namespace EXIM.Portal.WebParts.RelatedSuccessStoryWebPart
{
    public partial class RelatedSuccessStoryWebPartUserControl : UserControl
    {

        public RelatedSuccessStoryWebPart WebPartRef = new RelatedSuccessStoryWebPart();
        /// <summary>Maximum carousel items to display.</summary>
        public int MaxItems { get; set; } = 10;

        /// <summary>LCID: 1025 = Arabic, else English.</summary>
        public int CurrentLanguage { get; set; }
            = System.Globalization.CultureInfo.CurrentUICulture.LCID;

        /// <summary>
        /// Optional message shown when no related items are found.
        /// Leave empty (default) to hide the section silently.
        /// </summary>
        public string NoResultsMessage { get; set; } = string.Empty;

        // ── Helpers ──────────────────────────────────────────────────────

        protected bool IsArabic => CurrentLanguage == 1025;

        // ── Runtime state (auto-resolved from current page) ──────────────

        private string CurrentStoryType { get; set; } = string.Empty;
        private string CurrentPageFileRef { get; set; } = string.Empty;

        // ── CAML ─────────────────────────────────────────────────────────

        private const string ArchiveCondition =
            "<Eq><FieldRef Name='EXIM_ShowInArchive'/><Value Type='Boolean'>1</Value></Eq>";

        // ── Lifecycle ────────────────────────────────────────────────────

        protected void Page_Load(object sender, EventArgs e)
        {
            ReadCurrentPageProperties();
            var items = FetchRelatedItems();
            RenderItems(items);
        }

        // ── Step 1: read current page properties ─────────────────────────

        /// <summary>
        /// Reads the success-story type and FileRef from the current
        /// publishing page via SPContext.Current.ListItem.
        /// </summary>
        private void ReadCurrentPageProperties()
        {
            try
            {
                var item = SPContext.Current?.ListItem;
                if (item == null) return;

                CurrentPageFileRef = item["FileRef"]?.ToString() ?? string.Empty;
                CurrentStoryType = "";// ReadField(item, "EXIM_SuccessStoryType");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(
                    "[RelatedSuccessStoryControl] ReadCurrentPageProperties: {0}", ex);
            }
        }

        // ── Step 2: fetch related items ──────────────────────────────────

        private IList<IDictionary<string, string>> FetchRelatedItems()
        {
            var results = new List<IDictionary<string, string>>();

            // Nothing to query if the current page has no story type set
            //if (string.IsNullOrWhiteSpace(CurrentStoryType))
            //    return results;

            try
            {
                var web = SPContext.Current.Web;
                var site = SPContext.Current.Site;

                var list = ResolveTargetList(web);
                if (list == null) return results;

                var query = new SPQuery
                {
                    RowLimit = (uint)(MaxItems + 1), // +1 to cover self-exclusion
                    Query = BuildWhereClause() + LandingPageHelper.DefaultOrderByClause, // 
                    QueryThrottleMode = SPQueryThrottleOption.Override
                };

                foreach (SPListItem item in list.GetItems(query))
                {
                    // Exclude the current page from related results
                    string fileRef = item["FileRef"]?.ToString() ?? "";
                    if (fileRef.Equals(CurrentPageFileRef, StringComparison.OrdinalIgnoreCase))
                        continue;

                    results.Add(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        ["Path"] = BuildAbsoluteUrl(site, fileRef),
                        ["Title"] = item["Title"]?.ToString() ?? "",
                        ["Desc"] = item["Comments"]?.ToString() ?? "",
                        ["StoryType"] = ReadField(item, "EXIM_SuccessStoryType"),
                        ["Image"] = LandingPageHelper.ExtractImageSrc(
                                            item["PublishingRollupImage"]?.ToString(),
                                            "/PublishingImages/DefaultImages/NewsDefaultImg.png")
                    });

                    if (results.Count >= MaxItems) break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(
                    "[RelatedSuccessStoryControl] FetchRelatedItems: {0}", ex);
            }

            return results;
        }

        private SPList ResolveTargetList(SPWeb web)
        {
            // If TargetListTitle property is set, try it first
            if (!string.IsNullOrEmpty(WebPartRef.TargetlistURL))
            {
                var explicitList = LandingPageHelper.TryGetListbyURL(web, WebPartRef.TargetlistURL);
                if (explicitList != null)
                    return explicitList;
            }

            // Fall back to default list names
            return LandingPageHelper.TryGetList(web, "Pages")
                ?? LandingPageHelper.TryGetList(web, "الصفحات");
        }


        /// <summary>
        /// Filters by EXIM_ShowInArchive AND EXIM_SuccessStoryType
        /// matching the current page's story type.
        /// </summary>
        private string BuildWhereClause() =>
            "<Where>" + ArchiveCondition + "</Where>";

        // ── Step 3: render ───────────────────────────────────────────────

        /// <summary>
        /// Renders carousel items and controls section visibility.
        /// No items → section stays hidden (mirrors the original JS check).
        /// </summary>
        private void RenderItems(IList<IDictionary<string, string>> items)
        {
            if (items == null || items.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(NoResultsMessage))
                {
                    phSection.Visible = true;
                    phNoResults.Visible = true;
                    litNoResults.Text = Enc(NoResultsMessage);
                }
                return;
            }

            phSection.Visible = true;

            // Mirrors: var KnowMoreLinkText = rtl ? "معرفة المزيد" : "Know more";
            string knowMore = IsArabic ? "معرفة المزيد" : "Know more";

            foreach (var item in items)
            {
                string url = SafeUrl(Val(item, "Path"));
                string title = Val(item, "Title");
                string desc = Val(item, "Desc");
                string img = Val(item, "Image");

                var html = new StringBuilder();

                // Mirrors the exact markup from Item_EXIM_RelatedSuccessStory.js
                html.Append("<div class=\"item\">");
                html.Append("<div class=\"img\">");
                html.Append($"<a href=\"{Enc(url)}\" class=\"img-hover\">");
                html.Append($"<img src=\"{Enc(img)}\" alt=\"{Enc(title)}\" />");
                html.Append("</a>");
                html.Append("</div>");
                html.Append("<div class=\"blog-data\">");
                html.Append($"<h3>{Enc(title)}</h3>");
                html.Append($"<p>{Enc(desc)}</p>");
                html.Append("<div class=\"blog-more1 mt-auto d-flex\">");
                html.Append($"<a href=\"{Enc(url)}\" class=\"btn btn-secondary\">{Enc(knowMore)}</a>");
                html.Append("</div>");
                html.Append("</div>");
                html.Append("</div>"); // .item

                phItems.Controls.Add(new LiteralControl(html.ToString()));
            }
        }

        // ── Utilities ────────────────────────────────────────────────────

        private static string ReadField(SPListItem item, string field)
        {
            if (!item.Fields.ContainsField(field) || item[field] == null) return "";
            var v = item[field];
            if (v is SPFieldLookupValue lv) return lv.LookupValue;
            if (v is SPFieldLookupValueCollection mc) return string.Join(", ", mc.Cast<SPFieldLookupValue>().Select(x => x.LookupValue));
            return v.ToString();
        }

        private static string Val(IDictionary<string, string> d, string k) =>
            d != null && d.TryGetValue(k, out var v) ? (v ?? "") : "";

        private static string BuildAbsoluteUrl(SPSite site, string rel)
        {
            if (string.IsNullOrWhiteSpace(rel)) return "#";
            var u = new Uri(site.Url);
            return $"{u.Scheme}://{u.Host}{(u.IsDefaultPort ? "" : ":" + u.Port)}{rel}";
        }

        private static string SafeUrl(string url) =>
            string.IsNullOrWhiteSpace(url) ? "#" :
            url.StartsWith("http", StringComparison.OrdinalIgnoreCase) ||
            url.StartsWith("mailto", StringComparison.OrdinalIgnoreCase) ? url : "#";

        private static string Enc(string v) => System.Web.HttpUtility.HtmlEncode(v ?? "");
    }
}
