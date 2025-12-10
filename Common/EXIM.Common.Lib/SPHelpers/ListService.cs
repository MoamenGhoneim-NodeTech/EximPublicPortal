using EXIM.Common.Lib.Models;
using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXIM.Common.Lib.SPHelpers
{
    public class ListService
    {
        /// <summary>
        /// get list items by list items collection position
        /// </summary>
        /// <param name="siteUrl">siteUrl string site collection url</param>
        /// <param name="listUrl">listUrl string list url ex.'/en/ContactUs/Lists/ContactUs'</param>
        /// <param name="viewId">list view id</param>
        /// <param name="pos">list items collection position</param>
        /// <returns>SPListItemCollection</returns>
        public static SPListItemCollection GetItems(string siteUrl, string listUrl, Guid viewId, SPListItemCollectionPosition pos)
        {
            SPListItemCollection listItems = null;
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.GetList(listUrl);
                    if (list != null)
                    {
                        SPFolder sPFolder = web.GetFolder(listUrl);
                        SPView view = list.Views[viewId];   //custom view name
                        SPQuery query = new SPQuery(view);
                        query.Folder = sPFolder;
                        string strQuery = view.Query;
                        query.Query = strQuery;
                        query.ListItemCollectionPosition = pos;
                        listItems = list.GetItems(query);
                    }
                }
            }
            return listItems;
        }

        /// <summary>
        /// get list item collection
        /// </summary>
        /// <param name="siteUrl">siteUrl string site collection url</param>
        /// <param name="listUrl">listUrl string list url ex.'/en/ContactUs/Lists/ContactUs'</param>
        /// <param name="withElvatedPrivileges">withElvatedPrivileges bool to indicate if you want to run with elvated privileges</param>
        /// <returns>SPListItemCollection</returns>
        public static SPListItemCollection GetItems(string siteUrl, string listUrl, bool withElvatedPrivileges = false)
        {
            if (!withElvatedPrivileges)
                return GetDefault(siteUrl, listUrl);

            SPListItemCollection listItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                listItems = GetDefault(siteUrl, listUrl);
            });

            return listItems;
        }

        /// <summary>
        /// get list item collection
        /// </summary>
        /// <param name="siteUrl">siteUrl string site collection url</param>
        /// <param name="listUrl">listUrl string list url ex.'/en/ContactUs/Lists/ContactUs'</param>
        /// <param name="query">query represent SPQuery you want to execute</param>
        /// <param name="withElvatedPrivileges">withElvatedPrivileges bool to indicate if you want to run with elvated privileges</param>
        /// <returns>SPListItemCollection</returns>
        public static SPListItemCollection GetItems(string siteUrl, string listUrl, SPQuery query, bool withElvatedPrivileges = false)
        {
            if (!withElvatedPrivileges)
                return GetByQuery(siteUrl, listUrl, query);

            SPListItemCollection listItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                listItems = GetByQuery(siteUrl, listUrl, query);
            });
            return listItems;
        }

        /// <summary>
        /// get list item collection
        /// </summary>
        /// <param name="siteUrl">siteUrl string site collection url</param>
        /// <param name="listUrl">listUrl string list url ex.'/en/ContactUs/Lists/ContactUs'</param>
        /// <param name="viewName">viewName string view name to get the data by it</param>
        /// <param name="withElvatedPrivileges">withElvatedPrivileges bool to indicate if you want to run with elvated privileges</param>
        /// <returns>SPListItemCollection</returns>
        public static SPListItemCollection GetItems(string siteUrl, string listUrl, string viewName, bool withElvatedPrivileges = false)
        {
            if (!withElvatedPrivileges)
                return GetByView(siteUrl, listUrl, viewName);

            SPListItemCollection listItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                listItems = GetByView(siteUrl, listUrl, viewName);
            });

            return listItems;
        }

        /// <summary>
        /// get list items with sub items
        /// </summary>
        /// <param name="siteUrl">siteUrl string site collection url</param>
        /// <param name="listUrl">listUrl string list url ex.'/en/ContactUs/Lists/ContactUs'</param>
        /// <param name="withElvatedPrivileges">withElvatedPrivileges bool to indicate if you want to run with elvated privileges</param>
        /// <returns>SPListItemCollection</returns>
        public static List<ItemWithSub> GetItemsWithSub(string siteUrl, string listUrl, string subListUrl, string viewName, string parent, string subItemsVisibilityColumnName, bool withElvatedPrivileges = false)
        {
            if (!withElvatedPrivileges)
                return GetWithSub(siteUrl, listUrl, subListUrl, viewName, parent, subItemsVisibilityColumnName);

            List<ItemWithSub> listItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                listItems = GetWithSub(siteUrl, listUrl, subListUrl, viewName, parent, subItemsVisibilityColumnName);
            });

            return listItems;
        }

        public static List<ItemWithSub> GetItemsWithMultiSub(string siteUrl, string listUrl, string subListUrl, string viewName, string parent, bool withElvatedPrivileges = false)
        {
            if (!withElvatedPrivileges)
                return GetWithMultiSub(siteUrl, listUrl, subListUrl, viewName, parent);

            List<ItemWithSub> listItems = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                listItems = GetWithMultiSub(siteUrl, listUrl, subListUrl, viewName, parent);
            });

            return listItems;
        }

        private static SPListItemCollection GetDefault(string siteUrl, string listUrl)
        {
            SPListItemCollection listItems = null;
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.GetList(listUrl);
                    listItems = list != null ? list.GetItems() : null;
                }
            }
            return listItems;
        }

        private static SPListItemCollection GetByQuery(string siteUrl, string listUrl, SPQuery query)
        {
            SPListItemCollection listItems = null;
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.GetList(listUrl);
                    listItems = list != null ? list.GetItems(query) : null;
                }
            }

            return listItems;
        }

        private static SPListItemCollection GetByView(string siteUrl, string listUrl, string viewName)
        {
            SPListItemCollection listItems = null;
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.GetList(listUrl);
                    listItems = list != null ? list.GetItems(list.Views[viewName]) : null;
                }
            }
            return listItems;
        }

        private static List<ItemWithSub> GetWithSub(string siteUrl, string listUrl, string subListUrl, string viewName, string parent, string subItemsVisibilityColumnName)
        {
            List<ItemWithSub> items = new List<ItemWithSub>();
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList mainLlist = web.GetList(listUrl);
                    DataTable firstLeveLDataTable = mainLlist != null ? mainLlist.GetItems(mainLlist.Views[viewName]).GetDataTable() : null;

                    firstLeveLDataTable.RemoveColumns(Enum.GetNames(typeof(RemoveColumnsEnum)));

                    foreach (DataRow item in firstLeveLDataTable.Rows)
                    {
                        var mainItem = new Dictionary<string, object>();
                        foreach (DataColumn col in item.Table.Columns)
                        {
                            mainItem.Add(col.ColumnName, item[col]);
                        }

                        SPQuery query = new SPQuery()
                        {
                            Query = $@"<Where><And><Eq><FieldRef Name='{parent}' /><Value Type='Lookup'>{item["Title"]}</Value></Eq><Eq><FieldRef Name='{subItemsVisibilityColumnName}' /><Value Type='Boolean'>1</Value></Eq></And></Where>
                                            <OrderBy><FieldRef Name='EXIM_ItemOrder' Ascending='TRUE' /></OrderBy>"
                        };
                        SPList Sublist = web.GetList(subListUrl);
                        var dtSublist = Sublist != null ? Sublist.GetItems(query).GetDataTable() : null;

                        dtSublist.RemoveColumns(Enum.GetNames(typeof(RemoveColumnsEnum)));

                        items.Add(new ItemWithSub(mainItem, dtSublist));
                    }
                    return items;
                }
            }
        }

        private static List<ItemWithSub> GetWithMultiSub(string siteUrl, string listUrl, string subListUrl, string viewName, string parent)
        {
            List<ItemWithSub> items = new List<ItemWithSub>();
            using (SPSite site = new SPSite(siteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList mainLlist = web.GetList(listUrl);
                    DataTable firstLeveLDataTable = mainLlist != null ? mainLlist.GetItems(mainLlist.Views[viewName]).GetDataTable() : null;

                    firstLeveLDataTable.RemoveColumns(Enum.GetNames(typeof(RemoveColumnsEnum)));

                    foreach (DataRow item in firstLeveLDataTable.Rows)
                    {
                        var mainItem = new Dictionary<string, object>();
                        foreach (DataColumn col in item.Table.Columns)
                        {
                            mainItem.Add(col.ColumnName, item[col]);
                        }

                        SPQuery query = new SPQuery()
                        {
                            Query = $@"<Where><And><Contains><FieldRef Name='{parent}' /><Value Type='Lookup'>{item["Title"]}</Value></Contains><Eq><FieldRef Name='EXIM_Visibility' /><Value Type='Boolean'>1</Value></Eq></And></Where>
                                            <OrderBy><FieldRef Name='EXIM_ItemOrder' Ascending='TRUE' /></OrderBy>"
                        };
                        SPList Sublist = web.GetList(subListUrl);
                        var dtSublist = Sublist != null ? Sublist.GetItems(query).GetDataTable() : null;

                        dtSublist.RemoveColumns(Enum.GetNames(typeof(RemoveColumnsEnum)));

                        items.Add(new ItemWithSub(mainItem, dtSublist));
                    }
                    return items;
                }
            }
        }
    }
}
