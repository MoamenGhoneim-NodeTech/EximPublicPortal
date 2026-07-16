using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EXIM.Common.Lib.Utils
{
    public static class ClientIpHelper
    {
        /// <summary>
        /// Returns the client's IP address, preferring X-Forwarded-For when present (e.g. behind
        /// an F5 / IIS ARR / other reverse proxy in front of the WFEs).
        ///
        /// SECURITY NOTE: Only trust X-Forwarded-For if your edge device is configured to strip
        /// any client-supplied value and set it itself. If a client can set this header directly
        /// (i.e. nothing in front of IIS overwrites it), an attacker can spoof it to get a fresh
        /// rate-limit bucket on every request. If you're not sure this is guaranteed in your
        /// topology, delete the X-Forwarded-For branch below and use Request.UserHostAddress only.
        /// </summary>
        public static string GetClientIp(HttpRequest request)
        {
            string forwarded = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(forwarded))
            {
                // X-Forwarded-For can be a comma-separated chain (client, proxy1, proxy2, ...);
                // the first entry is the original client.
                string first = forwarded.Split(',')[0].Trim();
                if (!string.IsNullOrEmpty(first))
                {
                    return first;
                }
            }

            return request.UserHostAddress;
        }
    }
}
