<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomePageControls.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.HomePage.HomePageControls" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>


    <link rel="stylesheet" href="/Style%20Library/exim/chatbot/chatbot.css" />
	<%--<script defer src="/Style Library/exim/js/homeV2.js"></script>
	<script defer src="/Style%20Library/exim/chatbot/chatbot.js"></script>--%>

<script src="/Style Library/exim/js/homeV2.js"></script>
<script src="/Style Library/exim/chatbot/chatbot.js"></script>

<%-- ══════════════════════════════════════════════════════
     HERO / BANNERS SLIDER
     ══════════════════════════════════════════════════════ --%>
<section class="section-home-hero">
    <div class="container">
        <div class="home-slider owl-carousel owl-theme">
            <asp:Literal ID="litBanners" runat="server" />
        </div>
    </div>
</section>


<%-- ══════════════════════════════════════════════════════
     QUICK LINKS / FINANCIAL SOLUTIONS
     ══════════════════════════════════════════════════════ --%>
<section class="quick-link-section">
    <div class="container">
        <div class="section-header">
            <div class="text-center">
                <SharePoint:LanguageSpecificContent runat="server" Languages="1033">
                    <contenttemplate>
                        <h2 data-aos="fade-up" data-aos-delay="100" class="text-blue">Financial and commercial solutions</h2>
                        <p data-aos="fade-up" data-aos-delay="200">Financial and insurance solutions that suit your needs</p>
                    </contenttemplate>
                </SharePoint:LanguageSpecificContent>
                <SharePoint:LanguageSpecificContent runat="server" Languages="1025">
                    <contenttemplate>
                        <h2 data-aos="fade-up" data-aos-delay="100" class="text-blue">الحلول التمويلية والتجارية</h2>
                        <p data-aos="fade-up" data-aos-delay="200">حلول تمويلية وتأمينية تناسب احتياجك</p>
                    </contenttemplate>
                </SharePoint:LanguageSpecificContent>
            </div>
        </div>
        <div class="financialSolutions cards-container row g-3">
            <asp:Literal ID="litFinancialSolutions" runat="server" />
        </div>
    </div>
</section>


<%-- ══════════════════════════════════════════════════════
     NUMBERS / COUNTERS
     ══════════════════════════════════════════════════════ --%>
<section class="number-section">
    <div class="container">
        <div class="number-section-bg">
            <div class="homeNumbers row g-5">
                <asp:Literal ID="litHomeNumbers" runat="server" />
            </div>
        </div>
    </div>
</section>


<%-- ══════════════════════════════════════════════════════
     FINANCIAL PRODUCTS ADVISOR  (fully static)
     ══════════════════════════════════════════════════════ --%>
<section class="financial-products-advisor-section">
    <div class="container">
        <div class="row">
            <div class="col-md-6 order-first order-md-last">
                <div class="financial-products-advisor-img" data-aos="fade-right" data-aos-delay="100">
                    <img src="/Style Library/exim/ar/assets/images/vis.png" alt="">
                </div>
            </div>
            <div class="col-md-6 order-md-first order-last d-flex align-items-center">
                <div class="financial-products-advisor-text">
                    <SharePoint:LanguageSpecificContent runat="server" Languages="1033">
                        <contenttemplate>
                            <h3 data-aos="fade-up" data-aos-delay="100">Your Financial Products Advisor</h3>
                            <h2 data-aos="fade-up" data-aos-delay="200">We help you choose the financial services that suit your needs.</h2>
                            <p data-aos="fade-up" data-aos-delay="300">The Saudi Export-Import Bank aims to promote the development of Saudi non-oil exports and increase their competitiveness in global markets across various sectors by providing credit solutions for export financing, guarantees, and export credit insurance with competitive advantages. This is in line with the objectives and pillars of the Kingdom&apos;s Vision 2030, which aims to increase the proportion of non-oil exports to non-oil GDP.</p>
                            <div class="financial-products-advisor-text--action">
                                <a href="" class="btn btn-secondary" data-aos="fade-up" data-aos-delay="200">
                                    Contact a Product Advisor <i class="ic-arrow-left"></i>
                                </a>
                            </div>
                        </contenttemplate>
                    </SharePoint:LanguageSpecificContent>
                    <SharePoint:LanguageSpecificContent runat="server" Languages="1025">
                        <contenttemplate>
                            <h3 data-aos="fade-up" data-aos-delay="100">مستشار منتجاتك المالية</h3>
                            <h2 data-aos="fade-up" data-aos-delay="200">نساعدك في اختيار الخدمات المالية المناسبة لك بناءً على احتياجاتك.</h2>
                            <p data-aos="fade-up" data-aos-delay="300">يهدف بنك التصدير والاستيراد السعودي إلى تعزيز تنمية الصادرات السعودية غير النفطية وزيادة تنافسيتها في الأسواق العالمية في مختلف القطاعات، وذلك من خلال تقديم حلول ائتمانية لتمويل الصادرات، والضمانات، وتأمين ائتمان الصادرات بمزايا تنافسية، وذلك ضمن أهداف ومرتكزات رؤية المملكة 2030 المعنية بزيادة نسبة الصادرات غير النفطية إلى الناتج الإجمالي المحلي غير النفطي.</p>
                            <div class="financial-products-advisor-text--action">
                                <a href="" class="btn btn-secondary" data-aos="fade-up" data-aos-delay="200">
                                    تواصل مع مستشار المنتجات <i class="ic-arrow-left"></i>
                                </a>
                            </div>
                        </contenttemplate>
                    </SharePoint:LanguageSpecificContent>
                </div>
            </div>
        </div>
    </div>
