using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Linq;
using EXIM.Common.Lib.SPHelpers;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.FinancialServices
{
    public partial class FinancialServiceControl : UserControl
    {
        // ----------------------------------------------------------------
        // Configuration
        // ----------------------------------------------------------------

        private const string WhereClause =
            "<Where><Eq><FieldRef Name='EXIM_ShowInArchive'/><Value Type='Boolean'>1</Value></Eq></Where>";

        public string DownloadGuideUrl { get; set; } = string.Empty;
        public int RowsPerPage { get; set; } = 9;
        public bool ShowPaging { get; set; } = true;
        public string NoResultsMessage { get; set; } = "No results found.";
        public int CurrentLanguage { get; set; }
            = System.Globalization.CultureInfo.CurrentUICulture.LCID;

        // ----------------------------------------------------------------
        // ViewState-backed state
        // ----------------------------------------------------------------

        private string CurrentKeyword
        {
            get => ViewState["VSKeyword"] as string ?? string.Empty;
            set => ViewState["VSKeyword"] = value ?? string.Empty;
        }

        private List<string> SelectedCategories
        {
            get
            {
                var raw = ViewState["VSCats"] as string ?? string.Empty;
                return raw.Length == 0 ? new List<string>()
                    : raw.Split('|').Where(s => s.Length > 0).ToList();
            }
            set => ViewState["VSCats"] = value == null ? "" : string.Join("|", value);
        }

        private List<string> SelectedServiceTypes
        {
            get
            {
                var raw = ViewState["VSSvcs"] as string ?? string.Empty;
                return raw.Length == 0 ? new List<string>()
                    : raw.Split('|').Where(s => s.Length > 0).ToList();
            }
            set => ViewState["VSSvcs"] = value == null ? "" : string.Join("|", value);
        }

        private int CurrentPageIndex
        {
            get => ViewState["VSPage"] as int? ?? 0;
            set => ViewState["VSPage"] = value;
        }

        // ----------------------------------------------------------------
        // ASCX-bound
        // ----------------------------------------------------------------

        protected string PagingNavLabel => IsArabic ? "عدد الصفحات المتاحة" : "Available pages";
        protected string TotalPagesLabel { get; private set; } = string.Empty;
        protected bool IsArabic => CurrentLanguage == 1025;

        // ----------------------------------------------------------------
        // Lifecycle
        //
        // ASP.NET event order:
        //   1. Page_Load
        //   2. Event handlers (btnApply_Click, btnClear_Click,
        //                      Pager_ItemCommand, btnSearch_Click)
        //   3. Page_PreRender  ← render happens here, AFTER all handlers
        // ----------------------------------------------------------------

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
            // Always runs after ALL event handlers, so ViewState is correct.
            var allItems = FetchFromPagesLibrary();
            BuildFilterCheckboxes(allItems);
            var filteredItems = ApplyFilters(allItems);
            BindAndRender(filteredItems);
        }

        // ----------------------------------------------------------------
        // Button handlers — update ViewState only, PreRender re-renders
        // ----------------------------------------------------------------

        protected void btnApply_Click(object sender, EventArgs e)
        {
            CurrentKeyword = txtSearch.Text.Trim();
            SelectedCategories = GetCheckedFormValues("fcat_");
            SelectedServiceTypes = GetCheckedFormValues("fsvc_");
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

        // ----------------------------------------------------------------
        // Pager handler
        //
        // rptPager is declared INSIDE pnlPaging in the ASCX but pnlPaging
        // uses asp:Panel which keeps children in the control tree even when
        // Visible=false — unlike PlaceHolder which removes them.
        // This means Pager_ItemCommand always fires correctly.
        // ----------------------------------------------------------------

        protected void Pager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Page") return;
            if (!int.TryParse(e.CommandArgument.ToString(), out int idx)) return;
            CurrentPageIndex = idx;
            // Page_PreRender re-renders with the new page index
        }

        // ================================================================
        // SECTION 1 – Data retrieval
        // ================================================================

        private IList<IDictionary<string, string>> FetchFromPagesLibrary()
        {
            var results = new List<IDictionary<string, string>>();
            try
            {
                SPWeb web = SPContext.Current.Web;
                SPSite site = SPContext.Current.Site;

                SPList list = ResolveTargetList(web);
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
                        ["Path"] = BuildAbsoluteUrl(site, item["FileRef"]?.ToString() ?? ""),
                        ["Title"] = item["Title"]?.ToString() ?? "",
                        ["Desc"] = item["Comments"]?.ToString() ?? "",
                        ["SvcType"] = GetFieldValue(item, IsArabic ? "Exim_FinSrv_ServiceType" : "Exim_FinSrv_ServiceType_En"),
                        ["Category"] = GetFieldValue(item, IsArabic ? "Exim_FinSrv_ServiceClass" : "Exim_FinSrv_ServiceClass_En")
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("[FinancialServiceControl] {0}", ex);
            }
            return results;
        }

        private static string GetFieldValue(SPListItem item, string field)
        {
            if (!item.Fields.ContainsField(field) || item[field] == null) return "";
            var v = item[field];
            if (v is SPFieldLookupValue lv) return lv.LookupValue;
            if (v is SPFieldLookupValueCollection mc)
                return string.Join(", ", mc.Cast<SPFieldLookupValue>().Select(x => x.LookupValue));
            return v.ToString();
        }

        private SPList ResolveTargetList(SPWeb web) =>
            LandingPageHelper.TryGetList(web, "Pages")
            ?? LandingPageHelper.TryGetList(web, "الصفحات");

        // ================================================================
        // SECTION 2 – Filtering
        // ================================================================

        private IList<IDictionary<string, string>> ApplyFilters(
            IList<IDictionary<string, string>> all)
        {
            var keyword = CurrentKeyword.ToLowerInvariant();
            var cats = SelectedCategories;
            var svcs = SelectedServiceTypes;

            return all.Where(item =>
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    string t = GetProp(item, "Title").ToLowerInvariant();
                    string d = GetProp(item, "Desc").ToLowerInvariant();
                    if (!t.Contains(keyword) && !d.Contains(keyword)) return false;
                }
                if (cats.Count > 0 &&
                    !cats.Contains(GetProp(item, "Category"), StringComparer.OrdinalIgnoreCase))
                    return false;
                if (svcs.Count > 0 &&
                    !svcs.Contains(GetProp(item, "SvcType"), StringComparer.OrdinalIgnoreCase))
                    return false;
                return true;
            }).ToList();
        }

        // ================================================================
        // SECTION 3 – Filter checkboxes
        // ================================================================

        private void BuildFilterCheckboxes(IList<IDictionary<string, string>> all)
        {
            RenderCheckboxGroup(phCategoryFilters,
                all.Select(i => GetProp(i, "Category")), "fcat_", SelectedCategories);
            RenderCheckboxGroup(phServiceTypeFilters,
                all.Select(i => GetProp(i, "SvcType")), "fsvc_", SelectedServiceTypes);
        }

        private static void RenderCheckboxGroup(PlaceHolder ph,
            IEnumerable<string> values, string prefix, List<string> selected)
        {
            ph.Controls.Clear();
            foreach (var v in values
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x))
            {
                string chk = selected.Contains(v, StringComparer.OrdinalIgnoreCase)
                             ? " checked=\"checked\"" : "";
                ph.Controls.Add(new LiteralControl(
                    $"<label class=\"exim-check\">" +
                    $"<input type=\"checkbox\" name=\"{HtmlEnc(prefix + v)}\" " +
                    $"value=\"{HtmlEnc(v)}\"{chk} />" +
                    $"<span>{HtmlEnc(v)}</span></label>"));
            }
        }

        // ================================================================
        // SECTION 4 – Bind + render
        // ================================================================

        private void BindAndRender(IList<IDictionary<string, string>> filtered)
        {
            int total = filtered.Count;
            int totalPages = RowsPerPage > 0
                ? (int)Math.Ceiling((double)total / RowsPerPage) : 0;

            if (CurrentPageIndex >= totalPages && totalPages > 0) CurrentPageIndex = totalPages - 1;
            if (CurrentPageIndex < 0) CurrentPageIndex = 0;

            var page = filtered.Skip(CurrentPageIndex * RowsPerPage).Take(RowsPerPage).ToList();

            RenderItems(page);
            RenderNoResults(total);
            RenderPaging(totalPages);
        }

        // ================================================================
        // SECTION 5 – Items
        // ================================================================

        private void RenderItems(IList<IDictionary<string, string>> items)
        {
            phResults.Controls.Clear();
            if (items == null || items.Count == 0) return;

            foreach (var p in items)
            {
                string url = EnsureAllowedProtocol(GetProp(p, "Path"));
                string title = GetProp(p, "Title");
                string desc = GetProp(p, "Desc");
                string type = GetProp(p, "SvcType");

                var sb = new StringBuilder();
                sb.Append("<div class=\"col-md-4 pagingItem\"><div class=\"services-item\">");
                sb.AppendFormat("<h3><a href=\"{0}\">{1}</a></h3>", HtmlEnc(url), HtmlEnc(title));
                if (!string.IsNullOrWhiteSpace(desc))
                    sb.AppendFormat("<p>{0}</p>", HtmlEnc(desc));
                sb.AppendFormat(
                    "<a href=\"{0}\" class=\"services-item-footer\">" +
                    "<span class=\"service-category color-blue\">{1}</span>" +
                    "<i class=\"ic-left-arrow-dark\"></i></a>",
                    HtmlEnc(url), HtmlEnc(type));
                sb.Append("</div></div>");
                phResults.Controls.Add(new LiteralControl(sb.ToString()));
            }
        }

        // ================================================================
        // SECTION 6 – No-results
        // ================================================================

        private void RenderNoResults(int total)
        {
            phNoResults.Visible = total == 0;
            if (total == 0) litNoResults.Text = HtmlEnc(NoResultsMessage);
        }

        // ================================================================
        // SECTION 7 – Paging
        //
        // rptPager is declared in the ASCX inside asp:Panel (pnlPaging).
        // asp:Panel keeps children in the control tree even when Visible=false,
        // so Pager_ItemCommand fires on every PostBack regardless of whether
        // pnlPaging was visible on the previous render.
        //
        // The datasource is a flat list of PagerItem objects — one per button
        // (Prev, page 1, page 2 ... page N, Next) — so the Repeater renders
        // all paging controls in a single pass with no dynamic LinkButtons.
        // ================================================================

        private void RenderPaging(int totalPages)
        {
            bool show = ShowPaging && totalPages > 1;

            pnlPaging.Visible = show;

            if (!show)
            {
                rptPager.DataSource = null;
                rptPager.DataBind();
                return;
            }

            TotalPagesLabel = IsArabic
                ? $"إجمالي {totalPages} صفحات"
                : $"Total {totalPages} pages";

            var items = new List<PagerItem>();

            // Previous
            items.Add(new PagerItem
            {
                Label = "<i class=\"fas fa-angle-right\"></i>",
                PageIndex = CurrentPageIndex - 1,
                CssClass = CurrentPageIndex > 0 ? "page-item" : "page-item disabled",
                Enabled = CurrentPageIndex > 0
            });

            // Numbered pages
            for (int p = 0; p < totalPages; p++)
            {
                bool active = p == CurrentPageIndex;
                items.Add(new PagerItem
                {
                    Label = (p + 1).ToString(),
                    PageIndex = p,
                    CssClass = active ? "page-item active" : "page-item",
                    Enabled = !active
                });
            }

            // Next
            items.Add(new PagerItem
            {
                Label = "<i class=\"fas fa-angle-left\"></i>",
                PageIndex = CurrentPageIndex + 1,
                CssClass = CurrentPageIndex < totalPages - 1 ? "page-item" : "page-item disabled",
                Enabled = CurrentPageIndex < totalPages - 1
            });

            rptPager.DataSource = items;
            rptPager.DataBind();
        }

        // ================================================================
        // Utilities
        // ================================================================

        private List<string> GetCheckedFormValues(string prefix) =>
            (Request.Form.AllKeys ?? new string[0])
                .Where(k => k != null && k.StartsWith(prefix, StringComparison.Ordinal))
                .Select(k => k.Substring(prefix.Length))
                .ToList();

        private static string GetProp(IDictionary<string, string> d, string k)
            => d != null && d.TryGetValue(k, out var v) ? (v ?? "") : "";

        private static string BuildAbsoluteUrl(SPSite site, string rel)
        {
            if (string.IsNullOrWhiteSpace(rel)) return "#";
            var u = new Uri(site.Url);
            return $"{u.Scheme}://{u.Host}{(u.IsDefaultPort ? "" : ":" + u.Port)}{rel}";
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

    // ── Pager item model ─────────────────────────────────────────────────
    public class PagerItem
    {
        public string Label { get; set; }
        public int PageIndex { get; set; }
        public string CssClass { get; set; }
        public bool Enabled { get; set; }
    }
}
