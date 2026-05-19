using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using EXIM.Common.Lib.SPHelpers;

namespace EXIM.Portal.WebParts.PartnersWebPart
{
    public partial class PartnersWebPartUserControl : UserControl
    {
        // ── Configuration ────────────────────────────────────────────────

        public string NoResultsMessage { get; set; } = "No results found.";
        public int CurrentLanguage { get; set; } = System.Globalization.CultureInfo.CurrentUICulture.LCID;

        // ── Helpers ──────────────────────────────────────────────────────

        protected bool IsArabic => CurrentLanguage == 1025;

        // Section heading text — mirrors renderHeader() in Exim_BoardMembers.js
        private string AboutText => IsArabic ? "عن البنك" : "About the bank";
        private string PartnersText => IsArabic ? "شركاؤنا" : "Our Partners";

        // ── Lifecycle ────────────────────────────────────────────────────

        protected void Page_Load(object sender, EventArgs e) { }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var categories = FetchCategoriesWithPartners();
            Render(categories);
        }

        // ── Data retrieval ───────────────────────────────────────────────

        private IList<PartnerCategory> FetchCategoriesWithPartners()
        {
            var results = new List<PartnerCategory>();
            try
            {
                var web = SPContext.Current.Site.RootWeb;
                var site = SPContext.Current.Site;

                var categoryList = LandingPageHelper.TryGetList(web, "PartnersCategory")
                                ?? LandingPageHelper.TryGetList(web, "فئات الشركاء");

                if (categoryList == null) return results;

                SPView aboutView = categoryList.Views["About"];
                var categoryQuery = new SPQuery(aboutView)
                {
                    QueryThrottleMode = SPQueryThrottleOption.Override
                };

                foreach (SPListItem catItem in categoryList.GetItems(categoryQuery))
                {
                    var category = new PartnerCategory
                    {
                        Id = catItem.ID,
                        Title = catItem["Title"]?.ToString() ?? "",
                        Partners = new List<IDictionary<string, string>>()
                    };

                    var partnersList = LandingPageHelper.TryGetList(web, "Partners")
                                    ?? LandingPageHelper.TryGetList(web, "الشركاء");

                    if (partnersList != null)
                    {
                        var partnerQuery = new SPQuery
                        {
                            Query = $@"
                            <Where>
                                <And>
                                    <Eq>
                                        <FieldRef Name='Exim_ShowInAboutPage'/>
                                        <Value Type='Boolean'>1</Value>
                                    </Eq>
                                    <Eq>
                                        <FieldRef Name='Parent' LookupId='TRUE'/>
                                        <Value Type='Lookup'>{catItem.ID}</Value>
                                    </Eq>
                                </And>
                            </Where>",
                            QueryThrottleMode = SPQueryThrottleOption.Override
                        };

                        foreach (SPListItem partner in partnersList.GetItems(partnerQuery))
                        {
                            category.Partners.Add(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                            {
                                ["Title"] = partner["Title"]?.ToString() ?? "",
                                ["Url"] = BuildAbsoluteUrl(site, partner["FileRef"]?.ToString()),
                                ["LogoUrl"] = ReadImageUrl(partner, "Exim_PartnerLogo"),
                                ["Category"] = catItem["Title"]?.ToString() ?? ""
                            });
                        }
                    }

                    if (category.Partners.Count > 0)
                        results.Add(category);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("[PartnersAboutControl] {0}", ex);
            }
            return results;
        }

        // ── Render ───────────────────────────────────────────────────────
        //
        // Produces one <section class="team-section"> block per category,
        // matching the HTML skeleton from Exim_BoardMembers.js:
        //   renderHeader()  →  <section> … <div class="team-slider__items owl-carousel owl-theme">
        //   DrawItem()      →  <div class="item"> … </div>
        //   renderFooter()  →  closing tags
        //
        // The ASCX script block calls owlCarousel on every .partners-slider__items
        // so each category gets its own independent carousel.

        private void Render(IList<PartnerCategory> categories)
        {
            phResults.Controls.Clear();

            if (categories.Count == 0)
            {
                phNoResults.Visible = true;
                litNoResults.Text = Enc(NoResultsMessage);
                return;
            }

            phNoResults.Visible = false;

            foreach (var category in categories)
            {
                var html = new StringBuilder();

                // ── Section header (mirrors renderHeader) ────────────────
                html.Append("<section class=\"team-section partners-section\">");
                html.Append("<div class=\"container\">");
                html.Append("<div class=\"section-header\">");
                html.Append("<div class=\"text-center\">");
                html.AppendFormat("<h2 data-aos=\"fade-up\" data-aos-delay=\"100\">{0}</h2>", Enc(AboutText));
                html.AppendFormat("<p data-aos=\"fade-up\" data-aos-delay=\"150\">{0}</p>", Enc(category.Title));
                html.Append("</div>");  // .text-center
                html.Append("</div>");  // .section-header
                html.Append("</div>");  // .container

                // Carousel wrapper
                html.Append("<div class=\"container-fluid\" data-aos=\"fade-up\" data-aos-delay=\"100\">");
                html.Append("<div class=\"partners-slider\">");
                html.Append("<div class=\"partners-slider__items owl-carousel owl-theme\">");

                // ── Items (mirrors DrawItem) ─────────────────────────────
                foreach (var partner in category.Partners)
                {
                    string logoUrl = Val(partner, "LogoUrl");
                    string title = Val(partner, "Title");
                    string url = SafeUrl(Val(partner, "Url"));

                    html.Append("<div class=\"item\">");

                    // Logo image — mirrors <div class="img"><img .../></div>
                    html.Append("<div class=\"img\">");
                    if (!string.IsNullOrWhiteSpace(logoUrl))
                        html.AppendFormat("<img src=\"{0}\" alt=\"{1}\" />", Enc(logoUrl), Enc(title));
                    html.Append("</div>");

                    // Text block — title + link (extends BoardMembers team-text pattern)
                    html.Append("<div class=\"partner-text\">");
                    html.AppendFormat("<h3>{0}</h3>", Enc(title));
                    if (!string.IsNullOrWhiteSpace(url) && url != "#")
                        html.AppendFormat("<a href=\"{0}\" title=\"{1}\">{1}</a>", Enc(url), Enc(title));
                    html.Append("</div>");  // .partner-text

                    html.Append("</div>");  // .item
                }

                // ── Footer (mirrors renderFooter) ────────────────────────
                html.Append("</div>");  // .partners-slider__items  (owl-carousel)
                html.Append("</div>");  // .partners-slider
                html.Append("</div>");  // .container-fluid
                html.Append("</section>");

                phResults.Controls.Add(new LiteralControl(html.ToString()));
            }
        }

        // ── Small helpers ────────────────────────────────────────────────

        private static string ReadImageUrl(SPListItem item, string field)
        {
            if (!item.Fields.ContainsField(field) || item[field] == null) return "";
            var raw = item[field].ToString();
            if (raw.StartsWith("<img", StringComparison.OrdinalIgnoreCase))
            {
                var srcStart = raw.IndexOf("src=\"", StringComparison.OrdinalIgnoreCase);
                if (srcStart < 0) srcStart = raw.IndexOf("src='", StringComparison.OrdinalIgnoreCase);
                if (srcStart >= 0)
                {
                    srcStart += 5;
                    var srcEnd = raw.IndexOfAny(new[] { '"', '\'' }, srcStart);
                    if (srcEnd > srcStart) return raw.Substring(srcStart, srcEnd - srcStart);
                }
                return "";
            }
            return raw;
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

    // ── Category model ───────────────────────────────────────────────────

    public class PartnerCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<IDictionary<string, string>> Partners { get; set; }
    }
}
