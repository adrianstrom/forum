using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Forum_Team2
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] != null)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ForumDatabaseConnectionString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("Select roleid FROM users WHERE username = @username", con);
                cmd.Parameters.AddWithValue("@username", Session["Username"]);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                string roleid = dt.Rows[0][0].ToString();
                con.Close();

                if (roleid == "2") // Fanger opp brukerrollen (bruker, admin). Kolonne 0 er roleid i DataTable.
                {
                    litAdmin.Text = "<li><a href ='/Adminpanel/Admin.aspx'><span class='glyphicon glyphicon-signal'></span>" + " " + "Admin</a></li>";
                } else
                {
                    litAdmin.Text = "<li><a href ='/logout.aspx'><span class='glyphicon glyphicon-log-out'></span>" + " " + "Logg ut</a></li>";
                    
                }
              


                litNavbarRight.Text = "<li><a href ='/users/Username=" + Session["Username"] + "'><span class='glyphicon glyphicon-user'></span>" + " " + Session["Username"] + "</a></li>";
                
                litNavbarLogIn.Text = "<li><a href ='/edit/Username="+ Session["Username"] + "'><span class='glyphicon glyphicon-cog'></span>" + " " + "Min profil</a></li>";
            }
            else
            {
                litNavbarRight.Text = "<li><a href ='/register.aspx'><span class='glyphicon glyphicon-user'></span> Registrer deg</a></li>";
                litNavbarLogIn.Text = "<li><a href ='/login.aspx'><span class='glyphicon glyphicon-log-in'></span>" + " " + "Logg inn</a></li>";
            }


        }

        
    }
}