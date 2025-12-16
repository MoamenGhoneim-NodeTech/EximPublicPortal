using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.SiteMap
{
    public partial class SiteMapWPUserControl : UserControl
    {
        // Taxonomy configuration
        private const string GROUP_NAME = "Exim";
        private const string ENGLISH_TERMSET = "SiteMap";
        private const string ARABIC_TERMSET = "خريطة الموقع";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // Try to load from taxonomy first
                    LoadSitemapFromTaxonomy();
                }
                catch (Exception ex)
                {
                    // Log error but don't fallback to static
                    litError.Text = "<div class='alert alert-danger'>خطأ في تحميل خريطة الموقع من قاعدة البيانات: " + ex.Message + "<br/>يرجى التحقق من إعدادات التصنيف (Taxonomy).</div>";
                    litError.Visible = true;

                    // Don't show static content - show error message only
                    // LoadStaticSitemap(); // REMOVED - No fallback to static
                }
            }
        }

        private void LoadSitemapFromTaxonomy()
        {
            try
            {
            //    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            //    {
                    SPSite site = SPContext.Current.Site;
                    // Detect site language
                    bool isArabicSite = IsArabicSite(site);

                    // Load appropriate TermSet
                    var mainCategories = GetMainCategories(site, isArabicSite);

                    if (mainCategories != null && mainCategories.Any())
                    {
                        rptMain.DataSource = mainCategories;
                        rptMain.DataBind();
                        litError.Visible = false; // Hide any previous errors
                    }
                    else
                    {
                        // Show specific error message
                        string termSetName = isArabicSite ? ARABIC_TERMSET : ENGLISH_TERMSET;
                        litError.Text = $"<div class='alert alert-warning'>لم يتم العثور على بيانات في التصنيف '{termSetName}'.<br/>يرجى التأكد من تنفيذ سكريبت إنشاء التصنيف أولاً.</div>";
                        litError.Visible = true;
                    }
              //  }
            }
            catch (Exception ex)
            {
                // Throw to be caught by Page_Load
                throw new Exception($"فشل تحميل البيانات من التصنيف: {ex.Message}");
            }
        }

        private bool IsArabicSite(SPSite site)
        {
            try
            {
                // Method 1: Check web language directly
                using (SPWeb web = site.RootWeb)
                {
                    if (web.Language == 1025) // Arabic LCID
                        return true;
                }

                // Method 2: Check current UI culture
                int currentLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
                if (currentLCID == 1025)
                    return true;

                // Method 3: Check URL for language indicator
                string url = site.Url.ToLower();
                if (url.Contains("/ar/") || url.Contains("/ar-") || url.Contains("-ar"))
                    return true;

                // Method 4: Check query string parameter
                string langParam = System.Web.HttpContext.Current.Request.QueryString["lang"];
                if (!string.IsNullOrEmpty(langParam) && langParam.ToLower() == "ar")
                    return true;

                // Default to English
                return false;
            }
            catch
            {
                return false;
            }
        }

        private List<CategoryInfo> GetMainCategories(SPSite site, bool isArabic)
        {
            var categories = new List<CategoryInfo>();

            try
            {
                TaxonomySession taxonomySession = new TaxonomySession(site);

                // Get first available term store
                if (taxonomySession.TermStores.Count == 0)
                {
                    throw new Exception("لا توجد مخازن مصطلحات (Term Stores) متاحة.");
                }

                TermStore termStore = taxonomySession.TermStores[0];

                // Get the group
                Group group = termStore.Groups[GROUP_NAME];
                if (group == null)
                {
                    throw new Exception($"المجموعة '{GROUP_NAME}' غير موجودة في التصنيف.");
                }

                // Get the appropriate TermSet based on language
                string termSetName = isArabic ? ARABIC_TERMSET : ENGLISH_TERMSET;
                TermSet termSet = group.TermSets[termSetName];
                if (termSet == null)
                {
                    throw new Exception($"مجموعة المصطلحات '{termSetName}' غير موجودة في المجموعة '{GROUP_NAME}'.");
                }

                // Get main categories (root level terms only) - sorted by SortOrder
                var mainTerms = new List<Term>();
                foreach (Term term in termSet.Terms)
                {
                    // Only get root-level terms (terms directly under the term set)
                    if (term.Parent == null) // Check if it's a root term
                    {
                        mainTerms.Add(term);
                    }
                }

                // Sort terms by SortOrder custom property
                foreach (Term term in mainTerms.OrderBy(t => GetTermCustomPropertyInt(t, "SortOrder", 1000)))
                {
                    bool isActive = GetTermCustomPropertyBool(term, "IsActive", true);
                    if (!isActive)
                        continue;

                    categories.Add(new CategoryInfo
                    {
                        Id = term.Id,
                        Name = term.Name,
                        SortOrder = GetTermCustomPropertyInt(term, "SortOrder", 1000),
                        HasSubTerms = term.TermsCount > 0
                    });
                }

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في جلب الفئات الرئيسية: {ex.Message}");
            }
        }

        protected void rptMain_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CategoryInfo category = (CategoryInfo)e.Item.DataItem;

                try
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        TaxonomySession taxonomySession = new TaxonomySession(site);
                        TermStore termStore = taxonomySession.TermStores[0];

                        Term term = termStore.GetTerm(category.Id);

                        if (term == null)
                        {
                            litError.Text += $"<div class='alert alert-warning'>الفئة '{category.Name}' غير موجودة في التصنيف.</div>";
                            return;
                        }

                        PlaceHolder phWithSub = (PlaceHolder)e.Item.FindControl("phWithSub");
                        PlaceHolder phWithoutSub = (PlaceHolder)e.Item.FindControl("phWithoutSub");

                        if (category.HasSubTerms)
                        {
                            // Load subcategories
                            phWithSub.Visible = true;
                            phWithoutSub.Visible = false;

                            var subCategories = GetSubCategories(term);
                            Repeater rptSub = (Repeater)e.Item.FindControl("rptSub");

                            if (rptSub != null && subCategories.Any())
                            {
                                rptSub.DataSource = subCategories;
                                rptSub.DataBind();
                            }
                            else
                            {
                                // No subcategories found, show direct links instead
                                phWithSub.Visible = false;
                                phWithoutSub.Visible = true;
                                var directLinks = GetLinksFromTerm(term);
                                Repeater rptDirectLinks = (Repeater)e.Item.FindControl("rptDirectLinks");

                                if (rptDirectLinks != null)
                                {
                                    rptDirectLinks.DataSource = directLinks;
                                    rptDirectLinks.DataBind();
                                }
                            }
                        }
                        else
                        {
                            // Load direct links
                            phWithSub.Visible = false;
                            phWithoutSub.Visible = true;

                            var directLinks = GetLinksFromTerm(term);
                            Repeater rptDirectLinks = (Repeater)e.Item.FindControl("rptDirectLinks");

                            if (rptDirectLinks != null && directLinks.Any())
                            {
                                rptDirectLinks.DataSource = directLinks;
                                rptDirectLinks.DataBind();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    litError.Text += $"<div class='alert alert-warning'>خطأ في تحميل الفئة '{category.Name}': {ex.Message}</div>";
                    litError.Visible = true;

                    // Hide both placeholders
                    PlaceHolder phWithSub = (PlaceHolder)e.Item.FindControl("phWithSub");
                    PlaceHolder phWithoutSub = (PlaceHolder)e.Item.FindControl("phWithoutSub");
                    phWithSub.Visible = false;
                    phWithoutSub.Visible = false;
                }
            }
        }

        protected void rptSub_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CategoryInfo subCategory = (CategoryInfo)e.Item.DataItem;

                try
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        TaxonomySession taxonomySession = new TaxonomySession(site);
                        TermStore termStore = taxonomySession.TermStores[0];

                        Term term = termStore.GetTerm(subCategory.Id);

                        if (term != null)
                        {
                            var links = GetLinksFromTerm(term);

                            Repeater rptLinks = (Repeater)e.Item.FindControl("rptLinks");
                            if (rptLinks != null)
                            {
                                rptLinks.DataSource = links;
                                rptLinks.DataBind();
                            }
                        }
                        else
                        {
                            litError.Text += $"<div class='alert alert-warning'>الفئة الفرعية '{subCategory.Name}' غير موجودة في التصنيف.</div>";
                        }
                    }
                }
                catch (Exception ex)
                {
                    litError.Text += $"<div class='alert alert-warning'>خطأ في تحميل الفئة الفرعية '{subCategory.Name}': {ex.Message}</div>";
                    litError.Visible = true;

                    Repeater rptLinks = (Repeater)e.Item.FindControl("rptLinks");
                    if (rptLinks != null)
                    {
                        rptLinks.Visible = false;
                    }
                }
            }
        }

        private List<CategoryInfo> GetSubCategories(Term parentTerm)
        {
            var subCategories = new List<CategoryInfo>();

            try
            {
                // Get all sub-terms and sort by SortOrder
                var subTerms = new List<Term>();
                foreach (Term subTerm in parentTerm.Terms)
                {
                    subTerms.Add(subTerm);
                }

                foreach (Term subTerm in subTerms.OrderBy(st => GetTermCustomPropertyInt(st, "SortOrder", 1000)))
                {
                    bool isActive = GetTermCustomPropertyBool(subTerm, "IsActive", true);
                    if (!isActive) continue;

                    subCategories.Add(new CategoryInfo
                    {
                        Id = subTerm.Id,
                        Name = subTerm.Name,
                        SortOrder = GetTermCustomPropertyInt(subTerm, "SortOrder", 1000),
                        HasSubTerms = false // Assuming only 2 levels deep
                    });
                }

                return subCategories;
            }
            catch (Exception ex)
            {
                litError.Text += $"<div class='alert alert-warning'>خطأ في جلب الفئات الفرعية: {ex.Message}</div>";
                return subCategories;
            }
        }

        private List<LinkInfo> GetLinksFromTerm(Term term)
        {
            var links = new List<LinkInfo>();

            try
            {
                if (term.CustomProperties.ContainsKey("LinksData"))
                {
                    string jsonData = term.CustomProperties["LinksData"];

                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        var serializer = new JavaScriptSerializer();
                        var rawLinks = serializer.Deserialize<List<RawLinkData>>(jsonData);

                        if (rawLinks != null)
                        {
                            // Sort links by SortOrder
                            foreach (var rawLink in rawLinks.OrderBy(rl => rl.SortOrder))
                            {
                                if (!rawLink.IsActive) continue;

                                links.Add(new LinkInfo
                                {
                                    Title = rawLink.Title,
                                    Url = FormatUrl(rawLink.Url),
                                    SortOrder = rawLink.SortOrder
                                });
                            }
                        }
                    }
                }
                else
                {
                    // If no LinksData, check if term has any custom properties that might contain links
                    litError.Text += $"<div class='alert alert-info'>الفئة '{term.Name}' لا تحتوي على بيانات روابط (LinksData).</div>";
                }
            }
            catch (Exception ex)
            {
                litError.Text += $"<div class='alert alert-warning'>خطأ في تحليل الروابط للفئة '{term.Name}': {ex.Message}</div>";
            }

            return links;
        }

        private string FormatUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "#";

            if (url == "#")
                return url;

            // Ensure proper URL formatting
            if (!url.StartsWith("http") && !url.StartsWith("/") && !url.StartsWith("#"))
            {
                // Assume relative URL
                return "/" + url;
            }

            return url;
        }

        #region Helper Methods

        private bool GetTermCustomPropertyBool(Term term, string key, bool defaultValue)
        {
            try
            {
                if (term.CustomProperties.ContainsKey(key))
                {
                    string value = term.CustomProperties[key];
                    if (bool.TryParse(value, out bool result))
                        return result;
                }
            }
            catch { }

            return defaultValue;
        }

        private int GetTermCustomPropertyInt(Term term, string key, int defaultValue)
        {
            try
            {
                if (term.CustomProperties.ContainsKey(key))
                {
                    string value = term.CustomProperties[key];
                    if (int.TryParse(value, out int result))
                        return result;
                }
            }
            catch { }

            return defaultValue;
        }

        #endregion

        #region Helper Classes

        public class CategoryInfo
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public int SortOrder { get; set; }
            public bool HasSubTerms { get; set; }
        }

        public class LinkInfo
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public int SortOrder { get; set; }
        }

        public class RawLinkData
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public int SortOrder { get; set; }
            public bool IsActive { get; set; }
        }

        #endregion
    }
}
