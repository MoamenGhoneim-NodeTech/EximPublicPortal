using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint;

namespace EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.FindMyProduct
{
    /// <summary>
    /// Product Consultant User Control - Helps users find suitable banking products
    /// </summary>
    public partial class FindMyProduct : UserControl
    {
        #region Constants

        private const int ARABIC_LCID = 1025;
        private const string BENEFICIARY_FIELD_AR = "Beneficiary_AR";
        private const string BENEFICIARY_FIELD_EN = "Beneficiary_EN";
        private const string USAGE_FIELD_AR = "Usage_AR";
        private const string USAGE_FIELD_EN = "Title";
        private const string SERVICE_NAME_AR = "Service_x0020_Name_AR";
        private const string SERVICE_NAME_EN = "Service_x0020_Name_EN";
        private const string SERVICE_DESC_AR = "Service_x0020_Description_AR";
        private const string SERVICE_DESC_EN = "Service_x0020_Description_EN";
        private const string EXTERNAL_URL_AR = "External_x0020_Apply_x0020_URL_A";
        private const string EXTERNAL_URL_EN = "External_x0020_Apply_x0020_URL_E";
        private const string INTERNAL_URL_AR = "Internal_x0020_URL_AR";
        private const string INTERNAL_URL_EN = "Internal_x0020_URL_EN";
        private const string LIST_NAME = "ProductConsultant";
     
        #endregion

        #region Private Fields

        private bool _isArabic;
        private string _beneficiaryFieldName;
        private SPList _productConsultantList;
        private SPSite _site;
        private SPWeb _productWeb;

        #endregion

        #region Page Events

        /// <summary>
        /// Page Load Event Handler
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeVariables();

