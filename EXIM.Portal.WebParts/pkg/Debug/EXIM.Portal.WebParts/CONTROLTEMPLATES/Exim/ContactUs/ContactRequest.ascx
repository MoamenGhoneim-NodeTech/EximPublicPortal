<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactRequest.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.ContactUs.ContactRequest" %>


<div class="row mb-4">
    <div class="col-12">
        <customtags:labelmessage runat="server" id="ucMessage" />
    </div>
</div>

<asp:Panel runat="server" ID="pnlFormBody">
    <div class="custom-form">
        <div class="row g-4">

            <!-- Sender Name -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblSenderName" meta:resourcekey="lblSenderName" AssociatedControlID="txtSenderName" />
                    <asp:TextBox runat="server" ID="txtSenderName" CssClass="form-control" meta:resourcekey="txtSenderName" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvSenderName" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtSenderName" Display="Dynamic" />
                </div>
            </div>

            <!-- Entity / Company Name -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="120">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblEntityName" meta:resourcekey="lblEntityName" AssociatedControlID="txtEntityName" />
                    <asp:TextBox runat="server" ID="txtEntityName" CssClass="form-control" meta:resourcekey="txtEntityName" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvEntityName" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtEntityName" Display="Dynamic" />
                </div>
            </div>

            <!-- Country -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="140">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblCountry" meta:resourcekey="lblCountry" />

                    <%-- Searchable country dropdown (same pattern as code picker) --%>
                    <div class="country-picker " id="divCountryPicker">
                        <div class="cp-selected form-control" id="cpSelected" >
                            <span id="cpDisplay" class="cp-placeholder"> <%= GetLocalResourceObject("cpDisplayPlaceHolder") %>  </span>
                            <span class="ccp-arrow">&#9662;</span>
                        </div>
                    </div>

                    <%-- Hidden fields: country name (for saving) and ID (for reference) --%>
                    <asp:HiddenField runat="server" ID="txtCountry" />
                    <asp:HiddenField runat="server" ID="hfCountryId" />
                    <asp:TextBox runat="server" ID="dtxtCountry"  CssClass="HiddenTxtbox" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvCountry" CssClass="text-danger"
                        meta:resourcekey="RequiredField" ValidationGroup="submit"
                        ControlToValidate="dtxtCountry" Display="Dynamic" />
                </div>
            </div>

            <!-- Mobile Number (country code + number) -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="150">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblMobileNumber" meta:resourcekey="lblMobileNumber"
                        AssociatedControlID="txtMobileNumber" />
                    <div class="input-group">

                        <%-- Custom searchable country-code picker --%>
                        <%-- Dropdown is appended to <body> via JS to escape overflow:hidden parents --%>
                        <div class="country-code-picker" id="divCCP">
                            <div class="ccp-selected" id="ccpSelected">
                                <span id="ccpDisplay">+---</span>
                                <span class="ccp-arrow">&#9662;</span>
                            </div>
                        </div>

                        <%-- Hidden field: holds the selected code (+93 etc.) submitted with the form --%>
                        <asp:HiddenField runat="server" ID="hfSelectedCountryCode" />

                        <%-- ddlCountryCode kept hidden so existing server-side references compile --%>
                        <asp:DropDownList runat="server" ID="ddlCountryCode" style="display:none;" />

                        <asp:TextBox runat="server" ID="txtMobileNumber" CssClass="form-control digits"
                            meta:resourcekey="txtMobileNumber" MaxLength="14" />
                    </div>
                    <asp:RequiredFieldValidator runat="server" ID="rfvMobileNumber" CssClass="text-danger"
                        meta:resourcekey="RequiredField" ValidationGroup="submit"
                        ControlToValidate="txtMobileNumber" Display="Dynamic" />
                    <asp:RegularExpressionValidator runat="server" ID="revMobileNumber" CssClass="text-danger"
                        meta:resourcekey="InvalidMobileNumber" ValidationGroup="submit"
                        ControlToValidate="txtMobileNumber" ValidationExpression="^\d{9,14}$" Display="Dynamic" />
                </div>
            </div>

            <!-- Request Type -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblRequestType" meta:resourcekey="lblRequestType" AssociatedControlID="ddlRequestType" />
                    <asp:DropDownList runat="server" ID="ddlRequestType" meta:resourcekey="ddlRequestType" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvRequestType" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="ddlRequestType" InitialValue="-1" Display="Dynamic" />
                </div>
            </div>

            <!-- Message Title -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblMessageTitle" meta:resourcekey="lblMessageTitle" AssociatedControlID="txtMessageTitle" />
                    <asp:TextBox runat="server" ID="txtMessageTitle" CssClass="form-control" meta:resourcekey="txtMessageTitle" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvMessageTitle" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtMessageTitle" Display="Dynamic" />
                </div>
            </div>

            <!-- Message -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblMessage" meta:resourcekey="lblMessage" AssociatedControlID="txtMessage" />
                    <asp:TextBox runat="server" ID="txtMessage" CssClass="form-control limit-1000" meta:resourcekey="txtMessage" TextMode="MultiLine" Rows="4" MaxLength="1000" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvMessage" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtMessage" Display="Dynamic" />
                </div>
            </div>

            <!-- Email -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblEmail" meta:resourcekey="lblEmail" AssociatedControlID="txtEmail" />
                    <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" meta:resourcekey="txtEmail" ValidationGroup="" TextMode="Email" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvEmail" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtEmail" Display="Dynamic" />
                    <asp:RegularExpressionValidator runat="server" ID="revEmail" CssClass="text-danger" meta:resourcekey="InvalidEmail" ValidationGroup="submit" ControlToValidate="txtEmail" ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,24}$" Display="Dynamic" />
                </div>
            </div>

            <!-- ID Number -->
            <div class="col-md-6" id="divIDNumber" runat="server" style="display: none;" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblIDNmber" meta:resourcekey="lblIDNmber" AssociatedControlID="txtIDNumber" />
                    <asp:TextBox runat="server" ID="txtIDNumber" CssClass="form-control" meta:resourcekey="txtIDNumber" />
                    <asp:RequiredFieldValidator
                        runat="server"
                        ID="rfvIDNumber"
                        CssClass="text-danger"
                        Enabled="false"
                        meta:resourcekey="RequiredField"
                        ValidationGroup="submit"
                        ControlToValidate="txtIDNumber"
                        Display="Dynamic" />
                </div>
            </div>

            <!-- Captcha -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <customtags:captcha runat="server" id="captcha" validationgroup="submit" />
                </div>
            </div>

            <!-- Submit -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <i class="fas fa-spinner fa-spin d-none"></i>
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-secondary" meta:resourcekey="btnSubmit" ValidationGroup="submit" OnClick="btnSubmit_Click" />
                </div>
            </div>

        </div>
    </div>

    <%-- ── limit-1000 ──────────────────────────────────────────────────────── --%>
    <script>
        $(document).ready(function () {
            $('.limit-1000').on('input', function () {
                if ($(this).val().length > 1000)
                    $(this).val($(this).val().substring(0, 1000));
            });
        });
    </script>

    <%-- ── Request-type rules ──────────────────────────────────────────────── --%>
    <script>
        $(document).ready(function () {
            var defaultMessageLabel = $('#<%= lblMessage.ClientID %>').text();

            function applyRequestTypeRules() {
                var selectedvalue = $('#<%= ddlRequestType.ClientID %> option:selected').val();
                var selectedText = $('#<%= ddlRequestType.ClientID %> option:selected').text();

                if (selectedvalue == '4') {
                    $('#<%= divIDNumber.ClientID %>').show();
                    ValidatorEnable(document.getElementById('<%= rfvIDNumber.ClientID %>'), true);
                    $('#<%= lblMessage.ClientID %>').text("<%= GetLocalResourceObject("PurposeofRequest") %>");
                    $('#<%= txtMessageTitle.ClientID %>').val(selectedText);
                } else if (selectedvalue == '5') {
                    $('#<%= divIDNumber.ClientID %>').hide();
                    ValidatorEnable(document.getElementById('<%= rfvIDNumber.ClientID %>'), false);
                    $('#<%= lblMessage.ClientID %>').text("<%= GetLocalResourceObject("PurposeofRequest") %>");
                    $('#<%= txtMessageTitle.ClientID %>').val(selectedText);
                } else {
                    $('#<%= divIDNumber.ClientID %>').hide();
                    ValidatorEnable(document.getElementById('<%= rfvIDNumber.ClientID %>'), false);
                    $('#<%= lblMessage.ClientID %>').text(defaultMessageLabel);
                }
            }

            applyRequestTypeRules();
            $('#<%= ddlRequestType.ClientID %>').change(function () {
                applyRequestTypeRules();
            });
        });
    </script>

    <%-- ── Country autocomplete + code picker ────────────────────────────── --%>
    <script>
    $(document).ready(function () {
        var data     = <%= CountriesJson %>;
        var isArabic = <%= IsArabic ? "true" : "false" %>;

        var $hfCode = $('#<%= hfSelectedCountryCode.ClientID %>');

        // ── Country-Code Picker ───────────────────────────────────────────────
        var $picker  = $('#divCCP');
        var $display = $('#ccpDisplay');

        // ── Build dropdown and append to <body> to escape any overflow:hidden parent ──
        var $dropdown = $(
            '<div id="ccpDropdown" class="ccp-dropdown" style="display:none;position:fixed;">' +
                '<input type="text" id="ccpSearch" class="ccp-search" placeholder="Search..." autocomplete="off" />' +
                '<ul id="ccpList"></ul>' +
            '</div>'
        ).appendTo('body');

        var $search = $dropdown.find('#ccpSearch');
        var $list   = $dropdown.find('#ccpList');

        function positionDropdown() {
            var rect = document.getElementById('ccpSelected').getBoundingClientRect();
            $dropdown.css({
                top  : rect.bottom,
                left : rect.left,
                width: Math.max(rect.width, 240)
            });
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
                var name = isArabic ? c.nameAr : c.nameEn;
                $('<li>')
                    .attr('data-code', c.code)
                    .attr('data-id',   c.id)
                    .append($('<span class="ccp-li-code">').text(c.code))
                    .append($('<span class="ccp-li-name">').text(name))
                    .appendTo($list);
            });
        }

        function applyCode(code) {
            $display.text(code);
            $hfCode.val(code);
            $dropdown.hide();
            $search.val('');
        }

        // Open / close picker
        $('#ccpSelected').on('click', function (e) {
            e.stopPropagation();
            var opening = !$dropdown.is(':visible');
            if (opening) {
                positionDropdown();
                $dropdown.show();
                buildList('');
                $search.focus();
            } else {
                $dropdown.hide();
            }
        });

        // Reposition on scroll/resize so it tracks the button
        $(window).on('scroll resize', function () {
            if ($dropdown.is(':visible')) positionDropdown();
        });

        // Live filter while typing in search box
        $search.on('input', function () { buildList($(this).val()); });

        // Select item from list
        $list.on('click', 'li', function () {
            applyCode($(this).data('code'));
        });

        // Keyboard navigation inside picker
        $search.on('keydown', function (e) {
            var $items  = $list.find('li');
            var $active = $list.find('li.ccp-active');
            if (e.key === 'ArrowDown') {
                e.preventDefault();
                var $next = $active.length ? $active.removeClass('ccp-active').next('li') : $items.first();
                if ($next.length) $next.addClass('ccp-active')[0].scrollIntoView({ block: 'nearest' });
            } else if (e.key === 'ArrowUp') {
                e.preventDefault();
                var $prev = $active.length ? $active.removeClass('ccp-active').prev('li') : $items.last();
                if ($prev.length) $prev.addClass('ccp-active')[0].scrollIntoView({ block: 'nearest' });
            } else if (e.key === 'Enter') {
                e.preventDefault();
                if ($active.length) $active.trigger('click');
            } else if (e.key === 'Escape') {
                $dropdown.hide();
            }
        });

        // Close picker when clicking outside (exclude both the trigger and the body-level dropdown)
        $(document).on('click', function (e) {
            if (!$picker.is(e.target)   && !$picker.has(e.target).length &&
                !$dropdown.is(e.target) && !$dropdown.has(e.target).length)
                $dropdown.hide();
        });

        // ── Country Searchable Dropdown ───────────────────────────────────────
        var $cpPicker   = $('#divCountryPicker');
        var $cpDisplay  = $('#cpDisplay');
        var $hfCountry  = $('#<%= txtCountry.ClientID %>');
        var $hfId       = $('#<%= hfCountryId.ClientID %>');
        var $dtxtCountry = $('#<%= dtxtCountry.ClientID %>'); 
        // Build country dropdown and append to <body>
        var $cpDropdown = $(
            '<div id="cpDropdown" class="ccp-dropdown" style="display:none;position:fixed;">' +
            '<input type="text" id="cpSearch" class="ccp-search" placeholder="Search..." autocomplete="off" />' +
            '<ul id="cpList"></ul>' +
            '</div>'
        ).appendTo('body');

        var $cpSearch = $cpDropdown.find('#cpSearch');
        var $cpList = $cpDropdown.find('#cpList');

        function positionCpDropdown() {
            var rect = document.getElementById('cpSelected').getBoundingClientRect();
            $cpDropdown.css({
                top: rect.bottom,
                left: rect.left,
                width: Math.max(rect.width, 260)
            });
        }

        function buildCountryList(filter) {
            filter = (filter || '').toLowerCase();
            $cpList.empty();
            var subset = filter
                ? $.grep(data, function (c) {
                    var name = isArabic ? c.nameAr : c.nameEn;
                    return name.toLowerCase().indexOf(filter) > -1;
                })
                : data;

            $.each(subset.slice(0, 100), function (_, c) {
                var name = isArabic ? c.nameAr : c.nameEn;
                $('<li>')
                    .attr('data-name', name)
                    .attr('data-code', c.code)
                    .attr('data-id', c.id)
                    .append($('<span class="cp-li-name">').text(name))
                    .appendTo($cpList);
            });
        }

        function applyCountry(name, code, id) {
            $cpDisplay.text(name).removeClass('cp-placeholder');
            $hfCountry.val(name);
            $dtxtCountry.val(name);
            $hfId.val(id);
            applyCode(code);           // sync the code picker too
            $cpDropdown.hide();
            $cpSearch.val('');
        }

        // Open / close
        $('#cpSelected').on('click', function (e) {
            e.stopPropagation();
            var opening = !$cpDropdown.is(':visible');
            if (opening) {
                positionCpDropdown();
                $cpDropdown.show();
                buildCountryList('');
                $cpSearch.focus();
            } else {
                $cpDropdown.hide();
            }
        });

        // Reposition on scroll/resize
        $(window).on('scroll resize', function () {
            if ($cpDropdown.is(':visible')) positionCpDropdown();
        });

        // Live filter
        $cpSearch.on('input', function () { buildCountryList($(this).val()); });

        // Select item
        $cpList.on('click', 'li', function () {
            applyCountry(
                $(this).data('name'),
                $(this).data('code'),
                $(this).data('id')
            );
        });

        // Keyboard navigation
        $cpSearch.on('keydown', function (e) {
            var $items = $cpList.find('li');
            var $active = $cpList.find('li.ccp-active');
            if (e.key === 'ArrowDown') {
                e.preventDefault();
                var $next = $active.length ? $active.removeClass('ccp-active').next('li') : $items.first();
                if ($next.length) $next.addClass('ccp-active')[0].scrollIntoView({ block: 'nearest' });
            } else if (e.key === 'ArrowUp') {
                e.preventDefault();
                var $prev = $active.length ? $active.removeClass('ccp-active').prev('li') : $items.last();
                if ($prev.length) $prev.addClass('ccp-active')[0].scrollIntoView({ block: 'nearest' });
            } else if (e.key === 'Enter') {
                e.preventDefault();
                if ($active.length) $active.trigger('click');
            } else if (e.key === 'Escape') {
                $cpDropdown.hide();
            }
        });

        // Close on outside click
        $(document).on('click', function (e) {
            if (!$cpPicker.is(e.target) && !$cpPicker.has(e.target).length &&
                !$cpDropdown.is(e.target) && !$cpDropdown.has(e.target).length)
                $cpDropdown.hide();
        });

        // Pre-build list on page load so picker is ready on first open
        buildList('');

        // Set KSA (+966) as the default country code
        var ksaEntry = data.filter(function (c) { return c.code === '966'; })[0];
        if (ksaEntry) applyCode(ksaEntry.code);

    });
    </script>

    <style>
        #dtxtCountry , .HiddenTxtbox
        {
            display:none !important;
        }
        
    </style>

</asp:Panel>
