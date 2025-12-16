<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteMapWPUserControl.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.SiteMap.SiteMapWPUserControl" %>
<!-- Error display at top -->
<asp:Literal ID="litError" runat="server" Visible="false"></asp:Literal>

<div class="MainContent">
    <section class="single-page-details" style="display:none;">
        <div class="container">
            <div class="single-page-header">
                <div class="single-page-header-right">
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item">
                                <a href="index.html">
                                    <%= System.Threading.Thread.CurrentThread.CurrentUICulture.LCID == 1025 ? "الرئيسية" : "Home" %>
                                </a>
                            </li>
                            <li class="breadcrumb-item active" aria-current="page">
                                <%= System.Threading.Thread.CurrentThread.CurrentUICulture.LCID == 1025 ? "خريطة الموقع" : "Site Map" %>
                            </li>
                        </ol>
                    </nav>
                    <h1><%= System.Threading.Thread.CurrentThread.CurrentUICulture.LCID == 1025 ? "خريطة الموقع" : "Site Map" %></h1>
                    <div class="blog-date">
                        <span><i class="ic-date"></i></span>
                        <%= System.Threading.Thread.CurrentThread.CurrentUICulture.LCID == 1025 ? "نشرت في" : "Published on" %> 
                        <%= DateTime.Now.ToString("dd-MM-yyyy") %>
                    </div>
                </div>
                <div class="single-page-header-left">
                    <div class="share-panel">
                        <h4><%= System.Threading.Thread.CurrentThread.CurrentUICulture.LCID == 1025 ? "مشاركة الصفحة" : "Share Page" %></h4>
                        <div class="social-links">
                            <a href="#" onclick="return false;"><i class="ic-whatsapp"></i></a>
                            <a href="#" onclick="return false;"><i class="ic-facebook"></i></a>
                            <a href="#" onclick="return false;"><i class="ic-linkedin"></i></a>
                            <a href="#" onclick="return false;"><i class="ic-x-twitter"></i></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <section class="sitemap-section">
        <div class="container">
            <div class="sitemap-items">
                <asp:Repeater ID="rptMain" runat="server" OnItemDataBound="rptMain_ItemDataBound">
                    <ItemTemplate>
                        <div class="col-md-12">
                            <div class="sitemap-tree">
                                <!-- Main Category Title -->
                                <h2><%# Eval("Name") %></h2>
                                
                                <!-- Placeholder for categories WITH subcategories -->
                                <asp:PlaceHolder ID="phWithSub" runat="server" Visible="false">
                                    <div class="row">
                                        <asp:Repeater ID="rptSub" runat="server" OnItemDataBound="rptSub_ItemDataBound">
                                            <ItemTemplate>
                                                <div class="col-md-4">
                                                    <div class="sitemap-tree">
                                                        <h3><%# Eval("Name") %></h3>
                                                        <ul>
                                                            <asp:Repeater ID="rptLinks" runat="server">
                                                                <ItemTemplate>
                                                                    <li>
                                                                        <a href="<%# Eval("Url") %>"><%# Eval("Title") %></a>
                                                                    </li>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </asp:PlaceHolder>
                                
                                <!-- Placeholder for categories WITHOUT subcategories (direct links) -->
                                <asp:PlaceHolder ID="phWithoutSub" runat="server" Visible="false">
                                    <ul>
                                        <asp:Repeater ID="rptDirectLinks" runat="server">
                                            <ItemTemplate>
                                                <li>
                                                    <a href="<%# Eval("Url") %>"><%# Eval("Title") %></a>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
</div>
