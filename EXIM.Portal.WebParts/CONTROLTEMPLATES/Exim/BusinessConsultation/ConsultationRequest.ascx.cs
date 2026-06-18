using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EXIM.Portal.WebParts.BusinessConsultation
{
    public partial class ConsultationRequest : UserControl
    {
        #region Design controls
        protected global::Exim.Portal.WebParts.LabelMessage ucMessage;
        #endregion

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

        private void initFormData()
        {
            Guid siteId = SPContext.Current.Site.ID;
            var isArabic = SPContext.Current.Web.Language == 1025;

            using (SPSite site = new SPSite(siteId))
            using (SPWeb web = site.OpenWeb(GetLocalResourceObject("consultationArSiteURL").ToString()))
            {
                // ddlCountryCode population removed — picker uses CountriesJson instead
                // Only bind other dropdowns here if needed in future
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
                    using (SPWeb eventSubSite = site.OpenWeb(GetLocalResourceObject("consultationArSiteURL").ToString()))
                    {
                        SPList subsList = eventSubSite.GetList(GetLocalResourceObject("consultationListRelativeURL").ToString());
                        if (subsList == null) return;

                        bool resetUnsafe = !eventSubSite.AllowUnsafeUpdates;
                        if (resetUnsafe) eventSubSite.AllowUnsafeUpdates = true;
                        try
                        {
                            // Read selected country code from the hidden field
                            string selectedCountryCode = hfSelectedCountryCode.Value;

                            SPListItem newItem = subsList.AddItem();
                            newItem["Title"] = txtCompanyName.Text;
                            newItem["ComRegNumber"] = txtCommNumber.Text;
                            newItem["ResponsibleName"] = txtResponsiblePersonName.Text;
                            newItem["MobileNumber"] = $"{selectedCountryCode}{txtMobileNumber.Text}";
                            newItem["Email"] = txtEmail.Text;
                            newItem["ProductDescription"] = txtProductDescription.Text;
                            newItem["ExportVolumValue"] = txtExportVolumeValue.Text;
                            newItem["TargetCountries"] = txtTargetCountries.Text;

                            bool isCurrentCustomer = false;
                            if (!string.IsNullOrEmpty(rblCurrentCustomer.SelectedValue))
                                isCurrentCustomer = Convert.ToBoolean(rblCurrentCustomer.SelectedValue);
                            newItem["CurrentEximCustomer"] = isCurrentCustomer;
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
