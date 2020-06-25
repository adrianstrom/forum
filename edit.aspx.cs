using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

namespace Forum_Team2
{
    public partial class edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Authenticator authenticator = new Authenticator();
            authenticator.CheckSession();
        }

        protected void btnUpdateUser_Click(object sender, EventArgs e)
        {
            Member member = new Member();
            member.UpdateProfile(txtImage, txtPassword, txtGender, txtEducation, txtDescription);
        }

        protected void btnGetInfo_Click(object sender, EventArgs e)
        {
            Member member = new Member();
            member.CustomizeProfile(txtImage, txtPassword, txtGender, txtEducation, txtDescription);
        }

        protected void btnDeleteData_Click(object sender, EventArgs e)
        {
            Member member = new Member();
            member.DeleteData();
        }
    }
}