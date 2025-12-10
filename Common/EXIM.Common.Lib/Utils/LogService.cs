using NLog;
using System;

namespace EXIM.Common.Lib.Utils
{
    public class LogService
    {
        private static readonly Logger log = NLog.LogManager.GetCurrentClassLogger();

        public static void LogException(Exception ex)
        {
            log.Fatal("Unexpected Exception: " + ex.ToString());
        }

        public static void LogInfo(string text)
        {
            log.Info(text);
        }

    }
}
