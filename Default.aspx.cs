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
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Member member = new Member();
            member.PasteDataCategoriesUpdated(phCategories);
            member.PasteDataNewTopics(phNewTopics);
            member.PasteDataPopularTopics(phPopularTopics);
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("/register.aspx");
        }
    }
}