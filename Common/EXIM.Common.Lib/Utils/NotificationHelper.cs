using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Security;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Administration;



namespace EXIM.Common.Lib.Utils
{
    public class NotificationHelper
    {
        SPWeb web;
        private const string TEMPLATE_LIST = "NotificationEmailTemplates";
        public NotificationHelper(string siteUrl)
        {
            if (string.IsNullOrWhiteSpace(siteUrl))
                throw new ArgumentNullException(nameof(siteUrl));

            web = new SPSite(siteUrl).OpenWeb();
        }
        public EmailTemplate GetTemplate(string templateTitle)
        {
            EmailTemplate CommonTemplate = new EmailTemplate();
            var list = web.Lists.TryGetList(TEMPLATE_LIST);

            var query = new SPQuery();
            query.Query = " <Where> <Eq>  <FieldRef Name='Title'/> <Value Type='Text'>" +
                           templateTitle + "</Value>  </Eq> </Where>";

            var items = list.GetItems(query);
            if (items.Count == 0)
                throw new Exception($"Template '{templateTitle}' was not found in the '{TEMPLATE_LIST}' list.");
            bool bIncludeCommonTemplate = bool.Parse(items[0]["IncludeCommonTemplate"]?.ToString());
            
            string templateDetailsEn = items[0]["TemplateDetailsEn"]?.ToString();
            string templateDetailsAr = items[0]["TemplateDetailsAr"]?.ToString();

            if (bIncludeCommonTemplate)
            {
                CommonTemplate = GetTemplate("CommonEmailTemplate");
                templateDetailsEn = CommonTemplate.TemplateDetailsEn.Replace("{EmailBody}", templateDetailsEn);
                templateDetailsAr = CommonTemplate.TemplateDetailsAr.Replace("{EmailBody}", templateDetailsAr);
            }
            return new EmailTemplate
            {
                Title = items[0]["Title"]?.ToString(),
                TemplateDetailsEn = templateDetailsEn,
                TemplateDetailsAr = templateDetailsAr,
                IncludeCommonTemplate = bIncludeCommonTemplate,
                SubjectAr= items[0]["SubjectAr"]?.ToString(),
                SubjectEn = items[0]["SubjectEn"]?.ToString(),
                MailTo= items[0]["MailTo"]?.ToString()
            };
        }

        public static string ReplacePlaceholders(string template, Dictionary<string, string> values)
        {
            if (string.IsNullOrEmpty(template)) return string.Empty;
            if (values == null || values.Count == 0) return template;

            foreach (var entry in values)
            {
                string placeholder = $"{{{entry.Key}}}";
                template = template.Replace(placeholder, entry.Value ?? string.Empty);
            }

            return template;
        }

