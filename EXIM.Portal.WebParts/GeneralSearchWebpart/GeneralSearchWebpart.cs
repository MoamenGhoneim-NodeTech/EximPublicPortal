using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace EXIM.Portal.WebParts.GeneralSearchWebpart
{
    [ToolboxItemAttribute(false)]
    public class GeneralSearchWebpart : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/EXIM.Portal.WebParts/GeneralSearchWebpart/GeneralSearchWebpartUserControl.ascx";

        protected override void CreateChildControls()
        {
            try
            {
                this.ExportMode = WebPartExportMode.All;
                this.ChromeType = PartChromeType.None;
                
                Control control = Page.LoadControl(_ascxPath);
                Controls.Add(control);
               

            }
            catch (Exception ex)
            {
                Controls.Add(new LiteralControl(
                    "<div style='color:red;padding:10px;'>GeneralSearchWebPart: " +
                    ex.Message + "</div>"));
            }
        }
    }
}
