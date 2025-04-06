using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlogSite.Components
{
    public partial class SearchBox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string searchTerm = SearchTextBox.Text.Trim();
            // 添加搜索逻辑，执行数据库查询等
            Response.Redirect("articles.aspx?keyword=" + searchTerm);
        }
    }
}