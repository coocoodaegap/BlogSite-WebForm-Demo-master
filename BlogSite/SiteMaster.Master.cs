using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlogSite
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        string UserNameNav;
        public string MVal { get { return UserNameNav; } set { UserNameNav = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                // 显示用户信息和下拉菜单，隐藏登录按钮
                userMenu.Visible = true;
                loginButton.Visible = false;

                // 设置用户名
                string strcon = System.Configuration.ConfigurationManager.ConnectionStrings["db_ConnectionString"].ToString();
                string strsql = "select username from tb_user where id='" + HttpContext.Current.User.Identity.Name + "'";
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    con.Open();
                    SqlDataReader rd = new SqlCommand(strsql, con).ExecuteReader();
                    if (rd.Read())
                    {
                        UserNameNav = rd["username"].ToString();
                    }
                }
            }
            else
            {
                // 未登录时，显示登录按钮，隐藏下拉菜单
                userMenu.Visible = false;
                loginButton.Visible = true;
            }
        }
    }
}