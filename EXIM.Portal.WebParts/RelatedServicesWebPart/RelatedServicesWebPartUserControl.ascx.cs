using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;

namespace EXIM.Portal.WebParts.RelatedServicesWebPart
{
    public partial class RelatedServicesWebPartUserControl : UserControl
    {

        /// <summary>Maximum number of related items to show in the carousel.</summary>
        public int MaxItems { get; set; } = 10;

        public int CurrentLanguage { get; set; }
            = System.Globalization.CultureInfo.CurrentUICulture.LCID;
        public string NoResultsMessage { get; set; } = string.Empty;
        private string CurrentServiceType { get; set; } = string.Empty;
        private string CurrentPageFileRef { get; set; } = string.Empty;
        protected bool IsArabic => CurrentLanguage == 1025;

        private const string ArchiveField =
            "<Eq><FieldRef Name='EXIM_ShowInArchive'/><Value Type='Boolean'>1</Value></Eq>";
        public RelatedServicesWebPart WebPartRef { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // ── Step 1: Read the current page's properties from SPContext ──
            //
            // SPContext.Current.ListItem is the SPListItem of the page that
            // is currently being rendered — this is always available on
            // publishing pages inside the Pages library.
            // We read EXIMFinSrvServiceType directly from it, so no one
            // needs to pass the value in from outside.

            ResolveCurrentPageProperties();

            // ── Step 2: Fetch related items and render ─────────────────
            var items = FetchRelatedItems();
            RenderItems(items);
        }

