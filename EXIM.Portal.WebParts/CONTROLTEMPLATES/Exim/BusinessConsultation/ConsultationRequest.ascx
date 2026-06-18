<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsultationRequest.ascx.cs" Inherits="EXIM.Portal.WebParts.BusinessConsultation.ConsultationRequest" %>

<div class="row mb-4">
    <div class="col-12">
        <customtags:labelmessage runat="server" id="ucMessage" />
    </div>
</div>

<asp:Panel runat="server" ID="pnlFormBody">
    <section class="business-consultation-section">
        <div class="container">
            <div class="custom-form">
                <div class="row g-4">

                    <!-- Title => اسم الشركة -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" AssociatedControlID="txtCompanyName" meta:resourcekey="lblCompanyName">اسم الشركة:</asp:Label>
                            <asp:TextBox runat="server" ID="txtCompanyName" CssClass="form-control" MaxLength="255" meta:resourcekey="txtCompanyName" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvCompanyName" ControlToValidate="txtCompanyName"
                                ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
                                meta:resourcekey="RequiredField" />
                        </div>
                    </div>

                    <!-- ComRegNumber => رقم السجل التجاري -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="120">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" AssociatedControlID="txtCommNumber" meta:resourcekey="lblCommNumber">رقم السجل التجاري:</asp:Label>
                            <asp:TextBox runat="server" ID="txtCommNumber" CssClass="form-control" meta:resourcekey="txtCommNumber" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvCommNumber" ControlToValidate="txtCommNumber"
                                ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
                                meta:resourcekey="RequiredField" />
                            <asp:RegularExpressionValidator runat="server" ID="revCommNumber"
    ControlToValidate="txtCommNumber"
    ValidationExpression="^\d+$"
    ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
    meta:resourcekey="InvalidCommNumber" />
                        </div>
                    </div>

                    <!-- ResponsibleName => اسم المسؤول -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="140">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" AssociatedControlID="txtResponsiblePersonName" meta:resourcekey="lblResponsiblePersonName">اسم المسؤول:</asp:Label>
                            <asp:TextBox runat="server" ID="txtResponsiblePersonName" CssClass="form-control" meta:resourcekey="txtResponsiblePersonName" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvResponsiblePersonName" ControlToValidate="txtResponsiblePersonName"
                                ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
                                meta:resourcekey="RequiredField" />
                            <asp:RegularExpressionValidator runat="server" ID="revResponsiblePersonName"
    ControlToValidate="txtResponsiblePersonName"
    ValidationExpression="^[\u0600-\u06FFa-zA-Z\s]+$"
    ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
    meta:resourcekey="InvalidResponsiblePersonName" />
                        </div>
                    </div>

                    <!-- MobileNumber => رقم الجوال (country code + number) -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="150">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" AssociatedControlID="txtMobileNumber" meta:resourcekey="lblMobileNumber">رقم الجوال:</asp:Label>
                            <div class="input-group">
                                <%-- Custom searchable country-code picker --%>
                                <div class="country-code-picker" id="divCCP_CR">
                                    <div class="ccp-selected" id="ccpSelected_CR">
                                        <span id="ccpDisplay_CR">+---</span>
                                        <span class="ccp-arrow">&#9662;</span>
                                    </div>
                                </div>
                                <asp:HiddenField runat="server" ID="hfSelectedCountryCode" />
                                <asp:DropDownList runat="server" ID="ddlCountryCode" style="display:none;" />
                                <asp:TextBox runat="server" ID="txtMobileNumber" CssClass="form-control digits" MaxLength="14" meta:resourcekey="txtMobileNumber" />
                            </div>
                            <asp:RequiredFieldValidator runat="server" ID="rfvMobileNumber" ControlToValidate="txtMobileNumber"
                                ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
                                meta:resourcekey="RequiredField" />
                            <asp:RegularExpressionValidator runat="server" ID="revMobileNumber" ControlToValidate="txtMobileNumber"
                                ValidationExpression="^\d{9,14}$" ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
                                meta:resourcekey="InvalidMobileNumber" />
                        </div>
                    </div>

                    <!-- Email => البريد الإلكتروني -->
                    <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" AssociatedControlID="txtEmail" meta:resourcekey="lblEmail">البريد الإلكتروني:</asp:Label>
                            <asp:TextBox runat="server" ID="txtEmail" TextMode="SingleLine" CssClass="form-control" meta:resourcekey="txtEmail" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvEmail" ControlToValidate="txtEmail"
                                ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
                                meta:resourcekey="RequiredField" />
                            <asp:RegularExpressionValidator runat="server" ID="revEmail" ControlToValidate="txtEmail"
                                ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,24}$"
                                ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
                                meta:resourcekey="InvalidEmail" />
                        </div>
                    </div>

                    <!-- ProductDescription => معلومات عن المنتج -->
                    <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" AssociatedControlID="txtProductDescription" meta:resourcekey="lblProductDescription">معلومات عن المنتج:</asp:Label>
                            <asp:TextBox runat="server" ID="txtProductDescription" CssClass="form-control" TextMode="MultiLine" Rows="4" MaxLength="1000"
                                meta:resourcekey="txtProductDescription" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvProductDescription" ControlToValidate="txtProductDescription"
                                ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
                                meta:resourcekey="RequiredField" />
                            <asp:RegularExpressionValidator runat="server" ID="revProductDescription"
    ControlToValidate="txtProductDescription"
    ValidationExpression="^[\s\S]{1,1000}$"
    ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
    meta:resourcekey="MaxLengthProductDescription" />
                        </div>
                    </div>

                    <!-- ExportVolumValue => حجم وقيمة الصادرات الحالية -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" AssociatedControlID="txtExportVolumeValue" meta:resourcekey="lblExportVolumeValue">حجم وقيمة الصادرات الحالية:</asp:Label>
                            <asp:TextBox runat="server" ID="txtExportVolumeValue" CssClass="form-control" meta:resourcekey="txtExportVolumeValue" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvExportVolumeValue" ControlToValidate="txtExportVolumeValue"
                                ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
                                meta:resourcekey="RequiredField" />
                        </div>
                    </div>

                    <!-- TargetCountries => الدول المستهدفة للتصدير -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" AssociatedControlID="txtTargetCountries" meta:resourcekey="lblTargetCountries">الدول المستهدفة للتصدير:</asp:Label>
                            <asp:TextBox runat="server" ID="txtTargetCountries" CssClass="form-control" meta:resourcekey="txtTargetCountries" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvTargetCountries" ControlToValidate="txtTargetCountries"
                                ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
                                meta:resourcekey="RequiredField" />
                        </div>
                    </div>

                    <!-- CurrentEximCustomer => هل أنت عميل حالي ... (Yes/No) -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" AssociatedControlID="rblCurrentCustomer" meta:resourcekey="lblCurrentEximCustomer">هل أنت عميل حالي لدى بنك التصدير والاستيراد السعودي:</asp:Label>
                            <asp:RadioButtonList runat="server" ID="rblCurrentCustomer" CssClass="form-radios-group" RepeatDirection="Horizontal">
                                <asp:ListItem Text="نعم" Value="true" meta:resourcekey="rblCurrentCustomerYes" />
                                <asp:ListItem Text="لا" Value="false" meta:resourcekey="rblCurrentCustomerNo" />
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvCurrentCustomer" ControlToValidate="rblCurrentCustomer"
                                InitialValue="" ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
                                meta:resourcekey="RequiredField" />
                        </div>
                    </div>

                    <!-- Captcha (same pattern as old form) -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                        <div class="form-group">
                            <customtags:captcha runat="server" validationgroup="submit" id="captcha" />
                        </div>
                    </div>

                    <!-- Submit -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                        <div class="form-group">
                            <i class="fas fa-spinner fa-spin d-none"></i>
                            <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-secondary"
                                meta:resourcekey="btnSubmit" ValidationGroup="submit"
                                OnClick="btnSubmit_Click" Text="إرسال" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </section>
    <script>
    $(document).ready(function () {
        var data     = <%= CountriesJson %>;
        var isArabic = <%= IsArabic ? "true" : "false" %>;
        var $hfCode  = $('#<%= hfSelectedCountryCode.ClientID %>');
        var $picker  = $('#divCCP_CR');
        var $display = $('#ccpDisplay_CR');

        var $dropdown = $(
            '<div class="ccp-dropdown" style="display:none;position:fixed;">' +
                '<input type="text" class="ccp-search" placeholder="Search..." autocomplete="off" />' +
                '<ul></ul>' +
            '</div>'
        ).appendTo('body');

        var $search = $dropdown.find('.ccp-search');
        var $list   = $dropdown.find('ul');

        function positionDropdown() {
            var rect = document.getElementById('ccpSelected_CR').getBoundingClientRect();
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

        $('#ccpSelected_CR').on('click', function (e) {
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

        // txtResponsiblePersonName — letters (Arabic + English) and spaces only
        $('#<%= txtResponsiblePersonName.ClientID %>').on('input', function () {
    $(this).val($(this).val().replace(/[^\u0600-\u06FFa-zA-Z\s]/g, ''));
});

// txtCommNumber — digits only
        $('#<%= txtCommNumber.ClientID %>').on('input', function () {
            $(this).val($(this).val().replace(/\D/g, ''));
        });
    });
    </script>

</asp:Panel>
