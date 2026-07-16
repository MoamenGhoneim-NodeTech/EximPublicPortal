<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvitationForm.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.InvitationForm.InvitationForm" %>

<div class="row mb-4">
    <div class="col-12">
        <customtags:labelmessage runat="server" id="ucMessage" />
    </div>
</div>

<asp:Panel runat="server" ID="pnlFormBody">
    <h3>
        <asp:Literal runat="server" meta:resourcekey="formTitle"></asp:Literal>
    </h3>
     <h4 data-aos="fade-up" data-aos-delay="100">
 <asp:Literal runat="server" meta:resourcekey="formSubTitle"></asp:Literal></h4>


    <div class="custom-form mt-5">
        <div class="row g-4">

            <%-- Row 1: Name | Mobile --%>
            <div class="col-md-6">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" AssociatedControlID="txtName" meta:resourcekey="lblName"></asp:Label>
                    <asp:TextBox runat="server" ID="txtName" TextMode="SingleLine" CssClass="form-control" meta:resourcekey="txtName"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="rfvtxtName" ControlToValidate="txtName"
                        meta:resourcekey="RequiredField" ValidationGroup="subscribe"
                        CssClass="text-danger" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <%-- Letters-only: Arabic, Latin, spaces, hyphen, apostrophe, dot --%>
                    <asp:RegularExpressionValidator runat="server" ID="revtxtName" ControlToValidate="txtName"
                        ValidationExpression="^[\u0600-\u06FFa-zA-Z\s\-'\.]+$"
                        meta:resourcekey="InvalidName" ValidationGroup="subscribe"
                        CssClass="text-danger" Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </div>
            </div>

          
                 <div class="col-md-6">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" AssociatedControlID="txtMobileNumber" meta:resourcekey="lblMobileNumber"></asp:Label>
                    <div class="input-group">
                        <%-- Custom searchable country-code picker — same pattern as ReportAViolation --%>
                        <div class="country-code-picker" id="divCCP_INV">
                            <div class="ccp-selected" id="ccpSelected_INV">
                                <span id="ccpDisplay_INV">+---</span>
                                <span class="ccp-arrow">&#9662;</span>
                            </div>
                        </div>
                        <%-- Hidden field carries selected dial code on postback --%>
                        <asp:HiddenField runat="server" ID="hfSelectedCountryCode" />
                        <%-- Invisible DropDownList kept for structural compatibility --%>
                        <asp:DropDownList runat="server" ID="ddlCountryCode" Style="display:none;" />
                        <asp:TextBox runat="server" ID="txtMobileNumber" TextMode="SingleLine"
                            CssClass="form-control digits" meta:resourcekey="txtMobileNumber" MaxLength="14"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator runat="server" ID="rfvtxtMobileNumber" ControlToValidate="txtMobileNumber"
                        meta:resourcekey="RequiredField" ValidationGroup="subscribe"
                        CssClass="text-danger" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="revtxtMobileNumber" ControlToValidate="txtMobileNumber"
                        ValidationExpression="^\d{9,14}$"
                        meta:resourcekey="InvalidMobileNumber" ValidationGroup="subscribe"
                        CssClass="text-danger" Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </div>
            </div>

            <%-- Row 2: Mobile | Company | Job Title  (3 × col-md-4) --%>
         <div class="col-md-6">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" AssociatedControlID="txtEmail" meta:resourcekey="lblEmail"></asp:Label>
                    <%-- SingleLine avoids browser-native email validation conflicts with ASP.NET validators --%>
                    <asp:TextBox runat="server" ID="txtEmail" TextMode="SingleLine" CssClass="form-control" meta:resourcekey="txtEmail"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="rfvtxtEmail" ControlToValidate="txtEmail"
                        meta:resourcekey="RequiredField" ValidationGroup="subscribe"
                        CssClass="text-danger" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="revtxtEmail" ControlToValidate="txtEmail"
                        ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,24}$"
                        meta:resourcekey="InvalidEmail" ValidationGroup="subscribe"
                        CssClass="text-danger" Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </div>
            </div>

            <div class="col-md-6">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" AssociatedControlID="txtCompanyName" meta:resourcekey="lblCompanyName"></asp:Label>
                    <asp:TextBox runat="server" ID="txtCompanyName" TextMode="SingleLine"
                        CssClass="form-control" meta:resourcekey="txtCompanyName" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="rfvtxtCompanyName" ControlToValidate="txtCompanyName"
                        meta:resourcekey="RequiredField" ValidationGroup="subscribe"
                        CssClass="text-danger" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <%-- Letters-only: Arabic, Latin, spaces, common punctuation --%>
                    <asp:RegularExpressionValidator runat="server" ID="revtxtCompanyName" ControlToValidate="txtCompanyName"
                        ValidationExpression="^[\u0600-\u06FFa-zA-Z\s\-'\.&,]+$"
                        meta:resourcekey="InvalidName" ValidationGroup="subscribe"
                        CssClass="text-danger" Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </div>
            </div>

            <div class="col-md-6">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" AssociatedControlID="txtJobTitle" meta:resourcekey="lblJobTitle"></asp:Label>
                    <asp:TextBox runat="server" ID="txtJobTitle" TextMode="SingleLine"
                        CssClass="form-control" meta:resourcekey="txtJobTitle" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtJobTitle"
                        meta:resourcekey="RequiredField" ValidationGroup="subscribe"
                        CssClass="text-danger" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <%-- Letters-only: Arabic, Latin, spaces, hyphen, apostrophe, dot --%>
                    <asp:RegularExpressionValidator runat="server" ID="revtxtJobTitle" ControlToValidate="txtJobTitle"
                        ValidationExpression="^[\u0600-\u06FFa-zA-Z\s\-'\.]+$"
                        meta:resourcekey="InvalidName" ValidationGroup="subscribe"
                        CssClass="text-danger" Display="Dynamic">
                    </asp:RegularExpressionValidator>
                </div>
            </div>

            <%-- Captcha --%>
            <div class="col-md-12">
                <div class="form-group">
                    <customtags:captcha runat="server" validationgroup="subscribe" id="captcha"></customtags:captcha>
                </div>
            </div>

            <%-- Submit --%>
            <div class="col-md-12">
                <div class="form-group">
                    <i class="fas fa-spinner fa-spin d-none"></i>
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-secondary"
                        meta:resourcekey="btnSubmit" ValidationGroup="subscribe"
                        OnClientClick="return validateFormBeforeSubmit();" OnClick="btnSubmit_Click">
                    </asp:Button>
                </div>
            </div>

        </div><%-- /.row --%>
    </div><%-- /.custom-form --%>
