using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
namespace EXIM.Portal.WebParts.GeneralSearchWebpart
{
    public partial class GeneralSearchWebpartUserControl : UserControl
    {
        public bool IsArabic { get; private set; }
        public string PageDir { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
     
            IsArabic = (SPContext.Current?.Web?.Language == 1025);
            PageDir = IsArabic ? "rtl" : "ltr";
        }
    }
}
