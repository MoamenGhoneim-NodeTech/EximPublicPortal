using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Drawing;
using Microsoft.SharePoint.Administration;
using System.Linq;
using System.Text;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.Violations
{
    public partial class ReportAViolation : UserControl
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

        #region Event Handlers

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

                            // Read selected country code from hidden field
                            string selectedCountryCode = hfSelectedCountryCode.Value;

                            SPListItem newItem = subsList.AddItem();
                            newItem["Title"] = txtViolationDetails.Text;

                            if (ddlViolationType.SelectedItem == null)
                            {
                                ucMessage.ShowError(GetLocalResourceObject("InvalidInput").ToString());
                                return;
                            }
                            newItem["ViolationType"] = ddlViolationType.SelectedValue;
                            newItem["CanIdentify"] = rblCanIdentifyParties.SelectedItem != null && !string.IsNullOrEmpty(CanIdentifyParties) ? CanIdentifyParties : string.Empty;
                            newItem["ViolationDetails"] = txtViolationDetails.Text;
                            newItem["CountryCode"] = selectedCountryCode;
                            newItem["Name"] = txt_Name.Text;
                            newItem["MobileNumber"] = txtMobileNumber.Text;
                            newItem["Email"] = txtEmail.Text;
                            newItem["OtherViolationType"] = txtOtherType.Text;

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
                            newItem["OtherAwareness"] = txt_Other.Text;

                            bool isAnonymous = false;
                            if (!string.IsNullOrEmpty(rblAnonymous.SelectedValue))
                                isAnonymous = Convert.ToBoolean(rblAnonymous.SelectedValue);
                            newItem["StayAnonymous"] = isAnonymous;

                            newItem.Update();

                            // Attach supporting documents
                            var httpFiles = System.Web.HttpContext.Current.Request.Files;
                            string inputName = fuSupportingDocuments.UniqueID;
                            bool hasNewAttachments = false;

                            for (int fi = 0; fi < httpFiles.Count; fi++)
                            {
                                if (!string.Equals(httpFiles.GetKey(fi), inputName, StringComparison.OrdinalIgnoreCase))
                                    continue;

                                var postedFile = httpFiles[fi];
                                if (postedFile == null || postedFile.ContentLength == 0) continue;

                                string fileName = System.IO.Path.GetFileName(postedFile.FileName);
                                if (string.IsNullOrEmpty(fileName)) continue;

                                byte[] fileBytes = new byte[postedFile.ContentLength];
                                int totalRead = 0;
                                while (totalRead < fileBytes.Length)
                                {
                                    int read = postedFile.InputStream.Read(fileBytes, totalRead, fileBytes.Length - totalRead);
                                    if (read == 0) break;
                                    totalRead += read;
                                }
                                if (totalRead == 0) continue;

                                string attachName = fileName;
                                int suffix = 1;
                                while (newItem.Attachments.Cast<string>().Any(a =>
                                    string.Equals(a, attachName, StringComparison.OrdinalIgnoreCase)))
                                {
                                    attachName = System.IO.Path.GetFileNameWithoutExtension(fileName)
                                                 + "_" + suffix++
                                                 + System.IO.Path.GetExtension(fileName);
                                }

                                newItem.Attachments.Add(attachName, fileBytes);
                                hasNewAttachments = true;
                            }

                            if (hasNewAttachments)
                                newItem.Update();

                            SaveViolationParties(newItem.ID);
                            SendEmail(newItem);
                        }
                        finally
                        {
                            if (resetUnsafe) eventSubSite.AllowUnsafeUpdates = false;
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
            using (SPWeb web = site.OpenWeb(GetLocalResourceObject("violationArSiteURL").ToString()))
            {
                bindViolationType(web);
                bindHowYouKnow(web);
                bindCanIdentifyParties(web);
                bindViolationOngoing(web);
                initAnonymousRadio();
                // BindCountryCode removed — picker uses CountriesJson instead
                BindRelations(web);
            }
        }

        private bool isArabic()
        {
            return SPContext.Current.Web.Language == 1025;
        }

        private void bindViolationType(SPWeb web)
        {
            ddlViolationType.Items.Clear();
            SPList list = web.Lists.TryGetList("ViolationType");
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
            SPList list = web.Lists.TryGetList("ViolationKnowledgeSource");
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
            rblCanIdentifyParties.SelectedIndex = 0;
        }

        private void bindViolationOngoing(SPWeb web)
        {
            rblViolationOngoing.Items.Clear();
            rblViolationOngoing.Items.Add(new ListItem(GetLocalResourceObject("Yes").ToString(), "1"));
            rblViolationOngoing.Items.Add(new ListItem(GetLocalResourceObject("No").ToString(), "2"));
            rblViolationOngoing.Items.Add(new ListItem(GetLocalResourceObject("DontKnow").ToString(), "3"));
        }

        private void BindRelations(SPWeb web)
        {
            ddlRelation.Items.Clear();
            SPList list = web.GetList(GetLocalResourceObject("RelationTypesListRelativeURL").ToString());
            if (list == null) return;

            ddlRelation.DataSource = list.Items.GetDataTable();
            ddlRelation.DataTextField = isArabic() ? "Title" : "TitleEn";
            ddlRelation.DataValueField = "ID";
            ddlRelation.DataBind();
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
                case "1": return "نعم";
                case "2": return "لا";
                case "3": return "لا اعلم";
                default: return string.Empty;
            }
        }

        private bool ValidateFileUpload(out string errorMessage)
        {
            errorMessage = string.Empty;
            var httpFiles = System.Web.HttpContext.Current.Request.Files;
            if (httpFiles == null || httpFiles.Count == 0) return true;

            string inputName = fuSupportingDocuments.UniqueID;
            var postedFiles = new List<System.Web.HttpPostedFile>();
            for (int i = 0; i < httpFiles.Count; i++)
            {
                if (httpFiles.GetKey(i) == inputName && httpFiles[i].ContentLength > 0)
                    postedFiles.Add(httpFiles[i]);
            }
            if (postedFiles.Count == 0) return true;

            const int maxFileSizeBytes = 5 * 1024 * 1024;
            const int maxTotalFiles = 5;
            string[] allowedExtensions = { "pdf", "jpg", "jpeg", "png", "rar" };

            if (postedFiles.Count > maxTotalFiles)
            {
                errorMessage = GetLocalResourceObject("FileTooManyFiles.ErrorMessage") != null
                    ? GetLocalResourceObject("FileTooManyFiles.ErrorMessage").ToString()
                    : string.Format("You may upload at most {0} files.", maxTotalFiles);
                return false;
            }

            foreach (var file in postedFiles)
            {
                if (file.ContentLength > maxFileSizeBytes)
                {
                    errorMessage = GetLocalResourceObject("FileTooLarge.ErrorMessage").ToString();
                    return false;
                }
                string extension = System.IO.Path.GetExtension(file.FileName);
                if (string.IsNullOrEmpty(extension))
                {
                    errorMessage = GetLocalResourceObject("FileInvalidExtension.ErrorMessage").ToString();
                    return false;
                }
                extension = extension.TrimStart('.').ToLowerInvariant();
                bool isAllowed = false;
                foreach (string ext in allowedExtensions)
                    if (extension == ext) { isAllowed = true; break; }

                if (!isAllowed)
                {
                    errorMessage = GetLocalResourceObject("FileInvalidExtension.ErrorMessage").ToString();
                    return false;
                }
            }
            return true;
        }

        #endregion

        protected void cvViolationDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                // IsDateEmpty can return true under ar-SA/UmAlQuraCalendar even when
                // the user picked a valid date. Check SelectedDate directly as fallback.
                if (!dtViolationDate.IsDateEmpty)
                {
                    args.IsValid = true;
                    return;
                }
                // Fallback: try parsing SelectedDate
                args.IsValid = (dtViolationDate.SelectedDate != DateTime.MinValue);
            }
            catch { args.IsValid = false; }
        }

        #region ViolationConcernedParties

        private bool IsArabicSite(SPSite site)
        {
            try
            {
                using (SPWeb web = site.RootWeb)
                {
                    if (web.Language == 1025) return true;
                }
                int currentLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
                if (currentLCID == 1025) return true;
                string url = site.Url.ToLower();
                if (url.Contains("/ar/") || url.Contains("/ar-") || url.Contains("-ar")) return true;
                string langParam = System.Web.HttpContext.Current.Request.QueryString["lang"];
                if (!string.IsNullOrEmpty(langParam) && langParam.ToLower() == "ar") return true;
                return false;
            }
            catch { return false; }
        }

        private List<ViolationParty> ParsePartiesFromHiddenField()
        {
            string json = hfPartiesJson.Value;
            if (string.IsNullOrWhiteSpace(json) || json == "[]")
                return new List<ViolationParty>();
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<List<ViolationParty>>(json);
        }

        private void SaveViolationParties(int violationItemId)
        {
            List<ViolationParty> parties = ParsePartiesFromHiddenField();
            if (parties.Count == 0) return;

            SPWeb web = SPContext.Current.Site.OpenWeb(GetLocalResourceObject("violationArSiteURL").ToString());
            SPList partiesList;
            try
            {
                partiesList = web.GetList(GetLocalResourceObject("ConcernedPartiesListRelativeURL").ToString());
            }
            catch
            {
                throw new Exception($"SharePoint list '{GetLocalResourceObject("ConcernedPartiesListRelativeURL").ToString()}' was not found.");
            }

            SPList violationsList = web.GetList(GetLocalResourceObject("violationListRelativeURL").ToString());
            SPListItem violationItem = violationsList.GetItemById(violationItemId);
            SPList relationTypesList = web.GetList(GetLocalResourceObject("RelationTypesListRelativeURL").ToString());

            web.AllowUnsafeUpdates = true;
            try
            {
                foreach (ViolationParty party in parties)
                {
                    SPListItem newItem = partiesList.Items.Add();
                    SPListItem RelationTypeItem = relationTypesList.GetItemById(int.Parse(party.Relation));
                    newItem["Title"] = party.Name;
                    newItem["JobTitle"] = party.JobTitle;
                    newItem["Company"] = party.Company;
                    newItem["Violation"] = new SPFieldLookupValue(violationItem.ID, violationItem.Title);
                    newItem["Relation"] = new SPFieldLookupValue(RelationTypeItem.ID, RelationTypeItem.Title);
                    newItem.Update();
                }
                partiesList.Update();
            }
            finally
            {
                web.AllowUnsafeUpdates = false;
            }
        }

        #endregion

        protected void SendEmail(SPListItem item)
        {
            var isArabicLang = SPContext.Current.Web.Language == 1025;
            NotificationHelper notification = new NotificationHelper(SPContext.Current.Site.RootWeb.Url);
            Dictionary<string, string> values = NotificationHelper.BuildTokenDictionary(item);
            values.Add("Parties", hfPartiesHTML.Value);
            values = AddConditionalContentKeys(values, item);
            string toEmail = "NA";
            if (toEmail != null)
            {
                if (toEmail.Contains("#"))
                    toEmail = toEmail.Split('#')[1];
                TemplateLanguage lang = isArabicLang ? TemplateLanguage.Ar : TemplateLanguage.En;
                notification.SendEmail("NewViolationReportEmailTemplate", toEmail, values, lang);
            }
        }

        public Dictionary<string, string> AddConditionalContentKeys(Dictionary<string, string> values, SPListItem item)
        {
            if (values == null) values = new Dictionary<string, string>();
            var updatedValues = new Dictionary<string, string>(values);

            string otherViolationText = Convert.ToString(GetLocalResourceObject("OtherViolationType"));
            string howDoYouKnowOther = Convert.ToString(GetLocalResourceObject("HowDoYouKnow-Other"));

            updatedValues["ShowOtherViolationTypeStyle"] =
                ddlViolationType.SelectedValue == otherViolationText ? "display:block;" : "display:none;";
            updatedValues["ShowPartiesStyle"] =
                !string.IsNullOrEmpty(hfPartiesJson.Value) ? "display:block;" : "display:none;";
            updatedValues["ShowOtherAwarenessStyle"] =
                ddlHowYouKnow.SelectedValue == howDoYouKnowOther ? "display:block;" : "display:none;";
            updatedValues["ShowNonAnonymousStyle"] =
                rblAnonymous.SelectedValue == "false" ? "display:block;" : "display:none;";
            updatedValues["StayAnonymousValue"] =
                rblAnonymous.SelectedValue == "false"
                    ? Convert.ToString(GetLocalResourceObject("No"))
                    : Convert.ToString(GetLocalResourceObject("Yes"));
            updatedValues["CanIdentifyPartiesValue"] = Convert.ToString(rblCanIdentifyParties.SelectedItem.Text);
            updatedValues["IsViolationOngoingValue"] = Convert.ToString(rblViolationOngoing.SelectedItem.Text);

            return updatedValues;
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

    public class ViolationParty
    {
        public string Relation { get; set; }
        public string RelationEn { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
    }
}
