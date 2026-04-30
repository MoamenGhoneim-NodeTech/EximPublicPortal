<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticlesArchiveWebPartUserControl.ascx.cs" Inherits="EXIM.Portal.WebParts.ArticlesArchiveWebPart.ArticlesArchiveWebPartUserControl" %>

<%-- Items grid --%>
<div class="row study-items-item g-4">
    <asp:Repeater ID="rptStudyItems" runat="server">
        <ItemTemplate>
            <div class="col-md-4" data-aos="fade-up" data-aos-delay="100">
                <div class="study-item">
                    <h3>
                        <a href='<%# Eval("FileRef") %>'><%# Eval("Title") %></a>
                    </h3>
                    <p><%# Eval("Comments") %></p>
                    <div class="study-action">
                        <a href='<%# Eval("FileRef") %>' class="btn btn-primary">
                            <%# Eval("ButtonText") %>
                            <i class="ic-more-link"></i>
                        </a>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

<%-- Pagination --%>
<div class="pagination-wrapper">
    <asp:Label   ID="lblPrevText"   runat="server" CssClass="prev-text" Visible="false" />
    <asp:Literal ID="litPagination" runat="server" />
    <asp:Label   ID="lblNextText"   runat="server" CssClass="next-text" Visible="false" />
</div>
