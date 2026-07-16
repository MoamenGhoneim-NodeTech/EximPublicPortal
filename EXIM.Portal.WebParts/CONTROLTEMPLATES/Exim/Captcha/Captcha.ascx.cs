using System;
using System.Web.UI.WebControls;
using EXIM.Common.Lib;
using EXIM.Common.Lib.Utils;

namespace Exim.Portal.WebParts
{
    public partial class Captcha : System.Web.UI.UserControl
    {
        public string ValidationGroup { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RequiredFieldValidator1.ValidationGroup = ValidationGroup;
                captchaCustomValidator.ValidationGroup = ValidationGroup;
            }
        }

        protected void captchaCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                string clientIp = ClientIpHelper.GetClientIp(Request);

                if (!CaptchaRateLimiter.AllowValidationAttempt(clientIp))
                {
                    args.IsValid = false;
                    captchaCustomValidator.ErrorMessage = Convert.ToString(GetLocalResourceObject("ExceedAttemptsMessage")); // "Too many attempts. Please wait a few minutes and try again.";
                    return;
                }

                args.IsValid = IsValid();
            }
            catch
            {
                args.IsValid = false;
            }
            finally
            {
                txtCaptcha.Text = string.Empty;
            }
        }

        public bool IsValid()
        {
            try
            {
                object storedValue = Session["_Captchastring"];
                object expiryValue = Session["_CaptchaExpiry"];

                // A captcha code is single-use: clear it as soon as we read it, whether the
                // attempt succeeds or fails, so replaying the same code never validates twice.
                Session.Remove("_Captchastring");
                Session.Remove("_CaptchaExpiry");

                if (storedValue == null || expiryValue == null)
                {
                    return false;
                }

                if (DateTime.UtcNow > (DateTime)expiryValue)
                {
                    return false; // expired - user waited too long, force a refresh
                }

                string entered = (txtCaptcha.Text ?? string.Empty).Trim();
                return string.Equals(storedValue.ToString(), entered, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
