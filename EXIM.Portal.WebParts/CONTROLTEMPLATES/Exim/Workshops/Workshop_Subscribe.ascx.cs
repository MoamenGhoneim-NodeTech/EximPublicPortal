using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EXIM.Portal.WebParts
{
    public partial class Workshop_Subscribe : UserControl
    {
        #region Design controls
        protected global::Exim.Portal.WebParts.LabelMessage ucMessage;
        #endregion

        private const string visibilityFieldInternalName = "Exim_WorkshopEnableSubscription";
        private const string eventDateFieldInternalName = "ArticleStartDate";

        /// <summary>Countries JSON array injected into the page for the code picker.</summary>
        public string CountriesJson { get; private set; } = "[]";

        public bool IsArabic
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture.Name
                       .StartsWith("ar", StringComparison.OrdinalIgnoreCase);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack)
                    return;

                CheckVisibility();
                initFormData();
                LoadCountriesAutocomplete();
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
                ucMessage.ShowUnexpectedError();
                pnlFormBody.Visible = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid) return;

                Guid siteId = SPContext.Current.Site.ID;
                var ctx = SPContext.Current;
                var currentItem = ctx?.ListItem ?? ctx?.File?.Item;

                if (currentItem == null)
                {
                    ucMessage.ShowUnexpectedError();
                    return;
                }

                // Read selected country code from the hidden field
                string selectedCountryCode = hfSelectedCountryCode.Value;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb eventSubSite = site.OpenWeb(GetLocalResourceObject("eventsArSiteURL").ToString()))
                    {
                        SPList subsList = eventSubSite.GetList(
                            GetLocalResourceObject("eventsSubscriptionListRelativeURL").ToString());
                        if (subsList == null) return;

                        bool resetUnsafe = !eventSubSite.AllowUnsafeUpdates;
                        if (resetUnsafe) eventSubSite.AllowUnsafeUpdates = true;
                        try
                        {
                            SPListItem newItem = subsList.AddItem();

                            newItem["Title"] = $"{currentItem.Title} - {txtEmail.Text}";
                            newItem["Workshop"] = new SPFieldLookupValue(currentItem.ID, currentItem.Title);
                            newItem["CompanyName"] = txtCompanyName.Text;
                            newItem["ComNumber"] = txtCommNumber.Text;
                            newItem["Representative"] = txtResponsiblePersonName.Text;
                            newItem["ContryCode"] = selectedCountryCode;
                            newItem["MobileNumber"] = $"{selectedCountryCode}{txtMobileNumber.Text}";
                            newItem["Email"] = txtEmail.Text;
                            newItem.Update();
                        }
                        finally
                        {
                            if (resetUnsafe) eventSubSite.AllowUnsafeUpdates = false;
                        }
                    }
                });

                ucMessage.ShowSuccess(GetLocalResourceObject("SubscribedSuccessfully").ToString());
                pnlFormBody.Visible = false;
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
                ucMessage.ShowUnexpectedError();
            }
        }

        private void CheckVisibility()
        {
            var ctx = SPContext.Current;
            if (ctx == null || ctx.ListItem == null) return;

            SPListItem item = ctx?.ListItem ?? ctx?.File?.Item;
            if (item == null) return;

            if (ctx.FormContext.FormMode == Microsoft.SharePoint.WebControls.SPControlMode.Edit) return;

            bool enableSubscription = false;
            if (item.Fields.ContainsFieldWithStaticName(visibilityFieldInternalName))
            {
                object val = item[visibilityFieldInternalName];
                enableSubscription = val != null && Convert.ToBoolean(val);
            }

            if (!enableSubscription) { HideControl(); return; }

            if (item.Fields.ContainsFieldWithStaticName(eventDateFieldInternalName))
            {
                object rawDate = item[eventDateFieldInternalName];
                if (rawDate != null)
                {
                    DateTime startDate = (DateTime)rawDate;
                    if (startDate.Date < DateTime.Today) { HideControl(); return; }
                }
            }
        }

        private void HideControl()
        {
            this.Visible = false;
            if (this.Parent != null)
                try { this.Parent.Controls.Remove(this); } catch { }
        }

        private void initFormData()
        {
            // ddlCountryCode population removed — picker uses CountriesJson instead
        }

        private void LoadCountriesAutocomplete()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                using (SPWeb web = site.OpenWeb("/"))
                {
                    SPList list = web.Lists.TryGetList("Countries");
                    if (list == null) return;

                    SPQuery q = new SPQuery
                    {
                        Query = "<OrderBy><FieldRef Name='CountryName' /></OrderBy>",
                        ViewFields = "<FieldRef Name='ID'/><FieldRef Name='CountryCode'/>" +
                                     "<FieldRef Name='CountryName'/><FieldRef Name='CountryNameAr'/>",
                        RowLimit = 300
                    };

                    var sb = new System.Text.StringBuilder("[");
                    bool first = true;

                    foreach (SPListItem item in list.GetItems(q))
                    {
                        if (!first) sb.Append(",");
                        first = false;
                        sb.AppendFormat(
                            "{{\"id\":{0},\"code\":\"{1}\",\"nameEn\":\"{2}\",\"nameAr\":\"{3}\"}}",
                            item.ID,
                            Encode(item["CountryCode"]?.ToString()),
                            Encode(item["CountryName"]?.ToString()),
                            Encode(item["CountryNameAr"]?.ToString()));
                    }
                    sb.Append("]");
                    CountriesJson = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("LoadCountriesAutocomplete: " + ex.Message);
                CountriesJson = "[]";
            }
        }

        private static string Encode(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\r", "")
                    .Replace("\n", "");
        }
    }
}
