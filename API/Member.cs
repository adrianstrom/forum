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
using System.Threading;

namespace Forum_Team2
{
    public class Member
    {
        // Connection string
        SqlConnection con = new SqlConnection
        (ConfigurationManager.ConnectionStrings["ForumDatabaseConnectionString"].ConnectionString);

        // Current
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

        public void SuccessAlert(string message)
        {
            context.Response.Write("<div class='alert alert-success'><strong>" + message + "</strong></div>");
        }

        public void FailAlert(string message)
        {
            context.Response.Write("<div class='alert alert-danger'><strong>" + message + "</strong></div>");
        }

        public void CategoryInfoPanel(PlaceHolder phDisplay)
        {
            StringBuilder table = new StringBuilder(); // String object that can be modified.

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspPasteDataCategoriesUpdated", con);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.

            SqlDataReader rd = cmd.ExecuteReader(); // Executes reader.

            // Starts div.
            table.Append("<div class='row'>");
            if (rd.HasRows) // If reader has rows.
            {
                while (rd.Read()) // Loops through data.
                {
                    // Assigns values to variables.
                    object categoryId = rd[0];
                    object categoryName = rd[1];
                    object categoryImage = rd[2];
                    object categoryDescription = rd[3];

                    // Some sort of "table".
                    table.Append("<div class='col-sm-3 text-center' style='margin-top:40px'>" +
                        "<a href=subcategories.aspx/categoryid=" + categoryId + "><img src='" + categoryImage + "' " +
                        "class='rounded-circle' style='width: 128;height: 128px'/>" +
                        "<h1 style='font-size:20pt'><b>" + categoryName + "</h1></b></a>" +
                        "<h4>" + rd[3] + "</h3></div>");
                }
            }
            table.Append("</div>"); // Ends div.
            phDisplay.Controls.Add(new Literal
            {
                Text = table.ToString()
            }
             );
            rd.Close(); // Closes reader.
            con.Close(); // Closes connection.
        }

        public void UpVoteSystem(Button btnUpVote)
        {
            try
            {
                // https://www.codeproject.com/Questions/823002/How-to-clear-Request-Querystring (for four lines of code below).
                string url = HttpContext.Current.Request.Url.AbsoluteUri; // Gets URL.
                string[] separateURL = url.Split('?'); // Splits URL.
                NameValueCollection queryString = HttpUtility.ParseQueryString(separateURL[0]); // Parameters.
                string topicId = queryString[0];  // Index 0 is topicId.

                if (context.Session["username"] != null) // If username is not null.
                {
                    // Inputs
                    string userId = context.Session["userid"].ToString(); // userId is session userid.

                    con.Open(); // Opens connection.

                    // Creates vote with a stored procedure.
                    SqlCommand cmd = new SqlCommand("uspCreateVote", con);
                    cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                    cmd.Parameters.AddWithValue("@userId", userId);

                    // Gets vote from stored procedure.
                    SqlCommand cmd2 = new SqlCommand("uspGetVote", con);
                    cmd2.CommandType = CommandType.StoredProcedure; // Stored Procedure.

                    SqlDataAdapter sda = new SqlDataAdapter(cmd2);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    string voteId = dt.Rows[0][0].ToString(); // If autoincrementid starts at 0 = dt.Rows.Count, if 1 = dt.Rows.Count - 1; .... 

                    if (voteId == "NULL") // On initalization (identity needs to start at 0).
                    {
                        voteId = "1";
                    }
                    cmd2.ExecuteNonQuery(); // Executes second operation.

                    // Insert data into Topic_Vote
                    SqlCommand cmd3 = new SqlCommand("uspCreateTopicVote", con);
                    cmd3.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                    cmd3.Parameters.AddWithValue("@topicId", topicId); // Parameter topicId.
                    cmd3.Parameters.AddWithValue("@voteId", voteId); // Parameter voteId.

                    cmd.ExecuteNonQuery(); // Executes first operation.
                    cmd3.ExecuteNonQuery(); // Executes third operation.

                    con.Close(); // Closes connection.
                    btnUpVote.Visible = false;
                }
                else // Not logged in.
                {
                    context.Response.Redirect("/Default.aspx");
                }
            }
            catch(System.Data.SqlClient.SqlException)
            {
                FailAlert("En feil har oppstått, prøv igjen!");
            }

        }

