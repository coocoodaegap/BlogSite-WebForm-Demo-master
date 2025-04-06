<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Articles.aspx.cs" Inherits="BlogSite.Articles" Theme="MainTheme" %>

<%@ Register Src="~/Components/GenresBox.ascx" TagName="GenresBox" TagPrefix="uc1" %>
<%@ Register Src="~/Components/PopularBox.ascx" TagName="PopularBox" TagPrefix="uc1" %>
<%@ Register Src="~/Components/SearchBox.ascx" TagName="SearchBox" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/App_Themes/MainTheme/ArticleList.css">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Wrapper for the carousel -->
    <div class="page-wrapper">
        <div class="content clearfix">
            <div style="margin: 1em 0.6em">
                <uc1:SearchBox ID="SearchBox1" runat="server" />
            </div>
            <div class="main-content" style="padding: 0 0.5em">
                <asp:DataList ID="DataList1" runat="server" OnItemCommand="DataList1_ItemCommand" OnItemDataBound="DataList1_ItemDataBound" Width="100%">
                    <ItemTemplate>
                        <div class="article-item">
                            <div class="article-image">
                                <a href='Article.aspx?id=<%#Eval("Id")%>'>
                                    <img class="img-responsive" src='<%#Eval("ImageUrl")%>' alt="<%#Eval("title")%>" />
                                </a>
                            </div>
                            <div class="article-info">
                                <h3><a href='Article.aspx?id=<%#Eval("Id")%>'><%#Eval("title")%></a></h3>
                                <p class="summary">
                                    <%#Eval("summary").ToString().Length > 100 ? Eval("summary").ToString().Substring(0, 100) + "..." : Eval("summary")%>
                                </p>
                                <div class="post-info">
                                    <a href='Articles.aspx?genre=<%#Eval("GenreName")%>'><i class="fa fa-tags"><%# Eval("GenreName") %></i></a>&nbsp;
                               
                                <i class="far fa-calendar"><%# Eval("published_date") %></i>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div class="pagination">
                            <asp:Label ID="labCurrentPage" runat="server" />
                            <span>/</span>
                            <asp:Label ID="labPageCount" runat="server" />
                            <div class="pagination-controls">
                                <asp:LinkButton ID="lnkbtnFirst" runat="server" CommandName="first">首页</asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnFront" runat="server" CommandName="pre">上一页</asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnNext" runat="server" CommandName="next">下一页</asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnLast" runat="server" CommandName="last">尾页</asp:LinkButton>
                                <span>跳转至：</span>
                                <asp:TextBox ID="txtPage" runat="server" Width="40px" Height="25px" />
                                <asp:Button ID="Button1" runat="server" CommandName="search" Text="GO" />
                            </div>
                        </div>
                    </FooterTemplate>
                </asp:DataList>
            </div>

            <div class="sidebar single">
                <uc1:PopularBox ID="PopularBox1" runat="server" />
                <uc1:GenresBox ID="GenresBox1" runat="server" />
            </div>
            <!-- End Sidebar -->
        </div>
    </div>
</asp:Content>
