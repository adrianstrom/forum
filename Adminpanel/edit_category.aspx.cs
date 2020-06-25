using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Forum_Team2.Adminpanel
{
    public partial class Rediger_kategorier : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
           

        }

        protected void btnCreateCategory_Click(object sender, EventArgs e) // OnClick
        {
            Administrator admin = new Administrator(); // Creating an object out of the class Administrator.
            admin.CreateCategory(txtCategoryName, txtImage); // Accessing the method within the class.
        }

        protected void btnDeleteCategory_Click(object sender, EventArgs e) // OnClick
        {
            Administrator admin = new Administrator(); // Creating an object out of the class Administrator.
            admin.DeleteCategory(ddlCategoryNameDelete); // Accessing the method within the class.
        }

        protected void btnCreateSubCategory_Click(object sender, EventArgs e) // OnClick
        {
            Administrator admin = new Administrator(); // Creating an object out of the class Administrator.
            admin.CreateSubCategory(txtSubCategoryName); // Accessing the method within the class.
        }

        protected void btnDeleteSubCategory_Click(object sender, EventArgs e) // OnClick
        {
            Administrator admin = new Administrator(); // Creating an object out of the class Administrator.
            admin.DeleteSubCategory(ddlSubCategoryNameDelete); // Accessing the method within the class.
        }

        protected void btnCategorySubCategory_Click(object sender, EventArgs e)
        {

            Administrator admin = new Administrator(); // Creating an object out of the class Administrator.
            admin.MakeSubCategoryBelongToCategory(ddlCategory, ddlSubCategory); // Accessing the method within the class.

        }

        protected void btnDeleteTopic_Click(object sender, EventArgs e)
        {
            Administrator admin = new Administrator(); // Creating an object out of the class Administrator.
            admin.DeleteTopic(ddlTopicDelete); // Accessing the method within the class.
        }

        protected void btnDeleteComment_Click(object sender, EventArgs e)
        {
            Administrator admin = new Administrator(); // Creating an object out of the class Administrator.
            admin.DeleteComment(ddlCommentDelete); // Accessing the method within the class.
        }
    }
}