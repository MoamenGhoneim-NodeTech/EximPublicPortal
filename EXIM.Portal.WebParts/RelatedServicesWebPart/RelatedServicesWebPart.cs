using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebPartPages;
namespace EXIM.Portal.WebParts.RelatedServicesWebPart
{
    [ToolboxItemAttribute(false)]
    public class RelatedServicesWebPart : Microsoft.SharePoint.WebPartPages.WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/EXIM.Portal.WebParts/RelatedServicesWebPart/RelatedServicesWebPartUserControl.ascx";
        [WebBrowsable,
        Personalizable(),
        Category("Settings"),
        WebPartStorage(Storage.Shared),
        FriendlyName("Target List URL:"),
        Description("Target list Relative URL")]
        public string TargetlistURL { get; set; }

        [WebBrowsable,
        Personalizable(),
        Category("Settings"),
        WebPartStorage(Storage.Shared),
        FriendlyName("Financial Service Type:"),
        Description("Financial Service Type, set value to \"NoFilter\" to ignore this filter,Will use the current service type if empty.")]
        public string FinancialServiceType { get; set; }
        protected override void CreateChildControls()
        {
            this.ExportMode = WebPartExportMode.All;
            this.ChromeType = PartChromeType.None;

            RelatedServicesWebPartUserControl control = Page.LoadControl(_ascxPath) as RelatedServicesWebPartUserControl;
            control.WebPartRef = this;  // pass reference so ASCX can read properties

            Controls.Add(control);

        }
    }
}
