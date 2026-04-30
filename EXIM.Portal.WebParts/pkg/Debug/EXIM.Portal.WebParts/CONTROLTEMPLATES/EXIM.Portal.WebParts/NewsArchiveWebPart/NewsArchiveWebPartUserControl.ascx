<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsArchiveWebPartUserControl.ascx.cs" Inherits="EXIM.Portal.WebParts.NewsArchiveWebPart.NewsArchiveWebPartUserControl" %>

<%-- Items grid --%>
<div class="row blog-items g-4">
    <asp:Repeater ID="rptNewsItems" runat="server">
        <ItemTemplate>
            <div class="col-md-4" data-aos="fade-up" data-aos-delay="100">
                <div class="blog-item">

                    <div class="img">
                        <a href='<%# Eval("FileRef") %>'>
                            <img src='<%# Eval("ImgPath") %>' alt='<%# Eval("Title") %>' />
                        </a>
                    </div>

                    <div class="blog-category blue-category">
                        <a href='<%# Eval("FileRef") %>'><%# Eval("ButtonText") %></a>
                    </div>

                    <h3><a href='<%# Eval("FileRef") %>'><%# Eval("Title") %></a></h3>

                    <%-- ArticleDate is only rendered when the field has a value --%>
                    <asp:PlaceHolder runat="server"
                        Visible='<%# !string.IsNullOrEmpty(Eval("ArticleDate") as string) %>'>
                        <div class="blog-date">
                            <span><i class="ic-date"></i></span>
                            <%# Eval("ArticleDate") %>
                        </div>
                    </asp:PlaceHolder>

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
