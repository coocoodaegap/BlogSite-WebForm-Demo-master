using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlogSite.Components
{
    public partial class GenresBox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataBase();
            }
        }
        public void BindDataBase()
        {
            //实例化SqlConnection对象
            SqlConnection sqlCon = new SqlConnection();
            //实例化SqlConnection对象连接数据库的字符串
            sqlCon.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["db_ConnectionString"].ToString();
            //实例化SqlDataAdapter对象
            SqlDataAdapter da = new SqlDataAdapter("select * from tb_genre", sqlCon);
            //实例化数据集DataSet
            DataSet ds = new DataSet();
            da.Fill(ds, "tb_genre");

            GenreRepeater.DataSource = ds;
            GenreRepeater.DataBind();
        }

    }
}