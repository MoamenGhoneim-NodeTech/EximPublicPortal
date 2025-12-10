<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadRequest.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.GuidForm.DownloadRequest" %>

<div class="row mb-4">
    <div class="col-12">
        <customtags:labelmessage runat="server" ID="ucMessage" />
    </div>
</div>

<asp:Panel runat="server" ID="pnlFormBody">

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
                    <asp:TextBox runat="server" ID="txtCompanyName" CssClass="form-control" meta:resourcekey="txtCompanyName" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvCompanyName" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtCompanyName" Display="Dynamic" />
                </div>
            </div>

            <!-- Beneficiary Name -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="120">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblBeneficiaryName" CssClass="" meta:resourcekey="lblBeneficiaryName" AssociatedControlID="txtBeneficiaryName" />
                    <asp:TextBox runat="server" ID="txtBeneficiaryName" CssClass="form-control" meta:resourcekey="txtBeneficiaryName" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvBeneficiaryName" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtBeneficiaryName" Display="Dynamic" />
                </div>
            </div>

            <!-- City -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="140">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblCity" CssClass="" meta:resourcekey="lblCity" AssociatedControlID="txtCity" />
                    <asp:TextBox runat="server" ID="txtCity" CssClass="form-control" meta:resourcekey="txtCity" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvCity" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtCity" Display="Dynamic" />
                </div>
            </div>

            <!-- Mobile Number -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="150">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblMobileNumber" CssClass="" meta:resourcekey="lblMobileNumber" AssociatedControlID="txtMobileNumber" />
                    <div class="input-group">
                        <asp:DropDownList runat="server" ID="ddlCountryCode" CssClass="country-code" meta:resourcekey="ddlCountryCode">
                        </asp:DropDownList>
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
                    <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" meta:resourcekey="txtEmail" TextMode="Email" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvEmail" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtEmail" Display="Dynamic" />
                    <asp:RegularExpressionValidator runat="server" ID="revEmail" CssClass="text-danger" meta:resourcekey="InvalidEmail" ValidationGroup="submit" ControlToValidate="txtEmail" ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,24}$" Display="Dynamic" />
                </div>
            </div>

            <!-- Captcha -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblCaptcha" CssClass="d-flex" meta:resourcekey="lblCaptcha" />
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

</asp:Panel>

<!-- Success panel (shown after submit) -->
<asp:Panel runat="server" ID="pnlSuccess" CssClass="guide-form" Visible="false">
    <section class="guide-ads">
        <div class="container">
            <div class="guide-ads--container">
                <h3><asp:Literal runat="server" ID="litGuideSuccessSmallTitle" meta:resourcekey="GuideSuccessSmallTitle" /></h3>
                <h2><asp:Literal runat="server" ID="litGuideSuccessTitle" meta:resourcekey="GuideSuccessTitle" /></h2>
                <p><asp:Literal runat="server" ID="litGuideSuccessDescription" meta:resourcekey="GuideSuccessDescription" /></p>

                <div class="guide-action">
                    <asp:HyperLink runat="server" ID="lnkDownloadGuide" CssClass="btn btn-secondary" meta:resourcekey="GuideDownloadLink" />
                </div>
            </div>
        </div>
    </section>
</asp:Panel>
