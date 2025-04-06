<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutlineBox.ascx.cs" Inherits="BlogSite.Components.OutlineBox" %>
<div class="section">
    <h2 class="section-title">大纲</h2>
    <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
        <HoverNodeStyle ForeColor="#006669" />
        <NodeStyle Font-Size="12pt" ForeColor="Black" HorizontalPadding="4px" NodeSpacing="0px" VerticalPadding="4px" />
        <ParentNodeStyle Font-Bold="False" />
        <SelectedNodeStyle ForeColor="#006669" HorizontalPadding="0px" VerticalPadding="0px" />
    </asp:TreeView>
</div>