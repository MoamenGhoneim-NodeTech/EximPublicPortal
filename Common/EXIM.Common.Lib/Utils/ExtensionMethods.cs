using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.SessionState;
using System.Xml.Serialization;

namespace EXIM.Common.Lib.Utils
{
    public static class ExtensionMethods
    {

        /// <summary>
        /// Generate XML format from any object
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// XML result as string
        /// </returns>
        public static T FromXML<T>(this string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            var serializer = new XmlSerializer(typeof(T));
            T result;

            using (TextReader reader = new StringReader(xml))
            {
                result = (T)serializer.Deserialize(reader);
            }

            return result;
        }
        /// <summary>
        /// The to guid.
        /// </summary>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/>.
        /// </returns>
        public static Guid ToGuid(this string str)
        {
            try
            {
                return new Guid(str);
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// The to guid.
        /// </summary>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/>.
        /// </returns>
        public static bool IsGuid(this string str)
        {
            try
            {
                var guidOutput = Guid.Empty;
                bool isValid = Guid.TryParse(str, out guidOutput);
                return isValid;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The to guid.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/>.
        /// </returns>
        public static Guid ToGuid(this object obj)
        {
            try
            {
                return new Guid(obj.ToString());
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// The to nullable guid.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="Guid?"/>.
        /// </returns>
        public static Guid? ToNullableGuid(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return null;
            }

            return obj.ToGuid();
        }

        /// <summary>
        /// The to short.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="short"/>.
        /// </returns>
        public static short ToShort(this object obj)
        {
            return Convert.ToInt16(obj);
        }

        /// <summary>
        /// The to nullable short.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="short?"/>.
        /// </returns>
        public static short? ToNullableShort(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt16(obj);
        }

        /// <summary>
        /// The to nullable double.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="double?"/>.
        /// </returns>
        public static double? ToNullableDouble(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDouble(obj);
        }

        /// <summary>
        /// The to nullable int.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="int?"/>.
        /// </returns>
        public static int? ToNullableInt(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// The to short.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="short"/>.
        /// </returns>
        public static short ToShort(this string obj, short defaultValue)
        {
            short result;
            if (!string.IsNullOrEmpty(obj) && short.TryParse(obj, out result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// The to int.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ToInt(this object obj)
        {
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// The to int.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ToInt(this object obj, int defaultValue)
        {
            var s = obj == null ? string.Empty : obj.ToString();

            int result;
            if (!string.IsNullOrEmpty(s) && int.TryParse(s, out result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// The to double.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        public static double ToDouble(this string obj, double defaultValue)
        {
            double result;
            if (!string.IsNullOrEmpty(obj) && double.TryParse(obj, out result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// The to bool.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ToBool(this object obj)
        {
            return Convert.ToBoolean(obj);
        }

        /// <summary>
        /// The to date time.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime ToDateTime(this object obj)
        {
            return Convert.ToDateTime(obj);
        }

        /// <summary>
        /// The to nullable date time.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime?"/>.
        /// </returns>
        public static DateTime? ToNullableDateTime(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return null;
            }

            return obj.ToDateTime();
        }

        /// <summary>
        /// The flatten.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="childrenSelector">
        /// The children selector.
        /// </param>
        /// <param name="condition">
        /// The condition.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Flatten<T>(
            this IEnumerable<T> source,
            Func<T, IEnumerable<T>> childrenSelector,
            Predicate<T> condition)
        {
            // return default if no items
            if (source == null || !source.Any())
            {
                return default(T);
            }

            // return result if found and stop traversing hierarchy
            var attempt = source.FirstOrDefault(t => condition(t));
            if (!Equals(attempt, default(T)))
            {
                return attempt;
            }

            // recursively call this function on lower levels of the
            // hierarchy until a match is found or the hierarchy is exhausted
            return source.SelectMany(childrenSelector).Flatten(childrenSelector, condition);
        }

        /// <summary>
        /// Remove a data column from a data table unless this field not in skipped-deleting fields
        /// </summary>
        /// <param name="dataTable">the data table</param>
        /// <param name="columnName">the column name wanted to be deleted</param>
        /// <param name="skipFields">array of skipped column's names</param>
        public static void RemoveColumn(this DataTable dataTable, string columnName, string[] skipFields = default)
        {
            if (dataTable.Columns.Contains(columnName) && (skipFields == null || (skipFields != null && skipFields.Length > 0 && !skipFields.Contains(columnName))))
                dataTable.Columns.Remove(columnName);
        }

        /// <summary>
        /// Remove a data column from a data table unless this field not in skipped-deleting fields
        /// </summary>
        /// <param name="dataTable">the data table</param>
        /// <param name="fieldsToRemove">array of column's names to remove</param>
        public static void RemoveColumns(this DataTable dataTable, string[] fieldsToRemove)
        {
            if (dataTable == null) return;

            foreach (var field in fieldsToRemove)
            {
                if (dataTable.Columns.Contains(field))
                    dataTable.Columns.Remove(field);

            }
        }

        //setting session
        public static void AddObject(this HttpSessionState session, string key, object value)
        {
            session[key] = JsonConvert.SerializeObject(value);
        }

        //getting session
        public static T GetObject<T>(this HttpSessionState session, string key)
        {
            try
            {
                string value = session[key]?.ToString() ?? null;
                return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}