        public void PasteDataNewTopics(PlaceHolder phDisplay)
        {

            StringBuilder table = new StringBuilder(); // String object that can be modified.

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspPasteDataNewTopics", con);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
            SqlDataReader rd = cmd.ExecuteReader(); // Executes reader.

            if (rd.HasRows) // If reader has rows.
            {
                // Table.
                table.Append("<table align='center' width='100%' class='table table'");
                table.Append("<tr> " +
                    "<th><b>Emnenavn</b></th> " + // Topicname (head of table).
                    "<th><b>Brukernavn</b></th> " + // Username (head of table).
                    "<th><b>Tid</b></th>"); // Time (head of table).
                table.Append("</tr>");

                while (rd.Read()) // Loops through data.
                {
                    // Assigns values to variables.
                    object topicId = rd[0];
                    object topicName = rd[1];
                    object userName = rd[2];
                    object topicTime = rd[3];

                    // Table.
                    table.Append("<tr>");
                    table.Append("<td><a href='comments.aspx/topicid=" + topicId + "'>" + topicName + "</a></td>");
                    table.Append("<td><a href='users.aspx/Username=" + userName + "'>" + userName + "</a></td>");
                    table.Append("<td>" + topicTime + "</td>");
                    table.Append("</tr>");
                }
            } else
            {
                table.Append("<br><p align='center'><i>Det eksisterer ingen emnner!</i></p>");
            }
            table.Append("</table>"); // Ends table.
            phDisplay.Controls.Add(new Literal
            {
                Text = table.ToString()
            }
             );
            rd.Close(); // Closes reader.
            con.Close(); // Closes connection.
        }

        public void PasteDataPopularTopics(PlaceHolder phDisplay)
        {

            StringBuilder table = new StringBuilder(); // // String object that can be modified.

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspPasteDataPopularTopics", con);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.

            SqlDataReader rd = cmd.ExecuteReader(); // Executes reader.

            if (rd.HasRows) // If reader has rows.
            {
                // Table.
                table.Append("<table align='center' width='100%' class='table table'");
                table.Append("<tr> " +
                    "<th><b>Votes</b></th> " + // Votes (head of table).
                    "<th><b>Emnenavn</b></th> " + // Topicname (head of table).
                    "<th><b>Brukernavn</b></th>"); // Username (head of table).
                table.Append("</tr>");

                while (rd.Read()) // Loops through data.
                {
                    // Assings values to variables.
                    object votes = rd[0];
                    object topicId = rd[1];
                    object topicName = rd[2];
                    object userName = rd[3];

                    // Table.
                    table.Append("<tr>");
                    table.Append("<td>" + votes + "</td>");
                    table.Append("<td><a href='comments.aspx/topicid=" + topicId + "'>" + topicName + "</a></td>");
                    table.Append("<td><a href='users.aspx/Username=" + userName + "'>" + userName + "</a></td>");
                    table.Append("</tr>");
                }
            } else
            {
                table.Append("<br><p align='center'><i>Ingen emner er stemt opp!</i></p>");
            }
            
            table.Append("</table>"); // Ends table.
            phDisplay.Controls.Add(new Literal
            {
                Text = table.ToString()
            }
             );
            rd.Close(); // Closes reader.
            con.Close(); // Closes connection.
        }

        public void PasteDataCategoriesUpdated(PlaceHolder phDisplay)
        {
            StringBuilder table = new StringBuilder(); // String object that can be modified.

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspPasteDataCategoriesUpdated", con);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.

            SqlDataReader rd = cmd.ExecuteReader(); // Executes reader.

            // Starts div.
            table.Append("<div class='row'>");
            if (rd.HasRows) // If reader has rows.
            {
                while (rd.Read()) // Loops through data.
                {
                    // Assigns values to variables.
                    object categoryId = rd[0];
                    object categoryName = rd[1];
                    object categoryImage = rd[2];

                    // Some sort of "table".
                    table.Append("<div class='col-sm-3 text-center' style='margin-top:40px'>" +
                        "<a href=subcategories.aspx/categoryid=" + categoryId + "><img src='" + categoryImage + "' " +
                        "class='rounded-circle' style='width: 128;height: 128px'/>" +
                        "<h1 style='font-size:20pt'><b>" + categoryName + "</h1></b></a></div>");
                }
            } else
            {
                table.Append("<br><p align='center'><i>Det eksisterer ingen kategorier!</i></p>");
            }

            table.Append("</div>"); // Ends div.
            phDisplay.Controls.Add(new Literal
            {
                Text = table.ToString()
            }
             );
            rd.Close(); // Closes reader.
            con.Close(); // Closes connection.

        }

