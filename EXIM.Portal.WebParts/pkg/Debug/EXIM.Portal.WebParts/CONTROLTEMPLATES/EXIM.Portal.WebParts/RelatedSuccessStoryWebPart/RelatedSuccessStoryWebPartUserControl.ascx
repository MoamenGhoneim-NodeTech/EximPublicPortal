<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedSuccessStoryWebPartUserControl.ascx.cs" Inherits="EXIM.Portal.WebParts.RelatedSuccessStoryWebPart.RelatedSuccessStoryWebPartUserControl" %>


<asp:PlaceHolder ID="phSection" runat="server" Visible="false">

    <div data-aos="fade-right" data-aos-delay="100">
        <div class="home-blogs-slider owl-carousel owl-theme">
            <asp:PlaceHolder ID="phItems" runat="server" />
        </div>
    </div>

    <%-- No-results message (only shown when NoResultsMessage is set) --%>
    <asp:PlaceHolder ID="phNoResults" runat="server" Visible="false">
        <div class="ms-srch-result-noResults">
            <asp:Literal ID="litNoResults" runat="server" />
        </div>
    </asp:PlaceHolder>

    <%-- Owl Carousel init — mirrors ctx.OnPostRender from the original JS --%>
    <script type="text/javascript">
        (function () {
            function initSuccessStoryCarousel() {
                if (typeof jQuery === 'undefined' || typeof jQuery.fn.owlCarousel === 'undefined') {
                    setTimeout(initSuccessStoryCarousel, 100);
                    return;
                }
                var owl = jQuery('.home-blogs-slider');
                if (!owl.find('.item').length) {
                    jQuery('.home-blogs-section').hide();
                    return;
                }
                owl.owlCarousel({
                    loop: true,
                    responsiveClass: true,
                    margin: 24,
                    stagePadding: 200,
                    rtl: <%: IsArabic ? "true" : "false" %>,
                    responsive: {
                        0: { items: 1, stagePadding: 0 },
                        575: { items: 1, stagePadding: 0 },
                        768: { items: 2, stagePadding: 50 },
                        992: { items: 2, stagePadding: 80 },
                        1300: { items: 2, stagePadding: 100 }
                    },
                    dots: false,
                    nav: false,
                    navText: ['<i class="ic-slider-nav-right"></i>',
                        '<i class="ic-slider-nav-left"></i>'],
                    autoplay: false
                });
            }

            if (document.readyState === 'loading')
                document.addEventListener('DOMContentLoaded', initSuccessStoryCarousel);
            else
                initSuccessStoryCarousel();
        })();
    </script>

</asp:PlaceHolder>
