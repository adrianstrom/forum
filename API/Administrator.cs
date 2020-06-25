using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace Forum_Team2
{
    public class Administrator : Member
    {
        // Connection string.
        SqlConnection con = new SqlConnection
        (ConfigurationManager.ConnectionStrings["ForumDatabaseConnectionString"].ConnectionString);

        // Current.
        HttpContext context = HttpContext.Current;

        static string ComputeSha256Hash(string rawData) // https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
        {
            //Mahesh Chand, published on sep 16, 2018. Downloaded 16.04.2019.

            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public void UpdateProfileAdmin(TextBox txtUserName, TextBox txtPassword, TextBox txtGender,
            TextBox txtEducation, TextBox txtDesc)
        {
            try
            {
                // Inputs
                string userName = txtUserName.Text;
                string password = ComputeSha256Hash(txtPassword.Text);
                string gender = txtGender.Text;
                string education = txtEducation.Text;
                string description = txtDesc.Text;

                con.Open(); // Opens connection.

                SqlCommand cmd = new SqlCommand("uspUpdateProfileAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure; // Stored procedure.
                cmd.Parameters.AddWithValue("@username", userName); // Parameter username.
                cmd.Parameters.AddWithValue("@password", password); // Parameter password.
                cmd.Parameters.AddWithValue("@gender", gender); // Parameter gender.
                cmd.Parameters.AddWithValue("@education", education); // Parameter education.
                cmd.Parameters.AddWithValue("@description", description); // Parameter description.

                cmd.ExecuteNonQuery(); // Executes Stored Procedure.
                con.Close(); // Closes connction.
                SuccessAlert(userName + " er oppdatert!");

            }
            catch (Exception error)
            {
                FailAlert(error.ToString());
            }

        }

        public void CreateCategory(TextBox txtCategoryName, TextBox txtImg)
        {
            try
            {
                // Inputs.
                string categoryName = txtCategoryName.Text;
                string img = txtImg.Text;

                con.Open(); // Opens connection.

                SqlCommand cmd = new SqlCommand("uspCreateCategory", con);
                cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                cmd.Parameters.AddWithValue("@categoryname", categoryName); // Parameter categoryname.
                cmd.Parameters.AddWithValue("@categoryimage", img); // Parameter img.

                cmd.ExecuteNonQuery(); // Executes Stored Procedure.
                con.Close(); // Closes connection.
                SuccessAlert("Kategorien " + categoryName + " er opprettet!");
            }
            catch (Exception error)
            {
                FailAlert(error.ToString());
            }
        }
        public void DeleteCategory(DropDownList ddlCategoryId)
        {
            try
            {
                con.Open(); // Opens connection.

                string categoryId = ddlCategoryId.SelectedItem.Value;

                SqlCommand cmd = new SqlCommand("uspDeleteCategory", con);
                cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                cmd.Parameters.AddWithValue("@categoryid", categoryId); // Paramater categoryid.

                cmd.ExecuteNonQuery(); // Executes Stored Procedure.
                con.Close(); // Closes connection.
                SuccessAlert("Kategorien er slettet!");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                FailAlert("Denne kategorien kan ikke fjernes fordi den er avhengig av at subkategoriene som hører til denne kategorien blir fjernet.");
            }
            catch (Exception error)
            {
                FailAlert(error.ToString());
            }
        }
        public void CreateSubCategory(TextBox txtSubCategoryName)
        {
            try
            {
                string subCategoryName = txtSubCategoryName.Text;

                con.Open(); // Opens connection.

                SqlCommand cmd = new SqlCommand("uspCreateSubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                cmd.Parameters.AddWithValue("@subcategoryname", subCategoryName); // Paramater  subcategoryname.

                cmd.ExecuteNonQuery(); // Executes Stored Procedure.
                con.Close(); // Closes connection.
                SuccessAlert("Subkategorien " + subCategoryName + " ble opprettet!");
            }
            catch (Exception error)
            {
                FailAlert(error.ToString());
            }
        }
        public void DeleteSubCategory(DropDownList ddlSubCategoryId)
        {
            try
            {
                con.Open(); // Opens connection.

                string subCategoryId = ddlSubCategoryId.SelectedItem.Value;

                SqlCommand cmd = new SqlCommand("uspDeleteSubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                cmd.Parameters.AddWithValue("@subcategoryid", subCategoryId); // Parameter subcategoryid.

                cmd.ExecuteNonQuery(); // Execute Stored Procedure.
                con.Close();
                SuccessAlert("Subkategorien " + ddlSubCategoryId.SelectedItem.Text + " ble slettet!");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                FailAlert("Denne subkategorien kan ikke fjernes fordi den er avhengig av at emnene som hører til subkategorien blir fjernet.");
            }
            catch (Exception error)
            {
                FailAlert(error.ToString());
            }
        }
        public void DeleteTopic(DropDownList ddlTopicId)
        {
            try
            {
                // Inputs.
                string topicId = ddlTopicId.SelectedItem.Value;

                con.Open(); // Opens connection.

                SqlCommand cmd = new SqlCommand("uspDeleteTopic", con);
                cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                cmd.Parameters.AddWithValue("@topicid", topicId); // Parameter topicid.

                cmd.ExecuteNonQuery(); // Executes Stored Procedure.
                con.Close(); // Closes connection.

                SuccessAlert("Emnet " + ddlTopicId.SelectedItem.Text + " ble slettet!");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                FailAlert("Dette emnet kan ikke fjernes fordi den er avhengig av at kommentarene som hører til emnet blir fjernet.");
            }
            catch (Exception error)
            {
                FailAlert(error.ToString());
            }
        }
        public void DeleteComment(DropDownList ddlCommentId)
        {
            try
            {
                // Inputs.
                string commentId = ddlCommentId.SelectedItem.Value;

                // Stored procedure and parameters.
                SqlCommand cmd = new SqlCommand("uspDeleteComment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@commentid", commentId); // Parameter commentid.

                con.Open(); // Opens connection.
                cmd.ExecuteNonQuery(); // Executes.
                con.Close(); // Closes connection.
            }
            catch (Exception error)
            {
                FailAlert(error.ToString());
            }
        }
        public void EditRole(TextBox txtUserName, DropDownList ddlRole)
        {
            try
            {
                // Inputs.
                string userName = txtUserName.Text;
                string roleId = ddlRole.SelectedItem.Value;
                string roleName = ddlRole.SelectedItem.Text;

                SqlCommand cmdCheckUserName = new SqlCommand("uspGetUserNames", con);
                cmdCheckUserName.CommandType = CommandType.StoredProcedure;
                cmdCheckUserName.Parameters.AddWithValue("@username", userName); // Username

                SqlDataAdapter sda = new SqlDataAdapter(cmdCheckUserName);  // Creates object of class SqlDataAdapter.
                DataTable dt = new DataTable(); // Creates object of class DataTable.
                sda.Fill(dt); // Fills DataTable.
                con.Open(); // Opens connections.
                cmdCheckUserName.ExecuteNonQuery(); // Executes.
                con.Close(); // Closes connection.

                if (dt.Rows.Count != 0) // If there exist a user with the username.
                {
                    // Stored procedure and parameters.
                    SqlCommand cmd = new SqlCommand("uspEditRole", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@roleid", roleId); // Parameter roleid.
                    cmd.Parameters.AddWithValue("@username", userName); // Paramter username.

                    con.Open(); // Opens connection.
                    cmd.ExecuteNonQuery(); // Executes.
                    con.Close(); // Closes connection.
                    SuccessAlert(userName + " har nå rollen: " + roleName);
                } else // No user with that username.
                {
                    FailAlert("Ingen bruker eksisterer med det brukernavnet: " + userName);
                }
            }
            catch (Exception error)
            {
                FailAlert(error.ToString());
            }

        }
        public void BanUser(TextBox txtUserName)
        {
            try
            {
                // Inputs.
                string userName = txtUserName.Text;

                // Stored procedure and parameters.
                SqlCommand cmd = new SqlCommand("uspBanUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", userName); // Paramter username.

                con.Open(); // Opens connection.
                cmd.ExecuteNonQuery(); // Executes.
                con.Close(); // Closes connection.
                SuccessAlert(userName + " ble nå utestengt!");

            }
            catch (Exception error)
            {
                FailAlert(error.ToString());
            }


        }  
        public void MakeSubCategoryBelongToCategory(DropDownList ddlCategory, DropDownList ddlSubCategory)
        {
            string categoryName = ddlCategory.SelectedItem.Text;
            string subCategoryName = ddlSubCategory.SelectedItem.Text;

            con.Open();

            string sql = "INSERT INTO Category_SubCategory(categoryid, subcategoryid)" +
                " VALUES(@categoryid, @subcategoryid)";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@categoryid", Convert.ToInt32(ddlCategory.SelectedItem.Value));
            cmd.Parameters.AddWithValue("@subcategoryid", Convert.ToInt32(ddlSubCategory.SelectedItem.Value));

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Close();

            SuccessAlert("Subkategorien " + subCategoryName + " hører nå til kategorien " + categoryName);
        }

    }
}