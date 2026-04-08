<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LandingPageControl.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.LandingPage.LandingPageControl" %>


<%-- Items grid --%>
<div class="row study-items-item g-4">
    <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound">
        <ItemTemplate>
            <div class="col-md-4" data-aos="fade-up" data-aos-delay="100">
                <div class="blog-item">
                    <div class="img">
                        <a href='<%# Eval("ItemLink") %>'>
                            <img src='<%# Eval("ImgPath") %>' alt='<%# Eval("Title") %>' />
                        </a>
                    </div>
                    <h3><a href='<%# Eval("ItemLink") %>'><%# Eval("Title") %></a></h3>
                    <p><%# Eval("Description") %></p>
                    <div class="blog-item-action">
                        <a href='<%# Eval("ItemLink") %>' class="btn btn-primary">
                            <%# Eval("ButtonText") %>
                        </a>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

<%-- Pagination — rendered entirely by BuildPaginationHtml in the code-behind --%>
<div class="pagination-wrapper">
    <asp:Literal ID="litPagination" runat="server" />
</div>
