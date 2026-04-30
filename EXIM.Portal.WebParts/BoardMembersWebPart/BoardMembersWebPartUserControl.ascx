<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BoardMembersWebPartUserControl.ascx.cs" Inherits="EXIM.Portal.WebParts.BoardMembersWebPart.BoardMembersWebPartUserControl" %>

<section class="team-section">

    <div class="container">
        <div class="section-header">
            <div class="text-center">
                <h2 data-aos="fade-up" data-aos-delay="100">
                    <asp:Literal ID="litAboutText" runat="server" />
                </h2>
                <p data-aos="fade-up" data-aos-delay="150">
                    <asp:Literal ID="litBoardText" runat="server" />
                </p>
            </div>
        </div>
    </div>

    <div class="container-fluid" data-aos="fade-up" data-aos-delay="100">
        <div class="team-slider">
            <div class="team-slider__items owl-carousel owl-theme">

                <asp:Repeater ID="rptBoardMembers" runat="server">
                    <ItemTemplate>
                        <div class="item">

                            <div class="img">
                                <asp:PlaceHolder runat="server"
                                    Visible='<%# !string.IsNullOrEmpty(Eval("ImgPath") as string) %>'>
                                    <img src='<%# Eval("ImgPath") %>'
                                         alt='<%# Eval("AltText") %>' />
                                </asp:PlaceHolder>
                                <asp:PlaceHolder runat="server"
                                    Visible='<%# string.IsNullOrEmpty(Eval("ImgPath") as string) %>'>
                                    <div class="img-placeholder" aria-hidden="true"></div>
                                </asp:PlaceHolder>
                            </div>

                            <div class="team-text">
                                <h2><%# Eval("Title") %></h2>
                                <h3><%# Eval("Name") %></h3>
                                <p><%# Eval("JobTitle") %></p>
                            </div>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>

            </div>
        </div>
    </div>

</section>
<script type="text/javascript">

    (function () {
        'use strict';

        // Derive RTL from SharePoint page context.
        var isRtl = _spPageContextInfo.currentLanguage !== 1033;

        // Nav arrow icons swap direction in RTL.
        var navPrev = isRtl
            ? '<i class="ic-slider-nav-left"  aria-hidden="true"></i>'
            : '<i class="ic-slider-nav-right" aria-hidden="true"></i>';

        var navNext = isRtl
            ? '<i class="ic-slider-nav-right" aria-hidden="true"></i>'
            : '<i class="ic-slider-nav-left"  aria-hidden="true"></i>';

        function initCarousel() {
            var $slider = $('.team-slider__items');

            // SharePoint may inject inline <script> tags into the rendered output —
            // remove them so Owl Carousel does not count them as slide items.
            $slider.children('script[id^="scriptBody"]').remove();

            $slider.owlCarousel({
                loop: true,
                autoplay: true,
                center: true,
                dots: false,
                nav: true,
                navText: [navPrev, navNext],
                margin: 40,
                rtl: isRtl,
                responsiveClass: true,
                responsive: {
                    0: {
                        items: 1,
                        stagePadding: 0,
                        margin: 16,
                        center: false,
                        nav: true
                    },
                    576: {
                        items: 2,
                        stagePadding: 0,
                        margin: 16,
                        center: false,
                        nav: true
                    },
                    768: {
                        items: 2,
                        stagePadding: 30,
                        nav: true
                    },
                    992: {
                        items: 3,
                        stagePadding: 80,
                        nav: true
                    },
                    1200: {
                        items: 3,
                        stagePadding: 120
                    },
                    1300: {
                        items: 3,
                        stagePadding: 250
                    }
                }
            });
        }

        $(document).ready(initCarousel);

    }());
</script>