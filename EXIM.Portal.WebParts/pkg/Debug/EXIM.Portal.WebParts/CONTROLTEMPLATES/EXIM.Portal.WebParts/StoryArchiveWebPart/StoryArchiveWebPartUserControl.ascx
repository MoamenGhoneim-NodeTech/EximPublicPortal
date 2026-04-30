<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StoryArchiveWebPartUserControl.ascx.cs" Inherits="EXIM.Portal.WebParts.StoryArchiveWebPart.StoryArchiveWebPartUserControl" %>

<%-- Items grid --%>
<div class="row study-items-item g-4">

    <asp:Repeater ID="rptItems" runat="server">
        <ItemTemplate>

            <div class="story-item pagingItem">

                <div class="story-item--img">
                    <a href='<%# Eval("ItemUrl") %>' class="story-item--img-container">
                        <img src='<%# Eval("ImgPath") %>' alt='<%# Eval("Title") %>' />
                    </a>
                </div>

                <div class="story-logo">
                    <a href='<%# Eval("ItemUrl") %>'>
                        <img src='<%# Eval("LogoPath") %>' alt='<%# Eval("Title") %>' />
                    </a>
                </div>

                <div class="story-item--text">

                    <div class="story-text">
                        <h3><%# Eval("Title") %></h3>
                        <p><%# Eval("Description") %></p>
                    </div>

                    <div class="story-user">
                        <h2><%# Eval("PersonName") %></h2>
                        <p><%# Eval("PersonWord") %></p>
                    </div>

                    <div class="story-action">
                        <a href='<%# Eval("ItemUrl") %>' class="btn btn-primary">
                            <%# Eval("ButtonText") %>
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
