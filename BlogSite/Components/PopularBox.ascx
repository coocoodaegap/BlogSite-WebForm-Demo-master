<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopularBox.ascx.cs" Inherits="BlogSite.Components.PopularBox" %>

<div class="section popular">
    <h2 class="section-title">热门贴</h2>
    <asp:Repeater ID="PopularPostsRepeater" runat="server">
        <ItemTemplate>
            <div class="post clearfix">
                <img src='<%# Eval("ImageUrl") %>' alt="image">
                <a href='Article.aspx?id=<%# Eval("Id") %>' class="title">
                    <h4><%# Eval("Title") %></h4>
                </a>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
