using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Forum_Team2
{
    public partial class register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Authenticator authenticator = new Authenticator();
            authenticator.LoggedIn();
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Authenticator authenticator = new Authenticator();
            authenticator.Register(txtUserName, txtPassword, txtPasswordCheck, txtEmail, ddlGender,
                txtEducation, chkTermsOfService);
        }

        protected void btnActivate_Click(object sender, EventArgs e)
        {
            Authenticator authenticator = new Authenticator();
            authenticator.Confirmation(txtUserNameCert, txtCertificationCode);
        }
    }
}
