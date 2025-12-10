using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using EXIM.Common.Lib.Utils;
using Microsoft.SharePoint.WebPartPages;

namespace EXIM.Portal.WebParts
{
	[ToolboxItemAttribute(false)]
	public class ControlLoaderWebPart : Microsoft.SharePoint.WebPartPages.WebPart
	{

		#region Properties

		[WebBrowsable,
		Personalizable(),
		Category("Settings"),
		WebPartStorage(Storage.Shared),
		FriendlyName("User Control Path:"),
		Description("Path of the user control; Enter the path after ~/_controltemplates/15/")]
		public string UserControlPath { get; set; }

		[WebBrowsable,
		Personalizable(),
		Category("Settings"),
		WebPartStorage(Storage.Shared),
		FriendlyName("User Control Properties:"),
		Description("The user control properties; ex: PropName#PropValue")]
		public string UserControlProperties { get; set; }

		[WebBrowsable,
		Personalizable(),
		Category("Settings"),
		WebPartStorage(Storage.Shared),
		FriendlyName("Controls Path prefix:"),
		Description("default value is ~/_controltemplates/15/")]
		public string ControlPathPrefix { get; set; }

		#endregion

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			try
			{
				this.ExportMode = WebPartExportMode.All;
				this.ChromeType = PartChromeType.None;
                WebPartManager wp = WebPartManager.GetCurrentWebPartManager(Page);
                if (wp.DisplayMode == WebPartManager.BrowseDisplayMode)
                {
                    if (!string.IsNullOrEmpty(UserControlPath))
                    {
                        if (UserControlPath[0] == '/')
                            UserControlPath = UserControlPath.Remove(0, 1);

                        if (string.IsNullOrEmpty(ControlPathPrefix))
                            ControlPathPrefix = "~/_controltemplates/15/";

                        string controlPath = string.Format("{0}{1}", ControlPathPrefix, UserControlPath);

                        Control control = Page.LoadControl(controlPath) as UserControl;
                        if (control != null)
                        {
                            setProperties(control);
                            this.Controls.Add(control);
                        }
                    }
                    else
                    {
                        Label lbl = new Label();
                        lbl.ID = "lbl_" + this.ClientID;
                        lbl.Text = "Must Added Control Path";
                        this.Controls.Add(lbl);
                    }
                }
			}
			catch (Exception ex)
			{
				LogService.LogException(ex);
			}
		}

		private void setProperties(Control ctrl)
		{
			// check if there is properties passed to the user control from the user
			if (!string.IsNullOrEmpty(this.UserControlProperties))
			{
				string msg = string.Empty;
				// get the user control properties
				List<PropertyInfo> originalPropsList = ctrl.GetType().GetProperties().ToList<PropertyInfo>();

				// convert the props to dictionary
				Dictionary<string, object> originalPropsDic = new Dictionary<string, object>();
				foreach (PropertyInfo p in originalPropsList)
					originalPropsDic.Add(p.Name, p);


				// geth the properties from the user
				List<string> userControlProperties = this.UserControlProperties.Split(";".ToCharArray()).ToList<string>();

				// split each property and its vlaue based on '#' char
				string[] keyvalue;
				Dictionary<string, object> passedPropsDic = new Dictionary<string, object>();
				foreach (string item in userControlProperties)
				{
					keyvalue = item.Split("#".ToCharArray());
					if (keyvalue.Length == 2)
					{
						// we will check for dublicated property name
						if (!passedPropsDic.ContainsKey(keyvalue[0]))
							passedPropsDic.Add(keyvalue[0], keyvalue[1]);
					}
					else
						// the property does not assigned with value,
						// then we will execulde it and put it in the msg
						msg += string.Format("There is no value for: <b>{0}</b><br />", item);
				}


				// matching between the user controls properties and the 
				// properties passed from the user
				foreach (string passedProperty in passedPropsDic.Keys)
				{
					if (!originalPropsDic.ContainsKey(passedProperty))
						// the property was not found the original list
						msg += string.Format("The property <b>{0}</b> was not found.<br />", passedProperty);
				}

				// if the msg variable is empty, then we passed all
				// the checks and there is no error otherwise there is an error
				if (!string.IsNullOrEmpty(msg))
				{
					// there is an error
					Label lbl = new Label();
					lbl.ID = "lbl_" + this.ClientID;
					lbl.Text = msg;
					this.Controls.Add(lbl);

					return;
				}


				// set the properties values to the user control
				PropertyInfo originalProperty;
				foreach (string passedProperty in passedPropsDic.Keys)
				{
					originalProperty = originalPropsDic[passedProperty] as PropertyInfo;
					if (originalProperty != null)
					{
						originalProperty.SetValue(ctrl, Convert.ChangeType(passedPropsDic[passedProperty], originalProperty.PropertyType), null);
					}
				}

			}
		}
	}
}
