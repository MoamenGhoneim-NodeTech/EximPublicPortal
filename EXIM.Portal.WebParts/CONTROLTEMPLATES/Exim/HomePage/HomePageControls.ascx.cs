using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.HomePage
{
    public partial class HomePageControls : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            System.IO.File.AppendAllText(
          @"C:\Temp\sp-debug.txt",
          DateTime.Now + " - Page_Load hit\n");
            try
            {
                EnsureViewportMeta();

                // ── Wrap everything in elevated privileges so anonymous users
                //    can read SharePoint list data just like authenticated users.
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    // Open a fresh elevated SPSite / SPWeb — never reuse
                    // SPContext objects inside elevated blocks (they still run
                    // under the original (anonymous) token).
                    var siteUrl = SPContext.Current != null
    ? SPContext.Current.Site.Url
    : HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                    using (var site = new SPSite(siteUrl))
                    using (var web = site.OpenWeb())
                    {
                        var isArabic = ResolveIsArabic(web);
                        var lang = isArabic ? "ar" : "en";

                        // ── 1. Fetch all data from SharePoint lists ──────────
                        var svc = new HomeDataService(web, isArabic);
                        var model = svc.BuildViewModel();

                        // ── 2. Populate Literal controls ─────────────────────
                        litBanners.Text = HomeHtmlRenderer.RenderBanners(model.Banners, isArabic);
                        litFinancialSolutions.Text = HomeHtmlRenderer.RenderFinancialSolutions(model.FinancialSolutions, isArabic);
                        litHomeNumbers.Text = HomeHtmlRenderer.RenderHomeNumbers(model.HomeNumbers, isArabic);

                        // Stories: hide the whole panel when fewer than 3 items
                        if (model.Stories != null && model.Stories.Count >= 3)
                        {
                            var (firstStory, otherStories) = HomeHtmlRenderer.RenderStories(model.Stories, isArabic, lang);
                            litFirstStory.Text = firstStory;
                            litOtherStories.Text = otherStories;
                            pnlStories.Visible = true;
                        }
                        else
                        {
                            pnlStories.Visible = false;
                        }

                        litNews.Text = HomeHtmlRenderer.RenderNews(model.News, isArabic, lang);

                        // Knowledge Center — 5 tabs
                        // Pass isArabic explicitly — never call SPContext inside elevated block
                        PopulateKnowledgeTab(model.KnowledgeResearch, lang, "Research", litKnowledgeSlider1, litKnowledgeNavs1, isArabic);
                        PopulateKnowledgeTab(model.KnowledgeDevelopment, lang, "Development", litKnowledgeSlider2, litKnowledgeNavs2, isArabic);
                        PopulateKnowledgeTab(model.KnowledgeManagement, lang, "Management", litKnowledgeSlider3, litKnowledgeNavs3, isArabic);
                        PopulateKnowledgeTab(model.KnowledgeMarket, lang, "Market", litKnowledgeSlider4, litKnowledgeNavs4, isArabic);
                        PopulateKnowledgeTab(model.KnowledgeProcedures, lang, "Procedures", litKnowledgeSlider5, litKnowledgeNavs5, isArabic);

                        litPartners.Text = HomeHtmlRenderer.RenderPartners(model.Partners, isArabic);
                    }
                });
            }
            catch (Exception ex)
            {
                litBanners.Text = string.Format(
                    "<div class='webpart-error'>{0}</div>",
                    HttpUtility.HtmlEncode("[HomePageControls] " + ex.Message));
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        //  HELPERS
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// isArabic is passed in explicitly so we never touch SPContext.Current
        /// from inside the elevated-privileges delegate.
        /// </summary>
        private void PopulateKnowledgeTab(
            List<KnowledgeItem> items, string lang, string section,
            Literal sliderLiteral, Literal navsLiteral, bool isArabic)
        {
            var listUrl = string.Format("/{0}/NonFinServices/KnowledgeCenter/{1}/Pages", lang, section);
            var (slider, navs) = HomeHtmlRenderer.RenderKnowledgeTab(items, isArabic, listUrl);
            sliderLiteral.Text = slider;
            navsLiteral.Text = navs;
        }

        /// <summary>
        /// Ensures a responsive viewport meta tag exists (mobile SEO, correct scale, touch).
        /// Safe if the master page already defines viewport — duplicate tags are skipped.
        /// </summary>
        private void EnsureViewportMeta()
        {
            if (Page?.Header == null) return;
            foreach (Control c in Page.Header.Controls)
            {
                if (c is HtmlMeta hm &&
                    string.Equals(hm.Name, "viewport", StringComparison.OrdinalIgnoreCase))
                    return;
            }
            var meta = new HtmlMeta { Name = "viewport", Content = "width=device-width, initial-scale=1" };
            Page.Header.Controls.Add(meta);
        }

        /// <summary>
        /// Mirrors JS currLang: 1033 = EN, 1025 = AR (set by SharePoint MUI pipeline).
        /// Works for both authenticated and anonymous users; falls back to web.Language.
        /// </summary>
        private static bool ResolveIsArabic(SPWeb web)
        {
            try
            {
                var lcid = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
                if (lcid == 1025) return true;
                if (lcid == 1033) return false;
            }
            catch { /* ignore — fall through to web.Language */ }

            return web.Language == 1025;
        }
    
    
    }

    // ═════════════════════════════════════════════════════════════════════════
    //  MODELS
    // ═════════════════════════════════════════════════════════════════════════

    public class BannerItem
    {
        public int ID { get; set; }
        public int EXIM_ItemOrder { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string PublishingRollupImage { get; set; }
        public string Url { get; set; }
        public string UrlAr { get; set; }
        public bool OpenInNewTab { get; set; }
        public string Exim_SliderVideoURL { get; set; }
    }

    public class FinancialSolutionItem
    {
        public int ID { get; set; }
        public int EXIM_ItemOrder { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public string PublishingRollupImage { get; set; }
        public string Url { get; set; }
        public string UrlAr { get; set; }
        public bool OpenInNewTab { get; set; }
    }

    public class HomeNumberItem
    {
        public int ID { get; set; }
        public int EXIM_ItemOrder { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string SubTitle { get; set; }
        public string SubTitleEn { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
    }

    public class StoryItem
    {
        public string Title { get; set; }
        public string PublishingRollupImage { get; set; }
        public string LinkFilename { get; set; }
        public string Comments { get; set; }
    }

    public class NewsItem
    {
        public string Title { get; set; }
        public string PublishingRollupImage { get; set; }
        public string LinkFilename { get; set; }
        public string ArticleStartDate { get; set; }
        public string Comments { get; set; }
    }

    public class KnowledgeItem
    {
        public string Title { get; set; }
        public string Comments { get; set; }
        public string PublishingRollupImage { get; set; }
        public string LinkFilename { get; set; }
        public string FileRef { get; set; }
    }

    public class PartnerItem
    {
        public int ID { get; set; }
        public int EXIM_ItemOrder { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string PublishingRollupImage { get; set; }
        public string Url { get; set; }
        public string UrlAr { get; set; }
        public bool OpenInNewTab { get; set; }
    }

    public class HomePageViewModel
    {
        public List<BannerItem> Banners { get; set; } = new List<BannerItem>();
        public List<FinancialSolutionItem> FinancialSolutions { get; set; } = new List<FinancialSolutionItem>();
        public List<HomeNumberItem> HomeNumbers { get; set; } = new List<HomeNumberItem>();
        public List<StoryItem> Stories { get; set; } = new List<StoryItem>();
        public List<NewsItem> News { get; set; } = new List<NewsItem>();
        public List<KnowledgeItem> KnowledgeResearch { get; set; } = new List<KnowledgeItem>();
        public List<KnowledgeItem> KnowledgeDevelopment { get; set; } = new List<KnowledgeItem>();
        public List<KnowledgeItem> KnowledgeManagement { get; set; } = new List<KnowledgeItem>();
        public List<KnowledgeItem> KnowledgeMarket { get; set; } = new List<KnowledgeItem>();
        public List<KnowledgeItem> KnowledgeProcedures { get; set; } = new List<KnowledgeItem>();
        public List<PartnerItem> Partners { get; set; } = new List<PartnerItem>();
        public bool IsArabic { get; set; }
        public string LangStr { get; set; }
    }

    // ═════════════════════════════════════════════════════════════════════════
    //  DATA SERVICE
    // ═════════════════════════════════════════════════════════════════════════

    internal class HomeDataService
    {
        private readonly SPWeb _web;
        private readonly bool _isArabic;
        private readonly string _langStr;

        public HomeDataService(SPWeb web, bool isArabic)
        {
            _web = web;
            _isArabic = isArabic;
            _langStr = isArabic ? "ar" : "en";
        }

        // ── Public entry point ───────────────────────────────────────────────
        public HomePageViewModel BuildViewModel()
        {
            return new HomePageViewModel
            {
                IsArabic = _isArabic,
                LangStr = _langStr,
                Banners = GetBanners(),
                FinancialSolutions = GetFinancialSolutions(),
                HomeNumbers = GetHomeNumbers(),
                Stories = GetStories(),
                News = GetNews(),
                KnowledgeResearch = GetKnowledgeItems("Research"),
                KnowledgeDevelopment = GetKnowledgeItems("Development"),
                KnowledgeManagement = GetKnowledgeItems("Management"),
                KnowledgeMarket = GetKnowledgeItems("Market"),
                KnowledgeProcedures = GetKnowledgeItems("Procedures"),
                Partners = GetPartners(),
            };
        }

        // ── List queries ─────────────────────────────────────────────────────

        public List<BannerItem> GetBanners()
        {
            return QueryList("HomeBanners", "Home")
                .Where(IsVisible)
                .OrderBy(i => GetInt(i, "EXIM_ItemOrder")).ThenBy(i => GetInt(i, "ID"))
                .Select(i => new BannerItem
                {
                    ID = GetInt(i, "ID"),
                    EXIM_ItemOrder = GetInt(i, "EXIM_ItemOrder"),
                    Title = GetStr(i, "Title"),
                    TitleAr = GetStr(i, "TitleAr"),
                    PublishingRollupImage = GetStr(i, "PublishingRollupImage"),
                    Url = GetStr(i, "Url"),
                    UrlAr = GetStr(i, "UrlAr"),
                    OpenInNewTab = GetBool(i, "EXIM_OpenInNewTab"),
                    Exim_SliderVideoURL = GetStr(i, "Exim_SliderVideoURL"),
                }).ToList();
        }

        public List<FinancialSolutionItem> GetFinancialSolutions()
        {
            return QueryList("FinancialSolutions", "Home")
                .Where(IsVisible)
                .OrderBy(i => GetInt(i, "EXIM_ItemOrder")).ThenBy(i => GetInt(i, "ID"))
                .Select(i => new FinancialSolutionItem
                {
                    ID = GetInt(i, "ID"),
                    EXIM_ItemOrder = GetInt(i, "EXIM_ItemOrder"),
                    Title = GetStr(i, "TitleEn"),
                    TitleAr = GetStr(i, "Title"),
                    Description = GetStr(i, "Description"),
                    DescriptionEn = GetStr(i, "DescriptionEn"),
                    PublishingRollupImage = GetStr(i, "PublishingRollupImage"),
                    Url = GetStr(i, "EXIM_URL_En"),
                    UrlAr = GetStr(i, "EXIM_URL"),
                    OpenInNewTab = GetBool(i, "EXIM_OpenInNewTab"),
                }).ToList();
        }

        public List<HomeNumberItem> GetHomeNumbers()
        {
            return QueryList("HomeNumbers", "Home")
                .Where(IsVisible)
                .OrderBy(i => GetInt(i, "EXIM_ItemOrder")).ThenBy(i => GetInt(i, "ID"))
                .Select(i => new HomeNumberItem
                {
                    ID = GetInt(i, "ID"),
                    EXIM_ItemOrder = GetInt(i, "EXIM_ItemOrder"),
                    Title = GetStr(i, "TitleEN"),
                    TitleAr = GetStr(i, "Title"),
                    SubTitle = GetStr(i, "SubTitle"),
                    SubTitleEn = GetStr(i, "SubTitleEn"),
                    Description = GetStr(i, "Description"),
                    DescriptionEn = GetStr(i, "DescriptionEn"),
                }).ToList();
        }

        public List<StoryItem> GetStories()
        {
            return QueryListByUrl(string.Format("/{0}/MediaCenter/SuccessStories/Pages", _langStr), "Home")
                .Select(i => new StoryItem
                {
                    Title = GetStr(i, "Title"),
                    PublishingRollupImage = GetStr(i, "PublishingRollupImage"),
                    LinkFilename = GetStr(i, "FileLeafRef"),
                    Comments = GetStr(i, "Comments"),
                }).ToList();
        }

        public List<NewsItem> GetNews()
        {
            return QueryListByUrl(string.Format("/{0}/MediaCenter/News/Pages", _langStr), "Home")
                .Select(i => new NewsItem
                {
                    Title = GetStr(i, "Title"),
                    PublishingRollupImage = GetStr(i, "PublishingRollupImage"),
                    LinkFilename = GetStr(i, "FileLeafRef"),
                    ArticleStartDate = GetStr(i, "ArticleStartDate"),
                    Comments = GetStr(i, "Comments"),
                }).ToList();
        }

        public List<KnowledgeItem> GetKnowledgeItems(string section)
        {
            return QueryListByUrl(
                string.Format("/{0}/NonFinServices/KnowledgeCenter/{1}/Pages", _langStr, section), "Home")
                .Take(3)
                .Select(i => new KnowledgeItem
                {
                    Title = GetStr(i, "Title"),
                    Comments = GetStr(i, "Comments"),
                    PublishingRollupImage = GetStr(i, "PublishingRollupImage"),
                    LinkFilename = GetStr(i, "FileLeafRef"),
                    FileRef = GetStr(i, "FileRef"),
                }).ToList();
        }

        public List<PartnerItem> GetPartners()
        {
            return QueryList("Partners", "Home")
                .Where(IsVisible)
                .OrderBy(i => GetInt(i, "EXIM_ItemOrder")).ThenBy(i => GetInt(i, "ID"))
                .Select(i => new PartnerItem
                {
                    ID = GetInt(i, "ID"),
                    EXIM_ItemOrder = GetInt(i, "EXIM_ItemOrder"),
                    Title = GetStr(i, "Title"),
                    TitleAr = GetStr(i, "TitleAr"),
                    PublishingRollupImage = GetStr(i, "PublishingRollupImage"),
                    Url = GetStr(i, "Url"),
                    UrlAr = GetStr(i, "UrlAr"),
                    OpenInNewTab = GetBool(i, "EXIM_OpenInNewTab"),
                }).ToList();
        }

        // ── SharePoint OM helpers ────────────────────────────────────────────

        private List<SPListItem> QueryList(string listName, string viewName)
        {
            try
            {
                var list = _web.Lists.TryGetList(listName);
                if (list == null) return new List<SPListItem>();
                return RunViewQuery(list, viewName);
            }
            catch (Exception ex)
            {
                Log("QueryList(" + listName + ")", ex);
                return new List<SPListItem>();
            }
        }

        private List<SPListItem> QueryListByUrl(string serverRelUrl, string viewName)
        {
            // serverRelUrl is like "/ar/MediaCenter/News/Pages"
            // Strategy:
            //   1. Split into subsite URL and list-folder name.
            //   2. Open the subsite (or reuse _web when it IS the subsite).
            //   3. Use SPWeb.GetList() for a direct, efficient lookup that works
            //      under anonymous access. Fall back to list enumeration only if
            //      direct lookup fails (e.g. custom list URLs).
            SPWeb subWeb = null;
            var dispose = false;
            try
            {
                var trimmed = serverRelUrl.TrimEnd('/');
                var lastSlash = trimmed.LastIndexOf('/');
                var listFolder = lastSlash >= 0 ? trimmed.Substring(lastSlash + 1) : trimmed;
                var webRelUrl = lastSlash > 0 ? trimmed.Substring(0, lastSlash) : "/";

                var siteRelUrl = _web.Site.ServerRelativeUrl.TrimEnd('/');
                var webServerRelUrl = siteRelUrl + webRelUrl;

                if (_web.ServerRelativeUrl.TrimEnd('/').Equals(
                        webServerRelUrl.TrimEnd('/'), StringComparison.OrdinalIgnoreCase))
                {
                    subWeb = _web;
                    dispose = false;
                }
                else
                {
                    subWeb = _web.Site.OpenWeb(webRelUrl.TrimStart('/'));
                    dispose = true;
                }

                if (subWeb == null || !subWeb.Exists)
                {
                    Log("QueryListByUrl(" + serverRelUrl + ")",
                        new Exception("Subweb not found: " + webRelUrl));
                    return new List<SPListItem>();
                }

                // ── PRIMARY: direct URL lookup — fast and anonymous-safe ────
                SPList list = null;
                try
                {
                    // Build the full server-relative URL of the list root folder
                    var fullListUrl = subWeb.ServerRelativeUrl.TrimEnd('/') + "/" + listFolder;
                    list = subWeb.GetList(fullListUrl);
                }
                catch
                {
                    // GetList() failed (wrong URL shape, custom path, etc.)
                    // ── FALLBACK: enumerate lists by title / root-folder name ─
                    // Wrapped in its own try so one bad list doesn't abort the page.
                    try
                    {
                        foreach (SPList candidate in subWeb.Lists)
                        {
                            if (string.Equals(candidate.Title, listFolder, StringComparison.OrdinalIgnoreCase) ||
                                candidate.RootFolder.ServerRelativeUrl.TrimEnd('/')
                                    .EndsWith("/" + listFolder, StringComparison.OrdinalIgnoreCase))
                            {
                                list = candidate;
                                break;
                            }
                        }
                    }
                    catch (Exception fallbackEx)
                    {
                        Log("QueryListByUrl fallback enumeration (" + serverRelUrl + ")", fallbackEx);
                    }
                }

                if (list == null)
                {
                    Log("QueryListByUrl(" + serverRelUrl + ")",
                        new Exception("List not found in web: " + subWeb.ServerRelativeUrl));
                    return new List<SPListItem>();
                }

                return RunViewQuery(list, viewName);
            }
            catch (Exception ex)
            {
                Log("QueryListByUrl(" + serverRelUrl + ")", ex);
                return new List<SPListItem>();
            }
            finally
            {
                if (dispose && subWeb != null) subWeb.Dispose();
            }
        }

        private List<SPListItem> RunViewQuery(SPList list, string viewName)
        {
            var query = new SPQuery { RowLimit = 100 };

            // list.Views can throw an access-denied for anonymous users when
            // view-level permissions are restricted. Wrap in try/catch and fall
            // back to a plain all-items query so the page still renders.
            try
            {
                var view = list.Views.Cast<SPView>()
                               .FirstOrDefault(v => v.Title.Equals(viewName, StringComparison.OrdinalIgnoreCase));
                if (view != null)
                {
                    query.Query = view.Query;
                    query.ViewFields = view.ViewFields.SchemaXml;
                    query.RowLimit = view.RowLimit > 0 ? view.RowLimit : 100;
                }
            }
            catch (Exception ex)
            {
                // Anonymous user cannot read views — continue with default (all-items) query.
                Log("RunViewQuery — views inaccessible for " + list.Title, ex);
            }

            var results = new List<SPListItem>();
            try
            {
                foreach (SPListItem item in list.GetItems(query))
                    results.Add(item);
            }
            catch (Exception ex)
            {
                Log("RunViewQuery — GetItems failed for " + list.Title, ex);
            }
            return results;
        }

        // ── Field helpers ────────────────────────────────────────────────────

        private static string GetStr(SPListItem i, string f)
        {
            try { return i[f] != null ? i[f].ToString() : ""; } catch { return ""; }
        }

        private static int GetInt(SPListItem i, string f)
        {
            try { return Convert.ToInt32(i[f] ?? 0); } catch { return 0; }
        }

        private static bool GetBool(SPListItem i, string f)
        {
            try { return Convert.ToBoolean(i[f] ?? false); } catch { return false; }
        }

        // Treat missing/inaccessible EXIM_IsVisible field as visible (safe default).
        private static bool IsVisible(SPListItem i)
        {
            try { var v = i["EXIM_IsVisible"]; return v == null || Convert.ToBoolean(v); }
            catch { return true; }
        }

        private static void Log(string ctx, Exception ex) =>
            System.Diagnostics.Trace.TraceError("[HomeDataService] " + ctx + ": " + ex.Message);
    }

    // ═════════════════════════════════════════════════════════════════════════
    //  HTML RENDERER
    // ═════════════════════════════════════════════════════════════════════════

    internal static class HomeHtmlRenderer
    {
        // Reserve layout space for LCP/CLS
        private const int HeroBannerImgWidth = 1920;
        private const int HeroBannerImgHeight = 800;
        private const int CardImageWidth = 640;
        private const int CardImageHeight = 400;

        // ── RenderBanners ────────────────────────────────────────────────────
        public static string RenderBanners(IList<BannerItem> data, bool ar)
        {
            if (data == null || data.Count == 0) return "";
            var learnMore = ar ? "معرفة المزيد" : "Learn more";
            var videoNoSupport = ar ? "متصفحك لا يدعم تشغيل الفيديو." : "Your browser does not support video playback.";
            var sb = new StringBuilder();

            for (var slideIndex = 0; slideIndex < data.Count; slideIndex++)
            {
                var x = data[slideIndex];
                var title = L(x.Title, x.TitleAr, ar);
                var url = L(x.Url, x.UrlAr, ar);
                var target = x.OpenInNewTab ? " target=\"_blank\" rel=\"noopener\"" : "";
                var img = ImgSrc(x.PublishingRollupImage);
                var video = (x.Exim_SliderVideoURL ?? "").Trim();

                if (video.HasVal())
                {
                    sb.Append("<div class=\"item hero-video-slide\">");
                    sb.Append("<div class=\"video-background\">");
                    sb.AppendFormat(
                        "<video class=\"exim-deferred-hero-video\" preload=\"none\" muted playsinline loop poster=\"{0}\" width=\"{1}\" height=\"{2}\">",
                        A(img), HeroBannerImgWidth, HeroBannerImgHeight);
                    sb.AppendFormat("<source data-exim-src=\"{0}\" type=\"video/mp4\">", A(video));
                    sb.Append(T(videoNoSupport));
                    sb.Append("</video></div>");
                }
                else
                {
                    sb.Append("<div class=\"item\">");
                    var isFirstSlide = slideIndex == 0;
                    var lazyAttr = isFirstSlide ? "" : " loading=\"lazy\"";
                    var fetchAttr = isFirstSlide ? " fetchpriority=\"high\"" : "";
                    sb.AppendFormat(
                        "<img src=\"{0}\" alt=\"{1}\" width=\"{2}\" height=\"{3}\" decoding=\"async\"{4}{5}>",
                        A(img), A(title), HeroBannerImgWidth, HeroBannerImgHeight, lazyAttr, fetchAttr);
                }

                sb.Append("<div class=\"slider-data\">");
                sb.AppendFormat("<h2 data-aos=\"fade-up\" data-aos-duration=\"1000\">{0}</h2>", T(title));
                if (url.HasVal())
                    sb.AppendFormat(
                        "<div class=\"actions-btns\" data-aos=\"fade-up\" data-aos-duration=\"1400\" data-aos-delay=\"400\">" +
                        "<a href=\"{0}\"{1} class=\"btn btn-primary\">{2} <span class=\"ic-arrow-left\"></span></a></div>",
                        A(url), target, T(learnMore));
                sb.Append("</div></div>");
            }
            return sb.ToString();
        }

        // ── RenderFinancialSolutions ─────────────────────────────────────────
        public static string RenderFinancialSolutions(IList<FinancialSolutionItem> data, bool ar)
        {
            if (data == null || data.Count == 0) return "";
            var moreTxt = ar ? "المزيد من التفاصيل" : "More details";
            var sb = new StringBuilder();

            for (int idx = 0; idx < data.Count; idx++)
            {
                var x = data[idx];
                var delay = (idx + 1) * 100;
                var title = L(x.Title, x.TitleAr, ar);
                var desc = ar ? (x.Description.HasVal() ? x.Description : x.DescriptionEn)
                               : (x.DescriptionEn.HasVal() ? x.DescriptionEn : x.Description);
                var url = L(x.Url, x.UrlAr, ar).Or("#");
                var target = x.OpenInNewTab ? " target=\"_blank\" rel=\"noopener\"" : "";
                var img = ImgSrc(x.PublishingRollupImage);

                sb.AppendFormat("<div class=\"col-lg-3 col-6\"><div class=\"card-item\" data-aos=\"fade-up\" data-aos-delay=\"{0}\">", delay);
                if (img.HasVal())
                    sb.AppendFormat(
                        "<img src=\"{0}\" alt=\"{1}\" loading=\"lazy\" decoding=\"async\" width=\"{2}\" height=\"{3}\">",
                        A(img), A(title), CardImageWidth, CardImageHeight);
                sb.AppendFormat("<div class=\"item-text\"><h2>{0}</h2>", T(title));
                if (desc.HasVal()) sb.AppendFormat("<p>{0}</p>", T(desc));
                sb.AppendFormat("<a href=\"{0}\"{1}>{2} <i class=\"ic-left-angle\"></i></a></div>", A(url), target, T(moreTxt));
                sb.AppendFormat("<div class=\"card-mask\"><h2>{0}</h2></div>", T(title));
                sb.Append("</div></div>");
            }
            return sb.ToString();
        }

        // ── RenderHomeNumbers ────────────────────────────────────────────────
        public static string RenderHomeNumbers(IList<HomeNumberItem> data, bool ar)
        {
            if (data == null || data.Count == 0) return "";
            var sb = new StringBuilder();

            for (int idx = 0; idx < data.Count; idx++)
            {
                var x = data[idx];
                var delay = (idx + 1) * 100;
                var title = L(x.Title, x.TitleAr, ar);
                var sub = ar ? (x.SubTitle.HasVal() ? x.SubTitle : x.SubTitleEn)
                               : (x.SubTitleEn.HasVal() ? x.SubTitleEn : x.SubTitle);
                var desc = ar ? (x.Description.HasVal() ? x.Description : x.DescriptionEn)
                               : (x.DescriptionEn.HasVal() ? x.DescriptionEn : x.Description);

                // Extract leading digits for the counter span
                var numMatch = Regex.Match(title ?? "", @"^([\d\.,]+)");
                var numPart = numMatch.Success ? numMatch.Value : "";
                var titleRest = numPart.HasVal() ? title.Substring(numPart.Length) : title;

                sb.AppendFormat("<div class=\"col-md-4\"><div class=\"number-item\" data-aos=\"fade-up\" data-aos-delay=\"{0}\">", delay);
                sb.AppendFormat("<h2><span class=\"counter\">{0}</span>{1}</h2>", T(numPart), T(titleRest));
                if (sub.HasVal()) sb.AppendFormat("<h3>{0}</h3>", T(sub));
                if (desc.HasVal()) sb.AppendFormat("<p>{0}</p>", T(desc));
                sb.Append("</div></div>");
            }
            return sb.ToString();
        }

        // ── RenderStories ────────────────────────────────────────────────────
        public static (string First, string OtherItems) RenderStories(IList<StoryItem> data, bool ar, string lang)
        {
            if (data == null || data.Count < 3) return ("", "");
            var moreText = ar ? "معرفة المزيد" : "Learn more";
            var firstSb = new StringBuilder();
            var otherSb = new StringBuilder();

            for (int i = 0; i < data.Count; i++)
            {
                var x = data[i];
                var url = string.Format("/{0}/MediaCenter/SuccessStories/pages/{1}", lang, Uri.EscapeDataString(x.LinkFilename ?? ""));
                var img = ImgSrc(x.PublishingRollupImage);
                var name = x.Title ?? "";
                var desc = x.Comments ?? "";

                if (i == 0)
                {
                    firstSb.AppendFormat(
                        "<a href=\"{0}\" class=\"img\"><img src=\"{1}\" alt=\"{2}\" loading=\"lazy\" decoding=\"async\" width=\"{3}\" height=\"{4}\"></a>",
                        A(url), A(img), A(name), CardImageWidth, CardImageHeight);
                    firstSb.Append("<div class=\"first-story-item-texts\">");
                    firstSb.AppendFormat("<h3><a href=\"{0}\">{1}</a></h3><p>{2}</p>", A(url), T(name), T(desc));
                    firstSb.AppendFormat("<div class=\"first-story-item-action d-flex\"><a href=\"{0}\">{1}<i class=\"fas fa-angle-left\"></i></a></div>", A(url), T(moreText));
                    firstSb.Append("</div>");
                }
                else
                {
                    otherSb.Append("<div class=\"home-story-item\" data-aos=\"fade-up\" data-aos-delay=\"150\">");
                    otherSb.AppendFormat(
                        "<div class=\"img\"><a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" loading=\"lazy\" decoding=\"async\" width=\"{3}\" height=\"{4}\"></a></div>",
                        A(url), A(img), A(name), CardImageWidth, CardImageHeight);
                    otherSb.Append("<div class=\"home-story-item-txt\">");
                    otherSb.AppendFormat("<h3><a href=\"{0}\">{1}</a></h3><p>{2}</p>", A(url), T(name), T(desc));
                    otherSb.AppendFormat("<div class=\"home-story-item-action\"><a href=\"{0}\">{1}<i class=\"fas fa-angle-left\"></i></a></div>", A(url), T(moreText));
                    otherSb.Append("</div></div>");
                }
            }
            return (firstSb.ToString(), otherSb.ToString());
        }

        // ── RenderNews ───────────────────────────────────────────────────────
        public static string RenderNews(IList<NewsItem> data, bool ar, string lang)
        {
            if (data == null || data.Count == 0) return "";
            var culture = ar ? new CultureInfo("ar-EG") : new CultureInfo("en-GB");
            var readMore = ar ? "عرض المزيد" : "Read more";
            var arrow = ar ? "ic-left-arrow" : "ic-right-arrow";
            var sb = new StringBuilder();

            foreach (var x in data)
            {
                var url = string.Format("/{0}/MediaCenter/News/pages/{1}", lang, Uri.EscapeDataString(x.LinkFilename ?? ""));
                var img = ImgSrc(x.PublishingRollupImage);
                var name = x.Title ?? "";
                var date = FormatDate(x.ArticleStartDate, culture);
                var desc = x.Comments ?? "";

                sb.Append("<div class=\"item\"><div class=\"img\">");
                sb.AppendFormat(
                    "<a href=\"{0}\" class=\"img-hover\"><img src=\"{1}\" alt=\"{2}\" loading=\"lazy\" decoding=\"async\" width=\"{3}\" height=\"{4}\"></a>",
                    A(url), A(img), A(name), CardImageWidth, CardImageHeight);
                sb.Append("</div><div class=\"blog-data\">");
                sb.AppendFormat("<div class=\"blog-date\">{0}</div>", T(date));
                sb.AppendFormat("<h3>{0}</h3><p>{1}</p>", T(name), T(desc));
                sb.AppendFormat("<div class=\"blog-more\"><a href=\"{0}\">{1} <i class=\"{2}\"></i></a></div>", A(url), T(readMore), arrow);
                sb.Append("</div></div>");
            }
            return sb.ToString();
        }

        // ── RenderKnowledgeTab ───────────────────────────────────────────────
        public static (string Slider, string Navs) RenderKnowledgeTab(IList<KnowledgeItem> data, bool ar, string listUrl)
        {
            if (data == null || data.Count == 0) return ("", "");
            var moreTxt = ar ? "المزيد من التفاصيل" : "More details";
            var defaultImg = "/PublishingImages/DefaultImages/KnowledgeDefaultImg.png";
            var sliderSb = new StringBuilder();
            var navsSb = new StringBuilder();

            foreach (var x in data)
            {
                var img = ImgSrc(x.PublishingRollupImage).Or(defaultImg);
                var name = x.Title ?? "";
                var desc = x.Comments ?? "";
                var url = x.FileRef.HasVal()
                    ? x.FileRef
                    : listUrl.TrimEnd('/') + "/" + Uri.EscapeDataString(x.LinkFilename ?? "");

                sliderSb.Append("<div class=\"item\"><div class=\"img\">");
                sliderSb.AppendFormat(
                    "<img src=\"{0}\" alt=\"{1}\" loading=\"lazy\" decoding=\"async\" width=\"{2}\" height=\"{3}\">",
                    A(img), A(name), CardImageWidth, CardImageHeight);
                sliderSb.Append("</div></div>");

                navsSb.AppendFormat(
                    "<div class=\"item\"><div class=\"img\"><img src=\"{0}\" alt=\"{1}\" loading=\"lazy\" decoding=\"async\" width=\"{2}\" height=\"{3}\"></div>",
                    A(img), A(name), CardImageWidth, CardImageHeight);
                navsSb.AppendFormat("<h3>{0}</h3><p>{1}</p>", T(name), T(desc));
                navsSb.AppendFormat("<a href=\"{0}\">{1} <i class=\"ic-left-angle\"></i></a>", A(url), T(moreTxt));
                navsSb.Append("</div>");
            }
            return (sliderSb.ToString(), navsSb.ToString());
        }

        // ── RenderPartners ───────────────────────────────────────────────────
        public static string RenderPartners(IList<PartnerItem> data, bool ar)
        {
            if (data == null || data.Count == 0) return "";
            var sb = new StringBuilder();

            foreach (var x in data)
            {
                var title = L(x.Title, x.TitleAr, ar);
                var url = L(x.Url, x.UrlAr, ar).Or("#");
                var target = x.OpenInNewTab ? " target=\"_blank\" rel=\"noopener\"" : "";
                var img = ImgSrc(x.PublishingRollupImage);

                sb.AppendFormat("<div class=\"item\"><a href=\"{0}\" title=\"{1}\"{2}>", A(url), A(title), target);
                if (img.HasVal())
                    sb.AppendFormat(
                        "<img src=\"{0}\" alt=\"{1}\" loading=\"lazy\" decoding=\"async\" width=\"{2}\" height=\"{3}\">",
                        A(img), A(title), CardImageWidth, CardImageHeight);
                sb.Append("</a></div>");
            }
            return sb.ToString();
        }

        // ── Shared utilities ─────────────────────────────────────────────────

        private static string ImgSrc(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";
            var m = Regex.Match(raw, @"src=""([^""]+)""", RegexOptions.IgnoreCase);
            return m.Success ? m.Groups[1].Value : raw.Trim();
        }

        private static string L(string en, string ar, bool isArabic) =>
            isArabic ? (ar.HasVal() ? ar : en) : en ?? "";

        private static string FormatDate(string raw, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";
            var p = raw.Split('/');
            if (p.Length != 3) return raw;
            if (!int.TryParse(p[0], out int d) ||
                !int.TryParse(p[1], out int mo) ||
                !int.TryParse(p[2], out int y)) return raw;
            try { return new DateTime(y, mo, d).ToString("d MMMM yyyy", culture); }
            catch { return raw; }
        }

        // Encode for HTML attribute values
        private static string A(string s) => HttpUtility.HtmlAttributeEncode(s ?? "");
        // Encode for HTML text nodes
        private static string T(string s) => HttpUtility.HtmlEncode(s ?? "");
    }

    // ═════════════════════════════════════════════════════════════════════════
    //  STRING EXTENSIONS
    // ═════════════════════════════════════════════════════════════════════════

    internal static class StringExtensions
    {
        public static bool HasVal(this string s) => !string.IsNullOrWhiteSpace(s);
        public static string Or(this string s, string fallback) => s.HasVal() ? s : fallback;
    }
}