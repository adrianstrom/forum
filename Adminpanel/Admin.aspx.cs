using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Forum_Team2.Adminpanel
{

    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAlertUsers_Click(object sender, EventArgs e)
        {
            string subject = txtSubject.Text;
            string body = txtBody.Text;

            Authenticator authenticator = new Authenticator();
            authenticator.MassSendEmails(subject, body);
        }
    }
}