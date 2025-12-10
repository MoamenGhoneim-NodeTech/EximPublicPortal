<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LabelMessage.ascx.cs" Inherits="Exim.Portal.WebParts.LabelMessage" %>
<asp:Repeater runat="server" ID="rptMessages">
    <ItemTemplate>
        <div class="<%# Eval("DivCss") %>">
            <span><%# Eval("Message") %></span>
        </div>
    </ItemTemplate>
</asp:Repeater>