        public static string ReplacePlaceholders(string template, SPListItem item)
        {
            if (string.IsNullOrEmpty(template)) return string.Empty;
            if (item == null) return template;

            // Find all {Placeholders} in template
            var placeholders = Regex.Matches(template, @"\{([\w:\.]+)\}");

            foreach (Match match in placeholders)
            {
                string fieldName = match.Groups[1].Value;
                string placeholder = match.Value;
                string fieldValue = string.Empty;

                try
                {
                    if (!item.Fields.ContainsField(fieldName)) continue;

                    SPField field = item.Fields.GetFieldByInternalName(fieldName);
                    object rawValue = item[fieldName];

                    if (rawValue == null)
                    {
                        template = template.Replace(placeholder, string.Empty);
                        continue;
                    }

                    switch (field.Type)
                    {
                        // ── Lookup (Single) ──────────────────────────────────────
                        case SPFieldType.Lookup:
                            if (field is SPFieldLookup lookupField && !lookupField.AllowMultipleValues)
                            {
                                var lookupValue = new SPFieldLookupValue(rawValue.ToString());
                                fieldValue = lookupValue.LookupValue; // Display text only
                            }
                            // ── Lookup (Multi) ───────────────────────────────────
                            else
                            {
                                var multiLookup = new SPFieldLookupValueCollection(rawValue.ToString());
                                fieldValue = string.Join(", ", multiLookup.Select(v => v.LookupValue));
                            }
                            break;

                        // ── User (Single) ────────────────────────────────────────
                        case SPFieldType.User:
                            if (field is SPFieldUser userField && !userField.AllowMultipleValues)
                            {
                                var userValue = new SPFieldUserValue(item.Web, rawValue.ToString());
                                fieldValue = userValue.User?.Name ?? userValue.LookupValue;
                            }
                            // ── User (Multi) ─────────────────────────────────────
                            else
                            {
                                var multiUser = new SPFieldUserValueCollection(item.Web, rawValue.ToString());
                                fieldValue = string.Join(", ", multiUser.Select(u => u.User?.Name ?? u.LookupValue));
                            }
                            break;

                        // ── DateTime ─────────────────────────────────────────────
                        case SPFieldType.DateTime:
                            if (DateTime.TryParse(rawValue.ToString(), out DateTime dateVal))
                                fieldValue = dateVal.ToString("dd/MM/yyyy HH:mm");
                            else
                                fieldValue = rawValue.ToString();
                            break;

                        // ── Boolean ──────────────────────────────────────────────
                        case SPFieldType.Boolean:
                            fieldValue = rawValue.ToString() == "1" ? "Yes" : "No";
                            break;

                        // ── Choice (Single) ──────────────────────────────────────
                        case SPFieldType.Choice:
                            fieldValue = rawValue.ToString();
                            break;

                        // ── Choice (Multi) ───────────────────────────────────────
                        case SPFieldType.MultiChoice:
                            fieldValue = rawValue.ToString()
                                .Replace(";#", ", ")
                                .Trim(',', ' ');
                            break;

                        // ── URL ──────────────────────────────────────────────────
                        case SPFieldType.URL:
                            var urlValue = new SPFieldUrlValue(rawValue.ToString());
                            fieldValue = !string.IsNullOrEmpty(urlValue.Description)
                                ? $"<a href='{urlValue.Url}'>{urlValue.Description}</a>"
                                : urlValue.Url;
                            break;

                        // ── Number / Currency ────────────────────────────────────
                        case SPFieldType.Number:
                        case SPFieldType.Currency:
                            if (double.TryParse(rawValue.ToString(), out double numVal))
                                fieldValue = numVal.ToString("N2");
                            else
                                fieldValue = rawValue.ToString();
                            break;

                        // ── Text / Note / Default ────────────────────────────────
                        default:
                            fieldValue = rawValue.ToString();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Log and leave placeholder empty on error
                    SPDiagnosticsService.Local.WriteTrace(0,
                        new SPDiagnosticsCategory("EmailTemplateHelper", TraceSeverity.Unexpected, EventSeverity.Error),
                        TraceSeverity.Unexpected,
                        $"ReplacePlaceholders failed for field '{fieldName}': {ex.Message}",
                        ex.StackTrace);

                    fieldValue = string.Empty;
                }

                template = template.Replace(placeholder, fieldValue);
            }

            return template;
        }
        public static string CleanSharePointRichText(string rawHtml)
        {
            if (string.IsNullOrEmpty(rawHtml)) return string.Empty;

            string cleaned = rawHtml;

            // Step 1: Remove ExternalClass wrapper div
            cleaned = Regex.Replace(cleaned,
                @"<div\s+class=""ExternalClass[^""]*"">",
                "", RegexOptions.IgnoreCase);

            // Step 2: Remove empty <p></p> SharePoint adds
            cleaned = cleaned.Replace("<p></p>", "");

            // Step 3: Remove SharePoint's extra <div> wrappers around each line
            cleaned = cleaned.Replace("<div>", "").Replace("</div>", "");

            // Step 4: Remove wrapping quotes SharePoint adds
            cleaned = cleaned.Trim();
            if (cleaned.StartsWith("\"")) cleaned = cleaned.Substring(1);
            if (cleaned.EndsWith("\"")) cleaned = cleaned.Substring(0, cleaned.Length - 1);

            // Step 5: Fix escaped double quotes "" → "
            cleaned = cleaned.Replace("\"\"", "\"");

            // Step 6: Final trim
            return cleaned.Trim();
        }
        public void SendEmail(
           string templateTitle,
           string toAddresses,
           Dictionary<string, string> placeholders,
           // List<string> ccAddresses = null,
           TemplateLanguage language = TemplateLanguage.En)
        {
            if (toAddresses == null || !toAddresses.Any())
                throw new ArgumentException("At least one recipient address is required.");

            var template = GetTemplate(templateTitle);
       
            var rawBody = language == TemplateLanguage.Ar
                ? template.TemplateDetailsAr
                : template.TemplateDetailsEn;

            string subject = language == TemplateLanguage.Ar
                ? template.SubjectAr
                : template.SubjectEn;

            if (toAddresses == "NA")
                toAddresses = template.MailTo;

            if (string.IsNullOrEmpty(rawBody))
                throw new Exception($"Template '{templateTitle}' body is empty for language '{language}'.");

            string cleanedTemplate = CleanSharePointRichText(rawBody);
            string finalBody = ReplacePlaceholders(cleanedTemplate, placeholders);
            subject = ReplacePlaceholders(subject, placeholders);

            bool sent =    SPUtility.SendEmail(web, true, false, toAddresses, subject, finalBody, false);
           
        }
        public void SendEmail(
         string templateTitle,
         string toAddresses,
        
         SPListItem item,
         // List<string> ccAddresses = null,
         TemplateLanguage language = TemplateLanguage.En)
        {
            if (toAddresses == null || !toAddresses.Any())
                throw new ArgumentException("At least one recipient address is required.");

            var template = GetTemplate(templateTitle);

            var rawBody = language == TemplateLanguage.Ar
                ? template.TemplateDetailsAr
                : template.TemplateDetailsEn;
            string subject = language == TemplateLanguage.Ar
            ? template.SubjectAr
            : template.SubjectEn;


            if (string.IsNullOrEmpty(rawBody))
                throw new Exception($"Template '{templateTitle}' body is empty for language '{language}'.");

            string cleanedTemplate = CleanSharePointRichText(rawBody);

            subject = ReplacePlaceholders(subject, item);

            string finalBody = ReplacePlaceholders(cleanedTemplate, item);

            bool sent = SPUtility.SendEmail(web, true, false, toAddresses, subject, finalBody, false);


        }


        #region Convert SPItem to Dictionary
        public static Dictionary<string, string> BuildTokenDictionary(SPListItem item)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (item == null) return dict;

            foreach (SPField field in item.Fields)
            {
                // Skip hidden/system fields
                if (field.Hidden || field.ReadOnlyField) continue;

                try
                {
                    object rawValue = item[field.InternalName];
                    if (rawValue == null) continue;

                    switch (field.Type)
                    {
                        // ── Lookup (Single) ──────────────────────────────────────
                        case SPFieldType.Lookup:
                            if (field is SPFieldLookup lookupField && !lookupField.AllowMultipleValues)
                            {
                                var lookupValue = new SPFieldLookupValue(rawValue.ToString());

                                // Add display value → {FieldName}
                                dict[field.InternalName] = lookupValue.LookupValue;

                                // Add all columns from lookup list → {FieldName:ColumnName}
                                AddLookupSubFields(item.Web, lookupField, lookupValue.LookupId,
                                    field.InternalName, dict);
                            }
                            // ── Lookup (Multi) ───────────────────────────────────
                            else if (field is SPFieldLookup multiLookupField && multiLookupField.AllowMultipleValues)
                            {
                                var multiLookup = new SPFieldLookupValueCollection(rawValue.ToString());
                                dict[field.InternalName] = string.Join(", ",
                                    multiLookup.Select(v => v.LookupValue));
                            }
                            break;

                        // ── User (Single) ────────────────────────────────────────
                        case SPFieldType.User:
                            if (field is SPFieldUser userField && !userField.AllowMultipleValues)
                            {
                                var userValue = new SPFieldUserValue(item.Web, rawValue.ToString());
                                string userName = userValue.User?.Name ?? userValue.LookupValue;

                                // Add display name → {FieldName}
                                dict[field.InternalName] = userName;

                                // Add sub-fields → {FieldName:Email}, {FieldName:LoginName}
                                if (userValue.User != null)
                                {
                                    dict[$"{field.InternalName}:Email"] = userValue.User.Email ?? string.Empty;
                                    dict[$"{field.InternalName}:LoginName"] = userValue.User.LoginName ?? string.Empty;
                                    dict[$"{field.InternalName}:Title"] = userValue.User.Name ?? string.Empty;
                                }
                            }
                            // ── User (Multi) ─────────────────────────────────────
                            else
                            {
                                var multiUser = new SPFieldUserValueCollection(item.Web, rawValue.ToString());
                                dict[field.InternalName] = string.Join(", ",
                                    multiUser.Select(u => u.User?.Name ?? u.LookupValue));
                            }
                            break;

                        // ── DateTime ─────────────────────────────────────────────
                        case SPFieldType.DateTime:
                            if (DateTime.TryParse(rawValue.ToString(), out DateTime dateVal))
                            {
                                dict[field.InternalName] = dateVal.ToString("dd/MM/yyyy");
                                dict[$"{field.InternalName}:Date"] = dateVal.ToString("dd/MM/yyyy");
                                dict[$"{field.InternalName}:Time"] = dateVal.ToString("HH:mm");
                                dict[$"{field.InternalName}:Full"] = dateVal.ToString("dd/MM/yyyy HH:mm");
                            }
                            break;

                        // ── Boolean ──────────────────────────────────────────────
                        case SPFieldType.Boolean:
                            dict[field.InternalName] = rawValue.ToString() == "1" ? "Yes" : "No";
                            break;

                        // ── MultiChoice ──────────────────────────────────────────
                        case SPFieldType.MultiChoice:
                            dict[field.InternalName] = rawValue.ToString()
                                .Replace(";#", ", ").Trim(',', ' ');
                            break;

                        // ── URL ──────────────────────────────────────────────────
                        case SPFieldType.URL:
                            var urlValue = new SPFieldUrlValue(rawValue.ToString());
                            dict[field.InternalName] = urlValue.Url;
                            dict[$"{field.InternalName}:Url"] = urlValue.Url;
                            dict[$"{field.InternalName}:Description"] = urlValue.Description ?? urlValue.Url;
                            break;

                        // ── Number / Currency ────────────────────────────────────
                        case SPFieldType.Number:
                        case SPFieldType.Currency:
                            if (double.TryParse(rawValue.ToString(), out double numVal))
                                dict[field.InternalName] = numVal.ToString("N2");
                            break;

                        // ── Text / Choice / Default ──────────────────────────────
                        default:
                            dict[field.InternalName] = rawValue.ToString();
                            AddAttachmentTokens(item, dict);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0,
                        new SPDiagnosticsCategory("EmailTemplateHelper", TraceSeverity.Unexpected, EventSeverity.Error),
                        TraceSeverity.Unexpected,
                        $"BuildTokenDictionary failed for field '{field.InternalName}': {ex.Message}",
                        ex.StackTrace);
                }
            }

            // Add common system tokens
            dict["ItemId"] = item.ID.ToString();
            dict["ItemUrl"] = item.Web.Url + "/Lists/" + item.ParentList.Title + "/DispForm.aspx?ID=" + item.ID;
            dict["SiteUrl"] = item.Web.Url;
            dict["Today"] = DateTime.Now.ToString("dd/MM/yyyy");
            dict["Now"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            return dict;
        }

        private static void AddAttachmentTokens(SPListItem item, Dictionary<string, string> dict)
        {
            try
            {
                int count = item.Attachments.Count;

                dict["AttachmentsCount"] = count.ToString();

                if (count == 0)
                {
                    dict["Attachments"] = string.Empty;
                    return;
                }

                string baseUrl = item.Web.Url.TrimEnd('/');
                string attachmentPrefix = item.Attachments.UrlPrefix;

                var htmlLinks = new StringBuilder();
                var plainLinks = new StringBuilder();

                htmlLinks.Append("<ul style='margin:0; padding-left:20px;'>");

                int index = 1;
                foreach (string fileName in item.Attachments)
                {
                    string fileUrl = baseUrl + "/" + attachmentPrefix.TrimStart('/') + fileName;
                    string htmlLink = $"<a href='{fileUrl}'>{fileName}</a>";

                    // Individual tokens per attachment
                    dict[$"Attachment:{index}"] = fileUrl;
                    dict[$"Attachment:{index}:Name"] = fileName;
                    dict[$"Attachment:{index}:Link"] = htmlLink;
                    dict[$"Attachment:{index}:Url"] = fileUrl;

                    // Accumulate for the combined {Attachments} token
                    htmlLinks.Append($"<li>{htmlLink}</li>");
                    plainLinks.Append($"{fileName}: {fileUrl}\n");

                    index++;
                }

                htmlLinks.Append("</ul>");

                // {Attachments}       → full HTML list of links
                // {Attachments:Plain} → plain text list
                dict["Attachments"] = htmlLinks.ToString();
                dict["Attachments:Plain"] = plainLinks.ToString().TrimEnd('\n');
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0,
                    new SPDiagnosticsCategory("EmailTemplateHelper", TraceSeverity.Unexpected, EventSeverity.Error),
                    TraceSeverity.Unexpected,
                    $"AddAttachmentTokens failed: {ex.Message}",
                    ex.StackTrace);

                dict["Attachments"] = string.Empty;
                dict["AttachmentsCount"] = "0";
            }
        }
        /// <summary>
        /// Fetches all columns from a lookup list item and adds them as {FieldName:ColumnName}
        /// </summary>
        private static void AddLookupSubFields(SPWeb web, SPFieldLookup lookupField,
            int lookupId, string fieldInternalName, Dictionary<string, string> dict)
        {
            if (lookupId <= 0) return;

            try
            {
                SPList srcList = web.Lists[new Guid(lookupField.LookupList)];
                SPListItem srcItem = srcList.GetItemById(lookupId);

                foreach (SPField srcField in srcItem.Fields)
                {
                    if (srcField.Hidden || srcField.ReadOnlyField) continue;

                    object val = srcItem[srcField.InternalName];
                    if (val == null) continue;

                    // Key format: {LookupFieldName:ColumnInternalName}
                    string key = $"{fieldInternalName}:{srcField.InternalName}";
                    dict[key] = val.ToString();
                }
            }
            catch { /* Lookup list not accessible */ }
        }
        #endregion
    }


    public class EmailTemplate
    {
        public string Title { get; set; }
        public string TemplateDetailsAr { get; set; }
        public string TemplateDetailsEn { get; set; }
        public bool IncludeCommonTemplate { get; set; }
        public string SubjectAr { get; set; }
        public string SubjectEn { get; set; }
        public string MailTo { get; set; }
    }
    public enum TemplateLanguage { En, Ar }

}
