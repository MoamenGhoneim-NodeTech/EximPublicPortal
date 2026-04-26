<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FinancialServiceControl.ascx.cs"
    Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.FinancialServices.FinancialServiceControl" %>
<asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">

<div class="exim-fin-services" dir="<%: IsArabic ? "rtl" : "ltr" %>">
    <div class="row">

        <%-- SIDEBAR --%>
        <div class="col-lg-3 col-md-4 exim-filters-sidebar">

            <div class="filter-group">
                <h5 class="filter-title"><%: IsArabic ? "الفئة" : "Beneficiary" %></h5>
                <asp:PlaceHolder ID="phCategoryFilters" runat="server" />
            </div>

            <div class="filter-group mt-4">
                <h5 class="filter-title"><%: IsArabic ? "نوع الخدمة" : "Service type" %></h5>
                <asp:PlaceHolder ID="phServiceTypeFilters" runat="server" />
            </div>

            <div class="filter-actions mt-3">
                <asp:Button ID="btnApply" runat="server" Text="Apply"
                    CssClass="exim-btn-link fw-bold"
                    CausesValidation="false" OnClick="btnApply_Click" />
                <span class="mx-2">|</span>
                <asp:Button ID="btnClear" runat="server" Text="Clear"
                    CssClass="exim-btn-link text-secondary"
                    CausesValidation="false" OnClick="btnClear_Click" />
            </div>

        </div>

        <%-- MAIN CONTENT --%>
        <div class="col-lg-9 col-md-8">

            <div class="exim-header d-flex justify-content-between align-items-center mb-3">
                <h2 class="exim-page-title mb-0">
                    <%: IsArabic ? "جميع الخدمات" : "All Services" %>
                </h2>
                <asp:HyperLink ID="hlDownload" runat="server"
                    CssClass="btn btn-outline-secondary btn-download"
                    Target="_blank" Visible="false">
                    <i class="fas fa-download <%: IsArabic ? "ms-1" : "me-1" %>"></i>
                    <%: IsArabic ? "تحميل دليل الخدمات" : "Download services guide" %>
                </asp:HyperLink>
            </div>

            <div class="exim-search-wrap mb-4">
                <div class="input-group">
                    <asp:TextBox ID="txtSearch" runat="server"
                        CssClass="form-control exim-search-input" TextMode="SingleLine" />
                    <asp:LinkButton ID="btnSearch" runat="server"
                        CssClass="btn btn-outline-secondary"
                        CausesValidation="false" OnClick="btnSearch_Click">
                        <i class="fas fa-search"></i>
                    </asp:LinkButton>
                </div>
            </div>

            <div class="services-items row g-3">
                <asp:PlaceHolder ID="phResults" runat="server" />
            </div>

            <asp:PlaceHolder ID="phNoResults" runat="server" Visible="false">
                <div class="ms-srch-result-noResults text-center py-4">
                    <asp:Literal ID="litNoResults" runat="server" />
                </div>
            </asp:PlaceHolder>

            <%-- ═══════════════════════════════════════════════════════
                 PAGING — rptPager declared here, always in the control
                 tree. Prev / numbered pages / Next are all rendered as
                 ItemTemplates so ASP.NET can route events correctly.
                 pnlPaging wraps the <ul> chrome and is shown/hidden
                 by RenderPaging() in code-behind.
            ═══════════════════════════════════════════════════════ --%>
            <asp:Panel ID="pnlPaging" runat="server" Visible="false"
                CssClass="page-pagination mt-4">
                <nav aria-label="<%: PagingNavLabel %>">
                    <ul class="pagination justify-content-center">
                        <asp:Repeater ID="rptPager" runat="server"
                            OnItemCommand="Pager_ItemCommand">
                            <ItemTemplate>
                                <li class='<%# (string)Eval("CssClass") %>'>
                                    <asp:LinkButton runat="server"
                                        Visible='<%# (bool)Eval("Enabled") %>'
                                        CssClass="page-link"
                                        CommandName="Page"
                                        CommandArgument='<%# Eval("PageIndex") %>'
                                        CausesValidation="false">
                                        <%# Eval("Label") %>
                                    </asp:LinkButton>
                                    <asp:Label runat="server"
                                        Visible='<%# !(bool)Eval("Enabled") %>'
                                        CssClass="page-link"
                                        Text='<%# Eval("Label") %>' />
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </nav>
                <div class="pages-total text-center mt-2"><%: TotalPagesLabel %></div>
            </asp:Panel>

        </div>
    </div>
</div>

<style type="text/css">
    .exim-check { display:flex; align-items:center; gap:8px; padding:3px 0; cursor:pointer; }
    .exim-check input[type="checkbox"] { flex-shrink:0; width:16px; height:16px; margin:0; cursor:pointer; }
    .exim-check span { line-height:1.3; }
    .exim-btn-link { border:none; background:none; color:inherit; padding:0;
                     text-decoration:none; cursor:pointer; display:inline; }
    .exim-btn-link:hover { text-decoration:underline; }
</style>

    </asp:Panel>
