using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Web;
using EXIM.Common.Lib.Utils;

namespace Exim.Portal.WebParts.Layouts
{
    public partial class Captcha : System.Web.UI.Page
    {
        // Numeric only, per requirement.
        private const string CharSet = "0123456789";

        // Fonts assumed present on the WFE - confirm these are actually installed on your
        // SharePoint servers (the previous "Chiller" font is a decorative font that's often
        // missing on Windows Server, which silently degrades to a fallback font). Trim this
        // list to whatever you've verified is installed.
        private static readonly string[] FontFamilies = { "Arial", "Times New Roman", "Trebuchet MS", "Georgia" };

        private static readonly Random Rng = new Random();
        private static readonly object RngLock = new object();

        private const int CaptchaLength = 6;
        private static readonly TimeSpan CaptchaLifetime = TimeSpan.FromMinutes(2);

        protected void Page_Load(object sender, EventArgs e)
        {
            string clientIp = ClientIpHelper.GetClientIp(Request);

            if (!CaptchaRateLimiter.AllowGeneration(clientIp))
            {
                // Return a real image instead of an empty 429 body - an <img> tag with no/failed
                // body just renders as a broken image icon, which looks like the feature is
                // broken rather than "you're refreshing too fast." We still don't set a session
                // captcha string here, so any submit will correctly fail validation until a real
                // image is served.
                int rlWidth = Clamp(Request["w"] != null ? Convert.ToInt32(Request["w"]) : 200, 100, 400);
                int rlHeight = Clamp(Request["h"] != null ? Convert.ToInt32(Request["h"]) : 50, 30, 120);

                Response.StatusCode = 200;
                Response.ContentType = "image/png";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Headers["Retry-After"] = "10";

                using (Bitmap waitImg = RenderThrottledImage(rlWidth, rlHeight))
                {
                    waitImg.Save(Response.OutputStream, ImageFormat.Png);
                }
                Response.End();
                return;
            }

            int width = Clamp(Request["w"] != null ? Convert.ToInt32(Request["w"]) : 200, 100, 400);
            int height = Clamp(Request["h"] != null ? Convert.ToInt32(Request["h"]) : 50, 30, 120);
            int fontSize = Clamp(Request["f"] != null ? Convert.ToInt32(Request["f"]) : 20, 12, 32);

            string captchaText = GenerateRandomText(CaptchaLength);

            Session["_Captchastring"] = captchaText;
            Session["_CaptchaExpiry"] = DateTime.UtcNow.Add(CaptchaLifetime);

            using (Bitmap img = RenderCaptchaImage(captchaText, width, height, fontSize))
            {
                Response.ContentType = "image/png";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetNoStore();
                img.Save(Response.OutputStream, ImageFormat.Png);
            }
        }

