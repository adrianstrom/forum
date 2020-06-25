﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Forum_Team2.Adminpanel
{
    public partial class Rediger_bruker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            Administrator admin = new Administrator();
            admin.UpdateProfileAdmin(txtUsername,txtPassword,txtGender,txtEducation,txtDescription);
        }

        protected void btnEditRole_Click(object sender, EventArgs e)
        {
            Administrator admin = new Administrator();
            admin.EditRole(txtUsernameRole, ddlRole);
        }
    }
}