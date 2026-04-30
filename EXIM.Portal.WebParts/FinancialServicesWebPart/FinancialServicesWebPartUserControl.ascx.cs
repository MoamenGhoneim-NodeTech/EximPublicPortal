using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;

namespace EXIM.Portal.WebParts.FinancialServicesWebPart
{
    public partial class FinancialServicesWebPartUserControl : UserControl
    {
        // ── Configuration ────────────────────────────────────────────────
        public FinancialServicesWebPart WebPartRef { get; set; }
        private const string WhereClause =
            "<Where><Eq><FieldRef Name='EXIM_ShowInArchive'/><Value Type='Boolean'>1</Value></Eq></Where>";

   
        public int RowsPerPage { get; set; } = 9;
        public bool ShowPaging { get; set; } = true;
        public string NoResultsMessage { get; set; } = "No results found.";
        public int CurrentLanguage { get; set; } = System.Globalization.CultureInfo.CurrentUICulture.LCID;

        // ── Helpers ──────────────────────────────────────────────────────

        protected bool IsArabic => CurrentLanguage == 1025;
        protected string PagingNavLabel => IsArabic ? "عدد الصفحات المتاحة" : "Available pages";
        protected string TotalPagesLabel { get; private set; } = string.Empty;
        public string DownloadGuideUrl => IsArabic ? "~/ar/FinServices/Documents/ServicesGuide.pdf" : "~/en/FinServices/Documents/ServicesGuide.pdf";
        // ── ViewState filter + paging state ─────────────────────────────
        // All user selections are stored in ViewState so they survive
        // PostBack automatically — no query string or hidden fields needed.

        private string CurrentKeyword
        {
            get => ViewState["VSKeyword"] as string ?? string.Empty;
            set => ViewState["VSKeyword"] = value ?? string.Empty;
        }

        private List<string> SelectedCategories
        {
            get => ParseList(ViewState["VSCats"] as string);
            set => ViewState["VSCats"] = value == null ? "" : string.Join("|", value);
        }

        private List<string> SelectedServiceTypes
        {
            get => ParseList(ViewState["VSSvcs"] as string);
            set => ViewState["VSSvcs"] = value == null ? "" : string.Join("|", value);
        }

        private int CurrentPageIndex
        {
            get => ViewState["VSPage"] as int? ?? 0;
            set => ViewState["VSPage"] = value;
        }

        private static List<string> ParseList(string raw) =>
            string.IsNullOrEmpty(raw) ? new List<string>()
            : raw.Split('|').Where(s => s.Length > 0).ToList();

        // ── Lifecycle ────────────────────────────────────────────────────
        // Page_Load    : UI setup only
        // Event handlers: update ViewState only
        // Page_PreRender: fetch → filter → render (runs after all handlers)

        protected void Page_Load(object sender, EventArgs e)
        {
            btnApply.Text = IsArabic ? "تطبيق" : "Apply";
            btnClear.Text = IsArabic ? "مسح" : "Clear";

            if (!string.IsNullOrWhiteSpace(DownloadGuideUrl))
            {
                hlDownload.NavigateUrl = DownloadGuideUrl;
                hlDownload.Visible = true;
            }

            if (!IsPostBack)
                txtSearch.Text = CurrentKeyword;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var all = FetchItems();
            var filtered = FilterItems(all);
            BuildCheckboxes(all);
            Render(filtered);
        }

        // ── Button handlers (ViewState update only) ──────────────────────

        protected void btnApply_Click(object sender, EventArgs e)
        {
            CurrentKeyword = txtSearch.Text.Trim();
            SelectedCategories = GetCheckedValues("fcat_");
            SelectedServiceTypes = GetCheckedValues("fsvc_");
            CurrentPageIndex = 0;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentKeyword = txtSearch.Text.Trim();
            CurrentPageIndex = 0;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            CurrentKeyword = string.Empty;
            SelectedCategories = new List<string>();
            SelectedServiceTypes = new List<string>();
            CurrentPageIndex = 0;
            txtSearch.Text = string.Empty;
        }

