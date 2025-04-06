<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BlogSite.Login" Theme="MainTheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    登录
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/App_Themes/MainTheme/Auth.css">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-content">
        <asp:Panel ID="LoginPanel" runat="server">
            <h2 class="form-title">登录</h2>
            <div style="width: fit-content; margin: 1em auto">
                <asp:Label ID="ErrorMessage" CssClass="msg error success" runat="server" Visible="false"></asp:Label>
            </div>

            <asp:Panel ID="FormContent" runat="server">
                <div>
                    <asp:Label ID="UsernameLabel" runat="server" Text="用户名"></asp:Label>
                    <asp:TextBox ID="UsernameTextBox" CssClass="text-input" runat="server" ToolTip="用户名"></asp:TextBox>
                </div>

                <div>
                    <asp:Label ID="PasswordLabel" runat="server" Text="密码"></asp:Label>
                    <asp:TextBox ID="PasswordTextBox" CssClass="text-input" TextMode="Password" runat="server" ToolTip="密码"></asp:TextBox>
                </div>

                <div style="width: fit-content; margin: 1em auto 0">
                    <asp:Button SkinID="ColoredBigBtn" ID="LoginButton" runat="server" Text="登录" OnClick="LoginButton_Click" />
                </div>

                <%--                <p>
                    没有账号？ <a href="Register.aspx">注册</a>
                </p>--%>
            </asp:Panel>
        </asp:Panel>
    </div>
</asp:Content>

