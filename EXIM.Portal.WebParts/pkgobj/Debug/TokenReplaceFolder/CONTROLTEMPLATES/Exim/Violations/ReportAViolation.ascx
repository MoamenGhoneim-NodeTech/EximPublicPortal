<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportAViolation.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.Violations.ReportAViolation" %>

<div class="row mb-4">
    <div class="col-12">
        <customtags:labelmessage runat="server" id="ucMessage" />
    </div>
</div>

<asp:Panel runat="server" ID="pnlFormBody">

    <div class="custom-form mt-5">
        <div class="row g-4">

            <!-- Violation Type -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblViolationType" CssClass="" meta:resourcekey="lblViolationType" AssociatedControlID="ddlViolationType" />
                    <asp:DropDownList runat="server" ID="ddlViolationType" CssClass="form-control" meta:resourcekey="ddlViolationType" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvViolationType" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="ddlViolationType" Display="Dynamic" InitialValue="-1" />
                </div>
            </div>

            <!-- Can Identify Parties -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblCanIdentifyParties" CssClass="" meta:resourcekey="lblCanIdentifyParties" AssociatedControlID="rblCanIdentifyParties" />
                    <asp:RadioButtonList runat="server" ID="rblCanIdentifyParties" CssClass="form-radios-group" meta:resourcekey="rblCanIdentifyParties" RepeatDirection="Horizontal" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvCanIdentifyParties" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="rblCanIdentifyParties" Display="Dynamic" />
                </div>
            </div>

            <!-- Violation Details -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblViolationDetails" CssClass="" meta:resourcekey="lblViolationDetails" AssociatedControlID="txtViolationDetails" />
                    <asp:TextBox runat="server" ID="txtViolationDetails" CssClass="form-control" meta:resourcekey="txtViolationDetails" TextMode="MultiLine" Rows="4" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvViolationDetails" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtViolationDetails" Display="Dynamic" />
                </div>
            </div>

            <!-- Relation -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="120">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblRelation" CssClass="" meta:resourcekey="lblRelation" AssociatedControlID="txtRelation" />
                    <asp:TextBox runat="server" ID="txtRelation" CssClass="form-control" meta:resourcekey="txtRelation" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvRelation" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtRelation" Display="Dynamic" />
                </div>
            </div>

            <!-- Name -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="140">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblName" CssClass="" meta:resourcekey="lblName" AssociatedControlID="txtName" />
                    <asp:TextBox runat="server" ID="txtName" CssClass="form-control" meta:resourcekey="txtName" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvName" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtName" Display="Dynamic" />
                </div>
            </div>

            <!-- Job Title -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblJobTitle" CssClass="" meta:resourcekey="lblJobTitle" AssociatedControlID="txtJobTitle" />
                    <asp:TextBox runat="server" ID="txtJobTitle" CssClass="form-control" meta:resourcekey="txtJobTitle" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvJobTitle" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtJobTitle" Display="Dynamic" />
                </div>
            </div>

            <!-- Company -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblCompany" CssClass="" meta:resourcekey="lblCompany" AssociatedControlID="txtCompany" />
                    <asp:TextBox runat="server" ID="txtCompany" CssClass="form-control" meta:resourcekey="txtCompany" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvCompany" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtCompany" Display="Dynamic" />
                </div>
            </div>

            <!-- Violation Date (Approximate) -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="Label1" CssClass="" meta:resourcekey="lblViolationDate" AssociatedControlID="dtViolationDate" />
                    <SharePoint:DateTimeControl runat="server" ID="dtViolationDate" CssClassTextBox="form-control" meta:resourcekey="txtViolationDate" DateOnly="true" />
                    <asp:CustomValidator runat="server" ID="cvViolationDate" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" OnServerValidate="cvViolationDate_ServerValidate" Display="Dynamic" />
                </div>
            </div>

            <!-- Is Violation Ongoing -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblViolationOngoing" CssClass="" meta:resourcekey="lblViolationOngoing" AssociatedControlID="rblViolationOngoing" />
                    <asp:RadioButtonList runat="server" ID="rblViolationOngoing" CssClass="form-radios-group" meta:resourcekey="rblViolationOngoing" RepeatDirection="Horizontal" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvViolationOngoing" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="rblViolationOngoing" Display="Dynamic" />
                </div>
            </div>

            <!-- How Did You Know -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblHowYouKnow" CssClass="" meta:resourcekey="lblHowYouKnow" AssociatedControlID="ddlHowYouKnow" />
                    <asp:DropDownList runat="server" ID="ddlHowYouKnow" CssClass="form-control" meta:resourcekey="ddlHowYouKnow" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvHowYouKnow" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="ddlHowYouKnow" Display="Dynamic" InitialValue="-1" />
                </div>
            </div>

            <!-- Supporting Documents -->
            <div class="col-md-12">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblSupportingDocuments" CssClass="" meta:resourcekey="lblSupportingDocuments" />
                    <div class="file-upload">
                        <div class="ic-upload"></div>
                        <h5><asp:Literal runat="server" meta:resourcekey="fileUploadTitle"></asp:Literal></h5>
                        <p><asp:Literal runat="server" meta:resourcekey="fileUploadDescription"></asp:Literal></p>
                        <div><strong><asp:Literal runat="server" meta:resourcekey="fileUploadBrowseText"></asp:Literal></strong></div>
                        <asp:FileUpload runat="server" ID="fuSupportingDocuments" CssClass="" />
                    </div>
                </div>
            </div>

            <!-- Anonymous? -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblAnonymous" CssClass="" meta:resourcekey="lblAnonymous" AssociatedControlID="rblAnonymous" />
                    <asp:RadioButtonList runat="server" ID="rblAnonymous" CssClass="form-radios-group" meta:resourcekey="rblAnonymous" RepeatDirection="Horizontal" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvAnonymous" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="rblAnonymous" Display="Dynamic" />
                </div>
            </div>

            <!-- Captcha -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <customtags:captcha runat="server" id="captcha" validationgroup="submit" />
                </div>
            </div>

            <!-- Submit -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <i class="fas fa-spinner fa-spin d-none"></i>
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-secondary" meta:resourcekey="btnSubmit" ValidationGroup="submit" OnClick="btnSubmit_Click" />
                </div>
            </div>

        </div>
    </div>

    <script>
        $(document).ready(function () {

            var dragDropText = '<%= GetLocalResourceObject("fileUploadTitle.Text") %>';
            var descText = '<%= GetLocalResourceObject("fileUploadDescription.Text") %>';
            var browseText = '<%= GetLocalResourceObject("fileUploadBrowseText.Text") %>';
            var deleteText = '<%= GetLocalResourceObject("fileUploadDeleteText.Text") %>';

            var fileInputSelector = "#<%= fuSupportingDocuments.ClientID %>";

            $(document).on("change", fileInputSelector, function () {
                var file = this.files[0];
                if (!file) return;

                var fileName = file.name;
                var wrapper = $(this).closest(".file-upload");

                wrapper.html(
                    '<div class="uploaded-file-preview d-flex align-items-center">' +
                        '<span class="file-name">' + fileName + '</span>' +
                        '<button type="button" class="btn btn-sm btn-danger remove-file ms-3">' + deleteText + '</button>' +
                    '</div>'
                );
            });

            $(document).on("click", ".remove-file", function () {
                var wrapper = $(this).closest(".file-upload");

                wrapper.html(
                    '<div class="ic-upload"></div>' +
                    '<h5>' + dragDropText + '</h5>' +
                    '<p>' + descText + '</p>' +
                    '<div><strong>' + browseText + '</strong></div>' +
                    '<input type="file" id="<%= fuSupportingDocuments.ClientID %>" name="<%= fuSupportingDocuments.UniqueID %>" />'
                    );
            });

        });
    </script>
</asp:Panel>
