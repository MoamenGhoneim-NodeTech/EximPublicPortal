using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EXIM.Common.Lib.Utils
{
    public class QueryString
    {
        /// <summary>
        /// get query string value by key
        /// </summary>
        /// <param name="key">string key</param>
        /// <returns>string contains the value of the key if it exists</returns>
        public static string GetValue(string key, bool maybeEmpty = false)
        {
            if (!IsValidQueryString(key, maybeEmpty))
                throw new MissingFieldException($"{key} is not exists");

            return HttpContext.Current.Request[key];
        }

        /// <summary>
        /// Get all query string parameters
        /// </summary>
        /// <param name="exceptedKeys">the keys you need to return</param>
        /// <returns>Hashtable contains the expected keys with its values</returns>
        public static Hashtable GetList(string[] exceptedKeys)
        {
            Hashtable queryStringTable = new Hashtable();

            foreach (string key in exceptedKeys)
                queryStringTable.Add(key, GetValue(key, true));

            return queryStringTable;
        }

        private static bool IsValidQueryString(string key, bool maybeEmpty)
        {
            if (maybeEmpty)
                return maybeEmpty;

            return HttpContext.Current.Request[key] != null;
        }
    }
}
