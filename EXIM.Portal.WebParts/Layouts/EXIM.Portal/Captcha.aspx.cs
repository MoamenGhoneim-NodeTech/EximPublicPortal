using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Exim.Portal.WebParts.Layouts
{
    public partial class Captcha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int width = Request["w"] != null ? Convert.ToInt32(Request["w"]) : 80;
            int height = Request["h"] != null ? Convert.ToInt32(Request["h"]) : 30;
            int font = Request["f"] != null ? Convert.ToInt32(Request["f"]) : 13;
            Bitmap img = new Bitmap(width, height);

            Graphics G = Graphics.FromImage(img);

            G.Clear(Color.WhiteSmoke);
            G.TextRenderingHint = TextRenderingHint.AntiAlias;

            //' Configure font to use for text
            Font objFont = new Font("Chiller", font, FontStyle.Italic);
            string randomStr = getRandomString();



            // Write out the text
            G.DrawString(randomStr, objFont, Brushes.DarkBlue, width / 4, height / 3);
            // Set the content type and return the image
            Response.ContentType = "image/GIF";
            img.Save(Response.OutputStream, ImageFormat.Gif);
            objFont.Dispose();
            G.Dispose();
            img.Dispose();
        }

        private string getRandomString()
        {
            Guid id = Guid.NewGuid();
            byte[] bytes = id.ToByteArray();

            string randomStr = "";

            foreach (var item in bytes)
            {
                randomStr += (item.ToString());
                if (randomStr.Length > 5)
                {
                    randomStr = randomStr.Substring(0, 5);
                    break;
                }
            }
            //This is to add the string to session cookie, to be compared later
            Session.Add("_Captchastring", randomStr);
            return String.Join(" ", randomStr.AsEnumerable());
        }
    }
}