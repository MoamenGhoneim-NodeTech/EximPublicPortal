<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedServicesWebPartUserControl.ascx.cs" Inherits="EXIM.Portal.WebParts.RelatedServicesWebPart.RelatedServicesWebPartUserControl" %>

<%-- ============================================================
     EXIM Related Financial Services – server-side WebPart
     Replaces: Control_EXIM_RelatedFinServices.html / .js
     Renders an Owl Carousel of related service cards.
     If no items exist the wrapping section is hidden via
     the server-side phRelatedSection PlaceHolder.
     ============================================================ --%>

<%-- Outer section – hidden by code-behind when result set is empty
     (mirrors: if(!$(".services-item").length) $(".service-details-related-section").hide()) --%>
<asp:PlaceHolder ID="phRelatedSection" runat="server" Visible="false">

    <div class="service-details-related-section" dir="<%: IsArabic ? "rtl" : "ltr" %>">

        <%-- Carousel wrapper – Owl Carousel initialised in the inline script below --%>
        <div class="services-items services-items-slider owl-carousel owl-theme">
            <asp:PlaceHolder ID="phResults" runat="server" />
        </div>

        <%-- No-results message (server-side, shown only when library is empty) --%>
        <asp:PlaceHolder ID="phNoResults" runat="server" Visible="false">
            <div class="ms-srch-result-noResults">
                <asp:Literal ID="litNoResults" runat="server" />
            </div>
        </asp:PlaceHolder>

    </div>

    <%-- Owl Carousel init – mirrors ctx.OnPostRender from the original JS template.
         Runs after the DOM is ready so the carousel element is available.
         rtl value is written server-side to match the current language. --%>
    <script type="text/javascript">
    (function () {
        function initRelatedServicesCarousel() {
            if (typeof jQuery === 'undefined' || typeof jQuery.fn.owlCarousel === 'undefined') {
                setTimeout(initRelatedServicesCarousel, 100);
                return;
            }
            jQuery('.services-items-slider').owlCarousel({
                loop            : false,
                responsiveClass : true,
                margin          : 24,
                items           : 3,
                rtl             : <%: IsArabic ? "true" : "false" %>,
                responsive: {
                    0: { items: 1 },
                    480: { items: 1 },
                    500: { items: 2 },
                    768: { items: 2 },
                    992: { items: 3 },
                    1200: { items: 3 },
                    1300: { items: 3 }
                },
                dots: false,
                nav: false,
                navText: ['<i class="ic-slider-nav-right"></i>',
                    '<i class="ic-slider-nav-left"></i>'],
                autoplay: true
            });
            }

            if (document.readyState === 'loading') {
                document.addEventListener('DOMContentLoaded', initRelatedServicesCarousel);
            } else {
                initRelatedServicesCarousel();
            }
        })();
    </script>

</asp:PlaceHolder>
