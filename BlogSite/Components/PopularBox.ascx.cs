using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlogSite.Components
{
    public partial class PopularBox : System.Web.UI.UserControl
    {
        private DataClasses1DataContext _dataClasses1 = new DataClasses1DataContext(System.Configuration.ConfigurationManager.ConnectionStrings["db_ConnectionString"].ToString());

        public List<Post> PopularPosts { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            PopularPosts = _dataClasses1.tb_article
    .OrderByDescending(a => a.published_date)
    .Take(3)
    .Select(a => new Post
    {
        Title = a.title,
        ImageUrl = ConfigurationManager.AppSettings["ImgBaseUrl"] + a.image,
        Id = a.Id.ToString()
    }).ToList();

            // 绑定热门贴数据
            PopularPostsRepeater.DataSource = PopularPosts;
            PopularPostsRepeater.DataBind();
        }
    }
    public class Post
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Id { get; set; }
    }
}