using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebPartPages;

namespace EXIM.Portal.WebParts.RelatedSuccessStoryWebPart
{
    [ToolboxItemAttribute(false)]
    public class RelatedSuccessStoryWebPart : Microsoft.SharePoint.WebPartPages.WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/EXIM.Portal.WebParts/RelatedSuccessStoryWebPart/RelatedSuccessStoryWebPartUserControl.ascx";
        [WebBrowsable,
        Personalizable(),
        Category("Settings"),
        WebPartStorage(Storage.Shared),
        FriendlyName("Target List URL:"),
        Description("Target list Relative URL")]
        public string TargetlistURL { get; set; }
        protected override void CreateChildControls()
        {
            this.ExportMode = WebPartExportMode.All;
            this.ChromeType = PartChromeType.None;
            RelatedSuccessStoryWebPartUserControl control = Page.LoadControl(_ascxPath) as RelatedSuccessStoryWebPartUserControl;
            control.WebPartRef = this;  // pass reference so ASCX can read properties

            Controls.Add(control);
        }


    }
}

