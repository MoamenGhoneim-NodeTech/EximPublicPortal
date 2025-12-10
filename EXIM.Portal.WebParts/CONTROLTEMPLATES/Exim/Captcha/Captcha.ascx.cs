using System;
using System.Web.UI.WebControls;

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
                args.IsValid = IsValid();
                txtCaptcha.Text = string.Empty;
            }
            catch
            {
                args.IsValid = false;
                txtCaptcha.Text = string.Empty;
            }
        }

        public bool IsValid()
        {
            bool result = false;
            try
            {               
                result = Session["_Captchastring"].ToString() == txtCaptcha.Text.Trim();
                Session.Remove("_Captchastring");
                txtCaptcha.Text = string.Empty;
                return result;
            }
            catch (Exception)
            {
                return result;
            }
           
        }
    }
}