</section>


<%-- ══════════════════════════════════════════════════════
     KNOWLEDGE CENTER / SOLUTIONS TABS
     ══════════════════════════════════════════════════════ --%>
<section class="solutions-section">
    <div class="container">
        <div class="section-header">
            <div class="text-center">
                <SharePoint:LanguageSpecificContent runat="server" Languages="1033">
                    <contenttemplate>
                        <h2 class="text-blue" data-aos="fade-up" data-aos-delay="100">Financing and Commercial Solutions</h2>
                        <p data-aos="fade-up" data-aos-delay="200">Everything you need to know about financing and commercial solutions designed for you</p>
                    </contenttemplate>
                </SharePoint:LanguageSpecificContent>
                <SharePoint:LanguageSpecificContent runat="server" Languages="1025">
                    <contenttemplate>
                        <h2 class="text-blue" data-aos="fade-up" data-aos-delay="100">الحلول التمويلية والتجارية</h2>
                        <p data-aos="fade-up" data-aos-delay="200">كل ما تحتاج معرفته عن الحلول التمويلية والتجارية المصممة لك</p>
                    </contenttemplate>
                </SharePoint:LanguageSpecificContent>
            </div>
        </div>

        <div class="solutions-tabs">

            <%-- Tab nav — EN --%>
            <SharePoint:LanguageSpecificContent runat="server" Languages="1033">
                <contenttemplate>
                    <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist" data-aos="fade-up" data-aos-delay="100">
                        <li class="nav-item" role="presentation">
                            <button class="nav-link active" id="pills-sol-5-tab" data-bs-toggle="pill"
                                    data-bs-target="#pills-sol-5" type="button" role="tab"
                                    aria-controls="pills-sol-5" aria-selected="true">Export Procedures and Requirements
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="pills-sol-4-tab" data-bs-toggle="pill"
                                    data-bs-target="#pills-sol-4" type="button" role="tab"
                                    aria-controls="pills-sol-4" aria-selected="false">Market Research and Export Planning
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="pills-sol-3-tab" data-bs-toggle="pill"
                                    data-bs-target="#pills-sol-3" type="button" role="tab"
                                    aria-controls="pills-sol-3" aria-selected="false">Managing and sustaining business operations
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="pills-sol-2-tab" data-bs-toggle="pill"
                                    data-bs-target="#pills-sol-2" type="button" role="tab"
                                    aria-controls="pills-sol-2" aria-selected="false">Business Development and Growth
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="pills-sol-1-tab" data-bs-toggle="pill"
                                    data-bs-target="#pills-sol-1" type="button" role="tab"
                                    aria-controls="pills-sol-1" aria-selected="false">Accurate Scientific Research and Articles
                            </button>
                        </li>
                    </ul>
                </contenttemplate>
            </SharePoint:LanguageSpecificContent>

            <%-- Tab nav — AR (pills-sol-1 hidden per original) --%>
            <SharePoint:LanguageSpecificContent runat="server" Languages="1025">
                <contenttemplate>
                    <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist" data-aos="fade-up" data-aos-delay="100">
                        <li class="nav-item" role="presentation">
                            <button class="nav-link active" id="pills-sol-5-tab" data-bs-toggle="pill"
                                    data-bs-target="#pills-sol-5" type="button" role="tab"
                                    aria-controls="pills-sol-5" aria-selected="true">إجراءات ومتطلبات التصدير
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="pills-sol-4-tab" data-bs-toggle="pill"
                                    data-bs-target="#pills-sol-4" type="button" role="tab"
                                    aria-controls="pills-sol-4" aria-selected="false">مصادر لدراسة الأسواق والتخطيط للتصدير
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="pills-sol-3-tab" data-bs-toggle="pill"
                                    data-bs-target="#pills-sol-3" type="button" role="tab"
                                    aria-controls="pills-sol-3" aria-selected="false">مصادر لإدارة واستدامة العمليات التجارية
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="pills-sol-2-tab" data-bs-toggle="pill"
                                    data-bs-target="#pills-sol-2" type="button" role="tab"
                                    aria-controls="pills-sol-2" aria-selected="false">مصادر لتطوير وتنمية الأعمال
                            </button>
                        </li>
                        <%-- pills-sol-1 intentionally hidden in AR per original layout --%>
                    </ul>
                </contenttemplate>
            </SharePoint:LanguageSpecificContent>

            <div class="tab-content" id="pills-tabContent">

                <div class="tab-pane fade show active" id="pills-sol-5" role="tabpanel"
                     aria-labelledby="pills-sol-5-tab" tabindex="0">
                    <div class="solutions-items-slider owl-carousel" data-aos="fade-up" data-aos-delay="100">
                        <asp:Literal ID="litKnowledgeSlider5" runat="server" />
                    </div>
                    <div class="solutions-items-navs owl-carousel" data-aos="fade-up" data-aos-delay="100">
                        <asp:Literal ID="litKnowledgeNavs5" runat="server" />
                    </div>
                </div>

                <div class="tab-pane fade" id="pills-sol-4" role="tabpanel"
                     aria-labelledby="pills-sol-4-tab" tabindex="0">
                    <div class="solutions-items-slider owl-carousel" data-aos="fade-up" data-aos-delay="100">
                        <asp:Literal ID="litKnowledgeSlider4" runat="server" />
                    </div>
                    <div class="solutions-items-navs owl-carousel" data-aos="fade-up" data-aos-delay="100">
                        <asp:Literal ID="litKnowledgeNavs4" runat="server" />
                    </div>
                </div>

                <div class="tab-pane fade" id="pills-sol-3" role="tabpanel"
                     aria-labelledby="pills-sol-3-tab" tabindex="0">
                    <div class="solutions-items-slider owl-carousel" data-aos="fade-up" data-aos-delay="100">
                        <asp:Literal ID="litKnowledgeSlider3" runat="server" />
                    </div>
                    <div class="solutions-items-navs owl-carousel" data-aos="fade-up" data-aos-delay="100">
                        <asp:Literal ID="litKnowledgeNavs3" runat="server" />
                    </div>
                </div>

                <div class="tab-pane fade" id="pills-sol-2" role="tabpanel"
                     aria-labelledby="pills-sol-2-tab" tabindex="0">
                    <div class="solutions-items-slider owl-carousel" data-aos="fade-up" data-aos-delay="100">
                        <asp:Literal ID="litKnowledgeSlider2" runat="server" />
                    </div>
                    <div class="solutions-items-navs owl-carousel" data-aos="fade-up" data-aos-delay="100">
                        <asp:Literal ID="litKnowledgeNavs2" runat="server" />
                    </div>
                </div>

                <%-- Tab 1: hidden in AR nav but pane rendered for EN --%>
                <div class="tab-pane fade" id="pills-sol-1" role="tabpanel"
                     aria-labelledby="pills-sol-1-tab" tabindex="0">
                    <div class="solutions-items-slider owl-carousel" data-aos="fade-up" data-aos-delay="100">
                        <asp:Literal ID="litKnowledgeSlider1" runat="server" />
                    </div>
                    <div class="solutions-items-navs owl-carousel" data-aos="fade-up" data-aos-delay="100">
                        <asp:Literal ID="litKnowledgeNavs1" runat="server" />
                    </div>
                </div>

            </div><%-- /tab-content --%>
        </div><%-- /solutions-tabs --%>
    </div><%-- /container --%>
