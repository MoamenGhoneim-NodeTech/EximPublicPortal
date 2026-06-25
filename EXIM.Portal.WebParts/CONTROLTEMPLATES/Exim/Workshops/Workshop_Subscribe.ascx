<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Workshop_Subscribe.ascx.cs" Inherits="EXIM.Portal.WebParts.Workshop_Subscribe" %>

<div class="row mb-4">
    <div class="col-12">
        <CustomTags:LabelMessage runat="server" id="ucMessage" />
    </div>
</div>

<asp:Panel runat="server" ID="pnlFormBody">
    <h3 data-aos="fade-up" data-aos-delay="100">
        <asp:Literal runat="server" meta:resourcekey="formTitle"></asp:Literal>
    </h3>
    <div class="row g-4">

        <!-- Company Name -->
        <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
            <div class="form-group">
                <span class="text-danger">*</span>
                <asp:Label runat="server" AssociatedControlID="txtCompanyName" meta:resourcekey="lblCompanyName" />
                <asp:TextBox runat="server" ID="txtCompanyName" TextMode="SingleLine" CssClass="form-control" meta:resourcekey="txtCompanyName" MaxLength="255" />
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtCompanyName" ControlToValidate="txtCompanyName" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic" />
            </div>
        </div>

        <!-- Commercial Number -->
        <div class="col-md-6" data-aos="fade-up" data-aos-delay="120">
            <div class="form-group">
                <span class="text-danger">*</span>
                <asp:Label runat="server" AssociatedControlID="txtCommNumber" meta:resourcekey="lblCommNumber" />
                <asp:TextBox runat="server" ID="txtCommNumber" TextMode="SingleLine" CssClass="form-control" meta:resourcekey="txtCommNumber" />
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtCommNumber" ControlToValidate="txtCommNumber" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic" />
            </div>
        </div>

        <!-- Responsible Person -->
        <div class="col-md-6" data-aos="fade-up" data-aos-delay="140">
            <div class="form-group">
                <span class="text-danger">*</span>
                <asp:Label runat="server" AssociatedControlID="txtResponsiblePersonName" meta:resourcekey="lblResponsiblePersonName" />
                <asp:TextBox runat="server" ID="txtResponsiblePersonName" TextMode="SingleLine" CssClass="form-control" meta:resourcekey="txtResponsiblePersonName" />
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtResponsiblePersonName" ControlToValidate="txtResponsiblePersonName" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic" />
            </div>
        </div>

        <!-- Mobile Number with country code picker -->
        <div class="col-md-6" data-aos="fade-up" data-aos-delay="150">
            <div class="form-group">
                <span class="text-danger">*</span>
                <asp:Label runat="server" AssociatedControlID="txtMobileNumber" meta:resourcekey="lblMobileNumber" />
                <div class="input-group">

                    <%-- Custom searchable country-code picker --%>
                    <div class="country-code-picker" id="divCCP_WS">
                        <div class="ccp-selected" id="ccpSelected_WS">
                            <span id="ccpDisplay_WS">+---</span>
                            <span class="ccp-arrow">&#9662;</span>
                        </div>
                    </div>

                    <%-- Holds selected code (+966) submitted with the form --%>
                    <asp:HiddenField runat="server" ID="hfSelectedCountryCode" />

                    <%-- ddlCountryCode kept hidden so server-side references compile --%>
                    <asp:DropDownList runat="server" ID="ddlCountryCode" style="display:none;" />

                    <asp:TextBox runat="server" ID="txtMobileNumber" TextMode="SingleLine" CssClass="form-control digits" meta:resourcekey="txtMobileNumber" />
                </div>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtMobileNumber" ControlToValidate="txtMobileNumber" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic" />
                <asp:RegularExpressionValidator runat="server" ID="revtxtMobileNumber" ControlToValidate="txtMobileNumber" ValidationExpression="^\d{9,14}$" meta:resourcekey="InvalidMobileNumber" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic" />
            </div>
        </div>

        <!-- Email -->
        <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
            <div class="form-group">
                <span class="text-danger">*</span>
                <asp:Label runat="server" AssociatedControlID="txtEmail" meta:resourcekey="lblEmail" />
                <asp:TextBox runat="server" ID="txtEmail" TextMode="Email" CssClass="form-control" meta:resourcekey="txtEmail" />
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtEmail" ControlToValidate="txtEmail" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic" />
                <asp:RegularExpressionValidator runat="server" ID="revtxtEmail" ControlToValidate="txtEmail" ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,24}$" meta:resourcekey="InvalidEmail" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic" />
            </div>
        </div>

        <!-- Captcha -->
        <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
            <div class="form-group">
                <customtags:captcha runat="server" validationgroup="subscribe" id="captcha" />
            </div>
        </div>

        <!-- Submit -->
        <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
            <div class="form-group">
                <i class="fas fa-spinner fa-spin d-none"></i>
                <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-secondary" meta:resourcekey="btnSubmit" ValidationGroup="subscribe" OnClick="btnSubmit_Click" />
            </div>
        </div>

    </div>

    <script>
        $(document).ready(function () {
            var data = <%= CountriesJson %>;
        var isArabic = <%= IsArabic ? "true" : "false" %>;
        var $hfCode  = $('#<%= hfSelectedCountryCode.ClientID %>');
        var $picker = $('#divCCP_WS');
        var $display = $('#ccpDisplay_WS');

        // Build dropdown and append to <body> to escape any overflow:hidden parent
        var $dropdown = $(
            '<div class="ccp-dropdown" style="display:none;position:fixed;">' +
            '<input type="text" class="ccp-search" placeholder="Search..." autocomplete="off" />' +
            '<ul></ul>' +
            '</div>'
        ).appendTo('body');

        var $search = $dropdown.find('.ccp-search');
        var $list = $dropdown.find('ul');

        function positionDropdown() {
            var rect = document.getElementById('ccpSelected_WS').getBoundingClientRect();
            $dropdown.css({ top: rect.bottom, left: rect.left, width: Math.max(rect.width, 240) });
        }

        function buildList(filter) {
            filter = (filter || '').toLowerCase();
            $list.empty();
            var subset = filter
                ? $.grep(data, function (c) {
                    var name = isArabic ? c.nameAr : c.nameEn;
                    return c.code.toLowerCase().indexOf(filter) > -1 ||
                        name.toLowerCase().indexOf(filter) > -1;
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

        $('#ccpSelected_WS').on('click', function (e) {
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
            } else if (e.key === 'Enter') {
                e.preventDefault();
                if ($active.length) $active.trigger('click');
            } else if (e.key === 'Escape') {
                $dropdown.hide();
            }
        });

        $(document).on('click', function (e) {
            if (!$picker.is(e.target) && !$picker.has(e.target).length &&
                !$dropdown.is(e.target) && !$dropdown.has(e.target).length)
                $dropdown.hide();
        });

        // Init with KSA default
        buildList('');

        // ── Restore state after postback (e.g. failed captcha / validation) ──────
        var existingCode = $hfCode.val();
        if (existingCode) {
            $display.text(existingCode);   // restore visible label; hidden field value already set
        } else {
            // Fresh page load — default to KSA (+966)
            var ksa = data.filter(function (c) { return c.code === '966'; })[0];
            if (ksa) applyCode(ksa.code);
        }
    });
    </script>


</asp:Panel>