        public void PasteDataSubCategories(PlaceHolder phDisplay)
        {
            // https://www.codeproject.com/Questions/823002/How-to-clear-Request-Querystring (for the four lines of code below).
            string url = context.Request.Url.AbsoluteUri; // Gets URL.
            string[] separateURL = url.Split('?'); // Splits URL.
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(separateURL[0]); // Parameters.
            string categoryId = queryString[0];  // Index 0 is categoryId.

            StringBuilder table = new StringBuilder(); // String object that can be modified.

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspPasteDataSubCategories", con);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
            cmd.Parameters.AddWithValue("@categoryid", categoryId); // Parameter categoryid.

            SqlDataReader rd = cmd.ExecuteReader(); // Executes reader.

            // Start table.
            table.Append("<table align='center' width='100%' class='table table'");
            table.Append("<tr> <th><b>Subkategorier</b></th>");
            table.Append("</tr>");

            if (rd.HasRows) // If reader has rows.
            {
                while (rd.Read()) // Loops through data.
                {
                    // Assign values to variables.
                    object subCategoryId = rd[0];
                    object subCategoryName = rd[1];

                    // Table.
                    table.Append("<tr>");
                    table.Append("<td><span class='glyphicon glyphicon-globe'></span>" + " " + "" +
                        "<a href=/topics.aspx/subcategoryid=" + subCategoryId + ">" + subCategoryName + "</a>" + "</td>");
                    table.Append("</tr>");
                }
            }
            table.Append("</table>"); // Ends table.
            phDisplay.Controls.Add(new Literal
            {
                Text = table.ToString()
            }
             );
            rd.Close(); // Closes reader.
            con.Close(); // Closes connection.
        }

        public void PasteDataTopics(PlaceHolder phDisplay)
        {
            // https://www.codeproject.com/Questions/823002/How-to-clear-Request-Querystring (used in four lines of code below).
            string url = context.Request.Url.AbsoluteUri; // Gets URL.
            string[] separateURL = url.Split('?'); // Splits URL.
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(separateURL[0]); // Parameters.
            string subCategoryId = queryString[0];  // Index 0 is subCategoryId.

            StringBuilder table = new StringBuilder(); // String object that can be modified.

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspPasteDataTopics", con);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
            cmd.Parameters.AddWithValue("@subcategoryid", subCategoryId); // Parameter subCategoryId.

            SqlDataReader rd = cmd.ExecuteReader(); // Executes Reader.

            // Table start.
            table.Append("<table align='center' width='100%' class='table table'");
            table.Append("<tr> " +
                "<th><b>Emne</b></th>" + // topicname (head of table).
                " <th><b>Brukernavn</b></th>" + // username (head of table).
                " <th><b>Tid</b></th>"); // time (head of table).
            table.Append("</tr>");

            if (rd.HasRows) // If reader has rows.
            {
                while (rd.Read()) // Loops through data.
                {
                    // Assign values to variables.
                    object topicId = rd[0];
                    object topicName = rd[1];
                    object userName = rd[2];
                    object topicTime = rd[3];

                    // Table.
                    table.Append("<tr>");
                    table.Append("<td><a href='/comments/topicid=" + topicId + "'>" + topicName + "</td>");
                    table.Append("<td><a href='/users.aspx/Username=" + userName + "'>" + userName + "</td>");
                    table.Append("<td>" + topicTime + "</td>");
                    table.Append("</tr>");
                }
            }
            table.Append("</table>"); // Ends table.
            phDisplay.Controls.Add(new Literal
            {
                Text = table.ToString()
            }
             );
            rd.Close(); // Closes reader.
            con.Close(); // Closes connection.
        }

        public void PasteDataTopicInTopics(PlaceHolder phDisplay)
        {
            // https://www.codeproject.com/Questions/823002/How-to-clear-Request-Querystring (for the four lines of code below).
            string url = context.Request.Url.AbsoluteUri; // Gets URL.
            string[] separateURL = url.Split('?'); // Splits URL.
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(separateURL[0]); // Parameters.
            string topicId = queryString[0];  // Index 0 is topicId.

            StringBuilder display = new StringBuilder(); // String object that can be modified.

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspPasteDataTopicInTopics", con);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
            cmd.Parameters.AddWithValue("@topicid", topicId); // Parameter topicid.

            SqlDataReader rd = cmd.ExecuteReader(); // Executes reader.

            display.Append("<div class='container'><br><div class='media'>"); // Starts divs.

            if (rd.HasRows) // If rd has rows.
            {
                while (rd.Read()) // Loops through data.
                {
                    // Assigns values to variables.
                    object userName = rd[0];
                    object topicName = rd[1];
                    object topicText = rd[2];
                    object topicTime = rd[3];
                    object userImage = rd[4];
                    
                    // Append
                    display.Append("" +
                        "<div class='media-left'>" +
                        "<img src='" + userImage + "' class='media-object' style='width:60px'></div>" +
                        "<div class='media-body'>" +
                        "<h4 class='media-heading'><b>" + userName + "</b></h4>" +
                        "<p>Emnenavn:<b> " + topicName + "</b></p>" + topicText + "</div><br>");
                }
            }
            display.Append("</div></div>"); // Ends divs.
            phDisplay.Controls.Add(new Literal
            {
                Text = display.ToString()
            }
             );
            rd.Close(); // Closes reader.
            con.Close(); // Cloes connection.
        }

