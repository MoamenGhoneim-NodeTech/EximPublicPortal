<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Workshop_Subscribe.ascx.cs" Inherits="EXIM.Portal.WebParts.Workshop_Subscribe" %>

<div class="row mb-4">
    <div class="col-12">
        <CustomTags:LabelMessage runat="server" id="ucMessage" />
    </div>
</div>
<asp:Panel runat="server" ID="pnlFormBody">
    <h3 data-aos="fade-up" data-aos-delay="100">
        <asp:Literal runat="server" meta:resourcekey="formTitle"></asp:Literal></h3>
    <div class="row g-4">
        <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
            <div class="form-group">
                <span class="text-danger">*</span><asp:Label runat="server" AssociatedControlID="txtCompanyName" meta:resourcekey="lblCompanyName"></asp:Label>
                <asp:TextBox runat="server" ID="txtCompanyName" TextMode="SingleLine" CssClass="form-control" meta:resourcekey="txtCompanyName" MaxLength="255"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtCompanyName" ControlToValidate="txtCompanyName" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="col-md-6" data-aos="fade-up" data-aos-delay="120">
            <div class="form-group">
                <span class="text-danger">*</span><asp:Label runat="server" AssociatedControlID="txtCommNumber" meta:resourcekey="lblCommNumber"></asp:Label>
                <asp:TextBox runat="server" ID="txtCommNumber" TextMode="SingleLine" CssClass="form-control" meta:resourcekey="txtCommNumber"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtCommNumber" ControlToValidate="txtCommNumber" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="col-md-6" data-aos="fade-up" data-aos-delay="140">
            <div class="form-group">
                <span class="text-danger">*</span><asp:Label runat="server" AssociatedControlID="txtResponsiblePersonName" meta:resourcekey="lblResponsiblePersonName"></asp:Label>
                <asp:TextBox runat="server" ID="txtResponsiblePersonName" TextMode="SingleLine" CssClass="form-control" meta:resourcekey="txtResponsiblePersonName"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtResponsiblePersonName" ControlToValidate="txtResponsiblePersonName" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="col-md-6" data-aos="fade-up" data-aos-delay="150">
            <div class="formm-group">
                <span class="text-danger">*</span><asp:Label runat="server" AssociatedControlID="ddlCountryCode" meta:resourcekey="lblMobileNumber"></asp:Label>
                <div class="input-group">
                    <asp:DropDownList runat="server" ID="ddlCountryCode" CssClass="country-code"></asp:DropDownList>
                    <asp:TextBox runat="server" ID="txtMobileNumber" TextMode="SingleLine" CssClass="form-control digits" meta:resourcekey="txtMobileNumber"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtMobileNumber" ControlToValidate="txtMobileNumber" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ID="revtxtMobileNumber" ControlToValidate="txtMobileNumber" ValidationExpression="^\d{9,14}$" meta:resourcekey="InvalidMobileNumber" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
            <div class="form-group">
                <span class="text-danger">*</span><asp:Label runat="server" AssociatedControlID="txtEmail" meta:resourcekey="lblEmail"></asp:Label>
                <asp:TextBox runat="server" ID="txtEmail" TextMode="Email" CssClass="form-control" meta:resourcekey="txtEmail"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtEmail" ControlToValidate="txtEmail" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ID="revtxtEmail" ControlToValidate="txtEmail" ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,24}$" meta:resourcekey="InvalidEmail" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
            </div>
        </div>


        <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
            <div class="form-group">
                <customtags:captcha runat="server" validationgroup="subscribe" id="captcha"></customtags:captcha>
            </div>
        </div>
        <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
            <div class="form-group">
                <i class="fas fa-spinner fa-spin d-none"></i>
                <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-secondary" meta:resourcekey="btnSubmit" ValidationGroup="subscribe" OnClick="btnSubmit_Click"></asp:Button>
            </div>
        </div>
    </div>
</asp:Panel>
