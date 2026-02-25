<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvitationForm.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.InvitationForm.InvitationForm" %>
test


<%= GetLocalResourceObject("lblCompanyName") %>



<div class="row mb-4">
    <div class="col-12">
        <customtags:labelmessage runat="server" id="ucMessage" />
    </div>
</div>
<asp:Panel runat="server" ID="pnlFormBody">
    <h3 data-aos="fade-up" data-aos-delay="100">
        <asp:Literal runat="server" meta:resourcekey="formTitle"></asp:Literal></h3>
    <div class="row g-4">
        <div class="col-md-6" data-aos="fade-up" data-aos-delay="140">
            <div class="form-group">
                <span class="text-danger">*</span>
                <asp:Label runat="server" AssociatedControlID="txtName" meta:resourcekey="lblName"></asp:Label>
                <asp:TextBox runat="server" ID="txtName" TextMode="SingleLine" CssClass="form-control" meta:resourcekey="txtName"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtName" ControlToValidate="txtName" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic">

                </asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
            <div class="form-group">
                <span class="text-danger">*</span><asp:Label runat="server" AssociatedControlID="txtEmail" meta:resourcekey="lblEmail"></asp:Label>
                <asp:TextBox runat="server" ID="txtEmail" TextMode="Email" CssClass="form-control" meta:resourcekey="txtEmail"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtEmail" ControlToValidate="txtEmail" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ID="revtxtEmail" ControlToValidate="txtEmail" ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,24}$" meta:resourcekey="InvalidEmail" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="col-md-4" data-aos="fade-up" data-aos-delay="150">
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


        <div class="col-md-4" data-aos="fade-up" data-aos-delay="100">
            <div class="form-group">
                <span class="text-danger">*</span><asp:Label runat="server" AssociatedControlID="txtCompanyName" meta:resourcekey="lblCompanyName"></asp:Label>
                <asp:TextBox runat="server" ID="txtCompanyName" TextMode="SingleLine" CssClass="form-control" meta:resourcekey="txtCompanyName" MaxLength="255"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtCompanyName" ControlToValidate="txtCompanyName" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
        </div>



        <div class="col-md-4" data-aos="fade-up" data-aos-delay="100">
            <div class="form-group">
                <span class="text-danger">*</span><asp:Label runat="server" AssociatedControlID="txtJobTitle" meta:resourcekey="lblJobTitle"></asp:Label>
                <asp:TextBox runat="server" ID="txtJobTitle" TextMode="SingleLine" CssClass="form-control" meta:resourcekey="txtJobTitle" MaxLength="255"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtJobTitle" meta:resourcekey="RequiredField" ValidationGroup="subscribe" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
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
