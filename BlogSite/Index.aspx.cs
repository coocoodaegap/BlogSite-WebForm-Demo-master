using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlogSite
{
    public partial class Index : System.Web.UI.Page
    {
        private DataClasses1DataContext _dataClasses1 = new DataClasses1DataContext(System.Configuration.ConfigurationManager.ConnectionStrings["db_ConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPostSlider();
                LoadRecentPosts();
            }
        }

        private string Author = "Ruan ZH";

        private void LoadPostSlider()
        {
            var posts = _dataClasses1.tb_article
                        .OrderByDescending(a => a.published_date) // 按日期排序
                        .Take(4)  // 获取最新的 4 条
                        .Select(a => new Post
                        {
                            ImageUrl = ConfigurationManager.AppSettings["ImgBaseUrl"] + a.image,
                            Title = a.title,
                            Author = Author,  // 假设 Author 存储在 Genre 字段中
                            Date = a.published_date.ToString(), // 格式化日期
                            Id = a.Id
                        }).ToList();

            PostRepeater.DataSource = posts;
            PostRepeater.DataBind();
        }

        private void LoadRecentPosts()
        {
            // 使用 LINQ 查询获取最近的 3 条帖子
            var posts = _dataClasses1.tb_article
                .OrderByDescending(a => a.published_date)
                .Take(3)
                .Select(a => new Post
                {
                    ImageUrl = ConfigurationManager.AppSettings["ImgBaseUrl"] + a.image,
                    Title = a.title,
                    Author = Author,  // 假设 Author 存储在 Genre 字段中
                    Date = a.published_date.ToString(),
                    Id = a.Id,
                    Summary = a.summary.Substring(0, 20) + "..."  // 假设摘要为内容的前 100 个字符
                }).ToList();

            // 绑定数据到 Repeater 控件
            RecentPostsRepeater.DataSource = posts;
            RecentPostsRepeater.DataBind();
        }


        // 模拟的文章类
        public class Post
        {
            public string ImageUrl { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string Date { get; set; }
            public int Id { get; set; }
            public string Summary { get; set; }  // 用于显示摘要
        }

    }
}