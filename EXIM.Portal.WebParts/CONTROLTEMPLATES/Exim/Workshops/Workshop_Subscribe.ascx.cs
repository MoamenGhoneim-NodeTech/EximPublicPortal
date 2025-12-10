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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack)
                    return;

                CheckVisibility();
                initFormData();

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
                int currentPageId = currentItem?.ID ?? 0;
                string currentPageTitle = currentItem?.Title ?? string.Empty;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb eventSubSite = site.OpenWeb(GetLocalResourceObject("eventsArSiteURL").ToString()))
                        {
                            SPList subsList = eventSubSite.GetList(GetLocalResourceObject("eventsSubscriptionListRelativeURL").ToString());
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
                                newItem["ContryCode"] = ddlCountryCode.SelectedValue;
                                newItem["MobileNumber"] = $"{ddlCountryCode.SelectedValue}{txtMobileNumber.Text}";
                                newItem["Email"] = txtEmail.Text;
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

        private void CheckVisibility()
        {
            var ctx = SPContext.Current;
            if (ctx == null || ctx.ListItem == null)
                return;

            SPListItem item = ctx?.ListItem ?? ctx?.File?.Item;
            if (item == null) return;

            if (ctx.FormContext.FormMode == Microsoft.SharePoint.WebControls.SPControlMode.Edit) return;

            bool enableSubscription = false;
            if (item.Fields.ContainsFieldWithStaticName(visibilityFieldInternalName))
            {
                object val = item[visibilityFieldInternalName];
                enableSubscription = val != null && Convert.ToBoolean(val);
            }

            if (!enableSubscription)
            {
                HideControl();
                return;
            }

            if (item.Fields.ContainsFieldWithStaticName(eventDateFieldInternalName))
            {
                object rawDate = item[eventDateFieldInternalName];
                if (rawDate != null)
                {
                    DateTime startDate = (DateTime)rawDate;
                    if (startDate.Date <= DateTime.Today)
                    {
                        HideControl();
                        return;
                    }
                }
            }
        }

        private void HideControl()
        {
            this.Visible = false;
            if (this.Parent != null)
            {
                try { this.Parent.Controls.Remove(this); } catch { }
            }
        }

        private void initFormData()
        {
            ddlCountryCode.Items.Clear();

            Guid siteId = SPContext.Current.Site.ID;
            using (SPSite site = new SPSite(siteId))
            {
                using (SPWeb web = site.OpenWeb(GetLocalResourceObject("eventsArSiteURL").ToString()))
                {
                    SPList list = web.Lists.TryGetList("Subscriptions");
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
    }
}
