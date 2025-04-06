<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="BlogSite.Index" Theme="MainTheme"%>

<%@ Register src="~/Components/GenresBox.ascx" TagName="GenresBox" TagPrefix="uc1" %>
<%@ Register src="~/Components/SearchBox.ascx" TagName="SearchBox" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    主页
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/App_Themes/MainTheme/Content.css">
    <link rel="stylesheet" type="text/css" href="/App_Themes/MainTheme/Slider.css">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Wrapper for the carousel -->
    <div class="page-wrapper">
        <!-- Carousel -->
        <div class="post-slider">
            <h1 class="slider-title">热门贴 </h1>
            <i class="fas fa-chevron-left prev" id="prevBtn"></i>
            <i class="fas fa-chevron-right next" id="nextBtn"></i>

            <div class="post-wrapper" id="postWrapper">
                <asp:Repeater ID="PostRepeater" runat="server">
                    <ItemTemplate>
                        <div class="post">
                            <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("Title") %>' class="slider-image">
                            <div class="post-info">
                                <h4><a href='Article.aspx?id=<%# Eval("Id") %>'><%# Eval("Title") %></a></h4>
                                <i class="far fa-user"><%# Eval("Author") %></i>&nbsp;
                               
                                <i class="far fa-calendar"><%# Eval("Date") %></i>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <!-- Content -->
        <div class="content clearfix">
            <div class="main-content">
                <h1 class="recent-post-title">最近 </h1>

                <asp:Repeater ID="RecentPostsRepeater" runat="server">
                    <ItemTemplate>
                        <div class="post">
                            <img src='<%# Eval("ImageUrl") %>' alt="post image" class="post-image">
                            <div class="post-preview">
                                <h2><a href='Article.aspx?id=<%# Eval("Id") %>'><%# Eval("Title") %></a></h2>
                                <i class="far fa-user"><%# Eval("Author") %></i>&nbsp;
                               
                                <i class="far fa-calendar"><%# Eval("Date") %></i>
                                <p class="preview-text">
                                    <%# Eval("Summary") %>
                                </p>
                                <a href='Article.aspx?id=<%# Eval("Id") %>' class="btn btn-colored read-more">详情</a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="sidebar">
                <uc1:SearchBox id="SearchBox1" runat="server" />

                <uc1:GenresBox id="GenresBox1" runat="server" />
            </div>
        </div>
    </div>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/slick-carousel@1.8.1/slick/slick.min.js"></script>
    <script src="js/scripts.js"></script>

    <script>
        $(document).ready(function () {
            $('.post-wrapper').slick({
                slidesToShow: 3,
                slidesToScroll: 1,
                autoplay: true,
                autoplaySpeed: 2000,
                nextArrow: $('#nextBtn'),
                prevArrow: $('#prevBtn')
            });
        });
    </script>
</asp:Content>

