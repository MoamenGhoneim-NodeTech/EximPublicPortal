<%@ Assembly Name="EXIM.Portal.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6c14352c1754619e" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FindMyProduct.ascx.cs" Inherits="EXIM.Portal.WebParts.CONTROLTEMPLATES.Exim.FindMyProduct.FindMyProduct" %>


<!--
  ====================================================================
     Product Consultant page
  ====================================================================
  -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.2/jquery.validate.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.2/additional-methods.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.2/localization/messages_ar.min.js"></script>


<%--<script>

    var form = $(".custom-form");
    form.validate({

        errorPlacement: function errorPlacement(error, element) {

            $(element).closest('.form-group').append(error);
        },
        rules: {}
    });


    $(document).on('submit', '.custom-form', function () {

        form.validate().settings.ignore = ":disabled,:hidden";

        if (form.valid()) {

            var $this = $(this);
            $this.find('button[type=submit]').attr('disabled', true);
            $this.find('button[type=submit]').find('i').removeClass('d-none');

            setTimeout(function () {

                $this.find('button[type=submit]').find('i').addClass('d-none');
                $this.find('button[type=submit]').attr('disabled', false);

                $(".product-panel").addClass('d-none');
                $(".product-panel-selected").removeClass('d-none');
            }, 1000);


        }

        return false;

    })


</script>--%>
      <asp:UpdatePanel ID="updatePanelForm" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
    <section class="product-consultant">
        <div class="container">
            <div class="row g-5">
          
                        <div class="col-md-6">

                            <div action="" method="post" class="custom-form mt-5">
                                <div class="row g-4">
                                    <div class="col-md-12" >
                                        <div class="form-group">
                                            <label for="f-1">
                                                <span class="text-danger">*</span>
                                                <asp:Label ID="lbl_Benificiary" runat="server" meta:resourcekey="lbl_Benificiary"></asp:Label>
                                            </label>

                                            <asp:DropDownList ID="ddl_Benificiary" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddl_Benificiary_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-12" >
                                        <div class="form-group">
                                            <label for="f-2">
                                                <span class="text-danger">*</span>
                                                <asp:Label ID="lbl_Usage" runat="server" meta:resourcekey="lbl_Usage"></asp:Label>

                                            </label>
                                            <asp:DropDownList ID="ddl_Usage" runat="server"  CssClass="form-control" OnSelectedIndexChanged="ddl_Usage_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                                        </div>
                                    </div>


                                    <div class="col-md-12" >
                                        <div class="form-group">

                                            <asp:Button ID="btn_Submit" runat="server" CssClass="btn btn-secondary" Text="Button" OnClick="btn_Submit_Click" meta:resourcekey="btn_Submit" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="col-md-6 d-flex align-items-center">
                            <div id="divProductPanel" class="product-panel" runat="server">
                                <i class="ic-i"></i>

                                <asp:Label ID="lbl_UserInstruction" runat="server" meta:resourcekey="lbl_UserInstruction"></asp:Label>
                            </div>
                            <div id="divProductPanelSelected" class="product-panel-selected d-none" runat="server">
                                <h2>
                                    <asp:Label ID="lbl_Result" runat="server" meta:resourcekey="lbl_Result"></asp:Label></h2>
                                <h3>
                                    <asp:Label ID="lbl_ServiceName" runat="server" meta:resourcekey="lbl_ServiceName"></asp:Label>
                                </h3>
                                <p>
                                    ​​​​​<asp:Label ID="lbl_ServiceDescription" runat="server" meta:resourcekey="lbl_ServiceDescription"></asp:Label>
                                </p>
                                <div class="product-panel-selected-action">
                                    <%--  <a href="" class="btn btn-secondary"></a>--%>
                                    <asp:HyperLink ID="hypr_RequestService" runat="server" class="btn btn-secondary" Target="_blank" meta:resourcekey="hypr_RequestService"></asp:HyperLink>
                                    <asp:HyperLink ID="hypr_ServiceDetails" runat="server" class="btn btn-secondary" Target="_blank"  meta:resourcekey="hypr_ServiceDetails"></asp:HyperLink>
                                    <%--<a href="service-1.html" class="btn btn-outline-light"  meta:resourcekey="hypr_RequestService"></a>--%>
                                </div>

                            </div>


                        </div>

            </div>
        </div>

    </section>


                    </ContentTemplate>
                </asp:UpdatePanel>

