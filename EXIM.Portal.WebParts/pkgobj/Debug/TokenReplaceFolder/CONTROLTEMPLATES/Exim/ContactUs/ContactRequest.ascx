<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
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
                    <asp:Label runat="server" ID="lblCountry" meta:resourcekey="lblCountry" AssociatedControlID="txtCountry" />
                    <asp:TextBox runat="server" ID="txtCountry" CssClass="form-control" meta:resourcekey="txtCountry" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvCountry" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtCountry" Display="Dynamic" />
                </div>
            </div>

            <!-- Mobile Number (country code + number) -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="150">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblMobileNumber" meta:resourcekey="lblMobileNumber" AssociatedControlID="txtMobileNumber" />
                    <div class="input-group">
                        <asp:DropDownList runat="server" ID="ddlCountryCode" CssClass="country-code" meta:resourcekey="ddlCountryCode"></asp:DropDownList>
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
                    <asp:Label runat="server" ID="lblEmail" meta:resourcekey="lblEmail" AssociatedControlID="txtEmail" />
                    <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" meta:resourcekey="txtEmail" ValidationGroup="" TextMode="Email" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvEmail" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtEmail" Display="Dynamic" />
                    <asp:RegularExpressionValidator runat="server" ID="revEmail" CssClass="text-danger" meta:resourcekey="InvalidEmail" ValidationGroup="submit" ControlToValidate="txtEmail" ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,24}$" Display="Dynamic" />
                </div>
            </div>

            <!-- Message -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblMessage" meta:resourcekey="lblMessage" AssociatedControlID="txtMessage" />
                    <asp:TextBox runat="server" ID="txtMessage" CssClass="form-control limit-1000" meta:resourcekey="txtMessage" TextMode="MultiLine" Rows="4" MaxLength="1000"/>
                    <asp:RequiredFieldValidator runat="server" ID="rfvMessage" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtMessage" Display="Dynamic" />
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

            <!-- Captcha -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <customtags:captcha runat="server" ID="captcha" validationgroup="submit" />
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
    <script>
        $(document).ready(function () {
            $('.limit-1000').on('input', function () {
                var max = 1000;
                if ($(this).val().length > max) {
                    $(this).val($(this).val().substring(0, max));
                }
            });
        });
    </script>
</asp:Panel>
