using Microsoft.SharePoint;
using System;
using System.Configuration;
using System.Web;

namespace EXIM.Common.Lib.Utils
{
    public class CommonProperties
    {
        public static bool IsArabic => SPContext.Current.Web.Language == 1025;
        public static bool IsEnglish => SPContext.Current.Web.Language == 1033;
        
        public static string CurrentUserName => HttpContext.Current.User.Identity.Name;
        public static string CurrentUserNameWithoutDomain => new UserData().GetUserNameWithoutDomain();
        
        public static string AdminSiteUrl => $"{SPContext.Current.Site.Url}/admin";
        
        public static string EmailTemplatesListUrl => $"{SPContext.Current.Site.Url}/admin/Lists/EmailTemplate";
        public static string SettingsListUrl => $"{SPContext.Current.Site.Url}/admin/Lists/Settings";

        public static string CommonTemplateName => "CommonDesignTemplate";
        public static string SupportMailKey => "SupportEmail";
        
        public static string RSSXMLSTARTTAG => "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";

        private static string CurrentEnviornment => ConfigurationManager.AppSettings["CurrentEnvironment"]?.ToLower() ?? "";
        private static string DevEnvironmentName => "development";
        public static bool isDevEnv()
        {
            return CurrentEnviornment == DevEnvironmentName;
        }        

        public static string MainPortalConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["PortalEntities"] == null)
                {
                    throw new Exception("PortalEntities connection string is not found");
                }

                return ConfigurationManager.ConnectionStrings["PortalEntities"].ConnectionString;
            }
        }
    }
}
