using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static BlogSite.Index;

namespace BlogSite
{
    public partial class Article1 : System.Web.UI.Page
    {
        public string PostTitle { get; set; }
        public string PostContent { get; set; }

        private DataClasses1DataContext _dataClasses1 = new DataClasses1DataContext(System.Configuration.ConfigurationManager.ConnectionStrings["db_ConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            var articleId = Request.QueryString["id"];

            if (!string.IsNullOrEmpty(articleId))
            {
                // 通过 LINQ 查询获取指定 id 的文章
                var article = _dataClasses1.tb_article
                    .Where(a => a.Id.ToString() == articleId)
                    .FirstOrDefault();

                // 如果找到对应的文章，绑定内容
                if (article != null)
                {
                    PostTitle = article.title;
                    PostContent = article.content;
                    LabelTitle.Text = article.title;
                    LiteralSummary.Text = article.summary;
                    LiteralContent.Text = OutlineBox1.GenerateOutline(PostContent);
                } else
                {
                    Response.Redirect("Index.aspx");
                }
            } else
            {
                Response.Redirect("Index.aspx");
            }

        }

    }
}