</section>


<%-- ══════════════════════════════════════════════════════
     SUCCESS STORIES
     pnlStories visibility controlled in code-behind (hidden when < 3 items)
     ══════════════════════════════════════════════════════ --%>
<asp:Panel ID="pnlStories" runat="server" CssClass="home-stories-section">
    <div class="container">

        <SharePoint:LanguageSpecificContent runat="server" Languages="1033">
            <contenttemplate>
                <div class="stories-sec-header">
                    <div data-aos="fade-up">
                        <h2 data-aos="fade-up" data-aos-delay="100">Success stories</h2>
                        <h3 data-aos="fade-up" data-aos-delay="150">Our clients&apos; success stories</h3>
                    </div>
                    <div class="d-flex" data-aos="fade-right" data-aos-delay="100">
                        <a href="/en/MediaCenter/SuccessStories" class="btn btn-secondary">More success stories <i class="ic-arrow-left"></i></a>
                    </div>
                </div>
            </contenttemplate>
        </SharePoint:LanguageSpecificContent>
        <SharePoint:LanguageSpecificContent runat="server" Languages="1025">
            <contenttemplate>
                <div class="stories-sec-header">
                    <div data-aos="fade-up">
                        <h2 data-aos="fade-up" data-aos-delay="100">قصص النجاح</h2>
                        <h3 data-aos="fade-up" data-aos-delay="150">قصص نجاح عملاؤنا</h3>
                    </div>
                    <div class="d-flex" data-aos="fade-right" data-aos-delay="100">
                        <a href="/ar/MediaCenter/SuccessStories" class="btn btn-secondary">المزيد من قصص النجاح <i class="ic-arrow-left"></i></a>
                    </div>
                </div>
            </contenttemplate>
        </SharePoint:LanguageSpecificContent>

        <div class="home-story-items">
            <div class="row g-3">
                <div class="col-md-6">
                    <div class="first-story-item" data-aos="fade-up" data-aos-delay="100">
                        <asp:Literal ID="litFirstStory" runat="server" />
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="home-story-left-items">
                        <asp:Literal ID="litOtherStories" runat="server" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Panel>


