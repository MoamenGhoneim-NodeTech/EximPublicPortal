using Microsoft.SharePoint;
using System;
using System.Runtime.Remoting.Messaging;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.InvitationForm
{
    public partial class InvitationForm : UserControl
    {
        protected global::Exim.Portal.WebParts.LabelMessage ucMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!Page.IsValid) return;

                Guid siteId = SPContext.Current.Site.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb invitationSubSite = site.OpenWeb(GetLocalResourceObject("InvitationArSiteURL").ToString()))
                        {
                            var ctx = SPContext.Current;
                            var currentItem = ctx?.ListItem ?? ctx?.File?.Item;
                            if (currentItem == null)
                            {
                                ucMessage.ShowUnexpectedError();
                                return;
                            }
                            int currentPageId = currentItem?.ID ?? 0;
                            SPList subsList = invitationSubSite.GetList(GetLocalResourceObject("InvitationArListURL").ToString());
                            if (subsList == null) return;

                            bool resetUnsafe = !invitationSubSite.AllowUnsafeUpdates;
                            if (resetUnsafe) invitationSubSite.AllowUnsafeUpdates = true;

                            try
                            {
                                SPListItem newItem = subsList.AddItem();

                                // Map fields to list columns (adjust internal names as needed)
                                newItem["Title"] = "Test";
                                newItem["EXIM_Invitation"] = new SPFieldLookupValue(currentItem.ID, currentItem.Title);
                                newItem["Name"] = txtName.Text;
                                newItem["Email"] = txtEmail.Text;
                                newItem["MobileNumber"] = txtMobileNumber.Text;
                                newItem["JobTitle"] = txtJobTitle.Text;
                                newItem["CompanyName"] = txtCompanyName.Text;
                                newItem.Update();

                            }
                            finally
                            {
                                if (resetUnsafe) invitationSubSite.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });

                ucMessage.ShowSuccess(GetLocalResourceObject("MessageSentSuccessfully").ToString());
                pnlFormBody.Visible = false;
            }
            catch { }
        }

    }
}
