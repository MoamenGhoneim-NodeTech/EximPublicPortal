<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportAViolation.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.Violations.ReportAViolation" %>
<style>
    /* SharePoint DatePicker */
    .ms-datepicker,
    .ms-dtinput,
    .ms-cal,
    .ui-datepicker,
    iframe[id*="DatePicker"],
    div[id*="DatePicker"] {
        z-index: 999999 !important;
    }
    /* File upload error message */
    #fileUploadError {
        display: none;
        color: #dc3545 !important;
        font-size: 14px;
    }
    #fileUploadError[style*="block"] {
        display: block !important;
    }
</style>
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
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="100">
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
                            <asp:Label runat="server" ID="lblRelation" CssClass="" meta:resourcekey="lblRelation" AssociatedControlID="ddlRelation" />
                            <asp:DropDownList runat="server" ID="ddlRelation" CssClass="form-control" RepeatDirection="Horizontal" ClientIDMode="Static">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="AddGroup" ControlToValidate="ddlRelation" Display="Dynamic" />

                        </div>
                    </div>

                    <!-- Name -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="120">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" ID="lblName" CssClass="" meta:resourcekey="lblName" AssociatedControlID="txtName" />
                            <asp:TextBox runat="server" ID="txtName" CssClass="form-control" meta:resourcekey="txtName" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvName" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="AddGroup" ControlToValidate="txtName" Display="Dynamic" />
                        </div>
                    </div>

                    <!-- Job Title -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="120">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblJobTitle" CssClass="" meta:resourcekey="lblJobTitle" AssociatedControlID="txtJobTitle" />
                            <asp:TextBox runat="server" ID="txtJobTitle" CssClass="form-control" meta:resourcekey="txtJobTitle" />
                        </div>
                    </div>

                    <!-- Company -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="120">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblCompany" CssClass="" meta:resourcekey="lblCompany" AssociatedControlID="txtCompany" />
                            <asp:TextBox runat="server" ID="txtCompany" CssClass="form-control" meta:resourcekey="txtCompany" />
                        </div>
                    </div>


                    <div class="vr-form-actions" style="padding-top: 5px;">
                        <asp:Button
                            ID="btnAdd"
                            runat="server"
                            meta:resourcekey="btnAdd"
                            CssClass="vr-btn vr-btn-add btn"
                            ValidationGroup="AddGroup"
                            UseSubmitBehavior="false"
                            OnClientClick="return vrAddEntry();" />
                        <span id="lblVrMessage" style="display:none; margin-inline-start:10px; font-weight:500;"></span>
                    </div>

                    <div class="vr-table-wrapper col-md-12" id="PartiesDiv" runat="server">
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
            <asp:HiddenField runat="server" ID="hfPartiesHTML" />
            <!-- Violation Date (Approximate) -->
            <div class="col-md-6">
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="Label1" CssClass="" meta:resourcekey="lblViolationDate" AssociatedControlID="dtViolationDate" />
                    <SharePoint:DateTimeControl runat="server" ID="dtViolationDate" CssClassTextBox="form-control"
                        meta:resourcekey="txtViolationDate" DateOnly="true"   IsRequiredField="true"  />
                    <asp:CustomValidator runat="server" ID="cvViolationDate" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" OnServerValidate="cvViolationDate_ServerValidate" Display="Dynamic" ClientValidationFunction="validateViolationDate" />
                </div>
            </div>

            <!-- Is Violation Ongoing -->
            <div class="col-md-6" >
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblViolationOngoing" CssClass="" meta:resourcekey="lblViolationOngoing" AssociatedControlID="rblViolationOngoing" />
                    <asp:RadioButtonList runat="server" ID="rblViolationOngoing" CssClass="form-radios-group" meta:resourcekey="rblViolationOngoing" RepeatDirection="Horizontal" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvViolationOngoing" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="rblViolationOngoing" Display="Dynamic" />
                </div>
            </div>

            <!-- How Did You Know -->
            <div class="col-md-12" >
                <div class="form-group">
                    <span class="text-danger">*</span>
                    <asp:Label runat="server" ID="lblHowYouKnow" CssClass="" meta:resourcekey="lblHowYouKnow" AssociatedControlID="ddlHowYouKnow" />
                    <asp:DropDownList runat="server" ID="ddlHowYouKnow" CssClass="form-control" meta:resourcekey="ddlHowYouKnow" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvHowYouKnow" CssClass="text-danger" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="ddlHowYouKnow" Display="Dynamic" InitialValue="-1" />
                </div>
            </div>

            <!-- Add Other Type  -->
            <asp:Panel ID="pnlOther" runat="server" Style="display: none">
                <div class="col-md-12" >
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
                        <%-- Keep the real input always in DOM so ASP.NET can read it on postback --%>
                        <asp:FileUpload runat="server" ID="fuSupportingDocuments" CssClass="" AllowMultiple="true"  />

                        <%-- Wrap default UI so we can show/hide it without touching the input --%>
                        <div class="file-upload-default-ui">
                            <div class="ic-upload"></div>
                            <h5>
                                <asp:Literal runat="server" meta:resourcekey="fileUploadTitle"></asp:Literal></h5>
                            <p>
                                <asp:Literal runat="server" meta:resourcekey="fileUploadDescription"></asp:Literal></p>
                            <div style="text-align:center;">
                                <strong>
                               <asp:Literal runat="server" meta:resourcekey="fileUploadBrowseText"></asp:Literal></strong>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Anonymous? -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="120">
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
                            <asp:Label runat="server" ID="Label3" CssClass="" meta:resourcekey="lblName" AssociatedControlID="txtName" />
                            <asp:TextBox runat="server" ID="txt_Name" CssClass="form-control" meta:resourcekey="txtName" />
                        </div>
                    </div>
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="140">
                        <div class="form-group">
                            <span class="text-danger">*</span>
                            <asp:Label runat="server" ID="lblMobileNumber" meta:resourcekey="lblMobileNumber" AssociatedControlID="txtMobileNumber" />
                            <div class="input-group">
                                <%-- Custom searchable country-code picker --%>
                                <div class="country-code-picker" id="divCCP_VR">
                                    <div class="ccp-selected" id="ccpSelected_VR">
                                        <span id="ccpDisplay_VR">+---</span>
                                        <span class="ccp-arrow">&#9662;</span>
                                    </div>
                                </div>
                                <asp:HiddenField runat="server" ID="hfSelectedCountryCode" />
                                <asp:DropDownList runat="server" ID="ddlCountryCode" style="display:none;" />
                                <asp:TextBox runat="server" ID="txtMobileNumber" CssClass="form-control digits" meta:resourcekey="txtMobileNumber" MaxLength="14" />
                            </div>
                            <asp:RequiredFieldValidator runat="server" ID="rfvMobileNumber" CssClass="text-danger" Enabled="false" meta:resourcekey="RequiredField" ValidationGroup="submit" ControlToValidate="txtMobileNumber" Display="Dynamic" />
                            <asp:RegularExpressionValidator runat="server" ID="revMobileNumber" CssClass="text-danger" Enabled="false" meta:resourcekey="InvalidMobileNumber" ValidationGroup="submit" ControlToValidate="txtMobileNumber" ValidationExpression="^\d{9,14}$" Display="Dynamic" />
                        </div>
                    </div>

                    <!-- Email -->
                    <div class="col-md-6" data-aos="fade-up" data-aos-delay="140">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblEmail" meta:resourcekey="lblEmail" AssociatedControlID="txtEmail" />
                            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" meta:resourcekey="txtEmail" ValidationGroup="" TextMode="Email" />
                            <asp:RegularExpressionValidator runat="server" ID="revEmail" CssClass="text-danger" Enabled="false" meta:resourcekey="InvalidEmail" ValidationGroup="submit" ControlToValidate="txtEmail" ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,24}$" Display="Dynamic" />
                        </div>
                    </div>

                </div>
            </asp:Panel>

            <!-- Captcha -->
            <div class="col-md-6" data-aos="fade-up" data-aos-delay="160">
                <div class="form-group">
                    <customtags:captcha runat="server" id="captcha" validationgroup="submit" />
                </div>
            </div>

            <!-- Submit -->
            <div class="col-md-12" data-aos="fade-up" data-aos-delay="160">
                <div class="form-group">
                    <i class="fas fa-spinner fa-spin d-none"></i>
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-secondary" meta:resourcekey="btnSubmit"
                        ValidationGroup="submit"  OnClientClick="return validateFormBeforeSubmit();" OnClick="btnSubmit_Click" />
                </div>
            </div>

        </div>
    </div>

    <script>
        // ── FILE UPLOAD: global scope so all functions are accessible anywhere ──
        var _fu = {
            maxMB: 5,
            allowed: ["png", "jpg", "jpeg", "pdf", "rar"],
            files: [],
            errTimer: null,
            deleteText: '<%= GetLocalResourceObject("fileUploadDeleteText.Text") %>' || 'Delete'
        };

        function fuExt(name) { return name.split('.').pop().toLowerCase(); }

        function fuValidate(file) {
            if (_fu.allowed.indexOf(fuExt(file.name)) === -1)
                return "<%= GetLocalResourceObject("InvalidFileType") %>";
            if (file.size > _fu.maxMB * 1024 * 1024)
                return "<%= GetLocalResourceObject("FileTooLargeFile") %>";
            return null;
        }

        function fuShowError(msg) {
            var old = document.getElementById("fileUploadError");
            if (old && old.parentNode) old.parentNode.removeChild(old);
            var el = document.createElement("div");
            el.id = "fileUploadError";
            el.textContent = msg;
            el.setAttribute("style",
                "display:block !important;color:#dc3545 !important;" +
                "font-size:14px !important;font-weight:500 !important;" +
                "margin-top:6px !important;padding:6px 10px !important;" +
                "background:#fff3f3 !important;border:1px solid #f5c6cb !important;" +
                "border-radius:4px !important;position:relative !important;z-index:9999 !important;");
            var wrapper = document.querySelector(".file-upload");
            if (wrapper && wrapper.parentNode) {
                wrapper.parentNode.insertBefore(el, wrapper);
            } else {
                document.body.appendChild(el);
            }
            clearTimeout(_fu.errTimer);
            _fu.errTimer = setTimeout(function () {
                var e = document.getElementById("fileUploadError");
                if (e && e.parentNode) e.parentNode.removeChild(e);
            }, 5000);
        }

        function fuClearError() {
            var e = document.getElementById("fileUploadError");
            if (e && e.parentNode) e.parentNode.removeChild(e);
        }

        function fuSync(inp) {
            if (!inp || typeof DataTransfer === "undefined") return;
            var dt = new DataTransfer();
            _fu.files.forEach(function (f) { dt.items.add(f); });
            try { inp.files = dt.files; } catch (e) { }
        }

        function fuRender() {
            var list = document.getElementById("fuPreviewList");
            if (!list) return;
            list.innerHTML = "";
            _fu.files.forEach(function (f, idx) {
                var li = document.createElement("li");
                li.className = "uploaded-file-preview d-flex align-items-center mb-1";
                var btn = document.createElement("button");
                btn.type = "button";
                btn.className = "btn btn-sm btn-danger me-2";
                btn.textContent = _fu.deleteText;
                btn.onclick = function () {
                    _fu.files.splice(idx, 1);
                    fuSync(document.querySelector("[id$='fuSupportingDocuments']"));
                    fuRender();
                };
                var name = document.createElement("span");
                name.className = "file-name me-2";
                name.textContent = f.name;
                var size = document.createElement("small");
                size.className = "text-muted ms-auto";
                size.textContent = "(" + (f.size / 1024).toFixed(1) + " KB)";
                li.appendChild(btn); li.appendChild(name); li.appendChild(size);
                list.appendChild(li);
            });
        }

        function fuInit() {
            var inp = document.querySelector("[id$='fuSupportingDocuments']");
            if (!inp) { setTimeout(fuInit, 300); return; }

            // Inject preview list after the .file-upload wrapper
            var wrapper = inp.parentElement;
            while (wrapper && !wrapper.classList.contains("file-upload")) {
                wrapper = wrapper.parentElement;
            }
            if (wrapper && !document.getElementById("fuPreviewList")) {
                var ul = document.createElement("ul");
                ul.id = "fuPreviewList";
                ul.className = "list-unstyled mt-2 mb-0";
                wrapper.parentNode.insertBefore(ul, wrapper.nextSibling);
            }

            inp.addEventListener("change", function () {
                var errorMsg = null;
                Array.from(this.files).forEach(function (nf) {
                    var err = fuValidate(nf);
                    if (err) { errorMsg = err; return; }
                    var dup = _fu.files.some(function (ef) {
                        return ef.name === nf.name && ef.size === nf.size;
                    });
                    if (!dup) _fu.files.push(nf);
                });
                if (errorMsg) fuShowError(errorMsg);
                // Reset value first, THEN sync DataTransfer back into the input
                // so the browser does not clear .files when value is set to ""
                try { this.value = ""; } catch (e) { }
                fuSync(inp);
                fuRender();
            });
        }

        // ── REST OF PAGE LOGIC ───────────────────────────────────────────────────
        $(document).ready(function () {

            fuInit(); // start file upload initialisation

            // Show/hide PartiesSection on change
            var NoValue = "2";
            var rblSelector = "input[name='<%= rblCanIdentifyParties.UniqueID %>']";

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

            var Anonymous = "false";
            var rblAnonymousSelector = "input[name='<%= rblAnonymous.UniqueID %>']";
            var notAnonValidatorIds = [
                '<%= rfvMobileNumber.ClientID %>',
                '<%= revMobileNumber.ClientID %>',
                '<%= revEmail.ClientID %>'
            ];

            function toggleNotAnonValidators(enable) {
                notAnonValidatorIds.forEach(function (id) {
                    var val = document.getElementById(id);
                    if (val) ValidatorEnable(val, enable);
                });
            }

            $(document).on("change", rblAnonymousSelector, function () {
                if ($(this).val() === Anonymous) {
                    $("#<%= NotAnonymous.ClientID %>").show();
                    toggleNotAnonValidators(true);
                } else {
                    $("#<%= NotAnonymous.ClientID %>").hide();
                    toggleNotAnonValidators(false);
                }
            });

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

            $(document).on("change", ddlViolationTypeSelector, function () { toggleOtherType(); });
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

            $(document).on("change", ddlHowDoYouKnowSelector, function () { toggleOther(); });
            toggleOther();

        }); // end document.ready

        // Confirmed from live DOM: SP renders the date input with id ending in
        // 'dtViolationDateDate' (not 'DateTimeField').
        function validateViolationDate(sender, args) {
            var inp = document.querySelector("[id$='dtViolationDateDate']");
            args.IsValid = !!(inp && inp.value.trim() !== '');
        }

        function validateFormBeforeSubmit() {
            var isValid = Page_ClientValidate('submit');
            return typeof isValid === 'boolean' ? isValid : Page_IsValid;
        }
    </script>

    <script type="text/javascript">
        (function () {

            var vrEntries = [];

            // ── Message label ────────────────────────────────────────
            window.vrShowToast = function (message, type) {
                var lbl = document.getElementById("lblVrMessage");
                if (!lbl) return;
                lbl.textContent = message;
                lbl.style.color = (type === "error") ? "#dc3545" : "#198754";
                lbl.style.display = "inline";
                // Auto-hide success after 3 s; keep error visible until next action
                if (type !== "error") {
                    setTimeout(function () { lbl.style.display = "none"; }, 3000);
                }
            };

            // ── Sync entries to hidden field ─────────────────────────
            function vrSyncHiddenField() {
                var hf = document.getElementById('<%= hfPartiesJson.ClientID %>');
            if (hf) hf.value = JSON.stringify(vrEntries);
        }

        // ── Clear form ───────────────────────────────────────────
        window.vrClearForm = function () {
            // Uncheck all radios in ddlRelation
            document.querySelectorAll('#ddlRelation input[type="radio"]').forEach(function (r) {
                r.checked = false;
            });
            document.getElementById('ddlRelation').style.outline = "";

            document.getElementById('<%= txtName.ClientID %>').value = "";
            document.getElementById('<%= txtJobTitle.ClientID %>').value = "";
            document.getElementById('<%= txtCompany.ClientID %>').value = "";

            document.querySelectorAll('.vr-input.error').forEach(function (el) {
                el.classList.remove('error');
            });

            // Clear the message label
            var lbl = document.getElementById("lblVrMessage");
            if (lbl) { lbl.style.display = "none"; lbl.textContent = ""; }
        };

        // ── Add entry ────────────────────────────────────────────
        window.vrAddEntry = function () {
            var hasError = false;

            // Read selected relation from ddlRelation RadioButtonList
            var selectedRelationInput = "#<%= ddlRelation.ClientID %>";
            //document.querySelector('#ddlRelation input[type="radio"]:checked');
            var vrSelectedRelationID = $(selectedRelationInput).val(); //selectedRelationInput ? selectedRelationInput.value : "";
            var vrSelectedRelation = $(selectedRelationInput + " option:selected").text(); //selectedRelationInput ? selectedRelationInput.value : "";

            var name = document.getElementById('<%= txtName.ClientID %>').value.trim();
            var job = document.getElementById('<%= txtJobTitle.ClientID %>').value.trim();
            var company = document.getElementById('<%= txtCompany.ClientID %>').value.trim();


            if (!Page_ClientValidate('AddGroup')) {
                hasError = true;
            }

            if (hasError) {
                vrShowToast('<%= GetLocalResourceObject("InvalidInput") %>', "error");
                return;
            }

            vrEntries.push({
                relation: vrSelectedRelationID,
                relationName: vrSelectedRelation,
                name: name,
                job: job,
                company: company
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
                        '<td><span class="vr-badge ' + bc + '">' + e.relationName + '</span></td>' +
                        '<td>' + (e.name || '—') + '</td>' +
                        '<td>' + (e.job || '—') + '</td>' +
                        '<td>' + (e.company || '—') + '</td>' +
                        '<td>' +
                        '<button type="button" class="vr-btn-delete" onclick="vrRemoveEntry(' + i + ')">' +
                        '<svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor">' +
                        '<path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM15.5 4l-1-1h-5l-1 1H5v2h14V4z"/>' +
                        '</svg></button>' +
                        '</td></tr>';

                    document.getElementById('<%= hfPartiesHTML.ClientID %>').value = tbody.innerHTML;

                });
                }

            }

        })();
    </script>


    <script>
    $(document).ready(function () {
        var data     = <%= CountriesJson %>;
        var isArabic = <%= IsArabic ? "true" : "false" %>;
        var $hfCode  = $('#<%= hfSelectedCountryCode.ClientID %>');
        var $picker = $('#divCCP_VR');
        var $display = $('#ccpDisplay_VR');

        var $dropdown = $(
            '<div class="ccp-dropdown" style="display:none;position:fixed;">' +
            '<input type="text" class="ccp-search" placeholder="Search..." autocomplete="off" />' +
            '<ul></ul>' +
            '</div>'
        ).appendTo('body');

        var $search = $dropdown.find('.ccp-search');
        var $list = $dropdown.find('ul');

        function positionDropdown() {
            var rect = document.getElementById('ccpSelected_VR').getBoundingClientRect();
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

        $('#ccpSelected_VR').on('click', function (e) {
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
    });
    </script>


</asp:Panel>