        protected void Pager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page" && int.TryParse(e.CommandArgument.ToString(), out int idx))
                CurrentPageIndex = idx;
        }

        // ── Data retrieval ───────────────────────────────────────────────
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
        
        private IList<IDictionary<string, string>> FetchItems()
        {
            var results = new List<IDictionary<string, string>>();
            try
            {
                var web = SPContext.Current.Web;
                var site = SPContext.Current.Site;
                var list = ResolveTargetList(web);

                if (list == null) return results;

                var query = new SPQuery
                {
                    RowLimit = (uint)(RowsPerPage * 20),
                    Query = WhereClause + LandingPageHelper.DefaultOrderByClause,
                    QueryThrottleMode = SPQueryThrottleOption.Override
                };

                foreach (SPListItem item in list.GetItems(query))
                {
                    results.Add(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        ["Path"] = BuildAbsoluteUrl(site, item["FileRef"]?.ToString()),
                        ["Title"] = item["Title"]?.ToString() ?? "",
                        ["Desc"] = item["Comments"]?.ToString() ?? "",
                        ["SvcType"] = ReadField(item, IsArabic ? "Exim_FinSrv_ServiceType" : "Exim_FinSrv_ServiceType_En"),
                        ["Category"] = ReadField(item, IsArabic ? "Exim_FinSrv_ServiceClass" : "Exim_FinSrv_ServiceClass_En")
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("[FinancialServiceControl] {0}", ex);
            }
            return results;
        }

        // ── Filtering ────────────────────────────────────────────────────

        private IList<IDictionary<string, string>> FilterItems(IList<IDictionary<string, string>> all)
        {
            var keyword = CurrentKeyword.ToLowerInvariant();
            var cats = SelectedCategories;
            var svcs = SelectedServiceTypes;

            return all.Where(item =>
            {
                // Keyword matches title or description
                if (!string.IsNullOrEmpty(keyword))
                {
                    bool inTitle = Val(item, "Title").ToLowerInvariant().Contains(keyword);
                    bool inDesc = Val(item, "Desc").ToLowerInvariant().Contains(keyword);
                    if (!inTitle && !inDesc) return false;
                }

                // Category filter (OR logic — any selected value matches)
                if (cats.Count > 0 && !cats.Contains(Val(item, "Category"), StringComparer.OrdinalIgnoreCase))
                    return false;

                // Service type filter (OR logic)
                if (svcs.Count > 0 && !svcs.Contains(Val(item, "SvcType"), StringComparer.OrdinalIgnoreCase))
                    return false;

                return true;
            }).ToList();
        }

        // ── Sidebar checkboxes ───────────────────────────────────────────

        private void BuildCheckboxes(IList<IDictionary<string, string>> all)
        {
            BuildCheckboxGroup(phCategoryFilters, all.Select(i => Val(i, "Category")), "fcat_", SelectedCategories);
            BuildCheckboxGroup(phServiceTypeFilters, all.Select(i => Val(i, "SvcType")), "fsvc_", SelectedServiceTypes);
        }

        private static void BuildCheckboxGroup(PlaceHolder ph, IEnumerable<string> values, string prefix, List<string> selected)
        {
            ph.Controls.Clear();
            foreach (var v in values.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x))
            {
                string chk = selected.Contains(v, StringComparer.OrdinalIgnoreCase) ? " checked=\"checked\"" : "";
                ph.Controls.Add(new LiteralControl(
                    $"<label class=\"exim-check\">" +
                    $"<input type=\"checkbox\" name=\"{Enc(prefix + v)}\" value=\"{Enc(v)}\"{chk} />" +
                    $"<span>{Enc(v)}</span></label>"));
            }
        }

        // ── Render ───────────────────────────────────────────────────────

        private void Render(IList<IDictionary<string, string>> filtered)
        {
            int total = filtered.Count;
            int totalPages = RowsPerPage > 0 ? (int)Math.Ceiling((double)total / RowsPerPage) : 0;

            // Clamp page index
            CurrentPageIndex = Math.Max(0, Math.Min(CurrentPageIndex, totalPages > 0 ? totalPages - 1 : 0));

            RenderCards(filtered.Skip(CurrentPageIndex * RowsPerPage).Take(RowsPerPage).ToList());
            RenderNoResults(total);
            RenderPaging(totalPages);
        }

        private void RenderCards(IList<IDictionary<string, string>> items)
        {
            phResults.Controls.Clear();
            foreach (var item in items)
            {
                string url = SafeUrl(Val(item, "Path"));
                string title = Val(item, "Title");
                string desc = Val(item, "Desc");
                string type = Val(item, "SvcType");

                var html = new StringBuilder();
                html.Append($"<div class=\"col-md-4 pagingItem\"><div class=\"services-item\">");
                html.Append($"<h3><a href=\"{Enc(url)}\">{Enc(title)}</a></h3>");
                if (!string.IsNullOrWhiteSpace(desc))
                    html.Append($"<p>{Enc(desc)}</p>");
                html.Append($"<a href=\"{Enc(url)}\" class=\"services-item-footer\">");
                html.Append($"<span class=\"service-category color-blue\">{Enc(type)}</span>");
                html.Append($"<i class=\"ic-left-arrow-dark\"></i></a>");
                html.Append($"</div></div>");
                phResults.Controls.Add(new LiteralControl(html.ToString()));
            }
        }

        private void RenderNoResults(int total)
        {
            phNoResults.Visible = total == 0;
            if (total == 0) litNoResults.Text = Enc(NoResultsMessage);
        }

        // ── Paging ───────────────────────────────────────────────────────
        // rptPager lives inside asp:Panel (not PlaceHolder) so it stays in
        // the control tree even when pnlPaging is hidden — this is what
        // makes Pager_ItemCommand fire reliably on every PostBack.

        private void RenderPaging(int totalPages)
        {
            pnlPaging.Visible = ShowPaging && totalPages > 1;

            if (!pnlPaging.Visible)
            {
                rptPager.DataSource = null;
                rptPager.DataBind();
                return;
            }

            TotalPagesLabel = IsArabic ? $"إجمالي {totalPages} صفحات" : $"Total {totalPages} pages";

            var items = new List<PagerItem>();

            // Previous arrow
            items.Add(PagerItem.Nav("<i class=\"fas fa-angle-right\"></i>", CurrentPageIndex - 1, CurrentPageIndex > 0));

            // Page numbers
            for (int p = 0; p < totalPages; p++)
                items.Add(PagerItem.Page(p, p == CurrentPageIndex));

            // Next arrow
            items.Add(PagerItem.Nav("<i class=\"fas fa-angle-left\"></i>", CurrentPageIndex + 1, CurrentPageIndex < totalPages - 1));

            rptPager.DataSource = items;
            rptPager.DataBind();
        }

        // ── Small helpers ────────────────────────────────────────────────

        private List<string> GetCheckedValues(string prefix) =>
            (Request.Form.AllKeys ?? new string[0])
                .Where(k => k != null && k.StartsWith(prefix, StringComparison.Ordinal))
                .Select(k => k.Substring(prefix.Length))
                .ToList();

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

    // ── Pager item model ─────────────────────────────────────────────────

    public class PagerItem
    {
        public string Label { get; set; }
        public int PageIndex { get; set; }
        public string CssClass { get; set; }
        public bool Enabled { get; set; }

        public static PagerItem Nav(string icon, int pageIndex, bool enabled) => new PagerItem
        {
            Label = icon,
            PageIndex = pageIndex,
            CssClass = enabled ? "page-item" : "page-item disabled",
            Enabled = enabled
        };

        public static PagerItem Page(int pageIndex, bool active) => new PagerItem
        {
            Label = (pageIndex + 1).ToString(),
            PageIndex = pageIndex,
            CssClass = active ? "page-item active" : "page-item",
            Enabled = !active
        };
    }
}