        private static int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        /// <summary>
        /// Small placeholder shown in place of the real captcha when generation is rate-limited.
        /// Visually distinct (amber) so it's obvious to anyone debugging that this is the
        /// throttled state, not a broken captcha.
        /// </summary>
        private Bitmap RenderThrottledImage(int width, int height)
        {
            var img = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;

                using (var bgBrush = new SolidBrush(Color.FromArgb(255, 244, 214)))
                {
                    g.FillRectangle(bgBrush, 0, 0, width, height);
                }

                using (var borderPen = new Pen(Color.FromArgb(210, 160, 60), 2f))
                {
                    g.DrawRectangle(borderPen, 1, 1, width - 3, height - 3);
                }

                string message = "\u062A\u0645 \u062A\u062C\u0627\u0648\u0632 \u0639\u062F\u062F \u0627\u0644\u0645\u062D\u0627\u0648\u0644\u0627\u062A، \u062D\u0627\u0648\u0644 \u0644\u0627\u062D\u0642\u0627\u064B"; // "تم تجاوز عدد المحاولات، يرجى إعادة المحاولة بعد دقيقة واحدة." (Too many attempts, try again after 1 minute.)

                using (var font = new Font("Arial", Math.Max(9, height / 4), FontStyle.Bold))
                using (var textBrush = new SolidBrush(Color.FromArgb(150, 100, 20)))
                using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.DrawString(message, font, textBrush, new RectangleF(2, 2, width - 4, height - 4), format);
                }
            }
            return img;
        }

        private string GenerateRandomText(int length)
        {
            var buffer = new char[length];
            lock (RngLock)
            {
                for (int i = 0; i < length; i++)
                {
                    buffer[i] = CharSet[Rng.Next(CharSet.Length)];
                }
            }
            return new string(buffer);
        }

        private Bitmap RenderCaptchaImage(string text, int width, int height, int baseFontSize)
        {
            var img = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;

                // Background: a clearly-contrasting gradient (not near-white) so the box reads
                // as a distinct element against a typical white/light page background.
                using (var bgBrush = new LinearGradientBrush(
                    new Rectangle(0, 0, width, height),
                    Color.FromArgb(210, 224, 238), Color.FromArgb(180, 198, 220), LinearGradientMode.ForwardDiagonal))
                {
                    g.FillRectangle(bgBrush, 0, 0, width, height);
                }

                lock (RngLock)
                {
                    DrawBackgroundNoise(g, width, height);
                    DrawDistortedText(g, text, width, height, baseFontSize);
                    DrawForegroundNoise(g, width, height);
                }

                // Solid border so the image edge is unmistakable against the page background,
                // regardless of what's directly behind/around it.
                using (var borderPen = new Pen(Color.FromArgb(90, 110, 140), 2f))
                {
                    g.DrawRectangle(borderPen, 1, 1, width - 3, height - 3);
                }
            }

            return img;
        }

        private void DrawBackgroundNoise(Graphics g, int width, int height)
        {
            // Random speckle dots
            int dotCount = (width * height) / 40;
            for (int i = 0; i < dotCount; i++)
            {
                int x = Rng.Next(width);
                int y = Rng.Next(height);
                using (var dotBrush = new SolidBrush(Color.FromArgb(
                    Rng.Next(120, 200), Rng.Next(120, 200), Rng.Next(120, 200))))
                {
                    g.FillRectangle(dotBrush, x, y, 1, 1);
                }
            }

            // Distracting arcs behind the text
            for (int i = 0; i < 4; i++)
            {
                using (var pen = new Pen(Color.FromArgb(
                    90, Rng.Next(100, 180), Rng.Next(100, 180), Rng.Next(100, 180)), 1.5f))
                {
                    var rect = new Rectangle(
                        Rng.Next(-width / 4, width / 2),
                        Rng.Next(-height, height / 2),
                        Rng.Next(width / 2, width * 2),
                        Rng.Next(height, height * 3));
                    g.DrawArc(pen, rect, Rng.Next(0, 360), Rng.Next(30, 180));
                }
            }
        }

        private void DrawDistortedText(Graphics g, string text, int width, int height, int baseFontSize)
        {
            float x = width * 0.06f;
            float usableWidth = width * 0.88f;
            float step = usableWidth / text.Length;

            for (int i = 0; i < text.Length; i++)
            {
                string fontFamily = FontFamilies[Rng.Next(FontFamilies.Length)];
                float sizeJitter = baseFontSize + Rng.Next(-2, 3);

                using (var font = new Font(fontFamily, sizeJitter, FontStyle.Bold))
                {
                    SizeF charSize = g.MeasureString(text[i].ToString(), font);
                    float charX = x + i * step;
                    float charY = ((height - charSize.Height) / 2f) + Rng.Next(-4, 5);

                    GraphicsState state = g.Save();
                    g.TranslateTransform(charX + charSize.Width / 2f, charY + charSize.Height / 2f);
                    g.RotateTransform(Rng.Next(-25, 26));
                    g.TranslateTransform(-(charSize.Width / 2f), -(charSize.Height / 2f));

                    using (var brush = new SolidBrush(Color.FromArgb(
                        255, Rng.Next(20, 80), Rng.Next(20, 80), Rng.Next(90, 160))))
                    {
                        g.DrawString(text[i].ToString(), font, brush, 0, 0);
                    }

                    g.Restore(state);
                }
            }
        }

        private void DrawForegroundNoise(Graphics g, int width, int height)
        {
            // A couple of lines crossing the text for extra obfuscation
            for (int i = 0; i < 2; i++)
            {
                using (var pen = new Pen(Color.FromArgb(70, Color.SlateGray), 1f))
                {
                    g.DrawLine(pen,
                        Rng.Next(0, width / 3), Rng.Next(0, height),
                        Rng.Next(width * 2 / 3, width), Rng.Next(0, height));
                }
            }
        }
    }
}
