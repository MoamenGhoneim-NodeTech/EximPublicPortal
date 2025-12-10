<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
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
                        </div>
                    </div>

                    <!-- MobileNumber => رقم الجوال (country code + number) -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="150">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" AssociatedControlID="txtMobileNumber" meta:resourcekey="lblMobileNumber">رقم الجوال:</asp:Label>
                            <div class="input-group">
                                <asp:DropDownList runat="server" ID="ddlCountryCode" CssClass="country-code"></asp:DropDownList>
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
                            <asp:TextBox runat="server" ID="txtEmail" TextMode="Email" CssClass="form-control" meta:resourcekey="txtEmail" />
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
                            <asp:TextBox runat="server" ID="txtProductDescription" CssClass="form-control" TextMode="MultiLine" Rows="4" meta:resourcekey="txtProductDescription" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvProductDescription" ControlToValidate="txtProductDescription"
                                ValidationGroup="submit" CssClass="text-danger" Display="Dynamic"
                                meta:resourcekey="RequiredField" />
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
</asp:Panel>