<%-- ══════════════════════════════════════════════════════
     NEWS / BLOGS SLIDER
     ══════════════════════════════════════════════════════ --%>
<section class="home-blogs-section">
    <div class="container">
        <div class="row">
            <div class="col-lg-3 col-md-4 d-flex flex-column">
                <SharePoint:LanguageSpecificContent runat="server" Languages="1033">
                    <contenttemplate>
                        <div class="home-blogs-text">
                            <div>
                                <h2 data-aos="fade-up" data-aos-delay="100">About the Bank</h2>
                                <h3 data-aos="fade-up" data-aos-delay="150">Latest News and Events</h3>
                            </div>
                            <p data-aos="fade-up" data-aos-delay="200">The Saudi Export-Import Bank works to empower non-oil national exports and enhance their presence in global markets.</p>
                            <div class="d-flex" data-aos="fade-up" data-aos-delay="250">
                                <a href="/en/MediaCenter/News" class="btn btn-secondary">Learn More <i class="ic-arrow-left"></i></a>
                            </div>
                        </div>
                    </contenttemplate>
                </SharePoint:LanguageSpecificContent>
                <SharePoint:LanguageSpecificContent runat="server" Languages="1025">
                    <contenttemplate>
                        <div class="home-blogs-text">
                            <div>
                                <h2 data-aos="fade-up" data-aos-delay="100">عن البنك</h2>
                                <h3 data-aos="fade-up" data-aos-delay="150">اخر الاخبار والأحداث</h3>
                            </div>
                            <p data-aos="fade-up" data-aos-delay="200">يعمل بنك التصدير والاستيراد السعودي على تمكين الصادرات الوطنية غير النفطية، وتعزيز حضورها في الأسواق العالمية</p>
                            <div class="d-flex" data-aos="fade-up" data-aos-delay="250">
                                <a href="/ar/MediaCenter/News" class="btn btn-secondary">معرفة المزيد <i class="ic-arrow-left"></i></a>
                            </div>
                        </div>
                    </contenttemplate>
                </SharePoint:LanguageSpecificContent>
                <div class="custom-nav-container mt-auto" data-aos="fade-up" data-aos-delay="100">
                    <a role="button" class="custom-prev-btn btn"><i class="ic-slider-nav-right"></i></a>
                    <a role="button" class="custom-next-btn btn"><i class="ic-slider-nav-left"></i></a>
                </div>
            </div>
            <div class="col-lg-9 col-md-8" data-aos="fade-right" data-aos-delay="100">
                <div class="home-blogs-slider owl-carousel owl-theme">
                    <asp:Literal ID="litNews" runat="server" />
                </div>
            </div>
        </div>
    </div>
