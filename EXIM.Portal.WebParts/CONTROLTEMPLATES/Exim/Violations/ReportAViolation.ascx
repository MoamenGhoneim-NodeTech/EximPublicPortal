<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
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

            <!-- Add Other Type  -->
            <asp:Panel ID="OtherType" runat="server" Style="display: none">
                <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                    <div class="form-group">
                        <span class="text-danger">*</span>
                        <asp:Label runat="server" ID="lblOtherType" CssClass="" meta:resourcekey="lblOtherType" AssociatedControlID="txtOtherType" />
                        <asp:TextBox runat="server" ID="txtOtherType" CssClass="form-control" meta:resourcekey="txtOtherType" TextMode="MultiLine" Rows="4" />
                        <asp:RequiredFieldValidator runat="server" ID="rfvOtherType" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" Enabled="false" ControlToValidate="txtOtherType" Display="Dynamic" />
                    </div>
                </div>
            </asp:Panel>

            <!-- Violation Details -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblViolationDetails" CssClass="" meta:resourcekey="lblViolationDetails" AssociatedControlID="txtViolationDetails" />
                    <asp:TextBox runat="server" ID="txtViolationDetails" CssClass="form-control" meta:resourcekey="txtViolationDetails" TextMode="MultiLine" Rows="4" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvViolationDetails" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtViolationDetails" Display="Dynamic" />
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

            <asp:Panel class="col-md-12" ID="PartiesSection" runat="server" Style="display: none">
                <div class="row">

                    <!-- Relation -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="120">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" ID="lblRelation" CssClass="" meta:resourcekey="lblRelation" AssociatedControlID="rblRelation" />
                            <asp:RadioButtonList runat="server" ID="rblRelation" CssClass="form-radios-group" RepeatDirection="Horizontal" ClientIDMode="Static">
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="AddGroup" ControlToValidate="rblRelation" Display="Dynamic" />

                        </div>
                    </div>

                    <!-- Name -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="140">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" ID="lblName" CssClass="" meta:resourcekey="lblName" AssociatedControlID="txtName" />
                            <asp:TextBox runat="server" ID="txtName" CssClass="form-control" meta:resourcekey="txtName" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvName" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="AddGroup" ControlToValidate="txtName" Display="Dynamic" />
                        </div>
                    </div>

                    <!-- Job Title -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" ID="lblJobTitle" CssClass="" meta:resourcekey="lblJobTitle" AssociatedControlID="txtJobTitle" />
                            <asp:TextBox runat="server" ID="txtJobTitle" CssClass="form-control" meta:resourcekey="txtJobTitle" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvJobTitle" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="AddGroup" ControlToValidate="txtJobTitle" Display="Dynamic" />
                        </div>
                    </div>

                    <!-- Company -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" ID="lblCompany" CssClass="" meta:resourcekey="lblCompany" AssociatedControlID="txtCompany" />
                            <asp:TextBox runat="server" ID="txtCompany" CssClass="form-control" meta:resourcekey="txtCompany" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvCompany" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="AddGroup" ControlToValidate="txtCompany" Display="Dynamic" />
                        </div>
                    </div>


                    <div class="vr-form-actions" style="padding-top:5px;">
                        <asp:Button
                            ID="btnAdd"
                            runat="server"
                            meta:resourcekey="btnAdd"
                            CssClass="vr-btn vr-btn-add btn"
                            ValidationGroup="AddGroup"
                            UseSubmitBehavior="false"
                            OnClientClick="return vrAddEntry();" />
                    </div>

                    <div class="vr-table-wrapper col-md-12">
                        <table class="vr-table" id="PartiesTable" style="display: none">
                            <thead>
                                <tr>
                                    <th>#</th>
                                    <th><%= GetLocalResourceObject("Parties-Relation") %></th>
                                    <th><%= GetLocalResourceObject("Parties-Name") %></th>
                                    <th><%= GetLocalResourceObject("Parties-Job") %></th>
                                    <th><%= GetLocalResourceObject("Parties-Company") %></th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody id="vrTableBody" runat="server">
                                <tr>
                                    <td colspan="6">
                                        <div class="vr-empty-state">
                                            <p>
                                                <%= GetLocalResourceObject("NoItemsAddedMsg") %>
                                            </p>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                </div>
            </asp:Panel>

            <!-- Hidden field to pass parties data to server on postback -->
            <asp:HiddenField runat="server" ID="hfPartiesJson" />

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

            <!-- Add Other Type  -->
            <asp:Panel ID="pnlOther" runat="server" Style="display: none">
                <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
                    <div class="form-group">
                        <span class="text-danger">*</span>
                        <asp:Label runat="server" ID="Label2" CssClass="" meta:resourcekey="lblOtherType" AssociatedControlID="txt_Other" />
                        <asp:TextBox runat="server" ID="txt_Other" CssClass="form-control" meta:resourcekey="txt_Other" TextMode="MultiLine" Rows="4" />
                        <asp:RequiredFieldValidator runat="server" ID="rfvOther" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" Enabled="false" ControlToValidate="txt_Other" Display="Dynamic" />
                    </div>
                </div>
            </asp:Panel>

            <!-- Supporting Documents -->
            <div class="col-md-12">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblSupportingDocuments" CssClass="" meta:resourcekey="lblSupportingDocuments" />
                    <div class="file-upload">
                        <div class="ic-upload"></div>
                        <h5>
                            <asp:Literal runat="server" meta:resourcekey="fileUploadTitle"></asp:Literal>
                        </h5>
                        <p>
                            <asp:Literal runat="server" meta:resourcekey="fileUploadDescription"></asp:Literal>
                        </p>
                        <div>
                            <strong>
                                <asp:Literal runat="server" meta:resourcekey="fileUploadBrowseText"></asp:Literal>
                            </strong>
                        </div>
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
            <asp:Panel class="col-md-12" ID="NotAnonymous" runat="server" Style="display: none">
                <div class="row">

                    <!-- Name -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="140">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" ID="Label3" CssClass="" meta:resourcekey="lblName" AssociatedControlID="txtName" />
                            <asp:TextBox runat="server" ID="txt_Name" CssClass="form-control" meta:resourcekey="txtName" />
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" CssClass="text-danger" Enabled="false" meta:resourcekey="RequiredField" ValidationGroup="AddGroup" ControlToValidate="txt_Name" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="150">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" ID="lblMobileNumber" meta:resourcekey="lblMobileNumber" AssociatedControlID="txtMobileNumber" />
                            <div class="input-group">
                                <asp:DropDownList runat="server" ID="ddlCountryCode" CssClass="country-code" meta:resourcekey="ddlCountryCode"></asp:DropDownList>
                                <asp:TextBox runat="server" ID="txtMobileNumber" CssClass="form-control digits" meta:resourcekey="txtMobileNumber" MaxLength="14" />
                            </div>
                            <asp:RequiredFieldValidator runat="server" ID="rfvMobileNumber" CssClass="text-danger" Enabled="false" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtMobileNumber" Display="Dynamic" />
                            <asp:RegularExpressionValidator runat="server" ID="revMobileNumber" CssClass="text-danger" Enabled="false" meta:resourcekey="InvalidMobileNumber" ValidationGroup="submit" ControlToValidate="txtMobileNumber" ValidationExpression="^\d{9,14}$" Display="Dynamic" />
                        </div>
                    </div>

                    <!-- Email -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="100">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" ID="lblEmail" meta:resourcekey="lblEmail" AssociatedControlID="txtEmail" />
                            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" meta:resourcekey="txtEmail" ValidationGroup="" TextMode="Email" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvEmail" CssClass="text-danger" meta:resourcekey="RequiredField" Enabled="false" ValidationGroup="submit" ControlToValidate="txtEmail" Display="Dynamic" />
                            <asp:RegularExpressionValidator runat="server" ID="revEmail" CssClass="text-danger" Enabled="false" meta:resourcekey="InvalidEmail" ValidationGroup="submit" ControlToValidate="txtEmail" ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,24}$" Display="Dynamic" />
                        </div>
                    </div>

                </div>
            </asp:Panel>

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
            var descText     = '<%= GetLocalResourceObject("fileUploadDescription.Text") %>';
            var browseText   = '<%= GetLocalResourceObject("fileUploadBrowseText.Text") %>';
            var deleteText   = '<%= GetLocalResourceObject("fileUploadDeleteText.Text") %>';

            var fileInputSelector = "#<%= fuSupportingDocuments.ClientID %>";

            $(document).on("change", fileInputSelector, function () {
                var file = this.files[0];
                if (!file) return;

                var fileName = file.name;
                var wrapper  = $(this).closest(".file-upload");

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


             // Show/hide PartiesSection on change
            var NoValue      = "2";
            var rblSelector  = "input[name='<%= rblCanIdentifyParties.UniqueID %>']";

           
            $(document).on("change", rblSelector, function () {
                if ($(this).val() !== NoValue) {
                    $("#<%= PartiesSection.ClientID %>").show();
                } else {
                    $("#<%= PartiesSection.ClientID %>").hide();
                }
            });

            // Handle initial state on page load (e.g. after postback)
            var selected = $(rblSelector + ":checked").val();
            if (selected !== undefined && selected !== NoValue) {
                $("#<%= PartiesSection.ClientID %>").show();
            } else {
                $("#<%= PartiesSection.ClientID %>").hide();
            }
            var Anonymous = "false"; // ⚠ change this to the actual value of "Yes"
            var rblAnonymousSelector = "input[name='<%= rblAnonymous.UniqueID %>']";
            var notAnonValidatorIds = [
                                        '<%= RequiredFieldValidator3.ClientID %>',
                                        '<%= rfvMobileNumber.ClientID %>',
                                        '<%= revMobileNumber.ClientID %>',
                                        '<%= rfvEmail.ClientID %>',
                                        '<%= revEmail.ClientID %>'
                                      ];
            
           
            function toggleNotAnonValidators(enable) {
                notAnonValidatorIds.forEach(function (id) {
                    var val = document.getElementById(id);
                    if (val) ValidatorEnable(val, enable);
                });
            }
                           
           // Show Hide NotAnonymous section /On change
            $(document).on("change", rblAnonymousSelector, function () {

                if ($(this).val() === Anonymous) {
                    $("#<%= NotAnonymous.ClientID %>").show();
                     toggleNotAnonValidators(true);
                } else {
                    $("#<%= NotAnonymous.ClientID %>").hide();
                     toggleNotAnonValidators(false);
                }
            });

            // Handle initial state after postback
            var selectedAnonymous = $(rblAnonymousSelector + ":checked").val();

            if (selectedAnonymous === Anonymous) {
                $("#<%= NotAnonymous.ClientID %>").show();
                 toggleNotAnonValidators(true);
            } else {
                $("#<%= NotAnonymous.ClientID %>").hide();
                 toggleNotAnonValidators(false);
            }

          var otherValue = "<%= GetLocalResourceObject("OtherViolationType") %>";
            
            var ddlViolationTypeSelector = "#<%= ddlViolationType.ClientID %>";
            var rfvOtherType = document.getElementById('<%= rfvOtherType.ClientID %>');

            function toggleOtherType() {
                var selected = $(ddlViolationTypeSelector).val();
                if (selected === otherValue) {
                    $("#<%= OtherType.ClientID %>").show();
                    if (rfvOtherType) ValidatorEnable(rfvOtherType, true);
                } else {
                    $("#<%= OtherType.ClientID %>").hide();
                    if (rfvOtherType) ValidatorEnable(rfvOtherType, false);
                }
            }

            // On change
            $(document).on("change", ddlViolationTypeSelector, function () {
                toggleOtherType();
            });

            // On page load / postback
            toggleOtherType();

            var HowDoYouKnowotherValue = "<%= GetLocalResourceObject("HowDoYouKnow-Other") %>";
  
          var ddlHowDoYouKnowSelector = "#<%= ddlHowYouKnow.ClientID %>";
          var rfvOther = document.getElementById('<%= rfvOther.ClientID %>');

          function toggleOther() {
              var selected = $(ddlHowDoYouKnowSelector).val();
              if (selected === HowDoYouKnowotherValue) {
                  $("#<%= pnlOther.ClientID %>").show();
                  if (rfvOther) ValidatorEnable(rfvOther, true);
              } else {
                  $("#<%= pnlOther.ClientID %>").hide();
                  if (rfvOther) ValidatorEnable(rfvOther, false);
              }
          }

          // On change
          $(document).on("change", ddlHowDoYouKnowSelector, function () {
              toggleOther();
          });

          // On page load / postback
          toggleOther();


        });




    </script>

    <script type="text/javascript">
    (function () {

        var vrEntries = [];

        // ── Sync entries to hidden field ─────────────────────────
        function vrSyncHiddenField() {
            var hf = document.getElementById('<%= hfPartiesJson.ClientID %>');
            if (hf) hf.value = JSON.stringify(vrEntries);
        }

        // ── Clear form ───────────────────────────────────────────
        window.vrClearForm = function () {
            // Uncheck all radios in rblRelation
            document.querySelectorAll('#rblRelation input[type="radio"]').forEach(function (r) {
                r.checked = false;
            });
            document.getElementById('rblRelation').style.outline = "";

            document.getElementById('<%= txtName.ClientID %>').value    = "";
            document.getElementById('<%= txtJobTitle.ClientID %>').value = "";
            document.getElementById('<%= txtCompany.ClientID %>').value  = "";

            document.querySelectorAll('.vr-input.error').forEach(function (el) {
                el.classList.remove('error');
            });
        };

        // ── Add entry ────────────────────────────────────────────
        window.vrAddEntry = function () {
            var hasError = false;

            // Read selected relation from rblRelation RadioButtonList
            var selectedRelationInput = document.querySelector('#rblRelation input[type="radio"]:checked');
            var vrSelectedRelation    = selectedRelationInput ? selectedRelationInput.value : "";

            var name    = document.getElementById('<%= txtName.ClientID %>').value.trim();
            var job     = document.getElementById('<%= txtJobTitle.ClientID %>').value.trim();
            var company = document.getElementById('<%= txtCompany.ClientID %>').value.trim();

           
            if (!Page_ClientValidate('AddGroup'))
            {  hasError = true;
            }

            if (hasError) {
                vrShowToast('<%= GetLocalResourceObject("InvalidInput") %>', "error"); 
                return;
            }

            vrEntries.push({
                relation: vrSelectedRelation,
                name:     name,
                job:      job,
                company:  company
            });

            vrRenderTable();
            vrClearForm();
            vrSyncHiddenField();
            vrShowToast('<%= GetLocalResourceObject("SavedSuccessfully") %>', "success"); 
        };

        // ── Remove entry ─────────────────────────────────────────
        window.vrRemoveEntry = function (index) {
            vrEntries.splice(index, 1);
            vrRenderTable();
            vrSyncHiddenField();
        };

        // ── Render table ─────────────────────────────────────────
        var vrTableBodyId = '<%= vrTableBody.ClientID %>';
        var table = document.getElementById('PartiesTable'); 

        function vrRenderTable() {
            var tbody = document.getElementById(vrTableBodyId);
            tbody.innerHTML = "";

            if (vrEntries.length === 0) {

                 if (table) table.style.display = 'none'
                tbody.innerHTML =
                    '<tr><td colspan="6">' +
                    '<div class="vr-empty-state">' +
                    '<svg viewBox="0 0 24 24" fill="currentColor"><path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-7 14l-5-5 1.41-1.41L12 14.17l7.59-7.59L21 8l-9 9z"/></svg>' +
                    '<p>' + '<%= GetLocalResourceObject("NoItemsAddedMsg") %>' + '</p>' +
                    '</div></td></tr>';
            } else {
                    if (table) table.style.display = '';  
                vrEntries.forEach(function (e, i) {
                    var bc = "vr-badge-unknown";
                    tbody.innerHTML +=
                        '<tr>' +
                        '<td>' + (i + 1) + '</td>' +
                        '<td><span class="vr-badge ' + bc + '">' + e.relation + '</span></td>' +
                        '<td>' + (e.name    || '—') + '</td>' +
                        '<td>' + (e.job     || '—') + '</td>' +
                        '<td>' + (e.company || '—') + '</td>' +
                        '<td>' +
                            '<button type="button" class="vr-btn-delete" onclick="vrRemoveEntry(' + i + ')">' +
                            '<svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor">' +
                            '<path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM15.5 4l-1-1h-5l-1 1H5v2h14V4z"/>' +
                            '</svg></button>' +
                        '</td></tr>';
                });
            }

        }

    })();
    </script>

</asp:Panel>
