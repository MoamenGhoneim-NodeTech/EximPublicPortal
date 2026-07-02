using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using Microsoft.SharePoint;
using Microsoft.Office.Server.Search.Query;

namespace EXIM.Portal.WebParts
{
    /// <summary>
    /// Server-side proxy for SharePoint Search REST API.
    ///
    /// WHY THIS EXISTS:
    ///   The built-in /_api/search/query endpoint throws:
    ///     SearchServiceException: The SafeQueryPropertiesTemplateUrl "{0}" is not a valid URL.
    ///   for anonymous users because SearchExecutor.OverlaySafeQueryPropertiesTemplate()
    ///   tries to resolve a display-template URL from SPWeb property-bag values
    ///   (SRCH_ENH_FTR_URL / SRCH_SB_SET_SITE) into an absolute URL, but in an
    ///   anonymous request context that combine operation fails.
    ///
    /// THE FIX:
    ///   By explicitly setting KeywordQuery.SafeQueryPropertiesTemplateUrl = ""
    ///   before execution we give OverlaySafeQueryPropertiesTemplate a valid (empty)
    ///   value, so it short-circuits the broken URL combine and proceeds normally.
    ///
    /// SECURITY:
    ///   NO RunWithElevatedPrivileges is used.
    ///   The query runs under the current HTTP user (anonymous or authenticated),
    ///   so SharePoint Search security trimming is fully respected — anonymous
    ///   users only ever receive results they are permitted to see.
    /// </summary>
    public class SearchProxy : IHttpHandler
    {
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // ── Read query-string params ──────────────────────────────────────
            string queryText = (context.Request.QueryString["q"] ?? "").Trim();
            string sortRaw = context.Request.QueryString["sort"] ?? "";  // "Property:direction"
            int rowLimit = TryParseInt(context.Request.QueryString["rows"], 500);
            int startRow = TryParseInt(context.Request.QueryString["start"], 0);

            if (string.IsNullOrEmpty(queryText))
            {
                Write(context, new { error = "Empty query" });
                return;
            }

            try
            {
                // SPContext.Current is available because the handler is inside
                // the SharePoint web application — no elevated privileges needed.
                using (var site = new SPSite(SPContext.Current.Site.Url))
                {
                    // ── Build query ───────────────────────────────────────────
                    var kq = new KeywordQuery(site)
                    {
                        QueryText = queryText,
                        RowLimit = rowLimit,
                        StartRow = startRow,
                        EnableQueryRules = false,   // rules are evaluated server-side
                        TrimDuplicates = true,

                        // ── THE CORE FIX ──────────────────────────────────────
                        // Setting this to an empty string prevents
                        // OverlaySafeQueryPropertiesTemplate() from attempting
                        // to resolve/combine the value from the web property bag,
                        // which is the exact call that throws for anonymous users.
                        // An empty string is treated as "no template" and the
                        // method returns immediately without throwing.
                        SafeQueryPropertiesTemplateUrl = ""
                    };

                    // Selected properties — same set as the original REST call
                    kq.SelectProperties.Add("Title");
                    kq.SelectProperties.Add("Path");
                    kq.SelectProperties.Add("CommentsOWSMTXT");
                    kq.SelectProperties.Add("ContentClass");
                    kq.SelectProperties.Add("Description");
                    kq.SelectProperties.Add("FileExtension");

                    // ── Apply sort ────────────────────────────────────────────
                    // sortRaw format: "Property:ascending" or "Property:descending"
                    if (!string.IsNullOrEmpty(sortRaw))
                    {
                        var parts = sortRaw.Split(new[] { ':' }, 2);
                        if (parts.Length == 2)
                        {
                            var dir = parts[1].Equals("descending",
                                          StringComparison.OrdinalIgnoreCase)
                                      ? SortDirection.Descending
                                      : SortDirection.Ascending;
                            kq.SortList.Add(parts[0].Trim(), dir);
                        }
                    }

                    // ── Execute ───────────────────────────────────────────────
                    var executor = new SearchExecutor();
                    var resultTables = executor.ExecuteQuery(kq);

                    var items = new List<object>();
                    int total = 0;

                    foreach (ResultTable rt in resultTables)
                    {
                        if (rt.TableType != KnownTableTypes.RelevantResults) continue;

                        total = (int)rt.TotalRows;

                        foreach (DataRow row in rt.Table.Rows)
                        {
                            items.Add(new
                            {
                                Title = GetVal(row, "Title"),
                                Path = GetVal(row, "Path"),
                                Description = GetVal(row, "CommentsOWSMTXT")
                                             ?? GetVal(row, "Description"),
                                ContentClass = GetVal(row, "ContentClass"),
                                FileExtension = GetVal(row, "FileExtension")
                            });
                        }

                        break; // RelevantResults table found, no need to continue
                    }

                    Write(context, new { totalRows = total, items = items });
                }
            }
            catch (Exception ex)
            {
                // Return a JSON error so the client JS can surface it via showError()
                Write(context, new { error = ex.Message });
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static string GetVal(DataRow row, string col)
        {
            if (!row.Table.Columns.Contains(col)) return null;
            var v = row[col];
            return (v == null || v == DBNull.Value) ? null : v.ToString();
        }

        private static int TryParseInt(string s, int fallback)
        {
            return int.TryParse(s, out int v) ? v : fallback;
        }

        private static void Write(HttpContext ctx, object obj)
        {
            ctx.Response.Write(new JavaScriptSerializer().Serialize(obj));
        }
    }
}
