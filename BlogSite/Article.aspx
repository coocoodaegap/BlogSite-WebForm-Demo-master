<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Article.aspx.cs" Inherits="BlogSite.Article1" Theme="MainTheme" %>

<%@ Register Src="~/Components/GenresBox.ascx" TagName="GenresBox" TagPrefix="uc1" %>
<%@ Register Src="~/Components/OutlineBox.ascx" TagName="OutlineBox" TagPrefix="uc1" %>
<%@ Register Src="~/Components/PopularBox.ascx" TagName="PopularBox" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-wrapper">
        <div class="content clearfix">
            <!-- Main Content -->
            <div class="main-content single">
                <h1 class="post-title">
                    <asp:Label ID="LabelTitle" runat="server" Text="Label"></asp:Label>
                </h1>
                <div class="post-content">
                    <asp:Literal ID="LiteralSummary" runat="server"></asp:Literal>
                </div>
                <div class="post-content">
                    <asp:Literal ID="LiteralContent" runat="server"></asp:Literal>
                </div>
            </div>
            <!-- End Main Content -->

            <!-- Sidebar -->
            <div class="sidebar single">
                <uc1:OutlineBox ID="OutlineBox1" runat="server" />
                <uc1:PopularBox ID="PopularBox1" runat="server" />
                <uc1:GenresBox ID="GenresBox1" runat="server" />
            </div>
            <!-- End Sidebar -->

        </div>
        <!-- End Content -->
    </div>
    <!-- End Page Wrapper -->
</asp:Content>