        public void PasteDataComments(PlaceHolder phDisplay)
        {
            // https://www.codeproject.com/Questions/823002/How-to-clear-Request-Querystring (for the four lines of code below).
            string url = context.Request.Url.AbsoluteUri; // Gets URL.
            string[] separateURL = url.Split('?'); // Splits URL.
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(separateURL[0]); // Parameters.
            string topicId = queryString[0];  // Index 0 is topicId.

            StringBuilder display = new StringBuilder(); // String object that can be modified.

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspPasteDataComments", con);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
            cmd.Parameters.AddWithValue("@topicid", topicId); // Parameter topicid.

            SqlDataReader rd = cmd.ExecuteReader(); // Executes reader.

            // Starts divs.
            display.Append("<div class='container'><div class='media'>");

            if (rd.HasRows) // If reader has rows.
            {
                while (rd.Read()) // Loops through data.
                {
                    // Variables.
                    object userName = rd[0];
                    object commentText = rd[1];
                    object commentTime = rd[2];
                    object userImage = rd[3];

                    display.Append("" +
                        "<div class='media-left'>" +
                        "<img src='"+ userImage +"' " +
                            "class='media-object' style='width:60px'></div>" +

                        "<div class='media-body'>" +

                        "<h4 class='media-heading'><b>" + userName + "</b></h4><i> " + commentTime + "</i></p>" +
                        "<p>" + commentText + "</p></div><br><br>");
                }
            }
            display.Append("</div></div>"); // Ends div.


            phDisplay.Controls.Add(new Literal
            {
                Text = display.ToString()
            }
             );
            rd.Close(); // Closes reader.
            con.Close(); // Closes connection.

        }

        public void PasteDataUsers(PlaceHolder phProfile, PlaceHolder phDesc)
        {
            // https://www.codeproject.com/Questions/823002/How-to-clear-Request-Querystring (for four lines of code below).
            string url = context.Request.Url.AbsoluteUri; // Gets URL.
            string[] separateURL = url.Split('?'); // Splits URL.
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(separateURL[0]); // Parameters.
            string userName = queryString[0];  // Index 0 is userName.

            StringBuilder profile = new StringBuilder(); // String object that can be modified.
            StringBuilder description = new StringBuilder(); // String object that can be modified.

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspPasteDataUsersWithUpvotes", con); // SQL for users with upvotes recieved.
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
            cmd.Parameters.AddWithValue("@username", userName); // Parameter userName.

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            cmd.ExecuteNonQuery(); // Executes query.

            if (dt.Rows.Count > 0)
            {
                // Gives variables values.
                string username = dt.Rows[0][0].ToString();
                string desc = dt.Rows[0][1].ToString();
                string gender = dt.Rows[0][2].ToString();
                string education = dt.Rows[0][3].ToString();
                string image = dt.Rows[0][4].ToString();
                string role = dt.Rows[0][5].ToString();
                string upvotes = dt.Rows[0][6].ToString();

                // Append into profile.
                profile.Append("<h3 align='center'><b>" + username + "</b></h3>");
                profile.Append("<h5 align='center'><i>" + role + "</i></h6>");
                profile.Append("<img width='200' height='175' style='display:block;margin-left: auto; margin-right: auto;' " +
                                "src='" + image + "'>" + "<br>");
                 
                // Append into description.
                description.Append("<b>Beskrivelse:</b> " + desc + "<br>");
                description.Append("<b>Kjønn:</b> " + gender + "<br>");
                description.Append("<b>Utdanning:</b> " + education + "<br>");
                description.Append("<b>Upvotes mottatt:</b> " + upvotes + "<br>");
            }
            else // If user has no upvotes, a new stored procedure will be ran.
            {

                SqlCommand cmdNoUpvotes = new SqlCommand("uspPasteDataUsersWithoutUpvotes", con); // SQL for users without upvotes recieved.
                cmdNoUpvotes.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                cmdNoUpvotes.Parameters.AddWithValue("@username", userName); // Parameter userName.

                SqlDataAdapter sdaNoUpvotes = new SqlDataAdapter(cmdNoUpvotes);
                DataTable dtNoUpvotes = new DataTable();
                sdaNoUpvotes.Fill(dtNoUpvotes);
                cmdNoUpvotes.ExecuteNonQuery();

                // Gives variables values.
                string username = dtNoUpvotes.Rows[0][0].ToString();
                string desc = dtNoUpvotes.Rows[0][1].ToString();
                string gender = dtNoUpvotes.Rows[0][2].ToString();
                string education = dtNoUpvotes.Rows[0][3].ToString();
                string image = dtNoUpvotes.Rows[0][4].ToString();
                string role = dtNoUpvotes.Rows[0][5].ToString();

                // Profile append.
                profile.Append("<h3 align='center'><b>" + username + "</b></h3>");
                profile.Append("<h5 align='center'><i>" + role + "</i></h6>");
                profile.Append("<img width='200' height='175' " +
                    "style='display:block;margin-left: auto; margin-right: auto;' src='" + image + "'>" + "<br>"); 
                
                // Description append.
                description.Append("<b>Beskrivelse:</b> " + desc + "<br>");
                description.Append("<b>Kjønn:</b> " + gender + "<br>");
                description.Append("<b>Utdanning:</b> " + education + "<br>");


            }
            con.Close(); // Closes connection.
                

            phProfile.Controls.Add(new Literal
            {
                Text = profile.ToString()
            }
             );

            phDesc.Controls.Add(new Literal
            {
                Text = description.ToString()
            }
             );

        }