</section>


<%-- ══════════════════════════════════════════════════════
     PARTNERS SLIDER
     ══════════════════════════════════════════════════════ --%>
<section class="partners-section">
    <div class="container">
        <SharePoint:LanguageSpecificContent runat="server" Languages="1033">
            <contenttemplate>
                <h2>100+ local and international cooperation bodies</h2>
            </contenttemplate>
        </SharePoint:LanguageSpecificContent>
        <SharePoint:LanguageSpecificContent runat="server" Languages="1025">
            <contenttemplate>
                <h2>100+ جهة تعاون محلية ودولية</h2>
            </contenttemplate>
        </SharePoint:LanguageSpecificContent>
        <div class="partners-slider owl-carousel owl-theme">
            <asp:Literal ID="litPartners" runat="server" />
        </div>
    </div>
</section>


<%-- ══════════════════════════════════════════════════════
     CHATBOT
     ══════════════════════════════════════════════════════ --%>
<SharePoint:LanguageSpecificContent runat="server" Languages="1025">
    <ContentTemplate>
        <div id="float-btn"></div>
        <div id="chat-container" style="display: none;">
            <iframe src="https://digital.saudiexim.gov.sa/ExAIAssistant/" loading="lazy" title="مساعد ExAI"></iframe>
        </div>
    </ContentTemplate>
</SharePoint:LanguageSpecificContent>

<SharePoint:LanguageSpecificContent runat="server" Languages="1033">
    <ContentTemplate>
        <div id="float-btn"></div>
        <div id="chat-container" style="display: none;">
            <iframe src="https://digital.saudiexim.gov.sa/ExAIAssistant/" loading="lazy" title="ExAI Assistant"></iframe>
        </div>
    </ContentTemplate>
</SharePoint:LanguageSpecificContent>

<%-- Hero slider videos: MP4 sources use data-exim-src; exim-deferred-hero-video loads when near viewport (mobile LCP). --%>
<script type="text/javascript">
(function () {
    function wireDeferredHeroVideos() {
        var vids = document.querySelectorAll('video.exim-deferred-hero-video');
        if (!vids.length) return;
        function activateVideo(v) {
            var s = v.querySelector('source[data-exim-src]');
            if (!s) return;
            var url = s.getAttribute('data-exim-src');
            if (!url || s.getAttribute('src')) return;
            s.setAttribute('src', url);
            try { v.load(); v.play().catch(function () { }); } catch (e) { }
        }
        if (!('IntersectionObserver' in window)) {
            for (var i = 0; i < vids.length; i++) activateVideo(vids[i]);
            return;
        }
        var io = new IntersectionObserver(function (entries) {
            for (var j = 0; j < entries.length; j++) {
                if (!entries[j].isIntersecting) continue;
                var v = entries[j].target;
                activateVideo(v);
                io.unobserve(v);
            }
        }, { rootMargin: '120px', threshold: 0.01 });
        for (var k = 0; k < vids.length; k++) io.observe(vids[k]);
    }
    if (document.readyState === 'loading')
        document.addEventListener('DOMContentLoaded', wireDeferredHeroVideos);
    else
        wireDeferredHeroVideos();
})();
</script>
