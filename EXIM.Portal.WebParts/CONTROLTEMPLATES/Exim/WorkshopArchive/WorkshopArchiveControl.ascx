<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkshopArchiveControl.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.WorkshopArchive.WorkshopArchiveControl" %>
<%-- Items grid --%>
<div class="row study-items-item g-4">

    <asp:Repeater ID="rptItems" runat="server">
        <ItemTemplate>

            <div class="col-md-4 pagingItem" data-aos="fade-up" data-aos-delay="100">
                <div class="blog-item">

                    <div class="img">
                        <a href='<%# Eval("EventUrl") %>'>
                            <img src='<%# Eval("ImgPath") %>' alt='<%# Eval("Title") %>' />
                        </a>
                    </div>

                    <div class="blog-category blue-category justify-content-between">
                        <a href='<%# Eval("EventUrl") %>'>
                            <%# Eval("EventType") %>
                        </a>
                        <span class='event-status <%# Eval("StatusClass") %>'>
                            <%# Eval("StatusText") %>
                        </span>
                    </div>

                    <h3>
                        <a href='<%# Eval("EventUrl") %>'><%# Eval("Title") %></a>
                    </h3>

                    <div class="blog-event-meta">

                        <%-- Use PlaceHolder to avoid inline HTML string concatenation --%>
                        <asp:PlaceHolder runat="server"
                            Visible='<%# !string.IsNullOrEmpty(Eval("EventLocation") as string) %>'>
                            <div class="blog-location">
                                <span><i class="ic-location"></i></span>
                                <%# Eval("EventLocation") %>
                            </div>
                        </asp:PlaceHolder>

                        <asp:PlaceHolder runat="server"
                            Visible='<%# !string.IsNullOrEmpty(Eval("EventDate") as string) %>'>
                            <div class="blog-date">
                                <span><i class="ic-date"></i></span>
                                <%# Eval("EventDate") %>
                            </div>
                        </asp:PlaceHolder>

                    </div>

                    <div class="blog-more1 mt-auto d-flex">
                        <a href='<%# Eval("EventUrl") %>'
                           class='btn btn-primary <%# (bool)Eval("DisableButton") ? "disabled" : "" %>'>
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