                if (!Page.IsPostBack)
                {
                    InitializeControls();
                }
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
            }
        }

        /// <summary>
        /// Page Unload - Clean up resources
        /// </summary>
        protected void Page_Unload(object sender, EventArgs e)
        {
            DisposeResources();
        }

        #endregion

        #region Control Events

        /// <summary>
        /// Beneficiary DropDown Selected Index Changed Event
        /// </summary>
        protected void ddl_Benificiary_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                    BindUsageDropDown();
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
            }
        }

        /// <summary>
        /// Submit Button Click Event Handler
        /// </summary>
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayProductResult(int.Parse(ddl_Usage.SelectedValue));
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
            }
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Initialize all variables and get SharePoint context
        /// </summary>
        private void InitializeVariables()
        {
            Guid siteId = SPContext.Current.Site.ID;
            _isArabic = IsArabicSite(SPContext.Current.Site); 
            _beneficiaryFieldName = _isArabic ? BENEFICIARY_FIELD_AR : BENEFICIARY_FIELD_EN;

            string siteUrl = GetResourceString("Product-ConsultantArSiteURL");
            if (string.IsNullOrEmpty(siteUrl))
            {
                throw new InvalidOperationException("Product-ConsultantArSiteURL resource not found");
            }

            // Store site reference for proper disposal
            _site = new SPSite(siteId);
            _productWeb = _site.OpenWeb(siteUrl);
            _productConsultantList = _productWeb.Lists.TryGetList(LIST_NAME);

            if (_productConsultantList == null)
            {
                throw new InvalidOperationException($"List '{LIST_NAME}' not found");
            }
        }

        /// <summary>
        /// Initialize all controls on page load
        /// </summary>
        private void InitializeControls()
        {
            BindBeneficiaryDropDown();
            BindUsageDropDown();
            HideResultPanel();
        }

        private bool IsArabicSite(SPSite site)
        {
            try
            {
                // Method 1: Check web language directly
                using (SPWeb web = site.RootWeb)
                {
                    if (web.Language == ARABIC_LCID) // Arabic LCID
                        return true;
                }

                // Method 2: Check current UI culture
                int currentLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
                if (currentLCID == ARABIC_LCID)
                    return true;

                // Method 3: Check URL for language indicator
                string url = site.Url.ToLower();
                if (url.Contains("/ar/") || url.Contains("/ar-") || url.Contains("-ar"))
                    return true;

                // Method 4: Check query string parameter
                string langParam = System.Web.HttpContext.Current.Request.QueryString["lang"];
                if (!string.IsNullOrEmpty(langParam) && langParam.ToLower() == "ar")
                    return true;

                // Default to English
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Data Binding Methods

        /// <summary>
        /// Bind Beneficiary dropdown with choices from SharePoint field
        /// </summary>
        private void BindBeneficiaryDropDown()
        {
            ddl_Benificiary.Items.Clear();
            if (_productConsultantList == null)
            {
              
                return;
            }

            try
            {
                SPField field = _productConsultantList.Fields.GetFieldByInternalName(_beneficiaryFieldName);
                SPFieldChoice choiceField = field as SPFieldChoice;

                if (choiceField == null)
                {
                    return;
                }

                foreach (string choice in choiceField.Choices)
                {
                    if (!string.IsNullOrEmpty(choice))
                    {
                        ddl_Benificiary.Items.Add(new ListItem(choice, choice));
                    }
                }
            }
            catch (ArgumentException ex)
            {
                LogService.LogException(ex);
            }
        }

        /// <summary>
        /// Bind Usage dropdown based on selected beneficiary
        /// </summary>
        private void BindUsageDropDown()
        {
            ddl_Usage.Items.Clear();

           
            if (_productConsultantList == null)
                return;

            string selectedBeneficiary = ddl_Benificiary.SelectedValue;
            string camlQuery = BuildBeneficiaryFilterQuery(selectedBeneficiary);

            SPListItemCollection filteredItems = GetFilteredListItems(camlQuery);

            if (filteredItems != null && filteredItems.Count > 0)
            {
                DataTable dataTable = filteredItems.GetDataTable();

                ddl_Usage.DataSource = dataTable;
                ddl_Usage.DataTextField = _isArabic ? USAGE_FIELD_AR : USAGE_FIELD_EN;
                ddl_Usage.DataValueField = "ID";
                ddl_Usage.DataBind();

               
            }
        }

        /// <summary>
        /// Clear Usage dropdown
        /// </summary>
        

        #endregion

        #region Query and Data Access Methods

        /// <summary>
        /// Build CAML query to filter items by beneficiary
        /// </summary>
        private string BuildBeneficiaryFilterQuery(string beneficiaryValue)
        {
            if (string.IsNullOrEmpty(beneficiaryValue))
                return string.Empty;

            // Encode XML special characters
            string encodedValue = System.Security.SecurityElement.Escape(beneficiaryValue);

            return string.Format(
                "<Where><Eq><FieldRef Name='{0}' /><Value Type='Choice'>{1}</Value></Eq></Where>",
                _beneficiaryFieldName,
                encodedValue);
        }

        /// <summary>
        /// Get filtered list items using CAML query
        /// </summary>
        private SPListItemCollection GetFilteredListItems(string camlQuery)
        {
            if (_productConsultantList == null)
                return null;

            SPListItemCollection items;

            if (string.IsNullOrEmpty(camlQuery))
            {
                items = _productConsultantList.Items;
            }
            else
            {
                SPQuery query = new SPQuery
                {
                    Query = camlQuery
                };
                items = _productConsultantList.GetItems(query);
            }

            return items;
        }

        /// <summary>
        /// Get specific list item by ID
        /// </summary>
        private SPListItem GetListItemById(int itemId)
        {
            if (_productConsultantList == null)
                return null;

            try
            {
                return _productConsultantList.GetItemById(itemId);
            }
            catch (ArgumentException ex)
            {
           
                return null;
            }
        }

        #endregion

        #region Display Methods

        /// <summary>
        /// Display product result based on selected item
        /// </summary>
        private void DisplayProductResult(int itemId)
        {
            SPListItem selectedItem = GetListItemById(itemId);

            // Set service information
            string serviceName = GetFieldValue(selectedItem,
                _isArabic ? SERVICE_NAME_AR : SERVICE_NAME_EN);
            string serviceDescription = GetFieldValue(selectedItem,
                _isArabic ? SERVICE_DESC_AR : SERVICE_DESC_EN);
            string requestUrl = GetFieldValue(selectedItem,
                _isArabic ? EXTERNAL_URL_AR : EXTERNAL_URL_EN);
            string detailsUrl = GetFieldValue(selectedItem,
                _isArabic ? INTERNAL_URL_AR : INTERNAL_URL_EN);


            // Update UI controls
            lbl_ServiceName.Text = serviceName;
            lbl_ServiceDescription.Text = serviceDescription;

            // Set URLs (validate before setting)
            hypr_RequestService.NavigateUrl = ValidateAndEncodeUrl(requestUrl);
            hypr_ServiceDetails.NavigateUrl = ValidateAndEncodeUrl(detailsUrl);

            // Toggle panels
            ShowResultPanel();
        }

        /// <summary>
        /// Show result panel and hide form panel
        /// </summary>
        private void ShowResultPanel()
        {
            divProductPanel.Attributes["class"] = "product-panel d-none";
            divProductPanelSelected.Attributes["class"] = "product-panel-selected";
        }

        /// <summary>
        /// Hide result panel and show form panel
        /// </summary>
        private void HideResultPanel()
        {
            divProductPanel.Attributes["class"] = "product-panel";
            divProductPanelSelected.Attributes["class"] = "product-panel-selected d-none";
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Validate form before submission
        /// </summary>
       
        /// <summary>
        /// Validate and encode URL
        /// </summary>
        private string ValidateAndEncodeUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "#";

            // Basic URL validation
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                return Server.HtmlEncode(url);
            }

            return "#";
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get field value from list item safely
        /// </summary>
        private string GetFieldValue(SPListItem item, string fieldName)
        {
            if (item == null || string.IsNullOrEmpty(fieldName))
                return string.Empty;

            try
            {
                object fieldValue = item[fieldName];
                return fieldValue != null ? fieldValue.ToString() : string.Empty;
            }
            catch (ArgumentException ex)
            {
                
                return string.Empty;
            }
        }

        /// <summary>
        /// Get resource string safely
        /// </summary>
        private string GetResourceString(string resourceKey)
        {
            try
            {
                object resource = GetLocalResourceObject(resourceKey);
                return resource != null ? resource.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
            
                return string.Empty;
            }
        }

        /// <summary>
        /// Dispose SPWeb and SPSite resources properly
        /// </summary>
        private void DisposeResources()
        {
            try
            {
                if (_productWeb != null)
                {
                    _productWeb.Dispose();
                    _productWeb = null;
                }

                if (_site != null)
                {
                    _site.Dispose();
                    _site = null;
                }
            }
            catch (Exception ex)
            {
              
                
            }
        }

        #endregion

    }
}