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

namespace Forum_Team2
{
    public class Moderator : Member
    {
        // Connection string.
        SqlConnection con = new SqlConnection
        (ConfigurationManager.ConnectionStrings["ForumDatabaseConnectionString"].ConnectionString);

        // Accesses current requests.
        HttpContext context = HttpContext.Current;

        public void DeleteTopic(TextBox txtTopicId)
        {
            // Inputs
            string topicId = txtTopicId.Text;

            con.Open(); // Opens connection.

            // Stored procedure and parameters.
            SqlCommand cmd = new SqlCommand("uspDeleteTopic", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@topcid", topicId); // Parameter topicid.

            cmd.ExecuteNonQuery(); // Executes.
            con.Close(); // Closes connection.
        }
        public void DeleteComment(TextBox txtCommentId)
        {
            string commentId = txtCommentId.Text;

            con.Open(); // Opens connection.

            // Stored procedure and parameters.
            SqlCommand cmd = new SqlCommand("uspDeleteComment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@commentid", commentId); // Parameter commentid.

            cmd.ExecuteNonQuery(); // Executes.
            con.Close(); // Closes connection.
        }
        public void BanUser()
        {

        }
    }
}