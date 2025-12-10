using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace EXIM.Common.Lib.Utils
{
    public static class DateTimeHelper
    {
        #region Enums

        public enum DaysNames
        {
            Saturday,
            Sunday,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday
        }

        #endregion

        #region From String to Date

        /// <summary>
        /// Convert string Hijri date to UmAlQura date
        /// </summary>
        /// <param name="hijriStr">Hijry date</param>
        /// <returns>UnAlQura DateTime format</returns>
        public static DateTime FromHijriStringToUmAlQuraDate(string hijriStr)
        {
            HijriCalendar hijriCal = new HijriCalendar();
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            DateTime hijriDate = DateTimeHelper.StringToDate(hijriStr, hijriCal);

            string umAlQuraString = hijriDate.ToString("dd/MM/yyyy", umAlQuraCulture);
            DateTime umAlQuraDate = DateTimeHelper.StringToDate(umAlQuraString, umAlQuraCal);
            return umAlQuraDate;
        }

        /// <summary>
        /// Convert Hijri string date to Miladi (US date time format) date
        /// </summary>
        /// <param name="hijriStr">Hijry date</param>
        /// <returns>Gregorian DateTime format</returns>
        public static DateTime FromHijriStringToMiladiDate(string hijriStr)
        {
            HijriCalendar hijriCal = new HijriCalendar();
            GregorianCalendar miladiCal = new GregorianCalendar();

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            DateTime hijriDate = DateTimeHelper.StringToDate(hijriStr, hijriCal);

            string miladiString = hijriDate.ToString("dd/MM/yyyy", miladiCulture);
            DateTime miladiDate = DateTimeHelper.StringToDate(miladiString, miladiCal);
            return miladiDate;
        }

        /// <summary>
        /// Convert string UmAlQUra date to Hijri date
        /// </summary>
        /// <param name="umAlQuraStr">Hijry date</param>
        /// <returns>Hijri DateTime format</returns>
        public static DateTime FromUmAlQuraStringToHijriDate(string umAlQuraStr)
        {
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();
            HijriCalendar hijriCal = new HijriCalendar();

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            DateTime umAlQuraDate = DateTimeHelper.StringToDate(umAlQuraStr, umAlQuraCal);

            string hijriString = umAlQuraDate.ToString("dd/MM/yyyy", hijriCulture);
            DateTime hijriDate = DateTimeHelper.StringToDate(hijriString, hijriCal);
            return hijriDate;
        }

        /// <summary>
        /// Convert UmAlQura string date to Miladi (US date time format) date
        /// </summary>
        /// <param name="hijriStr">UmAlQura date</param>
        /// <returns>Gregorian DateTime format</returns>
        public static DateTime FromUmAlQuraStringToMiladiDate(string umAlQuraStr)
        {
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();
            GregorianCalendar miladiCal = new GregorianCalendar();

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            DateTime umAlQuraDate = DateTimeHelper.StringToDate(umAlQuraStr, umAlQuraCal);

            string miladiString = umAlQuraDate.ToString("dd/MM/yyyy", miladiCulture);
            DateTime miladiDate = DateTimeHelper.StringToDate(miladiString, miladiCal);
            return miladiDate;
        }

        /// <summary>
        /// Convert Miladi string date to UmAlQura date
        /// </summary>
        /// <param name="miladiStr">Miladi string date</param>
        /// <returns>UmAlQura DateTime format</returns>
        public static DateTime FromMiladiStringToUmAlQuraDate(string miladiStr)
        {
            GregorianCalendar miladiCal = new GregorianCalendar();
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            DateTime miladiDate = DateTimeHelper.StringToDate(miladiStr, miladiCal);

            string umAlQuraString = miladiDate.ToString("dd/MM/yyyy", umAlQuraCulture);
            DateTime umAlQuraDate = DateTimeHelper.StringToDate(umAlQuraString, umAlQuraCal);
            return umAlQuraDate;
        }

        /// <summary>
        /// Convert Miladi string date to Miladi date
        /// </summary>
        /// <param name="miladiStr">Miladi string date</param>
        /// <returns>Miladi DateTime format</returns>
        public static DateTime FromMiladiStringToMiladiDate(string miladiStr)
        {
            try
            {
                GregorianCalendar miladiCal = new GregorianCalendar();
                CultureInfo miladiCulture = new CultureInfo("en-US");
                miladiCulture.DateTimeFormat.Calendar = miladiCal;

                DateTime miladiDate = DateTimeHelper.StringToDate(miladiStr, miladiCal);

                return miladiDate;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Convert Miladi string date to Hijri date
        /// </summary>
        /// <param name="miladiStr">Miladi string date</param>
        /// <returns>Hijri DateTime format</returns>
        public static DateTime FromMiladiStringToHijriDate(string miladiStr)
        {
            GregorianCalendar miladiCal = new GregorianCalendar();
            HijriCalendar hijriCal = new HijriCalendar();

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            DateTime miladiDate = DateTimeHelper.StringToDate(miladiStr, miladiCal);

            string hijriString = miladiDate.ToString("dd/MM/yyyy", hijriCulture);
            DateTime hijriDate = DateTimeHelper.StringToDate(hijriString, hijriCal);
            return hijriDate;
        }

        #endregion

        #region Date To String

        public static string FromDateToMiladiString(DateTime Date, string format = "dd/MM/yyyy")
        {
            GregorianCalendar miladiCal = new GregorianCalendar();

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            string miladiString = Date.ToString(format, miladiCulture);
            return miladiString;
        }

        public static string FromHijriDateToUmAlQuraString(DateTime hijriDate)
        {
            HijriCalendar hijriCal = new HijriCalendar();
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            string umAlQuraString = hijriDate.ToString("dd/MM/yyyy", umAlQuraCulture);
            return umAlQuraString;
        }

        public static string FromHijriDateToMiladiString(DateTime hijriDate)
        {
            HijriCalendar hijriCal = new HijriCalendar();
            GregorianCalendar miladiCal = new GregorianCalendar();

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            string miladiString = hijriDate.ToString("dd/MM/yyyy", miladiCulture);
            return miladiString;
        }

        public static string FromUmAlQuraDateToHijriString(DateTime umAlQuraDate)
        {
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();
            HijriCalendar hijriCal = new HijriCalendar();

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            string hijriString = umAlQuraDate.ToString("dd/MM/yyyy", hijriCulture);
            return hijriString;
        }

        public static string FromUmAlQuraDateToUmAlQuraString(DateTime umAlQuraDate)
        {
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();
            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");

            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            return umAlQuraDate.ToString("dd/MM/yyyy", umAlQuraCulture);
        }

        public static string FromMiladiDateToMiladiString(DateTime gregDate)
        {
            GregorianCalendar greg = new GregorianCalendar();
            CultureInfo enCulture = new CultureInfo("en-US");

            enCulture.DateTimeFormat.Calendar = greg;

            return gregDate.ToString("dd/MM/yyyy", enCulture);
        }

        public static string FromUmAlQuraDateToMiladiString(DateTime umAlQuraDate)
        {
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();
            GregorianCalendar miladiCal = new GregorianCalendar();

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            string miladiString = umAlQuraDate.ToString("dd/MM/yyyy", miladiCulture);
            return miladiString;
        }

        public static string FromMiladiDateToUmAlQuraString(DateTime miladiDate)
        {
            GregorianCalendar miladiCal = new GregorianCalendar();
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            CultureInfo miladiCulture = new CultureInfo("ar-EG");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            string umAlQuraString = miladiDate.ToString("dd/MM/yyyy", umAlQuraCulture);
            return umAlQuraString;
        }

        /// <summary>
        /// Convert from Gregorean date to UmAlQura date
        /// </summary>
        /// <param name="miladiDate">Gregorean Date</param>
        /// <param name="dateFormat">String format (The format must be true format or exception will rised)</param>
        /// <returns></returns>
        public static string FromMiladiDateToUmAlQuraString(DateTime miladiDate, string dateFormat)
        {
            GregorianCalendar miladiCal = new GregorianCalendar();
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            string umAlQuraString = miladiDate.ToString(dateFormat, umAlQuraCulture);
            return umAlQuraString;
        }

        public static string FromMiladiDateToHijriString(DateTime miladiDate)
        {
            GregorianCalendar miladiCal = new GregorianCalendar();
            HijriCalendar hijriCal = new HijriCalendar();

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            string hijriString = miladiDate.ToString("dd/MM/yyyy", hijriCulture);
            return hijriString;
        }

        #endregion

        #region From String to String

        public static String FromHijriStringToUmAlQuraString(string hijriStr)
        {
            HijriCalendar hijriCal = new HijriCalendar();
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            DateTime hijriDate = DateTimeHelper.StringToDate(hijriStr, hijriCal);

            string umAlQuraString = hijriDate.ToString("dd/MM/yyyy", umAlQuraCulture);
            return umAlQuraString;
        }

        public static String FromHijriStringToMiladiString(string hijriStr)
        {
            HijriCalendar hijriCal = new HijriCalendar();
            GregorianCalendar miladiCal = new GregorianCalendar();

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            DateTime hijriDate = DateTimeHelper.StringToDate(hijriStr, hijriCal);

            string miladiString = hijriDate.ToString("dd/MM/yyyy", miladiCulture);
            return miladiString;
        }

        public static String FromUmAlQuraStringToHijriString(string umAlQuraStr)
        {
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();
            HijriCalendar hijriCal = new HijriCalendar();

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            DateTime umAlQuraDate = DateTimeHelper.StringToDate(umAlQuraStr, umAlQuraCal);

            string hijriString = umAlQuraDate.ToString("dd/MM/yyyy", hijriCulture);
            return hijriString;
        }

        public static String FromUmAlQuraStringToMiladiString(string umAlQuraStr)
        {
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();
            GregorianCalendar miladiCal = new GregorianCalendar();

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            DateTime umAlQuraDate = DateTimeHelper.StringToDate(umAlQuraStr, umAlQuraCal);

            string miladiString = umAlQuraDate.ToString("dd/MM/yyyy", miladiCulture);
            return miladiString;
        }

        public static String FromUmAlQuraStringToMiladiString(string umAlQuraStr, string format)
        {
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();
            GregorianCalendar miladiCal = new GregorianCalendar();

            CultureInfo miladiCulture = new CultureInfo("ar-EG");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            DateTime umAlQuraDate = DateTimeHelper.StringToDate(umAlQuraStr, umAlQuraCal);

            string miladiString = umAlQuraDate.ToString(format, miladiCulture);
            return miladiString;
        }

        public static String FromMiladiStringToUmAlQuraString(string miladiStr)
        {
            GregorianCalendar miladiCal = new GregorianCalendar();
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            DateTime miladiDate = DateTimeHelper.StringToDate(miladiStr, miladiCal);

            string umAlQuraString = miladiDate.ToString("dd/MM/yyyy", umAlQuraCulture);
            return umAlQuraString;
        }

        public static String FromMiladiStringToUmAlQuraString(string miladiStr, string dateFormat)
        {
            GregorianCalendar miladiCal = new GregorianCalendar();
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            CultureInfo miladiCulture = new CultureInfo("ar-EG");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            DateTime miladiDate = DateTimeHelper.StringToDate(miladiStr, miladiCal);

            string umAlQuraString = miladiDate.ToString(dateFormat, umAlQuraCulture);
            return umAlQuraString;
        }

        public static String FromMiladiStringToHijriString(string miladiStr)
        {
            GregorianCalendar miladiCal = new GregorianCalendar();
            HijriCalendar hijriCal = new HijriCalendar();

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            DateTime miladiDate = DateTimeHelper.StringToDate(miladiStr, miladiCal);

            string hijriString = miladiDate.ToString("dd/MM/yyyy", hijriCulture);
            return hijriString;
        }

        #endregion

        #region From Date To Date

        public static DateTime FromHijriDateToUmAlQuraDate(DateTime hijriDate)
        {
            HijriCalendar hijriCal = new HijriCalendar();
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            string umAlQuraString = hijriDate.ToString("dd/MM/yyyy", umAlQuraCulture);

            DateTime umAlQuraDate = DateTimeHelper.StringToDate(umAlQuraString, umAlQuraCal);
            return umAlQuraDate;
        }

        public static DateTime FromHijriDateToMiladiDate(DateTime hijriDate)
        {
            HijriCalendar hijriCal = new HijriCalendar();
            GregorianCalendar miladiCal = new GregorianCalendar();

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            string miladiString = hijriDate.ToString("dd/MM/yyyy", miladiCulture);

            DateTime miladiDate = DateTimeHelper.StringToDate(miladiString, miladiCal);
            return miladiDate;
        }

        public static DateTime FromUmAlQuraDateToHijriDate(DateTime umAlQuraDate)
        {
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();
            HijriCalendar hijriCal = new HijriCalendar();

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            string hijriString = umAlQuraDate.ToString("dd/MM/yyyy", hijriCulture);

            DateTime hijriDate = DateTimeHelper.StringToDate(hijriString, hijriCal);
            return hijriDate;
        }

        public static DateTime FromUmAlQuraDateToMiladiDate(DateTime umAlQuraDate)
        {
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();
            GregorianCalendar miladiCal = new GregorianCalendar();

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            string miladiString = umAlQuraDate.ToString("dd/MM/yyyy", miladiCulture);

            DateTime miladiDate = DateTimeHelper.StringToDate(miladiString, miladiCal);
            return miladiDate;
        }

        public static DateTime FromMiladiDateToUmAlQuraDate(DateTime miladiDate)
        {
            GregorianCalendar miladiCal = new GregorianCalendar();
            System.Globalization.UmAlQuraCalendar umAlQuraCal = new System.Globalization.UmAlQuraCalendar();

            CultureInfo umAlQuraCulture = new CultureInfo("ar-SA");
            umAlQuraCulture.DateTimeFormat.Calendar = umAlQuraCal;

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            string umAlQuraString = miladiDate.ToString("dd/MM/yyyy", umAlQuraCulture);

            DateTime umAlQuraDate = DateTimeHelper.StringToDate(umAlQuraString, umAlQuraCal);
            return umAlQuraDate;
        }

        public static DateTime FromMiladiDateToHijriDate(DateTime miladiDate)
        {
            GregorianCalendar miladiCal = new GregorianCalendar();
            HijriCalendar hijriCal = new HijriCalendar();

            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            hijriCulture.DateTimeFormat.Calendar = hijriCal;

            CultureInfo miladiCulture = new CultureInfo("en-US");
            miladiCulture.DateTimeFormat.Calendar = miladiCal;

            string hijriString = miladiDate.ToString("dd/MM/yyyy", hijriCulture);

            DateTime hijriDate = DateTimeHelper.StringToDate(hijriString, hijriCal);
            return hijriDate;
        }

        #endregion

        #region Misc

        public static string[] GetMonthNames(System.Globalization.Calendar calendar, System.Globalization.CultureInfo Culture)
        {
            if ((calendar is UmAlQuraCalendar) && (Culture.Name == "en-US"))
            {
                return new[]
                           {
                               "Muharram", "Safar", "Rabi' al-awwal", "Rabi' al-thani", "Jumada al-awwal",
                               "Jumada al-thani", "Rajab", "Sha'aban", "Ramadan", "Shawwal", "Dhu al-Qi'dah",
                               "Dhu al-Hijjah"
                           };
            }

            DateTimeFormatInfo info = Culture.DateTimeFormat;
            info.Calendar = calendar;
            string[] MonthNames = info.MonthNames;
            Array.Resize<string>(ref MonthNames, 12);
            return MonthNames;
        }

        public static string[] GetMonthNames(System.Globalization.Calendar calendar)
        {
            return GetMonthNames(calendar, new System.Globalization.CultureInfo(CultureInfo.CurrentCulture.ToString(), false));
        }

        public static string[] GetCurrentUmAlquraDate()
        {
            string strDate = FromMiladiDateToUmAlQuraString(DateTime.Now);
            return GetDateParts(strDate);
        }

        public static string[] GetCurrentGregDate()
        {
            string strDate = FromMiladiDateToMiladiString(DateTime.Now);
            return GetDateParts(strDate);
        }

        /// <summary>
        /// return date' parts as string array
        /// </summary>
        /// <param name="d"></param>
        /// <returns>day, month, year</returns>
        public static string[] GetDateParts(string d)
        {
            return d.Split('/');
        }

        /// <summary>
        /// return array of 31 days
        /// </summary>
        /// <returns></returns>
        public static string[] GetDays()
        {
            string[] days = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", 
                                "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"};
            return days;
        }

        /// <summary>
        /// return array of 30 days for Hijri dates
        /// </summary>
        /// <returns></returns>
        public static string[] GetHijriDays()
        {
            string[] days = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", 
                                "21", "22", "23", "24", "25", "26", "27", "28", "29", "30"};
            return days;
        }

        /// <summary>
        /// return array of 12 months
        /// </summary>
        /// <returns></returns>
        public static string[] GetMonths()
        {
            string[] months = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            return months;
        }

        /// <summary>
        /// return array of min-max years
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static List<string> GetYears(int min, int max)
        {
            if (min > max)
                throw new Exception("The min parameter must be less than max parameter");

            List<string> years = new List<string>();
            for (int i = min; i <= max; i++)
            {
                years.Add(i.ToString());
            }

            return years;
        }

        public static DateTime StringToDate(string dateStr, System.Globalization.Calendar calendar)
        {
            string[] dateParts = dateStr.Split('/');
            int day = int.Parse(dateParts[0]);
            int month = int.Parse(dateParts[1]);
            int year = int.Parse(dateParts[2]);

            DateTime date = new DateTime(year, month, day, calendar);
            return date;
        }

        public static String GetGregoreanDate(string dateFormat)
        {
            dateFormat = String.IsNullOrEmpty(dateFormat) ? "dd/MM/yyyy" : dateFormat;
            GregorianCalendar egCal = new GregorianCalendar();

            CultureInfo egCulture = new CultureInfo("ar-SA");
            egCulture.DateTimeFormat.Calendar = egCal;

            return DateTime.Now.ToString(dateFormat, egCulture);
        }

        public static String GetGregoreanDate(string dateFormat, string cultureName)
        {
            dateFormat = String.IsNullOrEmpty(dateFormat) ? "dd/MM/yyyy" : dateFormat;
            GregorianCalendar egCal = new GregorianCalendar();

            CultureInfo egCulture = new CultureInfo(cultureName);
            egCulture.DateTimeFormat.Calendar = egCal;

            return DateTime.Now.ToString(dateFormat, egCulture);
        }

        public static String GetUmAlQuraDate(string dateFormat)
        {
            dateFormat = String.IsNullOrEmpty(dateFormat) ? "dd/MM/yyyy" : dateFormat;
            System.Globalization.UmAlQuraCalendar arCal = new System.Globalization.UmAlQuraCalendar();

            CultureInfo arCulture = new CultureInfo("ar-SA");
            arCulture.DateTimeFormat.Calendar = arCal;

            return DateTime.Now.ToString(dateFormat, arCulture);
        }

        /// <summary>
        /// Check if day, month, and year present correct date
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsDate(string day, string month, string year)
        {
            try
            {
                DateTime dt = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if day, month, and year present correct UnAlQura date
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsUmAlquraDate(string day, string month, string year)
        {
            try
            {
                DateTime dt = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), new System.Globalization.UmAlQuraCalendar());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if day, month, and year present correct Miladi date
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsMiladiDate(string day, string month, string year)
        {
            try
            {
                DateTime dt = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), new GregorianCalendar());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if day, month, and year present correct Greg date
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsGregDate(string day, string month, string year)
        {
            try
            {
                DateTime dt = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), new GregorianCalendar());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Replace Hijry month name with its english name
        /// </summary>
        /// <param name="date">
        /// String contains Arabic Hijry month name
        /// </param>
        /// <param name="month">
        /// index of month (start from 1)
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ReplaceHijryMonth(string date, int month)
        {
            var arabicMonthNames = GetMonthNames(new UmAlQuraCalendar(), new CultureInfo("ar-SA"));
            var englishMonthNames = GetMonthNames(new UmAlQuraCalendar(), new CultureInfo("en-US"));
            return date.Replace(arabicMonthNames[month - 1], englishMonthNames[month - 1]);
        }

        #endregion
    }
}