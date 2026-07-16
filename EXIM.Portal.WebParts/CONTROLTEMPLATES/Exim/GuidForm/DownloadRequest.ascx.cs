using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.GuidForm
{
    public partial class DownloadRequest : UserControl
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
                pnlSuccess.Visible = false;
            }
        }

        private void initFormData()
        {
            // ddlCountryCode population removed — picker uses CountriesJson instead
        }

        private string GetGuideFileUrl(SPWeb web)
        {
            string targetFileName = "Export Credit Financing and Insurance.pdf";
            foreach (SPList list in web.Lists)
            {
                SPDocumentLibrary lib = list as SPDocumentLibrary;
                if (lib == null || lib.Hidden) continue;
                try
                {
                    SPFile file = lib.RootFolder.Files[targetFileName];
                    if (file != null && file.Exists)
                        return file.ServerRelativeUrl;
                }
                catch { }
            }
            return null;
        }

        private void DownloadGuideFile(SPWeb spweb)
        {
            lnkbtnDownloadGuide.Enabled = false;
            string targetFileName = "Export Credit Financing and Insurance.pdf";
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(spweb.Site.Url))
                using (SPWeb web = site.AllWebs[spweb.ID])
                {
                    foreach (SPList list in web.Lists)
                    {
                        SPDocumentLibrary lib = list as SPDocumentLibrary;
                        if (lib == null || lib.Hidden) continue;
                        try
                        {
                            SPFile file = lib.RootFolder.Files[targetFileName];
                            if (file == null || !file.Exists) continue;

                            byte[] fileBytes = file.OpenBinary();
                            HttpResponse response = HttpContext.Current.Response;
                            response.Clear();
                            response.ContentType = "application/pdf";
                            response.AddHeader("Content-Disposition", $"attachment; filename=\"{targetFileName}\"");
                            response.AddHeader("Content-Length", fileBytes.Length.ToString());
                            response.BinaryWrite(fileBytes);
                            response.Flush();
                            response.End();
                            return;
                        }
                        catch { }
                    }
                }
            });
            
            ucMessage.ShowError("File not found.");
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
                    using (SPWeb web = site.OpenWeb(GetLocalResourceObject("guideArSiteURL").ToString()))
                    {
                        SPList subsList = web.GetList(GetLocalResourceObject("guideListRelativeURL").ToString());
                        if (subsList == null) return;

                        bool resetUnsafe = !web.AllowUnsafeUpdates;
                        if (resetUnsafe) web.AllowUnsafeUpdates = true;
                        try
                        {
                            // Read selected country code from the hidden field
                            string selectedCountryCode = hfSelectedCountryCode.Value;

                            SPListItem newItem = subsList.AddItem();
                            newItem["Title"] = txtCompanyName.Text;
                            newItem["CompanyName"] = txtCompanyName.Text;
                            newItem["BeneficiaryName"] = txtBeneficiaryName.Text;
                            newItem["City"] = txtCity.Text;
                            newItem["MobileNumber"] = string.Format("{0}{1}", selectedCountryCode, txtMobileNumber.Text);
                            newItem["Email"] = txtEmail.Text;
                            newItem.Update();
                        }
                        finally
                        {
                            if (resetUnsafe) web.AllowUnsafeUpdates = false;
                        }
                    }
                });

                //ucMessage.ShowSuccess(GetLocalResourceObject("MessageSentSuccessfully").ToString());
                pnlFormBody.Visible = false;
                pnlSuccess.Visible = true;
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
                ucMessage.ShowUnexpectedError();
            }
        }

        protected void lnkbtnDownloadGuide_Click(object sender, EventArgs e)
        {
            DownloadGuideFile(SPContext.Current.Web);
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
