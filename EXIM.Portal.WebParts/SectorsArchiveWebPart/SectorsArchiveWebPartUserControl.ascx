<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SectorsArchiveWebPartUserControl.ascx.cs" Inherits="EXIM.Portal.WebParts.SectorsArchiveWebPart.SectorsArchiveWebPartUserControl" %>
<section class="blogs-section">
    <div class="container">
        <div class="row blog-items g-4">

            <asp:Repeater ID="rptSectorItems" runat="server">
                <ItemTemplate>
                    <div class="col-md-4 aos-init" data-aos="fade-up" data-aos-delay="100">
                        <div class="blog-item">

                            <div class="img">
                                <a href='<%# Eval("ItemLink") %>'
                                   <%# (bool)Eval("OpenInNewTab") ? "target=\"_blank\"" : "" %>>
                                    <img src='<%# Eval("ImgPath") %>' alt='<%# Eval("Title") %>' />
                                </a>
                            </div>

                            <h3 class="text-center">
                                <a href='<%# Eval("ItemLink") %>'><%# Eval("Title") %></a>
                            </h3>

                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </div>
    </div>
</section>
