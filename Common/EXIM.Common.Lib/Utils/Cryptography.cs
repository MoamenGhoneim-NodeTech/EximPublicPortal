using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EXIM.Common.Lib.Utils
{
    public class Cryptography
    {
        private static string password = "2v!6zT_Y&{rm";
        private static byte[] GetKey(string password)
        {
            string pwd = null;

            if (Encoding.UTF8.GetByteCount(password) < 24)
            {
                pwd = password.PadRight(24, ' ');
            }
            else
            {
                pwd = password.Substring(0, 24);
            }
            return Encoding.UTF8.GetBytes(pwd);
        }

        public static string Encrypt(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                    return null;

                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                DES.Mode = CipherMode.ECB;
                DES.Key = GetKey(password);

                DES.Padding = PaddingMode.PKCS7;
                ICryptoTransform DESEncrypt = DES.CreateEncryptor();
                Byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(data);

                return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
                return null;
            }
        }

        public static string Decrypt(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                    return null;

                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                DES.Mode = CipherMode.ECB;
                DES.Key = GetKey(password);

                DES.Padding = PaddingMode.PKCS7;
                ICryptoTransform DESEncrypt = DES.CreateDecryptor();
                Byte[] Buffer = Convert.FromBase64String(data.Replace(" ", "+"));

                return Encoding.UTF8.GetString(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
                return null;
            }
        }
    }
}
