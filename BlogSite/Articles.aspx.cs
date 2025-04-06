using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace BlogSite
{
    public partial class Articles : System.Web.UI.Page
    {
        static PagedDataSource pds = new PagedDataSource(); // 创建分页数据源
        private DataClasses1DataContext _dataClasses1 = new DataClasses1DataContext(System.Configuration.ConfigurationManager.ConnectionStrings["db_ConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
    }
}