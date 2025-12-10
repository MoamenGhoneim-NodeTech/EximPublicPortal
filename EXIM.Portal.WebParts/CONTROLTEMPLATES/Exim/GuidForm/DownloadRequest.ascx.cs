using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint;
using System;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
            ddlCountryCode.Items.Clear();

            Guid siteId = SPContext.Current.Site.ID;

            using (SPSite site = new SPSite(siteId))
            {
                using (SPWeb web = site.OpenWeb(GetLocalResourceObject("guideArSiteURL").ToString()))
                {
                    SPList list = web.GetList(GetLocalResourceObject("guideListRelativeURL").ToString());
                    if (list == null) return;

                    // Try to get the country-code choice field (handle common internal names)
                    SPField field = list.Fields.TryGetFieldByStaticName("CountryCode");

                    if (field == null) return;

                    SPFieldChoice choiceField = field as SPFieldChoice;
                    if (choiceField == null) return;

                    foreach (string choice in choiceField.Choices)
                    {
                        ddlCountryCode.Items.Add(new ListItem(choice, choice));
                    }

                    // Default value if set in the column
                    if (!string.IsNullOrEmpty(choiceField.DefaultValue))
                    {
                        ListItem defaultItem = ddlCountryCode.Items.FindByValue(choiceField.DefaultValue)
                                                  ?? ddlCountryCode.Items.FindByText(choiceField.DefaultValue);
                        if (defaultItem != null)
                        {
                            ddlCountryCode.ClearSelection();
                            defaultItem.Selected = true;
                        }
                    }
                }
            }
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
                    {
                        return file.ServerRelativeUrl;
                    }
                }
                catch
                {
                    // Ignore and continue searching other libraries
                }
            }

            return null;
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
                    {
                        using (SPWeb web = site.OpenWeb(GetLocalResourceObject("guideArSiteURL").ToString()))
                        {
                            SPList subsList = web.GetList(GetLocalResourceObject("guideListRelativeURL").ToString());
                            if (subsList == null) return;

                            bool resetUnsafe = !web.AllowUnsafeUpdates;
                            if (resetUnsafe) web.AllowUnsafeUpdates = true;

                            try
                            {
                                SPListItem newItem = subsList.AddItem();

                                newItem["Title"] = txtCompanyName.Text;
                                newItem["CompanyName"] = txtCompanyName.Text;
                                newItem["BeneficiaryName"] = txtBeneficiaryName.Text;
                                newItem["City"] = txtCity.Text;
                                newItem["MobileNumber"] = string.Format("{0}{1}", ddlCountryCode.SelectedValue, txtMobileNumber.Text);
                                newItem["Email"] = txtEmail.Text;

                                newItem.Update();

                                string fileUrl = GetGuideFileUrl(web);
                                if (!string.IsNullOrEmpty(fileUrl))
                                {
                                    lnkDownloadGuide.NavigateUrl = fileUrl;
                                    lnkDownloadGuide.Visible = true;
                                }
                                else
                                {
                                    lnkDownloadGuide.Visible = false;
                                }
                            }
                            finally
                            {
                                if (resetUnsafe) web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });

                ucMessage.ShowSuccess(GetLocalResourceObject("MessageSentSuccessfully").ToString());
                pnlFormBody.Visible = false;
                pnlSuccess.Visible = true;
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
                ucMessage.ShowUnexpectedError();
            }
        }
    }
}