</asp:Panel>

<%-- ============================================================
     Country-code picker — identical jQuery pattern to ReportAViolation.ascx
     CountriesJson and IsArabic are properties on the code-behind.
     ============================================================ --%>
<script>
    $(document).ready(function () {
        var data = <%= CountriesJson %>;
    var isArabic = <%= IsArabic ? "true" : "false" %>;
    var $hfCode  = $('#<%= hfSelectedCountryCode.ClientID %>');
    var $picker = $('#divCCP_INV');
    var $display = $('#ccpDisplay_INV');

    var $dropdown = $(
        '<div class="ccp-dropdown" style="display:none;position:fixed;">' +
        '<input type="text" class="ccp-search" placeholder="Search..." autocomplete="off" />' +
        '<ul></ul>' +
        '</div>'
    ).appendTo('body');

    var $search = $dropdown.find('.ccp-search');
    var $list = $dropdown.find('ul');

    function positionDropdown() {
        var rect = document.getElementById('ccpSelected_INV').getBoundingClientRect();
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

    function applyCode(code) {
        $display.text(code);
        $hfCode.val(code);
        $dropdown.hide();
        $search.val('');
    }

    $('#ccpSelected_INV').on('click', function (e) {
        e.stopPropagation();
        var opening = !$dropdown.is(':visible');
        if (opening) { positionDropdown(); $dropdown.show(); buildList(''); $search.focus(); }
        else { $dropdown.hide(); }
    });

    $(window).on('scroll resize', function () {
        if ($dropdown.is(':visible')) positionDropdown();
    });

    $search.on('input', function () { buildList($(this).val()); });

    $list.on('click', 'li', function () { applyCode($(this).data('code')); });

    $search.on('keydown', function (e) {
        var $items = $list.find('li');
        var $active = $list.find('li.ccp-active');
        if (e.key === 'ArrowDown') {
            e.preventDefault();
            var $n = $active.length ? $active.removeClass('ccp-active').next('li') : $items.first();
            if ($n.length) $n.addClass('ccp-active')[0].scrollIntoView({ block: 'nearest' });
        } else if (e.key === 'ArrowUp') {
            e.preventDefault();
            var $p = $active.length ? $active.removeClass('ccp-active').prev('li') : $items.last();
            if ($p.length) $p.addClass('ccp-active')[0].scrollIntoView({ block: 'nearest' });
        } else if (e.key === 'Enter') { e.preventDefault(); if ($active.length) $active.trigger('click'); }
        else if (e.key === 'Escape') { $dropdown.hide(); }
    });

    $(document).on('click', function (e) {
        if (!$picker.is(e.target) && !$picker.has(e.target).length &&
            !$dropdown.is(e.target) && !$dropdown.has(e.target).length)
            $dropdown.hide();
    });

    buildList('');

    // ── Restore state after postback (failed captcha / validation) ──
    var existingCode = $hfCode.val();
    if (existingCode) {
        $display.text(existingCode);   // restore visible label; hidden field value already set
    } else {
        // Fresh page load — default to KSA (+966)
        var ksa = data.filter(function (c) { return c.code === '966'; })[0];
        if (ksa) applyCode(ksa.code);
    }

    // ── digits-only enforcement on mobile number field ──
    $(document).on('keypress', '.digits', function (e) {
        if (!/\d/.test(String.fromCharCode(e.which))) e.preventDefault();
    });
    $(document).on('paste', '.digits', function (e) {
        e.preventDefault();
        var pasted = (e.originalEvent.clipboardData || window.clipboardData)
            .getData('text').replace(/\D/g, '');
        document.execCommand('insertText', false, pasted);
    });

    // ── letters-only enforcement (Name, Company, Job Title) ──
    $(document).on('keypress', '.letters-only', function (e) {
        var ch = String.fromCharCode(e.which);
        if (!/[\u0600-\u06FFa-zA-Z\s\-'\.,&]/.test(ch)) e.preventDefault();
    });
    $(document).on('paste', '.letters-only', function (e) {
        e.preventDefault();
        var pasted = (e.originalEvent.clipboardData || window.clipboardData)
            .getData('text').replace(/[^\u0600-\u06FFa-zA-Z\s\-'\.,&]/g, '');
        document.execCommand('insertText', false, pasted);
    });
});

    // Outside document.ready — must be global so OnClientClick="return validateFormBeforeSubmit();"
    // can reach it. Mirrors the identical function in ReportAViolation.ascx.
    //function validateFormBeforeSubmit() {
    //    var isValid = Page_ClientValidate('subscribe');
      
    //    return typeof isValid === 'boolean' ? isValid : Page_IsValid;
    //}

    function validateFormBeforeSubmit() {
        try {
            var isValid = Page_ClientValidate('subscribe');
            return typeof isValid === 'boolean' ? isValid : Page_IsValid;
        } catch (ex) {
            console.error('Validation error, blocking submit:', ex);
            return false; // fail closed, not open
        }
    }
</script>