        public void CreateTopic(TextBox txtTopicName, TextBox txtTopicText)
        {

            /* Order of execution:
            * 1. First get topicid (cmd2).
            * 2. Then create a topic (cmd).
            * 3. Then make a topic belong to a subcategory (cmd3).
            */
            try
            {
                if (context.Session["username"] != null) // If session username is not null. (Logged in)
                {
                    // Gets subCategoryId from URL.
                    // https://www.codeproject.com/Questions/823002/How-to-clear-Request-Querystring (for three lines of code below).
                    string url = context.Request.Url.AbsoluteUri; // Gets URL.
                    string[] separateURL = url.Split('?'); // Splits URL.
                    NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(separateURL[0]); // Parameters.

                    // Inputs
                    string userId = context.Session["userid"].ToString(); // Session userId.
                    string topicName = txtTopicName.Text;
                    string topicText = txtTopicText.Text;
                    int topicNameCharacters = txtTopicName.Text.Length;
                    int topicTextCharacters = txtTopicText.Text.Length;
                    DateTime time = DateTime.Now;
                    string subCategoryId = queryString[0];  // Index 0 is subCategoryId.

                    if (4 <= topicNameCharacters && topicNameCharacters <= 25) // If topicNameCharacters is between 4 and 25.
                    {
                        if (topicTextCharacters >= 4) // If topicTextCharacters is more than 4 or 4.
                        {

                            con.Open(); // Opens connection.

                            SqlCommand cmd = new SqlCommand("uspCreateTopic", con);
                            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                            cmd.Parameters.AddWithValue("@topicname", topicName); // Parameter topicname.
                            cmd.Parameters.AddWithValue("@topictext", topicText); // Parameter topictext.
                            cmd.Parameters.AddWithValue("@userid", userId); // Parameter userid.
                            cmd.Parameters.AddWithValue("@time", time); // Parameter time.

                            SqlCommand cmd2 = new SqlCommand("uspGetTopic", con);
                            cmd2.CommandType = CommandType.StoredProcedure; // Stored Procedure.

                            SqlDataAdapter sda = new SqlDataAdapter(cmd2);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            string topicId = dt.Rows[0][0].ToString(); // If autoincrementid starts at 0 = dt.Rows.Count, if 1 = dt.Rows.Count - 1; .... 

                            if (topicId == "NULL") // On initalization (identity needs to start at 0).
                            {
                                topicId = "1";
                            }

                            cmd2.ExecuteNonQuery(); // Executes second operation (gets identity value).

                            cmd.ExecuteNonQuery(); // Executes first operation (posts topic).

                            SqlCommand cmd3 = new SqlCommand("uspInsertTopicIntoSubCategory", con);
                            cmd3.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                            cmd3.Parameters.AddWithValue("@subcategoryid", subCategoryId); // Parameter subcategoryid.
                            cmd3.Parameters.AddWithValue("@topicid", topicId); // Parameter topicid.

                            cmd3.ExecuteNonQuery(); // Executes third operation.

                            con.Close(); // Closes connection.

                            context.Response.Redirect(context.Request.Url.ToString(), true); // "Refreshes site" (autopost).

                        }
                        else // topicTextCharacters is less than 4.
                        {
                            FailAlert("Emneteksten må ha fler enn 3 karakterer.");
                        }
                    }
                    else // topicNameCharacters not between 4 and 25.
                    {
                        FailAlert("Emnenavnet må ha mellom 4 og 25 karakterer!");
                    }
                }
                else // User is not logged in.
                {
                    context.Response.Redirect("/Default.aspx"); // Redirects if not logged in.
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                FailAlert("En feil har oppstått, prøv igjen!");
            }
        }

        public void CreateComment(TextBox txtCommentText)
        {
            try
            {
                /* Order of execution:
                 * 1. First get commentid (cmd2).
                 * 2. Then create a comment (cmd).
                 * 3. Then make a comment belong to a topic (cmd3).
                 */

                if (context.Session["username"] != null) // If session username is not null
                {
                    string userId = context.Session["userid"].ToString(); // userId is session userid

                    // Gets topicId from URL.
                    // https://www.codeproject.com/Questions/823002/How-to-clear-Request-Querystring (for four lines of code below).
                    string url = context.Request.Url.AbsoluteUri; // Gets URL
                    string[] separateURL = url.Split('?'); // Splits URL
                    NameValueCollection queryString = HttpUtility.ParseQueryString(separateURL[0]); // Parameters
                    string topicId = queryString[0];  // Index 0 is topicid.

                    // Inputs
                    string commentText = txtCommentText.Text; // Input CommentText.
                    DateTime time = DateTime.Now; // Time.

                    if (commentText.Length >= 4) // If 4 or more characters are written in the comment
                    {
                        con.Open(); // Opens connection.

                        // The code below (cmd) adds a commenent in the DB.
                        SqlCommand cmd = new SqlCommand("uspCreateComment", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@commentText", commentText); /// Parameter commenttext.
                        cmd.Parameters.AddWithValue("@userId", userId); // Parameter userid.
                        cmd.Parameters.AddWithValue("@time", time); // Parameter time.

                        // The code below (cmd2) gets the identity value of commentid.
                        SqlCommand cmd2 = new SqlCommand("uspGetIdentityValueComment", con);
                        cmd2.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter sda = new SqlDataAdapter(cmd2);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        string commentId = dt.Rows[0][0].ToString();

                        if (commentId == "NULL") // On initalization (identity needs to start at 0).
                        {
                            commentId = "1";
                        }

                        cmd2.ExecuteNonQuery(); // Executes second operation.

                        // The code below (cmd3) makes a comment belong to a topic in the DB.
                        SqlCommand cmd3 = new SqlCommand("uspPasteCommentIntoTopic", con);
                        cmd3.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                        cmd3.Parameters.AddWithValue("@topicId", topicId); // Parameter topicid.
                        cmd3.Parameters.AddWithValue("@commentId", commentId); // Parameter commentid.
                        
                        cmd.ExecuteNonQuery(); // Executs first operasjon.
                        
                        cmd3.ExecuteNonQuery(); // Executes third operation.

                        con.Close(); // Closes connection.

                        SuccessAlert("Kommentaren din ble postet!");


                        context.Response.Redirect(context.Request.Url.ToString(), true);
                        

                    }
                    else // If comment has less than 4 characters.
                    {
                        FailAlert("Kommentaren må ha flere enn 3 karakterer!");
                    }
                }
                else // If user is not logged in
                {
                    FailAlert("Du er ikke logget inn!");
                }
            }
            catch(System.Data.SqlClient.SqlException)
            {
                FailAlert("En feil har oppstått, prøv igjen!");
            }

        }

        public void CustomizeProfile(TextBox txtImg, TextBox txtPassword, TextBox txtGender, TextBox txtEducation, TextBox txtDesc)
        {
            // The purpose of this method is to get all these values (img, pass, etc...) from the database and paste them into textboxes.

            // https://www.codeproject.com/Questions/823002/How-to-clear-Request-Querystring (source for the four lines of code below).
            string url = context.Request.Url.AbsoluteUri; // Gets URL.
            string[] separateURL = url.Split('?'); // Splits URL.
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(separateURL[0]); // Parameters.
            string qUsername = queryString[0];  // Index 0 is userName.

            string sessionUserName = context.Session["username"].ToString(); // Session username.

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspCustomizeProfile", con);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
            cmd.Parameters.AddWithValue("@username", sessionUserName); // sessionUsername is parameter.

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            int i = cmd.ExecuteNonQuery(); // Executes.


            // Assign values to parameters.
            string img = dt.Rows[0][8].ToString();
            string password = dt.Rows[0][2].ToString();
            string gender = dt.Rows[0][3].ToString();
            string education = dt.Rows[0][6].ToString();
            string description = dt.Rows[0][7].ToString();

            txtImg.Text = img;
            txtPassword.Text = "";
            txtGender.Text = gender;
            txtEducation.Text = education;
            txtDesc.Text = description;

            con.Close(); // Closes connection.
        }

        public void UpdateProfile(TextBox txtImg, TextBox txtPassword, TextBox txtGender, TextBox txtEducation, TextBox txtDesc)
        {
            // The purpose of this method is to put all these values (img, pass, etc...) into the database.

            // Inputs.
            string sessionUserName = context.Session["username"].ToString();

            string img = txtImg.Text;
            string password = ComputeSha256Hash(txtPassword.Text);
            string gender = txtGender.Text;
            string education = txtEducation.Text;
            string description = txtDesc.Text;

            int passwordCharacters = txtPassword.Text.Length;
            int educationCharacters = txtEducation.Text.Length;

            if (passwordCharacters >= 6) // If password characters above 6 or 6.
            {
                if ((gender == "Mann") || (gender == "Kvinne")) // If gender is man or woman
                {
                    if (educationCharacters >= 4) // If education characters is 4 or above.
                    {
                        con.Open(); // Opens connection.

                        SqlCommand cmd = new SqlCommand("uspUpdateProfile", con);
                        cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                        cmd.Parameters.AddWithValue("@sessionusername", sessionUserName); // Parameter sessionusername.
                        cmd.Parameters.AddWithValue("@image", img); // Parameter img.
                        cmd.Parameters.AddWithValue("@password", password); // Parameter password.
                        cmd.Parameters.AddWithValue("@gender", gender); // Parameter gender.
                        cmd.Parameters.AddWithValue("@education", education); // Parameter education.
                        cmd.Parameters.AddWithValue("@description", description); // Parameter description.

                        cmd.ExecuteNonQuery(); // Executes query.

                        con.Close(); // Closes connection.
                    } else // If education characters is not 4 or above.
                    {
                        FailAlert("Utdanningen din må ha flere enn 3 karakterer!");
                    }
                } else // If gender is not man or woman.
                {
                    FailAlert("Du må skrive at du er Mann eller Kvinne.");
                }

            } else // If password characters is less than 6.
            {
                FailAlert("Passordet ditt må ha flere enn 5 karakterer!");
            }

        }

        public void DeleteData()
        {
            // Input is current session.
            string userName = context.Session["username"].ToString();

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspDeleteData", con);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
            cmd.Parameters.AddWithValue("@username", userName); // Parameter is session userName.

            cmd.ExecuteNonQuery(); // Executes query.
            con.Close(); // Closes connection.

            context.Session.Abandon(); // Abandons session.
            context.Response.Redirect("/Default.aspx"); // Redirects to default page.

        }

        public void ViewMemberList(PlaceHolder phMemberList)
        {

            StringBuilder table = new StringBuilder(); // String object that can be modified.

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspViewMemberlist", con);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.

            SqlDataReader rd = cmd.ExecuteReader();

            

            if (rd.HasRows)
            {
                // Table.
                table.Append("<table align='center' width='100%' class='table table'");
                table.Append("<tr> " +
                    "<th><b>Brukernavn</b></th>" + // Username.
                    " <th><b>Rolle</b></th> " + // Role.
                    "<th><b>Kjønn</b></th>" + // Gender.
                    " <th><b>Utdannelse</b></th>"); // Education.
                table.Append("</tr>");

                while (rd.Read()) // Loops through data
                {
                    object userName = rd[0]; // userName.
                    object role = rd[1]; // role.
                    object gender = rd[2]; // gender.
                    object education = rd[3]; // education.

                    table.Append("<tr>");
                    table.Append("<td><a href='users.aspx/Username=" + userName + "'>" + userName + "</td>");
                    table.Append("<td>" + role + "</td>");
                    table.Append("<td>" + gender + "</td>");
                    table.Append("<td>" + education + "</td>");
                    table.Append("</tr>");
                }
            }
            else // No data.
            {
                FailAlert("Ingen registrerte brukere!"); // If there are no results.
            }

            table.Append("</table>"); // Ends table.
            phMemberList.Controls.Add(new Literal
            {
                Text = table.ToString()
            }
             );
            rd.Close(); // Closes read.
            con.Close(); // Closes connection.
        }

        public void SearchForum(PlaceHolder phTopics, TextBox txtSearch)
        {

            // Inputs
            string topicNameSearch = txtSearch.Text;
            int topicNameSearchCharacters = txtSearch.Text.Length;

            StringBuilder table = new StringBuilder(); // String object that can be modified.

            if (topicNameSearchCharacters >= 3)
            {

                con.Open(); // Opens connection.

                SqlCommand cmd = new SqlCommand("uspSearchForum", con);
                cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                cmd.Parameters.AddWithValue("@topicname", topicNameSearch); // Parameter is the text of what the user search.

                SqlDataReader rd = cmd.ExecuteReader(); // Executes reader.


                if (rd.HasRows) // If reader has rows.
                {
                    // Table
                    table.Append("<table align='center' width='100%' class='table table'");
                    table.Append("<tr> " +
                        "<th><b>Emnenavn</b></th> " + // Topicname (head of table).
                        "<th><b>Brukernavn</b></th>"); // Username (head of table).

                    table.Append("</tr>");
                    while (rd.Read()) // Loops through data
                    {
                        object topicId = rd[0]; // topicId
                        object topicName = rd[1]; // topicName
                        object userName = rd[2]; // userName

                        table.Append("<tr>");
                        table.Append("<td><a href='/comments/topicid=" + topicId + "'>" + topicName + "</td>");
                        table.Append("<td><a href='/users.aspx/Username=" + userName + "'>" + userName + "</td>");
                        table.Append("</tr>");
                    }
                }
                else // No data
                {
                    FailAlert("Ingen resultater!"); // If there are no results.
                }
                rd.Close(); // Closes read.
                con.Close(); // Closes connection.
            } else // If topicname got less than 3 characters.
            {
                FailAlert("Du må skrive inn flere enn 3 karakterer!");
            }

            table.Append("</table>"); // Ends table.

            phTopics.Controls.Add(new Literal
            {
                Text = table.ToString()
            }
             );
        }

        public void UserActivity(PlaceHolder phUserActivity)
        {

            StringBuilder userActivity = new StringBuilder(); // String object that can be modified.

            if (context.Session["username"] != null) // If user is logged in.
            {
                // Inputs.
                // https://www.codeproject.com/Questions/823002/How-to-clear-Request-Querystring (for four lines of code below).
                string url = HttpContext.Current.Request.Url.AbsoluteUri; // Gets URL
                string[] separateURL = url.Split('?'); // Splits URL
                NameValueCollection queryString = HttpUtility.ParseQueryString(separateURL[0]); // Parameterne
                string userName = queryString[0];  // Indekx 0 is userName.

                con.Open(); // Opens connection.

                SqlCommand cmd = new SqlCommand("uspUserActivity", con);
                cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                cmd.Parameters.AddWithValue("@username", userName); // username is parameter.

                SqlDataReader rd = cmd.ExecuteReader(); // Executes reader

                

                if (rd.HasRows) // If there are any rows.
                {
                    // Table
                    userActivity.Append("<table align='center' width='100%' class='table table'");
                    userActivity.Append("<tr> <th><b>Emne</b></th>" + // topicname (head of table).
                        " <th><b>Klokkeslett</b></th> "); // time (head of table).
                    userActivity.Append("</tr>");

                    while (rd.Read()) // Reads data.
                    {
                        object topicId = rd[0];
                        object topicName = rd[1];
                        object time = rd[2];

                        userActivity.Append("<tr>");
                        userActivity.Append("<td><a href='/comments" +
                            "/topicid=" + topicId + "'>" + topicName + "</td>");
                        userActivity.Append("<td>" + time + "</td>");
                        userActivity.Append("</tr>");
                    }
                }
                else // No results
                {
                    userActivity.Append("<i align='center'>" + userName + " har ikke vært aktiv i noen emner.</i>");
                }
                userActivity.Append("</table>"); // Ends table.
                phUserActivity.Controls.Add(new Literal
                {
                    Text = userActivity.ToString()
                }
                 );
                rd.Close(); // Closes reader.
                con.Close(); // Closes connection.

            }
            else // If not logged in.
            {
                context.Response.Redirect("/Default.aspx");
                
            }

            }

        public void CategoryDescription(PlaceHolder phCategoryDescription)
        {
            // Inputs.
            // https://www.codeproject.com/Questions/823002/How-to-clear-Request-Querystring (for four lines of code below).
            string url = HttpContext.Current.Request.Url.AbsoluteUri; // Gets URL.
            string[] separateURL = url.Split('?'); // Splits URL.
            NameValueCollection queryString = HttpUtility.ParseQueryString(separateURL[0]); // Parameters
            string categoryId_URL = queryString[0];  // Index 0 is categoryId.


            StringBuilder table = new StringBuilder(); // String object that can be modified.

            con.Open(); // Opens connection.

            SqlCommand cmd = new SqlCommand("uspPasteDataCategoryDesc", con);
            cmd.CommandType = CommandType.StoredProcedure; // Stored Procedure.
            cmd.Parameters.AddWithValue("@categoryid", categoryId_URL); // username is parameter.


            SqlDataReader rd = cmd.ExecuteReader(); // Executes reader.

            // Starts div.
            table.Append("<div class='row'>");
            if (rd.HasRows) // If reader has rows.
            {
                while (rd.Read()) // Loops through data.
                {
                    // Assigns values to variables.
                    object categoryId = rd[0];
                    object categoryName = rd[1];
                    object categoryImage = rd[2];
                    object categoryDesc = rd[3];

                    // Some sort of "table".
                    table.Append("<div class='col-sm-3 text-center' style='margin-top:40px'>" +
                        "<a href=subcategories.aspx/categoryid=" + categoryId + "><img src='" + categoryImage + "' " +
                        "class='rounded-circle' style='width: 128;height: 128px'/>" +
                        "<h1 style='font-size:20pt'><b>" + categoryName + "</h1></b></a><br>" +
                        "<p>" + categoryDesc + "</div>");
                }
            }
            table.Append("</div>"); // Ends div.
            phCategoryDescription.Controls.Add(new Literal
            {
                Text = table.ToString()
            }
             );
            rd.Close(); // Closes reader.
            con.Close(); // Closes connection.
        }

        }
    
}