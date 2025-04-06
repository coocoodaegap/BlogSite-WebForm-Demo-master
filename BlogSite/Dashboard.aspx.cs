using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlogSite
{
    public partial class Dashboard : System.Web.UI.Page
    {
        static PagedDataSource pds = new PagedDataSource(); // 创建分页数据源
        private DataClasses1DataContext _dataClasses1 = new DataClasses1DataContext(System.Configuration.ConfigurationManager.ConnectionStrings["db_ConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserInfo();
                BindDataList(0); // 加载第一页数据
            }
        }

        private void BindDataList(int currentPage)
        {
            pds.AllowPaging = true; // 允许分页
            pds.PageSize = 10; // 每页显示10条数据
            pds.CurrentPageIndex = currentPage; // 当前页为传入的参数

            // 从 URL 参数获取值
            string keyword = Request.QueryString["keyword"]; // 获取 keyword 参数
            string genre = Request.QueryString["genre"]; // 获取 genre 参数

            // 初始化 LINQ 查询，联合 Article 表和 Genre 表
            var articlesQuery = from article in _dataClasses1.tb_article
                                join genreItem in _dataClasses1.tb_genre on article.genre equals genreItem.id
                                select new
                                {
                                    article.Id,
                                    article.title,
                                    article.summary,
                                    article.content,
                                    article.published_date,
                                    GenreName = genreItem.genre_name, // 获取实际的 genre_name
                                    ImageUrl = ConfigurationManager.AppSettings["ImgBaseUrl"] + article.image
                                };

            // 如果有 keyword 参数，则查询标题、内容和摘要中包含该关键词的文章
            if (!string.IsNullOrEmpty(keyword))
            {
                articlesQuery = articlesQuery.Where(a => a.title.Contains(keyword) ||
                                                          a.summary.Contains(keyword) ||
                                                          a.content.Contains(keyword)); // 使用 GenreName 进行模糊查询
            }

            // 如果有 genre 参数，则查询符合 genre 名称的文章
            if (!string.IsNullOrEmpty(genre))
            {
                articlesQuery = articlesQuery.Where(a => a.GenreName == genre); // 使用 genre_name 进行筛选
            }

            // 执行查询并按发布日期降序排列
            articlesQuery = articlesQuery.OrderByDescending(a => a.published_date);

            // 获取查询结果
            var articles = articlesQuery.ToList();

            // 设置分页数据源
            pds.DataSource = articles; // 将查询结果绑定到分页数据源
            DataList1.DataSource = pds; // 绑定到 DataList 控件
            DataList1.DataBind();
        }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "first": // 第一页
                    pds.CurrentPageIndex = 0;
                    BindDataList(pds.CurrentPageIndex);
                    break;
                case "pre": // 上一页
                    pds.CurrentPageIndex = pds.CurrentPageIndex - 1;
                    BindDataList(pds.CurrentPageIndex);
                    break;
                case "next": // 下一页
                    pds.CurrentPageIndex = pds.CurrentPageIndex + 1;
                    BindDataList(pds.CurrentPageIndex);
                    break;
                case "last": // 最后一页
                    pds.CurrentPageIndex = pds.PageCount - 1;
                    BindDataList(pds.CurrentPageIndex);
                    break;
                case "search": // 页面跳转
                    if (e.Item.ItemType == ListItemType.Footer)
                    {
                        int PageCount = int.Parse(pds.PageCount.ToString());
                        TextBox txtPage = e.Item.FindControl("txtPage") as TextBox;
                        int MyPageNum = 0;
                        if (!txtPage.Text.Equals(""))
                            MyPageNum = Convert.ToInt32(txtPage.Text.ToString());
                        if (MyPageNum <= 0 || MyPageNum > PageCount)
                            Response.Write("<script>alert('请输入页数并确定没有超出总页数！')</script>");
                        else
                            BindDataList(MyPageNum - 1);
                    }
                    break;
                case "Edit":
                    int articleId = Convert.ToInt32(e.CommandArgument);
                    var article = _dataClasses1.tb_article.FirstOrDefault(a => a.Id == articleId);
                    if (article != null)
                    {
                        txtArticleTitle.Text = article.title;
                        imgCurrentCover.ImageUrl = article.image;
                        txtArticleGenre.Text = GetGenreName(Convert.ToInt32(article.genre));
                        txtArticleSummary.Text = article.summary;
                        txtArticleContent.Text = article.content;
                        ViewState["EditArticleId"] = articleId;
                        pnlOverlay.Visible = true;
                        pnlEditArticle.Visible = true;
                    }
                    if(imgCurrentCover.ImageUrl == null)
                    {
                        imgCurrentCover.Visible = false;
                    } else
                    {
                        imgCurrentCover.ImageUrl = ConfigurationManager.AppSettings["ImgBaseUrl"] + imgCurrentCover.ImageUrl;
                        imgCurrentCover.Visible = true;
                    }
                    break;
                case "Delete":
                    articleId = Convert.ToInt32(e.CommandArgument);
                    article = _dataClasses1.tb_article.FirstOrDefault(a => a.Id == articleId);
                    if (article != null)
                    {
                        _dataClasses1.tb_article.DeleteOnSubmit(article);
                        _dataClasses1.SubmitChanges();
                        BindDataList(0);
                    }
                    break;
            }
        }

        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                // 获取并绑定分页控件
                Label CurrentPage = e.Item.FindControl("labCurrentPage") as Label;
                Label PageCount = e.Item.FindControl("labPageCount") as Label;
                LinkButton FirstPage = e.Item.FindControl("lnkbtnFirst") as LinkButton;
                LinkButton PrePage = e.Item.FindControl("lnkbtnFront") as LinkButton;
                LinkButton NextPage = e.Item.FindControl("lnkbtnNext") as LinkButton;
                LinkButton LastPage = e.Item.FindControl("lnkbtnLast") as LinkButton;

                CurrentPage.Text = (pds.CurrentPageIndex + 1).ToString(); // 显示当前页
                PageCount.Text = pds.PageCount.ToString(); // 显示总页数

                // 禁用上一页和首页按钮（如果是第一页）
                if (pds.IsFirstPage)
                {
                    FirstPage.Enabled = false;
                    PrePage.Enabled = false;
                }

                // 禁用下一页和尾页按钮（如果是最后一页）
                if (pds.IsLastPage)
                {
                    NextPage.Enabled = false;
                    LastPage.Enabled = false;
                }
            }
        }

        private void LoadUserInfo()
        {
            var userQuery = from user in _dataClasses1.tb_user
                                where user.Id == Convert.ToInt32(HttpContext.Current.User.Identity.Name)
                                select new
                                {
                                    user.username,
                                    user.email,
                                    user.bio,
                                };
            var userData = userQuery.FirstOrDefault();
            if (userData == null)
            {
                Response.Redirect("Login.aspx");
            }
            lblUsername.Text = userData.username;
            lblEmail.Text = userData.email;
            lblBio.Text = userData.bio;
        }


        // 用户信息部分
        protected void btnEditProfile_Click(object sender, EventArgs e)
        {
            txtEditUsername.Text = lblUsername.Text;
            txtEditEmail.Text = lblEmail.Text;
            txtEditBio.Text = lblBio.Text;
            pnlOverlay.Visible = true;
            pnlEditProfile.Visible = true;
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            var userQuery = _dataClasses1.tb_user.FirstOrDefault(a => a.Id == Convert.ToInt32(HttpContext.Current.User.Identity.Name));
            if (userQuery == null)
            {
                Response.Redirect("Login.aspx");
            }
            userQuery.username = txtEditUsername.Text;
            userQuery.email = txtEditEmail.Text;
            userQuery.bio = txtEditBio.Text;
            userQuery.password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtEditPassword.Text, "SHA1");
            _dataClasses1.SubmitChanges();
            Response.Redirect(Request.RawUrl);
        }

        protected void btnCancelProfile_Click(object sender, EventArgs e)
        {
            CloseDialogs();
        }

        protected void btnCancelArticle_Click(object sender, EventArgs e)
        {
            CloseDialogs();
        }

        private void CloseDialogs()
        {
            pnlOverlay.Visible = false;
            pnlEditProfile.Visible = false;
            pnlEditArticle.Visible = false;
        }

        protected void btnAddArticle_Click(object sender, EventArgs e)
        {
            // 清空模态框字段
            txtArticleTitle.Text = "";
            imgCurrentCover.Visible = false;
            txtArticleGenre.Text = "";
            txtArticleSummary.Text = "";
            txtArticleContent.Text = "";
            ViewState["EditArticleId"] = null;
            pnlOverlay.Visible = true;
            pnlEditArticle.Visible = true;
        }

        protected void btnSaveArticle_Click(object sender, EventArgs e)
        {
            string fileName = null;
            if (fileUploadCoverImage.HasFile)
            {
                fileName = Path.GetFileName(fileUploadCoverImage.PostedFile.FileName);
                string filePath = Server.MapPath("~/" + ConfigurationManager.AppSettings["ImgBaseUrl"]) + fileName;
                fileUploadCoverImage.SaveAs(filePath);
            }
            int? articleId = ViewState["EditArticleId"] as int?;
            if (articleId.HasValue)
            {
                // 更新文章
                var article = _dataClasses1.tb_article.FirstOrDefault(a => a.Id == articleId.Value);
                if (article != null)
                {
                    article.title = txtArticleTitle.Text;
                    if(fileName != null) article.image = fileName;
                    article.genre = GetGenreId(txtArticleGenre.Text);
                    article.content = txtArticleContent.Text;
                    article.summary = txtArticleSummary.Text;
                }
            }
            else
            {
                // 新增文章
                var newArticle = new tb_article
                {
                    title = txtArticleTitle.Text,
                    image = fileName,
                    genre = GetGenreId(txtArticleGenre.Text),
                    content = txtArticleContent.Text,
                    summary = txtArticleSummary.Text,
                    published_date = DateTime.Now
                };
                _dataClasses1.tb_article.InsertOnSubmit(newArticle);
            }

            _dataClasses1.SubmitChanges();
            CloseDialogs();
            BindDataList(0);
        }

        private int GetGenreId(string genreName)
        {
            var genre = _dataClasses1.tb_genre.FirstOrDefault(g => g.genre_name == genreName);

            if (genre != null)
            {
                // 如果存在，直接返回该 genre 的 id
                return genre.id;
            }
            else
            {
                // 如果不存在，创建新的 genre
                var newGenre = new tb_genre
                {
                    genre_name = genreName
                };

                // 将新的 genre 添加到数据库
                _dataClasses1.tb_genre.InsertOnSubmit(newGenre);
                _dataClasses1.SubmitChanges(); // 提交更改，保存新记录

                // 返回新创建的 genre 的 id
                return newGenre.id;
            }
        }

        private string GetGenreName(int genreId)
        {
            var genre = _dataClasses1.tb_genre.FirstOrDefault(g => g.id == genreId);
            return genre?.genre_name ?? "未分类";
        }


    }
}