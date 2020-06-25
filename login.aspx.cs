using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Forum_Team2
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Authenticator authenticator = new Authenticator();
            authenticator.LoggedIn();
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            Authenticator authenticator = new Authenticator(); // Creates object out of class Authenticator.
            authenticator.LogIn(txtUsername, txtPassword); // Calls up method LogIn().

        }
    }
}