using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlogSite
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 退出登录
            FormsAuthentication.SignOut();

            // 重定向到登录页面或主页
            Response.Redirect("Login.aspx");
        }
    }
}