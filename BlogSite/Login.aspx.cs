using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlogSite
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect("Logout.aspx");
            }
        }
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                DisplayError("用户名不能为空！");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                DisplayError("密码不能为空！");
                return;
            }
            string strcon = System.Configuration.ConfigurationManager.ConnectionStrings["db_ConnectionString"].ToString();
            string strsql = "select id, username, password from tb_user where username='" + username + "'";
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                SqlDataReader rd = new SqlCommand(strsql, con).ExecuteReader();
                if (rd.Read())
                {
                    string hashed = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
                    if (hashed == rd["password"].ToString())
                    {
                        FormsAuthentication.SetAuthCookie(rd["id"].ToString(), false);
                        Response.Redirect("Dashboard.aspx");
                    }
                    else
                    {
                        DisplayError("您的输入有误，请核对后重新登录！" + hashed + ":" + rd["id"].ToString());
                    }
                }
                else
                {
                    DisplayError("请求错误！");
                }
            }
        }

        private void DisplayError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visible = true;
        }

    }
}