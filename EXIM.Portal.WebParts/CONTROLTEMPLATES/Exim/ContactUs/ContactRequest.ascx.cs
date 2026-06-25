using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using EXIM.Common.Lib.SPHelpers;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.ContactUs
{
    public partial class ContactRequest : UserControl
    {
        #region Design controls

        protected global::Exim.Portal.WebParts.LabelMessage ucMessage;

        #endregion

        /// <summary>Countries JSON array injected into the page for the autocomplete + code picker.</summary>
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCountriesAutocomplete();
                if (Page.IsPostBack)
                    return;

                initFormData();
              
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
                ucMessage.ShowUnexpectedError();
                pnlFormBody.Visible = false;
            }
        }

        private void initFormData()
        {
            Guid siteId = SPContext.Current.Site.ID;
            var isArabic = SPContext.Current.Web.Language == 1025;

            using (SPSite site = new SPSite(siteId))
            using (SPWeb web = site.OpenWeb(GetLocalResourceObject("contactArSiteURL").ToString()))
            {
                // ddlCountryCode is now hidden — no need to populate it.
                // Only RequestType list is needed here.
                SPList contactRequestTypeList = web.Lists.TryGetList("ContactRequestType");
                if (contactRequestTypeList == null) return;

                ddlRequestType.DataSource = contactRequestTypeList.Items.GetDataTable();
                ddlRequestType.DataTextField = isArabic ? "Title" : "TitleEn";
                ddlRequestType.DataValueField = "ID";
                ddlRequestType.DataBind();
                ddlRequestType.Items.Insert(0, new ListItem(GetLocalResourceObject("Select").ToString(), "-1"));
                ddlRequestType.SelectedIndex = 0;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid) return;

                Guid siteId = SPContext.Current.Site.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb contactUsSubSite = site.OpenWeb(GetLocalResourceObject("contactArSiteURL").ToString()))
                    {
                        SPList contactUsList = contactUsSubSite.GetList(
                            GetLocalResourceObject("contactListRelativeURL").ToString());

                        if (contactUsList == null) return;

                        bool resetUnsafe = !contactUsSubSite.AllowUnsafeUpdates;
                        if (resetUnsafe) contactUsSubSite.AllowUnsafeUpdates = true;

                        try
                        {
                            // Read the selected country code from the hidden field
                            // (hfSelectedCountryCode is populated by the JS picker)
                            string selectedCountryCode = hfSelectedCountryCode.Value;

                            SPListItem newItem = contactUsList.AddItem();

                            newItem["Title"] = txtMessageTitle.Text;
                            newItem["Name"] = txtSenderName.Text;
                            newItem["Company"] = txtEntityName.Text;
                            newItem["Country"] = txtCountry.Value;
                            newItem["MobileNumber"] = string.Format("{0}{1}", selectedCountryCode, txtMobileNumber.Text);
                            newItem["Email"] = txtEmail.Text;
                            newItem["Message"] = txtMessage.Text;
                            newItem["RequestType"] = ddlRequestType.SelectedValue;
                            newItem["IDNumber"] = txtIDNumber.Text;
                            newItem.Update();

                            SendEmail(newItem);
                        }
                        finally
                        {
                            if (resetUnsafe) contactUsSubSite.AllowUnsafeUpdates = false;
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

        /// <summary>
        /// Queries the Countries list and serialises the result into a JSON array
        /// that is rendered inline as a JS variable for the autocomplete and code picker.
        /// Shape: [{ id, code, nameEn, nameAr }, …]
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
                        ViewFields = "<FieldRef Name='ID'/>" +
                                     "<FieldRef Name='CountryCode'/>" +
                                     "<FieldRef Name='CountryName'/>" +
                                     "<FieldRef Name='CountryNameAr'/>",
                        RowLimit = 300
                    };

                    var sb = new System.Text.StringBuilder("[");
                    bool first = true;

                    foreach (SPListItem item in list.GetItems(q))
                    {
                        if (!first) sb.Append(",");
                        first = false;

                        string code = Encode(item["CountryCode"]?.ToString());
                        string nameEn = Encode(item["CountryName"]?.ToString());
                        string nameAr = Encode(item["CountryNameAr"]?.ToString());

                        sb.AppendFormat(
                            "{{\"id\":{0},\"code\":\"{1}\",\"nameEn\":\"{2}\",\"nameAr\":\"{3}\"}}",
                            item.ID, code, nameEn, nameAr);
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

        /// <summary>Escapes a string for safe embedding inside a JS double-quoted string.</summary>
        private static string Encode(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\r", "")
                    .Replace("\n", "");
        }

        protected void SendEmail(SPListItem item)
        {
            try
            {
                var isArabic = SPContext.Current.Web.Language == 1025;

                NotificationHelper notification = new NotificationHelper(SPContext.Current.Site.RootWeb.Url);
                Dictionary<string, string> values = NotificationHelper.BuildTokenDictionary(item);

                values.Add("Messagelable",
                    ddlRequestType.SelectedValue == Convert.ToString(GetLocalResourceObject("RequestforInfoId")) ||
                    ddlRequestType.SelectedValue == Convert.ToString(GetLocalResourceObject("ShareDataTypeId"))
                        ? Convert.ToString(GetLocalResourceObject("PurposeofRequest"))
                        : lblMessage.Text);

                string showIDNumberStyle = ddlRequestType.SelectedValue ==
                    Convert.ToString(GetLocalResourceObject("RequestforInfoId")) ? "" : "display:none;";
                values.Add("ShowIDNumber", showIDNumberStyle);

                string toEmail = item["RequestType:ToEmail"]?.ToString();
                if (toEmail != null)
                {
                    if (toEmail.Contains("#"))
                        toEmail = toEmail.Split('#')[1];

                    TemplateLanguage lang = isArabic ? TemplateLanguage.Ar : TemplateLanguage.En;
                    notification.SendEmail("ContactUsEmailTemplate", toEmail, values, lang);
                }

            }
            catch (Exception ex)
            { }
        }
    }
}
