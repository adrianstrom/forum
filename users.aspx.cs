using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Forum_Team2
{
    public partial class users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Member member = new Member();
            member.PasteDataUsers(phProfile, phDesc);
            member.UserActivity(phUserActivity);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}