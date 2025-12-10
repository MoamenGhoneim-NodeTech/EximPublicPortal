<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Captcha.ascx.cs" Inherits="Exim.Portal.WebParts.Captcha" %>

<asp:Label runat="server" AssociatedControlID="txtCaptcha" class="d-flex "><span class="text-danger">*</span> <asp:Literal runat="server" meta:resourcekey="VerificationCode"></asp:Literal>:
    <a class="btn btn-sm ms-3 btn-success refresh-btn refreshcaptchabtn" id="refresh-btn"><i class="ic-refresh"></i></a>
</asp:Label>
<div class="d-flex align-items-center gap-3">
    <img src="/_layouts/15/Exim.Portal/Captcha.aspx?w=200&h=50&f=20" alt="الرجاء إدخال النص الظاهر في الصورة">
    <asp:TextBox runat="server" ID="txtCaptcha" CssClass="form-control" placeholder="Please enter the text shown in the image" autocomplete="off" meta:resourcekey="TextBox1Resource1"></asp:TextBox>
</div>


    <asp:RequiredFieldValidator Display="Dynamic" CssClass="text-danger" ID="RequiredFieldValidator1" ControlToValidate="txtCaptcha" runat="server" ErrorMessage="RequiredFieldValidator" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
    <asp:CustomValidator Display="Dynamic" CssClass="text-danger" ID="captchaCustomValidator" OnServerValidate="captchaCustomValidator_ServerValidate" runat="server" meta:resourcekey="captchaCustomValidatorResource1"></asp:CustomValidator>
<script type="text/javascript">
    $('.refreshcaptchabtn').click(function () {
        var date = new Date();
		$('.captchaimg').attr('src', '/_layouts/15/Exim.Portal/Captcha.aspx?w=200&h=50&f=20&d=' + date);
    });    
</script>
