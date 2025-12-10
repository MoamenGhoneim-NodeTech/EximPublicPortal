using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.ContactUs
{
    public partial class ContactRequest : UserControl
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
            }
        }

        private void initFormData()
        {
            ddlCountryCode.Items.Clear();

            Guid siteId = SPContext.Current.Site.ID;
            var isArabic = SPContext.Current.Web.Language == 1025;
            using (SPSite site = new SPSite(siteId))
            {
                using (SPWeb web = site.OpenWeb(GetLocalResourceObject("contactArSiteURL").ToString()))
                {
                    SPList contactRequestList = web.Lists.TryGetList("ContactRequests");
                    SPList contactRequestTypeList = web.Lists.TryGetList("ContactRequestType");

                    if (contactRequestList == null || contactRequestTypeList == null) return;

                    SPField field = contactRequestList.Fields["CountryCode"];

                    var choiceField = field as SPFieldChoice;
                    if (choiceField == null)
                        return;

                    foreach (string choice in choiceField.Choices)
                    {
                        ddlCountryCode.Items.Add(new ListItem(choice, choice));
                    }

                    // Default value (if set in the column settings)
                    if (!string.IsNullOrEmpty(choiceField.DefaultValue))
                    {
                        var item = ddlCountryCode.Items.FindByValue(choiceField.DefaultValue)
                                   ?? ddlCountryCode.Items.FindByText(choiceField.DefaultValue);
                        if (item != null) ddlCountryCode.SelectedValue = item.Value;
                    }

                    ddlRequestType.DataSource = contactRequestTypeList.Items.GetDataTable();
                    ddlRequestType.DataTextField = isArabic ? "Title" : "TitleEn";
                    ddlRequestType.DataValueField = "ID";
                    ddlRequestType.DataBind();
                    ddlRequestType.Items.Insert(0, new ListItem(GetLocalResourceObject("Select").ToString(), "-1"));
                    ddlRequestType.SelectedIndex = 0;
                }
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
                    {
                        using (SPWeb contactUsSubSite = site.OpenWeb(GetLocalResourceObject("contactArSiteURL").ToString()))
                        {
                            SPList contactUsList = contactUsSubSite.GetList(GetLocalResourceObject("contactListRelativeURL").ToString());
                            if (contactUsList == null) return;

                            bool resetUnsafe = !contactUsSubSite.AllowUnsafeUpdates;
                            if (resetUnsafe) contactUsSubSite.AllowUnsafeUpdates = true;

                            try
                            {
                                SPListItem newItem = contactUsList.AddItem();

                                newItem["Title"] = txtMessageTitle.Text;
                                newItem["Name"] = txtSenderName.Text;
                                newItem["Company"] = txtEntityName.Text;
                                newItem["Country"] = txtCountry.Text;
                                newItem["MobileNumber"] = string.Format("{0}{1}", ddlCountryCode.SelectedValue, txtMobileNumber.Text);
                                newItem["Email"] = txtEmail.Text;
                                newItem["Message"] = txtMessage.Text;
                                newItem["RequestType"] = ddlRequestType.SelectedValue;

                                newItem.Update();
                            }
                            finally
                            {
                                if (resetUnsafe) contactUsSubSite.AllowUnsafeUpdates = false;
                            }
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
    }
}
