using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.Violations
{
    public partial class ReportAViolation : UserControl
    {
        #region Design controls 
        protected global::Exim.Portal.WebParts.LabelMessage ucMessage;
        #endregion

        #region Event Handlers
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid) return;

                string fileErrorMessage;
                if (!ValidateFileUpload(out fileErrorMessage))
                {
                    ucMessage.ShowError(fileErrorMessage);
                    return;
                }

                Guid siteId = SPContext.Current.Site.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb eventSubSite = site.OpenWeb(GetLocalResourceObject("violationArSiteURL").ToString()))
                        {
                            SPList subsList = eventSubSite.GetList(GetLocalResourceObject("violationListRelativeURL").ToString());
                            if (subsList == null) return;

                            bool resetUnsafe = !eventSubSite.AllowUnsafeUpdates;
                            if (resetUnsafe) eventSubSite.AllowUnsafeUpdates = true;

                            try
                            {
                                string CanIdentifyParties = NumberToValue(rblCanIdentifyParties.SelectedValue);
                                string ViolationOngoing = NumberToValue(rblViolationOngoing.SelectedValue);
                                SPListItem newItem = subsList.AddItem();

                                // Map fields to list columns (adjust internal names as needed)
                                newItem["Title"] = txtViolationDetails.Text;

                                if (ddlViolationType.SelectedItem == null)
                                {
                                    ucMessage.ShowError(GetLocalResourceObject("InvalidInput").ToString());
                                    return;
                                }
                                newItem["ViolationType"] = ddlViolationType.SelectedValue;

                                newItem["CanIdentify"] = rblCanIdentifyParties.SelectedItem != null && !string.IsNullOrEmpty(CanIdentifyParties) ? CanIdentifyParties : string.Empty;
                                newItem["ViolationDetails"] = txtViolationDetails.Text;
                                newItem["Relation"] = txtRelation.Text;
                                newItem["Name"] = txtName.Text;
                                newItem["Job"] = txtJobTitle.Text;
                                newItem["Company"] = txtCompany.Text;
                                if (!dtViolationDate.IsDateEmpty)
                                    newItem["Date"] = dtViolationDate.SelectedDate;
                                else
                                    newItem["Date"] = null;

                                newItem["OnGoing"] = rblViolationOngoing.SelectedItem != null && !string.IsNullOrEmpty(ViolationOngoing) ? ViolationOngoing : string.Empty;

                                if (ddlHowYouKnow.SelectedItem == null)
                                {
                                    ucMessage.ShowError(GetLocalResourceObject("InvalidInput").ToString());
                                    return;
                                }
                                newItem["AwarenessMethod"] = ddlHowYouKnow.SelectedValue;

                                bool isAnonymous = false;
                                if (!string.IsNullOrEmpty(rblAnonymous.SelectedValue))
                                    isAnonymous = Convert.ToBoolean(rblAnonymous.SelectedValue);
                                newItem["StayAnonymous"] = isAnonymous;

                                // Save item first so we can attach files
                                newItem.Update();

                                // Attach supporting document if provided
                                if (fuSupportingDocuments != null && fuSupportingDocuments.HasFile)
                                {
                                    string fileName = System.IO.Path.GetFileName(fuSupportingDocuments.FileName);
                                    byte[] fileBytes = fuSupportingDocuments.FileBytes;
                                    if (fileBytes != null && fileBytes.Length > 0)
                                    {
                                        newItem.Attachments.Add(fileName, fileBytes);
                                        newItem.Update();
                                    }
                                }
                            }
                            finally
                            {
                                if (resetUnsafe) eventSubSite.AllowUnsafeUpdates = false;
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

        #endregion

        #region Private Methods
        private void initFormData()
        {
            Guid siteId = SPContext.Current.Site.ID;

            using (SPSite site = new SPSite(siteId))
            {
                using (SPWeb web = site.OpenWeb(GetLocalResourceObject("violationArSiteURL").ToString()))
                {
                    bindViolationType(web);
                    bindHowYouKnow(web);
                    bindCanIdentifyParties(web);
                    bindViolationOngoing(web);
                    initAnonymousRadio();
                }
            }
        }

        private bool isArabic()
        {
            return SPContext.Current.Web.Language == 1025;
        }

        private void bindViolationType(SPWeb web)
        {
            ddlViolationType.Items.Clear();

            string listName = "ViolationType";
            SPList list = web.Lists.TryGetList(listName);
            if (list == null) return;

            ddlViolationType.DataSource = list.Items.GetDataTable();
            ddlViolationType.DataTextField = isArabic() ? "Title" : "TitleEn";
            ddlViolationType.DataValueField = "ID";
            ddlViolationType.DataBind();

            ddlViolationType.Items.Insert(0, new ListItem(GetLocalResourceObject("Select.Text").ToString(), "-1"));
            ddlViolationType.SelectedIndex = 0;
        }

        private void bindHowYouKnow(SPWeb web)
        {
            ddlHowYouKnow.Items.Clear();

            string listName = "ViolationKnowledgeSource";
            SPList list = web.Lists.TryGetList(listName);
            if (list == null) return;

            ddlHowYouKnow.DataSource = list.Items.GetDataTable();
            ddlHowYouKnow.DataTextField = isArabic() ? "Title" : "TitleEn";
            ddlHowYouKnow.DataValueField = "ID";
            ddlHowYouKnow.DataBind();

            ddlHowYouKnow.Items.Insert(0, new ListItem(GetLocalResourceObject("Select.Text").ToString(), "-1"));
            ddlHowYouKnow.SelectedIndex = 0;
        }

        private void bindCanIdentifyParties(SPWeb web)
        {
            rblCanIdentifyParties.Items.Clear();
            rblCanIdentifyParties.Items.Add(new ListItem(GetLocalResourceObject("Yes").ToString(), "1"));
            rblCanIdentifyParties.Items.Add(new ListItem(GetLocalResourceObject("No").ToString(), "2"));
            rblCanIdentifyParties.Items.Add(new ListItem(GetLocalResourceObject("DontKnow").ToString(), "3"));
        }

        private void bindViolationOngoing(SPWeb web)
        {
            rblViolationOngoing.Items.Clear();
            rblViolationOngoing.Items.Add(new ListItem(GetLocalResourceObject("Yes").ToString(), "1"));
            rblViolationOngoing.Items.Add(new ListItem(GetLocalResourceObject("No").ToString(), "2"));
            rblViolationOngoing.Items.Add(new ListItem(GetLocalResourceObject("DontKnow").ToString(), "3"));
        }

        private void initAnonymousRadio()
        {
            rblAnonymous.Items.Clear();
            rblAnonymous.Items.Add(new ListItem(GetLocalResourceObject("Yes").ToString(), "true"));
            rblAnonymous.Items.Add(new ListItem(GetLocalResourceObject("No").ToString(), "false"));
        }

        private string NumberToValue(string number)
        { 
            switch (number)
            {
                case "1":
                    return "نعم";
                case "2":
                    return "لا";
                case "3":
                    return "لا اعلم";
                default:
                    return string.Empty;
            }
        }

        private bool ValidateFileUpload(out string errorMessage)
        {
            errorMessage = string.Empty;

            // If file is optional and nothing is uploaded, treat as valid
            if (fuSupportingDocuments == null || !fuSupportingDocuments.HasFile)
                return true;

            // 2 MB in bytes
            const int maxFileSizeBytes = 2 * 1024 * 1024;

            if (fuSupportingDocuments.PostedFile.ContentLength > maxFileSizeBytes)
            {
                errorMessage = GetLocalResourceObject("FileTooLarge.ErrorMessage").ToString();
                return false;
            }

            string extension = System.IO.Path.GetExtension(fuSupportingDocuments.FileName);
            if (string.IsNullOrEmpty(extension))
            {
                errorMessage = GetLocalResourceObject("FileInvalidExtension.ErrorMessage").ToString();
                return false;
            }

            extension = extension.TrimStart('.').ToLowerInvariant();

            // Allowed: pdf, doc, docx, jpeg, png (+ jpg if you want)
            string[] allowedExtensions = { "pdf", "doc", "docx", "jpeg", "jpg", "png" };

            bool isAllowed = false;
            foreach (string ext in allowedExtensions)
            {
                if (extension == ext)
                {
                    isAllowed = true;
                    break;
                }
            }

            if (!isAllowed)
            {
                errorMessage = GetLocalResourceObject("FileInvalidExtension.ErrorMessage").ToString();
                return false;
            }

            return true;
        }

        #endregion

        protected void cvViolationDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                args.IsValid = !dtViolationDate.IsDateEmpty;
            }
            catch
            {
                args.IsValid = false;
            }
        }
    }
}
