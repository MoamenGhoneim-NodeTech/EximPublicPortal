using Microsoft.SharePoint;
using System;
using System.Web;
using System.Web.Caching;

namespace EXIM.Common.Lib.Utils
{
    public class Caching
    {
        /// <summary>
        /// Add To Cache
        /// </summary>
        /// <param name="key">key represent the cache value</param>
        /// <param name="value">value to be cached</param>
        /// <param name="minutes">minutes caching time</param>
        public static void AddToCache(string key, object value, int minutes = default)
        {
            if (SPContext.Current.Web.CurrentUser == null)
                HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// Get Object From Cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns>object of the cached value if it exists</returns>
        public static object GetFromCache(string key)
        {
            return HttpContext.Current.Cache[key];
        }
    }
}
