<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="YearlyReportsControl.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.YearlyReports.YearlyReportsControl" %>


<%-- Items grid --%>
<div class="row study-items-item g-4">

    <asp:Repeater ID="rptItems" runat="server">
        <ItemTemplate>

            <div class="col-md-4 pagingItem" data-aos="fade-up" data-aos-delay="100">
                <div class="study-item gap-4">

                    <div class="study-icon">
                        <span><i class="ic-file"></i></span>
                    </div>

                    <h3>
                        <a href='<%# Eval("ItemUrl") %>'><%# Eval("Title") %></a>
                    </h3>

                    <div class="study-action gap-3">

                        <%-- download attribute triggers browser save-as dialog --%>
                        <a href='<%# Eval("ItemUrl") %>'
                           download
                           class="btn btn-secondary">
                            <%# Eval("DownloadText") %>
                        </a>

                        <a href='<%# Eval("ReportDetailsUrl") %>'
                           target="_blank"
                           class="btn btn-outline-light">
                            <%# Eval("ViewText") %>
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
