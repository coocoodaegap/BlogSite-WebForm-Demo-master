<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchBox.ascx.cs" Inherits="BlogSite.Components.SearchBox" %>

<div class="section search">
    <h2 class="section-title">搜索</h2>
    <asp:TextBox ID="SearchTextBox" runat="server" CssClass="text-input" placeholder="输入关键词......" />
    <div class="clearfix">
        <div class="clearfix" style="float: right; margin-top: 0.5em">

            <asp:Button SkinID="ColoredBtn" ID="SearchButton" runat="server" Text="搜索" OnClick="SearchButton_Click" />
        </div>
    </div>
</div>
