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
            using (SPSite site = new SPSite(siteId))
            {
                using (SPWeb web = site.OpenWeb(GetLocalResourceObject("consultationArSiteURL").ToString()))
                {
                    SPList list = web.Lists.TryGetList("ConsultationRequest");
                    if (list == null) return;

                    // Use internal name when possible
                    SPField field = list.Fields.TryGetFieldByStaticName("ContryCode")
                                    ?? list.Fields["CountryCode"];

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
                        using (SPWeb eventSubSite = site.OpenWeb(GetLocalResourceObject("consultationArSiteURL").ToString()))
                        {
                            SPList subsList = eventSubSite.GetList(GetLocalResourceObject("consultationListRelativeURL").ToString());
                            if (subsList == null) return;

                            bool resetUnsafe = !eventSubSite.AllowUnsafeUpdates;
                            if (resetUnsafe) eventSubSite.AllowUnsafeUpdates = true;
                            try
                            {
                                SPListItem newItem = subsList.AddItem();

                                newItem["Title"] = txtCompanyName.Text;
                                newItem["ComRegNumber"] = txtCommNumber.Text;
                                newItem["ResponsibleName"] = txtResponsiblePersonName.Text;
                                newItem["MobileNumber"] = $"{ddlCountryCode.SelectedValue}{txtMobileNumber.Text}";
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
    }
}
