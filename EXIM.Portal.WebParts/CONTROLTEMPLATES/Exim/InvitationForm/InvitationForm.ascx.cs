using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.InvitationForm
{
    public partial class InvitationForm : UserControl
    {
        #region Design controls
        protected global::Exim.Portal.WebParts.LabelMessage ucMessage;
        #endregion

        // ── Same two properties as ReportAViolation ──────────────────────────────

        /// <summary>Countries JSON array injected into the page for the code picker.</summary>
        public string CountriesJson { get; private set; } = "[]";

        /// <summary>True when the current UI culture is Arabic.</summary>
        public bool IsArabic
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture.Name
                       .StartsWith("ar", StringComparison.OrdinalIgnoreCase);
            }
        }

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Must run on every request (including postbacks) so CountriesJson
                // is always available for the picker — same pattern as ReportAViolation.
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

                // ── Capture all values BEFORE entering elevated scope ─────────────
                // SPContext is not reliably available inside RunWithElevatedPrivileges.
                Guid siteId = SPContext.Current.Site.ID;

                var ctx = SPContext.Current;
                var currentItem = ctx?.ListItem ?? ctx?.File?.Item;
                if (currentItem == null)
                {
                    ucMessage.ShowUnexpectedError();
                    return;
                }

                int currentPageId = currentItem.ID;
                string currentPageTitle = currentItem.Title;

                string nameVal = txtName.Text.Trim();
                string emailVal = txtEmail.Text.Trim();
                string mobileVal = txtMobileNumber.Text.Trim();
                string jobTitleVal = txtJobTitle.Text.Trim();
                string companyNameVal = txtCompanyName.Text.Trim();

                // Read selected country dial code from hidden field (same as ReportAViolation)
                string countryCode = hfSelectedCountryCode.Value;
                string fullMobile = countryCode + mobileVal;

                string invitationSiteUrl = GetLocalResourceObject("InvitationArSiteURL").ToString();
                string invitationListUrl = GetLocalResourceObject("InvitationArListURL").ToString();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb invitationSubSite = site.OpenWeb(invitationSiteUrl))
                    {
                        SPList subsList = invitationSubSite.GetList(invitationListUrl);
                        if (subsList == null) return;

                        bool resetUnsafe = !invitationSubSite.AllowUnsafeUpdates;
                        if (resetUnsafe) invitationSubSite.AllowUnsafeUpdates = true;

                        try
                        {
                            SPListItem newItem = subsList.AddItem();

                            newItem["Title"] = nameVal;
                            newItem["EXIM_Invitation"] = new SPFieldLookupValue(currentPageId, currentPageTitle);
                            newItem["Name"] = nameVal;
                            newItem["Email"] = emailVal;
                            newItem["CountryCode"] = countryCode;
                            newItem["MobileNumber"] = fullMobile;
                            newItem["JobTitle"] = jobTitleVal;
                            newItem["CompanyName"] = companyNameVal;
                            newItem.Update();
                        }
                        finally
                        {
                            if (resetUnsafe) invitationSubSite.AllowUnsafeUpdates = false;
                        }
                    }
                });

                ucMessage.ShowSuccess(GetLocalResourceObject("MessageSentSuccessfully").ToString());
                pnlFormBody.Visible = false;
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
                ucMessage.ShowUnexpectedError();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the SharePoint Countries list into CountriesJson.
        /// Identical implementation to ReportAViolation — reads ID, CountryCode,
        /// CountryName, CountryNameAr and emits a JSON array.
        /// Must be called on every request (including postbacks).
        /// </summary>
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

                    var sb = new StringBuilder("[");
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

        /// <summary>JSON-safe string encoder — same helper as ReportAViolation.</summary>
        private static string Encode(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\r", "")
                    .Replace("\n", "");
        }

        #endregion
    }
}
