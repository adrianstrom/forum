using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Forum_Team2
{
    public partial class memberlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Member member = new Member();
            member.ViewMemberList(phMemberlist);
        }
    }
}