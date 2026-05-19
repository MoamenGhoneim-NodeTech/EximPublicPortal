<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PartnersWebPartUserControl.ascx.cs" Inherits="EXIM.Portal.WebParts.PartnersWebPart.PartnersWebPartUserControl" %>

<%-- ═══════════════════════════════════════════════════
     Partners carousel — one owlCarousel per category,
     structure mirrors Exim_BoardMembers.js exactly.
     ═══════════════════════════════════════════════════ --%>

<%-- Per-category carousel blocks are injected here by code-behind --%>
<asp:PlaceHolder ID="phResults" runat="server" />

<%-- No-results fallback --%>
<asp:PlaceHolder ID="phNoResults" runat="server" Visible="false">
    <div class="ms-srch-result-noResults text-center py-4">
        <asp:Literal ID="litNoResults" runat="server" />
    </div>
</asp:PlaceHolder>

<%-- ── Scoped styles ──────────────────────────────────────────────── --%>
<style type="text/css">
    /* Category header */
    .partners-section .section-header          { text-align: center; margin-bottom: 32px; }
    .partners-section .section-header h2       { font-size: 1.6rem; font-weight: 700; margin-bottom: 6px; }
    .partners-section .section-header p        { font-size: 1rem; color: #666; margin: 0; }

    /* Slider wrapper */
    .partners-section .partners-slider         { position: relative; }
    .partners-section .partners-slider__items  { }

    /* Partner card — mirrors .item in BoardMembers */
    .partners-section .item                    { padding: 8px; }
    .partners-section .item .img               { text-align: center; margin-bottom: 10px; }
    .partners-section .item .img img           { max-width: 100%; max-height: 90px; object-fit: contain; }
    .partners-section .partner-text            { text-align: center; }
    .partners-section .partner-text h3         { font-size: 1rem; font-weight: 600; margin: 0 0 4px; }
    .partners-section .partner-text a          { font-size: 0.85rem; color: inherit; text-decoration: none; }
    .partners-section .partner-text a:hover    { text-decoration: underline; }

    /* owlCarousel nav arrows */
    .partners-section .owl-nav button          { background: transparent !important; border: none; font-size: 1.4rem; }
</style>

<%-- ── owlCarousel init — one carousel per category ────────────────── --%>
<script type="text/javascript">
(function () {
    function initPartnersCarousels() {
        $('.partners-slider__items').each(function () {
            var $el = $(this);
            /* Remove any stray SP script tags injected inside the container */
            $el.children('script[id^="scriptBody"]').remove();

            var isRtl = typeof rtl !== 'undefined' ? rtl : ($('html').attr('dir') === 'rtl');

            $el.owlCarousel({
                loop: true,
                rtl: isRtl,
                responsiveClass: true,
                dots: false,
                nav: true,
                center: true,
                autoplay: true,
                margin: 40,
                stagePadding: 200,
                navText: [
                    '<i class="ic-slider-nav-right"></i>',
                    '<i class="ic-slider-nav-left"></i>'
                ],
                responsive: {
                    0:    { items: 1, stagePadding: 0, nav: true, center: false, margin: 16 },
                    480:  { items: 1, stagePadding: 0, nav: true, center: false, margin: 16 },
                    500:  { items: 2, stagePadding: 0, nav: true, center: false, margin: 16 },
                    768:  { items: 2, stagePadding: 30, nav: true },
                    992:  { items: 3, stagePadding: 80, nav: true },
                    1200: { items: 3, stagePadding: 120 },
                    1300: { items: 3, stagePadding: 250 }
                }
            });
        });
    }

    /* Wait for DOM + owlCarousel to be ready, isolated from other webparts */
    $(document).ready(function () {
        if ($.fn.owlCarousel) {
            initPartnersCarousels();
        } else {
            /* owlCarousel not yet loaded — retry once after a short delay */
            setTimeout(function () {
                if ($.fn.owlCarousel) initPartnersCarousels();
            }, 600);
        }
    });
}());
</script>