        private void ResolveCurrentPageProperties()
        {
            try
            {
                SPListItem currentItem = SPContext.Current?.ListItem;

                if (currentItem == null)
                    return;

                // FileRef — used to exclude this page from results
                CurrentPageFileRef = currentItem["FileRef"]?.ToString() ?? string.Empty;

                // EXIMFinSrvServiceType — the filter value for related items
                if (!string.IsNullOrEmpty(WebPartRef.FinancialServiceType))
                    CurrentServiceType = WebPartRef.FinancialServiceType;
                else
                    CurrentServiceType = GetFieldValue(currentItem, IsArabic ? "Exim_FinSrv_ServiceType" : "Exim_FinSrv_ServiceType_En");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(
                    "[RelatedFinancialServiceControl] ResolveCurrentPageProperties error: {0}", ex);
            }
        }
        private IList<IDictionary<string, string>> FetchRelatedItems()
        {
            var results = new List<IDictionary<string, string>>();

            try
            {
                SPWeb web = SPContext.Current.Web;
                SPSite site = SPContext.Current.Site;

                SPList pagesList = ResolveTargetList(web);
                if (pagesList == null) return results;

                // Only bother querying if we have a service type to filter by
                if (string.IsNullOrWhiteSpace(CurrentServiceType))
                    return results;

                SPQuery query = BuildQuery();
                SPListItemCollection items = pagesList.GetItems(query);

                foreach (SPListItem item in items)
                {
                    string fileRef = item["FileRef"]?.ToString() ?? "";

                    // Exclude the current page from related results
                    if (!string.IsNullOrWhiteSpace(CurrentPageFileRef) &&
                        fileRef.Equals(CurrentPageFileRef, StringComparison.OrdinalIgnoreCase))
                        continue;

                    string absoluteUrl = BuildAbsoluteUrl(site, fileRef);

                    results.Add(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        ["Path"] = absoluteUrl,
                        ["Title"] = item["Title"]?.ToString() ?? "",
                        ["CommentsOWSMTXT"] = item["Comments"]?.ToString() ?? "",
                        ["EXIMFinSrvServiceType"] = GetFieldValue(item, IsArabic ? "Exim_FinSrv_ServiceType" : "Exim_FinSrv_ServiceType_En")
                    });

                    if (results.Count >= MaxItems) break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(
                    "[RelatedFinancialServiceControl] FetchRelatedItems error: {0}", ex);
            }

            return results;
        }
        private SPQuery BuildQuery()
        {
            string whereClause = "";
            if (CurrentServiceType != "NoFilter")
            {
                whereClause = "<Where><And>" +
                    ArchiveField +
                    "<Eq>" +
                        "<FieldRef Name='" + (IsArabic ? "Exim_FinSrv_ServiceType" : "Exim_FinSrv_ServiceType_En") + "' />" +
                        $"<Value Type='Text'>{System.Security.SecurityElement.Escape(CurrentServiceType)}</Value>" +
                    "</Eq>" +
                "</And></Where>";
            }
         
            return new SPQuery
            {
                // Fetch one extra item to account for self-exclusion
                RowLimit = (uint)(MaxItems + 1),
                Query = whereClause + LandingPageHelper.DefaultOrderByClause,
                QueryThrottleMode = SPQueryThrottleOption.Override
            };
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
        private void RenderItems(IList<IDictionary<string, string>> items)
        {
            if (items == null || items.Count == 0)
            {
                // Show an optional no-results message; otherwise hide silently
                if (!string.IsNullOrWhiteSpace(NoResultsMessage))
                {
                    phRelatedSection.Visible = true;
                    phNoResults.Visible = true;
                    litNoResults.Text = HtmlEnc(NoResultsMessage);
                }
                // phRelatedSection.Visible remains false → section hidden
                return;
            }

            phRelatedSection.Visible = true;

            foreach (var props in items)
            {
                string sUrl = EnsureAllowedProtocol(GetProp(props, "Path"));
                string sTitle = GetProp(props, "Title");
                string sDesc = GetProp(props, "CommentsOWSMTXT");
                string sType = GetProp(props, "EXIMFinSrvServiceType");

                var card = new StringBuilder();

                card.Append(@"<div class=""services-item"">");

                card.AppendFormat(
                    @"<h3><a href=""{0}"">{1}</a></h3>",
                    HtmlEnc(sUrl), HtmlEnc(sTitle));

                if (!string.IsNullOrWhiteSpace(sDesc))
                    card.AppendFormat(@"<p>{0}</p>", HtmlEnc(sDesc));

                card.AppendFormat(
                    @"<a href=""{0}"" class=""services-item-footer"">
                        <span class=""service-category color-blue"">{1}</span>
                        <i class=""ic-left-arrow-dark""></i>
                    </a>",
                    HtmlEnc(sUrl), HtmlEnc(sType));

                card.Append(@"</div>");

                phResults.Controls.Add(new LiteralControl(card.ToString()));
            }
        }

        private static string GetFieldValue(SPListItem item, string fieldName)
        {
            if (!item.Fields.ContainsField(fieldName) || item[fieldName] == null)
                return "";

            var value = item[fieldName];

            if (value is SPFieldLookupValue lookup)
                return lookup.LookupValue;

            if (value is SPFieldLookupValueCollection multi)
            {
                var vals = new List<string>();
                foreach (SPFieldLookupValue v in multi)
                    vals.Add(v.LookupValue);
                return string.Join(", ", vals);
            }

            return value.ToString();
        }

        private static string GetProp(IDictionary<string, string> dict, string key)
            => dict != null && dict.TryGetValue(key, out var v) ? (v ?? "") : "";

        private static string BuildAbsoluteUrl(SPSite site, string serverRelativeUrl)
        {
            if (string.IsNullOrWhiteSpace(serverRelativeUrl)) return "#";
            var uri = new Uri(site.Url);
            string port = uri.IsDefaultPort ? "" : ":" + uri.Port;
            return $"{uri.Scheme}://{uri.Host}{port}{serverRelativeUrl}";
        }

        private static string EnsureAllowedProtocol(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return "#";
            return url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase)
                ? url : "#";
        }

        private static string HtmlEnc(string v)
            => System.Web.HttpUtility.HtmlEncode(v ?? "");

    }
}
