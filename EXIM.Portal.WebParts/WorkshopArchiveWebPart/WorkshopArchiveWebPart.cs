using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebPartPages;

namespace EXIM.Portal.WebParts.WorkshopArchiveWebPart
{
    [ToolboxItemAttribute(false)]
    public class WorkshopArchiveWebPart : Microsoft.SharePoint.WebPartPages.WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/EXIM.Portal.WebParts/WorkshopArchiveWebPart/WorkshopArchiveWebPartUserControl.ascx";

        [WebBrowsable,
        Personalizable(),
        Category("Settings"),
        WebPartStorage(Storage.Shared),
        FriendlyName("Target List Title:"),
        Description("TargetlistTitle")]
        public string TargetlistTitle { get; set; }
        protected override void CreateChildControls()
        {
            this.ExportMode = WebPartExportMode.All;
            this.ChromeType = PartChromeType.None;
            WorkshopArchiveWebPartUserControl control = Page.LoadControl(_ascxPath) as WorkshopArchiveWebPartUserControl;
            
            control.WebPartRef = this;  // pass reference so ASCX can read properties

            Controls.Add(control);
        }
    }
}
