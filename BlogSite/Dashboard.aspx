<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="BlogSite.Dashboard" Theme="MainTheme" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Dashboard
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="/App_Themes/MainTheme/ArticleList.css">
    <style>
        /* 背景遮罩 */
        .overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 99;
        }

        /* 模态对话框 */
        .dialog {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background-color: white;
            border-radius: 10px;
            padding: 20px;
            z-index: 100;
            width: 400px;
            box-shadow: 0px 5px 15px rgba(0, 0, 0, 0.3);
            display: block;
        }

        .debug {
            /*            display: none*/
        }
        /* 为文章列表标题和按钮添加样式 */
        h2 {
            display: inline-block;
            margin: 0;
        }

        .user-edit {
            background: white;
            margin: 0.2em 0.5em;
            padding: 0.5em;
            border-radius: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-wrapper">
        <div class="content clearfix">
            <div class="user-edit">
                <asp:Label ID="lblUsername" runat="server"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="lblEmail" runat="server"></asp:Label>
                <span style="float: right; padding: 0 0.5em">
                    <asp:Button SkinID="ColoredBtn" ID="btnEditProfile" runat="server" OnClick="btnEditProfile_Click" Text="编辑个人信息" /></span>
                <br />
                <br />
                <asp:Label ID="lblBio" runat="server"></asp:Label><br />
            </div>
        </div>

        <div class="content">
            <h2>文章列表</h2>
            <span style="float: right; padding: 0 0.5em">
                <asp:Button ID="btnAddArticle" runat="server" Text="添加文章" OnClick="btnAddArticle_Click" /></span>
        </div>

        <div class="content clearfix">
            <div style="padding: 0 0.5em">
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
                            <div class="article-actions">
                                <asp:Button SkinID="ColoredBtn" ID="btnEditArticle" runat="server" Text="编辑" CommandName="Edit" CommandArgument='<%#Eval("Id")%>' />
                                <asp:Button SkinID="ColoredBtn" ID="btnDeleteArticle" runat="server" Text="删除" CommandName="Delete" CommandArgument='<%#Eval("Id")%>' BackColor="#D9534F" />
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
        </div>

        <asp:Panel ID="pnlOverlay" runat="server" CssClass="overlay" Visible="False"></asp:Panel>

        <asp:Panel ID="pnlEditArticle" runat="server" CssClass="dialog" Visible="False">
            <h3 id="modalTitle">文章编辑</h3>
            <table>
                <tr>
                    <td>标题：</td>
                    <td>
                        <asp:TextBox ID="txtArticleTitle" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>封面：</td>
                    <td>
                        <!-- 显示当前图片 -->
                        <asp:Image ID="imgCurrentCover" runat="server" Width="100" Height="100" Visible="false" />
                        <br />
                        <!-- 上传新图片 -->
                        <asp:FileUpload ID="fileUploadCoverImage" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>分类：</td>
                    <td>
                        <asp:TextBox ID="txtArticleGenre" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>概要：</td>
                    <td>
                        <asp:TextBox ID="txtArticleSummary" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>内容：</td>
                    <td>
                        <asp:TextBox ID="txtArticleContent" runat="server" TextMode="MultiLine" Rows="10" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button SkinID="ColoredBtn" ID="btnSaveArticle" runat="server" Text="保存" OnClick="btnSaveArticle_Click" />
                        <asp:Button SkinID="ColoredBtn" ID="btnCancelArticle" runat="server" Text="取消" OnClick="btnCancelArticle_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <!-- 修改个人信息弹出框 -->
        <asp:Panel ID="pnlEditProfile" runat="server" CssClass="dialog debug" Visible="False">
            <h3>修改个人信息</h3>
            <table>
                <tr>
                    <td>用户名：</td>
                    <td>
                        <asp:TextBox ID="txtEditUsername" runat="server" /></td>
                </tr>
                <tr>
                    <td>邮箱：</td>
                    <td>
                        <asp:TextBox ID="txtEditEmail" runat="server" /></td>
                </tr>
                <tr>
                    <td>简介：</td>
                    <td>
                        <asp:TextBox ID="txtEditBio" runat="server" TextMode="MultiLine" /></td>
                </tr>
                <tr>
                    <td>新密码：</td>
                    <td>
                        <asp:TextBox ID="txtEditPassword" runat="server" TextMode="Password" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button SkinID="ColoredBtn" ID="btnSaveProfile" runat="server" Text="保存" OnClick="btnSaveProfile_Click" />
                        <asp:Button SkinID="ColoredBtn" ID="btnCancelProfile" runat="server" Text="取消" OnClick="btnCancelProfile_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>

    <script src="https://cdn.ckeditor.com/4.22.1/basic/ckeditor.js"></script>
    <script>
        window.onload = function () {
            CKEDITOR.config.versionCheck = false;
            CKEDITOR.replace('<%= txtArticleContent.ClientID %>');
        };
    </script>
</asp:Content>

