<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenresBox.ascx.cs" Inherits="BlogSite.Components.GenresBox" %>
<div class="section topics">
    <h2 class="section-title">话题</h2>
    <asp:Repeater ID="GenreRepeater" runat="server">
        <ItemTemplate>
            <ul><li><a class="genre" href='Articles.aspx?genre=<%# Eval("genre_name") %>'><%# Eval("genre_name") %></a></li></ul>
        </ItemTemplate>
    </asp:Repeater>
</div>
