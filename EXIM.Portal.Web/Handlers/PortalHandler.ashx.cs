using EXIM.Common.Lib.Models;
using EXIM.Common.Lib.SPHelpers;
using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace EXIM.Portal.Web.Handlers
{
    /// <summary>
    /// Summary description for PortalHandler
    /// </summary>
    public class PortalHandler : IHttpHandler
    {
        #region Private Fields

        private HttpContext currContext;
        private int PageSize
        {
            get
            {
                var pageSize = 30;
                int.TryParse(currContext.Request["pageSize"], out pageSize);
                return pageSize;
            }
        }

        #endregion

        #region Public Methods
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                currContext = context;
                if (currContext.Request["op"] != null)
                {
                    switch (currContext.Request["op"].ToLowerInvariant())
                    {
                        case "loaditems":
                            GetListItemsByViewName();
                            break;
                        case "loaditemswithsubs":
                            GetListItemsWithSubItemsByViewName();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
                var error = new { errorMessage = "Unexpected error has occurred. Please contact system administrator." };
                string data = JsonConvert.SerializeObject(error);
                WriteResponse(data);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region LoadItemsFromLists
        private void GetListItemsByViewName()
        {
            if (currContext.Request["listurl"] != null && currContext.Request["viewname"] != null)
            {
                string listUrl = currContext.Request["listurl"].ToString();
                string viewName = currContext.Request["viewname"].ToString();
                string calType = string.IsNullOrEmpty(currContext.Request["caltype"]) ? "" : currContext.Request["caltype"].ToString();
                string[] skipFields = (currContext.Request["skip"] != null) ? currContext.Request["skip"].Split(',') : null;
                string cachKey = $"listItem_{listUrl}_{viewName}";

                DataTable dt;
                if (Caching.GetFromCache(cachKey) == null)
                {
                    if (currContext.Request["isDoc"] == "1")
                    {
                        SPListItemCollection files = ListService.GetItems(SPContext.Current.Site.Url, listUrl, viewName);
                        var result = new List<SPFile>();
                        foreach (SPListItem item in files)
                        {
                            if (item.File != null)
                                result.Add(item.File);
                        }
                        var res = from f in result select new { f.Name, f.ServerRelativeUrl, f.Title, DocIcon = f.IconUrl };
                        string filedata = JsonConvert.SerializeObject(res);
                        WriteResponse(filedata);
                        return;
                    }
                    else
                    {
                        dt = ListService.GetItems(SPContext.Current.Site.Url, listUrl, viewName).GetDataTable();
                        RemoveMetadataColumns(dt, skipFields);
                        Caching.AddToCache(cachKey, dt);
                    }
                }
                else
                    dt = (DataTable)Caching.GetFromCache(cachKey);

                IsoDateTimeConverter isoDate = ValidateCluture(calType);

                string data = JsonConvert.SerializeObject(dt, isoDate);
                WriteResponse(data);
            }
        }
        private void GetListItemsWithSubItemsByViewName()
        {
            if (currContext.Request["listUrl"] != null && currContext.Request["viewName"] != null)
            {
                string listUrl = currContext.Request["listUrl"].ToString();
                string viewName = currContext.Request["viewName"].ToString();
                string subList = currContext.Request["subList"].ToString();
                string parentCol = currContext.Request["parent"] != null ? currContext.Request["parent"].ToString() : "Parent";
                string subItemsVisibilityColumnName = currContext.Request["subviscolumn"] != null ? currContext.Request["subviscolumn"].ToString() : "EXIM_Visibility";
                List<ItemWithSub> items = new List<ItemWithSub>();
                string cachKey = $"listItem_{listUrl}_{viewName}";

                if (Caching.GetFromCache(cachKey) == null)
                {
                    items = ListService.GetItemsWithSub(SPContext.Current.Site.Url, listUrl, subList, viewName, parentCol, subItemsVisibilityColumnName);
                    Caching.AddToCache(cachKey, items);
                }
                else
                    items = Caching.GetFromCache(cachKey) as List<ItemWithSub>;

                string data = JsonConvert.SerializeObject(items);
                WriteResponse(data);
            }
        }
        #endregion

        #region Private Methods
        private void WriteResponse(string data, string contentType = "application/json")
        {
            currContext.Response.ClearContent();
            currContext.Response.ContentType = contentType;
            currContext.Response.ContentEncoding = Encoding.UTF8;
            currContext.Response.Write(data);
            currContext.ApplicationInstance.CompleteRequest();
        }

        private void RemoveMetadataColumns(DataTable dt, string[] skipFields = default)
        {
            if (dt == null) return;

            foreach (var column in Enum.GetNames(typeof(RemoveColumnsEnum)))
                dt.RemoveColumn(column, skipFields);
        }

        private IsoDateTimeConverter ValidateCluture(string calType)
        {
            IsoDateTimeConverter isoDate = new IsoDateTimeConverter();
            isoDate.DateTimeFormat = currContext.Request["df"] == null ? "dd/MM/yyyy" : currContext.Request["df"];
            isoDate.Culture = new CultureInfo("en-US");

            if (currContext.Request["lang"] == "ar")
            {
                isoDate.Culture = (calType == CalendarTypeEnum.miladi.ToString()) ? new CultureInfo("ar-AE") : new CultureInfo("ar-SA");
            }

            return isoDate;
        }
        #endregion
    }
}