using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Forum_Team2.Adminpanel
{
    public partial class ban_user : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBan_Click(object sender, EventArgs e)
        {
            Administrator admin = new Administrator();
            admin.BanUser(txtUserName);
        }
    }
}