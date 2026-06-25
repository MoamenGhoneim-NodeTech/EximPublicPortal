<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadRequest.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.GuidForm.DownloadRequest" %>

<div class="row mb-4">
    <div class="col-12">
        <customtags:labelmessage runat="server" id="ucMessage" />
    </div>
</div>

<asp:Panel runat="server" ID="pnlFormBody" DefaultButton="btnSubmit">

    <h2>
        <asp:Literal runat="server" ID="litFormTitle" meta:resourcekey="litFormTitle" />
    </h2>

    <div class="custom-form">
        <div class="row g-4">

            <!-- Company Name -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblCompanyName" CssClass="" meta:resourcekey="lblCompanyName" AssociatedControlID="txtCompanyName" />

                    <asp:TextBox runat="server" ID="txtCompanyName" CssClass="form-control" meta:resourcekey="txtCompanyName" TabIndex="1" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvCompanyName" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtCompanyName" Display="Dynamic" />
                </div>
            </div>

            <!-- Beneficiary Name -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="120">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblBeneficiaryName" CssClass="" meta:resourcekey="lblBeneficiaryName" AssociatedControlID="txtBeneficiaryName" />
                    <asp:TextBox runat="server" ID="txtBeneficiaryName" CssClass="form-control" meta:resourcekey="txtBeneficiaryName" TabIndex="2" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvBeneficiaryName" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtBeneficiaryName" Display="Dynamic" />
                    <asp:RegularExpressionValidator runat="server" ID="revBeneficiaryName" CssClass="text-danger"
                        meta:resourcekey="InvalidTextOnly" ValidationGroup="submit"
                        ControlToValidate="txtBeneficiaryName"
                        ValidationExpression="^[^\d]+$" Display="Dynamic" />
                </div>
            </div>

            <!-- City -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="140">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblCity" CssClass="" meta:resourcekey="lblCity" AssociatedControlID="txtCity" />

                    <asp:TextBox runat="server" ID="txtCity" CssClass="form-control" meta:resourcekey="txtCity" TabIndex="3" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvCity" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtCity" Display="Dynamic" />
                    <asp:RegularExpressionValidator runat="server" ID="revCity" CssClass="text-danger"
                        meta:resourcekey="InvalidTextOnly" ValidationGroup="submit"
                        ControlToValidate="txtCity"
                        ValidationExpression="^[^\d]+$" Display="Dynamic" />
                </div>
            </div>

            <!-- Mobile Number -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="150">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblMobileNumber" CssClass="" meta:resourcekey="lblMobileNumber" AssociatedControlID="txtMobileNumber" />
                    <div class="input-group">

                        <%-- Custom searchable country-code picker --%>
                        <div class="country-code-picker" id="divCCP_DR">
                            <div class="ccp-selected" id="ccpSelected_DR">
                                <span id="ccpDisplay_DR">+---</span>
                                <span class="ccp-arrow">&#9662;</span>
                            </div>
                        </div>
                        <%-- Hidden: keeps server-side ID intact for btnSubmit_Click --%>
                        <asp:HiddenField runat="server" ID="hfSelectedCountryCode" />
                        <asp:DropDownList runat="server" ID="ddlCountryCode" style="display:none;" />
                        <asp:TextBox runat="server" ID="txtMobileNumber" CssClass="form-control digits" meta:resourcekey="txtMobileNumber" MaxLength="14" />
                    </div>
                    <asp:RequiredFieldValidator runat="server" ID="rfvMobileNumber" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtMobileNumber" Display="Dynamic" />
                    <asp:RegularExpressionValidator runat="server" ID="revMobileNumber" CssClass="text-danger" meta:resourcekey="InvalidMobileNumber" ValidationGroup="submit" ControlToValidate="txtMobileNumber" ValidationExpression="^\d{9,14}$" Display="Dynamic" />
                </div>
            </div>

            <!-- Email -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblEmail" CssClass="" meta:resourcekey="lblEmail" AssociatedControlID="txtEmail" />
                    <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" meta:resourcekey="txtEmail" TextMode="Email" TabIndex="5" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvEmail" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtEmail" Display="Dynamic" />
                    <asp:RegularExpressionValidator runat="server" ID="revEmail" CssClass="text-danger" meta:resourcekey="InvalidEmail" ValidationGroup="submit" ControlToValidate="txtEmail" ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,24}$" Display="Dynamic" />
                </div>
            </div>

            <!-- Captcha -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblCaptcha" CssClass="d-flex" meta:resourcekey="lblCaptcha" />
                    <customtags:captcha runat="server" id="captcha" validationgroup="submit" />
                </div>
            </div>

            <!-- Submit -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <i class="fas fa-spinner fa-spin d-none"></i>
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-secondary" meta:resourcekey="btnSubmit" ValidationGroup="submit" OnClick="btnSubmit_Click" TabIndex="6" />
                </div>
            </div>

        </div>
    </div>
       <p> </p>
   <p> <%= GetLocalResourceObject("FooterText") %> <a href="mailto:nfs@saudiexim.gov.sa">nfs@saudiexim.gov.sa</a> .</p>


    <script>
        $(document).ready(function () {
            var data = <%= CountriesJson %>;
        var isArabic = <%= IsArabic ? "true" : "false" %>;
        var $hfCode = $('#<%= hfSelectedCountryCode.ClientID %>');
        var $picker  = $('#divCCP_DR');
        var $display = $('#ccpDisplay_DR');

        var $dropdown = $(
            '<div class="ccp-dropdown" style="display:none;position:fixed;">' +
                '<input type="text" class="ccp-search" placeholder="Search..." autocomplete="off" />' +
                '<ul></ul>' +
            '</div>'
        ).appendTo('body');

        var $search = $dropdown.find('.ccp-search');
        var $list   = $dropdown.find('ul');

        function positionDropdown() {
            var rect = document.getElementById('ccpSelected_DR').getBoundingClientRect();
            $dropdown.css({ top: rect.bottom, left: rect.left, width: Math.max(rect.width, 240) });
        }
        function buildList(filter) {
            filter = (filter || '').toLowerCase();
            $list.empty();
            var subset = filter
                ? $.grep(data, function (c) {
                    var name = isArabic ? c.nameAr : c.nameEn;
                    return c.code.toLowerCase().indexOf(filter) > -1 || name.toLowerCase().indexOf(filter) > -1;
                  })
                : data;
            $.each(subset.slice(0, 80), function (_, c) {
                $('<li>').attr('data-code', c.code)
                    .append($('<span class="ccp-li-code">').text(c.code))
                    .append($('<span class="ccp-li-name">').text(isArabic ? c.nameAr : c.nameEn))
                    .appendTo($list);
            });
        }
        function applyCode(code) { $display.text(code); $hfCode.val(code); $dropdown.hide(); $search.val(''); }

        $('#ccpSelected_DR').on('click', function (e) {
            e.stopPropagation();
            var opening = !$dropdown.is(':visible');
            if (opening) { positionDropdown(); $dropdown.show(); buildList(''); $search.focus(); }
            else { $dropdown.hide(); }
        });
        $(window).on('scroll resize', function () { if ($dropdown.is(':visible')) positionDropdown(); });
        $search.on('input', function () { buildList($(this).val()); });
        $list.on('click', 'li', function () { applyCode($(this).data('code')); });
        $search.on('keydown', function (e) {
            var $items = $list.find('li'), $active = $list.find('li.ccp-active');
            if (e.key === 'ArrowDown') { e.preventDefault(); var $n = $active.length ? $active.removeClass('ccp-active').next('li') : $items.first(); if ($n.length) $n.addClass('ccp-active')[0].scrollIntoView({ block: 'nearest' }); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); var $p = $active.length ? $active.removeClass('ccp-active').prev('li') : $items.last(); if ($p.length) $p.addClass('ccp-active')[0].scrollIntoView({ block: 'nearest' }); }
            else if (e.key === 'Enter') { e.preventDefault(); if ($active.length) $active.trigger('click'); }
            else if (e.key === 'Escape') { $dropdown.hide(); }
        });
        $(document).on('click', function (e) {
            if (!$picker.is(e.target) && !$picker.has(e.target).length && !$dropdown.is(e.target) && !$dropdown.has(e.target).length)
                $dropdown.hide();
        });
        buildList('');
        var ksa = data.filter(function (c) { return c.code === '966'; })[0];
        if (ksa) applyCode(ksa.code);

        // txtBeneficiaryName — letters (Arabic + English) and spaces only
        $('#<%= txtBeneficiaryName.ClientID %>').on('input', function () {
            $(this).val($(this).val().replace(/[^\u0600-\u06FFa-zA-Z\s]/g, ''));
        });
        $('#<%= txtCity.ClientID %>').on('input', function () {
            $(this).val($(this).val().replace(/[^\u0600-\u06FFa-zA-Z\s]/g, ''));
        });
        $('#<%= txtCompanyName.ClientID %>').on('input', function () {
            $(this).val($(this).val().replace(/[^\u0600-\u06FFa-zA-Z\s]/g, ''));
        });

    });
    </script>

  
</asp:Panel>

<!-- Success panel (shown after submit) -->
<asp:Panel runat="server" ID="pnlSuccess" CssClass="guide-form" Visible="false">
    <section class="guide-ads">
        <div class="container">
            <div class="guide-ads--container">
             <%--   <h3>
                    <asp:Literal runat="server" ID="litGuideSuccessSmallTitle" meta:resourcekey="GuideSuccessSmallTitle" /></h3>--%>
                <h2>
                    <asp:Literal runat="server" ID="litGuideSuccessTitle" meta:resourcekey="GuideSuccessTitle" /></h2>
                <p>
                    <asp:Literal runat="server" ID="litGuideSuccessDescription" meta:resourcekey="GuideSuccessDescription" /></p>

                <div class="guide-action">
                    <asp:LinkButton runat="server" ID="lnkbtnDownloadGuide" OnClick="lnkbtnDownloadGuide_Click" CssClass="btn btn-secondary" meta:resourcekey="GuideDownloadLink" >


                    </asp:LinkButton>
                    
                </div>
            </div>
        </div>
    </section>
</asp:Panel>
<asp:Panel ID="pnlFooter" runat="server">
 </asp:Panel>
